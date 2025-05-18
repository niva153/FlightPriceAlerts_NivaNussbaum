
namespace FlightPriceAlert.Domain.Models
{
    public class Notification
    {
        public Guid Id { get; set; }
        public Guid AlertId { get; set; }
        public Guid UserId { get; set; }
        public Guid FlightPriceId { get; set; }
        public NotificationType Type { get; set; }
        public NotificationStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? SentAt { get; set; }
        
        public Alert? Alert { get; set; }
        public FlightDetails? FlightPrice { get; set; }
    }

    public enum NotificationType
    {
        PriceDropAlert,
        PriceRiseAlert,
        LastMinuteDeal,
        FlightScheduleChange
    }

    public enum NotificationStatus
    {
        Pending,
        Sent,
        Failed
    }
}
