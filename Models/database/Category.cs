using System.ComponentModel.DataAnnotations;

namespace pwc_project.Models.database
{
    public class Category
    {
        [Key]
        public int categoryID { get; set; }

        public string categoryName { get; set; }
    }
}
