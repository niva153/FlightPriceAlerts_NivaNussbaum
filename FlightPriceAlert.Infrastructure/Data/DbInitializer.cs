
using FlightPriceAlert.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FlightPriceAlert.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                await context.Database.MigrateAsync();
                await SeedDataAsync(context);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<ApplicationDbContext>>();
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        private static async Task SeedDataAsync(ApplicationDbContext context)
        {
            // Only seed data if no users exist
            if (context.Users.Any())
                return;

            // Add sample users
            var user1 = new User
            {
                Id = Guid.Parse("a8b7c6d5-e4f3-42a1-b0c9-d8e7f6a5b4c3"),
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PasswordHash = "hashed_password_1",
                PhoneNumber = "1234567890",
                PushNotificationsEnabled = true,
                DeviceToken = "sample_device_token_1",
                DevicePlatform = "iOS",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var user2 = new User
            {
                Id = Guid.Parse("b7c6d5e4-f3a1-42b1-c0b9-e7f6a5b4c3d2"),
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                PasswordHash = "hashed_password_2",
                PhoneNumber = "0987654321",
                PushNotificationsEnabled = true,
                DeviceToken = "sample_device_token_2",
                DevicePlatform = "Android",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await context.Users.AddRangeAsync(user1, user2);
            
            // Add sample alerts
            var alert1 = new Alert
            {
                Id = Guid.NewGuid(),
                UserId = user1.Id,
                DepartureAirport = "JFK",
                ArrivalAirport = "LAX",
                DepartureDate = DateTime.UtcNow.AddMonths(1),
                ReturnDate = DateTime.UtcNow.AddMonths(1).AddDays(7),
                IsFlexibleDate = false,
                PriceThreshold = 300.00m,
                Currency = "USD",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var alert2 = new Alert
            {
                Id = Guid.NewGuid(),
                UserId = user1.Id,
                DepartureAirport = "SFO",
                ArrivalAirport = "ORD",
                DepartureDate = DateTime.UtcNow.AddMonths(2),
                ReturnDate = DateTime.UtcNow.AddMonths(2).AddDays(10),
                IsFlexibleDate = true,
                PriceThreshold = 400.00m,
                Currency = "USD",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var alert3 = new Alert
            {
                Id = Guid.NewGuid(),
                UserId = user2.Id,
                DepartureAirport = "LHR",
                ArrivalAirport = "CDG",
                DepartureDate = DateTime.UtcNow.AddMonths(3),
                ReturnDate = DateTime.UtcNow.AddMonths(3).AddDays(5),
                IsFlexibleDate = false,
                PriceThreshold = 200.00m,
                Currency = "EUR",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await context.Alerts.AddRangeAsync(alert1, alert2, alert3);
            await context.SaveChangesAsync();
        }
    }
}
