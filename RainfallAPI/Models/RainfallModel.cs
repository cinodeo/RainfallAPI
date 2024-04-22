using Newtonsoft.Json;

public class RainfallReadingResponse
{
    [JsonProperty("@context")]
    public string Context { get; set; }
    public Meta Meta { get; set; }
    public List<RainfallReadingItem> Items { get; set; }
}

public class Meta
{
    public string Publisher { get; set; }
    public string Licence { get; set; }
    public string Documentation { get; set; }
    public string Version { get; set; }
    public string Comment { get; set; }
    public List<string> HasFormat { get; set; }
    public int Limit { get; set; }
}

public class RainfallReadingItem
{
    [JsonProperty("@id")]
    public string Id { get; set; }
    public DateTime DateTime { get; set; }
    public string Measure { get; set; }
    public double Value { get; set; }
}
