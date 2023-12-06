using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using dotNet_bakery.Repo; 

namespace w2routing.Controllers;

[Route("json")]
public class JsonController : ControllerBase
{
    private readonly ILogger<JsonController> _logger;
    private DataRepository _dataRepository;

    public JsonController(ILogger<JsonController> logger, DataRepository dataRepository)
    {
        _logger = logger;
        _dataRepository = dataRepository;
    }

    [Route("")]
    public String Index()
    {
        return "todo json";
    }

    [Route("abra")]
    public int abra()
    {
        return 2;
    }
}
