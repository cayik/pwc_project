using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using System.Threading;

namespace pwc_project.Models
{
    public class WoWDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public DbSet<Monster> Monsters { get; set; }

        public DbSet<Monster> Users { get; set; }

        public DbSet<Monster> Characters { get; set; }

        public DbSet<Monster> Categories { get; set; }

        public DbSet<Monster> Equipments { get; set; }

        public DbSet<Monster> EquipmentStat { get; set; }

        public DbSet<Monster> EquipmentSlot { get; set; }





        public WoWDbContext(IConfiguration configuration)
        {
            _configuration = configuration;

        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = $"Server={_configuration["SERVER_IP"]};" +
                                        $"User ID={_configuration["USER_NAME"]};" +
                                        $"Password={_configuration["USER_PASSWORD"]};" +
                                        $"Database={_configuration["DATABASE_NAME"]}";
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }


    }
}