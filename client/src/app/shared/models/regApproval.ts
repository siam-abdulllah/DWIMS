export interface IRegApproval {
    id: number;
    userId: string;
    employeeId: number;
    employeeSapCode : string;
    employeeName : string;
    departmentName : string;
    designationName : string;
    phone : string;
    email : string;
    marketName : string;
    regionName : string;
    zoneName : string;
    territoryName : string;
    divisionName : string;
    approvalStatus : string;
    role : string;
}

export class RegApproval implements IRegApproval {
    id: number;
    userId: string;
    employeeId: number;
    employeeSapCode : string;
    employeeName : string;
    departmentName : string;
    designationName : string;
    phone : string;
    email : string;
    marketName : string;
    regionName : string;
    zoneName : string;
    territoryName : string;
    divisionName : string;
    approvalStatus : string=null;
    role : string=null;
}