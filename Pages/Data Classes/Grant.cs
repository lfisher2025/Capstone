﻿namespace Lab1.Pages.Data_Classes
{
    // see comment in GrantApplication Class
    public class Grant
    {
        public int GrantID { get; set; }
        public string GrantName { get; set; }
        public string FundingAgency { get; set; }
        public DateTime Deadline { get; set; }
        public int ProposalID { get; set; }
        public decimal FundingAmount { get; set; }
        public string Type { get; set; }
        public string GrantDescription { get; set; }
     
    }
}
