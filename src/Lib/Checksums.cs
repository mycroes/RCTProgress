namespace RCTProgress.Lib;

public static partial class Checksums
{
    public const uint Sv4Secret = 120001;

    public static uint Calculate(Span<byte> data)
    {
        uint result = 0;
        foreach (var b in data)
        {
            result = (0xff & (result + b)) | (result & 0xffffff00);
            result = BitMath.RotateLeft(result, 3);
        }

        return result;
    }
}