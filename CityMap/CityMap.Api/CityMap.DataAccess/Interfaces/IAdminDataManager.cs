using CityMap.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CityMap.DataAccess.Interfaces
{
    public interface IAdminDataManager
    {
        Task<AdminUser> UserLogin(string userName, string password);
        Task<bool> AddUser(AdminUser adminUser);
        Task<bool> UpdateUser(AdminUser adminUser);
        Task<List<AdminUser>> GetUsers();
    }
}
