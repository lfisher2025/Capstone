using Lab1.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Lab1.Pages.Data_Classes;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace Lab1.Pages.Messaging
{
    public class InboxModel : PageModel 
    {
        public List<Message> Messages { get; set; } = new List<Message>();
        
        public void OnGet()
        {
            string userID = HttpContext.Session.GetString("UserID");
            SqlDataReader MessagesReader = DBClass.GetUserMessages(userID);

            while (MessagesReader.Read())
            {
                Messages.Add(new Message
                {
                    messageID = MessagesReader.GetInt32(0),
                    sender = MessagesReader.GetString(1),
                    recipient = MessagesReader.GetString(2),
                    title = MessagesReader.GetString(3),
                    content = MessagesReader.GetString(4),
                    readStatus = MessagesReader.GetBoolean(5),
                    timestamp = MessagesReader.GetDateTime(6)
                });

            }
            MessagesReader.Close();
            DBClass.Lab1DBConnection.Close();
        }
    }

}


