
using FlightPriceAlert.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightPriceAlert.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Alert> Alerts { get; set; } = null!;
        public DbSet<FlightDetails> FlightPrices { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Alert>()
                .HasOne(a => a.User)
                .WithMany(u => u.Alerts)
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Alert)
                .WithMany(a => a.Notifications)
                .HasForeignKey(n => n.AlertId);
        }
    }
}
