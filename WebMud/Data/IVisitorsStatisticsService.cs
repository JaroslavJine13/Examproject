using MudBlazor;

namespace WebMud.Data
{
    public interface IVisitorsStatisticsService
    {

        Task<int> NumberOfVisitors();
        Task<int> NumberOfVisitorsToday();
        Task<List<DateTime>> GetVisits();
        Task<List<double>> GetArrayOfVisitsPerRange(DateRange dateRange);
    }
}