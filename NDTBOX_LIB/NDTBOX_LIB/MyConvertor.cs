using System;

namespace NDTBOX_LIB
{
    public class MyConvertor
    {
        public static unsafe byte[] getbytes(UInt32 address, int iReadLength)
        {
            byte* p = (byte*)address;
            byte[] ret = new byte[iReadLength];

            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = *(p + i);
            }
            return ret;
        }

        unsafe public static UInt32 byteArrToUint(byte[] b)
        {
            fixed (byte* p0 = b)
            {
                byte* p = p0;
                return (UInt32)p;
            }
        }

        public static int byteArrToInt(int v)
        {
            if (v >= UInt16.MaxValue / 2)
            {
                v = v - UInt16.MaxValue;
            }
            return v;
        }
    }
}