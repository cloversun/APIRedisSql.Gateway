using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using NLog;
using APIRedisSql.Gateway.Model;


namespace APIRedisSql.Gateway
{
    // Data Access Layer
    public class SqlDal
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly string Connectionstring =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;


        private static SqlConnection OpenConnection()
        {
            var connection = new SqlConnection(Connectionstring);
            connection.Open();
            return connection;
        }


        static readonly int OnceRedisPushNumber = Convert.ToInt16(ConfigurationManager.AppSettings["OncRedisPushNumber"]);
        public static List<AAGetModel> GetPeriodAAModelList()
        {

            //写and status = 0; 状态是三种，0 没取，1 取到redis里但还没有被API拿走，2取完了
            string selectSql =
                $"select top({OnceRedisPushNumber}) ChannelID as ChannelID, ChannelName as ChannelName from AAA  where MoveStatus = '0'";
            try
            {
                using (var conn = OpenConnection())
                {
                    var periodSortingInfoList = conn.Query<AAGetModel>(selectSql).ToList(); //取出数据
                    return periodSortingInfoList;
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Error happened when GetPeriodAAModelList, message:{ex}");
                return null;
            }
        }



        public static void ChangePeriodAAModelInfoListStatusInSql(List<AAGetModel> sortingInfoList)
        {
            var exchangeNumberList = sortingInfoList.Aggregate("",
                (current, sortingInfo) => current + "'" + sortingInfo.BillNumber + "'" + ",");
            string setStutasTo1 =
             $"update AAA SET MoveStatus = '1' where WaybillNumber in ({exchangeNumberList.TrimEnd(',')})";
            try
            {
                using (var conn = OpenConnection())
                {
                    conn.Query(setStutasTo1);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Error happened when ChangePeriodAAModelInfoListStatusInSql, message:{ex}");
            }
        }


        public static void InsertPostInfo(List<BBPostModel> BBModelList)
        {
            string insertString =
                $"INSERT INTO BBB ( ChannelID, ChannelName) VALUES ";
            insertString = BBModelList.Aggregate(insertString, (current, BBModel) => current + $"( {BBModel.ChannelID}, '{BBModel.ChannelName}'),");
            insertString = insertString.TrimEnd(',');
            try
            {
                using (var conn = OpenConnection())
                {
                    conn.Query(insertString);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Error happened when InsertPostInfo, message:{ex}");
            }

        }
     
        public static int GetRowNumberForTable(string tableName)
        {
            int count = 0;
            string queryString = $"select count(*) from {tableName}";
            using (var conn = OpenConnection())
            {
                var a = conn.Query<string>(queryString);
                count = Convert.ToInt16(a.First());
            }
            return count;
        }
    }
}
