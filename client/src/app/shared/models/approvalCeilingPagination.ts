import { IApprovalCeiling } from './approvalCeiling';

export interface IApprovalCeilingPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IApprovalCeiling[];
}

export class ApprovalCeilingPagination implements IApprovalCeilingPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IApprovalCeiling[] = [];
}