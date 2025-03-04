using System.Windows;
using Microsoft.Extensions.Caching;
using EnterApp.Services;
using Microsoft.Extensions.Caching.Memory;

namespace EnterApp
{
    public partial class AdminWindow : Window
    {
        private readonly ClientService _clientService;
        private readonly IMemoryCache _cache;
        private readonly Logger _logger;
        private readonly string _userName;
        private readonly string _uniqueID;
        public AdminWindow(string userName, string uniqueID, IMemoryCache cache)
        {
            InitializeComponent();
            _cache = cache;
            _clientService = new ClientService(App.DbContext, _cache);
            _logger = new Logger(App.DbContext);
            _userName = userName;
            WelcomeLabel.Content = $"Добро пожаловать, {userName}!";
            _uniqueID = uniqueID;
        }
        private void EditClientButton_Click(object sender, RoutedEventArgs e)
        {
            var editClients = new EditClientsWindow(_userName, _cache);
            editClients.Show();
        }
        private async void ViewLogsButton_Click(object sender, RoutedEventArgs e)
        {
            var logsWindow = new LogsWindow();
            logsWindow.ShowDialog();
            await _logger.LogActionAsync(_userName, "Admin", "Просмотр логов", false);
        }
        private async void AddClientButton_Click(object sender, RoutedEventArgs e)
        {
            var addClientWindow = new AddClientWindow(_cache);
            addClientWindow.ShowDialog();
            await _logger.LogActionAsync(_userName, "Admin", "Открыто окно добавления пользователя", false);
        }
        private async void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            bool isLoggedOut = await _clientService.LogoutClientAsync(_uniqueID);
            if (isLoggedOut)
            {
                MessageBox.Show("Успешный выход из системы");
                var loginWindow = new MainWindow();
                loginWindow.Show();
                this.Close();
                await _clientService.LogoutClientAsync(_uniqueID);
                await _logger.LogActionAsync(_userName, "Admin", "Пользователь вышел из системы", false);
            }
            else
            {
                MessageBox.Show("Ошибка при выходе из системы");
            }
        }
        private void ViewOnline_Click(object sender, RoutedEventArgs e)
        {
            var OnlineUsersWindow = new OnlineUsersWindow(_clientService);
            OnlineUsersWindow.Show();
        }

    }
}
