using ApiRestCRUDCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ApiRestCRUDCore.Data.EF
{
    public class ApiRestCRUDCoreDataContext : DbContext
    {

        private readonly IConfiguration _config;

        public ApiRestCRUDCoreDataContext(IConfiguration config) { _config = config; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config.GetConnectionString("ApiConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<eUser>().HasKey(p => new { p.Id });
        }

        public DbSet<eUser> User { get; set; }
    }
}
