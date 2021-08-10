import { IRegApproval } from './regApproval';

export interface IRegApprovalPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IRegApproval[];
}

export class RegApprovalPagination implements IRegApprovalPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IRegApproval[] = [];
}