namespace RCTProgress.Lib;

internal static class RunLengthEncoding
{
    private const byte Msb = 0b1000_0000;

    public static void Decode(ReadOnlyMemory<byte> input, ref Memory<byte> output)
    {
        var len = 0;

        while (input.Length > 0)
        {
            var b = input.Span[0];
            if (b < Msb)
            {
                input.Slice(1, b + 1).CopyTo(output[len..]);

                len += b + 1;
                input = input[(b + 2)..];
            }
            else
            {
                var count = unchecked(-(sbyte)b + 1);
                var data = input.Span[1];

                var target = output.Span;
                for (var i = 0; i < count; i++)
                {
                    target[len + i] = data;
                }

                len += count;
                input = input[2..];
            }
        }

        output = output[..len];
    }

    public static int Encode(ReadOnlyMemory<byte> data, Memory<byte> encoded)
    {
        var input = data.Span;
        var output = encoded.Span;
        var writeIndex = 0;

        while (input.Length > 0)
        {
            if (input.Length == 1) {
                output[writeIndex++] = 0;
                output[writeIndex++] = input[0];

                break;
            }

            var fb = input[0];
            var nb = input[1];

            if (fb != nb)
            {
                var i = 1;
                do {
                    i++;
                }
                while (i < input.Length && input[i] != input[i - 1] && i <= 125);

                if (i < input.Length && input[i] == input[i - 1]) i--;

                output[writeIndex++] = (byte) (i - 1);
                input[..i].CopyTo(output[writeIndex..]);

                input = input[i..];
                writeIndex += i;
            }
            else
            {
                var i = 1;
                do {
                    i++;
                }
                while (i < input.Length && input[i] == fb && i <= 125);

                output[writeIndex++] = (byte)(sbyte) -(i - 1);
                output[writeIndex++] = fb;

                input = input[i..];
            }
        }

        return writeIndex;
    }
}