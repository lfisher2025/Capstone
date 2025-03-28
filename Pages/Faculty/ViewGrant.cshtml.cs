using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;
using Lab1.Pages.Data_Classes;
using Lab1.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab1.Pages.Faculty
{
    public class ViewGrantModel : PageModel
    {
      

        [BindProperty(SupportsGet = true)]
        public string SearchName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchCategory { get; set; }

        [BindProperty(SupportsGet = true)]
        public double? SearchAmount { get; set; }

        public List<Grant> GrantSearch { get; set; } = new();
        public List<Grant> GrantInfo { get; set; }
        public ViewGrantModel()
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
            if (UserType != "1" && UserType != "2")
            { return RedirectToPage("/Shared/UnauthorizedResource"); }


            SqlDataReader ViewGrants = DBClass.ViewAllGrants();
            while (ViewGrants.Read())
            {
                GrantInfo.Add(new Grant
                {
                    GrantID = Int32.Parse(ViewGrants["GrantID"].ToString()),
                    Name = ViewGrants["Name"].ToString(),
                    Category = ViewGrants["Category"].ToString(),
                    GrantStatus = ViewGrants["GrantStatus"].ToString(),
                    Amount = Convert.ToDouble(ViewGrants["Amount"].ToString())
                });
            }
            DBClass.Lab1DBConnection.Close();

            return Page();

        }


        public IActionResult OnPost()
        {
            Grant searchGrant = new Grant();
            searchGrant.Name = SearchName;
            searchGrant.Category = SearchCategory;
            searchGrant.Amount = SearchAmount;

            GrantInfo.Clear();
            SqlDataReader SearchRead = DBClass.GrantSearch(searchGrant);


            while (SearchRead.Read())
            {
                GrantInfo.Add(new Grant
                {
                    GrantID = Int32.Parse(SearchRead["GrantID"].ToString()),
                    Name = SearchRead["Name"].ToString(),
                    Category = SearchRead["Category"].ToString(),
                    GrantStatus = SearchRead["GrantStatus"].ToString(),
                    Amount = Convert.ToDouble(SearchRead["Amount"])
                });
            }
            DBClass.Lab1DBConnection.Close();
            return Page();
        }

    
        
    }
}
