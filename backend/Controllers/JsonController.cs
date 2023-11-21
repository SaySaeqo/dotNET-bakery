using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace w2routing.Controllers;

[Route("json")]
public class JsonController : ControllerBase
{
    private readonly ILogger<JsonController> _logger;

    public JsonController(ILogger<JsonController> logger)
    {
        _logger = logger;
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
