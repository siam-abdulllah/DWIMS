import { ICampaignDtl } from "./campaign";
import { IDonation } from "./donation";
import { IEmployee } from "./employee";
import { IMarketGroupMst } from "./marketGroupMst";
import { IMedicineProduct } from "./medicineProduct";
import { IProduct } from "./product";

export interface IInvestmentInit {
    id: number;
    referenceNo: string;
    proposeFor: string;
    donationTo: string;
    marketCode: string;
    marketName: string;
    employeeId: number;
    employee: IEmployee;
    confirmation: boolean;
    donationId: number;
    donation:IDonation;
    sbu:string;
    setOn: Date;
}
 
export class InvestmentInit implements IInvestmentInit {
    id: number=0;

    referenceNo: string;
    proposeFor: string=null;
    donationTo: string=null;

    marketCode: string;
    marketName: string;
    employeeId: number;
    employee: IEmployee;
    confirmation: boolean;
    donationId: number=null;
    donation:IDonation;
    sbu:string;
    setOn: Date;
}
export interface IInvestmentForm {
    id: number;
    investmentInitId:number;
    referenceNo: string;
    proposeFor: string;
    approvedStatus:string;
    type:string;
    donationTypeName:string;
    proposalDateStr:string;
    subCampaignId:number;
    subCampaignName:string;
    propsalDate:Date;
    depotCode:string;
    depotName:string;
    employee: IEmployee;
    proposedAmount:string,
    paymentMethod:string
    remarks:string;
    initiatorId: number;
    approverId: number;
    sbu:string;
    sbuName:string;
    donationTo: string;
    address:string;
    chequeTitle: string;
    approval:string;
    boxQuantity:number;
    investmentMedicineProd:InvestmentMedicineProd[];
    investmentRecProducts:InvestmentTargetedProd[];

}
export class InvestmentForm implements IInvestmentForm {
    id: number=0;
    investmentInitId:number;
    referenceNo: string;
    approvedStatus:string=null;
    type:string=null;
    donationTypeName:string;
    proposeFor: string=null;
    proposalDateStr:string;
    subCampaignId:number=0;
    subCampaignName:string;
    propsalDate:Date;
    depotCode:string;
    depotName:string;
    initiatorId: number;
    approverId: number;
    employee: IEmployee;
    proposedAmount:string;
    paymentMethod:string=null;
    remarks:string;
    sbu:string=null;
    sbuName:string;
    donationTo: string;
    address:string;
    chequeTitle: string;
    approval:string;
    boxQuantity:number;
    investmentMedicineProd:InvestmentMedicineProd[];
    investmentRecProducts:InvestmentTargetedProd[];
}


export interface IInvestmentDetail {
    id: number;
    investmentInitId: number;
    chequeTitle: string;
    paymentMethod: string;
    paymentFreq: string;
    commitmentAllSBU: string;
    commitmentOwnSBU: string;
    
    shareAllSBU: string;
    shareOwnSBU: string;
    totalMonth: number;
    commitmentTotalMonth: number;
    proposedAmount: string;
    purpose: string;
    fromDate: Date;
    toDate: Date;
    commitmentFromDate: any;
    commitmentToDate: any;
}
 
export class InvestmentDetail implements IInvestmentDetail {
    id: number=0;
    investmentInitId: number;
    chequeTitle: string;
    paymentMethod: string=null;
    paymentFreq: string=null;
    commitmentAllSBU: string;
    commitmentOwnSBU: string;
    shareAllSBU: string;
    shareOwnSBU: string;
    totalMonth: number;
    commitmentTotalMonth: number;
    proposedAmount: string;
    purpose: string;
    fromDate: any;
    toDate: any;
    commitmentFromDate: any;
    commitmentToDate: any;
}
export interface IInvestmentDetailOld {
    id: number;
    investmentInitId: number;
    chequeTitle: string;
    paymentMethod: string;
    comtSharePrcntAll: string;
    comtSharePrcnt: string;
    prescribedSharePrcntAll: string;
    prescribedSharePrcnt: string;
    totalMonth: number;
    investmentAmount: string;
    purpose: string;
    fromDate: any;
    toDate: any;
}
 
