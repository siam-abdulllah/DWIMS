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

export interface IrptDepotLetter {
    id: number;
    setOn: Date;
    referenceNo: string;
    donationTypeName: string;
    doctorName: string;
    proposedAmount: number;
    address: string;
    docId: number;
    designationName: string;
    employeeName: string;
    empId: number;
    depotName: string;
    marketName: string;
}

export class rptDepotLetter implements IrptDepotLetter {
    id: number;
    referenceNo: string;
    setOn: Date;
    donationTypeName: string;
    doctorName: string;
    proposedAmount: number;
    address: string;
    docId: number;
    designationName: string;
    empId: number;
    employeeName: string;
    depotName: string;
    marketName: string;
}

export interface IDepotLetterPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IrptDepotLetter[];
}

export class DepotLetterPagination implements IDepotLetterPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IrptDepotLetter[] = [];
}