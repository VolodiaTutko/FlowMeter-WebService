using FlowMeter_WebService.Models;
using Microsoft.EntityFrameworkCore;

namespace FlowMeter_WebService.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options)
	   : base(options)
		{
		}

		public DbSet<Account> accounts { get; set; }
		public DbSet<Accrual> accruals { get; set; }
		public DbSet<Admin> admins { get; set; }
		public DbSet<Consumer> consumers { get; set; }
		public DbSet<Counter> counters { get; set; }
		public DbSet<House> houses { get; set; }
		public DbSet<Payment> payments { get; set; }
		public DbSet<Receipt> receipts { get; set; }
		public DbSet<Service> services { get; set; }
		public DbSet<User> users { get; set; }
	}
}
