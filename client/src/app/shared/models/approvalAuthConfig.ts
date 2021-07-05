
export interface IApprovalAuthConfig {
    id: number;
    approvalAuthorityId: string;
    employeeId: string; 
    status: string; 
}
 
export class ApprovalAuthConfig implements IApprovalAuthConfig {
    id: number=0;
    approvalAuthorityId: string=null;
    employeeId: string=null; 
    status: string; 
}