import { InvestmentRecPagination, IInvestmentRecPagination } from '../shared/models/investmentRecPagination';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { InvestmentRec, IInvestmentRec,InvestmentInit,IInvestmentInit,
  InvestmentTargetedProd,IInvestmentTargetedProd,InvestmentTargetedGroup,IInvestmentTargetedGroup,InvestmentRecComment,IInvestmentRecComment } from '../shared/models/investmentRec';
import { InvestmentDoctor, IInvestmentDoctor,InvestmentInstitution,IInvestmentInstitution,InvestmentCampaign,IInvestmentCampaign } from '../shared/models/investmentRec';
import { InvestmentBcds, IInvestmentBcds,InvestmentSociety,IInvestmentSociety } from '../shared/models/investmentRec';

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
  investmentRecFormData: InvestmentInit = new InvestmentInit();
  investmentDetailFormData: InvestmentRec = new InvestmentRec();
  investmentRecCommentFormData: InvestmentRecComment = new InvestmentRecComment();
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

  
  getProduct(){    
    return this.http.get(this.baseUrl + 'product/getProductForInvestment');
  }
  
  getInvestmentDoctors(investmentInitId:number){    
    return this.http.get(this.baseUrl + 'investment/investmentDoctors/'+investmentInitId);
  }
  getInvestmentTargetedProds(investmentInitId:number){    
    return this.http.get(this.baseUrl + 'investment/investmentTargetedProds/'+investmentInitId);
  }
  getInvestmentTargetedGroups(investmentInitId:number){    
    return this.http.get(this.baseUrl + 'investment/investmentTargetedGroups/'+investmentInitId);
  }
  getInvestmentInstitutions(investmentInitId:number){    
    return this.http.get(this.baseUrl + 'investment/investmentInstitutions/'+investmentInitId);
  }
  getInvestmentCampaigns(investmentInitId:number){    
    return this.http.get(this.baseUrl + 'investment/investmentCampaigns/'+investmentInitId);
  }
  getCampaignDtlProducts(dtlId:number){    
    return this.http.get(this.baseUrl + 'campaign/campaignDtlProductsForInvestment/'+dtlId);
  }
  getInvestmentBcds(investmentInitId:number){    
    return this.http.get(this.baseUrl + 'investment/investmentBcds/'+investmentInitId);
  }
  getInvestmentSociety(investmentInitId:number){    
    return this.http.get(this.baseUrl + 'investment/investmentSociety/'+investmentInitId);
  }
  getInvestmentDetails(investmentInitId:number){    
    return this.http.get(this.baseUrl + 'investment/investmentDetails/'+investmentInitId);
  }
  getInvestmentInit(){    
    let params = new HttpParams();
    if (this.genParams.search) {
      params = params.append('search', this.genParams.search);
    }
    params = params.append('sort', this.genParams.sort);
    params = params.append('pageIndex', this.genParams.pageNumber.toString());
    params = params.append('pageSize', this.genParams.pageSize.toString());
    return this.http.get<IInvestmentRecPagination>(this.baseUrl + 'investmentRec/investmentInits', { observe: 'response', params })
    //return this.http.get<IDonationPagination>(this.baseUrl + 'donation/donations', { observe: 'response', params })
    .pipe(
      map(response => {
        this.investmentRecs = [...this.investmentRecs, ...response.body.data]; 
        this.investmentRecPagination = response.body;
        return this.investmentRecPagination;
      })
    );
    
  }
  getInvestmentRecommended(){    
    let params = new HttpParams();
    if (this.genParams.search) {
      params = params.append('search', this.genParams.search);
    }
    params = params.append('sort', this.genParams.sort);
    params = params.append('pageIndex', this.genParams.pageNumber.toString());
    params = params.append('pageSize', this.genParams.pageSize.toString());
    return this.http.get<IInvestmentRecPagination>(this.baseUrl + 'investmentRec/investmentRecommended', { observe: 'response', params })
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
    return this.http.post(this.baseUrl+ 'investment/insertInit', this.investmentRecCommentFormData);

  }
  
  updateInvestmentRec() {
    return this.http.post(this.baseUrl+ 'investment/updateInit',  this.investmentRecCommentFormData);
  }
  insertInvestmentDetail() {
    debugger;
    return this.http.post(this.baseUrl+ 'investmentRec/insertDetail', this.investmentDetailFormData);
  }
  
  updateInvestmentDetail() {
    return this.http.post(this.baseUrl+ 'investmentRec/updateDetail',  this.investmentDetailFormData);
  }
  
  insertInvestmentTargetedProd() {
    debugger;
    return this.http.post(this.baseUrl+ 'investmentRec/insertRecProd', this.investmentTargetedProdFormData);

  }
  updateInvestmentTargetedProd() {
    debugger;
    return this.http.post(this.baseUrl+ 'investmentRec/updateRecProd', this.investmentTargetedProdFormData);

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

