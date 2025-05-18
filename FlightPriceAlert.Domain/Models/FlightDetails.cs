
namespace FlightPriceAlert.Domain.Models
{
    public class FlightDetails
    {
        public Guid Id { get; set; }
        public string DepartureAirport { get; set; } = string.Empty;
        public string ArrivalAirport { get; set; } = string.Empty;
        public string Airline { get; set; } = string.Empty;
        public DateTime DepartureDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string FlightNumber { get; set; } = string.Empty;
        public string Provider { get; set; } = string.Empty;
    }
}
