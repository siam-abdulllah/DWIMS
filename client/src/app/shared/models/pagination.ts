import { IPost } from './post';


export interface IPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IPost[];
}

export class Pagination implements IPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IPost[] = [];
}