using CityMap.Models.Contracts;
using CityMap.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityMap.Api.Interfaces
{
    public interface ICityMapManager
    {
        Task<CityMapResponse> GetCityMapByZipCode(string zipCode);
        Task<OpenWeatherResponse> GetOpenWeatherByZipCode(string zipCode);
        Task<TimeZoneResponse> GetTimeZoneByCoordinates(CoordinateRequest coord);
        Task<ElevationResponse> GetElevationByCoordinates(CoordinateRequest coord);
        Task<List<CityMapHistory>> GetCityMapHistoryByZipCode(string zipCode);
        Task<List<CityMapHistory>> GetCityMapHistoryDaily();
        Task<bool> AddCityMapHistory(CityMapHistory cityMapHistory);

    }
}
