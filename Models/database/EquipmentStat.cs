using System.ComponentModel.DataAnnotations;

namespace pwc_project.Models.database
{
    public class EquipmentStat
    {
        [Key]
        public int equipmentID { get; set; }

        public string statName { get; set; }

        public int statValue { get; set; }
    }
}
