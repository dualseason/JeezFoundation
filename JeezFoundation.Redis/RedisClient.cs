using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;

namespace JeezFoundation.Redis
{

    public class RedisClient : IDisposable
    {
        private IConfiguration _config;
        private ConcurrentDictionary<string, ConnectionMultiplexer> _connections;
        public RedisClient(IConfiguration config)
        {
            _config = config;
            _connections = new ConcurrentDictionary<string, ConnectionMultiplexer>();
        }
        /// <summary>
        /// ��ȡConnectionMultiplexer
        /// </summary>
        /// <param name="redisConfig">RedisConfig�����ļ�</param>
        /// <returns></returns>
        private ConnectionMultiplexer GetConnect(IConfigurationSection redisConfig)
        {
            var redisInstanceName = redisConfig["InstanceName"];
            var connStr = redisConfig["Connection"];
            return _connections.GetOrAdd(redisInstanceName, p => ConnectionMultiplexer.Connect(connStr));
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="configName">RedisConfig�����ļ��е� Redis_Default/Redis_6 ����</param>
        /// <returns></returns>
        private IConfigurationSection CheckeConfig(string configName)
        {
            IConfigurationSection redisConfig = _config.GetSection("RedisConfig").GetSection(configName);
            if (redisConfig == null)
            {
                throw new ArgumentNullException($"{configName}�Ҳ�����Ӧ��RedisConfig���ã�");
            }
            var redisInstanceName = redisConfig["InstanceName"];
            var connStr = redisConfig["Connection"];
            if (string.IsNullOrEmpty(redisInstanceName))
            {
                throw new ArgumentNullException($"{configName}�Ҳ�����Ӧ��InstanceName");
            }
            if (string.IsNullOrEmpty(connStr))
            {
                throw new ArgumentNullException($"{configName}�Ҳ�����Ӧ��Connection");
            }
            return redisConfig;
        }
        /// <summary>
        /// ��ȡ���ݿ�
        /// </summary>
        /// <param name="configName"></param>
        /// <param name="db">Ĭ��Ϊ0�����ȴ����db���ã����config�е�����</param>
        /// <returns></returns>
        public IDatabase GetDatabase(string configName = null, int? db = null)
        {
            int defaultDb = 0;
            IConfigurationSection redisConfig = CheckeConfig(configName);
            if (db.HasValue)
            {
                defaultDb = db.Value;
            }
            else
            {
                var strDefalutDatabase = redisConfig["DefaultDatabase"];
                if (!string.IsNullOrEmpty(strDefalutDatabase) && Int32.TryParse(strDefalutDatabase, out var intDefaultDatabase))
                {
                    defaultDb = intDefaultDatabase;
                }
            }
            return GetConnect(redisConfig).GetDatabase(defaultDb);
        }

        public IServer GetServer(string configName = null, int endPointsIndex = 0)
        {
            IConfigurationSection redisConfig = CheckeConfig(configName);
            var connStr = redisConfig["Connection"];

            var confOption = ConfigurationOptions.Parse((string)connStr);
            return GetConnect(redisConfig).GetServer(confOption.EndPoints[endPointsIndex]);
        }

        public ISubscriber GetSubscriber(string configName = null)
        {
            IConfigurationSection redisConfig = CheckeConfig(configName);
            return GetConnect(redisConfig).GetSubscriber();
        }

        public void Dispose()
        {
            if (_connections != null && _connections.Count > 0)
            {
                foreach (var item in _connections.Values)
                {
                    item.Close();
                }
            }
        }
    }
}