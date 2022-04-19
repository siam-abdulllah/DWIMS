
export interface IBudgetCeiling {

    amountPerTransacion:  string;
    amountPerMonth:  string;
    monthlyExpense: string;
    monthlyRemaining: string;
    sbuWiseBudget: string;
    sbuWiseExpense: string;
    sbuWiseRemaining: string;
    donationType: string;
    sbu: string;
}

export class BudgetCeiling implements IBudgetCeiling {
    amountPerTransacion:  string;
    amountPerMonth:  string;
    monthlyExpense:  string;
    monthlyRemaining:  string;
    sbuWiseBudget:  string;
    sbuWiseExpense:  string;
    sbuWiseRemaining:  string;
    donationType: string;
    sbu: string;
}