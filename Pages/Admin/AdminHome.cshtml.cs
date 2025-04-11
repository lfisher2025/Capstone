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
        
        public Dictionary<string, List<string>> ProjectStatusData { get; set; } = new();
        public Dictionary<string, List<string>> GrantStatusData { get; set; } = new();
        public Dictionary<string, List<string>> PartnerStatusData { get; set; } = new();
        public List<Project> Projects { get; set; } = new();
        public List<BusinessPartner> businessPartners { get; set; } = new();
        public string UserID { get; set; }
        public List<Grant> GrantInfo { get; set; } = new();
        [BindProperty]
        public String UserName { get; set; }
        [BindProperty]
        public int inProgressCount { get; set; } = 0;
        [BindProperty]
        public int GrantCount { get; set; } = 0;
        [BindProperty]
        public int ActiveCount { get; set; } = 0;

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

            foreach(var project in Projects)
            {
                if (project.ProjectStatus == "In Progress")
                {
                   inProgressCount++;
                }
            }

            //Scrape comppletion status for each project and make a dictionary for JSON object in view
            ProjectStatusData = Projects
            .GroupBy(p => p.ProjectStatus)
            .ToDictionary(
             grp => grp.Key,
             grp => grp.Select(p => p.ProjectName).ToList()
                         );

            ViewData["ProjectStatusCounts"] = new Dictionary<string, int>
                {
                    { "Not Started", ProjectStatusData.ContainsKey("Not Started") ? ProjectStatusData["Not Started"].Count : 0 },
                    { "In Progress", ProjectStatusData.ContainsKey("In Progress") ? ProjectStatusData["In Progress"].Count : 0 },
                    { "Completed", ProjectStatusData.ContainsKey("Completed") ? ProjectStatusData["Completed"].Count : 0 }
                };
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

            foreach(var grant in GrantInfo)
                            {
                              GrantCount++;
                            }

            //Scrape grant type from grants for chart
            GrantStatusData = GrantInfo
                .GroupBy(g => g.Type)
                .ToDictionary(
                 grp => grp.Key,
                 grp => grp.Select(g => g.GrantName).ToList()
                 );

            ViewData["GrantFundingCounts"] = new Dictionary<string, int>
                {
                    { "Federal Grants", GrantStatusData.ContainsKey("Federal") ? GrantStatusData["Federal"].Count : 0 },
                    { "State Grants", GrantStatusData.ContainsKey("State") ? GrantStatusData["State"].Count : 0 },
                    { "Internal Funding", GrantStatusData.ContainsKey("Internal") ? GrantStatusData["Internal"].Count : 0 },
                    { "Private Foundations", GrantStatusData.ContainsKey("Private") ? GrantStatusData["Private"].Count : 0 }
                };
        }

        public void GetPartners()
        {
            //Call DB to get partners and read them into a list
            SqlDataReader partnerReader = DBClass.GetPartners();

            while (partnerReader.Read())
            {
                businessPartners.Add(new BusinessPartner
                {
                    PartnerID = partnerReader.GetInt32(partnerReader.GetOrdinal("PartnerID")),
                    PartnerName = partnerReader.GetString(partnerReader.GetOrdinal("PartnerName")),
                    PartnerOrg = partnerReader.GetString(partnerReader.GetOrdinal("PartnerOrg")),
                    PartnerContact = partnerReader.GetString(partnerReader.GetOrdinal("PartnerContact")),
                    PartnerType = partnerReader.GetString(partnerReader.GetOrdinal("PartnerType")),
                    Sector = partnerReader.GetString(partnerReader.GetOrdinal("Sector")),
                    Status = partnerReader.GetString(partnerReader.GetOrdinal("Status")),
                    LastInteractionDate = partnerReader.IsDBNull(partnerReader.GetOrdinal("LastInteractionDate"))
                              ? null
                              : partnerReader.GetDateTime(partnerReader.GetOrdinal("LastInteractionDate")),
                    GrantID = partnerReader.GetInt32(partnerReader.GetOrdinal("GrantID")),
                    RepresentativeID = partnerReader.GetInt32(partnerReader.GetOrdinal("RepresentativeID"))
                });
            }
            DBClass.Lab1DBConnection.Close();

            foreach (var partner in businessPartners)
            {
                if (partner.Status == "Active")
                {
                    ActiveCount++;
                }
            }

            //Scrape partner status for chart
            PartnerStatusData = businessPartners
                .GroupBy(p => p.Status)
                .ToDictionary(
                grp => grp.Key,
                grp => grp.Select(p => p.PartnerName).ToList()
                );
            ViewData["PartnerStageCounts"] = new Dictionary<string, int>
                {
                    { "Prospect", PartnerStatusData.ContainsKey("Prospect") ? PartnerStatusData["Prospect"].Count : 0 },
                    { "Initial Contact", PartnerStatusData.ContainsKey("Initial Contact") ? PartnerStatusData["Initial Contact"].Count : 0 },
                    { "Negotiation", PartnerStatusData.ContainsKey("Negotiation") ? PartnerStatusData["Negotiation"].Count : 0 },
                    { "Active", PartnerStatusData.ContainsKey("Active") ? PartnerStatusData["Active"].Count : 0 },
                    { "Completed", PartnerStatusData.ContainsKey("Completed") ? PartnerStatusData["Completed"].Count : 0 }
                };
        }


        
    }
}
