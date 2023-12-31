using System.Collections;
using CSVFilters.Services.Interfaces;
using EmployeeAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CSVFilters.Controllers;

[ApiController]
[Route("/api/csv")]
public class CsvController : ControllerBase
{
    private readonly ICsvService _service;
    private readonly IMemoryCache _cache;

    public CsvController(ICsvService service, IMemoryCache cache)
    {
        _service = service;
        _cache = cache;
    }

    [HttpPost, DisableRequestSizeLimit]
    [Route("fileupload/{fileName}")]
    public IActionResult Upload(string fileName)
    {
        try
        {
            var file = Request.Form.Files[0];
            if (file.Length <= 0)
            {
                return BadRequest();
            }
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var filePath = Path.Combine(pathToSave, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return Ok(new { fileName });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex}");
        }
    }

    [HttpGet("processfile/{filename}")]
    public async Task<IEnumerable> ProcessFile(string fileName)
    {
        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var filePath = Path.Combine(pathToSave, fileName);
        var records = await _service.ProcessFile(filePath);
        Response.AddPaginationHeader(records);
        return records;
    }

    [HttpGet("properties")]
    public FilterPropertiesList getProperties()
    {
        Type type = _cache.Get<Type>("type");
        FilterPropertiesList? prop = PropertyHelper.GetAllProperties(type);
        return prop;
    }

    [HttpPost("filter")]
    public async Task<PagedList> GetFilterRecords(
        FilterRequest request,
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize
    )
    {
        var records = await _service.GetFilterRecords(request, pageNumber, pageSize);
        Response.AddPaginationHeader(records);
        return records;
    }

    [HttpGet("searching/{searchProperty}")]
    public async Task<IEnumerable> GetSearchedValues(
        string searchProperty,
        [FromQuery] string searchString = null
    )
    {
        return await _service.GetSearchedValues(searchProperty, searchString);
    }
}
