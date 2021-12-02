export interface IrptInvestSummary {
    id: number;
    referenceNo: string;
    donationTypeName: string;
    donationTo: string;
    proposedAmount: number;
    fromDate: Date;
    toDate: Date;
    invStatus: string;
    employeeName: string;
}

export class rptInvestSummary implements IrptInvestSummary {
    id: number;
    referenceNo: string;
    donationTypeName: string;
    donationTo: string;
    proposedAmount: number;
    fromDate: Date;
    toDate: Date;
    invStatus: string;
    employeeName: string;
}
