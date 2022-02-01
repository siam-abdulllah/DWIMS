export interface IMedicineDispatch {
    id: number;
    investmentInitId: number;
    issueReference: string;
    issueDate: Date;
    depotCode: string;
    depotName: string;
    remarks: string;
    employeeId: number;
    dispatchAmt : number;
    proposeAmt : number;
}

export class MedicineDispatch implements IMedicineDispatch {
    id: number = 0;
    investmentInitId: number;
    issueReference: string;
    issueDate: Date;
    depotCode: string;
    depotName: string;
    remarks: string;
    employeeId: number;
    dispatchAmt : number;
    proposeAmt : number;
}

export interface IMedicineDispatchDtl {
    id: number;
    productId: number;
    productName: string;
    investmentInitId: number;
    employeeId: number;
    tpVat : number;
    boxQuantity : number;
    dispatchQuantity : number;
    dispatchTpVat : number;
}

export class MedicineDispatchDtl implements IMedicineDispatchDtl {
    id: number = 0;
    productId: number;
    investmentInitId: number;
    productName: string;
    employeeId: number;
    tpVat : number;
    boxQuantity : number;
    dispatchQuantity : number;
    dispatchTpVat : number;
}


export interface IMedDispSearch {
    id: number;
    referenceNo: string;
    depotName: string;
    donationTypeName: string;
    proposeFor: string;
    doctorName: string;
    proposedAmount: number;
    employeeName: string;
    marketName: string;
    approvedBy: string;
    approvedDate: Date;
}

export class MedDispSearch implements IMedDispSearch {
    id: number;
    referenceNo: string;
    donationTypeName: string;
    proposeFor: string;
    depotName: string;
    doctorName: string;
    proposedAmount: number;
    employeeName: string;
    marketName: string;
    approvedBy: string;
    approvedDate: Date;
}

