import { InvestmentRecPagination, IInvestmentRecPagination } from '../shared/models/investmentRecPagination';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { InvestmentRec, IInvestmentRec,InvestmentDetail,IInvestmentDetail,
  InvestmentTargetedProd,IInvestmentTargetedProd,InvestmentTargetedGroup,IInvestmentTargetedGroup } from '../shared/models/investment';
import { InvestmentDoctor, IInvestmentDoctor,InvestmentInstitution,IInvestmentInstitution,InvestmentCampaign,IInvestmentCampaign } from '../shared/models/investment';
import { InvestmentBcds, IInvestmentBcds,InvestmentSociety,IInvestmentSociety } from '../shared/models/investment';

import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { BehaviorSubject, ReplaySubject, of, from } from 'rxjs';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router';
import { GenericParams } from '../shared/models/genericParams';

@Injectable({
  providedIn: 'root'
})
export class InvestmentRecService {
  investmentRecs: IInvestmentRec[]=[];
  investmentRecPagination = new InvestmentRecPagination();
  investmentRecFormData: InvestmentRec = new InvestmentRec();
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

  getDonations(){    
    return this.http.get(this.baseUrl + 'donation/donationsForInvestment');
  }
  getMarkets(){    
    return this.http.get(this.baseUrl + 'employee/marketForInvestment');
  }
  getProduct(){    
    return this.http.get(this.baseUrl + 'product/getProductForInvestment');
  }
  getMarketGroupMsts(){    
    return this.http.get(this.baseUrl + 'marketGroup/getMarketGroupMstsForInvestment');
  }
  getApprovalAuthority(){    
    return this.http.get(this.baseUrl + 'approvalAuthority/approvalAuthoritiesForConfig');
  }
  getEmployees(){    
    return this.http.get(this.baseUrl + 'employee/employeesForInvestment');
  }
  getInstitutions(){    
    return this.http.get(this.baseUrl + 'institution/institutionsForInvestment');
  }
  getDoctors(){    
    return this.http.get(this.baseUrl + 'doctor/doctorsForInvestment');
  }
  getBcds(){    
    return this.http.get(this.baseUrl + 'bcds/bcdsForInvestment');
  }
  getSociety(){    
    return this.http.get(this.baseUrl + 'society/societyForInvestment');
  }
  getCampaignMsts(){    
    return this.http.get(this.baseUrl + 'campaign/campaignMstsForInvestment');
  }
  getCampaignDtls(mstId:number){    
    return this.http.get(this.baseUrl + 'campaign/campaignDtlsForInvestment/'+mstId);
  }
  getCampaignDtlProducts(dtlId:number){    
    return this.http.get(this.baseUrl + 'campaign/campaignDtlProductsForInvestment/'+dtlId);
  }
  getSubCampaigns(){    
    return this.http.get(this.baseUrl + 'campaign/subCampaignsForInvestment');
  }
  getInvestmentDoctors(investmentRecId:number){    
    return this.http.get(this.baseUrl + 'investment/investmentDoctors/'+investmentRecId);
  }
  getInvestmentTargetedProds(investmentRecId:number){    
    return this.http.get(this.baseUrl + 'investment/investmentTargetedProds/'+investmentRecId);
  }
  getInvestmentTargetedGroups(investmentRecId:number){    
    return this.http.get(this.baseUrl + 'investment/investmentTargetedGroups/'+investmentRecId);
  }
  getInvestmentInstitutions(investmentRecId:number){    
    return this.http.get(this.baseUrl + 'investment/investmentInstitutions/'+investmentRecId);
  }
  getInvestmentCampaigns(investmentRecId:number){    
    return this.http.get(this.baseUrl + 'investment/investmentCampaigns/'+investmentRecId);
  }
  getInvestmentBcds(investmentRecId:number){    
    return this.http.get(this.baseUrl + 'investment/investmentBcds/'+investmentRecId);
  }
  getInvestmentSociety(investmentRecId:number){    
    return this.http.get(this.baseUrl + 'investment/investmentSociety/'+investmentRecId);
  }
  getInvestmentDetails(investmentRecId:number){    
    return this.http.get(this.baseUrl + 'investment/investmentDetails/'+investmentRecId);
  }
  getInvestmentRec(){    
    let params = new HttpParams();
    if (this.genParams.search) {
      params = params.append('search', this.genParams.search);
    }
    params = params.append('sort', this.genParams.sort);
    params = params.append('pageIndex', this.genParams.pageNumber.toString());
    params = params.append('pageSize', this.genParams.pageSize.toString());
    return this.http.get<IInvestmentRecPagination>(this.baseUrl + 'investment/investmentRecs', { observe: 'response', params })
    //return this.http.get<IDonationPagination>(this.baseUrl + 'donation/donations', { observe: 'response', params })
    .pipe(
      map(response => {
        this.investmentRecs = [...this.investmentRecs, ...response.body.data]; 
        this.investmentRecPagination = response.body;
        return this.investmentRecPagination;
      })
    );
    
  }
  insertInvestmentRec() {
    debugger;
    return this.http.post(this.baseUrl+ 'investment/insertInit', this.investmentRecFormData);

  }
  
