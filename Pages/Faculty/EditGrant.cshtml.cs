using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Lab1.Pages.DataClasses;
using Lab1.Pages.DB;
using System.Data.SqlClient;
using Lab1.Pages.Data_Classes;

namespace Lab1.Pages.Faculty
{
    public class EditGrantModel : PageModel
    {
        [BindProperty]
        public Grant GrantToUpdate { get; set; } = new Grant();
        public EditGrantModel()
        {
            GrantToUpdate = new Grant();
        }
        public IActionResult OnGet(int grantid)
        {
            string UserID = HttpContext.Session.GetString("UserID");
            string UserType = HttpContext.Session.GetString("UserType");


            if (string.IsNullOrEmpty(UserID))
            {
                return RedirectToPage("/HashedLogin/HashedLogin"); // Redirect if not currently logged in
            }
            if (UserType != "2" && UserType != "1")
            { return RedirectToPage("/Shared/UnauthorizedResource"); }

            SqlDataReader singleGrant = DBClass.SingleGrantReader(grantid);

            while (singleGrant.Read())
            {
                GrantToUpdate = new Grant
                {
                    GrantID = grantid,
                    GrantName = singleGrant["GrantName"] != DBNull.Value ? singleGrant["GrantName"].ToString() : string.Empty,
                    FundingAgency = singleGrant["FundingAgency"] != DBNull.Value ? singleGrant["FundingAgency"].ToString() : string.Empty,
                    Deadline = singleGrant["Deadline"] != DBNull.Value ? Convert.ToDateTime(singleGrant["Deadline"]) : DateTime.MinValue,
                    ProposalID = singleGrant["ProposalID"] != DBNull.Value ? Convert.ToInt32(singleGrant["ProposalID"]) : 0,
                    FundingAmount = singleGrant["FundingAmount"] != DBNull.Value ? Convert.ToDecimal(singleGrant["FundingAmount"]) : 0,
                    Type = singleGrant["Type"] != DBNull.Value ? singleGrant["Type"].ToString() : string.Empty,
                    GrantDescription = singleGrant["GrantDescription"] != DBNull.Value ? singleGrant["GrantDescription"].ToString() : string.Empty,

                };
            }
            DBClass.Lab1DBConnection.Close();

            return Page();
        }

        public IActionResult OnPost()
        {
            DBClass.EditGrant(GrantToUpdate);
            DBClass.Lab1DBConnection.Close();
            return RedirectToPage("/Faculty/ViewGrant");
        }
    }
}
