
export interface ICampaign {
    id: number;
    campaignName: string;
    SBU: string;
    brand: string;
    subCampaign: string;
    budget: string;
    startDate: string;
    endDate: string;
    remarks: string; 
    status: string; 
    setOn: Date;
}
 
export class Campaign implements ICampaign {
    id: number=0;
    campaignName: string;
    SBU: string;
    brand: string;
    subCampaign: string;
    budget: string;
    startDate: string;
    endDate: string;
    remarks: string; 
    status: string="Active"; 
    setOn: Date;
}