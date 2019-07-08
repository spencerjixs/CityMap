using System;
using System.Collections.Generic;
using System.Text;

namespace CityMap.Models.Contracts
{
    public partial class OpenWeatherResponse
    {
        public int id { get; set; }
        public Coord coord { get; set; }
        public Main main { get; set; }
        public string name { get; set; }
        public long dt { get; set; }
    }

    public partial class Main
    {
        public double temp { get; set; }
        public double pressure { get; set; }
        public double humidity { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }

    }
    public partial class Coord
    {
        public double lat { get; set; }
        public double lon { get; set; }
    }
}
