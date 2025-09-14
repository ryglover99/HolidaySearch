using HolidaySearch.Models;

namespace HolidaySearch.Services.Interfaces
{
    public interface IHolidaySearchService
    {
        Task<HolidaySearchResponse> SearchAsync(HolidaySearchRequest request);
    }
}
