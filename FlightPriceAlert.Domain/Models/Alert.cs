
namespace FlightPriceAlert.Domain.Models
{
    public class Alert
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string DepartureAirport { get; set; } = string.Empty;
        public string ArrivalAirport { get; set; } = string.Empty;
        public DateTime? DepartureDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsFlexibleDate { get; set; }
        public decimal PriceThreshold { get; set; }
        public string Currency { get; set; } = "USD";
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        

        public User? User { get; set; }
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
