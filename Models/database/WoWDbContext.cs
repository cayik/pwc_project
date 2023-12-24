using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;

namespace pwc_project.Models.database
{
    // Eine Klasse, die die Datenbankkontext-Klasse für die Anwendung definiert.
    public class WoWDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        // Die DbSet-Eigenschaften für verschiedene Entitätsklassen.
        public DbSet<Monster> Monsters { get; set; }
        public DbSet<MonsterDrop> MonsterDrop { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<EquipmentStat> EquipmentStat { get; set; }
        public DbSet<EquipmentSlot> EquipmentSlot { get; set; }

        // Konstruktor für den WoWDbContext, der die Konfiguration übergeben bekommt.
        public WoWDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Überschreibt die Methode OnConfiguring, um die Datenbankverbindung zu konfigurieren.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Die Verbindungszeichenfolge aus der Konfiguration zusammenstellen.
            string connectionString = $"Server={_configuration["SERVER_IP"]};" +
                                        $"User ID={_configuration["USER_NAME"]};" +
                                        $"Password={_configuration["USER_PASSWORD"]};" +
                                        $"Database={_configuration["DATABASE_NAME"]}";

            // Die Verbindung mit der MySQL-Datenbank konfigurieren.
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }
}
