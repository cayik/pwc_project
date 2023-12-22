using System.ComponentModel.DataAnnotations;

namespace pwc_project.Models.database
{
    public class User
    {
        [Key]
        public int userID { get; set; }

        public string userName { get; set; }

        public string userEmail { get; set; }

        public string userPassword { get; set; }

        public string userRole { get; set; }
    }
}
