using System.Buffers;
using System.Buffers.Binary;
using System.Text;

namespace RCTProgress.Lib;

public class ScenarioFileReader
{
    private const int ScenarioCountOffset = 0x3a00;
    private const int FlagOffset = 0x3a01;
    private const int Css1TimeRefOffset = 0x3a02;
    private const int ScenarioFileSizeOffset = 0x3a04;
    private readonly MemoryPool<byte> _memoryPool = MemoryPool<byte>.Shared;

    public async ValueTask<ScenarioFile> ReadAsync(string fileName)
    {
        using var file = File.OpenRead(fileName);
        using var fileData = _memoryPool.Rent(20000);
        var len = await file.ReadAsync(fileData.Memory);
        var checksum = BinaryPrimitives.ReadUInt32LittleEndian(fileData.Memory.Span.Slice(len - 4));

        using var target = _memoryPool.Rent(20000);
        var decoded = target.Memory;
        RunLengthEncoding.Decode(fileData.Memory[..len][..^4], ref decoded);
        Sv4Data.Decrypt(decoded);

        var numScenarios = decoded.Span[ScenarioCountOffset];
        var scenarios = new List<Scenario>(128);
        for (var i = 0; i < 128; i++)
        {
            var filename = Encoding.ASCII.GetString(decoded.Span.Slice(i * 16, 16));
            var name = Encoding.ASCII.GetString(decoded.Span.Slice(0x0800 + i * 64, 64));
            var value = BinaryPrimitives.ReadInt32LittleEndian(decoded.Span.Slice(0x2800 + i * 4, 4));
            var winner = Encoding.ASCII.GetString(decoded.Span.Slice(0x2a00 + i * 32, 32));
            var avail = ((decoded.Span[0x3a0c + i / 8] >> (i % 8)) & 1) == 0;

            scenarios.Add(new Scenario
            {
                FileName = filename,
                CompanyValue = value,
                Name = name,
                Winner = winner,
                Available = avail
            });
        }

        var megaParkHash = decoded.Span.Slice(0x2a0 + 21 * 32, 32).ToArray();

        var flag = decoded.Span[FlagOffset];
        var css1TimeRef = BinaryPrimitives.ReadUInt16LittleEndian(decoded[Css1TimeRefOffset..].Span);
        var scenarioFileSize = BinaryPrimitives.ReadInt64LittleEndian(decoded[ScenarioFileSizeOffset..].Span);
        
        return new ScenarioFile(scenarios, numScenarios, megaParkHash, flag, css1TimeRef, checksum, scenarioFileSize);
    }
}
