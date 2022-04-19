import { IInvestmentApr } from './investmentApr';

export interface IInvestmentAprPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IInvestmentApr[];
}

export class InvestmentAprPagination implements IInvestmentAprPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IInvestmentApr[] = [];
}