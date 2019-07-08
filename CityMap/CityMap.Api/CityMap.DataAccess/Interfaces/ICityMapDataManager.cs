using CityMap.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CityMap.DataAccess.Interfaces
{
    public interface ICityMapDataManager
    {
        Task<List<CityMapHistory>> GetCityMapHistoryByZipCode(string zipCode);
        Task<List<CityMapHistory>> GetCityMapHistoryDaily();

        Task<bool> AddCityMapHistory(CityMapHistory cityMapHistory);
    }
}
