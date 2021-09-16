import { IMenuHead } from './menuHead';

export interface IMenuHeadPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IMenuHead[];
}

export class MenuHeadPagination implements IMenuHeadPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IMenuHead[] = [];
}