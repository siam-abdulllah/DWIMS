import { IMarketGroupDtl } from "./marketGroupDtl";

export interface IMarketGroupMst {
    id: number;
    marketCode: string;
    groupName: string;
    employeeId: number; 
    status: string; 
    marketGroupDtls:IMarketGroupDtl[];
}
 
export class MarketGroupMst implements IMarketGroupMst {
    id: number=0;
    marketCode: string;
    groupName: string;
    employeeId: number; 
    status: string=null; 
    marketGroupDtls:IMarketGroupDtl[];
}