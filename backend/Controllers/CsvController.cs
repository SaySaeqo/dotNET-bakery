using Microsoft.AspNetCore.Mvc;

namespace w2routing.Controllers;

[Route("csv")]
public class CsvController : ControllerBase
{
    public String Index()
    {
        return "todo csv";
    }
}