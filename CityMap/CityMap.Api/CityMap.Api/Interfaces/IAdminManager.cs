using CityMap.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityMap.Api.Interfaces
{
    public interface IAdminManager
    {
        Task<AdminUser> UserLogin(string userName, string password);
        Task<bool> AddUser(AdminUser adminUser);
        Task<bool> UpdateUser(AdminUser adminUser);
        Task<List<AdminUser>> GetUsers();
    }
}
