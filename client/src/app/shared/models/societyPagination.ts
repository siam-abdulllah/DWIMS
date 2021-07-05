import { ISocietyInfo } from './societyInfo';

export interface ISocietyPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: ISocietyInfo[];
}

export class SocietyPagination implements ISocietyPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: ISocietyInfo[] = [];
}