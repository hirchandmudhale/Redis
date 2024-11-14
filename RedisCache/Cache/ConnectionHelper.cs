using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace RedisCacheDemo.Cache
{
    public class ConnectionHelper
    {
        static ConnectionHelper()
        {
            ConnectionHelper.lazyConnection = new Lazy<ConnectionMultiplexer>(() => {
                return ConnectionMultiplexer.Connect("localhost:6379");
                //return ConnectionMultiplexer.Connect(ConfigurationManager.GetSection("AppSettings:MaxAttachmentSize").Value;
            });
        }
        private static Lazy<ConnectionMultiplexer> lazyConnection;
        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
    }
}