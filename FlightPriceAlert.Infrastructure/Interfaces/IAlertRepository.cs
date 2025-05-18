
using FlightPriceAlert.Domain.Models;

namespace FlightPriceAlert.Infrastructure.Interfaces
{
    public interface IAlertRepository
    {
        Task<Alert?> GetByIdAsync(Guid id);
        Task<IEnumerable<Alert>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<Alert>> GetActiveAlertsAsync();
        Task<Alert> AddAsync(Alert alert);
        Task UpdateAsync(Alert alert);
        Task DeleteAsync(Alert alert);
    }
}
