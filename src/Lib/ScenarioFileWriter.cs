using System.Buffers;
using System.Buffers.Binary;
using System.Collections;
using System.Text;

namespace RCTProgress.Lib;

public class ScenarioFileWriter
{
    private const int Length = 0x3a1b;
    private const int FileNameSize = 16;
    private const int NameSize = 64;
    private const int CompanyValueSize = sizeof(int);
    private const int WinnerSize = 32;
    private const int NumScenarios = 128;
    private const int ScenarioCountOffset = 0x3a00;
    private const int FlagOffset = 0x3a01;
    private const int Css1TimeRefOffset = 0x3a02;
    private const int ScenarioFileSizeOffset = 0x3a04;
    private const int ScenarioBlockedOffset = 0x3a0c;
    private const int ChecksumSecret = 120001;

    private readonly MemoryPool<byte> _memoryPool = MemoryPool<byte>.Shared;

    public async ValueTask WriteAsync(string fileName, ScenarioFile file)
    {
        using var fs = File.OpenWrite(fileName);
        using var data = _memoryPool.Rent(Length);

        WriteData(data.Memory.Span, file);

        Sv4Data.Encrypt(data.Memory[..Length]);
        
        using var encoded = _memoryPool.Rent(Length);

        var outputLen = RunLengthEncoding.Encode(data.Memory[..Length], encoded.Memory);
        var checksum = Checksums.Calculate(encoded.Memory[..outputLen].Span);
        checksum += ChecksumSecret;

        BinaryPrimitives.WriteUInt32LittleEndian(encoded.Memory.Span[outputLen..], checksum);

        await fs.WriteAsync(encoded.Memory.Slice(0, outputLen + sizeof(uint)));
    }

    private void WriteData(Span<byte> buffer, ScenarioFile file)
    {
        for (var i = 0; i < ScenarioCountOffset; i++)
        {
            buffer[i] = 0;
        }

        var fileNames = buffer;
        var names = fileNames.Slice(FileNameSize * NumScenarios);
        var companyValues = names.Slice(NameSize * NumScenarios);
        var winners = companyValues.Slice(CompanyValueSize * NumScenarios);
        var ba = new BitArray(NumScenarios);

        for (var i = 0; i < file.Scenarios.Count; i++)
        {
            var sc = file.Scenarios[i];
            Encoding.ASCII.GetBytes(sc.FileName, fileNames.Slice(i * FileNameSize, FileNameSize));
            Encoding.ASCII.GetBytes(sc.Name, names.Slice(i * NameSize, NameSize));
            BinaryPrimitives.WriteInt32LittleEndian(companyValues.Slice(i * CompanyValueSize, CompanyValueSize), sc.CompanyValue);
            Encoding.ASCII.GetBytes(sc.Winner, winners.Slice(i * WinnerSize, WinnerSize));
            ba[i] = !sc.Available;
        }

        file.MegaParkHash.AsSpan().CopyTo(winners.Slice(21 * WinnerSize, WinnerSize));

        buffer[ScenarioCountOffset] = file.NumScenarios;
        buffer[FlagOffset] = 0;
        BinaryPrimitives.WriteUInt32LittleEndian(buffer.Slice(Css1TimeRefOffset), file.Css1TimeRef);
        BinaryPrimitives.WriteInt64LittleEndian(buffer.Slice(ScenarioFileSizeOffset), file.ScenarioFileSize);

        var flagCopy = new byte[NumScenarios / 8];
        ba.CopyTo(flagCopy, 0);
        flagCopy.AsSpan().CopyTo(buffer.Slice(ScenarioBlockedOffset));
    }
}