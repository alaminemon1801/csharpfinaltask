using System.Security.Cryptography;
using System.Text;

namespace PasswordResetApp.Models
{
    public class CreatePassword
    {
        private static readonly string Salt = "STATIC_SALT";

        public string EncryptPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = password + Salt;
                var saltedPasswordBytes = Encoding.UTF8.GetBytes(saltedPassword);
                var hashBytes = sha256.ComputeHash(saltedPasswordBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
