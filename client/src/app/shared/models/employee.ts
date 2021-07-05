
export interface IEmployee {
    id: number;
    employeeName: string;
    employeeCode: string;
    SBU: string;
    designationName: string;
    remarks: string; 
    status: string; 
   
}
 
export class Employee implements IEmployee {
    id: number=0;
    employeeName: string;
    employeeCode: string;
    SBU: string;
    designationName: string;
    remarks: string; 
    status: string="Active"; 
}