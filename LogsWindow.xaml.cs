using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EnterApp.Services;

namespace EnterApp
{
    public partial class LogsWindow : Window
    {
        private readonly Logger _logger;
        public LogsWindow()
        {
            InitializeComponent();
            _logger = new Logger(App.DbContext);
            LoadLogs();
        }
        private async void LoadLogs()
        {
            try
            {
                var logs = await _logger.GetAllLogsAsync();
                LogsDataGrid.ItemsSource = logs.ToList();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки логов: {ex.Message}");
            }
        }
        private async void FilterLogsButton_Click(object sender, RoutedEventArgs e)
        {
            if (DatePicker.SelectedDate.HasValue)
            {
                try
                {
                    var logs = await _logger.GetLogsByDateAsync(DatePicker.SelectedDate.Value);
                    LogsDataGrid.ItemsSource = logs.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка фильтрации логов: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите дату для фильтрации логов");
            }
        }

        private void ResetFilterButton_Click(object sender, RoutedEventArgs e)
        {
            LoadLogs();
        }
    }
}
