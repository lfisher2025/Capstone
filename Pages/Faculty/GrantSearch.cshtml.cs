using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;
using Lab1.Pages.Data_Classes;
using Lab1.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
namespace Lab1.Pages.Faculty
{
    public class GrantSearchModel : PageModel
    {


        [BindProperty(SupportsGet = true)]
        public string SearchName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchCategory { get; set; }

        [BindProperty(SupportsGet = true)]
        public double? SearchAmount { get; set; }

        [BindProperty]
        public Grant tempGrant { get; set; } = new Grant();
        public List<Grant> GrantInfo { get; set; }
        public GrantSearchModel()
        {
            GrantInfo = new List<Grant>();
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


            SqlDataReader ViewGrants = DBClass.ViewAllGrants();
            while (ViewGrants.Read())
            {
                GrantInfo.Add(new Grant
                {
                    GrantID = ViewGrants["GrantID"] != DBNull.Value ? Convert.ToInt32(ViewGrants["GrantID"]) : 0,
                    GrantName = ViewGrants["GrantName"] != DBNull.Value ? ViewGrants["GrantName"].ToString() : string.Empty,
                    FundingAgency = ViewGrants["FundingAgency"] != DBNull.Value ? ViewGrants["FundingAgency"].ToString() : string.Empty,
                    Deadline = ViewGrants["Deadline"] != DBNull.Value ? Convert.ToDateTime(ViewGrants["Deadline"]) : DateTime.MinValue,
                    ProposalID = ViewGrants["ProposalID"] != DBNull.Value ? Convert.ToInt32(ViewGrants["ProposalID"]) : 0,
                    FundingAmount = ViewGrants["FundingAmount"] != DBNull.Value ? Convert.ToDecimal(ViewGrants["FundingAmount"]) : 0,
                    Type = ViewGrants["Type"] != DBNull.Value ? ViewGrants["Type"].ToString() : string.Empty,
                    GrantDescription = ViewGrants["GrantDescription"] != DBNull.Value ? ViewGrants["GrantDescription"].ToString() : string.Empty
                });
            }
            DBClass.Lab1DBConnection.Close();

            return Page();

        }


        public IActionResult OnPost()
        {


            GrantInfo.Clear();
            SqlDataReader SearchRead = DBClass.GrantSearch(tempGrant);


            while (SearchRead.Read())
            {
                GrantInfo.Add(new Grant
                {
                    GrantID = SearchRead["GrantID"] != DBNull.Value ? Convert.ToInt32(SearchRead["GrantID"]) : 0,
                    GrantName = SearchRead["GrantName"] != DBNull.Value ? SearchRead["GrantName"].ToString() : string.Empty,
                    FundingAgency = SearchRead["FundingAgency"] != DBNull.Value ? SearchRead["FundingAgency"].ToString() : string.Empty,
                    Deadline = SearchRead["Deadline"] != DBNull.Value ? Convert.ToDateTime(SearchRead["Deadline"]) : DateTime.MinValue,
                    ProposalID = SearchRead["ProposalID"] != DBNull.Value ? Convert.ToInt32(SearchRead["ProposalID"]) : 0,
                    FundingAmount = SearchRead["FundingAmount"] != DBNull.Value ? Convert.ToDecimal(SearchRead["FundingAmount"]) : 0,
                    Type = SearchRead["Type"] != DBNull.Value ? SearchRead["Type"].ToString() : string.Empty,
                    GrantDescription = SearchRead["GrantDescription"] != DBNull.Value ? SearchRead["GrantDescription"].ToString() : string.Empty

                });
            }
            DBClass.Lab1DBConnection.Close();
            return Page();
        }
        public void OnPostArchive()
        {
            DBClass.InsertArchive();
        }
    }
}
