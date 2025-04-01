namespace Lab1.Pages.Data_Classes
{
    public class User
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int UserTypeID { get; set; }
        public string Department { get; set; }
        public DateTime DateJoined { get; set; }
        public int AccessLevel { get; set; }
    }
}
