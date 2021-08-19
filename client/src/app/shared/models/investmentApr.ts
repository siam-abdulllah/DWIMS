import { IBcdsInfo } from "./bcdsInfo";
import { ICampaignDtl, ICampaignMst } from "./campaign";
import { IDoctor } from "./docotor";
import { IInstitution } from "./institution";
import { IMarketGroupMst } from "./marketGroupMst";
import { IProduct } from "./product";
import { ISocietyInfo } from "./societyInfo";

export interface IInvestmentInit {
    id: number;
    referenceNo: string;
    proposeFor: string;
    donationType: string;
    donationTo: string;
    employeeId: number;
}
 
export class InvestmentInit implements IInvestmentInit {
    id: number=0;
    referenceNo: string;
    proposeFor: string=null;
    donationType: string=null;
    donationTo: string=null;
    employeeId: number;
}


export interface IInvestmentApr {
    id: number;
    investmentInitId: number;
    chequeTitle: string;
    paymentMethod: string;
    commitmentAllSBU: string;
    commitmentOwnSBU: string;
    shareAllSBU: string;
    shareOwnSBU: string;
    totalMonth: number;
    proposedAmount: string;
    purpose: string;
    fromDate: Date;
    toDate: Date;
}
 
export class InvestmentApr implements IInvestmentApr {
    id: number=0;
    investmentInitId: number;
    chequeTitle: string;
    paymentMethod: string=null;
    commitmentAllSBU: string;
    commitmentOwnSBU: string;
    shareAllSBU: string;
    shareOwnSBU: string;
    totalMonth: number;
    proposedAmount: string;
    purpose: string;
    fromDate: Date;
    toDate: Date;
}
export interface IInvestmentAprComment {
    id: number;
    investmentInitId: number;
    comments: string;
    recStatus: string;
    employeeId: number;
}
 
export class InvestmentAprComment implements InvestmentAprComment {
    id: number=0;
    investmentInitId: number;
    comments: string;
    recStatus: string=null;
    employeeId: number;
}
export interface IInvestmentTargetedProd {
    id: number;
    investmentInitId: number;
    productId: number;
    productInfo:IProduct;
}
 
export class InvestmentTargetedProd implements IInvestmentTargetedProd {
    id: number=0;
    investmentInitId: number;
    productId: number=null;
    productInfo:IProduct;
}
export interface IInvestmentTargetedGroup {
    id: number;
    marketGroupMstId: number;
    investmentInitId: number;
    marketCode: string;
    marketName: string;
    marketGroupMst:IMarketGroupMst;
}
 
export class InvestmentTargetedGroup implements IInvestmentTargetedGroup {
    id: number=0;
    marketGroupMstId: number=null;
    investmentInitId: number;
    marketCode: string;
    marketName: string;
    marketGroupMst:IMarketGroupMst;
}
export interface IInvestmentDoctor {
    id: number;
    investmentInitId: number;
    institutionName: string;
    institutionId: number;
    doctorId: number;
    doctorName: string;
    address: string;
    degree: string;
    designation: string;
    doctorCategory: string;
    doctorType: string;
    practiceDayPerMonth: string;
    patientsPerDay: string;
    doctorInfo:IDoctor;
    institutionInfo:IInstitution;
    
}
 
export class InvestmentDoctor implements IInvestmentDoctor {
    id: number=0;
    investmentInitId: number;
    institutionId: number=null;
    institutionName: string;
    doctorId: number=null;
    degree: string;
    designation: string;
    doctorCategory: string=null;
    doctorName: string=null;
    address: string;
    doctorType: string=null;
    practiceDayPerMonth: string;
    patientsPerDay: string;
    doctorInfo:IDoctor;
    institutionInfo:IInstitution;
}
export interface IInvestmentInstitution {
    id: number;
    investmentInitId: number;
    institutionId: number;
    institutionName:string;
    resposnsibleDoctorId: number;
    resposnsibleDoctorName:string;
    institutionType: string;
    address: string;
    noOfBed: string;
    departmentUnit: string;
    institutionInfo:IInstitution;
    doctorInfo:IDoctor;
    
}
 
export class InvestmentInstitution implements IInvestmentInstitution {
    id: number=0;
    investmentInitId: number;
    institutionId: number=null;
    institutionName:string;
    resposnsibleDoctorId: number=null;
    resposnsibleDoctorName:string;
    institutionType: string;
    address: string;
    noOfBed: string;
    departmentUnit: string;
    institutionInfo:IInstitution;
    doctorInfo:IDoctor;
}
export interface IInvestmentCampaign {
    id: number;
    investmentInitId: number;
    campaignDtlId: number;
    campaignMstId: number;
    doctorId: number;
    institutionId: number;
    campaignName:string;
    subCampaignName:string;
    doctorName:string;
    institutionName:string;
    subCampStartDate:string;
    subCampEndDate:string;
    campaignMst:ICampaignMst;
    campaignDtl:ICampaignDtl;
    doctorInfo:IDoctor;
    institutionInfo:IInstitution;
}
 
export class InvestmentCampaign implements IInvestmentCampaign {
    id: number=0;
    investmentInitId: number;
    campaignDtlId: number=null;
    campaignMstId: number=null;
    doctorId: number=null;
    institutionId: number=null;
    campaignName:string;
    subCampaignName:string;
    doctorName:string;
    institutionName:string;
    subCampStartDate:string;
    subCampEndDate:string;
    campaignMst:ICampaignMst;
    campaignDtl:ICampaignDtl;
    doctorInfo:IDoctor;
    institutionInfo:IInstitution;
}
export interface IInvestmentBcds {
    id: number;
    investmentInitId: number;
    bcdsId: number;
    bcdsName: string;
    bcdsAddress: string;
    noOfMember: string;
    bcds:IBcdsInfo;
    
}
 
export class InvestmentBcds implements IInvestmentBcds {
    id: number=0;
    investmentInitId: number;
    bcdsId: number=null;
    bcdsName: string;
    bcdsAddress: string;
    noOfMember: string;
    bcds:IBcdsInfo;
}
export interface IInvestmentSociety {
    id: number;
    investmentInitId: number;
    societyId: number;
    societyName: string;
    societyAddress: string;
    noOfMember: string;
    society:ISocietyInfo;
    
}
 
export class InvestmentSociety implements IInvestmentSociety {
    id: number=0;
    investmentInitId: number;
    societyId: number=null;
    societyName: string;
    societyAddress: string;
    noOfMember: string;
    society:ISocietyInfo;
}
