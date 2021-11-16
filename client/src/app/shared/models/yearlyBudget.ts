export interface IYearlyBudget {
    id: number;
    year: string;
    amount: number;
    fromDate: any;
    toDate: any;
    status: string;
}

export class YearlyBudget implements IYearlyBudget {
    id: number = 0;
    year: string;
    amount: number;
    fromDate: any;
    toDate: any;
    status: string;
}



