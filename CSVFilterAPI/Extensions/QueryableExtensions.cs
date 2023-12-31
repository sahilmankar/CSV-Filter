using System.Linq.Dynamic.Core;
using System.Text;

namespace EmployeeAPI.Extensions;

public static class QueryableExtensions
{
    public static IQueryable ApplyFilters(this IQueryable query, FilterRequest request)
    {
        query = query.ApplyEqualFilters(request.EqualFilters);
        query = query.ApplyDateRangeFilter(request.DateRangeFilters);
        query = query.ApplyRangeFilter(request.RangeFilters);
        query = query.ApplySorting(request.SortBy, request.SortAscending);
        return query;
    }

    public static IQueryable ApplyEqualFilters(
        this IQueryable query,
        List<EqualFilter>? equalFilters
    )
    {
        if (equalFilters != null && equalFilters.Any())
        {
            foreach (var property in equalFilters)
            {
                var stringBuilder = new StringBuilder();
                string propertyName = property.PropertyName;
                var propertyValues = property.PropertyValues;
                if (propertyValues != null)
                {
                    foreach (string pvalue in propertyValues)
                    {
                        if (!string.IsNullOrEmpty(pvalue))
                        {
                            stringBuilder.Append($"{propertyName} = \"{pvalue}\" ");
                            stringBuilder.Append(" OR ");
                        }
                    }

                    if (stringBuilder.Length > 0)
                    {
                        stringBuilder.Length -= 4; 
                        string filterString = stringBuilder.ToString();
                        query = query.Where(filterString);
                    }
                }
            }
        }

        return query;
    }

    public static IQueryable ApplyDateRangeFilter(
        this IQueryable query,
        List<DateRangeFilter>? dateRangeFilters
    )
    {
        if (dateRangeFilters != null && dateRangeFilters.Any())
        {
            foreach (var filterOptions in dateRangeFilters)
            {
                bool hasFromDate = DateTime.TryParse(filterOptions.FromDate, out DateTime fromDate);

                if (hasFromDate)
                {
                    query = query.Where($"{filterOptions.PropertyName} >= @0", fromDate);
                }

                bool hasToDate = DateTime.TryParse(filterOptions.ToDate, out DateTime toDate);

                if (hasToDate)
                {
                    query = query.Where($"{filterOptions.PropertyName} <= @0", toDate);
                }
            }
        }

        return query;
    }

    public static IQueryable ApplyRangeFilter(
        this IQueryable query,
        List<RangeFilter>? rangeFilters
    )
    {
        if (rangeFilters != null && rangeFilters.Any())
        {
            foreach (var property in rangeFilters)
            {
                string propertyName = property.PropertyName;
                double minValue = property.MinValue ?? default;
                double maxValue = property.MaxValue ?? default;

                if (minValue > maxValue && maxValue != default)
                {
                    (maxValue, minValue) = (minValue, maxValue); // int temp = minValue;
                    // minValue = maxValue;
                    // maxValue = temp;
                }

                if (minValue != default)
                {
                    query = query.Where($"{propertyName} >= @0", minValue);
                }
                if (maxValue != default)
                {
                    query = query.Where($"{propertyName} <= @0", maxValue);
                }
            }
        }

        return query;
    }

    public static IQueryable ApplySorting(this IQueryable query, string? sortBy, bool sortAscending)
    {
        if (!string.IsNullOrEmpty(sortBy))
        {
            query = query.OrderBy($"{sortBy} {(sortAscending ? "ascending" : "descending")}");
        }
        return query;
    }

    public static IQueryable ApplySearching(
        this IQueryable query,
        string searchProperty,
        string? searchString
    )
    {
        if (!string.IsNullOrEmpty(searchString))
        {
            searchString = searchString.Trim();
            Console.WriteLine(searchString);
            searchProperty = searchProperty.Trim();
            Console.WriteLine(searchProperty);
            query = query.Where($"{searchProperty}.Contains(@0,StringComparison.OrdinalIgnoreCase)", searchString).Select(searchProperty);
            return query.Take(5);
        }
       return query.Select(searchProperty).Take(5);
    }
}
