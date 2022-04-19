
export interface IDoctorHonAppr {
    id: number;
    investmentInitId: number;
    doctorID: number;
    honMonth: string; 
    status: string; 
    honAmount: string; 
    setOn: Date;
}
 
export class DoctorHonAppr implements IDoctorHonAppr {
    id: number=0;
    investmentInitId: number;
    doctorID: number;
    honMonth: string; 
    status: string=null; 
    honAmount: string; 
    setOn: Date;
}