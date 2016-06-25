using System;
using System.Configuration;
using System.Linq;
using NUnit.Framework;
using StackExchange.Redis;

namespace APIRedisSql.Gateway.Test
{
    [TestFixture]
    public class RedisDalTest
    {
        [Test]
        public void PushToRedisTest()
        {
            Redishelper.Delete("testRedisList");
            Redishelper.Push("testRedisList", new SortingInfo());
            //Assert.AreEqual(resultnumber,1);
        }

        [Test]
        public void PopFromRedisTest()
        {
            Redishelper.Delete("testRedisList");
            Redishelper.Push("testRedisList", "test1");
            Redishelper.Push("testRedisList", "test2");
            var popObject = Redishelper.Pop<string>("testRedisList");
            Assert.AreEqual(popObject, "test1");
        }

        [Test]
        public void RedisConnectTest()
        {
            var redisConnectionString = ConfigurationManager.ConnectionStrings["RedisConnection"].ConnectionString;
            int redisDatebaseId = Convert.ToInt16(ConfigurationManager.AppSettings["redis.databaseId"]);
            //取连接对象
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(redisConnectionString);
            //取得DB对象
            IDatabase db = redis.GetDatabase(redisDatebaseId);
            //设置数据对象
            db.StringSet("User", "{Name:\"TOM\"}");
            //添加数据对象
            db.StringAppend("User", ",{Name:\"JACK\"}");
            //取得数据对象
            string user = db.StringGet("User");
            Assert.AreEqual("{Name:\"TOM\"},{Name:\"JACK\"}", user);
        }

        [Test]
        public void RedisToApi()
        {
            Redishelper.Delete("SortingList");
            var peroidLists = SqlDal.GetPeriodSortingInfoList();
            if (peroidLists.Count <= 0) return;
            Redishelper.PushList<SortingInfo>("SortingList", peroidLists);
            var peroidListsPop = Redishelper.PopList<SortingInfo>("SortingList", Convert.ToInt16(ConfigurationManager.AppSettings["OncRedisPopNumber"]));
            var a = peroidListsPop.First();
            Assert.IsNotNull(a.WaybillNumber);
        }
    }
}