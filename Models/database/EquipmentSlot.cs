using System.ComponentModel.DataAnnotations;

namespace pwc_project.Models.database
{
    public class EquipmentSlot
    {
        [Key]
        public int equipmentID { get; set; }

        public int characterID { get; set; }
    }
}
