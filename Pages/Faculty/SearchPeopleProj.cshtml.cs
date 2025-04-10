using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Lab1.Pages.DB;
using Lab1.Pages.Data_Classes;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Lab1.Pages.Faculty
{
    public class PeopleModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public List<SearchResult> SearchResults { get; set; } = new();

        public User SelectedUser { get; set; }

        public List<User> ProjectUsers { get; set; } = new();

        public void OnGet(int? id, string? type)
        {
            // Search Bar
            if (!string.IsNullOrEmpty(SearchString))
            {
                SqlDataReader reader = DBClass.SearchPeopleAndProjects(SearchString);

                while (reader.Read())
                {
                    SearchResults.Add(new SearchResult
                    {
                        ID = Convert.ToInt32(reader["ID"]),
                        Name = reader["Name"].ToString(),
                        Type = reader["Type"].ToString()
                    });
                }

                DBClass.Lab1DBConnection.Close();
            }

            // If user clicks on a name or project
            if (id.HasValue && !string.IsNullOrEmpty(type))
            {
                // If user was clicked, load contact info
                if (type == "User")
                {
                    SqlDataReader reader = DBClass.GetUserByID(id.Value);

                    if (reader.Read())
                    {
                        SelectedUser = new User
                        {
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Email = reader["Email"].ToString(),
                            Department = reader["Department"].ToString()
                        };
                    }

                    DBClass.Lab1DBConnection.Close();
                }

                // If project was clicked, load user list
                else if (type == "Project")
                {
                    SqlDataReader reader = DBClass.GetUsersByProjectID(id.Value);

                    while (reader.Read())
                    {
                        ProjectUsers.Add(new User
                        {
                            UserID = Convert.ToInt32(reader["UserID"]),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Email = reader["Email"].ToString()
                        });
                    }

                    DBClass.Lab1DBConnection.Close();
                }
            }
        }
    }
}
