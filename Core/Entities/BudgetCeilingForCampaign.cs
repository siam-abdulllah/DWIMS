using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class BudgetCeilingForCampaign : BaseEntity
    {
        public  double CampaignBudget { get; set; }
        public  double TotalExpense { get; set; }
        public  double TotalRemaining { get; set; }
        public int DonationId { get; set; }
        public string SBU { get; set; }
    }
}