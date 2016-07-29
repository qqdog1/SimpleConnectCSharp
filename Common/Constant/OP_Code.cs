using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Constant
{
    class OP_Code
    {
        public byte CONFIRM = Convert.ToByte('C');
        public byte DATA = Convert.ToByte('D');
        public byte HEARTBEAT = Convert.ToByte('H');
        public byte REJECT = Convert.ToByte('R');
    }
}
