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

        [BindProperty]
        public Event NewEvent { get; set; }
        [BindProperty]
        public List<int> SelectedUserIDs { get; set; } = new List<int>();

        public List<Event> UserEvents { get; set; } = new List<Event>();

        public Dictionary<int, string> UserList { get; set; }

        public int CurrentMonth { get; set; }
        public int CurrentYear { get; set; }

        public void OnGet()
        {
            DateTime currentDate = DateTime.Now;
            //var firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            //var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            CurrentMonth = currentDate.Month;
            CurrentYear = currentDate.Year;

            //int startingDay = (int)firstDayOfMonth.DayOfWeek;
            //int daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);

            //Days.Clear(); // Clear any previous days

            //int dayCounter = 1;

            //// Prepare days for calendar
            //for (int i = 0; i < 6; i++) // 6 weeks max in a month
            //{
            //    var week = new List<Day>(); // Each week is a List of Day objects
            //    for (int j = 0; j < 7; j++)
            //    {
            //        if (i == 0 && j < startingDay)
            //        {
            //            week.Add(new Day()); // Empty day for the leading blank cells
            //        }
            //        else if (dayCounter <= daysInMonth)
            //        {
            //            week.Add(new Day { DayOfMonth = dayCounter }); // Add day to the week
            //            dayCounter++;
            //        }
            //        else
            //        {
            //            week.Add(new Day()); // Empty day for the trailing blank cells
            //        }
            //    }
            //    Days.Add(week); // Add the week to the overall calendar
            //}

         GenerateCalendarGrid(CurrentYear, CurrentMonth); //Render the calendar, same code just move to a method so it can me re-rendered after OnPost

        //Get current userID for queries
        string temp = HttpContext.Session.GetString("UserID");
            int UserID = int.Parse(temp);

            GetUserTasks(UserID);
            GetUserList();
            GetUserEvents(UserID);
        }

        public IActionResult OnPost()
        {
            int newEventID = DBClass.AddEvent(NewEvent);

            for (int i = 0; i < SelectedUserIDs.Count(); i++)
            {
                int UserID = SelectedUserIDs[i];
                DBClass.AddEventUsers(newEventID, UserID);

            }

            DBClass.Lab1DBConnection.Close();
            // Rebuild calendar and other data after post
            DateTime now = DateTime.Now;
            CurrentMonth = now.Month;
            CurrentYear = now.Year;
            GenerateCalendarGrid(CurrentYear, CurrentMonth);

            string temp = HttpContext.Session.GetString("UserID");
            int userID = int.Parse(temp);
            GetUserTasks(userID);
            GetUserList();
            GetUserEvents(userID);
            return Page();
        }


        private void GenerateCalendarGrid(int year, int month)
        {
            Days.Clear();
            var firstDayOfMonth = new DateTime(year, month, 1);
            int startingDay = (int)firstDayOfMonth.DayOfWeek;
            int daysInMonth = DateTime.DaysInMonth(year, month);

            Days = new List<List<Day>>();
            int dayCounter = 1;

            for (int i = 0; i < 6; i++)
            {
                var week = new List<Day>();
                for (int j = 0; j < 7; j++)
                {
                    if (i == 0 && j < startingDay)
                    {
                        week.Add(new Day());
                    }
                    else if (dayCounter <= daysInMonth)
                    {
                        week.Add(new Day { DayOfMonth = dayCounter });
                        dayCounter++;
                    }
                    else
                    {
                        week.Add(new Day());
                    }
                }
                Days.Add(week);
            }
        }

        public void GetUserTasks(int UserID)
        {
            //Gather User tasks to put on the calendar
            SqlDataReader TaskReader = DBClass.GetUserTasks(UserID);

            //read tasks into the list

            while (TaskReader.Read())
            {
                UserTask taskobj = new UserTask
                {
                    TaskID = TaskReader.GetInt32(TaskReader.GetOrdinal("TaskID")),
                    TaskName = TaskReader.GetString(TaskReader.GetOrdinal("TaskName")),
                    TaskDescription = TaskReader.IsDBNull(TaskReader.GetOrdinal("TaskDescription")) ? null : TaskReader.GetString(TaskReader.GetOrdinal("TaskDescription")),
                    Deadline = TaskReader.GetDateTime(TaskReader.GetOrdinal("Deadline")),
                    Status = TaskReader.IsDBNull(TaskReader.GetOrdinal("Status")) ? null: TaskReader.GetString(TaskReader.GetOrdinal("Status")),
                    AssignedTo = TaskReader.GetInt32(TaskReader.GetOrdinal("AssignedTo")),
                    ProjectID = TaskReader.GetInt32(TaskReader.GetOrdinal("ProjectID"))
                };

                TaskList.Add(taskobj);
            }
            DBClass.Lab1DBConnection.Close();
        }

        public void GetUserList()
        {
            SqlDataReader userReader = DBClass.GetUsers();

            while (userReader.Read())
            {
                int userId = userReader["UserID"] != DBNull.Value ? Convert.ToInt32(userReader["UserID"]) : 0;
                string fullName = userReader["FullName"] != DBNull.Value ? userReader["FullName"].ToString() : "";

                if (UserList == null)
                {
                    UserList = new Dictionary<int, string>();
                }

                if (!UserList.ContainsKey(userId))
                {
                    UserList.Add(userId, fullName);
                }
            }
            DBClass.Lab1DBConnection.Close();

        }

        public void GetUserEvents(int UserID)
        {
            SqlDataReader EventReader = DBClass.GetUserEvents(UserID);

            while (EventReader.Read())
            {
                UserEvents.Add(new Event
                {
                    EventID = EventReader.GetInt32(EventReader.GetOrdinal("EventID")),
                    Title = EventReader.GetString(EventReader.GetOrdinal("Title")),
                    EventDateTime = EventReader.GetDateTime(EventReader.GetOrdinal("ScheduledDate")),
                    Description = EventReader.IsDBNull(EventReader.GetOrdinal("Description")) ? null : EventReader.GetString(EventReader.GetOrdinal("Description")),
                });
            }
            DBClass.Lab1DBConnection.Close();
        }

        public class Day
        {
            public int DayOfMonth { get; set; }
        }
    }
}
