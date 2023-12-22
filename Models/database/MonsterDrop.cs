using System.ComponentModel.DataAnnotations;

namespace pwc_project.Models.database
{
    public class MonsterDrop
    {
        [Key]
        public int monsterID { get; set; }

        public int equipmentID { get; set; }

        public float dropChance { get; set; }
    }
}
