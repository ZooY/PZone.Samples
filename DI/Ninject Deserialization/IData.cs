using Newtonsoft.Json;


namespace PZone.Samples
{
    internal interface IData
    {
        string StringValue { get; set; }
        int IntValue { get; set; }

        [JsonIgnore]
        Service Service { get; set; }
    }
}