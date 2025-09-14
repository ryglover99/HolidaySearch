namespace HolidaySearch.Models
{
    public class HolidaySearchRequest
    {
        public string DepartingFrom { get; set; } = string.Empty;
        public string TravelingTo { get; set; } = string.Empty;
        public DateOnly DepartureDate { get; set; }
        public int Duration { get; set; }
    }
}