  updateInvestmentRec() {
    return this.http.post(this.baseUrl+ 'investment/updateInit',  this.investmentRecFormData);
  }
  insertInvestmentDetail() {
    debugger;
    return this.http.post(this.baseUrl+ 'investment/insertDetail', this.investmentDetailFormData);
  }
  
  updateInvestmentDetail() {
    return this.http.post(this.baseUrl+ 'investment/updateDetail',  this.investmentDetailFormData);
  }
  insertInvestmentDoctor() {
    debugger;
    return this.http.post(this.baseUrl+ 'investment/insertInvestmentDoctor', this.investmentDoctorFormData);

  }
  insertInvestmentInstitution() {
    debugger;
    return this.http.post(this.baseUrl+ 'investment/insertInvestmentInstitution', this.investmentInstitutionFormData);

  }
  insertInvestmentCampaign() {
    debugger;
    return this.http.post(this.baseUrl+ 'investment/insertInvestmentCampaign', this.investmentCampaignFormData);

  }
  insertInvestmentBcds() {
    debugger;
    return this.http.post(this.baseUrl+ 'investment/insertInvestmentBcds', this.investmentBcdsFormData);

  }
  insertInvestmentSociety() {
    debugger;
    return this.http.post(this.baseUrl+ 'investment/insertInvestmentSociety', this.investmentSocietyFormData);

  }
  insertInvestmentTargetedProd() {
    debugger;
    return this.http.post(this.baseUrl+ 'investment/insertInvestmentTargetedProd', this.investmentTargetedProdFormData);

  }
  updateInvestmentTargetedProd() {
    debugger;
    return this.http.post(this.baseUrl+ 'investment/updateInvestmentTargetedProd', this.investmentTargetedProdFormData);

  }
  insertInvestmentTargetedGroup(investmentTargetedGroups:IInvestmentTargetedGroup[]) {
    debugger;
    return this.http.post(this.baseUrl+ 'investment/insertInvestmentTargetedGroup', investmentTargetedGroups,
    {responseType: 'text'});

  }
  removeInvestmentDoctor() {
    debugger;
    return this.http.post(this.baseUrl+ 'investment/removeInvestmentDoctor', this.investmentDoctorFormData,
    {responseType: 'text'});

  }
  removeInvestmentInstitution() {
    debugger;
    return this.http.post(this.baseUrl+ 'investment/removeInvestmentInstitution', this.investmentInstitutionFormData,
    {responseType: 'text'});

  }
  removeInvestmentCampaign() {
    debugger;
    return this.http.post(this.baseUrl+ 'investment/removeInvestmentCampaign', this.investmentCampaignFormData,
    {responseType: 'text'});

  }
  removeInvestmentBcds() {
    debugger;
    return this.http.post(this.baseUrl+ 'investment/removeInvestmentBcds', this.investmentBcdsFormData,
    {responseType: 'text'});

  }
  removeInvestmentSociety() {
    debugger;
    return this.http.post(this.baseUrl+ 'investment/removeInvestmentSociety', this.investmentSocietyFormData,
    {responseType: 'text'});

  }
  removeInvestmentTargetedProd() {
    debugger;
    return this.http.post(this.baseUrl+ 'investment/removeInvestmentTargetedProd', this.investmentTargetedProdFormData,
    {responseType: 'text'});

  }
  removeInvestmentTargetedGroup(investmentTargetedGroups:IInvestmentTargetedGroup[]) {
    debugger;
    return this.http.post(this.baseUrl+ 'investment/removeInvestmentTargetedGroup', investmentTargetedGroups,
    {responseType: 'text'});

  }
}

