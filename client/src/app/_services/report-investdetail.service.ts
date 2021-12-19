import { InvestmentInitPagination, IInvestmentInitPagination } from '../shared/models/investmentPagination';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import {
  InvestmentInit, IInvestmentInit, InvestmentDetail, IInvestmentDetail,
  InvestmentTargetedProd, IInvestmentTargetedProd, InvestmentTargetedGroup, IInvestmentTargetedGroup
} from '../shared/models/investment';
import { InvestmentInstitution, IInvestmentInstitution, InvestmentCampaign, IInvestmentCampaign } from '../shared/models/investmentRcv';
import { InvestmentBcds, IInvestmentBcds, InvestmentSociety, IInvestmentSociety } from '../shared/models/investmentRcv';
import { InvestmentDoctor, IInvestmentDoctor} from '../shared/models/investmentRec';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { BehaviorSubject, ReplaySubject, of, from } from 'rxjs';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router';
import { GenericParams } from '../shared/models/genericParams';
import { InvestmentRcvComment } from '../shared/models/investmentRcv';

@Injectable({
  providedIn: 'root'
})
export class RptInvestmentDetailService {
  investmentInits: IInvestmentInit[] = [];
  investmentInitPagination = new InvestmentInitPagination();
  investmentRcvFormData: InvestmentInit = new InvestmentInit();
  investmentInitFormData: InvestmentInit = new InvestmentInit();
  investmentRcvCommentFormData: InvestmentRcvComment = new InvestmentRcvComment();
  investmentDetailFormData: InvestmentDetail = new InvestmentDetail();
  investmentTargetedProdFormData: InvestmentTargetedProd = new InvestmentTargetedProd();
  investmentTargetedGroupFormData: InvestmentTargetedGroup = new InvestmentTargetedGroup();
  investmentDoctorFormData: InvestmentDoctor = new InvestmentDoctor();
  investmentInstitutionFormData: InvestmentInstitution = new InvestmentInstitution();
  investmentCampaignFormData: InvestmentCampaign = new InvestmentCampaign();
  investmentBcdsFormData: InvestmentBcds = new InvestmentBcds();
  investmentSocietyFormData: InvestmentSociety = new InvestmentSociety();
  baseUrl = environment.apiUrl;
  genParams = new GenericParams();


