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
                    businessID = Int32.Parse(partnerReader["businessID"].ToString()),
                    name = partnerReader["name"].ToString(),
                    representativeID = Int32.Parse(partnerReader["representativeID"].ToString()),
                    status = partnerReader["status"].ToString()
                });
            }
            DBClass.Lab1DBConnection.Close();

            return Page();

        }
    }
}
