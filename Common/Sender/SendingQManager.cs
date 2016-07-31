using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using Common.Packer;

namespace Common.Sender
{
    class SendingQManager
    {
        private String sSenderName;

        private BlockingCollection<byte[]> queue;
        private int iQueueSize;
        
        private int iHeartbeatInterval;

        private Socket socket;

        private bool bRunFlag;
        private Thread thread;

        public SendingQManager(int iQueueSize, int iHeartbeatInterval, Socket socket, String sSenderName)
        {
            this.iQueueSize = iQueueSize;
            this.iHeartbeatInterval = iHeartbeatInterval;
            this.socket = socket;
            this.sSenderName = sSenderName;

            startSendingQueue();
        }

        private void startSendingQueue()
        {
            queue = new BlockingCollection<byte[]>(iQueueSize);
            bRunFlag = true;
            ThreadStart threadStart = new ThreadStart(run);
            Thread thread = new Thread(threadStart);
            thread.Start();
        }

        public bool putQueue(byte[] bData)
        {
            return queue.TryAdd(SimpleConnectDataPacker.packingData(bData));
        }

        public bool putQueue(byte[] bData, int lTimeout)
        {
            return queue.TryAdd(SimpleConnectDataPacker.packingData(bData), lTimeout);
        }

        public void run()
        {
            while (bRunFlag)
            {
                byte[] bData;
                if (queue.TryTake(out bData, iHeartbeatInterval))
                {
                    send(bData);
                }
                else
                {
                    sendHeartbeat();
                }
            }
        }

        private void sendHeartbeat()
        {
            send(SimpleConnectDataPacker.packingHeartbeat());
        }

        private void send(byte[] bData)
        {
            socket.Send(bData);
        }

        public void closeSendingQThread()
        {
            bRunFlag = false;
        }
    }
}
