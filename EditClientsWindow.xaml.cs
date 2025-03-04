using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using EnterApp.Models;
using EnterApp.Services;
using Microsoft.Extensions.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace EnterApp
{
    public partial class EditClientsWindow : Window
    {
        private readonly ClientService _clientService;
        private readonly Logger _logger;
        private readonly string _adminName;
        public EditClientsWindow(string adminName, IMemoryCache cache)
        {
            InitializeComponent();
            _clientService = new ClientService(App.DbContext, cache);
            LoadClients();
            _logger = new Logger(App.DbContext);
            _adminName = adminName;
        }
        private async void LoadClients()
        {
            var clients = await _clientService.GetAllClientsAsync();
            ClientsDataGrid.ItemsSource = clients.ToList();
        }

        private async void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsDataGrid.SelectedItem is Client client)
            {
                if (client.Unique_Id.Length != 8)
                {
                    MessageBox.Show("Идентификатор должен быть 8 символов.", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                try
                {
                    await _clientService.UpdateClientAsync(client);
                    MessageBox.Show("Клиент обновлён успешно");
                    LoadClients();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка обновления клиента: {ex.Message}");
                }
                await _logger.LogActionAsync(_adminName, "Admin", $"Изменил данные пользователя {client.Id}", false);
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите клиента для обновления");
            }
        }
        private async void DeleteClientButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedClient = ClientsDataGrid.SelectedItem as Client;
            if (selectedClient != null)
            {
                var result = MessageBox.Show($"Вы действительно хотите удалить пользователя '{selectedClient.Name}'?", "Подтвердить", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        await _logger.LogActionAsync(_adminName, "Admin", $"Удалил пользователя {selectedClient.Id}", false);
                        await _clientService.DeleteClientAsync(selectedClient);
                        MessageBox.Show("Пользователь успешно удалён");
                        LoadClients();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка удаления клиента: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите клиента для удаления");
                }
            }
        }        
    }
}
