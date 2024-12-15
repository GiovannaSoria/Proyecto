using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace Security
{
    public class PasswordManager
    {
        public static byte[] HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public static bool VerifyPassword(string password, byte[] storedHash)
        {
            var computedHash = HashPassword(password);
            return StructuralComparisons.StructuralEqualityComparer.Equals(storedHash, computedHash);
        }
    }
}
