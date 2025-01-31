using Newtonsoft.Json;
using StackExchange.Redis;

namespace MensageriaDominio
{
    public class RedisService
    {
        private readonly ConnectionMultiplexer _redis;

        public RedisService(string connectionString)
        {
            _redis = ConnectionMultiplexer.Connect(connectionString);
        }

        public void PublishMessage(string queueName, string message)
        {
            var db = _redis.GetDatabase();
            var jsonMessage = JsonConvert.SerializeObject(message);
            db.ListRightPush(queueName, jsonMessage);
        }

        public string ConsumeMessage(string queueName)
        {
            var db = _redis.GetDatabase();
            return db.ListLeftPop(queueName);
        }
    }
}