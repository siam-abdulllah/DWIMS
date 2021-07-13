export interface IApprovalTimeLimit {
    id: number;
    approvalAuthorityId: number;
    timeLimit: string;
    remarks: string;
    status: number;
    setOn: Date;
}


export class ApprovalTimeLimit implements IApprovalTimeLimit {
    id: number = 0;
    approvalAuthorityId: number;
    timeLimit: string;
    remarks: string;
    status: number;
    setOn: Date;
}