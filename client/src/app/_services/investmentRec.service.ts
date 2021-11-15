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

  getLastFiveInvestment(marketCode:string,toDayDate:string){    
    return this.http.get(this.baseUrl + 'investment/getLastFiveInvestment/'+marketCode+'/'+toDayDate);
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
  getProduct(sbu:string){    
    return this.http.get(this.baseUrl + 'product/getProductForInvestment/'+sbu);
  }
  getCampaignMsts(){    
    return this.http.get(this.baseUrl + 'campaign/campaignMstsForInvestment');
  }
  getInvestmentDoctors(investmentInitId:number){    
    return this.http.get(this.baseUrl + 'investment/investmentDoctors/'+investmentInitId);
  }
  getInvestmentRecComment(investmentInitId:number,empId:string){    
    return this.http.get(this.baseUrl + 'investmentRec/getInvestmentRecComment/'+investmentInitId+'/'+parseInt(empId));
  }
  getInvestmentDetails(investmentInitId:number){    
    return this.http.get(this.baseUrl + 'investment/investmentDetails/'+investmentInitId);
  }
  getInvestmentTargetedProds(investmentInitId:number,sbu:string){    
    return this.http.get(this.baseUrl + 'investment/investmentTargetedProds/'+investmentInitId+'/'+sbu);
  }
  getInvestmentRecDetails(investmentInitId:number,empId:number){    
    return this.http.get(this.baseUrl + 'investmentRec/investmentRecDetails/'+investmentInitId);
  }
  getInvestmentRecProducts(investmentInitId:number,sbu:string){    
    return this.http.get(this.baseUrl + 'investmentRec/investmentRecProducts/'+investmentInitId+'/'+sbu);
  }
  getInvestmentTargetedGroups(investmentInitId:number,empId:number){    
    //return this.http.get(this.baseUrl + 'investment/investmentTargetedGroups/'+investmentInitId);
    return this.http.get(this.baseUrl + 'InvestmentRec/investmentTargetedGroups/'+investmentInitId+'/'+empId);
  }
  getInvestmentInstitutions(investmentInitId:number){    
    return this.http.get(this.baseUrl + 'investment/investmentInstitutions/'+investmentInitId);
  }
  getInvestmentCampaigns(investmentInitId:number){    
    return this.http.get(this.baseUrl + 'investment/investmentCampaigns/'+investmentInitId);
  }
  getCampaignDtls(mstId:number){    
    return this.http.get(this.baseUrl + 'campaign/campaignDtlsForInvestment/'+mstId);
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
  
  getInvestmentInit(empId:number,sbu:string){    
    let params = new HttpParams();
    if (this.genParams.search) {
      params = params.append('search', this.genParams.search);
    }
    params = params.append('sort', this.genParams.sort);
    params = params.append('pageIndex', this.genParams.pageIndex.toString());
    params = params.append('pageSize', this.genParams.pageSize.toString());
    return this.http.get<IInvestmentRecPagination>(this.baseUrl + 'investmentRec/investmentInits/'+empId+'/'+sbu, { observe: 'response', params })
    //return this.http.get<IDonationPagination>(this.baseUrl + 'donation/donations', { observe: 'response', params })
    .pipe(
      map(response => {
        this.investmentRecs = [...this.investmentRecs, ...response.body.data]; 
        this.investmentRecPagination = response.body;
        return this.investmentRecPagination;
      })
    );
    
  }
  getInvestmentRecommended(empId:number,sbu:string){    
    let params = new HttpParams();
    if (this.genParams.search) {
      params = params.append('search', this.genParams.search);
    }
    params = params.append('sort', this.genParams.sort);
    params = params.append('pageIndex', this.genParams.pageIndex.toString());
    params = params.append('pageSize', this.genParams.pageSize.toString());
    return this.http.get<IInvestmentRecPagination>(this.baseUrl + 'investmentRec/investmentRecommended/'+empId+'/'+sbu, { observe: 'response', params })
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
    
    return this.http.post(this.baseUrl+ 'investmentRec/insertRecCom', this.investmentRecCommentFormData);

  }
  
  updateInvestmentRec() {
    return this.http.post(this.baseUrl+ 'investmentRec/updateRecCom',  this.investmentRecCommentFormData);
  }
  insertInvestmentDetail(empId:number,sbu:string) {
    
    return this.http.post(this.baseUrl+ 'investmentRec/insertRec/'+empId+'/'+this.investmentRecCommentFormData.recStatus+'/'+sbu, this.investmentDetailFormData);
  
  }
  
 
  
  insertInvestmentTargetedProd(investmentTargetedProds:IInvestmentTargetedProd[]) {
    
    return this.http.post(this.baseUrl+ 'investmentRec/insertRecProd', investmentTargetedProds,
    {responseType: 'text'});

  }
  
  removeInvestmentTargetedProd() {
    
    return this.http.post(this.baseUrl+ 'investment/removeInvestmentTargetedProd', this.investmentTargetedProdFormData,
    {responseType: 'text'});

  }
  
}

