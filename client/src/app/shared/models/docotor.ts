
export interface IDoctor {
    id: number;
    doctorName: string;
    doctorCode: string;
    doctorID: number;
    designation: string;
    degree: string;
    year: string; 
    month: string; 
    status: string; 
    amount: string; 
    setOn: Date;
}
 
export class Doctor implements IDoctor {
    id: number=0;
    doctorName: string;
    doctorCode: string;
    doctorID: number;
    designation: string;
    degree: string;
    year: string; 
    month: string; 
    setOn: Date;
    status: string="Active"; 
    amount: string; 
}