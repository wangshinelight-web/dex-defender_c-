using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DexDefender
{
    public static class BitwiseUtils
    {
        // 基础实现（直接转换）
        public static int UnsignedRightShift(int value, int shiftBits)
        {
            return (int)((uint)value >> shiftBits);
        }

        public static int Clamp(int value, int min, int max)
        {
            return value < min ? min : (value > max ? max : value);
        }
        // 带边界检查的安全版本
        public static int SafeUnsignedRightShift(int value, int shiftBits)
        {
            //shiftBits = Clamp(shiftBits, 0, 31);
            return (int)((uint)value >> shiftBits);
        }

        // 支持long类型的扩展
        public static long UnsignedRightShift(long value, int shiftBits)
        {
            return (long)((ulong)value >> shiftBits);
        }

 

    }
}
