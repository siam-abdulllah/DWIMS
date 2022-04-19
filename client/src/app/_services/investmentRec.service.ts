import { InvestmentMedicineProd } from './../shared/models/investmentRec';
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
import { IInvestmentInitPagination, InvestmentInitPagination } from '../shared/models/investmentPagination';

@Injectable({
  providedIn: 'root'
})
export class InvestmentRecService {
  investmentRecs: IInvestmentRec[]=[];
  investmentRecPagination = new InvestmentRecPagination();
  investmentInits: IInvestmentInit[]=[];
  investmentInitPagination = new InvestmentInitPagination();
  investmentRecFormData: InvestmentInit = new InvestmentInit();
  investmentDetailFormData: InvestmentRec = new InvestmentRec();
  investmentRecCommentFormData: InvestmentRecComment = new InvestmentRecComment();
  investmentTargetedProdFormData: InvestmentTargetedProd = new InvestmentTargetedProd();
  investmentTargetedGroupFormData: InvestmentTargetedGroup = new InvestmentTargetedGroup();
  investmentMedicineProdFormData: InvestmentMedicineProd = new InvestmentMedicineProd();
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
  setGenParams(genParams: GenericParams) {
    this.genParams = genParams;
  }
  getDonations() {
    return this.http.get(this.baseUrl + 'donation/donationsForInvestment');
  }
  getMedicineProduct() {
    return this.http.get(this.baseUrl + 'product/getMedicineProductForInvestment');
}
  getLastFiveInvestment(marketCode:string,toDayDate:string){    
    return this.http.get(this.baseUrl + 'investment/getLastFiveInvestment/'+marketCode+'/'+toDayDate).toPromise();
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
  getProduct(sbu:string){    
    return this.http.get(this.baseUrl + 'product/getProductForInvestment/'+sbu);
  }
  getCampaignMsts(empId:number){    
    return this.http.get(this.baseUrl + 'campaign/campaignMstsForInvestment/'+empId);
  }
  async  getInvestmentDoctors(investmentInitId:number){    
    return await  this.http.get(this.baseUrl + 'investment/investmentDoctors/'+investmentInitId).toPromise();
  }
  getInvestmentRecComment(investmentInitId:number,empId:string){    
    return this.http.get(this.baseUrl + 'investmentRec/getInvestmentRecComment/'+investmentInitId+'/'+parseInt(empId));
  }
  getInvestmentDetails(investmentInitId:number){    
    return this.http.get(this.baseUrl + 'investment/investmentDetails/'+investmentInitId).toPromise();
  }
  getInvestmentMedicineProds(investmentInitId: number, sbu: string) {
    return this.http.get(this.baseUrl + 'investment/investmentMedicineProds/' + investmentInitId + '/' + sbu);
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
    return this.http.get(this.baseUrl + 'investment/investmentInstitutions/'+investmentInitId).toPromise();
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
    return this.http.get(this.baseUrl + 'investment/investmentBcds/'+investmentInitId).toPromise();
  }
  getInvestmentSociety(investmentInitId:number){    
    return this.http.get(this.baseUrl + 'investment/investmentSociety/'+investmentInitId).toPromise();
  }
  
  getInvestmentInit(empId:number,sbu:string){    
    // let params = new HttpParams();
    // if (this.genParams.search) {
    //   params = params.append('search', this.genParams.search);
    // }
    // params = params.append('sort', this.genParams.sort);
    // params = params.append('pageIndex', this.genParams.pageIndex.toString());
    // params = params.append('pageSize', this.genParams.pageSize.toString());
    // return this.http.get<IInvestmentInitPagination>(this.baseUrl + 'investmentRec/investmentInits/'+empId+'/'+sbu, { observe: 'response', params })
    // //return this.http.get<IDonationPagination>(this.baseUrl + 'donation/donations', { observe: 'response', params })
    // .pipe(
    //   map(response => {
    //     this.investmentInits = [...this.investmentInits, ...response.body.data]; 
    //     this.investmentInitPagination = response.body;
    //     return this.investmentInitPagination;
    //   })
    // );
    return this.http.get(this.baseUrl + 'investmentRec/investmentInits/'+empId+'/'+sbu);

  }
  getInvestmentRecommended(empId:number,sbu:string,userRole:string){    
    return this.http.get(this.baseUrl + 'investmentRec/investmentRecommended/'+empId+'/'+sbu+'/'+userRole);
    // let params = new HttpParams();
    // if (this.genParams.search) {
    //   params = params.append('search', this.genParams.search);
    // }
    // params = params.append('sort', this.genParams.sort);
    // params = params.append('pageIndex', this.genParams.pageIndex.toString());
    // params = params.append('pageSize', this.genParams.pageSize.toString());
    // return this.http.get<IInvestmentInitPagination>(this.baseUrl + 'investmentRec/investmentRecommended/'+empId+'/'+sbu+'/'+userRole, { observe: 'response', params })
    // //return this.http.get<IDonationPagination>(this.baseUrl + 'donation/donations', { observe: 'response', params })
    // .pipe(
    //   map(response => {
    //     this.investmentInits = [...this.investmentInits, ...response.body.data]; 
    //     this.investmentInitPagination = response.body;
    //     return this.investmentInitPagination;
    //   })
    // );
    
  }
  insertRecommendForOwnSBU(empId:number,sbu:string,investmentTargetedProds:IInvestmentTargetedProd[]) {
    var investmentRecComment=this.investmentRecCommentFormData;
    var investmentRec=this.investmentDetailFormData;
    var investmentRecProducts=investmentTargetedProds;
    var investmentRecOwnSBUInsertDto={investmentRecComment,investmentRec,investmentRecProducts}
    const headers = new HttpHeaders().set('Content-Type','application/json');
    return this.http.post(this.baseUrl+ 'investmentRec/insertRecommendForOwnSBU/'+empId+'/'+sbu, investmentRecOwnSBUInsertDto,{headers:headers});
  }
  insertRecommendForOtherSBU(empId:number,sbu:string,investmentTargetedProds:IInvestmentTargetedProd[]) {
    var investmentRecComment=this.investmentRecCommentFormData;
    var investmentRecProducts=investmentTargetedProds;
    var investmentRecOtherSBUInsertDto={investmentRecComment,investmentRecProducts}
    const headers = new HttpHeaders().set('Content-Type','application/json');
    return this.http.post(this.baseUrl+ 'investmentRec/insertRecommendForOtherSBU/'+empId+'/'+sbu, investmentRecOtherSBUInsertDto,{headers:headers});
  }
  updateRecommendForOwnSBU(empId:number,sbu:string,investmentTargetedProds:IInvestmentTargetedProd[]) {
    var investmentRecComment=this.investmentRecCommentFormData;
    var investmentRec=this.investmentDetailFormData;
    var investmentRecProducts=investmentTargetedProds;
    var investmentRecOwnSBUInsertDto={investmentRecComment,investmentRec,investmentRecProducts}
    const headers = new HttpHeaders().set('Content-Type','application/json');
    return this.http.post(this.baseUrl+ 'investmentRec/updateRecommendForOwnSBU/'+empId+'/'+sbu, investmentRecOwnSBUInsertDto,{headers:headers});
  }
  updateRecommendForOtherSBU(empId:number,sbu:string,investmentTargetedProds:IInvestmentTargetedProd[]) {
    var investmentRecComment=this.investmentRecCommentFormData;
    var investmentRecProducts=investmentTargetedProds;
    var investmentRecOtherSBUInsertDto={investmentRecComment,investmentRecProducts}
    const headers = new HttpHeaders().set('Content-Type','application/json');
    return this.http.post(this.baseUrl+ 'investmentRec/updateRecommendForOtherSBU/'+empId+'/'+sbu, investmentRecOtherSBUInsertDto,{headers:headers});
  }
  // insertInvestmentRec() {
  //   return this.http.post(this.baseUrl+ 'investmentRec/insertRecCom', this.investmentRecCommentFormData);
  // }
  
  // updateInvestmentRec() {
  //   return this.http.post(this.baseUrl+ 'investmentRec/updateRecCom',  this.investmentRecCommentFormData);
  // }

  // insertInvestmentDetail(empId:number,sbu:string) {
  //   return this.http.post(this.baseUrl+ 'investmentRec/insertRec/'+empId+'/'+this.investmentRecCommentFormData.recStatus+'/'+sbu, this.investmentDetailFormData);
  // }
  

  insertInvestmentMedicineProd() {
    return this.http.post(this.baseUrl + 'investmentRec/insertInvestmentMedicineProd', this.investmentMedicineProdFormData);
  }

  insertInvestmentTargetedProd(investmentTargetedProds:IInvestmentTargetedProd[]) {
    return this.http.post(this.baseUrl+ 'investmentRec/insertRecProd', investmentTargetedProds,
    {responseType: 'text'});
  }
  
  removeInvestmentTargetedProd() {
    debugger;
    return this.http.post(this.baseUrl+ 'investmentRec/removeInvestmentTargetedProd', this.investmentTargetedProdFormData,
    {responseType: 'text'});
  }

  removeInvestmentMedicineProd() {
    return this.http.post(this.baseUrl + 'investmentRec/removeInvestmentMedicineProd', this.investmentMedicineProdFormData,
      { responseType: 'text' });
  }
}

