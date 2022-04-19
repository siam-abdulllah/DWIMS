import { IInvestmentRcv } from './investmentRcv';

export interface IInvestmentRcvPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IInvestmentRcv[];
}

export class InvestmentRcvPagination implements IInvestmentRcvPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IInvestmentRcv[] = [];
}