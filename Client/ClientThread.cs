using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using Common.Packer;
using Common.Packer.Vo;
using Common.Constant;
using Common.Receiver;
using Common.Sender;
using Client.Receiver;

namespace Client
{
    class ClientThread
    {
        private ClientConfigLoader configLoader;
        private Socket socket;

        private bool bInitStatus = true;
        private bool bConnectStatus = false;

        private bool bRunFlag = false;
        private string sClientName;

        private ISimpleConnectReceiver receiver;
        private SendingQManager sendingQManager;
        private ReceivingQManager receivingQManager;
        private ClientReceivingThread clientReceivingThread;

        public ClientThread(String sConfigPath, ISimpleConnectReceiver receiver, String sClientName)
        {
            this.sClientName = sClientName;
            this.receiver = receiver;
            initConfig(sConfigPath);
        }

        public void connectToServer()
        {
            if (!bInitStatus)
            {
                return;
            }

            if (connectServer())
            {
                initReceiver();
                ThreadStart threadStart = new ThreadStart(run);
                Thread thread = new Thread(threadStart);
                thread.Start();
            }
        }

        private void initConfig(String sConfigPath)
        {
            configLoader = ClientConfigLoader.Instance;
            configLoader.init(sConfigPath);
        }

        private void initReceiver()
        {
            if (receiver != null)
            {
                receivingQManager = new ReceivingQManager(configLoader.getReceivingQueueSize(), configLoader.getHeartbeatInterval(), configLoader.getHeartbeatCount());
                clientReceivingThread = new ClientReceivingThread(this, receivingQManager);
            }
            else
            {
                bInitStatus = false;
            }
        }

        private bool connectServer()
        {
            IPHostEntry entry = Dns.GetHostEntry(configLoader.getServerIp());
            IPEndPoint remoteEP = new IPEndPoint(entry.AddressList[0], configLoader.getServerPort());
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Connect(remoteEP);
              
            bRunFlag = true;
            return true;
        }

        private void run()
        {
            while (bRunFlag)
            {
                try
                {
                    PackVo vo = SimpleConnectDataPacker.unpackingData(socket);
                    receivingQManager.add(vo);
                }
                catch (Exception e)
                {
                    // TODO notify ap
                    bRunFlag = false;
                    disconnect();
                }
            }
        }

        public void receiveConfirm()
        {
            if (!bConnectStatus)
            {
                initSendingQManager();
                bConnectStatus = true;
            }
            else
            {
                // TODO error
            }
        }

        private void initSendingQManager()
        {
            sendingQManager = new SendingQManager(configLoader.getSendingQueueSize(), configLoader.getHeartbeatInterval(), socket, sClientName);
        }

        public bool send(byte[] bData)
        {
            return sendingQManager.putQueue(bData);
        }

        public void receiveData(byte[] bData)
        {
            receiver.onMessage(sClientName, bData);
        }

        public void receiveReject(byte[] bData)
        {
            this.bRunFlag = false;

            if(bData == REJ_Code.SAME_IP_FULL)
            {

            }

            disconnect();

            // TODO notify ap
        }

        public void heartbeatTimeout()
        {
            disconnect();

            // TODO notify ap
        }

        public void disconnect()
        {
            if (sendingQManager != null)
            {
                sendingQManager.closeSendingQThread();
            }
            clientReceivingThread.closeReceivingThread();
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();

            bRunFlag = false;
            bConnectStatus = false;
        }

        public bool isConnect()
        {
            return bConnectStatus;
        }
    }
}
