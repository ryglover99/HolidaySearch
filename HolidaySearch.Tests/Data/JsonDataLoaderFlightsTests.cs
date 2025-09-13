using HolidaySearch.Models;
using HolidaySearch.Services;
using Newtonsoft.Json;

namespace HolidaySearch.Tests.Data
{
    public class JsonDataLoaderFlightsTests
    {
        [Fact]
        public async Task Load_ValidFlightJson_ReturnsFlightObjects()
        {
            //Arrange
            var jsonDataLoaderService = new JsonDataLoaderService();

            //Act
            var parsedFlights = await jsonDataLoaderService.LoadAllAsync<Flight>("Data/flights.json");

            //Assert
            Assert.NotNull(parsedFlights);
            Assert.NotEmpty(parsedFlights);
            Assert.IsType<List<Flight>>(parsedFlights);
            Assert.All(parsedFlights, f => Assert.IsType<Flight>(f));
        }

        [Fact]
        public void Load_EmptyFlightJson_ReturnsEmptyCollectionFlightObjects()
        {
            //Arrange
            var jsonDataLoaderService = new JsonDataLoaderService();
            string emptyFlightJson = "[]";

            //Act
            var parsedFlights = jsonDataLoaderService.LoadAllFromString<Flight>(emptyFlightJson);

            //Assert
            Assert.NotNull(parsedFlights);
            Assert.IsType<List<Flight>>(parsedFlights);
            Assert.Empty(parsedFlights);
        }

        [Fact]
        public void Load_InvalidFlightJson_ThrowsException()
        {
            //Arrange
            var jsonDataLoaderService = new JsonDataLoaderService();
            string invalidFlightJson = "[{ not valid }]";

            //Act & Assert
            Assert.Throws<JsonException>(() =>
            {
                jsonDataLoaderService.LoadAllFromString<Flight>(invalidFlightJson);
            });
        }

    }
}
