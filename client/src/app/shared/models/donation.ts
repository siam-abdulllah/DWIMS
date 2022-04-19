
export interface IDonation {
    id: number;
    donationTypeName: string;
    donationShortName: string;
    remarks: string; 
    status: string; 
    setOn: Date;
}
 
export class Donation implements IDonation {
    id: number=0;
    donationTypeName: string;
    donationShortName: string;
    remarks: string; 
    status: string=null; 
    setOn: Date;
}