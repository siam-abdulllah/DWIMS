import { IBcdsInfo } from './bcdsInfo';

export interface IBcdsPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IBcdsInfo[];
}

export class BcdsPagination implements IBcdsPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IBcdsInfo[] = [];
}