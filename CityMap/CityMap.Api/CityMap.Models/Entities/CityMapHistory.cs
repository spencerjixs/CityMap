using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CityMap.Models.Entities
{
    public class CityMapHistory
    {
        [Key]
        public Guid CityMapHistoryId { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public double Temperature { get; set; }
        public string TimeZone { get; set; }
        public double Elevation { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime RequestDateTime { get; set; }
        public bool ResponseSuccess { get; set; }       
    }
}
