using System.ComponentModel.DataAnnotations;

namespace Lab1.Pages.Data_Classes
{
    public class Task
    {
        public int TaskID { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public DateTime Deadline { get; set; }
        public string Status { get; set; }
        public int AssignedTo { get; set; }
        public int ProjectID { get; set; }
    }

}
