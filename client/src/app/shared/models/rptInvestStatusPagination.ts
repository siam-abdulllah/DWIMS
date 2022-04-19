import { IrptInvestStatus } from './rptInvestStatus';

export interface IrptInvestStatusPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IrptInvestStatus[];
}

export class rptInvestStatusPagination implements IrptInvestStatusPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IrptInvestStatus[] = [];
}