
using NUnit.Framework;
using APIRedisSql.Gateway.Model;

namespace APIRedisSql.Gateway.Test
{
    [TestFixture]
    public class SqlDalTest
    {

        [Test]
        public void GetPeriodSortingInfoSqlToRedisTest()
        {
            var peroidLists = SqlDal.GetPeriodAAModelList();
            if (peroidLists.Count > 0)
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
