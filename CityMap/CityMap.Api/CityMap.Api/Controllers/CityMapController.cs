using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityMap.Api.Interfaces;
using CityMap.Models.Contracts;
using CityMap.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityMap.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CityMapController : ControllerBase
    {
        private readonly ICityMapManager _cityMapManager;

        public CityMapController(ICityMapManager cityMapManager)
        {
            this._cityMapManager = cityMapManager;
        }

        [HttpGet("api/CityMap/GetOpenWeather/{zipCode}")]
        public async Task<IActionResult> GetOpenWeather(string zipCode)
        {
            try
            {
                var result = await _cityMapManager.GetOpenWeatherByZipCode(zipCode);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch(Exception ex)
            {
                var logMsg = string.Format("Controller: GetOpenWeather, Error: {0}", ex.Message);
                //Todo: add log4net.
                return NotFound();
            }
        }

        [HttpPost("api/CityMap/GetTimeZoneByCoordinates")]
        public async Task<IActionResult> GetTimeZoneByCoordinates([FromBody] CoordinateRequest coord)
        {
            try
            {
                var result = await _cityMapManager.GetTimeZoneByCoordinates(coord);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                var logMsg = string.Format("Controller: GetTimeZoneByCoordinates, Error: {0}", ex.Message);
                //Todo: add log4net.
                return NotFound();
            }
        }

        [HttpPost("api/CityMap/GetElevationByCoordinates")]
        public async Task<IActionResult> GetElevationByCoordinates([FromBody] CoordinateRequest coord)
        {
            try
            {
                var result = await _cityMapManager.GetElevationByCoordinates(coord);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                var logMsg = string.Format("Controller: GetElevationByCoordinates, Error: {0}", ex.Message);
                //Todo: add log4net.
                return NotFound();
            }
        }

        [AllowAnonymous]
        [HttpGet("api/CityMap/GetCityMapByZipCode/{zipCode}")]
        public async Task<IActionResult> GetCityMapByZipCode(string zipCode)
        {
            try
            {
                var result = await _cityMapManager.GetCityMapByZipCode(zipCode);
                var cityMapHistoy = new CityMapHistory
                {
                    ZipCode = zipCode,
                    CityMapHistoryId = new Guid(),
                    RequestDateTime = DateTime.UtcNow,
                    ResponseSuccess = false
                };
                if (result == null)
                {
                    await _cityMapManager.AddCityMapHistory(cityMapHistoy);
                    return NotFound();
                }
                cityMapHistoy.City = result.City;
                cityMapHistoy.TimeZone = result.TimeZone;
                cityMapHistoy.Temperature = result.Temperature;
                cityMapHistoy.Elevation = result.Elevation;
                cityMapHistoy.Latitude = result.Latitude;
                cityMapHistoy.Longitude = result.Longitude;
                cityMapHistoy.ResponseSuccess = true;
                await _cityMapManager.AddCityMapHistory(cityMapHistoy);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var logMsg = string.Format("Controller: GetCityMapByZipCode, Error: {0}", ex.Message);
                //Todo: add log4net.
                return NotFound();
            }
        }



        [HttpGet]
        [Route("api/CityMap/GetCityMapHistoryByZipCode/{zipCode}")]
        public async Task<IActionResult> GetCityMapHistoryByZipCode(string zipCode)
        {
            try
            {
                var result = await _cityMapManager.GetCityMapHistoryByZipCode(zipCode);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                var logMsg = string.Format("Controller: GetCityMapHistoryByZipCode, Error: {0}", ex.Message);
                //Todo: add log4net.
                return NotFound();
            }
        }

        [HttpGet]
        [Route("api/CityMap/GetCityMapHistoryDaily")]
        public async Task<IActionResult> GetCityMapHistoryDaily()
        {
            try
            {
                var result = await _cityMapManager.GetCityMapHistoryDaily();
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                var logMsg = string.Format("Controller: GetCityMapHistoryDaily, Error: {0}", ex.Message);
                //Todo: add log4net.
                return NotFound();
            }
        }
    }
}