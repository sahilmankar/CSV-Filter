using System.Collections;
using System.Reflection;
using CSVFilters.Services.Interfaces;
using CSVFilters.Helpers;
using EmployeeAPI.Extensions;
using System.Linq.Dynamic.Core;
using Microsoft.Extensions.Caching.Memory;

namespace CSVFilters.Services;

public class CsvService : ICsvService
{
    private readonly IMemoryCache _cache;

    public CsvService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public async Task<PagedList> ProcessFile(string fileName)
    {
        string code = CsvToClass.CSharpClassCodeFromCsvFile(
            fileName,
            out string className,
            out string[] data
        );
        Assembly assembly = AssemblyGenrator.CompileCode(code, className);

        if (assembly == null)
        {
            return null;
        }
        var type = AssemblyGenrator.GetTypeOfClass(assembly, className);
        var properties = AssemblyGenrator.GetProperties(type);
        _cache.Set("assembly", assembly);
        _cache.Set("type", type);
        if (properties == null)
        {
            
            return null;
        }
        var li = CsvReaderHelper.PopulateListWithData(data, properties, type);
        _cache.Set("data", li);
        return await PagedList.ToPagedList(li.AsQueryable());
    }

    public async Task<PagedList> GetFilterRecords(
        FilterRequest request,
        int pageNumber,
        int pageSize
    )
    {
        IEnumerable data = _cache.Get<IEnumerable>("data");
        var query = data.AsQueryable().ApplyFilters(request);
        return await PagedList.ToPagedList(query, pageNumber, pageSize);
    }

    public async Task<IEnumerable> GetSearchedValues(string searchProperty, string searchString)
    {
        IEnumerable data = _cache.Get<IEnumerable>("data");
        var query = data.AsQueryable().ApplySearching(searchProperty, searchString);
        return await query.ToDynamicListAsync();
    }
}
