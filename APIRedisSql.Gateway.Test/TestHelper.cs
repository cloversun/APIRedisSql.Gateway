using System.Collections.Generic;
using APIRedisSql.Gateway.Model;

namespace APIRedisSql.Gateway.Test
{
    public class TestHelper
    {
        public static BBPostModel CreateBBModel()
        {
            var BbPostModel = new BBPostModel
            {
                ChannelID = 1,
            ChannelName = "ChannelName1"
            };
            return BbPostModel;
        }

        public static List<BBPostModel> CreateBBModelList()
        {
            var BbPostModel1 = CreateBBModel();
            var BbPostModel2 = new BBPostModel
            {
                ChannelID = 2,
                ChannelName = "ChannelName2"
            };
            var BBModelList = new List<BBPostModel>() { BbPostModel1, BbPostModel2 };
            return BBModelList;
        }
    }
}
