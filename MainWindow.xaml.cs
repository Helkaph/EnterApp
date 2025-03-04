using System.Windows;
using EnterApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace EnterApp
{
    public partial class MainWindow : Window
    {
        private readonly Logger _logger;
        private readonly IMemoryCache _cache;
        public MainWindow()
        {
            InitializeComponent();
            _logger = new Logger(App.DbContext);
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginButton.IsEnabled = false;
            string uniqueId = UniqueIdTextBox.Text;
            if (uniqueId.Length != 8)
            {
                MessageBox.Show("Вы ввели идентификатор некорректной длины.");
                await _logger.LogActionAsync("unauthorized", "unauthorized", "Совершена неудачная попытка входа", true);
                LoginButton.IsEnabled = true;
                return;
            }
            var client = await App.DbContext.Clients.FirstOrDefaultAsync(c => c.Unique_Id == uniqueId);

            if (client == null)
            {
                MessageBox.Show("Пользователь не найден. Свяжитесь с администратором.");
                await _logger.LogActionAsync("unauthorized", "unauthorized", "Совершена неудачная попытка входа", true);
                LoginButton.IsEnabled = true;
                return;
            }
            if (client.Role == "Admin")
            {
                client.Authorized = true;
                await App.DbContext.SaveChangesAsync();
                await _logger.LogActionAsync(client.Name, client.Role,"Произведён вход администратора", false);
                var adminWindow = new AdminWindow(client.Name, client.Unique_Id, _cache);
                adminWindow.Show();
                this.Close();
            }
            else if (client.Role == "Visitor")
            {
                client.Authorized = true;
                await App.DbContext.SaveChangesAsync();
                await _logger.LogActionAsync(client.Name, client.Role, "Произведён вход пользователя", false);
                MessageBox.Show("Добро пожаловать в систему!");
                var userWindow = new UserWindow(client.Name, client.Unique_Id, _cache);
                userWindow.Show();
                this.Close();
            }

            LoginButton.IsEnabled = true;
        }
    }
}