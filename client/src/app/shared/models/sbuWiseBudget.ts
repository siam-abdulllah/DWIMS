import { ISBU } from "./sbu";

export interface ISBUWiseBudget {
    id: number;
    sbu: string;
    amount: number;
    fromDate: Date;
    toDate: Date;
    status: string;
}

export class SBUWiseBudget implements ISBUWiseBudget {
    id: number = 0;
    sbu: string=null;
    amount: number;
    fromDate: Date;
    toDate: Date;
    status: string;
}



