using System.IO;

namespace PasswordResetApp.Models
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
