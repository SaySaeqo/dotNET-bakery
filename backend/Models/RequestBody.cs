public class RequestBody
{
    public DateTime? fromDateTime { get; set; }
    public DateTime? toDateTime { get; set; }
    public List<string>? types { get; set; }
    public List<int>? ids { get; set; }
    public string? sortBy { get; set; }

    public override string ToString()
    {
        return "fromDateTime: " + fromDateTime + ", toDateTime: " + toDateTime + ", types: " + types + ", ids: " + ids + ", sortBy: " + sortBy + "\n";
    }
}

/* example json:
{
    "fromDateTime": "2022-01-01T00:00:00",
    "toDateTime": "2022-12-31T23:59:59",
    "types": ["type1", "type2"],
    "ids": [1, 2, 3],
    "sortBy": "value"
}
*/
