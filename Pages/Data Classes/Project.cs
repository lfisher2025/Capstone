using System.ComponentModel.DataAnnotations;

namespace Lab1.Pages.Data_Classes
{
    public class Project
    {
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ProjectStatus { get; set; }
        public string ProgressStatus { get; set; }
        public int ProjectLead { get; set; }
        public int UserID { get; set; }
        public int GrantID { get; set; }
        public decimal FundingAmount { get; set; }

    }
}