export class InvestmentDetailOld implements IInvestmentDetailOld {
    id: number=0;
    investmentInitId: number;
    chequeTitle: string;
    paymentMethod: string=null;
    comtSharePrcntAll: string;
    comtSharePrcnt: string;
    prescribedSharePrcntAll: string;
    prescribedSharePrcnt: string;
    totalMonth: number;
    investmentAmount: string;
    purpose: string;
    fromDate: Date;
    toDate: Date;
}
export interface ILastFiveInvestmentDetail {
    id: number;
    comtSharePrcntAll: string;
    comtSharePrcnt: string;
    prescribedSharePrcntAll: string;
    prescribedSharePrcnt: string;
    investmentAmount: string;
    donationShortName: string;
}
 
export class LastFiveInvestmentDetail implements ILastFiveInvestmentDetail {
    id: number;
    comtSharePrcntAll: string;
    comtSharePrcnt: string;
    prescribedSharePrcntAll: string;
    prescribedSharePrcnt: string;
    investmentAmount: string;
    donationShortName: string;
}
export interface IInvestmentMedicineProd {
    id: number;
    investmentInitId: number;
    productId: number;
    medicineProduct:IMedicineProduct;
    boxQuantity:number;
    tpVat: number;
    employeeId: number;
}
 
export class InvestmentMedicineProd implements IInvestmentMedicineProd {
    id: number=0;
    investmentInitId: number;
    productId: number;
    medicineProduct:IMedicineProduct;
    boxQuantity:number;
    tpVat: number;
    employeeId: number;
}
export interface IInvestmentTargetedProd {
    id: number;
    investmentInitId: number;
    productId: number;
    productInfo:IProduct;
    sbu:string;
    employeeId: number;
}
 
export class InvestmentTargetedProd implements IInvestmentTargetedProd {
    id: number=0;
    investmentInitId: number;
    productId: number=null;
    productInfo:IProduct;
    sbu:string;
    employeeId: number;
}
export interface IInvestmentTargetedGroup {
    marketGroupMstId: number;
    id: number;
    investmentInitId: number;
    completionStatus: boolean;
    marketCode: string;
    marketName: string;
    sbuName:string;
    sbu:string;
    marketGroupMst:IMarketGroupMst;
}
 
export class InvestmentTargetedGroup implements IInvestmentTargetedGroup {
    id: number=0;
    marketGroupMstId: number=null;
    investmentInitId: number;
    completionStatus: boolean;
    marketCode: string;
    marketName: string;
    sbuName:string;
    sbu:string;
    marketGroupMst:IMarketGroupMst;
}
export interface IInvestmentDoctor {
    id: number;
    investmentInitId: number;
    institutionId: number;
    doctorId: number;
    doctorName: string;
    doctorCode: string;
    //address: string;
    //degree: string;
    //designation: string;
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
    //degree: string;
    //designation: string;
    doctorCategory: string=null;
    doctorName: string=null;
    doctorCode: string;
    //address: string;
    doctorType: string=null;
    practiceDayPerMonth: string;
    patientsPerDay: string;
}
export interface IInvestmentInstitution {
    id: number;
    investmentInitId: number;
    institutionId: number;
    responsibleDoctorId: number;
    institutionType: string;
    address: string;
    noOfBed: string;
    departmentUnit: string;
    
}
 
export class InvestmentInstitution implements IInvestmentInstitution {
    id: number=0;
    investmentInitId: number;
    institutionId: number=null;
    responsibleDoctorId: number=null;
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
    responsibleDoctorId: number;
    
}
 
export class InvestmentBcds implements IInvestmentBcds {
    id: number=0;
    investmentInitId: number;
    bcdsId: number=null;
    bcdsAddress: string;
    noOfMember: string;
    responsibleDoctorId: number=null;
}
export interface IInvestmentSociety {
    id: number;
    investmentInitId: number;
    societyId: number;
    societyAddress: string;
    noOfMember: string;
    responsibleDoctorId: number;
    
}
 
export class InvestmentSociety implements IInvestmentSociety {
    id: number=0;
    investmentInitId: number;
    societyId: number=null;
    societyAddress: string;
    noOfMember: string;
    responsibleDoctorId: number=null;
}
