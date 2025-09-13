using Newtonsoft.Json;

namespace HolidaySearch.Tests.Data
{
    public class JsonDataLoaderHotelTests
    {
        [Fact]
        public async Task Load_ValidHotelJson_ReturnsHotelObjects()
        {
            //Arrange
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "hotels.json");
            string flightJson = await File.ReadAllTextAsync(filePath);

            //Act
            var parsedHotels = JsonConvert.DeserializeObject<List<Hotel>>(flightJson);

            //Assert
            Assert.NotNull(parsedHotels);
            Assert.NotEmpty(parsedHotels);
            Assert.IsType<List<Hotel>>(parsedHotels);
            Assert.All(parsedHotels, f => Assert.IsType<Hotel>(f));
        }

        [Fact]
        public async Task Load_EmptyHotelJson_ReturnsEmptyCollectionHotelObjects()
        {
            //Arrange
            string emptyHotelJson = "[]";

            //Act
            var parsedHotels = JsonConvert.DeserializeObject<List<Hotel>>(emptyHotelJson);

            //Assert
            Assert.NotNull(parsedHotels);
            Assert.IsType<List<Hotel>>(parsedHotels);
            Assert.Empty(parsedHotels);
        }

        [Fact]
        public void Load_InvalidHotelJson_ThrowsException()
        {
            //Arrange
            string invalidHotelJson = "[{ not valid }]";

            //Act & Assert
            Assert.Throws<JsonException>(() =>
            {
                JsonConvert.DeserializeObject<List<Hotel>>(invalidHotelJson);
            });
        }

    }
}
