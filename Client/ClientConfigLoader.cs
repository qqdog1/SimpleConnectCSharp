using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.PropertiesReader;

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

        public void init(String sConfigPath)
        {
            this.sConfigPath = sConfigPath;

            loadProperties();

            readServerIpfromConfig();

            readServerPortfromConfig();

            readHeartbeatIntervalfromConfig();

            readHeartbeatCountfromConfig();

            readSendingQueueSizefromConfig();

            readReceivingQueueSizefromConfig();
        }

        private void loadProperties()
        {
            properties = new Properties(sConfigPath);
        }

        private void readServerIpfromConfig()
        {
            sServerIp = properties.getProperty(SERVER_IP);
        }

        private void readServerPortfromConfig()
        {
            string sServerPort = properties.getProperty(SERVER_PORT);
		    if(sServerPort != null)
            {
                int port;
                bool success = Int32.TryParse(sServerPort, out port);
                if(success)
                {
                    iServerPort = port;
                }
            }
        }

        private void readHeartbeatIntervalfromConfig()
        {
            string sHeartbeatInterval = properties.getProperty(HEARTBEAT_INTERVAL);
		    if(sHeartbeatInterval != null) {
                int heartbeat;
                bool success = Int32.TryParse(sHeartbeatInterval, out heartbeat);
                if (success)
                {
                    iHeartbeatInterval = heartbeat;
                }
            }
        }

        private void readHeartbeatCountfromConfig()
        {
            string sHeartbeatCount = properties.getProperty(HEARTBEAT_COUNT);
            if (sHeartbeatCount != null)
            {
                int heartbeatCount;
                bool success = Int32.TryParse(sHeartbeatCount, out heartbeatCount);
                if (success)
                {
                    iHeartbeatCount = heartbeatCount;
                }
            }
        }

        private void readSendingQueueSizefromConfig()
        {
            string sSendingQueueSize = properties.getProperty(SENDING_QUEUE_SIZE);
		    if(sSendingQueueSize != null) {
                int queueSize;
                bool success = Int32.TryParse(sSendingQueueSize, out queueSize);
                if (success)
                {
                    iSendingQueueSize = queueSize;
                }
            }
        }

        private void readReceivingQueueSizefromConfig()
        {
            string sReceivingQueueSize = properties.getProperty(RECEIVING_QUEUE_SIZE);
		    if(sReceivingQueueSize != null) {
                int queueSize;
                bool success = Int32.TryParse(sReceivingQueueSize, out queueSize);
                if (success)
                {
                    iReceivingQueueSize = queueSize;
                }
            }
        }

        public string getServerIp()
        {
            return sServerIp;
        }

        public int getServerPort()
        {
            return iServerPort;
        }

        public int getHeartbeatInterval()
        {
            return iHeartbeatInterval;
        }

        public int getHeartbeatCount()
        {
            return iHeartbeatCount;
        }

        public int getSendingQueueSize()
        {
            return iSendingQueueSize;
        }

        public int getReceivingQueueSize()
        {
            return iReceivingQueueSize;
        }
    }
}
