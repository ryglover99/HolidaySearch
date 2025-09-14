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

            foreach (var flight in flights)
            {
                //Check for valid flight
                if (!request.DepartingFrom.Contains(flight.From)) continue;
                if (flight.To != request.TravelingTo) continue;
                if (flight.DepartureDate < request.DepartureDate) continue;

                var daysDiff = Math.Abs((flight.DepartureDate.DayNumber - request.DepartureDate.DayNumber));
                var dateScore = 100.0 / (1 + daysDiff);

                foreach (var hotel in hotels)
                {
                    //Check for valid hotel
                    if (!hotel.LocalAirports.Contains(flight.To)) continue;
                    if (hotel.ArrivalDate < request.DepartureDate) continue;
                    if (hotel.Nights != request.Duration) continue;

                    var totalPrice = flight.Price + (hotel.PricePerNight * request.Duration);
                    var priceScore = 1000.0 / (1 + totalPrice);

                    var arrivalDaysDiff = Math.Abs((hotel.ArrivalDate.DayNumber - request.DepartureDate.DayNumber));
                    var arrivalDateScore = 100.0 / (1 + arrivalDaysDiff);

                    packageHolidays.Add(new HolidayPackage()
                    {
                        Flight = flight,
                        Hotel = hotel,
                        TotalPrice = flight.Price + (hotel.PricePerNight * request.Duration),
                        BestValueScore = dateScore + priceScore
                    });
                }
            }

            response.HolidayPackages = packageHolidays.OrderByDescending(p => p.BestValueScore);

            return response;

        }
      
    }
}
