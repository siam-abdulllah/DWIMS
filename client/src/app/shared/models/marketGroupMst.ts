import { IMarketGroupDtl } from "./marketGroupDtl";

export interface IMarketGroupMst {
    id: number;
    groupName: string;
    employeeId: string; 
    status: string; 
    marketGroupDtl:IMarketGroupDtl[];
}
 
export class MarketGroupMst implements IMarketGroupMst {
    id: number=0;
    groupName: string=null;
    employeeId: string=null; 
    status: string; 
    marketGroupDtl:IMarketGroupDtl[];
}