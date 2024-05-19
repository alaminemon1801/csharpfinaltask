using System.IO;

namespace PasswordResetAPP
{
    public class FileStorage
    {
        public void SaveToFile(string encryptedPassword, string filePath)
        {
            File.WriteAllText(filePath, encryptedPassword);
        }

        public string ReadFromFile(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}
