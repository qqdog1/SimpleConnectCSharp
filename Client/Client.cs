using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Receiver;

namespace Client
{
    public class Client
    {
        private ClientThread clientThread;

        public Client(String sConfigPath, ISimpleConnectReceiver receiver, String sClientName)
        {
            clientThread = new ClientThread(sConfigPath, receiver, sClientName);
        }

        public void connectToServer()
        {
            clientThread.connectToServer();
        }

        public bool send(byte[] bData)
        {
            return clientThread.send(bData);
        }

        public bool isConnect()
        {
            return clientThread.isConnect();
        }

        public void disconnect()
        {
            clientThread.disconnect();
        }
    }
}
