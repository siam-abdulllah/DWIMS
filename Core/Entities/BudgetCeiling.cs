using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class BudgetCeiling :BaseEntity
    {
        public  double AmountPerTransacion { get; set; }
        public  double AmountPerMonth { get; set; }
        public  double MonthlyExpense { get; set; }
        public  double MonthlyRemaining { get; set; } 
        public  double SBUWiseBudget { get; set; }
        public  double SBUWiseExpense { get; set; }
        public  double SBUWiseRemaining { get; set; }
        public string DonationType { get; set; }
        public string SBU { get; set; }
        //public string SBUName { get; set; }
    }
}