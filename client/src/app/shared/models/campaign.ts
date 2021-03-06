import { ISubCampaign } from "./subCampaign";

export interface ICampaignMst {
    id: number;
    campaignNo: string;
    campaignName: string;
    sbu: string;
    brandCode: string;
    setOn: Date;
    employeeId: string; 
}
 
export class CampaignMst implements ICampaignMst {
    id: number=0;
    campaignNo: string;
    campaignName: string;
    sbu: string=null;
    brandCode: string=null;
    employeeId: string=null;
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
    subCampaign:ISubCampaign;
}
 
export class CampaignDtl implements ICampaignDtl {
    id: number=0;
    mstId: number;
    subCampaignId: number=null;
    subCampaignName: string;
    budget: string;
    subCampStartDate: Date;
    subCampEndDate: Date;
    subCampaign:ISubCampaign;
}
export interface ISubCampaignRapid {
    subCampId: number;
    sbu: string;
    subCampaignName: string;

}
export class SubCampaignRapid implements ISubCampaignRapid {
    subCampId: number;
    sbu: string;
    subCampaignName: string;
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