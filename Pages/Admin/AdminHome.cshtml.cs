using System.Data.SqlClient;
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
        public string UserID { get; set; }
        public List<Grant> GrantInfo { get; set;  }

        public AdminHomeModel()
        {
            GrantInfo = new List<Grant>();
        }

        public IActionResult OnGet()
        {
            string UserID = HttpContext.Session.GetString("username");
            string UserType = HttpContext.Session.GetString("UserType");

            if (string.IsNullOrEmpty(UserID))
            {
                return RedirectToPage("/HashedLogin/HashedLogin"); // Redirect if not logged in
            }
            if (UserType != "1")
            { return RedirectToPage("/Shared/UnauthorizedResource"); }

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

            SqlDataReader projectReader = DBClass.ViewAllProjects();

            while (projectReader.Read())
            {
                var row = new Dictionary<string, object>();
                for (int i = 0; i < projectReader.FieldCount; i++)
                {
                    row[projectReader.GetName(i)] = projectReader[i];
                }
                TableData.Add(row);
            }

            DBClass.Lab1DBConnection.Close();

            return Page();
        }


 


        
    }
}
