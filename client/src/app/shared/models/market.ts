
export interface IMarket {
    id: number;
    marketName: string;
    marketCode: string;
    status: string; 
    sbu: string;
    setOn: Date;
}
 
export class Market implements IMarket {
    id: number=0;
    marketName: string;
    marketCode: string;
    status: string="Active"; 
    sbu: string;
    setOn: Date;
}