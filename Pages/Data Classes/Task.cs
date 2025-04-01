using System.ComponentModel.DataAnnotations;

namespace Lab1.Pages.Data_Classes
{
    public class Task
    {
        [Required]
        public int taskID { get; set; }
        [Required]
        public int projectID { get; set; }

        public String title { get; set; }

        public DateTime dueDate { get; set; }
        [Required]
        public DateTime assignDate { get; set; }
        [Required]
   
        public int assignedBy { get; set; }

        public int assignedTo { get; set; }

    }
}
