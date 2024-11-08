﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Text.Json;

namespace DemoNunitTest
{
    public class Place
    {
        [JsonPropertyName("place name")]
        public string PlaceName { get; set; }
        public string State { get; set; }
        public string StateAbreviation {  get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

    }
}
