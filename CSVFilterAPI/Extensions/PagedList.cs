using System.Collections;
using System.Linq.Dynamic.Core;

namespace EmployeeAPI.Extensions;

public class PagedList : List<dynamic>
{
    public int CurrentPage { get; private set; }
    public int TotalPages { get; private set; }
    public int TotalCount { get; private set; }
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;

    public PagedList(List<dynamic> items, int count, int totalpages, int pageNumber)
    {
        TotalCount = count;
        CurrentPage = pageNumber;
        TotalPages = totalpages;
        AddRange(items);
    }

    public static async Task<PagedList> ToPagedList(
        IQueryable query,
        int pageNumber = 1,
        int pageSize = 10
    )
    {
        int count = query.Cast<dynamic>().Count();
     
         if(pageSize==0){
            pageSize=10;
        }
        int totalPages = (int)Math.Ceiling(count / (double)pageSize);
        Console.WriteLine(totalPages);

        if (pageNumber <= 0 || (pageNumber > totalPages && totalPages != 0))
        {
            pageNumber = 1;
        }
       
        var items =  await query.Cast<dynamic>().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToDynamicListAsync();
        return new PagedList(items, count, totalPages, pageNumber);
    }
}
