import { ISBUWiseBudget } from './sbuWiseBudget';

export interface ISBUWiseBudgetPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: ISBUWiseBudget[];
}

export class SBUWiseBudgetPagination implements ISBUWiseBudgetPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: ISBUWiseBudget[] = [];
}