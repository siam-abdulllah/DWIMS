
export interface ISubCampaign {
    id: number;
    subCampaignName: string;
    remarks: string; 
    status: string; 
    setOn: Date;
}
 
export class SubCampaign implements ISubCampaign {
    id: number=0;
    subCampaignName: string;
    remarks: string; 
    status: string=null; 
    setOn: Date;
}