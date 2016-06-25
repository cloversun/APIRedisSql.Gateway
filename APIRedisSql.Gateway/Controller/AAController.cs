using System;
using System.Collections.Generic;
using APIRedisSql.Gateway.Model;
using System.Configuration;

namespace APIRedisSql.Gateway.Controller
{
    public class AAController : GatewayControllerBase
    {
        public void PostSuccessedSortingIntoSql(List<BBPostModel> value)
        {
            if (value != null)
            {
                SqlDal.InsertPostInfo(value);
            }
            else
            {
                logger.Error("get null in BBPostModel");
            }

        }
            public List<AAGetModel> GetPeroidListsFromRedis()
            {
                List<AAGetModel> peroidLists = Redishelper.PopList<AAGetModel>("SortingList", Convert.ToInt16(ConfigurationManager.AppSettings["OncRedisPopNumber"]));
                return peroidLists;
            }

    }
}
