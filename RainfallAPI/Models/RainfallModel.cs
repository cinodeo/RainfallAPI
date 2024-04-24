using Newtonsoft.Json;

public class RainfallReadingResponse
{
    [JsonProperty("@context")]
    public string Context { get; set; }
    public List<RainfallReadingItem> Items { get; set; }
}


public class RainfallReadingItem
{
    [JsonProperty("@id")]
    public string Id { get; set; }
    public DateTime DateTime { get; set; } // 
    public string Measure { get; set; }
    public double Value { get; set; }
}
public class MeasureResponse
{
    //measure tba
    [JsonProperty("@id")]
    public string Id { get; set; }
    public DateTime DateTime { get; set; } // 
    public string Measure { get; set; }
    public double Value { get; set; }
}