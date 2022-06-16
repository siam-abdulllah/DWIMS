import { InvestmentAprPagination, IInvestmentAprPagination } from '../shared/models/investmentAprPagination';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import {
  InvestmentApr, IInvestmentApr,
  InvestmentTargetedProd, IInvestmentTargetedProd, InvestmentTargetedGroup, IInvestmentTargetedGroup, InvestmentAprComment, IInvestmentAprComment, InvestmentInitForApr
} from '../shared/models/investmentApr';
import { InvestmentDoctor, IInvestmentDoctor, InvestmentInstitution, IInvestmentInstitution, InvestmentCampaign, IInvestmentCampaign } from '../shared/models/investmentApr';
import { InvestmentBcds, IInvestmentBcds, InvestmentSociety, IInvestmentSociety } from '../shared/models/investmentApr';

import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { BehaviorSubject, ReplaySubject, of, from } from 'rxjs';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router';
import { GenericParams } from '../shared/models/genericParams';
import { IInvestmentInitPagination, InvestmentInitPagination } from '../shared/models/investmentPagination';
import { InvestmentRecDepot } from '../shared/models/InvestmentRecDepot';
import { InvestmentMedicineProd } from '../shared/models/investment';

@Injectable({
  providedIn: 'root'
})
export class InvestmentCancelService {
  investmentAprs: IInvestmentApr[] = [];
  investmentAprPagination = new InvestmentAprPagination();
  investmentInits: IInvestmentInit[] = [];
  investmentInitPagination = new InvestmentInitPagination();
  investmentDepotFormData: InvestmentRecDepot = new InvestmentRecDepot();
  investmentCancelFormData: InvestmentInit = new InvestmentInit();
  investmentDetailFormData: InvestmentApr = new InvestmentApr();
  investmentCancelCommentFormData: InvestmentAprComment = new InvestmentAprComment();
  investmentMedicineProdFormData: InvestmentMedicineProd = new InvestmentMedicineProd();
  investmentTargetedProdFormData: InvestmentTargetedProd = new InvestmentTargetedProd();
  investmentTargetedGroupFormData: InvestmentTargetedGroup = new InvestmentTargetedGroup();
  investmentDoctorFormData: InvestmentDoctor = new InvestmentDoctor();
  investmentInstitutionFormData: InvestmentInstitution = new InvestmentInstitution();
  investmentCampaignFormData: InvestmentCampaign = new InvestmentCampaign();
  investmentBcdsFormData: InvestmentBcds = new InvestmentBcds();
  investmentSocietyFormData: InvestmentSociety = new InvestmentSociety();
  //investmentRcvFormData: InvestmentInit = new InvestmentInit();
  baseUrl = environment.apiUrl;
  genParams = new GenericParams();


  constructor(private http: HttpClient, private router: Router) { }
  GetInvestmentSummarySingleDoc(referenceNo: string){ 
  
    if(referenceNo==undefined || referenceNo=="")
    {
      referenceNo="undefined";
    }
    return this.http.get(this.baseUrl+ 'reportInvestment/getInvestmentSummaryCancel/'+referenceNo);  
  }
  getEmpLoc(initId: any) {
    debugger;
    return this.http.get(this.baseUrl + 'InvestmentAprNoSbu/getEmpMarket/' + initId);
  }
  getDonations() {
    return this.http.get(this.baseUrl + 'donation/donationsForInvestment');
  }
  getMarkets() {
    return this.http.get(this.baseUrl + 'employee/marketForInvestment');
  }
  getProduct(sbu: string) {
    if (sbu == 'All') {
      return this.http.get(this.baseUrl + 'product/getProductForInvestment');
    }
    else {
      return this.http.get(this.baseUrl + 'product/getProductForInvestment/' + sbu);
    }
  }
  getMarketGroupMsts(empId: string) {
    return this.http.get(this.baseUrl + 'marketGroup/getMarketGroupMstsForInvestment/' + empId);
  }
  getApprovalAuthority() {
    return this.http.get(this.baseUrl + 'approvalAuthority/approvalAuthoritiesForConfig');
  }
  getEmployees() {
    return this.http.get(this.baseUrl + 'employee/employeesForInvestment');
  }
  getInstitutions(marketCode: string) {
    return this.http.get(this.baseUrl + 'institution/institutionsForInvestment/' + marketCode);
  }
  getDoctors(marketCode: string) {
    return this.http.get(this.baseUrl + 'doctor/doctorsForInvestment/' + marketCode);
  }
  getBcds() {
    return this.http.get(this.baseUrl + 'bcds/bcdsForInvestment');
  }
  getSociety() {
    return this.http.get(this.baseUrl + 'society/societyForInvestment');
  }
  getCampaignMsts(mstId: number) {
    return this.http.get(this.baseUrl + 'campaign/campaignMstsForInvSummaryReport/' + mstId);
  }
  getCampaignDtls(mstId: number) {
    return this.http.get(this.baseUrl + 'campaign/campaignDtlsForInvestment/' + mstId);
  }
  getCampaignDtlProducts(dtlId: number) {
    return this.http.get(this.baseUrl + 'campaign/campaignDtlProductsForInvestment/' + dtlId);
  }
  getSubCampaigns() {
    return this.http.get(this.baseUrl + 'campaign/subCampaignsForInvestment');
  }
  getInvestmentDoctors(investmentInitId: number) {
    return this.http.get(this.baseUrl + 'investment/investmentDoctors/' + investmentInitId);
  }
  getInvestmentTargetedProds(investmentInitId: number, sbu: string) {
    return this.http.get(this.baseUrl + 'investmentRec/investmentRecProducts/' + investmentInitId + '/' + sbu);
  }
  getLastFiveInvestment(marketCode: string, toDayDate: string) {
    return this.http.get(this.baseUrl + 'investment/getLastFiveInvestment/' + marketCode + '/' + toDayDate);
  }
  getLastFiveInvestmentForDoc(donationId: number, docId: number, marketCode: string, toDayDate: string) {
    return this.http.get(this.baseUrl + 'investment/getLastFiveInvestmentForDoc/' + donationId + '/' + docId + '/' + marketCode + '/' + toDayDate);
  }
  getLastFiveInvestmentForInstitute(donationId: number, instituteId: number, marketCode: string, toDayDate: string) {
    return this.http.get(this.baseUrl + 'investment/getLastFiveInvestmentForInstitute/' + donationId + '/' + instituteId + '/' + marketCode + '/' + toDayDate);
  }
  getLastFiveInvestmentForCampaign(donationId: number, campaignId: number, marketCode: string, toDayDate: string) {
    return this.http.get(this.baseUrl + 'investment/getLastFiveInvestmentForCampaign/' + donationId + '/' + campaignId + '/' + marketCode + '/' + toDayDate);
  }
  getLastFiveInvestmentForBcds(donationId: number, bcdsId: number, marketCode: string, toDayDate: string) {
    return this.http.get(this.baseUrl + 'investment/getLastFiveInvestmentForBcds/' + donationId + '/' + bcdsId + '/' + marketCode + '/' + toDayDate);
  }
  getLastFiveInvestmentForSociety(donationId: number, societyId: number, marketCode: string, toDayDate: string) {
    return this.http.get(this.baseUrl + 'investment/getLastFiveInvestmentForSociety/' + donationId + '/' + societyId + '/' + marketCode + '/' + toDayDate);
  }
  getInvestmentTargetedGroups(investmentInitId: number) {
    return this.http.get(this.baseUrl + 'InvestmentRec/investmentTargetedGroups/' + investmentInitId);
  }

