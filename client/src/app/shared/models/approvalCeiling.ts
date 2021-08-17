import { IApprovalAuthority } from "./approvalAuthority";
import { IDonation } from "./donation";

export interface IApprovalCeiling {
    id: number;
    approvalAuthorityId: number;
    donationType: string;
    amountPerTransacion: number;
    amountPerMonth: number;
    remarks: string;
    additional: string;
    status: number;
    investmentFrom: Date;
    investmentTo: Date;
    approvalAuthority:IApprovalAuthority;
}

export class ApprovalCeiling implements IApprovalCeiling {
    id: number = 0;
    approvalAuthorityId: number=null;
    donationType: string=null;
    amountPerTransacion: number;
    amountPerMonth: number;
    remarks: string;
    additional: string;
    status: number;
    investmentFrom: Date;
    investmentTo: Date;
    approvalAuthority:IApprovalAuthority;
}