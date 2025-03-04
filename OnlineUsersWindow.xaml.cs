using EnterApp.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using EnterApp.Models;

namespace EnterApp
{
    /// <summary>
    /// Логика взаимодействия для OnlineUsersWindow.xaml
    /// </summary>
    public partial class OnlineUsersWindow : Window
    {
        private readonly ClientService _clientService;
        public ObservableCollection<Client> OnlineUsers { get; private set; }
        private DispatcherTimer _timer;
        public OnlineUsersWindow(ClientService clientService)
        {
            InitializeComponent();
            _clientService = clientService;
            OnlineUsers = new ObservableCollection<Client>();
            DataContext = this;
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            _timer.Tick += async (s, e) => await LoadOnlineUsersAsync();
            _timer.Start();

            Task.Run(LoadOnlineUsersAsync);
        }
        private async Task LoadOnlineUsersAsync()
        {
            var users = await _clientService.GetOnlineClientsAsync();
            Application.Current.Dispatcher.Invoke(() =>
            {
                OnlineUsers.Clear();
                foreach (var user in users) 
                    OnlineUsers.Add(user);
            });
        }
    }
}
