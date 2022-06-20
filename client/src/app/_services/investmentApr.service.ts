import { InvestmentAprPagination, IInvestmentAprPagination } from '../shared/models/investmentAprPagination';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import {
  InvestmentApr, IInvestmentApr, InvestmentInit, IInvestmentInit,
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
export class InvestmentAprService {
  investmentAprs: IInvestmentApr[] = [];
  investmentAprPagination = new InvestmentAprPagination();
  investmentInits: IInvestmentInit[] = [];
  investmentInitPagination = new InvestmentInitPagination();
  investmentDepotFormData: InvestmentRecDepot = new InvestmentRecDepot();
  investmentAprFormData: InvestmentInitForApr = new InvestmentInitForApr();
  investmentDetailFormData: InvestmentApr = new InvestmentApr();
  investmentAprCommentFormData: InvestmentAprComment = new InvestmentAprComment();
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
  getDepot() {
    return this.http.get(this.baseUrl + 'employee/depotForInvestment');
  }
  getProduct(sbu: string) {
    return this.http.get(this.baseUrl + 'product/getProductForInvestment/' + sbu);
  }
  getMedicineProduct() {
    return this.http.get(this.baseUrl + 'product/getMedicineProductForInvestment');
  }
  getBudget(sbu: string, empID: number, donationId: number) {
    return this.http.get(this.baseUrl + 'approvalCeiling/getBudgetCeiling/' + empID + '/' + sbu + '/' + donationId);
  }
  
  getBudgetForCampaign(sbu: string, empID: number, donationId: number, campaignDtlId: number) {
    debugger;
    return this.http.get(this.baseUrl + 'approvalCeiling/getBudgetCeilingForCampaign/' + empID + '/' + sbu + '/' + donationId + '/' + campaignDtlId);
  }
  getLastFiveInvestment(marketCode: string, toDayDate: string) {
    return this.http.get(this.baseUrl + 'investment/getLastFiveInvestment/' + marketCode + '/' + toDayDate);
  }
  async getLastFiveInvestmentForDoc(donationId: number, docId: number, marketCode: string, toDayDate: string) {
    return await this.http.get(this.baseUrl + 'investment/getLastFiveInvestmentForDoc/' + donationId + '/' + docId + '/' + marketCode + '/' + toDayDate).toPromise();
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
  getCampaignMsts(empId: number) {
    return this.http.get(this.baseUrl + 'campaign/campaignMstsForInvestment/' + empId);
  }
  async getInvestmentDoctors(investmentInitId: number) {
    return await this.http.get(this.baseUrl + 'investment/investmentDoctors/' + investmentInitId).toPromise();
  }
  getInvestmentAprComment(investmentInitId: number, empId: string) {
    return this.http.get(this.baseUrl + 'investmentApr/getInvestmentAprComment/' + investmentInitId + '/' + parseInt(empId));
  }
  getInvestmentDetails(investmentInitId: number, empId: number, userRole: string) {
    if (userRole == 'GPM') {
      return this.http.get(this.baseUrl + 'investmentApr/investmentRecDetailsForGPM/' + investmentInitId + '/' + empId).toPromise();
    }
    if (userRole == 'M') {
      if(this.investmentAprFormData.donationTo == "Campaign")
      {
        return this.http.get(this.baseUrl + 'investmentApr/investmentRecDetailsMCamp/' + investmentInitId + '/' + empId).toPromise();

      }
      return this.http.get(this.baseUrl + 'investmentApr/investmentRecDetailsForM/' + investmentInitId + '/' + empId).toPromise();
    }
    else {
      return this.http.get(this.baseUrl + 'investmentApr/investmentRecDetails/' + investmentInitId + '/' + empId).toPromise();
    }

  }
  getInvestmentTargetedProds(investmentInitId: number, sbu: string) {
    return this.http.get(this.baseUrl + 'investmentRec/investmentRecProducts/' + investmentInitId + '/' + sbu);
  }
  getInvestmentAprDetails(investmentInitId: number, empId: number) {
    return this.http.get(this.baseUrl + 'investmentApr/investmentAprDetails/' + investmentInitId + '/' + empId).toPromise();
  }
  getInvestmentAprProducts(investmentInitId: number, sbu: string) {
    return this.http.get(this.baseUrl + 'investmentApr/investmentAprProducts/' + investmentInitId + '/' + sbu);
  }
  getInvestmentTargetedGroups(investmentInitId: number, empId: number) {
    return this.http.get(this.baseUrl + 'InvestmentRec/investmentTargetedGroups/' + investmentInitId + '/' + empId);
  }
  getInvestmentInstitutions(investmentInitId: number) {
    return this.http.get(this.baseUrl + 'investment/investmentInstitutions/' + investmentInitId).toPromise();
  }
  getInvestmentCampaigns(investmentInitId: number) {
    return this.http.get(this.baseUrl + 'investment/investmentCampaigns/' + investmentInitId);
  }
  getCampaignDtls(mstId: number) {
    return this.http.get(this.baseUrl + 'campaign/campaignDtlsForInvestment/' + mstId);
  }
  getCampaignDtlProducts(dtlId: number) {
    return this.http.get(this.baseUrl + 'campaign/campaignDtlProductsForInvestment/' + dtlId);
  }
  getInvestmentBcds(investmentInitId: number) {
    return this.http.get(this.baseUrl + 'investment/investmentBcds/' + investmentInitId).toPromise();
  }
  getInvestmentSociety(investmentInitId: number) {
    return this.http.get(this.baseUrl + 'investment/investmentSociety/' + investmentInitId).toPromise();
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
    var actionName = 'investmentInits';
    // debugger;
    if (userRole == 'RSM') {
      actionName = 'investmentInitsForRSM';
    }
    else if (userRole == 'GPM') {
      actionName = 'investmentInitsForGPM';
    }
    else if (userRole == 'M') {
      actionName = 'investmentInitsForSM';
    }
    else {
      actionName = 'investmentInits';
    }
    return this.http.get(this.baseUrl + 'investmentApr/' + actionName + '/' + empId + '/' + sbu);

    // return this.http.get<IInvestmentInitPagination>(this.baseUrl + 'investmentApr/'+actionName+'/'+empId+'/'+sbu, { observe: 'response', params })
    // //return this.http.get<IDonationPagination>(this.baseUrl + 'donation/donations', { observe: 'response', params })
    // .pipe(
    //   map(response => {
    //     this.investmentInits = [...this.investmentInits, ...response.body.data]; 
    //     this.investmentInitPagination = response.body;
    //     return this.investmentInitPagination;
    //   })
    // );

  }
  getInvestmentApproved(empId: number, sbu: string, userRole: string) {
    // let params = new HttpParams();
    // if (this.genParams.search) {
    //   params = params.append('search', this.genParams.search);
    // }
    // params = params.append('sort', this.genParams.sort);
    // params = params.append('pageIndex', this.genParams.pageIndex.toString());
    // params = params.append('pageSize', this.genParams.pageSize.toString());
    var actionName = 'investmentApproved';
    if (userRole == 'RSM') {
      actionName = 'investmentApprovedForRSM';
    }
    else if (userRole == 'M') {
      actionName = 'investmentApprovedForSM';
    }
    else if (userRole == 'GPM') {
      actionName = 'investmentApprovedForGPM';
    }
    else {
      actionName = 'investmentApproved';
    }
    return this.http.get(this.baseUrl + 'investmentApr/' + actionName + '/' + empId + '/' + sbu + '/' + userRole);

    // return this.http.get<IInvestmentInitPagination>(this.baseUrl + 'investmentApr/'+actionName+'/'+empId+'/'+sbu+'/'+userRole, { observe: 'response', params })
    // //return this.http.get<IDonationPagination>(this.baseUrl + 'donation/donations', { observe: 'response', params })
    // .pipe(
    //   map(response => {
    //     this.investmentInits = [...this.investmentInits, ...response.body.data]; 
    //     this.investmentInitPagination = response.body;
    //     return this.investmentInitPagination;
    //   })
    // );

  }
  insertInvestmentAprForOwnSBU(userRole: string, empId: number, sbu: string, investmentTargetedProds: IInvestmentTargetedProd[]) {
    var investmentRecDepot = this.investmentDepotFormData;
    var investmentRecComment = this.investmentAprCommentFormData;
    var investmentApr = this.investmentDetailFormData;
    var investmentRecProducts = investmentTargetedProds;
    var investmentAprForOwnSBUInsertDto = { investmentRecComment, investmentApr, investmentRecProducts, investmentRecDepot }
    //var actionName='insertAprForOwnSBU';
    if (userRole == 'GPM') {
      return this.http.post(this.baseUrl + 'investmentApr/insertAprForOwnSBUGPM/' + empId + '/' + this.investmentAprCommentFormData.recStatus + '/' + sbu + '/' + this.investmentAprFormData.donationId+ '/' + this.investmentCampaignFormData.campaignDtlId, investmentAprForOwnSBUInsertDto);
    }
    if (this.investmentAprFormData.donationTo == 'Campaign' && userRole == 'RSM') {
      return this.http.post(this.baseUrl + 'investmentApr/insertAprForOwnSBUCampaign/' + empId + '/' + this.investmentAprCommentFormData.recStatus + '/' + sbu + '/' + this.investmentAprFormData.donationId + '/' + this.investmentCampaignFormData.campaignDtlId, investmentAprForOwnSBUInsertDto);
    }

    if (this.investmentAprFormData.donationTo != 'Campaign' && userRole == 'RSM') {
      return this.http.post(this.baseUrl + 'investmentApr/insertAprForOwnSBURSM/' + empId + '/' + this.investmentAprCommentFormData.recStatus + '/' + sbu + '/' + this.investmentAprFormData.donationId, investmentAprForOwnSBUInsertDto);
    }
    return this.http.post(this.baseUrl + 'investmentApr/insertAprForOwnSBU/' + empId + '/' + this.investmentAprCommentFormData.recStatus + '/' + sbu + '/' + this.investmentAprFormData.donationId, investmentAprForOwnSBUInsertDto);

  }
  updateInvestmentAprForOwnSBU(userRole: string, empId: number, sbu: string, investmentTargetedProds: IInvestmentTargetedProd[]) {
    var investmentRecDepot = this.investmentDepotFormData;
    var investmentRecComment = this.investmentAprCommentFormData;
    var investmentApr = this.investmentDetailFormData;
    var investmentRecProducts = investmentTargetedProds;
    var investmentAprForOwnSBUInsertDto = { investmentRecComment, investmentApr, investmentRecProducts, investmentRecDepot }
     //var actionName = 'updateAprForOwnSBU';
    if (userRole == 'GPM') {
      return this.http.post(this.baseUrl + 'investmentApr/updateAprForOwnSBUGPM/' + empId + '/' + this.investmentAprCommentFormData.recStatus + '/' + sbu + '/' + this.investmentAprFormData.donationId+ '/' + this.investmentCampaignFormData.campaignDtlId, investmentAprForOwnSBUInsertDto);
    }
    if (this.investmentAprFormData.donationTo == 'Campaign' && userRole == 'RSM') {
      return this.http.post(this.baseUrl + 'investmentApr/updateAprForOwnSBUCampaign/' + empId + '/' + this.investmentAprCommentFormData.recStatus + '/' + sbu + '/' + this.investmentAprFormData.donationId + '/' + this.investmentCampaignFormData.campaignDtlId, investmentAprForOwnSBUInsertDto);
    }
    if (this.investmentAprFormData.donationTo != 'Campaign' && userRole == 'RSM') {
      return this.http.post(this.baseUrl + 'investmentApr/updateAprForOwnSBURSM/' + empId + '/' + this.investmentAprCommentFormData.recStatus + '/' + sbu + '/' + this.investmentAprFormData.donationId, investmentAprForOwnSBUInsertDto);
    }
    return this.http.post(this.baseUrl + 'investmentApr/updateAprForOwnSBU/' + empId + '/' + this.investmentAprCommentFormData.recStatus + '/' + sbu + '/' + this.investmentAprFormData.donationId, investmentAprForOwnSBUInsertDto);
  }
  insertInvestmentApr(userRole: string) {
    var actionName = 'insertAprCom';
    if (userRole == 'GPM') {
      actionName = 'insertAprComForGPM';
    }
    return this.http.post(this.baseUrl + 'investmentApr/' + actionName, this.investmentAprCommentFormData);
  }
  updateInvestmentApr(userRole: string) {
    var actionName = 'updateAprCom';
    if (userRole == 'GPM') {
      actionName = 'updateAprComForGPM';
    }
    return this.http.post(this.baseUrl + 'investmentApr/' + actionName, this.investmentAprCommentFormData);
  }
  insertInvestmentDetail(empId: number, sbu: string) {
    if (this.investmentAprFormData.donationTo == 'Campaign') {
      if (this.investmentAprCommentFormData.recStatus == 'Approved') {
        return this.http.post(this.baseUrl + 'investmentApr/insertAprForCampaign/' + empId + '/' + this.investmentAprCommentFormData.recStatus + '/' + sbu + '/' + this.investmentAprFormData.donationId + '/' + this.investmentCampaignFormData.campaignDtlId, this.investmentDetailFormData);
      }
      else {
        return this.http.post(this.baseUrl + 'investmentApr/insertRec/' + empId + '/' + this.investmentAprCommentFormData.recStatus + '/' + sbu + '/' + this.investmentAprFormData.donationId, this.investmentDetailFormData);
      }
    }
    else {
      if (this.investmentAprCommentFormData.recStatus == 'Approved') {
        return this.http.post(this.baseUrl + 'investmentApr/insertApr/' + empId + '/' + this.investmentAprCommentFormData.recStatus + '/' + sbu + '/' + this.investmentAprFormData.donationId, this.investmentDetailFormData);
      }
      else {
        return this.http.post(this.baseUrl + 'investmentApr/insertRec/' + empId + '/' + this.investmentAprCommentFormData.recStatus + '/' + sbu + '/' + this.investmentAprFormData.donationId, this.investmentDetailFormData);
      }
    }
  }
  insertInvestmentRecDepot() {
    return this.http.post(this.baseUrl + 'investmentApr/insertInvestmentRecDepot', this.investmentDepotFormData);
  }
  getInvestmentRecDepot(initId: any) {
    return this.http.get(this.baseUrl + 'investmentApr/getInvestmentRecDepot/' + initId).toPromise();
  }
  insertInvestmentTargetedProd(investmentTargetedProds: IInvestmentTargetedProd[]) {
    return this.http.post(this.baseUrl + 'investmentApr/insertAprProd', investmentTargetedProds,
      { responseType: 'text' });
  }
  removeInvestmentTargetedProd() {
    return this.http.post(this.baseUrl + 'investmentApr/removeInvestmentTargetedProd', this.investmentTargetedProdFormData,
      { responseType: 'text' });
  }
  getInvestmentMedicineProds(investmentInitId: number, sbu: string) {
    return this.http.get(this.baseUrl + 'investment/investmentMedicineProds/' + investmentInitId + '/' + sbu);
  }
  insertInvestmentMedicineProd() {
    return this.http.post(this.baseUrl + 'investment/insertInvestmentMedicineProd', this.investmentMedicineProdFormData);
  }
  removeInvestmentMedicineProd() {
    return this.http.post(this.baseUrl + 'investment/removeInvestmentMedicineProd', this.investmentMedicineProdFormData,
      { responseType: 'text' });
  }
}

