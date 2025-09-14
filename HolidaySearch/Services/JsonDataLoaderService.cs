using HolidaySearch.Services.Interfaces;
using Newtonsoft.Json;

namespace HolidaySearch.Services
{
    public class JsonDataLoaderService : IJsonDataLoaderService
    {
        private List<T> DeserializeJsonIntoCollectionOfObjects<T>(string? filePath, string json) where T : class
        {
            if (string.IsNullOrEmpty(json))
                return new List<T>();

            try
            {
                var result = JsonConvert.DeserializeObject<List<T>>(json);

                if (result == null)
                {
                    string locationInfo = filePath != null ? $"while reading '{filePath}'" : string.Empty;
                    throw new JsonException($"Deserialization returned null {locationInfo} for object {typeof(T)}");
                }

                return result;

            }
            catch (JsonException ex)
            {
                string locationInfo = filePath != null ? $"json at '{filePath}'" : string.Empty;
                throw new JsonException($"JSON was invalid while attempting to deserialize {locationInfo} with object {typeof(T)}", ex);
            }
        }

        public async Task<List<T>> LoadAllAsync<T>(string filePath) where T : class
        {
            string fullFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);

            if (!File.Exists(fullFilePath))
                throw new FileNotFoundException($"File not found: {fullFilePath}");

            string json = await File.ReadAllTextAsync(fullFilePath);

            return DeserializeJsonIntoCollectionOfObjects<T>(filePath, json) ?? new List<T>();

        }

        public List<T> LoadAllFromString<T>(string json) where T : class
        {
            return DeserializeJsonIntoCollectionOfObjects<T>(filePath: null, json) ?? new List<T>();
        }
    }
}
