import { IInvestmentInit } from './investment';

export interface IInvestmentInitPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IInvestmentInit[];
}

export class InvestmentInitPagination implements IInvestmentInitPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IInvestmentInit[] = [];
}