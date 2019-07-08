using CityMap.Api.Interfaces;
using CityMap.DataAccess.Interfaces;
using CityMap.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityMap.Api.Managers
{
    public class AdminManager : IAdminManager
    {
        private readonly IAdminDataManager _adminDataManager;
        public AdminManager(IAdminDataManager adminDataManager)
        {
            _adminDataManager = adminDataManager;
        }
        public async Task<AdminUser> UserLogin(string userName, string password)
        {
            var resust = await _adminDataManager.UserLogin(userName, password);
            return resust;
        }
        public async Task<bool> AddUser(AdminUser adminUser)
        {
            var resust = await _adminDataManager.AddUser(adminUser);
            return resust;
        }
        public async Task<bool> UpdateUser(AdminUser adminUser)
        {
            var resust = await _adminDataManager.UpdateUser(adminUser);
            return resust;
        }
        public async Task<List<AdminUser>> GetUsers()
        {
            var resust = await _adminDataManager.GetUsers();
            return resust;
        }
    }
}