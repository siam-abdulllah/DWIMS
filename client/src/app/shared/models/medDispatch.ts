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
