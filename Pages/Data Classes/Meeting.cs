using System.ComponentModel.DataAnnotations;

namespace Lab1.Pages.Data_Classes
{
    public class Meeting
    {
        [Required]
        public int meetingId { get; set; }
        public int adminID { get; set; }
        public int representativeID { get; set; }
        public int minutes { get; set; }
    }
}
