using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecuredSigningClientSdk
{
    public class AuthHelper
    {
        /// <summary>
        /// Used by SDK and clients to make requests, so we must use the HttpWebRequest class
        /// </summary>
        /// <param name="webRequest"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static string CreateToken(string apiKey, string secret, string date, string nonce)
        {
            return _CreateToken(_FlattenRequestDetails(apiKey, date, nonce), secret);
        }
        private static string _CreateToken(string message, string secret)
        {
            // don't allow null secrets
            secret = secret ?? "";
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new System.Security.Cryptography.HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }
        private static string _FlattenRequestDetails(string apiKey, string date, string nonce)
        {
            var message = string.Format("{0}\n{1}\n{2}", apiKey, date, nonce);
            return message;
        }
    }

    public class KeyGenerator
    {
        public static string GetUniqueKey(int maxSize)
        {
            char[] chars = new char[62];
            chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            using (System.Security.Cryptography.RNGCryptoServiceProvider crypto = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }
    }
}
