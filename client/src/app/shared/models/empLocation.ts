export interface IEmployeeLocation {
    id: number;
    employeeName: string;
    priority: number;
    marketCode : string;
    marketName : string;
    territoryCode: string;
    territoryName: string;
    regionCode: string; 
    regionName: string; 
    zoneCode: string; 
    zoneName: string; 
}
 
export class EmployeeLocation implements IEmployeeLocation {
    id: number=0;
    employeeName: string;
    priority: number;
    marketCode : string;
    marketName : string;
    territoryCode: string;
    territoryName: string;
    regionCode: string; 
    regionName: string; 
    zoneCode: string; 
    zoneName: string; 
}

