using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acr.Core.Helpers
{
    public static class Utility
    {
    }
    public static class Extensions
    {
        public static string Serialize(this object obj)
        {
            return JsonConvert.SerializeObject(obj,
                Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
        }
        public static T Deserialize<T>(this string obj)
        {
            if (!string.IsNullOrEmpty(obj))
                return JsonConvert.DeserializeObject<T>(obj);
            else
                return default(T);
        }
        public static object Deserialize(this string obj)
        {
            return JsonConvert.DeserializeObject(obj);
        }
    }
}