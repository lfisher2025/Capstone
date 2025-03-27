using System.ComponentModel.DataAnnotations;

namespace Lab1.Pages.Data_Classes
{
    public class Message
    {
        [Required]
        public int messageID { get; set; }
        [Required]
        public string sender { get; set; }
        [Required]
        public string recipient { get; set; }
        [Required]
        public string content { get; set; }
        [Required]
        public string title { get; set; }
        [Required]
        public bool readStatus { get; set; }
        [Required]
        public DateTime timestamp { get; set; }
    }
}
