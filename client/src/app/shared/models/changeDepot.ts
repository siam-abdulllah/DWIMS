export interface IChangeDepot {
    id: number;
    investmentInitId: number;
    oldDepotName: string;
    oldDepotCode : string;
    depotCode : string;
    remarks: string;
    employeeId: number;
}

export class ChangeDepot implements IChangeDepot {
    id: number = 0;
    investmentInitId: number;
    oldDepotName: string;
    oldDepotCode : string;
    depotCode : string;
    remarks: string;
    employeeId: number;
}

export interface IChangeDepotSearch {
    id: number;
    investmentInitId: number;
    referenceNo: string;
    donationTypeName: string;
    proposeFor: string;
    doctorName: string;
    proposedAmount: number;
    employeeName: string;
    marketName: string;
    depotCode: string;
    depotName: string;
  }

