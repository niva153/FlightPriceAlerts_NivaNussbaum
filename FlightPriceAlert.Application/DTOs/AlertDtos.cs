namespace FlightPriceAlert.Application.DTOs
{
    public class AlertResponseDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string DepartureAirport { get; set; } = string.Empty;
        public string ArrivalAirport { get; set; } = string.Empty;
        public DateTime? DepartureDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsFlexibleDate { get; set; }
        public decimal PriceThreshold { get; set; }
        public string Currency { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateAlertDto
    {
        public string DepartureAirport { get; set; } = string.Empty;
        public string ArrivalAirport { get; set; } = string.Empty;
        public DateTime? DepartureDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsFlexibleDate { get; set; }
        public decimal PriceThreshold { get; set; }
        public string Currency { get; set; } = "USD";
    }

    public class UpdateAlertDto
    {
        public Guid Id { get; set; }
        public string? DepartureAirport { get; set; }
        public string? ArrivalAirport { get; set; }
        public DateTime? DepartureDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool? IsFlexibleDate { get; set; }
        public decimal? PriceThreshold { get; set; }
        public string? Currency { get; set; }
        public bool? IsActive { get; set; }
    }
}
