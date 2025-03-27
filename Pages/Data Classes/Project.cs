using System.ComponentModel.DataAnnotations;

namespace Lab1.Pages.Data_Classes
{
    public class Project
    {
        [Required]
        public int projectID { get; set; }
        [Required]
        public int grantID { get; set; }
        [Required]
        public int employeeID { get; set; }
        [Required]
        public int adminID { get; set; }
        [Required]
        public int facultyID { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public DateTime? DueDate { get; set; }
        [Required]
        public DateTime? submissionDate { get; set; }
        [Required]
        public bool? CompleteStatus { get; set; }
        public string note { get; set; }
        public List<string> Notes { get; set; } = new List<string>();
        
    }
}
