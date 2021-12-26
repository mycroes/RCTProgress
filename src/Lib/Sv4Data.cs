using System.Runtime.InteropServices;

namespace RCTProgress.Lib;

internal class Sv4Data
{
    public const uint Secret = 0x39393939;

    public static void Decrypt(Memory<byte> memory)
    {
        var span = MemoryMarshal.Cast<byte, uint>(memory.Span);
        for (var i = 0; i < span.Length; i++)
        {
            span[i] = BitMath.RotateLeft(span[i], 5);
            span[i] -= Secret;
        }
    }

    public static void Encrypt(Memory<byte> memory)
    {
        var span = MemoryMarshal.Cast<byte, uint>(memory.Span);
        for (var i = 0; i < span.Length; i++)
        {
            span[i] += Secret;
            span[i] = BitMath.RotateRight(span[i], 5);
        }
    }
}