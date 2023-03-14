using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace DexDefender
{
    class Cryptor
    {

        public static int[] StringKey(int i)
        {
            int[] ret = i != 0 ? i != 1 ? i != 2 ? i != 3 ? new int[0] : new int[] { ',', 'w', 65532, 180, 65480, 178, 'K', 131, 'r', 'A', '}', 139, 133, 193, '\f', 169, 'O', 172, 139, '8', 17, 'D', ')', 'z', ']', 198, 228, 'W', 179, 142, 65524, 161, 65515, '0', 134, 143, 31, 65526, 172, 203, 'Y', 65519, 203, 't', ' ', 65528, '8', 183, 213, 'U', 'o', 65500, 's', 27, 21, '1', '.', 'z', '*', 141, '+', '\\', 'm', 11, 149, '9', '<', 'r', 220, 144, 181, 194, 169, 'j', 65520 } : new int[] { 24627 } : new int[] { 12293, 12294 } : new int[] { 37469, 12893, 58265, 34626, 61595, 5235, 30980, 36330, 54953, 54553, 35458, 50593 };
            return ret;
        }


        public static String StringEncoder(String str, int i)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                for (int i2 = 0; i2 < str.Length; i2++)
                {
                    sb.Append((char)(str[i2] ^ StringKey(i)[i2 % StringKey(i).Length]));
                }
                return sb.ToString();
            }
            catch
            {
                return "";
            }
        }

        public static sbyte[] JavaToCSharp(byte[] javaBytes)
        {
            return javaBytes.Select(b => (sbyte)b).ToArray();
        }

        public static byte[] CSharpToJava(sbyte[] csharpBytes)
        {
            return csharpBytes.Select(sb => (byte)sb).ToArray();
        }

        public static void Encoder(string f4, FileStream inputStream, FileStream outputStream)
        {
            sbyte c = (sbyte)2;
            char[] charArray = StringEncoder(f4, 2).ToCharArray();
            int[] iArr = { charArray[0] | (charArray[1] << 16), charArray[2] | (charArray[3] << 16), charArray[4] | (charArray[5] << 16), charArray[6] | (charArray[7] << 16) };
            int[] iArr2 = { charArray[8] | (charArray[9] << 16), charArray[10] | (charArray[11] << 16) };
            int[] iArr3 = new int[27];
            int i = iArr[0];
            iArr3[0] = i;
            int[] iArr4 = new int[3];
            iArr4[0] = iArr[1];
            iArr4[1] = iArr[2];
            iArr4[2] = iArr[3];
            int i2 = 0;
            while (i2 < 26)
            {
                int i3 = i2 % 3;
                iArr4[i3] = ((BitwiseUtils.SafeUnsignedRightShift(iArr4[i3], 8) | (iArr4[i3] << 24)) + i) ^ i2;
                i = (BitwiseUtils.SafeUnsignedRightShift(i, 29) | (i << 3)) ^ iArr4[i3];
                i2++;
                iArr3[i2] = i;
            }
            byte[] bytes = new byte[8192];
            sbyte[] bArr = new sbyte[8192];

            int i4 = 0;
            while (true)
            {
                int i5 = inputStream.Read(bytes, 0, bArr.Length);
                bArr = JavaToCSharp(bytes);

                if (i5 <= 0)
                {
                    return;
                }

                int i6 = i4 + i5;
                int i7 = i4;
                int i8 = 0;
                while (i7 < i6)
                {
                    int i9 = i7 % 8;
                    int i10 = i9 / 4;
                    int i11 = i7 % 4;
                    if (i9 == 0)
                    {
                        int i12 = iArr2[0];
                        int i13 = iArr2[1];
                        int i14 = (((i13 << 24) | BitwiseUtils.SafeUnsignedRightShift(i13, 8)) + i12) ^ iArr3[0];
                        int i15 = (BitwiseUtils.SafeUnsignedRightShift(i12, 29) | (i12 << 3)) ^ i14;
                        int i16 = (((i14 << 24) | BitwiseUtils.SafeUnsignedRightShift(i14, 8)) + i15) ^ iArr3[1];
                        int i17 = (BitwiseUtils.SafeUnsignedRightShift(i15, 29) | (i15 << 3)) ^ i16;
                        int i18 = (((i16 << 24) | BitwiseUtils.SafeUnsignedRightShift(i16, 8)) + i17) ^ iArr3[c];
                        int i19 = (BitwiseUtils.SafeUnsignedRightShift(i17, 29) | (i17 << 3)) ^ i18;
                        int i20 = (((i18 << 24) | BitwiseUtils.SafeUnsignedRightShift(i18, 8)) + i19) ^ iArr3[3];
                        int i21 = (BitwiseUtils.SafeUnsignedRightShift(i19, 29) | (i19 << 3)) ^ i20;
                        int i22 = (((i20 << 24) | BitwiseUtils.SafeUnsignedRightShift(i20, 8)) + i21) ^ iArr3[4];
                        int i23 = (BitwiseUtils.SafeUnsignedRightShift(i21, 29) | (i21 << 3)) ^ i22;
                        int i24 = (((i22 << 24) | BitwiseUtils.SafeUnsignedRightShift(i22, 8)) + i23) ^ iArr3[5];
                        int i25 = (BitwiseUtils.SafeUnsignedRightShift(i23, 29) | (i23 << 3)) ^ i24;
                        int i26 = (((i24 << 24) | BitwiseUtils.SafeUnsignedRightShift(i24, 8)) + i25) ^ iArr3[6];
                        int i27 = (BitwiseUtils.SafeUnsignedRightShift(i25, 29) | (i25 << 3)) ^ i26;
                        int i28 = (((i26 << 24) | BitwiseUtils.SafeUnsignedRightShift(i26, 8)) + i27) ^ iArr3[7];
                        int i29 = (BitwiseUtils.SafeUnsignedRightShift(i27, 29) | (i27 << 3)) ^ i28;
                        int i30 = (((i28 << 24) | BitwiseUtils.SafeUnsignedRightShift(i28, 8)) + i29) ^ iArr3[8];
                        int i31 = (BitwiseUtils.SafeUnsignedRightShift(i29, 29) | (i29 << 3)) ^ i30;
                        int i32 = (((i30 << 24) | BitwiseUtils.SafeUnsignedRightShift(i30, 8)) + i31) ^ iArr3[9];
                        int i33 = (BitwiseUtils.SafeUnsignedRightShift(i31, 29) | (i31 << 3)) ^ i32;
                        int i34 = (((i32 << 24) | BitwiseUtils.SafeUnsignedRightShift(i32, 8)) + i33) ^ iArr3[10];
                        int i35 = (BitwiseUtils.SafeUnsignedRightShift(i33, 29) | (i33 << 3)) ^ i34;
                        int i36 = (((i34 << 24) | BitwiseUtils.SafeUnsignedRightShift(i34, 8)) + i35) ^ iArr3[11];
                        int i37 = (BitwiseUtils.SafeUnsignedRightShift(i35, 29) | (i35 << 3)) ^ i36;
                        int i38 = (((i36 << 24) | BitwiseUtils.SafeUnsignedRightShift(i36, 8)) + i37) ^ iArr3[12];
                        int i39 = (BitwiseUtils.SafeUnsignedRightShift(i37, 29) | (i37 << 3)) ^ i38;
                        int i40 = (((i38 << 24) | BitwiseUtils.SafeUnsignedRightShift(i38, 8)) + i39) ^ iArr3[13];
                        int i41 = (BitwiseUtils.SafeUnsignedRightShift(i39, 29) | (i39 << 3)) ^ i40;
                        int i42 = (((i40 << 24) | BitwiseUtils.SafeUnsignedRightShift(i40, 8)) + i41) ^ iArr3[14];
                        int i43 = (BitwiseUtils.SafeUnsignedRightShift(i41, 29) | (i41 << 3)) ^ i42;
                        int i44 = (((i42 << 24) | BitwiseUtils.SafeUnsignedRightShift(i42, 8)) + i43) ^ iArr3[15];
                        int i45 = (BitwiseUtils.SafeUnsignedRightShift(i43, 29) | (i43 << 3)) ^ i44;
                        int i46 = (((i44 << 24) | BitwiseUtils.SafeUnsignedRightShift(i44, 8)) + i45) ^ iArr3[16];
                        int i47 = (BitwiseUtils.SafeUnsignedRightShift(i45, 29) | (i45 << 3)) ^ i46;
                        int i48 = (((i46 << 24) | BitwiseUtils.SafeUnsignedRightShift(i46, 8)) + i47) ^ iArr3[17];
                        int i49 = (BitwiseUtils.SafeUnsignedRightShift(i47, 29) | (i47 << 3)) ^ i48;
                        int i50 = (((i48 << 24) | BitwiseUtils.SafeUnsignedRightShift(i48, 8)) + i49) ^ iArr3[18];
                        int i51 = (BitwiseUtils.SafeUnsignedRightShift(i49, 29) | (i49 << 3)) ^ i50;
                        int i52 = (((i50 << 24) | BitwiseUtils.SafeUnsignedRightShift(i50, 8)) + i51) ^ iArr3[19];
                        int i53 = (BitwiseUtils.SafeUnsignedRightShift(i51, 29) | (i51 << 3)) ^ i52;
                        int i54 = (((i52 << 24) | BitwiseUtils.SafeUnsignedRightShift(i52, 8)) + i53) ^ iArr3[20];
                        int i55 = (BitwiseUtils.SafeUnsignedRightShift(i53, 29) | (i53 << 3)) ^ i54;
                        int i56 = (((i54 << 24) | BitwiseUtils.SafeUnsignedRightShift(i54, 8)) + i55) ^ iArr3[21];
                        int i57 = (BitwiseUtils.SafeUnsignedRightShift(i55, 29) | (i55 << 3)) ^ i56;
                        int i58 = (((i56 << 24) | BitwiseUtils.SafeUnsignedRightShift(i56, 8)) + i57) ^ iArr3[22];
                        int i59 = (BitwiseUtils.SafeUnsignedRightShift(i57, 29) | (i57 << 3)) ^ i58;
                        int i60 = (((i58 << 24) | BitwiseUtils.SafeUnsignedRightShift(i58, 8)) + i59) ^ iArr3[23];
                        int i61 = (BitwiseUtils.SafeUnsignedRightShift(i59, 29) | (i59 << 3)) ^ i60;
                        int i62 = (((i60 << 24) | BitwiseUtils.SafeUnsignedRightShift(i60, 8)) + i61) ^ iArr3[24];
                        int i63 = (BitwiseUtils.SafeUnsignedRightShift(i61, 29) | (i61 << 3)) ^ i62;
                        int i64 = (((i62 << 24) | BitwiseUtils.SafeUnsignedRightShift(i62, 8)) + i63) ^ iArr3[25];
                        int i65 = (BitwiseUtils.SafeUnsignedRightShift(i63, 29) | (i63 << 3)) ^ i64;
                        int i66 = iArr3[26] ^ (((i64 << 24) | BitwiseUtils.SafeUnsignedRightShift(i64, 8)) + i65);
                        iArr2[0] = (BitwiseUtils.SafeUnsignedRightShift(i65, 29) | (i65 << 3)) ^ i66;
                        iArr2[1] = i66;
                    }
                    bArr[i8] = (sbyte)(((sbyte)(iArr2[i10] >> (i11 * 8))) ^ bArr[i8]);
                    i7++;
                    i8++;
                    c = (sbyte)2;
                }
                byte[] buf = CSharpToJava(bArr);
                outputStream.Write(buf, 0, i5);
                i4 = i7;
                c = (sbyte)2;
            }


        }

    }
}
