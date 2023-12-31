using System.Collections;
using EmployeeAPI.Extensions;

namespace CSVFilters.Services.Interfaces;

public interface ICsvService
{
    Task<PagedList> ProcessFile(string fileName);
    Task<PagedList> GetFilterRecords(FilterRequest request, int pageNumber, int pageSize);
    Task<IEnumerable> GetSearchedValues(string searchProperty, string searchString);
}
