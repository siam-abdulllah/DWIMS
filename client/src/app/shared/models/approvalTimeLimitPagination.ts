import { IApprovalTimeLimit } from './approvalTimeLimit';

export interface IApprovalTimeLimitPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IApprovalTimeLimit[];
}

export class ApprovalTimeLimitPagination implements IApprovalTimeLimitPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IApprovalTimeLimit[] = [];
}