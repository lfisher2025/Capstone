using Lab1.Pages.Data_Classes;
using Lab1.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace Lab1.Pages.Calendar
{
    public class CalendarModel : PageModel
    {
        // Change Days to List<List<Day>> to hold multiple weeks of days
        public List<List<Day>> Days { get; set; } = new List<List<Day>>();

        [BindProperty]
        public List<UserTask> TaskList { get; set; } = new List<UserTask>(); 

        public void OnGet()
        {
            DateTime currentDate = DateTime.Now;
            var firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            int startingDay = (int)firstDayOfMonth.DayOfWeek;
            int daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);

            Days.Clear(); // Clear any previous days

            int dayCounter = 1;

            // Prepare days for calendar
            for (int i = 0; i < 6; i++) // 6 weeks max in a month
            {
                var week = new List<Day>(); // Each week is a List of Day objects
                for (int j = 0; j < 7; j++)
                {
                    if (i == 0 && j < startingDay)
                    {
                        week.Add(new Day()); // Empty day for the leading blank cells
                    }
                    else if (dayCounter <= daysInMonth)
                    {
                        week.Add(new Day { DayOfMonth = dayCounter }); // Add day to the week
                        dayCounter++;
                    }
                    else
                    {
                        week.Add(new Day()); // Empty day for the trailing blank cells
                    }
                }
                Days.Add(week); // Add the week to the overall calendar
            }

            //Get current userID for task query
            string temp = HttpContext.Session.GetString("UserID");
            int UserID =  int.Parse(temp);

            //Gather User tasks to put on the calendar
            SqlDataReader TaskReader = DBClass.GetUserTasks(UserID);

            //read tasks into the list

            while (TaskReader.Read())
            {
                UserTask taskobj = new UserTask
                {
                    TaskID = TaskReader.GetInt32(TaskReader.GetOrdinal("TaskID")),
                    TaskName = TaskReader.GetString(TaskReader.GetOrdinal("TaskName")),
                    TaskDescription = TaskReader.IsDBNull(TaskReader.GetOrdinal("TaskDescription"))
           ? null
           : TaskReader.GetString(TaskReader.GetOrdinal("TaskDescription")),
                    Deadline = TaskReader.GetDateTime(TaskReader.GetOrdinal("Deadline")),
                    Status = TaskReader.IsDBNull(TaskReader.GetOrdinal("Status"))
           ? null
           : TaskReader.GetString(TaskReader.GetOrdinal("Status")),
                    AssignedTo = TaskReader.GetInt32(TaskReader.GetOrdinal("AssignedTo")),
                    ProjectID = TaskReader.GetInt32(TaskReader.GetOrdinal("ProjectID"))
                };

                TaskList.Add(taskobj);
            }
            DBClass.Lab1DBConnection.Close();

        }

        
    }

    public class Day
    {
        public int DayOfMonth { get; set; }
    }
}