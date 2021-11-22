export interface IYearlyBudget {
    id: number;
    year: any;
    amount: number;
    fromDate: any;
    toDate: any;
    status: string;
}

export class YearlyBudget implements IYearlyBudget {
    id: number = 0;
    year: any;
    amount: number;
    fromDate: any;
    toDate: any;
    status: string;
}



