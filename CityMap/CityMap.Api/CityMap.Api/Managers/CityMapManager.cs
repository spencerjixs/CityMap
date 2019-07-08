using CityMap.Api.Interfaces;
using CityMap.DataAccess.Interfaces;
using CityMap.Models;
using CityMap.Models.Contracts;
using CityMap.Models.Entities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CityMap.Api.Managers
{
    public class CityMapManager : ICityMapManager
    {
        private readonly ICityMapDataManager _cityMapDataManager;
        private readonly IHttpClientFactory _clientFactory;
        private readonly AppSettings _appSettings;

        public CityMapManager(IHttpClientFactory clientFactory, ICityMapDataManager cityMapDataManager,IOptions<AppSettings> appSettings)
        {
            _clientFactory = clientFactory;
            _appSettings = appSettings.Value;
            _cityMapDataManager = cityMapDataManager;
        }

        public async Task<OpenWeatherResponse> GetOpenWeatherByZipCode(string zipCode)
        {
            try
            {
                var requestUrl = string.Format(_appSettings.OpenWeatherApiUrl, zipCode, _appSettings.OpenWeatherApiKey);
                var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

                var client = _clientFactory.CreateClient();

                var response = await client.SendAsync(request);
                if(response.IsSuccessStatusCode)
                {
                    var res = await response.Content.ReadAsAsync<OpenWeatherResponse>();
                    return res;
                }
                return null;
            }
            catch (Exception ex)
            {
                var logMsg = string.Format("Method: GetOpenWeatherByZipCode, Error: {0}", ex.Message);
                //Todo: add log4net.
                throw ex;
            }
        }
        public async Task<TimeZoneResponse> GetTimeZoneByCoordinates(CoordinateRequest coord)
        {
            try
            {
                var requestUrl = string.Format(_appSettings.TimeZoneApiUrl, coord.Latitude, coord.Longitude, (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds, _appSettings.GoogleMapApiKey);
                var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

                var client = _clientFactory.CreateClient();

                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var res = await response.Content.ReadAsAsync<TimeZoneResponse>();
                    return res;
                }
                return null;
            }
            catch (Exception ex)
            {
                var logMsg = string.Format("Method: GetTimeZoneByCoordinates, Error: {0}", ex.Message);
                //Todo: add log4net.
                throw ex;
            }
        }
        public async Task<ElevationResponse> GetElevationByCoordinates(CoordinateRequest coord)
        {
            try
            {
                var requestUrl = string.Format(_appSettings.ElevationApiUrl, coord.Latitude, coord.Longitude,  _appSettings.GoogleMapApiKey);
                var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

                var client = _clientFactory.CreateClient();

                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var res = await response.Content.ReadAsAsync<ElevationResponse>();
                    return res;
                }
                return null;
            }
            catch (Exception ex)
            {
                var logMsg = string.Format("Method: GetElevationByCoordinates, Error: {0}", ex.Message);
                //Todo: add log4net.
                throw ex;
            }
        }


        public async Task<CityMapResponse> GetCityMapByZipCode(string zipCode)
        {
            try
            {
                var openWeather = await GetOpenWeatherByZipCode(zipCode);
                if(openWeather==null || openWeather.coord == null)
                {
                    return null;
                }
                CoordinateRequest coord = new CoordinateRequest(openWeather.coord.lat, openWeather.coord.lon);
                var timeZone = await GetTimeZoneByCoordinates(coord);
                if(timeZone == null)
                {
                    return null;
                }
                var elevation = await GetElevationByCoordinates(coord);
                if (elevation == null)
                {
                    return null;
                }
                var result = new CityMapResponse
                {
                    City = openWeather.name,
                    Temperature = openWeather.main.temp,
                    TimeZone = timeZone.timeZoneName,
                    Elevation = elevation.results.FirstOrDefault().elevation,
                    Latitude = coord.Latitude,
                    Longitude = coord.Longitude,
                    ZipCode = zipCode
                };
                return result;
            }
            catch (Exception ex)
            {
                var logMsg = string.Format("Method: GetCityMapByZipCode, Error: {0}", ex.Message);
                //Todo: add log4net.
                throw ex;
            }
        }


        public async Task<List<CityMapHistory>> GetCityMapHistoryByZipCode(string zipCode)
        {
            try
            {
                var result = await _cityMapDataManager.GetCityMapHistoryByZipCode(zipCode);
                return result;
            }
            catch (Exception ex)
            {
                var logMsg = string.Format("Method: GetCityMapHistoryByZipCode, Error: {0}", ex.Message);
                //Todo: add log4net.
                throw ex;
            }
        }

        public async Task<List<CityMapHistory>> GetCityMapHistoryDaily()
        {
            try
            {
                var result = await _cityMapDataManager.GetCityMapHistoryDaily();
                return result;
            }
            catch (Exception ex)
            {
                var logMsg = string.Format("Method: GetCityMapHistoryDaily, Error: {0}", ex.Message);
                //Todo: add log4net.
                throw ex;
            }
        }

        public async Task<bool> AddCityMapHistory(CityMapHistory cityMapHistory)
        {
            try
            {
                var resust = await _cityMapDataManager.AddCityMapHistory(cityMapHistory);
                return resust;
            }
            catch (Exception ex)
            {
                var logMsg = string.Format("Method: AddCityMapHistory, Error: {0}", ex.Message);
                //Todo: add log4net.
                throw ex;
            }
        }
    }
}
