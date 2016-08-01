using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Client;

namespace TestClient
{
    class Program
    {
        private Client.Client client;
        static void Main(string[] args)
        {
            new Program();
        }

        private Program()
        {
            Console.WriteLine("++++++++++++++++++++++");

            client = new Client.Client("../../config/ClientConfig.txt", new ReceiverImpl(), "QQ");

            client.connectToServer();

            Console.ReadLine();

            client.send(Encoding.UTF8.GetBytes("GG"));

            Console.WriteLine("++++++++++++++++++++++");
            
            Console.ReadLine();

            client.disconnect();
        }
    }
}
