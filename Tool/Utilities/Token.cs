using System.Security.Cryptography;
using System.Text;

namespace Tool.Utilities
{
    public static class Token
    {
        private static string Generate(int length)
        {
            // Characters except I, l, O, 1, and 0 to decrease confusion when hand typing tokens
            string charSet = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789";

            var data = new byte[1];

            var characters = charSet.ToCharArray();

            RNGCryptoServiceProvider cryptography = new RNGCryptoServiceProvider();

            cryptography.GetNonZeroBytes(data);

            data = new byte[length];

            cryptography.GetNonZeroBytes(data);

            StringBuilder stringBuilder = new StringBuilder(length);

            foreach (var b in data)
            {
                stringBuilder.Append(characters[b % (characters.Length)]);
            }

            return stringBuilder.ToString();
        }

        public static string Get(int length = 48)
        {
            return Generate(length);
        }
    }
}
