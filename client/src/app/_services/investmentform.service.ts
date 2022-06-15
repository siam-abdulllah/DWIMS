import { InvestmentInitPagination, IInvestmentInitPagination } from '../shared/models/investmentPagination';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import {
   InvestmentForm,IInvestmentForm,InvestmentMedicineProd ,InvestmentDetail, IInvestmentDetail,InvestmentTargetedProd
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
export class InvestmentFormService {
  IInvestmentForm: IInvestmentForm[] = [];
  investmentTargetedProdFormData: InvestmentTargetedProd = new InvestmentTargetedProd();
  investmentFormData: InvestmentForm = new InvestmentForm();
  investmentMedicineProdFormData: InvestmentMedicineProd = new InvestmentMedicineProd();
  investmentDetailFormData: InvestmentDetail = new InvestmentDetail();
  baseUrl = environment.apiUrl;
  genParams = new GenericParams();


  constructor(private http: HttpClient, private router: Router) { }
  getBudget(sbu: string, empID: number, donationId: number,proposeFor:any) {
    if(proposeFor=='Sales')
    {
      return this.http.get(this.baseUrl + 'approvalCeiling/getBudgetCeilingForRapidSales/' + empID + '/' + sbu + '/' + donationId);
    }
    else if(proposeFor=='PMD'){
      return this.http.get(this.baseUrl + 'approvalCeiling/getBudgetCeilingForRapidPMD/' + empID + '/' + sbu + '/' + donationId);
    }
    
  }
  getBudgetForCampaign(sbu: string, empID: number, donationId: number, campaignDtlId: number) {
    debugger;
    return this.http.get(this.baseUrl + 'approvalCeiling/getBudgetCeilingForCampaign/' + empID + '/' + sbu + '/' + donationId + '/' + campaignDtlId);
  }
  getDepot() {
    return this.http.get(this.baseUrl + 'employee/depotForInvestment');
  }
  getBrand(sbu:string){    
    return this.http.get(this.baseUrl + 'product/getBrand/'+sbu);
  }
  getProductByBrand(brandCode:string,sbu:string){    
    return this.http.get(this.baseUrl + 'product/getProductByBrand/'+brandCode+'/'+sbu);
  }
  getSBU(){    
    return this.http.get(this.baseUrl + 'employee/getSBU');
  }
  getDonations() {
    return this.http.get(this.baseUrl + 'donation/donationsForInvestment');
  }
  getMarkets() {
    return this.http.get(this.baseUrl + 'employee/marketForInvestment');
  }
  getProduct(sbu: string) {
  
      return this.http.get(this.baseUrl + 'product/getProductForInvestment/' + sbu);
    
  }
  getInitiatorName(employeeId: any) {
  
      return this.http.get(this.baseUrl + 'employee/getInitiatorName/' + employeeId);
    
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
 

insertInvestmentMedicineProd() {
  return this.http.post(this.baseUrl + 'investment/insertInvestmentMedicineProd', this.investmentMedicineProdFormData);
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
    return this.http.get(this.baseUrl + 'campaign/getCampaignForReport');
  }
  getInvestmentDoctors(investmentInitId: number) {
    return this.http.get(this.baseUrl + 'investment/investmentDoctors/' + investmentInitId);
  }
  getInvestmentMedicineProds(investmentInitId: number, sbu: string) {
    return this.http.get(this.baseUrl + 'investment/investmentMedicineProds/' + investmentInitId + '/' + sbu);
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
  getInvestmentRapids(empId: number,from:string,For:string){    
    debugger;
    return this.http.get(this.baseUrl + 'investmentRapid/getInvestmentRapids/' + empId  + '/' + from+'/'+For);
  }
  getInvestmentmedicineProducts(invInitId: number){    
    debugger;
    return this.http.get(this.baseUrl + 'investmentRapid/getInvestmentmedicineProducts/' + invInitId );
  }
  getEmployeesforrapid(){    
    return this.http.get(this.baseUrl + 'investmentrapid/employeesForRapid');
   
  }
  getEmployeesforRapidByDpt(proposeFor:any,sbu:any){    
    return this.http.get(this.baseUrl + 'investmentrapid/getEmployeesforRapidByDpt/'+proposeFor+'/'+sbu);
   
  }
  getEmployeesforRapidByCamp(subCampaignId:any){    
    return this.http.get(this.baseUrl + 'investmentrapid/getEmployeesforRapidByCamp/'+subCampaignId);
   
  }
  getEmployeesforRapidBySBU(proposeFor:any,sbu:any){    
    return this.http.get(this.baseUrl + 'investmentrapid/getEmployeesforRapidBySBU/'+proposeFor+'/'+sbu);
   
  }
  getInvestmentTargetedProds(investmentInitId: number, sbu: string) {
    return this.http.get(this.baseUrl + 'investmentrapid/investmentTargetedProds/' + investmentInitId + '/' + sbu);
  }
  getRapidSubCampaigns(sbu:string) {
    return this.http.get(this.baseUrl + 'investmentrapid/getRapidSubCampaigns/' + sbu);
  }
  submitInvestment() {
    debugger;
    return this.http.post(this.baseUrl + 'investmentrapid/saveInvestmentRapid', this.investmentFormData);
  }
}

