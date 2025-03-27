using System.ComponentModel.DataAnnotations;

namespace Lab1.Pages.Data_Classes
{
    public class BusinessPartner
    {
        [Required]
        public int businessID { get; set; }
        [Required]
        public String name { get; set; }
        [Required]
        public int representativeID { get; set; }
        [Required]
        public String status { get; set; }
    }
}
