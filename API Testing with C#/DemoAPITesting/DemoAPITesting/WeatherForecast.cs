
using Newtonsoft.Json;

namespace DemoAPITesting
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; } = DateTime.Now;

        // JSON.NET renaming property (it will show in the printed result)
        //[JsonProperty("temperature_c")]
        public int TemperatureC { get; set; } = 30;

        // JSON.NET property will be ignored (it will not show in the printed result)
        //[JsonIgnore]
        public string Summary { get; set; } = "Hot summer day";
    }
}
