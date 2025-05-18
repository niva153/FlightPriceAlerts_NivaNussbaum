
using FlightPriceAlert.Application.DTOs;

namespace FlightPriceAlert.Application.Interfaces
{
    public interface IAlertService
    {
        Task<IEnumerable<AlertResponseDto>> GetUserAlertsAsync(Guid userId);
        Task<AlertResponseDto?> GetAlertByIdAsync(Guid alertId, Guid userId);
        Task<AlertResponseDto> CreateAlertAsync(CreateAlertDto createAlertDto, Guid userId);
        Task<bool> UpdateAlertAsync(UpdateAlertDto updateAlertDto, Guid userId);
        Task<bool> DeleteAlertAsync(Guid alertId, Guid userId);
    }
}
