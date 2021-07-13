export interface ISBUWiseBudget {
    id: number;
    SBUId: number;
    amount: number;
    fromDate: Date;
    toDate: Date;
    remarks: string;
    status: number;
}

export class SBUWiseBudget implements ISBUWiseBudget {
    id: number = 0;
    SBUId: number;
    amount: number;
    fromDate: Date;
    toDate: Date;
    remarks: string;
    status: number;
}



