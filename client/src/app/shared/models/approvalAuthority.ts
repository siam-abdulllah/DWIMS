
export interface IApprovalAuthority {
    id: number;
    compId: number;
    deptId: number;
    approvalAuthorityName: string;
    priority: number;
    remarks: string; 
    status: string; 
    setOn: Date;
}
 
export class ApprovalAuthority implements IApprovalAuthority {
    id: number=0;
    compId: number;
    deptId: number;
    approvalAuthorityName: string;
    remarks: string; 
    priority: number=null;
    status: string=null; 
    setOn: Date;
}