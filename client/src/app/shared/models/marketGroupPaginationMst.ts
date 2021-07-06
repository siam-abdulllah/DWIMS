import { IMarketGroupMst } from './marketGroupMst';

export interface IMarketGroupPaginationMst {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IMarketGroupMst[];
}

export class MarketGroupPaginationMst implements IMarketGroupPaginationMst {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IMarketGroupMst[] = [];
}