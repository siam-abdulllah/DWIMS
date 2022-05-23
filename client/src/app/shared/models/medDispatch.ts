export interface IMedicineDispatch {
    id: number;
    investmentInitId: number;
    issueReference: string;
    payRefNo: string;
    issueDate: Date;
    depotCode: string;
    depotName: string;
    remarks: string;
    employeeId: number;
    dispatchAmt : number;
    proposeAmt : number;
    sapRefNo: string;
}

export class MedicineDispatch implements IMedicineDispatch {
    id: number = 0;
    investmentInitId: number;
    issueReference: string;
    payRefNo: string;
    issueDate: Date;
    depotCode: string;
    depotName: string;
    remarks: string;
    employeeId: number;
    dispatchAmt : number;
    proposeAmt : number;
    sapRefNo: string;
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
    sbu : string;
    productCode : string;
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
    sbu : string;
    productCode : string;
}


export interface IMedDispSearch {
    id: number;
    payRefNo: string;
    referenceNo: string;
    depotName: string;
    investmentInitId: number;
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
    payRefNo: string;
    donationTypeName: string;
    investmentInitId: number;
    proposeFor: string;
    depotName: string;
    doctorName: string;
    proposedAmount: number;
    employeeName: string;
    marketName: string;
    approvedBy: string;
    approvedDate: Date;
}

