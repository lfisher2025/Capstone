namespace Lab1.Pages.Data_Classes
{
    //This is the data class for a grant that JMU Care chooses to pursue. the other grant class is for initially adding a grant to the system.
    public class GrantApplication
    {
        public int GrantID { get; set; }
        public string GrantName { get; set; }
        public string FundingAgency { get; set; }
        public DateTime Deadline { get; set; }
        public int ProposalID { get; set; }
        public decimal FundingAmount { get; set; }
        public string GrantType { get; set; }
        public string GrantDescription { get; set; }
        public int GrantApplicationID { get; set; }
        public string ApplicationStatus { get; set; }
        public int PrincipleInvestigator { get; set; }
    }
}
