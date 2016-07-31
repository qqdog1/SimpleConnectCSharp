using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Constant
{
    class PackerConstant
    {
        public const int BOU_EOU_LENGTH = 2;

        public const byte BOU = (byte)0xFE;
        public const byte EOU = (byte)0xEF;

        public const int LENGTH_LENGTH = 2;
        public const int OP_LENGTH = 1;

        public const int POSITIVE_BYTE_SIZE = 128;
    }
}
