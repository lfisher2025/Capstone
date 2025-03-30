using System.ComponentModel.DataAnnotations;

namespace Lab1.Pages.Data_Classes
{
    public class BusinessPartner
    {
        public int PartnerID { get; set; }
        public string PartnerName { get; set; }
        public string PartnerOrg { get; set; }
        public string PartnerContact { get; set; }
        public string PartnerType { get; set; }
        public string Sector { get; set; }
        public string Status { get; set; }
        public DateTime LastInteractionDate { get; set; }
        public int GrantID { get; set; }
        public int RepresentativeID { get; set; }
    }
}

