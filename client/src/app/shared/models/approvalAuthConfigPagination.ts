import { IApprovalAuthConfig } from './approvalAuthConfig';

export interface IApprovalAuthConfigPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IApprovalAuthConfig[];
}

export class ApprovalAuthConfigPagination implements IApprovalAuthConfigPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IApprovalAuthConfig[] = [];
}