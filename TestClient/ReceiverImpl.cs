using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Receiver;

namespace TestClient
{
    class ReceiverImpl : ISimpleConnectReceiver
    {
        public void onMessage(string sSessionId, byte[] bData)
        {
            Console.WriteLine(Encoding.UTF8.GetString(bData));
        }
    }
}
