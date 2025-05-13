using Newtonsoft.Json;

namespace csharp_mfca.API.Extensions
{
    public static class Extension
    {
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T ToObject<T>(this string str)
        {
            return JsonConvert.DeserializeObject<T>(str)!;
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);
        }
    }
}
