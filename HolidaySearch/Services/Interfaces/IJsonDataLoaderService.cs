namespace HolidaySearch.Services.Interfaces
{
    public interface IJsonDataLoaderService
    {
        Task<List<T>> LoadAllAsync<T>(string filePath) where T : class;
        List<T> LoadAllFromString<T>(string json) where T : class;
    }
}
