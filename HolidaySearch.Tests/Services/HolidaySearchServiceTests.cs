namespace HolidaySearch.Tests.Services
{
    public class HolidaySearchServiceTests
    {
        [Fact]
        public async Task Search_ManchesterToMalaga_July7_ReturnsResultsWithBestValueHotelAndFlight()
        {
            //Arrange
            var holidaySearchService = new HolidaySearchService();

            //Act
            var results = await holidaySearchService.SearchAsync({
                DepartingFrom: "MAN",
                TravelingTo: "AGP",
                DepartureDate: DateOnly.Parse("2023/07/01"),
                Duration: 7
            });

            //Assert
            Assert.NotNull(results);
            Assert.NotEmpty(results);
            Assert.IsType<List<HolidayResult>>(results);

            var firstResult = results.First();
            Assert.Equal(2, firstResult.Flight.Id);
            Assert.Equal("MAN", firstResult.Flight.DepartingFrom);
            Assert.Equal("AGP", firstResult.Flight.TravelingTo);
            Assert.True(firstResult.Flight.Price > 0);

            Assert.Equal(9, firstResult.Hotel.Id);
            Assert.True(firstResult.Hotel.Price > 0);
            Assert.False(string.IsNullOrWhiteSpace(firstResult.Hotel.Name));
        }

        [Fact]
        public void Search_LondonToMallorca_June15_ReturnsResultsWithBestValueHotelAndFlight()
        {
            //Arrange
            var holidaySearchService = new HolidaySearchService();

            //Act
            var results = await holidaySearchService.SearchAsync({
                DepartingFrom: "LTN",
                TravelingTo: "PMI",
                DepartureDate: DateOnly.Parse("2023/06/15"),
                Duration: 10
            });

            //Assert
            Assert.NotNull(results);
            Assert.NotEmpty(results);
            Assert.IsType<List<HolidayResult>>(results);

            var firstResult = results.First();
            Assert.Equal(6, firstResult.Flight.Id);
            Assert.Equal("LTN", firstResult.Flight.DepartingFrom);
            Assert.Equal("PMI", firstResult.Flight.TravelingTo);
            Assert.True(firstResult.Flight.Price > 0);

            Assert.Equal(5, firstResult.Hotel.Id);
            Assert.True(firstResult.Hotel.Price > 0);
            Assert.False(string.IsNullOrWhiteSpace(firstResult.Hotel.Name));
        }

        [Fact]
        public void Search_AnywhereToGranCanaria_Nov10_ReturnsResultsWithBestValueHotelAndFlight()
        {
            //Arrange
            var holidaySearchService = new HolidaySearchService();

            //Act
            var results = await holidaySearchService.SearchAsync({
                DepartingFrom: "",
                TravelingTo: "LPA",
                DepartureDate: DateOnly.Parse("2022/11/10"),
                Duration: 14
            });

            //Assert
            Assert.NotNull(results);
            Assert.NotEmpty(results);
            Assert.IsType<List<HolidayResult>>(results);

            var firstResult = results.First();
            Assert.Equal(7, firstResult.Flight.Id);
            Assert.Equal("LPA", firstResult.Flight.TravelingTo);
            Assert.True(firstResult.Flight.Price > 0);

            Assert.Equal(6, firstResult.Hotel.Id);
            Assert.True(firstResult.Hotel.Price > 0);
            Assert.False(string.IsNullOrWhiteSpace(firstResult.Hotel.Name));
        }

        [Fact]
        public void Search_WithNoMatchingFlights_ReturnEmptyResults()
        {
            //Arrange
            var holidaySearchService = new HolidaySearchService();

            //Act
            var results = await holidaySearchService.SearchAsync({
                DepartingFrom: "MAN",
                TravelingTo: "LTN",
                DepartureDate: DateOnly.Parse("2022/11/10"),
                Duration: 6
            });

            //Assert
            Assert.NotNull(results);
            Assert.Empty(results);
            Assert.IsType<List<HolidayResult>>(results);
        }

        [Fact]
        public void Search_WithEmptySearchInput_ThrowNullArgumentException()
        {
            //Arrange
            var holidaySearchService = new HolidaySearchService();

            //Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                var results = await holidaySearchService.SearchAsync({
                    DepartingFrom: null,
                    TravelingTo: null,
                    DepartureDate: null,
                    Duration: 0
                });
            });
        }

        [Fact]
        public void Search_WithDurationZero_ReturnsEmptyResults()
        {
            //Arrange
            var holidaySearchService = new HolidaySearchService();

            //Act
            var results = await holidaySearchService.SearchAsync({
                DepartingFrom: "MAN",
                TravelingTo: "LTN",
                DepartureDate: DateOnly.Parse("2022/11/10"),
                Duration: 0
            });

            //Assert
            Assert.NotNull(results);
            Assert.Empty(results);
            Assert.IsType<List<HolidayResult>>(results);
        }

    }
}
