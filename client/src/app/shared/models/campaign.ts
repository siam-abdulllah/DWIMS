
export interface ICampaignMst {
    id: number;
    campaignNo: string;
    campaignName: string;
    sbu: string;
    brandCode: string;
    setOn: Date;
}
 
export class CampaignMst implements ICampaignMst {
    id: number=0;
    campaignNo: string;
    campaignName: string;
    sbu: string=null;
    brandCode: string=null;
    setOn: Date;
}
export interface ICampaignDtl {
    id: number;
    mstId: number;
    subCampaignId: number;
    subCampaignName: string;
    budget: string;
    subCampStartDate: Date;
    subCampEndDate: Date;
}
 
export class CampaignDtl implements ICampaignDtl {
    id: number=0;
    mstId: number;
    subCampaignId: number=null;
    subCampaignName: string;
    budget: string;
    subCampStartDate: Date;
    subCampEndDate: Date;
}
export interface ICampaignDtlProduct {
    id: number;
    dtlId: number;
    productId: number;
    productName: string;
    
}
 
export class CampaignDtlProduct implements ICampaignDtlProduct {
    id: number=0;
    dtlId: number;
    productId: number=null;
    productName: string;
}