using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public sealed class ClientConfigLoader
    {
        private static readonly ClientConfigLoader instance = new ClientConfigLoader();
        private ClientConfigLoader() { }

        private const string SERVER_IP = "ServerIp";
	    private const string SERVER_PORT = "ServerPort";
	    private const string HEARTBEAT_INTERVAL = "Heartbeat-Interval";
	    private const string HEARTBEAT_COUNT = "Heartbeat-Count";
	    private const string SENDING_QUEUE_SIZE = "SendingQueue-Size";
	    private const string RECEIVING_QUEUE_SIZE = "ReceivingQueue-Size";

        private String sConfigPath;

        private Properties properties;

        private String sServerIp;
        private int iServerPort;
        private int iHeartbeatInterval;
        private int iHeartbeatCount;

        private int iSendingQueueSize;
        private int iReceivingQueueSize;

        public static ClientConfigLoader Instance
        {
            get
            {
                return instance;
            }
        }

    }
}
