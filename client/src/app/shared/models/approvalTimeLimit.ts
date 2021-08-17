import { IApprovalAuthority } from "./approvalAuthority";

export interface IApprovalTimeLimit {
    id: number;
    approvalAuthorityId: number;
    timeLimit: string;
    remarks: string;
    status: string;
    setOn: Date;
    approvalAuthority:IApprovalAuthority;
}


export class ApprovalTimeLimit implements IApprovalTimeLimit {
    id: number = 0;
    approvalAuthorityId: number;
    timeLimit: string=null;
    remarks: string;
    status: string=null;
    setOn: Date;
    approvalAuthority:IApprovalAuthority;
}