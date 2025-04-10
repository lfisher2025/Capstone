using System.Data.SqlClient;
using Lab1.Pages.Data_Classes;
using Lab1.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab1.Pages.Archive
{
    public class ArchiveModel : PageModel
    {
        [BindProperty]
        public ArchiveClass tempArchive { get; set; } = new ArchiveClass();
        [BindProperty]
        public int ArchiveID { get; set; }
        [BindProperty]
        public int GrantID { get; set; }
        [BindProperty]
        public int ProjectID { get; set; }
        [BindProperty]
        public DateTime DateArchived { get; set; }
        public List<ArchiveClass> ArchiveInfo { get; set; }

        public ArchiveModel()
        {

            ArchiveInfo = new List<ArchiveClass>();
        }

        public IActionResult OnGet()
        {
            string UserID = HttpContext.Session.GetString("UserID");
            string UserType = HttpContext.Session.GetString("UserType");

            if (string.IsNullOrEmpty(UserID))
            {
                return RedirectToPage("/HashedLogin/HashedLogin"); // Redirect if not logged in
            }
            if (UserType != "1" && UserType != "2")
            { return RedirectToPage("/Shared/UnauthorizedResource"); }

            SqlDataReader ViewArchive = DBClass.ViewArchive();
            while (ViewArchive.Read())
            {
                ArchiveInfo.Add(new ArchiveClass
                {
                    GrantID = ViewArchive["GrantID"] != DBNull.Value ? Convert.ToInt32(ViewArchive["GrantID"]) : 0,
                    GrantName = ViewArchive["GrantName"] != DBNull.Value ? ViewArchive["GrantName"].ToString() : string.Empty,
                    FundingAgency = ViewArchive["FundingAgency"] != DBNull.Value ? ViewArchive["FundingAgency"].ToString() : string.Empty,
                    SubmissionDate = ViewArchive["SubmissionDate"] != DBNull.Value ? Convert.ToDateTime(ViewArchive["SubmissionDate"]) : DateTime.MinValue,
                    Deadline = ViewArchive["Deadline"] != DBNull.Value ? Convert.ToDateTime(ViewArchive["Deadline"]) : DateTime.MinValue,
                    ProposalID = ViewArchive["ProposalID"] != DBNull.Value ? Convert.ToInt32(ViewArchive["ProposalID"]) : 0,
                    FundingAmount = ViewArchive["FundingAmount"] != DBNull.Value ? Convert.ToDecimal(ViewArchive["FundingAmount"]) : 0,
                    Type = ViewArchive["Type"] != DBNull.Value ? ViewArchive["Type"].ToString() : string.Empty,
                    GrantDescription = ViewArchive["GrantDescription"] != DBNull.Value ? ViewArchive["GrantDescription"].ToString() : string.Empty,
                    UserID = ViewArchive["UserID"] != DBNull.Value ? Convert.ToInt32(ViewArchive["UserID"]) : 0,
                    ProjectID = ViewArchive["ProjectID"] != DBNull.Value ? Convert.ToInt32(ViewArchive["ProjectID"]) : 0,
                    ProjectName = ViewArchive["ProjectName"] != DBNull.Value ? ViewArchive["ProjectName"].ToString() : string.Empty,
                    StartDate = ViewArchive["StartDate"] != DBNull.Value ? Convert.ToDateTime(ViewArchive["StartDate"]) : DateTime.MinValue,
                    EndDate = ViewArchive["EndDate"] != DBNull.Value ? Convert.ToDateTime(ViewArchive["EndDate"]) : DateTime.MinValue,
                    ProjectStatus = ViewArchive["ProjectStatus"] != DBNull.Value ? ViewArchive["ProjectStatus"].ToString() : string.Empty,
                    ProjectLead = ViewArchive["ProjectLead"] != DBNull.Value ? Convert.ToInt32(ViewArchive["ProjectLead"]) : 0,
                    ArchiveDate = ViewArchive["ArchiveDate"] != DBNull.Value ? Convert.ToDateTime(ViewArchive["ArchiveDate"]) : DateTime.MinValue
                });
            }
            DBClass.Lab1DBConnection.Close();
            return Page();
        }
    }
}
