export interface IrptDepotLetter {
    id: number;
    referenceNo: string;
    setOn: Date;
    donationTo: string;
    donationTypeName: string;
    doctorName: string;
    proposedAmount: number;
    address: string;
    docId: number;
    employeeName: string;
    empId: number;
    designationName: string;
    marketName: string;
    depotName: string;
    chequeTitle: string;
}

export class rptDepotLetter implements IrptDepotLetter {
    id: number;
    referenceNo: string;
    setOn: Date;
    donationTo: string;
    donationTypeName: string;
    doctorName: string;
    proposedAmount: number;
    address: string;
    docId: number;
    employeeName: string;
    empId: number;
    designationName: string;
    marketName: string;
    depotName: string;
    chequeTitle: string;
}