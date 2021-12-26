namespace RCTProgress;

public static class BitMath
{
    public static int RotateLeft(int input, int count)
    {
        return input << count | input >> (sizeof(int) * 8 - count);
    }

    public static uint RotateLeft(uint input, int count)
    {
        return input << count | input >> (sizeof(uint) * 8 - count);
    }

    public static int RotateRight(int input, int count)
    {
        return input >> count | input << (sizeof(int) * 8 - count);
    }

    public static uint RotateRight(uint input, int count)
    {
        return input >> count | input << (sizeof(uint) * 8 - count);
    }
}