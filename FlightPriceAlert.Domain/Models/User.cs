
namespace FlightPriceAlert.Domain.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool PushNotificationsEnabled { get; set; }
        public string DeviceToken { get; set; } = string.Empty;
        public string DevicePlatform { get; set; } = string.Empty; // iOS, Android etc.
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<Alert> Alerts { get; set; } = new List<Alert>();
    }
}
