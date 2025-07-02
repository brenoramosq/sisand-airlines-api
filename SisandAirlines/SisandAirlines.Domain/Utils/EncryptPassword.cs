using System.Security.Cryptography;
using System.Text;

namespace SisandAirlines.Domain.Utils
{
    public static class EncryptPassword
    {
        public static string ToSHA256(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordInBytes = Encoding.UTF8.GetBytes(password);

                byte[] hash = sha256.ComputeHash(passwordInBytes);

                StringBuilder stringBuilder = new StringBuilder();

                for (int i = 0; i < hash.Length; i++)
                {
                    stringBuilder.Append(hash[i].ToString("x2"));
                }

                return stringBuilder.ToString();
            }
        }
    }
}
