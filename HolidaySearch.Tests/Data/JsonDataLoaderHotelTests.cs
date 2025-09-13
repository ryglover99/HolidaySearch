using HolidaySearch.Models;
using HolidaySearch.Services;
using Newtonsoft.Json;

namespace HolidaySearch.Tests.Data
{
    public class JsonDataLoaderHotelTests
    {
        [Fact]
        public async Task Load_ValidHotelJson_ReturnsHotelObjects()
        {
            //Arrange
            var jsonDataLoaderService = new JsonDataLoaderService();

            //Act
            var parsedHotels = await jsonDataLoaderService.LoadAllAsync<Hotel>("Data/hotels.json");

            //Assert
            Assert.NotNull(parsedHotels);
            Assert.NotEmpty(parsedHotels);
            Assert.IsType<List<Hotel>>(parsedHotels);
            Assert.All(parsedHotels, f => Assert.IsType<Hotel>(f));
        }

        [Fact]
        public void Load_EmptyHotelJson_ReturnsEmptyCollectionHotelObjects()
        {
            //Arrange
            var jsonDataLoaderService = new JsonDataLoaderService();
            string emptyHotelJson = "[]";

            //Act
            var parsedHotels = jsonDataLoaderService.LoadAllFromString<Hotel>(emptyHotelJson);

            //Assert
            Assert.NotNull(parsedHotels);
            Assert.IsType<List<Hotel>>(parsedHotels);
            Assert.Empty(parsedHotels);
        }

        [Fact]
        public void Load_InvalidHotelJson_ThrowsException()
        {
            //Arrange
            var jsonDataLoaderService = new JsonDataLoaderService();
            string invalidHotelJson = "[{ not valid }]";

            //Act & Assert
            Assert.Throws<JsonException>(() =>
            {
                jsonDataLoaderService.LoadAllFromString<Hotel>(invalidHotelJson);
            });
        }

    }
}
