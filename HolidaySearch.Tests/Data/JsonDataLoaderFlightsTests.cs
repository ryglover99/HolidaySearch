using HolidaySearch.Models;
using Newtonsoft.Json;

namespace HolidaySearch.Tests.Data
{
    public class JsonDataLoaderFlightsTests
    {
        [Fact]
        public async Task Load_ValidFlightJson_ReturnsFlightObjects()
        {
            //Arrange
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/flights.json");
            string flightJson = await File.ReadAllTextAsync(filePath);

            //Act
            var parsedFlights = JsonConvert.DeserializeObject<List<Flight>>(flightJson);

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
            string emptyFlightJson = "[]";

            //Act
            var parsedFlights = JsonConvert.DeserializeObject<List<Flight>>(emptyFlightJson);

            //Assert
            Assert.NotNull(parsedFlights);
            Assert.IsType<List<Flight>>(parsedFlights);
            Assert.Empty(parsedFlights);
        }

        [Fact]
        public void Load_InvalidFlightJson_ThrowsException()
        {
            //Arrange
            string invalidFlightJson = "[{ not valid }]";

            //Act & Assert
            Assert.Throws<JsonReaderException>(() =>
            {
                JsonConvert.DeserializeObject<List<Flight>>(invalidFlightJson);
            });
        }

    }
}
