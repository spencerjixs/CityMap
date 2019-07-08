using System;
using System.Collections.Generic;
using System.Text;

namespace CityMap.Models
{
    public class AppSettings
    {
        public string JWTAuthenticationSecret { get; set; }
        public string GoogleMapApiKey { get; set; }
        public string OpenWeatherApiKey { get; set; }
        public string OpenWeatherApiUrl { get; set; }
        public string TimeZoneApiUrl { get; set; }
        public string ElevationApiUrl { get; set; }
        public string AngularClientUrl { get; set; }
    }
}
