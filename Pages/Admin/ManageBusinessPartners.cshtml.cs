using Lab1.Pages.Data_Classes;
using Lab1.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Lab1.Pages.Admin
{
    public class ManageBusinessPartnersModel : PageModel
    {
        public List<Dictionary<string, object>> TableData { get; set; } = new();
        public string UserID { get; set; }
        public List<BusinessPartner> PartnerInfo { get; set; }

        public ManageBusinessPartnersModel()
        {
            PartnerInfo = new List<BusinessPartner>();
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


            SqlDataReader partnerReader = DBClass.PartnerReader();
            while (partnerReader.Read())
            {
                PartnerInfo.Add(new BusinessPartner
                {
                    PartnerName = partnerReader["PartnerName"] != DBNull.Value ? partnerReader["PartnerName"].ToString() : string.Empty,
                    PartnerOrg = partnerReader["PartnerOrg"] != DBNull.Value ? partnerReader["PartnerOrg"].ToString() : string.Empty,
                    PartnerContact = partnerReader["PartnerContact"] != DBNull.Value ? partnerReader["PartnerContact"].ToString() : string.Empty,
                    PartnerType = partnerReader["PartnerType"] != DBNull.Value ? partnerReader["PartnerType"].ToString() : string.Empty,
                    Sector = partnerReader["Sector"] != DBNull.Value ? partnerReader["Sector"].ToString() : string.Empty,
                    Status = partnerReader["Status"] != DBNull.Value ? partnerReader["Status"].ToString() : string.Empty,
                    LastInteractionDate = partnerReader["LastInteractionDate"] != DBNull.Value ? Convert.ToDateTime(partnerReader["LastInteractionDate"]) : DateTime.MinValue,
                    GrantID = partnerReader["GrantID"] != DBNull.Value ? Convert.ToInt32(partnerReader["GrantID"]) : 0
                });
            }
            DBClass.Lab1DBConnection.Close();

            return Page();

        }
    }
}
