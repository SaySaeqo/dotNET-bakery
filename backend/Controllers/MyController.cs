using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using dotNet_bakery.Repo; 
using dotNet_bakery.Models;
using System.Text;
using Microsoft.AspNetCore.Cors;

namespace dotNet_bakery.Controllers;


[EnableCors("CorsPolicy")]
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

    [Route("test")]
    public async Task<string> Test()
    {
        List<string> types = new List<string> { "temperature", "weight", "gas_density", "humidity" };

        // add 256 test data for 16 ids
        for (int i = 0; i < 16; i++)
        {
            for (int j = 0; j < 256; j++)
            {
                DataModel dataModel = new DataModel
                {
                    id = i,
                    date = DateTime.Now.AddDays(-j),
                    value = j,
                    type = types[i % 4]
                };
                await _dataRepository.CreateAsync(dataModel);
            }
        }

        string typesString = string.Join(", ", types);

        return "Created 256 test data for 16 ids. Types are: " + typesString + "\n";
    }

    [Route("clean")]
    public async Task<string> Clean()
    {
        List<DataModel> dataModels = await _dataRepository.GetAsync();
        int count = 0;
        foreach (var dataModel in dataModels)
        {
            await _dataRepository.DeleteAsync(dataModel.dataId);
            count++;
        }

        return "Deleted " + count + " entries.\n";
    }


    [HttpPost]
    [Route("json")]
    [Produces("application/json")]
    public async Task<List<DataModel>> GetJson([FromBody] RequestBody? body)
    {
        return await GetFilteredAndSorted(body);
    }

    [HttpPost]
    [Route("csv")]
    [Produces("text/csv")]
    public async Task<ContentResult> GetCsv([FromBody] RequestBody? body)
    {
        List<DataModel> dataModels = await GetFilteredAndSorted(body);

        var csv = new StringBuilder();

        foreach (var item in dataModels)
        {
            csv.AppendLine($"{item.id},{item.date},{item.value},{item.type}");
        }

        return Content(csv.ToString(), "text/csv");
    }

    [HttpGet]
    [Route("json/{id}")]
    [Produces("application/json")]
    public async Task<AverageModel> GetAverage(int id)
    {
        RequestBody body = new RequestBody
        {
            ids = new List<int> {id},
            sortBy = "date",
        };
        List<DataModel> dataModels = await GetFilteredAndSorted(body);

        int last = dataModels.First().value;
        // get first 100 values from list
        dataModels = dataModels.Take(100).ToList();
        // calculate average
        double average = dataModels.Average(s => s.value);
        return new AverageModel
        {
            last = last,
            average = average
        };
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

    [HttpPut]
    [Route("json/{id}")]
    public async void PutJson(int id, [FromBody] DataModel dataModel)
    {
    
        dataModel.id = id;
        await _dataRepository.CreateAsync(dataModel);
    }
}
