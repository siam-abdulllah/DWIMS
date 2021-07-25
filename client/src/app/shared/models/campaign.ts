
export interface ICampaignMst {
    id: number;
    campaignNo: string;
    campaignName: string;
    SBU: string;
    brand: string;
    setOn: Date;
}
 
export class CampaignMst implements ICampaignMst {
    id: number;
    campaignNo: string;
    campaignName: string;
    SBU: string=null;
    brand: string=null;
    setOn: Date;
}
export interface ICampaignDtl {
    id: number;
    mstId: number;
    subCampaignId: number;
    subCampaignName: string;
    budget: string;
    startDate: string;
    endDate: string;
}
 
export class CampaignDtl implements ICampaignDtl {
    id: number=0;
    mstId: number;
    subCampaignId: number=null;
    subCampaignName: string;
    budget: string;
    startDate: string;
    endDate: string;
}
export interface ICampaignDtlProduct {
    id: number;
    dtlId: number;
    ProductId: number;
    
}
 
export class CampaignDtlProduct implements ICampaignDtlProduct {
    id: number=0;
    dtlId: number;
    ProductId: number;
}