  // getInvestmentRcvComment(investmentInitId:number){    
  //   return this.http.get(this.baseUrl + 'investmentRecv/getInvestmentRecvComment/'+investmentInitId);
  // }
  getInvestmentRcvComment(investmentInitId: number) {
    return this.http.get(this.baseUrl + 'investmentRecv/getInvestmentRecvCommentList/' + investmentInitId);
  }
  getInvestmentTargetedGroupStatus(investmentInitId: number) {
    //return this.http.get(this.baseUrl + 'investment/investmentTargetedGroups/'+investmentInitId);
    return this.http.get(this.baseUrl + 'ReportInvestment/investmentTargetedGroups/' + investmentInitId);
  }
  getInvestmentInstitutions(investmentInitId: number) {
    return this.http.get(this.baseUrl + 'investment/investmentInstitutions/' + investmentInitId);
  }
  getInvestmentCampaigns(investmentInitId: number) {
    return this.http.get(this.baseUrl + 'investment/investmentCampaigns/' + investmentInitId);
  }
  getInvestmentBcds(investmentInitId: number) {
    return this.http.get(this.baseUrl + 'investment/investmentBcds/' + investmentInitId);
  }
  getInvestmentSociety(investmentInitId: number) {
    return this.http.get(this.baseUrl + 'investment/investmentSociety/' + investmentInitId);
  }
  getInvestmentDetails(investmentInitId: number, empId: number, userRole: string) {
    return this.http.get(this.baseUrl + 'reportInvestment/investmentDetailsForSummary/' + investmentInitId + '/' + empId + '/' + userRole);
  }
  getInvestmentDetailTracker(investmentInitId: number) {
    return this.http.get(this.baseUrl + 'reportInvestment/investmentDetailTracker/' + investmentInitId);
  }
  getInvestmentInit(id: number) {
    return this.http.get(this.baseUrl + 'reportInvestment/investmentInits/' + id);
  }
  // removeInvestmentDetal(id: number) {
  //   return this.http.post(this.baseUrl + 'reportInvestment/removeInvestmentDetal/' + id,
  //   { responseType: 'text' });
  // }
  removeInvestmentDetail(id: number,empId:number) {
    return this.http.post(this.baseUrl + 'investmentCancel/removeInvestmentDetalTracker/'+id+'/'+empId,"", { responseType: 'text' });
  }

  cancelInv(invId: number,empId:number) {
    return this.http.post(this.baseUrl + 'investmentCancel/cancelInv/'+invId+'/'+empId,"",
    { responseType: 'text' });
  }
  isInvestmentDetailExist(id: number,empId:number) {
    debugger;
    //return this.http.post(this.baseUrl + 'reportInvestment/isInvestmentDetailExist/'+id+'/'+empId,
    return this.http.post(this.baseUrl + 'reportInvestment/isInvestmentDetailExist/'+id,"",
    { responseType: 'text' });
  }
}
export interface IInvestmentInit {
  id: number;
  dataStatus: number;
  referenceNo: string;
  proposeFor: string;
  donationTypeName: string;
  donationTo: string;
  marketCode: string;
  sbu: string;
  employeeId: number;
  employee: IEmployee;
  setOn: Date;
}
export class InvestmentInit implements IInvestmentInit {
  id: number = 0;
  dataStatus: number;
  referenceNo: string;
  proposeFor: string = null;
  donationTypeName: string;
  
  donationTo: string = null;
  marketCode: string;
  sbu: string;
  employeeId: number;
  employee: IEmployee;
  setOn: Date;
}
export interface IEmployee {
  id: number;
  employeeName: string;
  employeeCode: string;
  SBU: string;
  designationName: string;
  remarks: string;
  status: string;

}



