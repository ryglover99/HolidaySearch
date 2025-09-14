using HolidaySearch.Models;
using HolidaySearch.Services;

namespace HolidaySearch.Tests.Services
{
    public class HolidaySearchServiceTests
    {
        [Fact]
        public async Task Search_ManchesterToMalaga_July7_ReturnsResultsWithBestValueHotelAndFlight()
        {
            //Arrange
            var jsonDataLoader = new JsonDataLoaderService();
            var holidaySearchService = new HolidaySearchService(jsonDataLoader);

            //Act
            var results = await holidaySearchService.SearchAsync(new HolidaySearchRequest()
            {
                DepartingFrom = ["MAN"],
                TravelingTo = "AGP",
                DepartureDate = DateOnly.Parse("2023/07/01"),
                Duration = 7
            });

            //Assert
            Assert.NotNull(results);
            Assert.IsType<HolidaySearchResponse>(results);
            Assert.NotNull(results.HolidayPackages);
            Assert.IsAssignableFrom<IEnumerable<HolidayPackage>>(results.HolidayPackages);
            Assert.NotEmpty(results.HolidayPackages);

            var firstResult = results.HolidayPackages.First();
            Assert.Equal(2, firstResult.Flight.Id);
            Assert.Equal("MAN", firstResult.Flight.From);
            Assert.Equal("AGP", firstResult.Flight.To);
            Assert.True(firstResult.Flight.Price > 0);

            Assert.Equal(9, firstResult.Hotel.Id);
            Assert.True(firstResult.Hotel.PricePerNight > 0);
            Assert.False(string.IsNullOrWhiteSpace(firstResult.Hotel.Name));
        }

        [Fact]
        public async Task Search_LondonToMallorca_June15_ReturnsResultsWithBestValueHotelAndFlight()
        {
            //Arrange
            var jsonDataLoader = new JsonDataLoaderService();
            var holidaySearchService = new HolidaySearchService(jsonDataLoader);

            //Act
            var results = await holidaySearchService.SearchAsync(new HolidaySearchRequest(){
                DepartingFrom = ["LTN", "LGW"],
                TravelingTo = "PMI",
                DepartureDate = DateOnly.Parse("2023/06/15"),
                Duration = 10
            });

            //Assert
            Assert.NotNull(results);
            Assert.IsType<HolidaySearchResponse>(results);
            Assert.NotNull(results.HolidayPackages);
            Assert.IsAssignableFrom<IEnumerable<HolidayPackage>>(results.HolidayPackages);
            Assert.NotEmpty(results.HolidayPackages);

            var firstResult = results.HolidayPackages.First();
            Assert.Equal(6, firstResult.Flight.Id);
            Assert.True(firstResult.Flight.Price > 0);

            Assert.Equal(5, firstResult.Hotel.Id);
            Assert.True(firstResult.Hotel.PricePerNight > 0);
            Assert.False(string.IsNullOrWhiteSpace(firstResult.Hotel.Name));
        }

        [Fact]
        public async Task Search_AnywhereToGranCanaria_Nov10_ReturnsResultsWithBestValueHotelAndFlight()
        {
            //Arrange
            var jsonDataLoader = new JsonDataLoaderService();
            var holidaySearchService = new HolidaySearchService(jsonDataLoader);

            //Act
            var results = await holidaySearchService.SearchAsync(new HolidaySearchRequest(){
                DepartingFrom = ["MAN", "TFS", "AGP", "PMI", "LTN", "LGW", "LPA"],
                TravelingTo = "LPA",
                DepartureDate = DateOnly.Parse("2022/11/10"),
                Duration = 14
            });

            //Assert
            Assert.NotNull(results);
            Assert.IsType<HolidaySearchResponse>(results);
            Assert.NotNull(results.HolidayPackages);
            Assert.IsAssignableFrom<IEnumerable<HolidayPackage>>(results.HolidayPackages);
            Assert.NotEmpty(results.HolidayPackages);

            var firstResult = results.HolidayPackages.First();
            Assert.Equal(7, firstResult.Flight.Id);
            Assert.Equal("LPA", firstResult.Flight.To);
            Assert.True(firstResult.Flight.Price > 0);

            Assert.Equal(6, firstResult.Hotel.Id);
            Assert.True(firstResult.Hotel.PricePerNight > 0);
            Assert.False(string.IsNullOrWhiteSpace(firstResult.Hotel.Name));
        }

        [Fact]
        public async Task Search_WithNoMatchingFlights_ReturnEmptyResults()
        {
            //Arrange
            var jsonDataLoader = new JsonDataLoaderService();
            var holidaySearchService = new HolidaySearchService(jsonDataLoader);

            //Act
            var results = await holidaySearchService.SearchAsync(new HolidaySearchRequest(){
                DepartingFrom = ["MAN"],
                TravelingTo = "LTN",
                DepartureDate = DateOnly.Parse("2022/11/10"),
                Duration = 6
            });

            //Assert
            Assert.NotNull(results);
            Assert.IsType<HolidaySearchResponse>(results);
            Assert.NotNull(results.HolidayPackages);
            Assert.IsAssignableFrom<IEnumerable<HolidayPackage>>(results.HolidayPackages);
            Assert.Empty(results.HolidayPackages);
        }

        [Fact]
        public async Task Search_WithEmptySearchInput_ThrowNullArgumentException()
        {
            //Arrange
            var jsonDataLoader = new JsonDataLoaderService();
            var holidaySearchService = new HolidaySearchService(jsonDataLoader);

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var results = await holidaySearchService.SearchAsync(new HolidaySearchRequest(){
                    DepartingFrom = null,
                    TravelingTo = null,
                    DepartureDate = DateOnly.Parse("2022/11/10"),
                    Duration = 0
                });
            });
        }

        [Fact]
        public async Task Search_WithDurationZero_ReturnsEmptyResults()
        {
            //Arrange
            var jsonDataLoader = new JsonDataLoaderService();
            var holidaySearchService = new HolidaySearchService(jsonDataLoader);

            //Act
            var results = await holidaySearchService.SearchAsync(new HolidaySearchRequest(){
                DepartingFrom = ["MAN"],
                TravelingTo = "LTN",
                DepartureDate = DateOnly.Parse("2022/11/10"),
                Duration = 0
            });

            //Assert
            Assert.NotNull(results);
            Assert.IsType<HolidaySearchResponse>(results);
            Assert.NotNull(results.HolidayPackages);
            Assert.IsAssignableFrom<IEnumerable<HolidayPackage>>(results.HolidayPackages);
            Assert.Empty(results.HolidayPackages);
        }

    }
}
