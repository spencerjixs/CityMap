using System;
using System.Collections.Generic;
using System.Text;

namespace CityMap.Models.Contracts
{
    public partial class TimeZoneResponse
    {
        public int dstOffset { get; set; }
        public int rawOffset { get; set; }
        public string status { get; set; }
        public string timeZoneId { get; set; }
        public string timeZoneName { get; set; }
    }
}