  constructor(private http: HttpClient, private router: Router) { }

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
  getInstitutions(marketCode:string) {
    return this.http.get(this.baseUrl + 'institution/institutionsForInvestment/'+marketCode);
  }
  getDoctors(marketCode:string) {
    return this.http.get(this.baseUrl + 'doctor/doctorsForInvestment/'+marketCode);
  }
  getBcds() {
    return this.http.get(this.baseUrl + 'bcds/bcdsForInvestment');
  }
  getSociety() {
    return this.http.get(this.baseUrl + 'society/societyForInvestment');
  }
  getCampaignMsts() {
    return this.http.get(this.baseUrl + 'campaign/campaignMstsForInvestment');
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
  getInvestmentDoctors(investmentInitId:number){    
    return this.http.get(this.baseUrl + 'investment/investmentDoctors/'+investmentInitId);
  }
  getInvestmentTargetedProds(investmentInitId: number, sbu: string) {
    return this.http.get(this.baseUrl + 'investment/investmentTargetedProds/' + investmentInitId + '/' + sbu);
  }
  getLastFiveInvestment(marketCode: string, toDayDate: string) {
    return this.http.get(this.baseUrl + 'investment/getLastFiveInvestment/' + marketCode + '/' + toDayDate);
  }
  getLastFiveInvestmentForDoc(donationId:number,docId:number,marketCode: string, toDayDate: string) {
    return this.http.get(this.baseUrl + 'investment/getLastFiveInvestmentForDoc/' + donationId + '/' + docId + '/' +marketCode + '/' +toDayDate);
  }
  getLastFiveInvestmentForInstitute(donationId:number,instituteId:number,marketCode: string, toDayDate: string) {
    return this.http.get(this.baseUrl + 'investment/getLastFiveInvestmentForInstitute/' + donationId + '/' + instituteId + '/' +marketCode + '/' +toDayDate);
  }
  getLastFiveInvestmentForCampaign(donationId:number,campaignId:number,marketCode: string, toDayDate: string) {
    return this.http.get(this.baseUrl + 'investment/getLastFiveInvestmentForCampaign/' + donationId + '/' + campaignId + '/' +marketCode + '/' +toDayDate);
  }
  getLastFiveInvestmentForBcds(donationId:number,bcdsId:number,marketCode: string, toDayDate: string) {
    return this.http.get(this.baseUrl + 'investment/getLastFiveInvestmentForBcds/' + donationId + '/' + bcdsId + '/' +marketCode + '/' +toDayDate);
  }
  getLastFiveInvestmentForSociety(donationId:number,societyId:number,marketCode: string, toDayDate: string) {
    return this.http.get(this.baseUrl + 'investment/getLastFiveInvestmentForSociety/' + donationId + '/' + societyId + '/' +marketCode + '/' +toDayDate);
  }
  getInvestmentTargetedGroups(investmentInitId: number) {
    return this.http.get(this.baseUrl + 'investment/investmentTargetedGroups/' + investmentInitId);
  }

  getInvestmentRcvComment(investmentInitId:number){    
    return this.http.get(this.baseUrl + 'investmentRecv/getInvestmentRecvComment/'+investmentInitId);
  }
  getInvestmentTargetedGroupStatus(investmentInitId:number,empId:number){    
    //return this.http.get(this.baseUrl + 'investment/investmentTargetedGroups/'+investmentInitId);
    return this.http.get(this.baseUrl + 'InvestmentRec/investmentTargetedGroups/'+investmentInitId+'/'+empId);
  }
  getInvestmentInstitutions(investmentInitId:number){    
    return this.http.get(this.baseUrl + 'investment/investmentInstitutions/'+investmentInitId);
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
  getInvestmentDetails(investmentInitId: number) {
    return this.http.get(this.baseUrl + 'investment/investmentDetails/' + investmentInitId);
  }
  getInvestmentInit(id: number) {
    let params = new HttpParams();
    if (this.genParams.search) {
      params = params.append('search', this.genParams.search);
    }
    params = params.append('sort', this.genParams.sort);
    params = params.append('pageIndex', this.genParams.pageIndex.toString());
    params = params.append('pageSize', this.genParams.pageSize.toString());
    return this.http.get<IInvestmentInitPagination>(this.baseUrl + 'ReportInvestment/investmentInits/' + id,  { observe: 'response', params })
      //return this.http.get<IDonationPagination>(this.baseUrl + 'donation/donations', { observe: 'response', params })
      .pipe(
        map(response => {
          this.investmentInits = [...this.investmentInits, ...response.body.data];
          this.investmentInitPagination = response.body;
          return this.investmentInitPagination;
        })
      );

  }

  submitInvestment() {
    return this.http.post(this.baseUrl + 'investment/submitInvestment', this.investmentInitFormData);
  }
  insertInvestmentInit() {
    return this.http.post(this.baseUrl + 'investment/insertInit', this.investmentInitFormData);
  }

  updateInvestmentInit() {
    return this.http.post(this.baseUrl + 'investment/updateInit', this.investmentInitFormData);
  }
  updateInvestmentInitOther(empId: number) {
    return this.http.post(this.baseUrl + 'investment/updateInitOther/' + empId, this.investmentInitFormData);
  }
  insertInvestmentDetail() {
    return this.http.post(this.baseUrl + 'investment/insertDetail', this.investmentDetailFormData);
  }

  updateInvestmentDetail() {
    return this.http.post(this.baseUrl + 'investment/updateDetail', this.investmentDetailFormData);
  }
  insertInvestmentDoctor() {
    return this.http.post(this.baseUrl + 'investment/insertInvestmentDoctor', this.investmentDoctorFormData);

  }
  insertInvestmentInstitution() {
    return this.http.post(this.baseUrl + 'investment/insertInvestmentInstitution', this.investmentInstitutionFormData);

  }
  insertInvestmentCampaign() {
    return this.http.post(this.baseUrl + 'investment/insertInvestmentCampaign', this.investmentCampaignFormData);

  }
  insertInvestmentBcds() {
    return this.http.post(this.baseUrl + 'investment/insertInvestmentBcds', this.investmentBcdsFormData);

  }
  insertInvestmentSociety() {
    return this.http.post(this.baseUrl + 'investment/insertInvestmentSociety', this.investmentSocietyFormData);

  }
  insertInvestmentTargetedProd() {
    return this.http.post(this.baseUrl + 'investment/insertInvestmentTargetedProd', this.investmentTargetedProdFormData);

  }
  updateInvestmentTargetedProd() {
    return this.http.post(this.baseUrl + 'investment/updateInvestmentTargetedProd', this.investmentTargetedProdFormData);

  }
  insertInvestmentTargetedGroup(investmentTargetedGroups: IInvestmentTargetedGroup[]) {
    return this.http.post(this.baseUrl + 'investment/insertInvestmentTargetedGroup', investmentTargetedGroups,
      { responseType: 'text' });

  }
  removeInvestmentDoctor() {
    return this.http.post(this.baseUrl + 'investment/removeInvestmentDoctor', this.investmentDoctorFormData,
      { responseType: 'text' });

  }
  removeInvestmentInstitution() {
    return this.http.post(this.baseUrl + 'investment/removeInvestmentInstitution', this.investmentInstitutionFormData,
      { responseType: 'text' });

  }
  removeInvestmentCampaign() {
    return this.http.post(this.baseUrl + 'investment/removeInvestmentCampaign', this.investmentCampaignFormData,
      { responseType: 'text' });

  }
  removeInvestmentBcds() {
    return this.http.post(this.baseUrl + 'investment/removeInvestmentBcds', this.investmentBcdsFormData,
      { responseType: 'text' });

  }
  removeInvestmentSociety() {
    return this.http.post(this.baseUrl + 'investment/removeInvestmentSociety', this.investmentSocietyFormData,
      { responseType: 'text' });

  }
  removeInvestmentTargetedProd() {
    return this.http.post(this.baseUrl + 'investment/removeInvestmentTargetedProd', this.investmentTargetedProdFormData,
      { responseType: 'text' });

  }
  removeInvestmentTargetedGroup(investmentTargetedGroups: IInvestmentTargetedGroup[]) {
    ;
    return this.http.post(this.baseUrl + 'investment/removeInvestmentTargetedGroup', investmentTargetedGroups,
      { responseType: 'text' });

  }
}

