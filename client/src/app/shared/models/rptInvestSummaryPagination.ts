import { IrptInvestSummary } from './rptInvestSummary';

export interface IrptInvestSummaryPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IrptInvestSummary[];
}

export class rptInvestSummaryPagination implements IrptInvestSummaryPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IrptInvestSummary[] = [];
}