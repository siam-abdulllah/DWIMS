
export interface IMarketGroupDtl {
    id: number;
    mstId: number;
    marketCode: string;
    status: string; 
}
 
export class MarketGroupDtl implements IMarketGroupDtl {
    id: number;
    mstId: number;
    marketCode: string;
    status: string; 
}