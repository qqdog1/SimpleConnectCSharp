using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Constant
{
    class OP_Code
    {
        public static byte CONFIRM = Convert.ToByte('C');
        public static byte DATA = Convert.ToByte('D');
        public static byte HEARTBEAT = Convert.ToByte('H');
        public static byte REJECT = Convert.ToByte('R');
    }
}
