using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Packer.Vo;

namespace Common.Receiver
{
    class ReceivingQManager
    {
        private BlockingCollection<PackVo> queue;
        private int iTimeOut;

        public ReceivingQManager(int iQueueSize, int iHeartbeatInterval, int iHeartbeatCount)
        {
            queue = new BlockingCollection<PackVo>(iQueueSize);
            iTimeOut = iHeartbeatInterval * iHeartbeatCount;
        }

        public void add(PackVo vo)
        {
            queue.Add(vo);
        }

        public PackVo poll()
        {
            PackVo vo;
            if (queue.TryTake(out vo, iTimeOut))
            {
                return vo;
            }
            return null;
        }
    }
}
