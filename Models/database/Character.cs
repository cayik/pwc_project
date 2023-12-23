using System.ComponentModel.DataAnnotations;

namespace pwc_project.Models.database
{
    public class Character
    {
        [Key]
        public int characterID { get; set; }

        public string characterName { get; set; }

        public int? userID { get; set; }
    }
}
