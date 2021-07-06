import { IMarketGroupDtl } from './marketGroupDtl';

export interface IMarketGroupPaginationDtlt {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IMarketGroupDtl[];
}

export class MarketGroupPaginationDtlt implements IMarketGroupPaginationDtlt {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IMarketGroupDtl[] = [];
}