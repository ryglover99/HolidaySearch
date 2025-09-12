namespace HolidaySearch.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public string Airline { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public int Price { get; set; }
        public DateOnly DepartureDate { get; set; }

    }
}
