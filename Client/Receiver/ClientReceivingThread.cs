using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Common.Receiver;
using Common.Packer.Vo;
using Common.Constant;

namespace Client.Receiver
{
    class ClientReceivingThread
    {
        private ClientThread clientThread;
        private ReceivingQManager receivingQManager;
        private bool bRunFlag;

        public ClientReceivingThread(ClientThread clientThread, ReceivingQManager receivingQManager)
        {
            this.clientThread = clientThread;
            this.receivingQManager = receivingQManager;
            bRunFlag = true;

            ThreadStart threadStart = new ThreadStart(run);
            Thread thread = new Thread(threadStart);
            thread.Start();
        }

        private void run()
        {
            while (bRunFlag)
            {
                PackVo vo = receivingQManager.poll();
                if (vo == null)
                {
                    if (bRunFlag)
                    {
                        clientThread.heartbeatTimeout();
                    }
                    continue;
                }

                byte op_code = vo.OP_Code;

                if(op_code == OP_Code.CONFIRM)
                {
                    clientThread.receiveConfirm();
                } else if(op_code == OP_Code.DATA)
                {
                    clientThread.receiveData(vo.Data);
                } else if(op_code == OP_Code.REJECT)
                {
                    clientThread.receiveReject(vo.Data);
                }
            }
        }

        public void closeReceivingThread()
        {
            bRunFlag = false;
        }
    }
}
