using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace Lab1.Pages.Calendar
{
    public class CalendarModel : PageModel
    {
        // Change Days to List<List<Day>> to hold multiple weeks of days
        public List<List<Day>> Days { get; set; } = new List<List<Day>>();

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
        }


    }

    public class Day
    {
        public int DayOfMonth { get; set; }
    }
}