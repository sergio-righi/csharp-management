using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Tool.Utilities
{
    public static class Cryptography
    {
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private const string defaultKey = "VYwBn9AE9Ynpmuip";
        // This constant is used to determine the keysize of the encryption algorithm
        private const int key_length = 256;

        private static string GenerateHash(string data)
        {
            byte[] hash;

            var bytes = new UTF8Encoding().GetBytes(data);

            using (var algoritmo = new SHA512Managed())
            {
                hash = algoritmo.ComputeHash(bytes);
            }

            return Convert.ToBase64String(hash);
        }
        
        private static string Encrypt(string data, string key)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            byte[] init = Encoding.UTF8.GetBytes(defaultKey);

            PasswordDeriveBytes password = new PasswordDeriveBytes(key, null);

            byte[] keyBytes = password.GetBytes(key_length / 8);

            RijndaelManaged symetricKey = new RijndaelManaged();

            symetricKey.Mode = CipherMode.CBC;

            ICryptoTransform encryptor = symetricKey.CreateEncryptor(keyBytes, init);

            MemoryStream memoryStream = new MemoryStream();

            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

            cryptoStream.Write(dataBytes, 0, dataBytes.Length);

            cryptoStream.FlushFinalBlock();

            byte[] hashBytes = memoryStream.ToArray();

            memoryStream.Close();

            cryptoStream.Close();

            return Convert.ToBase64String(hashBytes);
        }

        private static string Decrypt(string data, string key)
        {
            byte[] init = Encoding.UTF8.GetBytes(defaultKey);

            byte[] hashBytes = Convert.FromBase64String(data);

            PasswordDeriveBytes password = new PasswordDeriveBytes(key, null);

            byte[] keyBytes = password.GetBytes(key_length / 8);

            RijndaelManaged symmetricKey = new RijndaelManaged();

            symmetricKey.Mode = CipherMode.CBC;

            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, init);

            MemoryStream memoryStream = new MemoryStream(hashBytes);

            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

            byte[] dataBytes = new byte[hashBytes.Length];

            int decryptedCount = cryptoStream.Read(dataBytes, 0, dataBytes.Length);

            memoryStream.Close();

            cryptoStream.Close();

            return Encoding.UTF8.GetString(dataBytes, 0, decryptedCount);
        }

        public static string GetHash(string data)
        {
            return GenerateHash(data);
        }

        public static string GetEncrypted(string data, string key = "VYwBn9AE9YnpmuipDCoySUcS3YczYQh7ZSVgPqMzt9Jhmr6R")
        {
            return !string.IsNullOrWhiteSpace(data) ? Encrypt(data.Trim(), key) : null;
        }

        public static string GetDecrypted(string data, string key = "VYwBn9AE9YnpmuipDCoySUcS3YczYQh7ZSVgPqMzt9Jhmr6R")
        {
            return !string.IsNullOrWhiteSpace(data) ? Decrypt(data.Trim(), key) : null;
        }
    }
}
