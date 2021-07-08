export interface IApprovalCeiling {
    id: number;
    approvalAuthorityId: number;
    investmentTypeId: number;
    transacionAmount: number;
    remarks: string;
    additional: string;
    status: number;
    investmentFrom: Date;
    investmentTo: Date;
}

export class ApprovalCeiling implements IApprovalCeiling {
    id: number = 0;
    approvalAuthorityId: number;
    investmentTypeId: number;
    transacionAmount: number;
    remarks: string;
    additional: string;
    status: number;
    investmentFrom: Date;
    investmentTo: Date;
}