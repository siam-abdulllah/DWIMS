import { IApprovalAuthority } from "./approvalAuthority";
import { IDonation } from "./donation";

export interface IApprovalCeiling {
    id: number;
    approvalAuthorityId: number;
    donationId: number;
    amountPerTransacion: number;
    amountPerMonth: number;
    remarks: string;
    additional: string;
    status: number;
    investmentFrom: any;
    investmentTo: any;
    approvalAuthority:IApprovalAuthority;
    donation:IDonation;
}

export class ApprovalCeiling implements IApprovalCeiling {
    id: number = 0;
    approvalAuthorityId: number=null;
    donationId: number=null;
    amountPerTransacion: number;
    amountPerMonth: number;
    remarks: string;
    additional: string;
    status: number;
    investmentFrom: any;
    investmentTo: any;
    approvalAuthority:IApprovalAuthority;
    donation:IDonation;
}