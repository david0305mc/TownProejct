using System;
using System.Security;
using Unity.Collections.LowLevel.Unsafe;

public static class ExtArray
{

    const int LenByte = sizeof(byte);
    const int LenShort = sizeof(short);
    const int LenInt = sizeof(int);
    const int LenLong = sizeof(long);



    #region Array clear
    public static void SetFill<T>(this T[] src, T value) { for (int index = 0; index < src.Length; ++index) src[index] = value; }
    public static void Clear<T>(this T[] src) { for (int index = 0; index < src.Length; ++index) src[index] = default; }
    public static void Clear<T>(this T[][] src) { for (int index = 0; index < src.Length; ++index) { var a1 = src[index]; if (a1 != null) { Clear(a1); src[index] = null; } } }
    public static void Clear<T>(this T[][][] src) { for (int index = 0; index < src.Length; ++index) { var a2 = src[index]; if (a2 != null) { Clear(a2); src[index] = null; } } }

    public static void Clear<T>(this T[,] src)
    {
        var length00 = src.GetLength(0);
        var length01 = src.GetLength(1);

        for (int index00 = 0; index00 < length00; ++index00)
        {
            for (int index01 = 0; index01 < length01; ++index01)
                src[index00, index01] = default;
        }
    }

    public static void Clear<T>(this T[,,] src)
    {
        var length00 = src.GetLength(0);
        var length01 = src.GetLength(1);
        var length02 = src.GetLength(2);

        for (int index00 = 0; index00 < length00; ++index00)
        {
            for (int index01 = 0; index01 < length01; ++index01)
            {
                for (int index02 = 0; index02 < length02; ++index02)
                    src[index00, index01, index02] = default;
            }
        }
    }
    #endregion// Array clear

    #region Array copy

    public static int BlockCopy(this Array src, Array dst, int dstOffset) => BlockCopy(src, 0, dst, dstOffset, src.Length);
    public static int BlockCopy(this Array src, Array dst, int dstOffset, int count) => BlockCopy(src, 0, dst, dstOffset, count);
    public static int BlockCopy(this Array src, int srcOffset, Array dst, int dstOffset) => BlockCopy(src, srcOffset, dst, dstOffset, src.Length);
    public static int BlockCopy(this Array src, int srcOffset, Array dst, int dstOffset, int count)
    {
        Buffer.BlockCopy(src, srcOffset, dst, dstOffset, count);
        return count;
    }
    #endregion// Array copy

}