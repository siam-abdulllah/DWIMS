export interface IEmployeeInfo {
    id: number;
    empSapCode : string;
    employeeName : string;
    departmentId : number;
    departmentName : string;
    designationId : number;
    designationName : string;
    companyId : number;
    companyName : number;
    joiningDate : Date;
    joiningPlace : string;
    phone : string;
    email : string;
    postingType : string;
    marketName : string;
    marketId : number;
    regionName : string;
    regionId : number;
    zoneName : string;
    zoneId : number;
    territoryName : string;
    territoryId : number;
    divisionName : string;
    divisionId : number;
    sbu:string;
}

export class EmployeeInfo implements IEmployeeInfo {
    id: number;
    empSapCode : string;
    employeeName : string;
    departmentId : number;
    departmentName : string;
    designationId : number;
    designationName : string;
    companyId : number;
    companyName : number;
    joiningDate : Date;
    joiningPlace : string;
    phone : string;
    email : string;
    postingType : string;
    marketName : string;
    marketId : number;
    regionName : string;
    regionId : number;
    zoneName : string;
    zoneId : number;
    territoryName : string;
    territoryId : number;
    divisionName : string;
    divisionId : number;
    sbu:string;
}