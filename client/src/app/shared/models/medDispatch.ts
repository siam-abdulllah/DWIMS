export interface IMedicineDispatch {
    id: number;
    investmentInitId: number;
    issueReference: string;
    issueDate: Date;
    depotCode: string;
    depotName: string;
    remarks: string;
    employeeId: number;
    dispatchAmount : number;
    proposedAmount : number;
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
    dispatchAmount : number;
    proposedAmount : number;
}
