export interface IEmployeeInfo {
    id: number;
    employeeid: number;
    employeeSAPCode : string;
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
    marketCode : string;
    regionName : string;
    regionCode : string;
    zoneName : string;
    zoneCode : string;
    territoryName : string;
    territoryCode : string;
    divisionName : string;
    divisionCode : string;
    sbu:string;
}

export class EmployeeInfo implements IEmployeeInfo {
    id: number;
    employeeid: number;
    employeeSAPCode : string;
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
    marketCode : string;
    regionName : string;
    regionCode : string;
    zoneName : string;
    zoneCode : string;
    territoryName : string;
    territoryCode : string;
    divisionName : string;
    divisionCode : string;
    sbu:string;
}