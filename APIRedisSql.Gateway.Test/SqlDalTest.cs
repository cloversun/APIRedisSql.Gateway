using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using APIRedisSql.Gateway.Model;

namespace APIRedisSql.Gateway.Test
{
    [TestFixture]
    public class SqlDalTest
    {


        [Test]
        public void OpenConnectionTest()
        {
            SqlConnection a = SqlDal.OpenConnection();

        }
        [Test]
        public void GetPeriodSortingInfoSqlToRedisTest()
        {
            List<AAGetModel> peroidLists = SqlDal.GetPeriodAAModelList();
            if (peroidLists.Any())
            {
                Redishelper.PushList<AAGetModel>("SortingList", peroidLists);
            }

        }
        [Test]
        public void InsertSuccessedSortingInfoTest()
        {
            var BBModelList = TestHelper.CreateBBModelList();

            var preCount = SqlDal.GetRowNumberForTable("Logistics.JinFengSuccessedSorting");
            SqlDal.InsertPostInfo(BBModelList);
            var afterCount = SqlDal.GetRowNumberForTable("Logistics.JinFengSuccessedSorting");

            Assert.AreEqual(preCount + 2, afterCount);
        }

    }
}
