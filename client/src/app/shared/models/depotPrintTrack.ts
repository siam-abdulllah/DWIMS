export interface IDepotPrintTrack {
    id: number;
    investmentInitId: number;
    paymentRefNo: string;
    paymentDate: Date;
    depotId: string;
    depotName: string;
    remarks: string;
    employeeId: number;
    lastPrintTime : Date;
    printCount : number;
    chequeNo: string;
    bankName: string;
}

export class DepotPrintTrack implements IDepotPrintTrack {
    id: number = 0;
    investmentInitId: number;
    paymentRefNo: string;
    paymentDate: Date;
    depotId: string;
    depotName: string;
    remarks: string;
    employeeId: number;
    lastPrintTime : Date = new Date();
    printCount : number;
    chequeNo: string;
    bankName: string;
}
