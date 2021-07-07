
export interface IMarketGroupDtl {
    id: number;
    mstId: number;
    marketName: string;
    status: string; 
}
 
export class MarketGroupDtl implements IMarketGroupDtl {
    id: number;
    mstId: number;
    marketCode: string;
    marketName: string;
    status: string; 
}