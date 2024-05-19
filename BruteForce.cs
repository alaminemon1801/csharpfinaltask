using System.Collections.Generic;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PasswordResetAPP
{
    public class BruteForce
    {
        private readonly string _targetHash;
        private static readonly string Salt = "STATIC_SALT";

        public BruteForce(string targetHash)
        {
            _targetHash = targetHash;
        }

        public string BruteForceAttack(int maxLength, int maxThreads)
        {
            string result = null;
            ParallelOptions parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = maxThreads };

            Parallel.ForEach(GetAllPossiblePasswords(maxLength), parallelOptions, (password, state) =>
            {
                if (EncryptPassword(password) == _targetHash)
                {
                    result = password;
                    state.Stop();
                }
            });

            return result;
        }

        private IEnumerable<string> GetAllPossiblePasswords(int maxLength)
        {
            char[] chars = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            foreach (var password in GeneratePasswords(chars, maxLength))
            {
                yield return password;
            }
        }

        private IEnumerable<string> GeneratePasswords(char[] chars, int maxLength)
        {
            for (int length = 1; length <= maxLength; length++)
            {
                foreach (var password in GeneratePasswords(chars, new char[length], 0))
                {
                    yield return password;
                }
            }
        }

        private IEnumerable<string> GeneratePasswords(char[] chars, char[] currentPassword, int position)
        {
            if (position == currentPassword.Length)
            {
                yield return new string(currentPassword);
                yield break;
            }

            for (int i = 0; i < chars.Length; i++)
            {
                currentPassword[position] = chars[i];
                foreach (var password in GeneratePasswords(chars, currentPassword, position + 1))
                {
                    yield return password;
                }
            }
        }

        private string EncryptPassword(string password)
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
