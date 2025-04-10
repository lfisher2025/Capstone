using System.Data.SqlClient;
using System.Numerics;
using System.Reflection.PortableExecutable;
using Lab1.Pages.Data_Classes;
using Lab1.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab1.Pages.Admin
{
    public class AdminHomeModel : PageModel
    {
        
        public List<Dictionary<string, object>> TableData { get; set; } = new();
        public Dictionary<string, List<string>> ProjectStatusData { get; set; } = new();
        public List<Project> Projects { get; set; } = new();
        public string UserID { get; set; }
        public List<Grant> GrantInfo { get; set; } = new();
        [BindProperty]
        public String UserName { get; set; }

        public IActionResult OnGet()
        {
            string UserID = HttpContext.Session.GetString("UserID");
            string UserType = HttpContext.Session.GetString("UserType");

            int numUserID = int.Parse(UserID);
            UserName = DBClass.GetUserName(numUserID); //Get current users name for display on page

            if (string.IsNullOrEmpty(UserID))
            {
                return RedirectToPage("/HashedLogin/HashedLogin"); // Redirect if not logged in
            }
            if (UserType != "1")
            { return RedirectToPage("/Shared/UnauthorizedResource"); } //Redirect if current user isn't an Admin (Director)

            GetGrants(); //Load grants from DB for display
            GetPartners(); //Load partner info from DB for display
            GetProjects(); // Load project information from DB for display


            return Page();
        }


        public void GetProjects()
        {
            //Read projects into a list for further processing
            SqlDataReader projectReader = DBClass.ViewAllProjects();

            while (projectReader.Read())
            {
                var project = new Project
                {
                    ProjectName = projectReader["ProjectName"].ToString(),
                    FundingAmount = Convert.ToDecimal(projectReader["FundingAmount"]),
                    EndDate = Convert.ToDateTime(projectReader["EndDate"]),
                    ProjectStatus = projectReader["ProjectStatus"].ToString()
                };

                Projects.Add(project);
            }

            DBClass.Lab1DBConnection.Close();

            //Scrape comppletion status for each project and make a dictionary for JSON object for view
        }

        public void GetGrants()
        {
            SqlDataReader grantReader = DBClass.ViewAllGrants();
            while (grantReader.Read())
            {
                GrantInfo.Add(new Grant
                {
                    GrantName = grantReader["GrantName"] != DBNull.Value ? grantReader["GrantName"].ToString() : string.Empty,
                    FundingAgency = grantReader["FundingAgency"] != DBNull.Value ? grantReader["FundingAgency"].ToString() : string.Empty,
                    Deadline = grantReader["Deadline"] != DBNull.Value ? Convert.ToDateTime(grantReader["Deadline"]) : DateTime.MinValue,
                    ProposalID = grantReader["ProposalID"] != DBNull.Value ? Convert.ToInt32(grantReader["ProposalID"]) : 0,
                    FundingAmount = grantReader["FundingAmount"] != DBNull.Value ? Convert.ToDecimal(grantReader["FundingAmount"]) : 0,
                    Type = grantReader["Type"] != DBNull.Value ? grantReader["Type"].ToString() : string.Empty,
                    GrantDescription = grantReader["GrantDescription"] != DBNull.Value ? grantReader["GrantDescription"].ToString() : string.Empty

                });
            }
            DBClass.Lab1DBConnection.Close();
        }

        public void GetPartners()
        {

        }


        
    }
}
