import { IBcdsInfo } from "./bcdsInfo";
import { ICampaignDtl, ICampaignMst } from "./campaign";
import { IDoctor } from "./docotor";
import { IDonation } from "./donation";
import { IInstitution } from "./institution";
import { IMarketGroupMst } from "./marketGroupMst";
import { IProduct } from "./product";
import { ISocietyInfo } from "./societyInfo";

export interface IInvestmentInit {
    id: number;
    referenceNo: string;
    proposeFor: string;
    donationTo: string;
    marketCode: string;
    marketName: string;
    sbu: string;
    employeeId: number;
    donationId: number;
    donation:IDonation;
    setOn: Date;
}
 
export class InvestmentInit implements IInvestmentInit {
    id: number=0;
    referenceNo: string;
    proposeFor: string=null;
    donationTo: string=null;
    marketCode: string;
    marketName: string;
    sbu: string;
    employeeId: number;
    donationId: number=null;
    donation:IDonation;
    setOn: Date;
}


export interface IInvestmentRec {
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
    employeeId: number;
}
 
export class InvestmentRec implements IInvestmentRec {
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
    employeeId: number;
}
export interface IInvestmentRecComment {
    id: number;
    investmentInitId: number;
    comments: string;
    recStatus: string;
    employeeId: number;
}
 
export class InvestmentRecComment implements InvestmentRecComment {
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
    employeeId: number;
    sbu: string;
}
 
export class InvestmentTargetedProd implements IInvestmentTargetedProd {
    id: number=0;
    investmentInitId: number;
    productId: number=null;
    productInfo:IProduct;
    employeeId: number;
    sbu: string;
}
export interface IInvestmentTargetedGroup {
    id: number;
    marketGroupMstId: number;
    investmentInitId: number;
    marketCode: string;
    marketName: string;
    recStatus: string;
    approvalAuthorityName: string;
    marketGroupMst:IMarketGroupMst;
}
 
export class InvestmentTargetedGroup implements IInvestmentTargetedGroup {
    id: number=0;
    marketGroupMstId: number=null;
    investmentInitId: number;
    marketCode: string;
    marketName: string;
    recStatus: string;
    approvalAuthorityName: string;
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
    responsibleDoctorId: number;
    responsibleDoctorName:string;
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
    responsibleDoctorId: number=null;
    responsibleDoctorName:string;
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
    responsibleDoctorId: number;
    responsibleDoctorName:string;
    doctorInfo:IDoctor;
    
}

export interface IInvestmentMedicineProd {
    id: number;
    investmentInitId: number;
    productId: number;
    productInfo:IProduct;
    boxQuantity:number;
    tpVat: number;
    employeeId: number;
}
 
export class InvestmentMedicineProd implements IInvestmentMedicineProd {
    id: number=0;
    investmentInitId: number;
    productId: number;
    productInfo:IProduct;
    boxQuantity:number;
    tpVat: number;
    employeeId: number;
}
 
export class InvestmentBcds implements IInvestmentBcds {
    id: number=0;
    investmentInitId: number;
    bcdsId: number=null;
    bcdsName: string;
    bcdsAddress: string;
    noOfMember: string;
    bcds:IBcdsInfo;
    responsibleDoctorId: number=null;
    responsibleDoctorName:string;
    doctorInfo:IDoctor;
}
export interface IInvestmentSociety {
    id: number;
    investmentInitId: number;
    societyId: number;
    societyName: string;
    societyAddress: string;
    noOfMember: string;
    society:ISocietyInfo;
    responsibleDoctorId: number;
    responsibleDoctorName:string;
    doctorInfo:IDoctor;
    
}
 
export class InvestmentSociety implements IInvestmentSociety {
    id: number=0;
    investmentInitId: number;
    societyId: number=null;
    societyName: string;
    societyAddress: string;
    noOfMember: string;
    society:ISocietyInfo;
    responsibleDoctorId: number=null;
    responsibleDoctorName:string;
    doctorInfo:IDoctor;
}
