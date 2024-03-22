namespace Infrastructure.Data
{
    using Application.Models;
    using Microsoft.EntityFrameworkCore;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
       : base(options)
        {
        }



        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //options.UseNpgsql(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"));
            options.UseNpgsql("Host=localhost; Port=5432; Database=flowmeterWeb; Username=postgres; Password=12345");
        }

        public DbSet<Account> accounts { get; set; }
        public DbSet<Accrual> accruals { get; set; }
        public DbSet<Admin> admins { get; set; }
        public DbSet<Consumer> consumers { get; set; }
        public DbSet<Meter> meters { get; set; }
        public DbSet<House> houses { get; set; }
        public DbSet<Payment> payments { get; set; }
        public DbSet<Receipt> receipts { get; set; }
        public DbSet<Service> services { get; set; }
        public DbSet<User> users { get; set; }
    }
}
