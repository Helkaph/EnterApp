using Microsoft.EntityFrameworkCore;
using EnterApp.Models;
using Microsoft.Extensions.Caching.Memory;

namespace EnterApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IMemoryCache _cache;
        public DbSet<Client> Clients { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<DeletedClient> Deleted_Clients { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMemoryCache cache) : base(options)
        {
            _cache = cache;
        }
        public async Task<List<Client>> GetCachedClientsAsync()
        {
            if (!_cache.TryGetValue("ClientsCache", out List<Client> clients))
            {
                clients = await Clients.ToListAsync();
                _cache.Set("ClientsCache", clients, TimeSpan.FromMinutes(10));
            }
            return clients;
        }
        public void UpdateClientsCache()
        {
            _cache.Remove("ClientsCache");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Client>().HasIndex(c => c.Unique_Id).IsUnique();
            modelBuilder.Entity<Log>().HasIndex(l => l.ActionTime);
        }
    }
}
