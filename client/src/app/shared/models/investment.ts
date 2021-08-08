import { ICampaignDtl } from "./campaign";
import { IProduct } from "./product";

export interface IInvestmentInit {
    id: number;
    referenceNo: string;
    proposeFor: string;
    donationType: string;
    donationTo: string;
    EmployeeId: number;
}
 
export class InvestmentInit implements IInvestmentInit {
    id: number=0;
    referenceNo: string;
    proposeFor: string=null;
    donationType: string=null;
    donationTo: string=null;
    EmployeeId: number=2;
}
export interface IInvestmentDetail {
    id: number;
    investmentInitId: number;
    chequeTitle: string;
    paymentMethod: string;
    commitmentAllSBU: string;
    commitmentOwnSBU: string;
    shareAllSBU: string;
    shareOwnSBU: string;
    totalMonth: string;
    proposedAmount: string;
    purpose: string;
    fromDate: Date;
    toDate: Date;
}
 
export class InvestmentDetail implements IInvestmentDetail {
    id: number=0;
    investmentInitId: number;
    chequeTitle: string;
    paymentMethod: string=null;
    commitmentAllSBU: string;
    commitmentOwnSBU: string;
    shareAllSBU: string;
    shareOwnSBU: string;
    totalMonth: string;
    proposedAmount: string;
    purpose: string;
    fromDate: Date;
    toDate: Date;
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
    investmentInitId: number;
    marketCode: string;
    marketName: string;
    
}
 
export class InvestmentTargetedGroup implements IInvestmentTargetedGroup {
    id: number=0;
    investmentInitId: number;
    marketCode: string;
    marketName: string;
}
export interface IInvestmentDoctor {
    id: number;
    investmentInitId: number;
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
    
}
 
export class InvestmentDoctor implements IInvestmentDoctor {
    id: number=0;
    investmentInitId: number;
    institutionId: number=null;
    doctorId: number=null;
    degree: string;
    designation: string;
    doctorCategory: string=null;
    doctorName: string=null;
    address: string;
    doctorType: string=null;
    practiceDayPerMonth: string;
    patientsPerDay: string;
}
export interface IInvestmentInstitution {
    id: number;
    investmentInitId: number;
    institutionId: number;
    resposnsibleDoctorId: number;
    institutionType: string;
    address: string;
    noOfBed: string;
    departmentUnit: string;
    
}
 
export class InvestmentInstitution implements IInvestmentInstitution {
    id: number=0;
    investmentInitId: number;
    institutionId: number=null;
    resposnsibleDoctorId: number=null;
    institutionType: string;
    address: string;
    noOfBed: string;
    departmentUnit: string;
}
export interface IInvestmentCampaign {
    id: number;
    investmentInitId: number;
    campaignDtlId: number;
    campaignMstId: number;
    doctorId: number;
    institutionId: number;
    subCampStartDate:string;
    subCampEndDate:string;
    campaignDtl:ICampaignDtl;
}
 
export class InvestmentCampaign implements IInvestmentCampaign {
    id: number=0;
    investmentInitId: number;
    campaignDtlId: number=null;
    campaignMstId: number=null;
    doctorId: number=null;
    institutionId: number=null;
    subCampStartDate:string;
    subCampEndDate:string;
    campaignDtl:ICampaignDtl;
}
export interface IInvestmentBcds {
    id: number;
    investmentInitId: number;
    bcdsId: number;
    bcdsAddress: string;
    noOfMember: string;
    
}
 
export class InvestmentBcds implements IInvestmentBcds {
    id: number=0;
    investmentInitId: number;
    bcdsId: number=null;
    bcdsAddress: string;
    noOfMember: string;
}
export interface IInvestmentSociety {
    id: number;
    investmentInitId: number;
    societyId: number;
    societyAddress: string;
    noOfMember: string;
    
}
 
export class InvestmentSociety implements IInvestmentSociety {
    id: number=0;
    investmentInitId: number;
    societyId: number=null;
    societyAddress: string;
    noOfMember: string;
}
