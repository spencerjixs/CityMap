using CityMap.DataAccess.Interfaces;
using CityMap.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CityMap.DataAccess.Managers
{
    public class CityMapDataManager : ICityMapDataManager
    {
        private readonly CityMapSqlDbContext _db;
        public CityMapDataManager(CityMapSqlDbContext db)
        {
            _db = db;
        }
        public async Task<List<CityMapHistory>> GetCityMapHistoryByZipCode(string zipCode)
        {
            var result = await _db.cityMapHistory.Where(x => x.ZipCode == zipCode).OrderByDescending(x => x.RequestDateTime).ToListAsync<CityMapHistory>();
            return result;
        }

        public async Task<List<CityMapHistory>> GetCityMapHistoryDaily()
        {
            var result = await _db.cityMapHistory.Where(x => x.RequestDateTime > DateTime.Now.AddDays(-1)).OrderByDescending(x=>x.RequestDateTime).ToListAsync<CityMapHistory>();
            return result;
        }

        public async Task<bool> AddCityMapHistory(CityMapHistory cityMapHistory)
        {
            _db.cityMapHistory.Add(cityMapHistory);
            var result = await _db.SaveChangesAsync() != 0;
            return result;
        }

    }
}
