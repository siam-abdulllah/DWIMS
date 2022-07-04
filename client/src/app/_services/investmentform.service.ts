import { InvestmentInitPagination, IInvestmentInitPagination } from '../shared/models/investmentPagination';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import {
  InvestmentForm, IInvestmentForm, InvestmentMedicineProd, InvestmentDetail, IInvestmentDetail, InvestmentTargetedProd, InvestmentOther
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
  investmentDoctorFormData: InvestmentDoctor = new InvestmentDoctor();
  investmentInstitutionFormData: InvestmentInstitution = new InvestmentInstitution();
  investmentCampaignFormData: InvestmentCampaign = new InvestmentCampaign();
  investmentBcdsFormData: InvestmentBcds = new InvestmentBcds();
  investmentSocietyFormData: InvestmentSociety = new InvestmentSociety();
  investmentOtherFormData: InvestmentOther = new InvestmentOther();
  baseUrl = environment.apiUrl;
  genParams = new GenericParams();


  constructor(private http: HttpClient, private router: Router) { }
  async getBudget(sbu: string, empID: number, donationId: number, proposeFor: any, userRole: any) {
    if (proposeFor == 'Sales') {
      if (userRole == 'M' || userRole == 'RSM' || userRole == 'DSM') {
        return this.http.get(this.baseUrl + 'approvalCeiling/getBudgetCeilingForRapidSales/' + empID + '/' + sbu + '/' + donationId).toPromise();
      }
      else if (userRole == 'GM' || userRole == 'Director' || userRole == 'MD') {
        return this.http.get(this.baseUrl + 'approvalCeiling/getBudgetCeilingForRapidSalesNoSBU/' + empID + '/' + sbu + '/' + donationId).toPromise();
      }
      else {
        return this.http.get(this.baseUrl + 'approvalCeiling/getBudgetCeilingForRapidSales/' + empID + '/' + sbu + '/' + donationId).toPromise();
      }
    }
    else if (proposeFor == 'PMD') {
      return this.http.get(this.baseUrl + 'approvalCeiling/getBudgetCeilingForRapidPMD/' + empID + '/' + sbu + '/' + donationId).toPromise();
    }

  }
  getBudgetForCampaign(sbu: string, empID: number, donationId: number, campaignDtlId: number) {
    return this.http.get(this.baseUrl + 'approvalCeiling/getBudgetCeilingForCampaign/' + empID + '/' + sbu + '/' + donationId + '/' + campaignDtlId);
  }
  getDepot() {
    return this.http.get(this.baseUrl + 'employee/depotForInvestment');
  }
  getBrand(sbu: string) {
    return this.http.get(this.baseUrl + 'product/getBrand/' + sbu);
  }
  getProductByBrand(brandCode: string, sbu: string) {
    return this.http.get(this.baseUrl + 'product/getProductByBrand/' + brandCode + '/' + sbu);
  }
  getSBU() {
    return this.http.get(this.baseUrl + 'employee/getSBU');
  }
  getDonations() {
    return this.http.get(this.baseUrl + 'donation/allDonation');
  }
  getMarkets() {
    return this.http.get(this.baseUrl + 'employee/marketForInvestment');
  }
  getProduct(sbu: string) {

    return this.http.get(this.baseUrl + 'product/getProductForInvestment/' + sbu);

  }
  getInitiatorName(employeeId: any) {

    return this.http.get(this.baseUrl + 'employee/getInitiatorName/' + employeeId).toPromise();

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

  async getInstitutions(sbu: string) {
    return this.http.get(this.baseUrl + 'institution/institutionsForInvestmentRapid/' + sbu).toPromise();
  }
  async getDoctors(sbu: string) {
    return this.http.get(this.baseUrl + 'doctor/doctorsForInvestmentRapid/' + sbu).toPromise();
  }
  async getBcds() {
    return this.http.get(this.baseUrl + 'bcds/bcdsForInvestment').toPromise();
  }
  async getSociety() {
    return this.http.get(this.baseUrl + 'society/societyForInvestment').toPromise();
  }
  async getCampaignMsts(sbu: string) {
    return this.http.get(this.baseUrl + 'campaign/campaignMstsForInvestmentRapid/' + sbu).toPromise();
  }
  async getCampaignDtls(mstId: number) {
    return this.http.get(this.baseUrl + 'campaign/campaignDtlsForInvestment/' + mstId).toPromise();
  }

  async getCampaignDtlProducts(dtlId: number) {
    return this.http.get(this.baseUrl + 'campaign/campaignDtlProductsForInvestment/' + dtlId).toPromise();
  }
  getSubCampaigns() {
    return this.http.get(this.baseUrl + 'campaign/getCampaignForReport');
  }
  async getInvestmentDoctors(investmentInitId: number) {
    return this.http.get(this.baseUrl + 'investment/investmentDoctors/' + investmentInitId).toPromise();
  }
  getInvestmentMedicineProds(investmentInitId: number, sbu: string) {
    return this.http.get(this.baseUrl + 'investment/investmentMedicineProds/' + investmentInitId + '/' + sbu);
  }

  getLastFiveInvestment(marketCode: string, toDayDate: string) {
    return this.http.get(this.baseUrl + 'investment/getLastFiveInvestment/' + marketCode + '/' + toDayDate).toPromise();
  }
  getLastFiveInvestmentForDoc(donationId: number, docId: number, marketCode: string, toDayDate: string) {
    return this.http.get(this.baseUrl + 'investment/getLastFiveInvestmentForDoc/' + donationId + '/' + docId + '/' + marketCode + '/' + toDayDate).toPromise();
  }
  getLastFiveInvestmentForInstitute(donationId: number, instituteId: number, marketCode: string, toDayDate: string) {
    return this.http.get(this.baseUrl + 'investment/getLastFiveInvestmentForInstitute/' + donationId + '/' + instituteId + '/' + marketCode + '/' + toDayDate).toPromise();
  }
  getLastFiveInvestmentForCampaign(donationId: number, campaignId: number, marketCode: string, toDayDate: string) {
    return this.http.get(this.baseUrl + 'investment/getLastFiveInvestmentForCampaign/' + donationId + '/' + campaignId + '/' + marketCode + '/' + toDayDate).toPromise();
  }
  getLastFiveInvestmentForBcds(donationId: number, bcdsId: number, marketCode: string, toDayDate: string) {
    return this.http.get(this.baseUrl + 'investment/getLastFiveInvestmentForBcds/' + donationId + '/' + bcdsId + '/' + marketCode + '/' + toDayDate).toPromise();
  }
  getLastFiveInvestmentForSociety(donationId: number, societyId: number, marketCode: string, toDayDate: string) {
    return this.http.get(this.baseUrl + 'investment/getLastFiveInvestmentForSociety/' + donationId + '/' + societyId + '/' + marketCode + '/' + toDayDate).toPromise();
  }
  getInvestmentTargetedGroups(investmentInitId: number) {
    return this.http.get(this.baseUrl + 'investment/investmentTargetedGroups/' + investmentInitId);
  }
  async getInvestmentInstitutions(investmentInitId: number) {
    return this.http.get(this.baseUrl + 'investment/investmentInstitutions/' + investmentInitId).toPromise();
  }
  async getInvestmentCampaigns(investmentInitId: number) {
    return this.http.get(this.baseUrl + 'investment/investmentCampaigns/' + investmentInitId).toPromise();
  }
  async getInvestmentBcds(investmentInitId: number) {
    return this.http.get(this.baseUrl + 'investment/investmentBcds/' + investmentInitId).toPromise();
  }
  async getInvestmentSociety(investmentInitId: number) {
    return this.http.get(this.baseUrl + 'investment/investmentSociety/' + investmentInitId).toPromise();
  }
  async getInvestmentOther(investmentInitId: number) {
    return this.http.get(this.baseUrl + 'investmentRapid/investmentOther/' + investmentInitId).toPromise();
  }
  getInvestmentDetails(investmentInitId: number) {
    return this.http.get(this.baseUrl + 'investment/investmentDetails/' + investmentInitId);
  }
  getGenParams() {
    return this.genParams;
  }
  setGenParams(genParams: GenericParams) {
    this.genParams = genParams;
  }
  getInvestmentInit(empId: number, sbu: string, userRole: string) {
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
    return this.http.get(this.baseUrl + 'investment/investmentInits/' + empId + '/' + sbu + '/' + userRole);
  }
  getInvestmentRapids(empId: number, from: string, For: string) {
    debugger;
    return this.http.get(this.baseUrl + 'investmentRapid/getInvestmentRapids/' + empId + '/' + from + '/' + For);
  }
  getInvestmentmedicineProducts(invInitId: number) {
    return this.http.get(this.baseUrl + 'investmentRapid/getInvestmentmedicineProducts/' + invInitId).toPromise();
  }
  getEmployeesforRapid() {
    return this.http.get(this.baseUrl + 'investmentrapid/employeesForRapid');

  }
  getEmployeesforRapidByDpt(proposeFor: any, sbu: any, empId: any) {
    return this.http.get(this.baseUrl + 'investmentrapid/getEmployeesforRapidByDpt/' + proposeFor + '/' + sbu + '/' + empId);

  }
  getEmployeesforRapidByCamp(subCampaignId: any, empId: any) {
    return this.http.get(this.baseUrl + 'investmentrapid/getEmployeesforRapidByCamp/' + subCampaignId + '/' + empId).toPromise();

  }
  getEmployeesforRapidBySBU(proposeFor: any, sbu: any, empId: any) {
    return this.http.get(this.baseUrl + 'investmentrapid/getEmployeesforRapidBySBU/' + proposeFor + '/' + sbu + '/' + empId).toPromise();

  }
  getInvestmentTargetedProds(investmentInitId: number, sbu: string) {
    return this.http.get(this.baseUrl + 'investmentrapid/investmentTargetedProds/' + investmentInitId + '/' + sbu);
  }
  getRapidSubCampaigns(sbu: string) {
    return this.http.get(this.baseUrl + 'investmentrapid/getRapidSubCampaigns/' + sbu);
  }
  submitInvestment(empId: any) {
    return this.http.post(this.baseUrl + 'investmentrapid/saveInvestmentRapid/' + empId, this.investmentFormData);
  }
  submitInvestmentAppr(empId: any) {
    return this.http.post(this.baseUrl + 'investmentrapid/saveInvestmentRapidAppr/' + empId, this.investmentFormData);
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
}

