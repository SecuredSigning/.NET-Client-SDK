using System;
using System.Text;

namespace SecuredSigningClientSdk
{
    internal class AuthHelper
    {
        /// <summary>
        /// Create request hash
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="secret"></param>
        /// <param name="date"></param>
        /// <param name="nonce"></param>
        /// <returns></returns>
        public static string CreateSignature(string apiKey, string secret, string date, string nonce)
        {
            return _CreateSignature(_FlattenRequestDetails(apiKey, date, nonce), secret);
        }
        private static string _CreateSignature(string message, string secret)
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

    internal class KeyGenerator
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
