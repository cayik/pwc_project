using pwc_project.Models.database;
using System.ComponentModel.DataAnnotations;

namespace pwc_project.Models.responses
{
    public class EquipmentResponse
    {
        [Key]
        public int equipmentID { get; set; }

        public string equipmentName { get; set; }


        public int categoryID { get; set; }

        public string categoryName { get; set; }

        public List<EquipmentStat>? equipmentStat { get; set; }
    }
}
