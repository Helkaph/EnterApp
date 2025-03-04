using System.Windows;
using EnterApp.Data;
using EnterApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace EnterApp
{
    public partial class App : Application
    {
        public static ApplicationDbContext DbContext { get; set; }
        private IMemoryCache _cache;
        private IServiceProvider _serviceProvider;
        protected override void OnStartup(StartupEventArgs e)
        {
            
            base.OnStartup(e);

            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
            _cache = _serviceProvider.GetRequiredService<IMemoryCache>();
            string defaultConnection = ConfigurationHelper.GetConnectionString("DefaultConnection");
            string secondaryConnection = ConfigurationHelper.GetConnectionString("HomeConnection");

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            try
            {
                optionsBuilder.UseSqlServer(defaultConnection);
                DbContext = new ApplicationDbContext(optionsBuilder.Options, _cache);
                DbContext.Database.OpenConnection();
                DbContext.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Подключение к основному соединению завершено ошибкой:\n{ex.Message}.\nПробуем дополнительное соединение...");

                try
                {
                    optionsBuilder.UseSqlServer(secondaryConnection);
                    DbContext = new ApplicationDbContext(optionsBuilder.Options, _cache);
                    DbContext.Database.OpenConnection();
                    DbContext.Database.CloseConnection();
                }
                catch(Exception secEx)
                {
                    MessageBox.Show($"Подключение к дополнительному соединению завершено ошибкой:\n {secEx.Message}.\nЗавершение работы приложения");
                    Environment.Exit(1);
                }
            }
        }
        private void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
        }
    }
}
