
export interface IDocotor {
    id: number;
    doctorName: string;
    doctorCode: string;
    doctorID: number;
    designation: string;
    year: string; 
    month: string; 
    status: string; 
    amount: string; 
    setOn: Date;
}
 
export class docotor implements IDocotor {
    id: number=0;
    doctorName: string;
    doctorCode: string;
    doctorID: number;
    designation: string;
    year: string; 
    month: string; 
    setOn: Date;
    status: string="Active"; 
    amount: string; 
}