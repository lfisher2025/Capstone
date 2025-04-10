namespace Lab1.Pages.Data_Classes
{
    public class ArchiveClass
    {
        public int ArchiveID { get; set; }
        public int GrantID { get; set; }
        public string GrantName { get; set; }
        public string FundingAgency { get; set; }
        public DateTime Deadline { get; set; }
        public int ProposalID { get; set; }
        public decimal FundingAmount { get; set; }
        public string Type { get; set; }
        public string GrantDescription { get; set; }
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ProjectStatus { get; set; }
        public string ProgressStatus { get; set; }
        public int ProjectLead { get; set; }
        public int UserID { get; set; }
        public DateTime ArchiveDate { get; set; }
        public DateTime SubmissionDate { get; set; }
    }
}
