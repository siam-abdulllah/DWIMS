
export interface IBudgetCeilingForCampaign {

    campaignBudget:  string;
    totalExpense:  string;
    totalRemaining: string;
    donationId: number;
    sbu: string;
}

export class BudgetCeilingForCampaign implements IBudgetCeilingForCampaign {
    campaignBudget:  string;
    totalExpense:  string;
    totalRemaining:  string;
    donationId: number;
    sbu: string;
}