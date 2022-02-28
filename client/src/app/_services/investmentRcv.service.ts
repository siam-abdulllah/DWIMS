import { IInvestmentInitPagination, InvestmentInitPagination } from './../shared/models/investmentPagination';
import { InvestmentRcvPagination, IInvestmentRcvPagination } from '../shared/models/investmentRcvPagination';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { InvestmentRcv,  IInvestmentRcv,InvestmentInit,IInvestmentInit,
  InvestmentTargetedProd,IInvestmentTargetedProd,InvestmentTargetedGroup,IInvestmentTargetedGroup,InvestmentRcvComment,IInvestmentRcvComment } from '../shared/models/investmentRcv';
import { InvestmentDoctor, IInvestmentDoctor,InvestmentInstitution,IInvestmentInstitution,InvestmentCampaign,IInvestmentCampaign } from '../shared/models/investmentRcv';
import { InvestmentBcds, IInvestmentBcds,InvestmentSociety,IInvestmentSociety } from '../shared/models/investmentRcv';
import { InvestmentAprPagination, IInvestmentAprPagination } from '../shared/models/investmentAprPagination';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { BehaviorSubject, ReplaySubject, of, from } from 'rxjs';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router';
import { GenericParams } from '../shared/models/genericParams';
import { IInvestmentApr } from '../shared/models/investmentApr';

@Injectable({
  providedIn: 'root'
})
export class InvestmentRcvService {
  investmentAprs: IInvestmentApr[]=[];
  investmentAprPagination = new InvestmentAprPagination();
  investmentInits: IInvestmentInit[]=[];
  investmentInitPagination = new InvestmentInitPagination();
  investmentRcvs: IInvestmentRcv[]=[];
  investmentRcvPagination = new InvestmentRcvPagination();
  investmentRcvFormData: InvestmentInit = new InvestmentInit();
  investmentDetailFormData: InvestmentRcv = new InvestmentRcv();
  investmentRcvCommentFormData: InvestmentRcvComment = new InvestmentRcvComment();
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

  getGenParams(){
    return this.genParams;
  }
   // tslint:disable-next-line: typedef
   setGenParams(genParams: GenericParams) {
    this.genParams = genParams;
  }

  getDonations() {
    return this.http.get(this.baseUrl + 'donation/donationsForInvestment');
  }
  getProduct(sbu:string){    
    return this.http.get(this.baseUrl + 'product/getProductForInvestment/'+sbu);
  }
  // getBudget(sbu:string,empID:number,donationId:number){    
  //   return this.http.get(this.baseUrl + 'approvalCeiling/getBudgetCeiling/'+empID+'/'+sbu+'/'+donationId);
  // }
  getCampaignMsts(empId:number){    
    return this.http.get(this.baseUrl + 'campaign/campaignMstsForInvestment/'+empId);
  }
  getInvestmentDoctors(investmentInitId:number){    
    return this.http.get(this.baseUrl + 'investment/investmentDoctors/'+investmentInitId);
  }
  getInvestmentRcvComment(investmentInitId:number){    
    return this.http.get(this.baseUrl + 'investmentRecv/getInvestmentRecvComment/'+investmentInitId);
  }
  getInvestmentDetails(investmentInitId:number){    
    return this.http.get(this.baseUrl + 'investmentRecv/investmentRecDetails/'+investmentInitId);
  }

  getInvestmentRcvDetails(investmentInitId:number,empId:number){    
    return this.http.get(this.baseUrl + 'investmentApr/investmentAprDetails/'+investmentInitId+'/'+empId);
  }
  getInvestmentRcvProducts(investmentInitId:number,sbu:string){    
    return this.http.get(this.baseUrl + 'investmentApr/investmentAprProducts/'+investmentInitId+'/'+sbu);
  }
  getInvestmentTargetedGroups(investmentInitId:number,empId:number){    
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
    // let params = new HttpParams();
    // if (this.genParams.search) {
    //   params = params.append('search', this.genParams.search);
    // }
    // params = params.append('sort', this.genParams.sort);
    // params = params.append('pageIndex', this.genParams.pageIndex.toString());
    // params = params.append('pageSize', this.genParams.pageSize.toString());
    // return this.http.get<IInvestmentInitPagination>(this.baseUrl + 'investmentRecv/investmentInits/'+empId+'/'+sbu, { observe: 'response', params })
    // //return this.http.get<IDonationPagination>(this.baseUrl + 'donation/donations', { observe: 'response', params })
    // .pipe(
    //   map(response => {
    //     this.investmentInits = [...this.investmentInits, ...response.body.data]; 
    //     this.investmentInitPagination = response.body;
    //     return this.investmentInitPagination;
    //   })
    // );

    return this.http.get(this.baseUrl+ 'investmentRecv/investmentInits/'+empId+'/'+sbu);
    
  }
  getInvestmentApproved(empId:number,sbu:string){    
    // let params = new HttpParams();
    // if (this.genParams.search) {
    //   params = params.append('search', this.genParams.search);
    // }
    // params = params.append('sort', this.genParams.sort);
    // params = params.append('pageIndex', this.genParams.pageIndex.toString());
    // params = params.append('pageSize', this.genParams.pageSize.toString());
    // //return this.http.get<IInvestmentAprPagination>(this.baseUrl + 'investmentRecv/investmentApproved/'+empId+'/'+sbu, { observe: 'response', params })
    // return this.http.get<IInvestmentInitPagination>(this.baseUrl + 'investmentRecv/GetinvestmentReceived/'+empId+'/'+sbu, { observe: 'response', params })
    // //return this.http.get<IDonationPagination>(this.baseUrl + 'donation/donations', { observe: 'response', params })
    // .pipe(
    //   map(response => {
    //     this.investmentInits = [...this.investmentInits, ...response.body.data]; 
    //     this.investmentInitPagination = response.body;
    //     return this.investmentInitPagination;
    //   })
    // );
    
    return this.http.get(this.baseUrl+ 'investmentRecv/GetinvestmentReceived/'+empId+'/'+sbu);
  }
  // insertInvestmentRcv() {  
  //   return this.http.post(this.baseUrl+ 'investmentRecv/insertRcv', this.investmentRcvCommentFormData);
  // }

  insertInvestmentRcv(model: any) {
    return this.http.post(this.baseUrl+ 'investmentRecv/InsertRecv', model);
  }
  
  updateInvestmentRcv(model: any) {
    return this.http.post(this.baseUrl+ 'investmentRecv/UpdateRecv',  model);
  }
  insertInvestmentDetail(empId:number,sbu:string) {
    
    return this.http.post(this.baseUrl+ 'investmentRcv/insertRcv/'+empId+'/'+this.investmentRcvCommentFormData.receiveStatus+'/'+sbu+'/'+this.investmentRcvFormData.donationId, this.investmentDetailFormData);
  
  }
  
 
  
  insertInvestmentTargetedProd(investmentTargetedProds:IInvestmentTargetedProd[]) {
    
    return this.http.post(this.baseUrl+ 'investmentRcv/insertRcvProd', investmentTargetedProds,
    {responseType: 'text'});

  }
  
  removeInvestmentTargetedProd() {
    
    return this.http.post(this.baseUrl+ 'investment/removeInvestmentTargetedProd', this.investmentTargetedProdFormData,
    {responseType: 'text'});

  }
  
}

