namespace Lab1.Pages.Data_Classes
{
    public class TaskWork
    {
        public int TaskID { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public DateTime Deadline { get; set; }
        public string Status { get; set; }
        public int AssignedTo { get; set; } // UserID of the user assigned to the task
        public int? ProjectId { get; set; } // ProjectID of the project the task belongs to
        public int? GrantApplicationID { get; set; } // GrantApplicationID of the grant application the task is related to

    }
}
