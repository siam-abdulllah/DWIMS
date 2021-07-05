
export interface IDonation {
    id: number;
    donationTypeName: string;
    remarks: string; 
    status: string; 
    setOn: Date;
}
 
export class Donation implements IDonation {
    id: number=0;
    donationTypeName: string;
    remarks: string; 
    status: string="Active"; 
    setOn: Date;
}