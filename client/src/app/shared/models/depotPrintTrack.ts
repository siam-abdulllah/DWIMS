export interface IDepotPrintTrack {
    id: number;
    investmentInitId: number;
    depotId: string;
    depotName: string;
    remarks: string;
    employeeId: number;
    lastPrintTime : Date;
    printCount : number
}

export class DepotPrintTrack implements IDepotPrintTrack {
    id: number = 0;
    investmentInitId: number;
    depotId: string;
    depotName: string;
    remarks: string;
    employeeId: number;
    lastPrintTime : Date = new Date();
    printCount : number;
}
