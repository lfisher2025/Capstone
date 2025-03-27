using Lab1.Pages.Data_Classes;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Lab1.Pages.DB;

namespace Lab1.Pages.Messaging
{
    public class NewMessageModel : PageModel
    {
        [BindProperty]
        public int RecipientID { get; set; }

        [BindProperty]
        public string Title { get; set; }

        [BindProperty]
        public string Content { get; set; }

        public List<User> Users { get; set; } = new List<User>();



        public void OnGet()
        {
            //Loading in users for drop down select for recipient
            SqlDataReader UsersReader = DBClass.GetUsers();

            while (UsersReader.Read())
            {
                Users.Add(new Data_Classes.User
                {
                    UserID = UsersReader.GetInt32(0),
                    FirstName = UsersReader.GetString(1),
                    LastName = UsersReader.GetString(2),
                });
            }
            DBClass.Lab1DBConnection.Close();
        }

        public IActionResult OnPost()
        {
            int senderID = Convert.ToInt32(HttpContext.Session.GetString("UserID")); // Get sender from session
            if (senderID == 0)
            {
                ViewData["MessageStatus"] = "Error: You must be logged in to send messages.";
                return Page();
            }

            if (!ModelState.IsValid)
            {
                ViewData["MessageStatus"] = "All fields are required.";
                return Page();
            }

            int rowsAffected = DBClass.NewMessage(senderID,RecipientID,Title,Content);
            if (rowsAffected > 0)
            {
                ViewData["MessageStatus"] = "Message sent successfully!";
                return RedirectToPage("/Messaging/Inbox"); // Redirect to inbox after sending
            }
            else
            {
                ViewData["MessageStatus"] = "Error sending message.";
            }

            DBClass.Lab1DBConnection.Close();
            return Page();

        }

        public IActionResult OnPostPopulateHandler()
        {
            ModelState.Clear();

            RecipientID = 1;
            Title = "Test";
            Content = "This is a test message to display functionality.";

            return Page();
        }
    }
}
