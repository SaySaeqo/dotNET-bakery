using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using dotNet_bakery.Repo; 
using dotNet_bakery.Models;

namespace dotNet_bakery.Controllers;


[Route("")]
public class MyController : ControllerBase
{
    private readonly ILogger<MyController> _logger;
    private DataRepository _dataRepository;

    public MyController(ILogger<MyController> logger, DataRepository dataRepository)
    {
        _logger = logger;
        _dataRepository = dataRepository;
    }

    [Route("")]
    public string Index()
    {
        return "Hello World!";
    }


    [HttpGet]
    [Route("json")]
    [Produces("application/json")]
    public async Task<List<DataModel>> GetJson([FromBody] RequestBody? body)
    {
        return await GetFilteredAndSorted(body);
    }

    [HttpGet]
    [Route("csv")]
    [Produces("ext/csv")]
    public async Task<List<DataModel>> GetCsv([FromBody] RequestBody? body)
    {
        return await GetFilteredAndSorted(body);
    }


    private async Task<List<DataModel>> GetFilteredAndSorted(RequestBody? body)
    {
        List<DataModel> sensors = await _dataRepository.GetAsync();
        
        if (body == null)
        {
            return sensors;
        }
        // apply filters from body
        if (body.fromDateTime != null)
        {
            sensors = sensors.Where(s => s.date >= body.fromDateTime).ToList();
        }
        if (body.toDateTime != null)
        {
            sensors = sensors.Where(s => s.date <= body.toDateTime).ToList();
        }
        if (body.types != null)
        {
            sensors = sensors.Where(s => body.types.Contains(s.type)).ToList();
        }
        if (body.ids != null)
        {
            sensors = sensors.Where(s => body.ids.Contains(s.id)).ToList();
        }
        // sort by field in sortBy
        if (body.sortBy != null)
        {
            sensors = sensors.OrderBy(s => s.GetType().GetProperty(body.sortBy)?.GetValue(s)).ToList();
        }

        return sensors;
    }
}
