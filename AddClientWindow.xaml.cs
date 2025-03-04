using System.Windows;
using EnterApp.Data;
using EnterApp.Models;
using EnterApp.Services;
using Microsoft.Extensions.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace EnterApp
{
    public partial class AddClientWindow : Window
    {
        private readonly ClientService _clientService;
        private readonly ApplicationDbContext _context;
        public AddClientWindow(IMemoryCache cache)
        {
            InitializeComponent();
            _clientService = new ClientService(App.DbContext, cache);
            _context = App.DbContext;
        }

        private async void AddClientButton_Click(object sender, RoutedEventArgs e)
        {
            var name = NameTextBox.Text;
            var uniqueId = IdTextBox.Text;
            var role = RoleComboBox.Text;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(uniqueId) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }
            if (uniqueId.Length != 8)
            {
                MessageBox.Show("Длина идентификатора должна быть 8 символов!");
                return;
            }
            if (_context.Clients.FirstOrDefault(c => c.Unique_Id == uniqueId) != null)
            {
                MessageBox.Show("Пользователь с таким идентификатором уже существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var newClient = new Client
            {
                Name = name,
                Unique_Id = uniqueId,
                Role = role,
                Authorized = false
            };

            await _clientService.AddClientAsync(newClient);
            MessageBox.Show("Клиент успешно добавлен");
            this.Close();
        }
    }
}
