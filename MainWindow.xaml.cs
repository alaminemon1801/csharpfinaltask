using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace PasswordResetApp
{
    public partial class MainWindow : Window
    {
        private PasswordResetAPP.CreatePassword _createPassword = new PasswordResetAPP.CreatePassword();
        private PasswordResetAPP.FileStorage _fileStorage = new PasswordResetAPP.FileStorage();
        private PasswordResetAPP.BruteForce _bruteForce;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CreateEncryptPassword_Click(object sender, RoutedEventArgs e)
        {
            string password = PasswordTextBox.Text;
            string salt = SaltTextBox.Text;
            if (string.IsNullOrEmpty(salt))
            {
                MessageBox.Show("Please enter a salt value.");
                return;
            }
            string encryptedPassword = _createPassword.EncryptPassword(password, salt);
            _fileStorage.SaveToFile(encryptedPassword, "password.txt");
            ResultTextBlock.Text = "Password encrypted and saved.";
        }

        private void StartBruteForce_Click(object sender, RoutedEventArgs e)
        {
            string encryptedPassword = _fileStorage.ReadFromFile("password.txt");
            if (string.IsNullOrEmpty(encryptedPassword))
            {
                ResultTextBlock.Text = "Error reading encrypted password from file.";
                return;
            }

            if (ThreadsComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select the number of threads.");
                return;
            }

            int maxThreads = int.Parse((ThreadsComboBox.SelectedItem as ComboBoxItem).Content.ToString());
            _bruteForce = new PasswordResetAPP.BruteForce(encryptedPassword);

            var stopwatch = Stopwatch.StartNew();
            string foundPassword = _bruteForce.BruteForceAttack(8, 32); // Example max length and max threads
            stopwatch.Stop();

            ResultTextBlock.Text = $"Found Password: {foundPassword}";
            TimeTextBlock.Text = $"Time Taken: {stopwatch.ElapsedMilliseconds} ms";
        }
    }
}
