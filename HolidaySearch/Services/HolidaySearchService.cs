using HolidaySearch.Models;
using HolidaySearch.Services.Interfaces;

namespace HolidaySearch.Services
{
    public class HolidaySearchService : IHolidaySearchService
    {
        private readonly IJsonDataLoaderService _jsonDataLoaderService;
        private const string FlightDataPath = "Data/flights.json";
        private const string HotelDataPath = "Data/hotels.json";
        private const double FlightDateWeight = 100.0;
        private const double HotelDateWeight = 100.0;
        private const double PriceWeight = 1000.0;


        public HolidaySearchService(IJsonDataLoaderService jsonDataLoaderService) 
        {
            _jsonDataLoaderService = jsonDataLoaderService;
        }

        private async Task<List<Flight>> LoadAllFlights()
            => await _jsonDataLoaderService.LoadAllAsync<Flight>(FlightDataPath);
        private async Task<List<Hotel>> LoadAllHotels()
            => await _jsonDataLoaderService.LoadAllAsync<Hotel>(HotelDataPath);

        private bool IsValidFlight(Flight flight, HolidaySearchRequest request)
            =>  request.DepartingFrom.Contains(flight.From) &&
                flight.To == request.TravelingTo &&
                flight.DepartureDate >= request.DepartureDate;

        private bool IsValidHotel(Hotel hotel, Flight flight, HolidaySearchRequest request)
            =>  hotel.LocalAirports.Contains(flight.To) &&
                hotel.ArrivalDate >= request.DepartureDate &&
                hotel.Nights == request.Duration;

        private double CalculateBestValueScore(Flight flight, Hotel hotel, HolidaySearchRequest request)
        {
            var flightDaysDiff = Math.Abs((flight.DepartureDate.DayNumber - request.DepartureDate.DayNumber));
            var flightDateScore = FlightDateWeight / (1 + flightDaysDiff);

            var totalPrice = flight.Price + (hotel.PricePerNight * request.Duration);
            var priceScore = PriceWeight / (1 + totalPrice);

            var hotelDaysDiff = Math.Abs((hotel.ArrivalDate.DayNumber - request.DepartureDate.DayNumber));
            var hotelDateScore = HotelDateWeight / (1 + hotelDaysDiff);

            return flightDateScore + priceScore + hotelDateScore;
        }

        private async Task<(List<Flight> flights, List<Hotel> hotels)> LoadFlightAndHotelData()
        {
            var flightsTask = LoadAllFlights();
            var hotelsTask = LoadAllHotels();

            await Task.WhenAll(flightsTask, hotelsTask);

            var flights = await flightsTask;
            var hotels = await hotelsTask;

            return new (flights, hotels);
        }

        public async Task<HolidaySearchResponse> SearchAsync(HolidaySearchRequest request)
        {
            HolidaySearchResponse response = new HolidaySearchResponse();

            var (flights, hotels) = await LoadFlightAndHotelData();

            List<HolidayPackage> packageHolidays = [];

            foreach (var flight in flights.Where(f => IsValidFlight(f, request)))
            {
                foreach (var hotel in hotels.Where(h => IsValidHotel(h, flight, request)))
                {
                    packageHolidays.Add(new HolidayPackage()
                    {
                        Flight = flight,
                        Hotel = hotel,
                        TotalPrice = flight.Price + (hotel.PricePerNight * request.Duration),
                        BestValueScore = CalculateBestValueScore(flight, hotel, request)
                    });
                }
            }

            response.HolidayPackages = packageHolidays.OrderByDescending(p => p.BestValueScore);

            return response;

        }
      
    }
}
