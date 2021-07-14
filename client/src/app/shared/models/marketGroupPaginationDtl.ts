import { IMarketGroupDtl } from './marketGroupDtl';

export interface IMarketGroupPaginationDtl {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IMarketGroupDtl[];
}

export class MarketGroupPaginationDtl implements IMarketGroupPaginationDtl {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IMarketGroupDtl[] = [];
}