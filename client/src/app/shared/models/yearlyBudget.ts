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

export interface IYearlyBudgetReport {
    id: number;
    year : number; 
    donationTypeName  : string;
    dnationId  : string;
    sbuName  : string;
    amount : number;
    expense : number;
    january : number;
    february : number;
    march : number;
    april : number;
    may : number;
    june : number;
    july : number;
    august : number;
    september : number;
    october : number;
    november : number;
    december : number;
}

export class YearlyBudgetReport implements IYearlyBudgetReport {
    id: number = 0;
    year : number; 
    donationTypeName  : string;
    dnationId  : string;
    sbuName  : string;
    amount : number;
    expense : number;
    january : number;
    february : number;
    march : number;
    april : number;
    may : number;
    june : number;
    july : number;
    august : number;
    september : number;
    october : number;
    november : number;
    december : number;
}



