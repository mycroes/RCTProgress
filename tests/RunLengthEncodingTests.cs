using Xunit;
using Shouldly;

namespace RCTProgress.Tests;

public class RunLengthEncodingTests
{
    [Fact]
    public void Should_Encode_Single_Byte()
    {
        Encode(1).ShouldBe(new byte[] {0, 1});
    }

    [Fact]
    public void Should_Encode_Uncompressed()
    {
        Encode(1, 2).ShouldBe(new byte[] {1, 1, 2});
    }

    [Fact]
    public void Should_Encode_Compressed()
    {
        Encode(1, 1).ShouldBe(new byte[] {255, 1});
    }

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(100)]
    [InlineData(125)]
    public void Should_Encode_Compressed_Long(int repeatCount)
    {
        Encode(Enumerable.Repeat((byte)99, repeatCount).ToArray()).ShouldBe(new byte[] { (byte)(257 - repeatCount), 99 });
    }

    [Fact]
    public void Should_Encode_Mixed_Uncompressed_Head() {
        Encode(1, 2, 1, 1).ShouldBe(new byte[] {1, 1, 2, 255, 1 });
    }

    [Fact]
    public void Should_Encode_Mixed_Compressed_Head() {
        Encode(99, 99, 66, 99).ShouldBe(new byte[] {255, 99, 1, 66, 99});
    }

    
    [Fact]
    public void Should_Encode_Mixed() {
        Encode(99, 99, 66, 99, 77, 77, 66, 99).ShouldBe(new byte[] {255, 99, 1, 66, 99, 255, 77, 1, 66, 99});
    }

    private static byte[] Encode(params byte[] input)
    {
        var mem = new byte[input.Length * 2].AsMemory();

        var len = RunLengthEncoding.Encode(input, mem);

        return mem.Span[..len].ToArray();
    }
}