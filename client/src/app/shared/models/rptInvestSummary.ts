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
    receiveStatus: string;
    receiveBy: string;
    approvedBy: string;
    marketName: string;
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
    receiveStatus: string;
    receiveBy: string;
    approvedBy: string;
    marketName: string;
}

export interface IrptDepotLetterSearch {
    id: number;
    referenceNo: string;
    donationTypeName: string;
    proposeFor: string;
    doctorName: string;
    proposedAmount: number;
    employeeName: string;
    marketName: string;
}

export class rptDepotLetterSearch implements IrptDepotLetterSearch {
    id: number;
    referenceNo: string;
    donationTypeName: string;
    proposeFor: string;
    doctorName: string;
    proposedAmount: number;
    employeeName: string;
    marketName: string;
}

export interface IDepotLetterSearchPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IrptDepotLetterSearch[];
}

export class DepotLetterSearchPagination implements IDepotLetterSearchPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IrptDepotLetterSearch[] = [];
}