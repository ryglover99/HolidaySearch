using HolidaySearch.Models;
using HolidaySearch.Services.Interfaces;

namespace HolidaySearch.Services
{
    public class HolidaySearchService : IHolidaySearchService
    {
        private readonly IJsonDataLoaderService _jsonDataLoaderService;
        private const string FLIGHT_DATA_PATH = "Data/flights.json";
        private const string HOTEL_DATA_PATH = "Data/hotels.json";

        public HolidaySearchService(IJsonDataLoaderService jsonDataLoaderService) 
        {
            _jsonDataLoaderService = jsonDataLoaderService;
        }

        private async Task<List<Flight>> LoadAllFlights()
            => await _jsonDataLoaderService.LoadAllAsync<Flight>(FLIGHT_DATA_PATH);
        private async Task<List<Hotel>> LoadAllHotels()
            => await _jsonDataLoaderService.LoadAllAsync<Hotel>(HOTEL_DATA_PATH);

        private bool isValidFlight(Flight flight, HolidaySearchRequest request)
            =>  request.DepartingFrom.Contains(flight.From) &&
                flight.To == request.TravelingTo &&
                flight.DepartureDate >= request.DepartureDate;

        private bool isValidHotel(Hotel hotel, Flight flight, HolidaySearchRequest request)
            =>  hotel.LocalAirports.Contains(flight.To) &&
                hotel.ArrivalDate >= request.DepartureDate &&
                hotel.Nights == request.Duration;

        private double CalculateBestValueScore(Flight flight, Hotel hotel, HolidaySearchRequest request)
        {
            var flightDaysDiff = Math.Abs((flight.DepartureDate.DayNumber - request.DepartureDate.DayNumber));
            var flightDateScore = 100.0 / (1 + flightDaysDiff);

            var totalPrice = flight.Price + (hotel.PricePerNight * request.Duration);
            var priceScore = 1000.0 / (1 + totalPrice);

            var hotelDaysDiff = Math.Abs((hotel.ArrivalDate.DayNumber - request.DepartureDate.DayNumber));
            var hotelDateScore = 100.0 / (1 + hotelDaysDiff);

            return flightDateScore + priceScore + hotelDateScore;
        }

        public async Task<HolidaySearchResponse> SearchAsync(HolidaySearchRequest request)
        {
            HolidaySearchResponse response = new HolidaySearchResponse();

            var flightsTask = LoadAllFlights();
            var hotelsTask = LoadAllHotels();

            await Task.WhenAll(flightsTask, hotelsTask);

            var flights = await flightsTask;
            var hotels = await hotelsTask;

            //Search Algorithm

            List<HolidayPackage> packageHolidays = [];

            foreach (var flight in flights.Where(f => isValidFlight(f, request)))
            {
                foreach (var hotel in hotels.Where(h => isValidHotel(h, flight, request)))
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
