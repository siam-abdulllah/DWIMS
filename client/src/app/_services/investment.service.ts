import { InvestmentInitPagination, IInvestmentInitPagination } from '../shared/models/investmentPagination';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import {
  InvestmentInit, IInvestmentInit, InvestmentDetail, IInvestmentDetail,
  InvestmentTargetedProd, IInvestmentTargetedProd, InvestmentTargetedGroup, IInvestmentTargetedGroup, InvestmentMedicineProd
} from '../shared/models/investment';
import { InvestmentDoctor, IInvestmentDoctor, InvestmentInstitution, IInvestmentInstitution, InvestmentCampaign, IInvestmentCampaign } from '../shared/models/investment';
import { InvestmentBcds, IInvestmentBcds, InvestmentSociety, IInvestmentSociety } from '../shared/models/investment';

import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { BehaviorSubject, ReplaySubject, of, from } from 'rxjs';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router';
import { GenericParams } from '../shared/models/genericParams';

@Injectable({
  providedIn: 'root'
})
export class InvestmentInitService {
  investmentInits: IInvestmentInit[] = [];
  investmentInitPagination = new InvestmentInitPagination();
  investmentInitFormData: InvestmentInit = new InvestmentInit();
  investmentDetailFormData: InvestmentDetail = new InvestmentDetail();
  investmentMedicineProdFormData: InvestmentMedicineProd = new InvestmentMedicineProd();
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
    if (sbu == null) {
      return this.http.get(this.baseUrl + 'product/getProductForInvestment');
    }
    else {
      return this.http.get(this.baseUrl + 'product/getProductForInvestment/' + sbu);
    }
  }
  getMedicineProduct() {
      return this.http.get(this.baseUrl + 'product/getMedicineProductForInvestment');
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
  getCampaignMsts(empId:number) {
    return this.http.get(this.baseUrl + 'campaign/campaignMstsForInvestment/'+empId);
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
  getInvestmentMedicineProds(investmentInitId: number, sbu: string) {
    return this.http.get(this.baseUrl + 'investment/investmentMedicineProds/' + investmentInitId + '/' + sbu);
  }
  getInvestmentTargetedProds(investmentInitId: number, sbu: string) {
    return this.http.get(this.baseUrl + 'investment/investmentTargetedProds/' + investmentInitId + '/' + sbu);
  }
  getLastFiveInvestment(marketCode: string, toDayDate: string) {
    return this.http.get(this.baseUrl + 'investment/getLastFiveInvestment/' + marketCode + '/' + toDayDate).toPromise();
  }
  getLastFiveInvestmentForDoc(donationId:number,docId:number,marketCode: string, toDayDate: string) {
    return this.http.get(this.baseUrl + 'investment/getLastFiveInvestmentForDoc/' + donationId + '/' + docId + '/' +marketCode + '/' +toDayDate).toPromise();
  }
  getLastFiveInvestmentForInstitute(donationId:number,instituteId:number,marketCode: string, toDayDate: string) {
    return this.http.get(this.baseUrl + 'investment/getLastFiveInvestmentForInstitute/' + donationId + '/' + instituteId + '/' +marketCode + '/' +toDayDate).toPromise();
  }
  getLastFiveInvestmentForCampaign(donationId:number,campaignId:number,marketCode: string, toDayDate: string) {
    return this.http.get(this.baseUrl + 'investment/getLastFiveInvestmentForCampaign/' + donationId + '/' + campaignId + '/' +marketCode + '/' +toDayDate).toPromise();
  }
  getLastFiveInvestmentForBcds(donationId:number,bcdsId:number,marketCode: string, toDayDate: string) {
    return this.http.get(this.baseUrl + 'investment/getLastFiveInvestmentForBcds/' + donationId + '/' + bcdsId + '/' +marketCode + '/' +toDayDate).toPromise();
  }
  getLastFiveInvestmentForSociety(donationId:number,societyId:number,marketCode: string, toDayDate: string) {
    return this.http.get(this.baseUrl + 'investment/getLastFiveInvestmentForSociety/' + donationId + '/' + societyId + '/' +marketCode + '/' +toDayDate).toPromise();
  }
  getInvestmentTargetedGroups(investmentInitId: number) {
    return this.http.get(this.baseUrl + 'investment/investmentTargetedGroups/' + investmentInitId);
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
  getInvestmentDetails(investmentInitId: number) {
    return this.http.get(this.baseUrl + 'investment/investmentDetails/' + investmentInitId);
  }
  getGenParams(){
    return this.genParams;
  }
  setGenParams(genParams: GenericParams) {
    this.genParams = genParams;
  }
  getInvestmentInit(empId: number, sbu: string,userRole:string) {
    // let params = new HttpParams();
    // if (this.genParams.search) {
    //   params = params.append('search', this.genParams.search);
    // }
    // params = params.append('sort', this.genParams.sort);
    // params = params.append('pageIndex', this.genParams.pageIndex.toString());
    // params = params.append('pageSize', this.genParams.pageSize.toString());
    // return this.http.get<IInvestmentInitPagination>(this.baseUrl + 'investment/investmentInits/' + empId + '/' + sbu+'/'+userRole, { observe: 'response', params })
    //   //return this.http.get<IDonationPagination>(this.baseUrl + 'donation/donations', { observe: 'response', params })
    //   .pipe(
    //     map(response => {
    //       this.investmentInits = [...this.investmentInits, ...response.body.data];
    //       this.investmentInitPagination = response.body;
    //       return this.investmentInitPagination;
    //     })
    //   );
    return this.http.get(this.baseUrl + 'investment/investmentInits/' + empId + '/' + sbu+'/'+userRole);
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
  insertInvestmentMedicineProd() {
    return this.http.post(this.baseUrl + 'investment/insertInvestmentMedicineProd', this.investmentMedicineProdFormData);
  }
  updateInvestmentTargetedProd() {
    return this.http.post(this.baseUrl + 'investment/updateInvestmentTargetedProd', this.investmentTargetedProdFormData);

  }
  //insertInvestmentTargetedGroup(investmentTargetedGroups: IInvestmentTargetedGroup[],initId:number) {
  insertInvestmentTargetedGroup(mktGrpMstId:number,initId:number) {
    return this.http.get(this.baseUrl + 'investment/insertInvestmentTargetedGroup/'+initId+'/'+mktGrpMstId,
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
  removeInvestmentMedicineProd() {
    return this.http.post(this.baseUrl + 'investment/removeInvestmentMedicineProd', this.investmentMedicineProdFormData,
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
  removeInvestmentIndTargetedGroup(investmentTargetedGroup: InvestmentTargetedGroup) {
    ;
    return this.http.post(this.baseUrl + 'investment/removeInvestmentIndTargetedGroup', investmentTargetedGroup,
      { responseType: 'text' });

  }
}

