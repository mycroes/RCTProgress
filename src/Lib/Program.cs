using System.Buffers;
using System.Buffers.Binary;
using System.Text;
using RCTProgress.Lib;

var pool = MemoryPool<byte>.Shared;
using var file = File.OpenRead(args[0]);

var input = pool.Rent(20000);
var output = pool.Rent(20000);

var mem = output.Memory;

var len = await file.ReadAsync(input.Memory);

//ScenarioData.Decrypt(input.Memory);
RunLengthEncoding.Decode(input.Memory[..len][..^4], ref mem);
Sv4Data.Decrypt(mem);

var numScenarios = mem.Span[0x3a00];
List<Scenario> scenarios = new();
for (var i = 0; i < numScenarios; i++)
{
    var filename = Encoding.ASCII.GetString(mem.Span.Slice(i * 16, 16));
    var name = Encoding.ASCII.GetString(mem.Span.Slice(0x0800 + i * 64, 64));
    var value = BinaryPrimitives.ReadInt32LittleEndian(mem.Span.Slice(0x2800 + i * 4, 4));
    var winner = Encoding.ASCII.GetString(mem.Span.Slice(0x2a00 + i * 32, 32));
    var avail = ((mem.Span[0x3a0c + i / 8] >> (i % 8)) & 1) == 1;

    scenarios.Add(new Scenario
    {
        Filename = filename,
        CompanyValue = value,
        Name = name,
        Winner = winner,
        Available = avail
    });
}

foreach (var sc in scenarios)
{
    Console.WriteLine($"{sc.Filename}\t{sc.Name}\t{sc.CompanyValue}\t{sc.Winner}\t{sc.Available}");
}

var checksum = Checksums.Calculate(input.Memory[..len][..^4].Span);
checksum += 120001;

var cb = new byte[4];
BinaryPrimitives.WriteUInt32LittleEndian(cb, checksum);

for (int i = 0; i < 4; i++) {
    Console.Write($"{cb[i]:X2} ");
}

Console.WriteLine("Calculated");

for (int i = 0; i < 4; i++) {
    Console.Write($"{input.Memory.Span[len - 4 + i]:X2} ");
}

Console.WriteLine("From file");

Console.WriteLine($"Len: {len:X4}");

if (args.Length < 2) return;

mem.Span[0x3a0c] = 0;
mem.Span[0x3a0c + 1] = 0;

var wdMemOwner = pool.Rent(mem.Length);
Sv4Data.Encrypt(mem);

var wdMem = wdMemOwner.Memory;
var wdLen = RunLengthEncoding.Encode(mem, wdMem);
var wdSum = Checksums.Calculate(wdMem.Span[..wdLen]);
wdSum += 120001;

BinaryPrimitives.WriteUInt32LittleEndian(wdMem.Span[wdLen..], wdSum);

using var outFile = File.OpenWrite(args[1]);

await outFile.WriteAsync(wdMem[..(wdLen +  4)]);

for (int i = wdLen - 10; i < wdLen + 4; i++) {
    Console.Write($"{wdMem.Span[i]:X2} ");
}

Console.WriteLine();