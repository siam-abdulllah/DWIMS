
export interface IMarketGroupDtl {
    id: number;
    mstId: number;
    marketCode: string;
    marketName: string;
    sbu: string;
    sbuName: string;
    status: string; 
}
 
export class MarketGroupDtl implements IMarketGroupDtl {
    id: number;
    mstId: number;
    marketCode: string;
    marketName: string;
    sbu: string;
    sbuName: string;
    status: string; 
}