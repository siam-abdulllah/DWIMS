import { IApprovalAuthority } from './approvalAuthority';

export interface IApprovalAuthorityPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IApprovalAuthority[];
}

export class ApprovalAuthorityPagination implements IApprovalAuthorityPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IApprovalAuthority[] = [];
}