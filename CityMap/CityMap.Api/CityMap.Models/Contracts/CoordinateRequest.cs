using System;
using System.Collections.Generic;
using System.Text;

namespace CityMap.Models.Contracts
{
    public partial class CoordinateRequest
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public CoordinateRequest(double lat, double lon)
        {
            Latitude = lat;
            Longitude = lon;
        }
    }
}
