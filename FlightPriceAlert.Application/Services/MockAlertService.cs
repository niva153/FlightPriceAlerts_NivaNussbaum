
using FlightPriceAlert.Application.DTOs;
using FlightPriceAlert.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace FlightPriceAlert.Application.Services
{
    public class MockAlertService : IAlertService
    {
        private readonly ILogger<MockAlertService> _logger;
        private readonly List<AlertResponseDto> _mockAlerts;

        public MockAlertService(ILogger<MockAlertService> logger)
        {
            _logger = logger;
            _mockAlerts = GenerateMockAlerts();
        }

        public Task<IEnumerable<AlertResponseDto>> GetUserAlertsAsync(Guid userId)
        {
            _logger.LogInformation($"MockAlertService: Getting alerts for user {userId}");
            return Task.FromResult(_mockAlerts.Where(a => a.UserId == userId));
        }

        public Task<AlertResponseDto?> GetAlertByIdAsync(Guid alertId, Guid userId)
        {
            _logger.LogInformation($"MockAlertService: Getting alert {alertId} for user {userId}");
            return Task.FromResult(_mockAlerts.FirstOrDefault(a => a.Id == alertId && a.UserId == userId));
        }

        public Task<AlertResponseDto> CreateAlertAsync(CreateAlertDto createAlertDto, Guid userId)
        {
            _logger.LogInformation($"MockAlertService: Creating new alert for user {userId}");

            var alert = new AlertResponseDto
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                DepartureAirport = createAlertDto.DepartureAirport,
                ArrivalAirport = createAlertDto.ArrivalAirport,
                DepartureDate = createAlertDto.DepartureDate,
                ReturnDate = createAlertDto.ReturnDate,
                IsFlexibleDate = createAlertDto.IsFlexibleDate,
                PriceThreshold = createAlertDto.PriceThreshold,
                Currency = createAlertDto.Currency,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _mockAlerts.Add(alert);
            return Task.FromResult(alert);
        }

        public Task<bool> UpdateAlertAsync(UpdateAlertDto updateAlertDto, Guid userId)
        {
            _logger.LogInformation($"MockAlertService: Updating alert {updateAlertDto.Id} for user {userId}");

            var alert = _mockAlerts.FirstOrDefault(a => a.Id == updateAlertDto.Id && a.UserId == userId);
            if (alert == null)
                return Task.FromResult(false);

            if (updateAlertDto.DepartureAirport != null)
                alert.DepartureAirport = updateAlertDto.DepartureAirport;

            if (updateAlertDto.ArrivalAirport != null)
                alert.ArrivalAirport = updateAlertDto.ArrivalAirport;

            if (updateAlertDto.DepartureDate.HasValue)
                alert.DepartureDate = updateAlertDto.DepartureDate;

            if (updateAlertDto.ReturnDate.HasValue)
                alert.ReturnDate = updateAlertDto.ReturnDate;

            if (updateAlertDto.IsFlexibleDate.HasValue)
                alert.IsFlexibleDate = updateAlertDto.IsFlexibleDate.Value;

            if (updateAlertDto.PriceThreshold.HasValue)
                alert.PriceThreshold = updateAlertDto.PriceThreshold.Value;

            if (updateAlertDto.Currency != null)
                alert.Currency = updateAlertDto.Currency;

            if (updateAlertDto.IsActive.HasValue)
                alert.IsActive = updateAlertDto.IsActive.Value;

            alert.UpdatedAt = DateTime.UtcNow;

            return Task.FromResult(true);
        }

        public Task<bool> DeleteAlertAsync(Guid alertId, Guid userId)
        {
            _logger.LogInformation($"MockAlertService: Deleting alert {alertId} for user {userId}");

            var alert = _mockAlerts.FirstOrDefault(a => a.Id == alertId && a.UserId == userId);
            if (alert == null)
                return Task.FromResult(false);

            _mockAlerts.Remove(alert);
            return Task.FromResult(true);
        }

        private List<AlertResponseDto> GenerateMockAlerts()
        {
            var userId1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var userId2 = Guid.Parse("22222222-2222-2222-2222-222222222222");

            return new List<AlertResponseDto>
            {
                new AlertResponseDto
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    UserId = userId1,
                    DepartureAirport = "JFK",
                    ArrivalAirport = "LAX",
                    DepartureDate = DateTime.UtcNow.AddDays(30),
                    ReturnDate = DateTime.UtcNow.AddDays(37),
                    IsFlexibleDate = true,
                    PriceThreshold = 350.00m,
                    Currency = "USD",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    UpdatedAt = DateTime.UtcNow.AddDays(-10)
                },
                new AlertResponseDto
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    UserId = userId1,
                    DepartureAirport = "SFO",
                    ArrivalAirport = "LHR",
                    DepartureDate = DateTime.UtcNow.AddDays(60),
                    ReturnDate = DateTime.UtcNow.AddDays(75),
                    IsFlexibleDate = false,
                    PriceThreshold = 800.00m,
                    Currency = "USD",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-5),
                    UpdatedAt = DateTime.UtcNow.AddDays(-5)
                },
                new AlertResponseDto
                {
                    Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    UserId = userId2,
                    DepartureAirport = "ORD",
                    ArrivalAirport = "CDG",
                    DepartureDate = DateTime.UtcNow.AddDays(45),
                    ReturnDate = DateTime.UtcNow.AddDays(52),
                    IsFlexibleDate = true,
                    PriceThreshold = 600.00m,
                    Currency = "USD",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-15),
                    UpdatedAt = DateTime.UtcNow.AddDays(-15)
                }
            };
        }
    }
}