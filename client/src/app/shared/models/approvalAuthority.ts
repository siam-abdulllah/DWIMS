
export interface IApprovalAuthority {
    id: number;
    approvalAuthorityName: string;
    priority: number;
    remarks: string; 
    status: string; 
    setOn: Date;
}
 
export class ApprovalAuthority implements IApprovalAuthority {
    id: number=0;
    approvalAuthorityName: string;
    remarks: string; 
    priority: number=1;
    status: string=null; 
    setOn: Date;
}