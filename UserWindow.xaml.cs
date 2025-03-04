using System.Windows;
using EnterApp.Services;
using Microsoft.Extensions.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace EnterApp
{
    public partial class UserWindow : Window
    {
        private readonly ClientService _clientService;
        private readonly Logger _logger;
        private string _userName;
        private string _uniqueId;
        private readonly IMemoryCache _cache;
        public UserWindow(string userName, string uniqueId, IMemoryCache cache)
        {
            InitializeComponent();
            _cache = cache;
            _clientService = new ClientService(App.DbContext, _cache);
            _userName = userName;
            _logger = new Logger(App.DbContext);
            _uniqueId = uniqueId;
            WelcomeLabel.Content = $"Добро пожаловать, {userName}!";
        }
        private async void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            bool isLoggedOut = await _clientService.LogoutClientAsync(_uniqueId);
            if (isLoggedOut)
            {
                MessageBox.Show("Вы вышли из системы");
                var loginWindow = new MainWindow();
                loginWindow.Show();
                await _logger.LogActionAsync(_userName, "User", "Пользователь вышел из системы", false);
                await _clientService.LogoutClientAsync(_uniqueId);
                this.Close();

            }
            else
            {
                MessageBox.Show("Произошла ошибка при выходе");
            }
        }
    }
}
