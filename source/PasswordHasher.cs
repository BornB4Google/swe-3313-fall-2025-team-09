using System.Security.Cryptography;
using System.Text;

namespace Backend
{
    public static class PasswordHasher
    {
        public static string ComputePasswordHash(string input)
        {
            using var hash = SHA256.Create();
            byte[] result = hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            var sb = new StringBuilder();
            foreach (var b in result)
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }
    }
}