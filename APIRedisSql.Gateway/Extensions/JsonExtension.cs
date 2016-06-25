using Jil;
namespace APIRedisSql.Gateway.Extensions
{
    public static class JsonExtension
    {
        public static string ToJson<T>(this T t)//为什么要用this
        {
            var content = JSON.Serialize<T>(t);
            return content;
        }


        public static T ToObject<T>(this string json)
        {
            var t = JSON.Deserialize<T>(json);
            return t;
        }
    }

}
