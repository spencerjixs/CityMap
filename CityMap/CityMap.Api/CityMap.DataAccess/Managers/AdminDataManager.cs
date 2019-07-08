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
    public class AdminDataManager : IAdminDataManager
    {
        private readonly CityMapSqlDbContext _db;
        public AdminDataManager(CityMapSqlDbContext db)
        {
            _db = db;
            AddTestUser();
        }
        public async Task<AdminUser> UserLogin(string userName, string password)
        {
            var result = await _db.adminUser.Where(x => x.UserName == userName && x.Password == password).ToListAsync<AdminUser>();
            return result.FirstOrDefault<AdminUser>();
        }

        public async Task<List<AdminUser>> GetUsers()
        {
            var result = await _db.adminUser.Where(x=>x.UserName!="").ToListAsync<AdminUser>();
            return result;
        }

        public async Task<bool> AddUser(AdminUser adminUser)
        {
            _db.adminUser.Add(adminUser);
            var result = await _db.SaveChangesAsync() != 0;
            return result;
        }
        public async Task<bool> UpdateUser(AdminUser adminUser)
        {
            _db.adminUser.Update(adminUser);
            var result = await _db.SaveChangesAsync() != 0;
            return result;
        }

        private void AddTestUser()
        {
            if (_db.adminUser.Find("admin") == null)
            {
                _db.adminUser.Add(
                    new AdminUser()
                    {
                        UserName = "admin",
                        Password = "admin",
                        CreatedDateTime = DateTime.UtcNow,
                        CreatedBy = "Admin",
                        UpdatedDateTime = DateTime.UtcNow,
                        UpdatedBy = "Admin"
                    });
                _db.SaveChanges();
            }
        }

    }
}

