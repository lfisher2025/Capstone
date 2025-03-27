using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab1.Pages.BusinessPartners
{
    public class BusinessPartnerHomeModel : PageModel
    {
        private readonly string connectionString = "Server=localhost;Database=Lab3;Trusted_Connection=True";

        public List<PartnerProjectData> PartnerProjects { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public IActionResult OnGet()
        {
            string Username = HttpContext.Session.GetString("username");
            string UserType = HttpContext.Session.GetString("UserType");

            if (string.IsNullOrEmpty(Username))
            {
                return RedirectToPage("/HashedLogin/HashedLogin"); // Redirect if not logged in
            }

            if (UserType != "4" && UserType != "1")
            { return RedirectToPage("/Shared/UnauthorizedResource"); }

            using (SqlConnection conn = new(connectionString))
            {
                conn.Open();

                string query = @"
                    SELECT bp.name AS BusinessName, p.name AS ProjectName
                    FROM BusinessPartner bp
                    LEFT JOIN Representative r ON bp.representativeID = r.representativeID
                    LEFT JOIN Grants g ON bp.businessID = g.businessID
                    LEFT JOIN Project p ON g.grantID = p.grantID
                    WHERE (@SearchTerm IS NULL OR bp.name LIKE '%' + @SearchTerm + '%')
                    ORDER BY bp.name";

                using (SqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SearchTerm", SearchTerm ?? (object)DBNull.Value);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PartnerProjects.Add(new PartnerProjectData
                            {
                                BusinessName = reader["BusinessName"].ToString(),
                                ProjectName = reader["ProjectName"].ToString()
                            });
                        }
                    }
                }
            }
            return Page();
        }

        public class PartnerProjectData
        {
            public string BusinessName { get; set; }
            public string ProjectName { get; set; }
        }
    }
}
