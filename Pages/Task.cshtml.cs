using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Lab1.Pages.Data_Classes;
using Lab1.Pages.DB;
using System.Data.SqlClient;

namespace Lab1.Pages
{
    public class TaskModel : PageModel
    {
        public List<TaskWork> TaskInfo { get; set; } = new List<TaskWork>();
        
        public TaskModel()
        {
            TaskInfo = new List<TaskWork>();
        }
        public void OnGet()
        {
            SqlDataReader taskReader = DBClass.TaskReader(); // Replace with the actual user ID
            while (taskReader.Read())
            {
                TaskWork task = new TaskWork
                { 
                    TaskName = taskReader.GetString(1),
                    TaskDescription = taskReader.GetString(2),
                    Deadline = taskReader.GetDateTime(3),
                    Status = taskReader.GetString(4),
                    AssignedTo = taskReader.GetInt32(5),
                    ProjectId = taskReader.IsDBNull(6) ? (int?)null : taskReader.GetInt32(6),
                    GrantApplicationID = taskReader.IsDBNull(7) ? (int?)null : taskReader.GetInt32(7)
                };
                TaskInfo.Add(task);
            }
            DBClass.Lab1DBConnection.Close();
        }
    }
}
