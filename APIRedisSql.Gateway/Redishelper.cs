using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using StackExchange.Redis;
using APIRedisSql.Gateway.Extensions;


namespace APIRedisSql.Gateway
{


    public static class Redishelper
    {
        private static readonly string RedisConnectionString = ConfigurationManager.ConnectionStrings["RedisConnection"].ConnectionString;
        private static readonly int RedisDatebaseId = Convert.ToInt16(ConfigurationManager.AppSettings["redis.databaseId"]);

        public static ConnectionMultiplexer OpenConnection()
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(RedisConnectionString);
            return redis;
        }

        private static IDatabase ConnectRedisAndReturnIDatabase()
        {
            ConnectionMultiplexer redis = OpenConnection();
            return redis.GetDatabase(RedisDatebaseId);
        }

        public static void Push<T>(string key, T t)
        {
            IDatabase db = ConnectRedisAndReturnIDatabase();
            db.ListLeftPush(key, t.ToJson());
        }
        public static void PushList<T>(string key, List<T> listT)
        {
            foreach (var t in listT)
            {
                Push<T>(key, t);
            }
        }

        public static Task<bool> BoolPushList<T>(string key, List<T> listT)
        {
            return Task.Run(() =>
            {
                PushList<T>(key, listT);
                return true;
            });
        }

        public static T Pop<T>(string key)
        {
            IDatabase db = ConnectRedisAndReturnIDatabase();
            RedisValue value = db.ListRightPop(key);
            if (value.HasValue)
            {
                return value.ToString().ToObject<T>();
            }
            return default(T);
        }
        public static List<T> PopList<T>(string key, int count)
        {
            List<T> list = new List<T>();

            for (int i = 0; i < count; i++)
            {
                var a = Pop<T>(key);
                if (a != null)
                {
                    list.Add(a);
                }
                else
                {
                    return list;
                }

            }
            return list;
        }

        public static void Delete(string key)
        {
            IDatabase db = ConnectRedisAndReturnIDatabase();
            db.KeyDelete(key);
        }
    }
}

