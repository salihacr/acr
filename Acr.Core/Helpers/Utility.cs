using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Acr.Core.Helpers
{
    public static class Utility
    {
    }
    public static class Extensions
    {

        #region Encryption
        public static string ComputeSHA256(this string password)
        {
            string key = Constant.SecretString;
            byte[] passwordBytes = password.GetUTF8Bytes();
            byte[] salt = key.GetUTF8Bytes();
            byte[] saltedPassword = passwordBytes.Concat(salt).ToArray();
            byte[] hashedPassword = new SHA256Managed().ComputeHash(saltedPassword);
            return BitConverter.ToString(hashedPassword).Replace("-", String.Empty);
        }
        #endregion

        #region Serialize Deserialize
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
        #endregion

        private static byte[] GetUTF8Bytes(this string txt)
        {
            return Encoding.UTF8.GetBytes(txt);
        }

    }
}