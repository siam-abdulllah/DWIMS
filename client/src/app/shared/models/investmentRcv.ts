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
    donationId: number;
    donation:IDonation;
    donationTo: string;
    marketCode: string;
    marketName: string;
    sbu: string;
    employeeId: number;
    approvedBy: string;
    approvedDate: Date;
    depotName: string;
}
 
export class InvestmentInit implements IInvestmentInit {
    id: number=0;
    referenceNo: string;
    proposeFor: string=null;
    donationId: number=null;
    donation:IDonation;
    donationTo: string=null;
    marketCode: string;
    marketName: string;
    sbu: string;
    employeeId: number;
    approvedBy: string;
    approvedDate: Date;
    depotName: string;
}


// export interface IInvestmentRcv {
//     id: number;
//     investmentInitId: number;
//     chequeTitle: string;
//     paymentMethod: string;
//     commitmentAllSBU: string;
//     commitmentOwnSBU: string;
//     shareAllSBU: string;
//     shareOwnSBU: string;
//     totalMonth: number;
//     proposedAmount: string;
//     purpose: string;
//     fromDate: Date;
//     toDate: Date;
// }
 
// export class InvestmentRcv implements IInvestmentRcv {
//     id: number=0;
//     investmentInitId: number;
//     chequeTitle: string;
//     paymentMethod: string=null;
//     commitmentAllSBU: string;
//     commitmentOwnSBU: string;
//     shareAllSBU: string;
//     shareOwnSBU: string;
//     totalMonth: number;
//     proposedAmount: string;
//     purpose: string;
//     fromDate: Date;
//     toDate: Date;
// }
export interface IInvestmentRcvComment {
    id: number;
    investmentInitId: number;
    comments: string;
    receiveStatus: string;
    employeeId: number;
}
 
export class InvestmentRcvComment implements InvestmentRcvComment {
    id: number=0;
    investmentInitId: number;
    comments: string;
    receiveStatus: string=null;
    employeeId: number;
}
export interface IInvestmentTargetedProd {
    id: number;
    investmentInitId: number;
    productId: number;
    productInfo:IProduct;
    employeeId: number;
}
 
export class InvestmentTargetedProd implements IInvestmentTargetedProd {
    id: number=0;
    investmentInitId: number;
    productId: number=null;
    productInfo:IProduct;
    employeeId: number;
}
export interface IInvestmentTargetedGroup {
    id: number;
    marketGroupMstId: number;
    investmentInitId: number;
    marketCode: string;
    marketName: string;
    recStatus: string;
    employeeName: string;
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
    employeeName: string;
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
    responsibleDoctorName: string;
    doctorInfo:IDoctor;
    bcds:IBcdsInfo;
    
}
 
export class InvestmentBcds implements IInvestmentBcds {
    id: number=0;
    investmentInitId: number;
    bcdsId: number=null;
    bcdsName: string;
    bcdsAddress: string;
    noOfMember: string;
    responsibleDoctorName: string;
    doctorInfo:IDoctor;
    bcds:IBcdsInfo;
}
export interface IInvestmentSociety {
    id: number;
    investmentInitId: number;
    societyId: number;
    societyName: string;
    societyAddress: string;
    noOfMember: string;
    responsibleDoctorName: string;
    doctorInfo:IDoctor;
    society:ISocietyInfo;
    
}
 
export class InvestmentSociety implements IInvestmentSociety {
    id: number=0;
    investmentInitId: number;
    societyId: number=null;
    societyName: string;
    societyAddress: string;
    noOfMember: string;
    responsibleDoctorName: string;
    doctorInfo:IDoctor;
    society:ISocietyInfo;
}



export interface IInvestmentRcv {
    id: number;
    investmentInitId: number;
    chequeTitle: string;
    paymentMethod: string;
    commitmentAllSBU: string;
    commitmentOwnSBU: string;
    shareAllSBU: string;
    shareOwnSBU: string;
    totalMonth: number;
    donationId: number;
    donation:IDonation;
    donationTo: string;
    proposedAmount: string;
    purpose: string;
    marketCode: string;
    sbu: string;
    fromDate: Date;
    toDate: Date;
}
 
export class InvestmentRcv implements IInvestmentRcv {
    id: number=0;
    investmentInitId: number;
    chequeTitle: string;
    paymentMethod: string=null;
    donationId: number=null;
    commitmentAllSBU: string;
    commitmentOwnSBU: string;
    shareAllSBU: string;
    shareOwnSBU: string;
    totalMonth: number;
    donation:IDonation;
    donationTo: string=null;
    proposedAmount: string;
    purpose: string;
    marketCode: string;
    sbu: string;
    fromDate: Date;
    toDate: Date;
}
