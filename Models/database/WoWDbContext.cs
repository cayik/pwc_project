using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using System.Threading;

namespace pwc_project.Models.database
{
    public class WoWDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public DbSet<Monster> Monsters { get; set; }

        public DbSet<MonsterDrop> MonsterDrop { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Character> Characters { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Equipment> Equipments { get; set; }

        public DbSet<EquipmentStat> EquipmentStat { get; set; }

        public DbSet<EquipmentSlot> EquipmentSlot { get; set; }


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