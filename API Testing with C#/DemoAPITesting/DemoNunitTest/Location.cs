using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Text.Json;

namespace DemoNunitTest
{
    public class Location
    {
        [JsonPropertyName("post code")]
        public string postCode { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("country abreviation")]
        public string CountryAbreviation { get; set; }

        [JsonPropertyName("places")]
        public List <Place> Places { get; set; }


    }
}
