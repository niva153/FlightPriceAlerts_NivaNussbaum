
using AutoMapper;
using FlightPriceAlert.Application.DTOs;
using FlightPriceAlert.Application.Interfaces;
using FlightPriceAlert.Domain.Models;
using FlightPriceAlert.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace FlightPriceAlert.Application.Services
{
    public class AlertService : IAlertService
    {
        private readonly IAlertRepository _alertRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AlertService> _logger;

        public AlertService(IAlertRepository alertRepository, IMapper mapper, ILogger<AlertService> logger)
        {
            _alertRepository = alertRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<AlertResponseDto>> GetUserAlertsAsync(Guid userId)
        {
            _logger.LogInformation($"AlertService: Getting alerts for user {userId}");
            var alerts = await _alertRepository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<AlertResponseDto>>(alerts);
        }

        public async Task<AlertResponseDto?> GetAlertByIdAsync(Guid alertId, Guid userId)
        {
            _logger.LogInformation($"AlertService: Getting alert {alertId} for user {userId}");
            var alert = await _alertRepository.GetByIdAsync(alertId);            
            if (alert == null || alert.UserId != userId)
                return null;
                
            return _mapper.Map<AlertResponseDto>(alert);
        }

        public async Task<AlertResponseDto> CreateAlertAsync(CreateAlertDto createAlertDto, Guid userId)
        {
            _logger.LogInformation($"AlertService: Creating new alert for user {userId}");

            var alert = _mapper.Map<Alert>(createAlertDto);            
            alert.UserId = userId;
            alert.CreatedAt = DateTime.UtcNow;
            alert.UpdatedAt = DateTime.UtcNow;
            alert.IsActive = true;
            
            if (string.IsNullOrEmpty(alert.DepartureAirport) || string.IsNullOrEmpty(alert.ArrivalAirport))
                throw new ArgumentException("Departure and arrival airports are required");
                
            if (alert.PriceThreshold <= 0)
                throw new ArgumentException("Price threshold must be greater than zero");
                
            var createdAlert = await _alertRepository.AddAsync(alert);
            return _mapper.Map<AlertResponseDto>(createdAlert);
        }

        public async Task<bool> UpdateAlertAsync(UpdateAlertDto updateAlertDto, Guid userId)
        {
            _logger.LogInformation($"AlertService: Updating alert {updateAlertDto.Id} for user {userId}");

            var existingAlert = await _alertRepository.GetByIdAsync(updateAlertDto.Id);
            
            if (existingAlert == null || existingAlert.UserId != userId)
                return false;
                
            _mapper.Map(updateAlertDto, existingAlert);
            existingAlert.UpdatedAt = DateTime.UtcNow;
            
            await _alertRepository.UpdateAsync(existingAlert);
            return true;
        }

        public async Task<bool> DeleteAlertAsync(Guid alertId, Guid userId)
        {
            _logger.LogInformation($"AlertService: Deleting alert {alertId} for user {userId}");

            var alert = await _alertRepository.GetByIdAsync(alertId);
            
            if (alert == null || alert.UserId != userId)
                return false;
                
            await _alertRepository.DeleteAsync(alert);
            return true;
        }
    }
}
