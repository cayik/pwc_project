using System.ComponentModel.DataAnnotations;

namespace pwc_project.Models.database
{
    public class Monster
    {
        [Key]
        public int monsterID { get; set; }
        public string monsterName { get; set; }

    }
}