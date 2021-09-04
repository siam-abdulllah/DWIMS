import { InvestmentAprPagination, IInvestmentAprPagination } from '../shared/models/investmentAprPagination';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { InvestmentApr, IInvestmentApr,InvestmentInit,IInvestmentInit,
  InvestmentTargetedProd,IInvestmentTargetedProd,InvestmentTargetedGroup,IInvestmentTargetedGroup,InvestmentAprComment,IInvestmentAprComment } from '../shared/models/investmentApr';
import { InvestmentDoctor, IInvestmentDoctor,InvestmentInstitution,IInvestmentInstitution,InvestmentCampaign,IInvestmentCampaign } from '../shared/models/investmentApr';
import { InvestmentBcds, IInvestmentBcds,InvestmentSociety,IInvestmentSociety } from '../shared/models/investmentApr';

import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { BehaviorSubject, ReplaySubject, of, from } from 'rxjs';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router';
import { GenericParams } from '../shared/models/genericParams';

@Injectable({
  providedIn: 'root'
})
export class InvestmentAprService {
  investmentAprs: IInvestmentApr[]=[];
  investmentAprPagination = new InvestmentAprPagination();
  investmentAprFormData: InvestmentInit = new InvestmentInit();
  investmentDetailFormData: InvestmentApr = new InvestmentApr();
  investmentAprCommentFormData: InvestmentAprComment = new InvestmentAprComment();
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

  
  getProduct(sbu:string){    
    return this.http.get(this.baseUrl + 'product/getProductForInvestment/'+sbu);
  }
  getLastFiveInvestment(marketCode:string,toDayDate:string){    
    return this.http.get(this.baseUrl + 'investment/getLastFiveInvestment/'+marketCode+'/'+toDayDate);
  }
  getCampaignMsts(){    
    return this.http.get(this.baseUrl + 'campaign/campaignMstsForInvestment');
  }
  getInvestmentDoctors(investmentInitId:number){    
    return this.http.get(this.baseUrl + 'investment/investmentDoctors/'+investmentInitId);
  }
  getInvestmentAprComment(investmentInitId:number,empId:string){    
    return this.http.get(this.baseUrl + 'investmentApr/getInvestmentAprComment/'+investmentInitId+'/'+parseInt(empId));
  }
  getInvestmentDetails(investmentInitId:number){    
    return this.http.get(this.baseUrl + 'investmentRec/investmentRecDetails/'+investmentInitId);
  }
  getInvestmentTargetedProds(investmentInitId:number,sbu:string){    
    return this.http.get(this.baseUrl + 'investmentRec/investmentRecProducts/'+investmentInitId+'/'+sbu);
  }
  getInvestmentAprDetails(investmentInitId:number){    
    return this.http.get(this.baseUrl + 'investmentApr/investmentAprDetails/'+investmentInitId);
  }
  getInvestmentAprProducts(investmentInitId:number,sbu:string){    
    return this.http.get(this.baseUrl + 'investmentApr/investmentAprProducts/'+investmentInitId+'/'+sbu);
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
    params = params.append('pageIndex', this.genParams.pageNumber.toString());
    params = params.append('pageSize', this.genParams.pageSize.toString());
    return this.http.get<IInvestmentAprPagination>(this.baseUrl + 'investmentApr/investmentInits/'+empId+'/'+sbu, { observe: 'response', params })
    //return this.http.get<IDonationPagination>(this.baseUrl + 'donation/donations', { observe: 'response', params })
    .pipe(
      map(response => {
        this.investmentAprs = [...this.investmentAprs, ...response.body.data]; 
        this.investmentAprPagination = response.body;
        return this.investmentAprPagination;
      })
    );
    
  }
  getInvestmentApproved(empId:number,sbu:string){    
    let params = new HttpParams();
    if (this.genParams.search) {
      params = params.append('search', this.genParams.search);
    }
    params = params.append('sort', this.genParams.sort);
    params = params.append('pageIndex', this.genParams.pageNumber.toString());
    params = params.append('pageSize', this.genParams.pageSize.toString());
    return this.http.get<IInvestmentAprPagination>(this.baseUrl + 'investmentApr/investmentApproved/'+empId+'/'+sbu, { observe: 'response', params })
    //return this.http.get<IDonationPagination>(this.baseUrl + 'donation/donations', { observe: 'response', params })
    .pipe(
      map(response => {
        this.investmentAprs = [...this.investmentAprs, ...response.body.data]; 
        this.investmentAprPagination = response.body;
        return this.investmentAprPagination;
      })
    );
    
  }
  insertInvestmentApr() {
    
    return this.http.post(this.baseUrl+ 'investmentApr/insertAprCom', this.investmentAprCommentFormData);

  }
  
  updateInvestmentApr() {
    return this.http.post(this.baseUrl+ 'investmentApr/updateAprCom',  this.investmentAprCommentFormData);
  }
  insertInvestmentDetail(empId:number,sbu:string) {
    
    return this.http.post(this.baseUrl+ 'investmentApr/insertApr/'+empId+'/'+this.investmentAprCommentFormData.aprStatus+'/'+sbu, this.investmentDetailFormData);
  
  }
  
 
  
  insertInvestmentTargetedProd(investmentTargetedProds:IInvestmentTargetedProd[]) {
    
    return this.http.post(this.baseUrl+ 'investmentApr/insertAprProd', investmentTargetedProds,
    {responseType: 'text'});

  }
  
  removeInvestmentTargetedProd() {
    
    return this.http.post(this.baseUrl+ 'investment/removeInvestmentTargetedProd', this.investmentTargetedProdFormData,
    {responseType: 'text'});

  }
  
}

