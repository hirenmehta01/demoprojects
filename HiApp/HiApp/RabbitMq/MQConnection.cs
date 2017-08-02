using RabbitMQ.Client;
using System.Configuration;
using System;

namespace HiApp.RabbitMq
{
    class MQConnection
    {
        /// <summary>
        /// connection object for rabbit mq
        /// </summary>
        internal IConnection connection = null;

        static internal string ExchangeName = "HiApp";

        ConnectionProp ConnProp { get; set; }

        public MQConnection()
        {
            ConnProp = new ConnectionProp();
        }

        /// <summary>
        /// make connection with mq.
        /// </summary>
        internal void Connect()
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = ConnProp.Host,
                Port = ConnProp.Port,
                UserName = ConnProp.UserName,
                Password = ConnProp.Password,
                VirtualHost = "/"
            };
            connection = connectionFactory.CreateConnection();
        }
    }

    class ConnectionProp
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public ConnectionProp()
        {
            SetProperties();
        }

        private void SetProperties()
        {
            var connection = ConfigurationManager.AppSettings["mqconnection"].ToString();
            var connArray = connection.Split(';');
            foreach (var item in connArray)
            {
                var itemSplit = item.Split(':');
                if (itemSplit[0].Equals("HostName"))
                {
                    Host = itemSplit[1];
                    continue;
                }
                else if (itemSplit[0].Equals("Port"))
                {
                    Port = Convert.ToInt32(itemSplit[1]);
                    continue;
                }
                else if (itemSplit[0].Equals("UserName"))
                {
                    UserName = itemSplit[1];
                    continue;
                }
                else if (itemSplit[0].Equals("Password"))
                {
                    Password = itemSplit[1];
                    continue;
                }
            }
        }
    }
}
