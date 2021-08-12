import { IInvestmentRec } from './investment';

export interface IInvestmentRecPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IInvestmentRec[];
}

export class InvestmentRecPagination implements IInvestmentRecPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IInvestmentRec[] = [];
}