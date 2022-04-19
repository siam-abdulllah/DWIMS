import { IDonation } from "./donation";
import { ISBU } from "./sbu";

export interface ISBUWiseBudget {
    id: number;
    sbu: string;
    sbuName: string;
    donationId: number;
    amount: number;
    fromDate: any;
    toDate: any;
    status: string;
    year: any;
    donation:IDonation;
}

export class SBUWiseBudget implements ISBUWiseBudget {
    id: number = 0;
    sbu: string=null;
    sbuName: string;
    donationId: number=null;
    amount: number;
    fromDate: any;
    toDate: any;
    status: string;
    year: any;
    donation:IDonation;
}



