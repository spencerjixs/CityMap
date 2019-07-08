using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CityMap.Models.Entities
{
    public class CityMapSqlDbContext : DbContext
    {
        public CityMapSqlDbContext(DbContextOptions<CityMapSqlDbContext> options): base(options)
        {
        }
        public DbSet<CityMapHistory> cityMapHistory { get; set; }
        public DbSet<AdminUser> adminUser { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CityMapHistory>().ToTable("CityMapHistory");
            modelBuilder.Entity<AdminUser>().ToTable("AdminUser");
        }
    }
}
