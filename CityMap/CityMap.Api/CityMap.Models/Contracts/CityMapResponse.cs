using System;
using System.Collections.Generic;
using System.Text;

namespace CityMap.Models.Contracts
{
    public partial class CityMapResponse
    {
        public string City { get; set; }
        public double Temperature { get; set; }
        public string TimeZone { get; set; }
        public double Elevation { get; set; }
        public string ZipCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
