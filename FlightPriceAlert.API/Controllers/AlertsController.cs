
using FlightPriceAlert.Application.DTOs;
using FlightPriceAlert.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlightPriceAlert.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AlertsController : ControllerBase
    {
        private readonly IAlertService _alertService;
        private readonly ILogger<AlertsController> _logger;

        public AlertsController(IAlertService alertService, ILogger<AlertsController> logger)
        {
            _alertService = alertService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlertResponseDto>>> GetAlerts()
        {
            Guid userId = GetUserIdFromClaims();
            var alerts = await _alertService.GetUserAlertsAsync(userId);
            return Ok(alerts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AlertResponseDto>> GetAlert(Guid id)
        {
            Guid userId = GetUserIdFromClaims();
            var alert = await _alertService.GetAlertByIdAsync(id, userId);
            
            if (alert == null)
                return NotFound();
                
            return Ok(alert);
        }

        [HttpPost]
        public async Task<ActionResult<AlertResponseDto>> CreateAlert(CreateAlertDto createAlertDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            Guid userId = GetUserIdFromClaims();
            
            try
            {
                var createdAlert = await _alertService.CreateAlertAsync(createAlertDto, userId);
                return CreatedAtAction(nameof(GetAlert), new { id = createdAlert.Id }, createdAlert);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating alert");
                return StatusCode(500, "An error occurred while creating the alert");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAlert(Guid id, UpdateAlertDto updateAlertDto)
        {
            if (id != updateAlertDto.Id)
                return BadRequest("Id mismatch");
                
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            Guid userId = GetUserIdFromClaims();
            
            try
            {
                var updated = await _alertService.UpdateAlertAsync(updateAlertDto, userId);
                if (!updated)
                    return NotFound("Alert not found or you don't have permission to update it");
                    
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating alert");
                return StatusCode(500, "An error occurred while updating the alert");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlert(Guid id)
        {
            Guid userId = GetUserIdFromClaims();
            
            try
            {
                var deleted = await _alertService.DeleteAlertAsync(id, userId);
                if (!deleted)
                    return NotFound("Alert not found or you don't have permission to delete it");
                    
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting alert");
                return StatusCode(500, "An error occurred while deleting the alert");
            }
        }

        private Guid GetUserIdFromClaims()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(userIdClaim ?? throw new InvalidOperationException("User ID claim not found"));
        }
    }
}
