using System;
using System.Collections.Generic;
using System.Text;

namespace CityMap.Models.Contracts
{
    public partial class ElevationResponse
    {
        public List<Elevation> results { get; set; }
        public string status { get; set; }
    }

    public partial class Elevation
    {
        public double elevation { get; set; }
        public Location location { get; set; }
        public double resolution { get; set; }
    }
    public partial class Location
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }
}
