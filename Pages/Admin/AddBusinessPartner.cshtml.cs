using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Lab1.Pages.Data_Classes;
using Lab1.Pages.DB;
using System.Data.SqlClient;

namespace Lab1.Pages.Admin
{
    public class AddBusinessPartnerModel : PageModel
    {
        [BindProperty]
        public BusinessPartner NewPartner { get; set; } = new BusinessPartner();


        [BindProperty]
        public int StatusSelect {  get; set; }
       
        public IList<User> Representitives { get; set; } = new List<User>();

        public String Status { get; set; }

        [BindProperty]
        public int RepresentativeID { get; set; }

        //Eventually, the OnGet method will need to select the representatives from the database for a user to select one to attach to the business
        public IActionResult OnGet()
        {
            string Username = HttpContext.Session.GetString("username");
            string UserType = HttpContext.Session.GetString("UserType");

            if (string.IsNullOrEmpty(Username))
            {
                return RedirectToPage("/HashedLogin/HashedLogin"); // Redirect if not logged in
            }
            if (UserType != "1")
            { return RedirectToPage("/Shared/UnauthorizedResource"); }

            else {
                SqlDataReader RepReader = DBClass.ViewAllRepresenatives();

                while (RepReader.Read())
                {
                    Representitives.Add(new Data_Classes.User
                    {
                        UserID = int.Parse(RepReader["userID"].ToString()),
                        FirstName = RepReader["firstName"].ToString(),
                        LastName = RepReader["lastName"].ToString()
                    });
                }
                DBClass.Lab1DBConnection.Close();

                
                return Page(); 
            }
        }

        public void OnPost()
        {
            if (StatusSelect == 1)
            {
                Status = "Prospect";

            }
            else if (StatusSelect == 2)
            {
                Status = "Initial Contact";
            }
            else if (StatusSelect == 3)
            {
                Status = "In Negotiaion";
            }
            else if (StatusSelect == 4)
            {
                Status = "Memo Signed";
            }
            else if (StatusSelect == 5)
            {
                Status = "Active Partner";
            }
            else if (StatusSelect == 6)
            {
                Status = "Completed";
            }
            else if (StatusSelect == 7)
            {
                Status = "On Hold";
            }
            
            NewPartner.Status = Status;

            DBClass.AddBusinessPartner(NewPartner);

            DBClass.Lab1DBConnection.Close();  
        }

        public IActionResult OnPostPopulateHandler() //Needs to be updated to reflect new partner structure
        {
            //ModelState.Clear();

            //CompanyName = "JMU Company";
            //RepresentativeID = 4;
            //Status = "Prospect";

            return Page();

        }

        public IActionResult OnPostClearHandler()
        {
            ModelState.Clear();
            return Page();
        }
    }
}
