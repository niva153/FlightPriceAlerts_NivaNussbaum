
using FlightPriceAlert.Domain.Models;
using FlightPriceAlert.Infrastructure.Data;
using FlightPriceAlert.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlightPriceAlert.Infrastructure.Repositories
{
    public class AlertRepository : IAlertRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<AlertRepository> _logger;

        public AlertRepository(ApplicationDbContext dbContext, ILogger<AlertRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Alert?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Alerts.FindAsync(id);
        }

        public async Task<IEnumerable<Alert>> GetByUserIdAsync(Guid userId)
        {
            return await _dbContext.Alerts
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.UpdatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Alert>> GetActiveAlertsAsync()
        {
            return await _dbContext.Alerts
                .Where(a => a.IsActive)
                .ToListAsync();
        }

        public async Task<Alert> AddAsync(Alert alert)
        {
            await _dbContext.Alerts.AddAsync(alert);
            await _dbContext.SaveChangesAsync();
            return alert;
        }

        public async Task UpdateAsync(Alert alert)
        {
            _dbContext.Alerts.Update(alert);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Alert alert)
        {
            _dbContext.Alerts.Remove(alert);
            await _dbContext.SaveChangesAsync();
        }
    }
}
