using System.ComponentModel.DataAnnotations;

namespace pwc_project.Models.database
{
    public class Equipment
    {
        [Key]
        public int equipmentID { get; set; }

        public string equipmentName { get; set; }


        public int categoryID { get; set; }

    }
}
