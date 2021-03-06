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
import { IInvestmentInitPagination, InvestmentInitPagination } from '../shared/models/investmentPagination';
import { InvestmentRecDepot } from '../shared/models/InvestmentRecDepot';
import { InvestmentMedicineProd } from '../shared/models/investment';

@Injectable({
  providedIn: 'root'
})
export class InvestmentAprNoSbuService {
  investmentAprs: IInvestmentApr[]=[];
  investmentAprPagination = new InvestmentAprPagination();
  investmentInits: IInvestmentInit[]=[];
  investmentInitPagination = new InvestmentInitPagination();
  investmentDepotFormData: InvestmentRecDepot = new InvestmentRecDepot();
  investmentAprFormData: InvestmentInit = new InvestmentInit();
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
  getProduct(sbu:string){    
    return this.http.get(this.baseUrl + 'product/getProductForInvestment/'+sbu);
  }
  getMedicineProduct() {
    return this.http.get(this.baseUrl + 'product/getMedicineProductForInvestment');
  }
  getBudget(sbu:string,empID:number,donationId:number){    
    return this.http.get(this.baseUrl + 'approvalCeiling/getBudgetCeiling/'+empID+'/'+sbu+'/'+donationId);
  }
  getLastFiveInvestment(marketCode:string,toDayDate:string){    
    return this.http.get(this.baseUrl + 'investment/getLastFiveInvestment/'+marketCode+'/'+toDayDate);
  }
  async getLastFiveInvestmentForDoc(donationId:number,docId:number,marketCode: string, toDayDate: string) {
    return await  this.http.get(this.baseUrl + 'investment/getLastFiveInvestmentForDoc/' + donationId + '/' + docId + '/' +marketCode + '/' +toDayDate).toPromise();
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
  getCampaignMsts(empId:number){    
    return this.http.get(this.baseUrl + 'campaign/campaignMstsForInvestment/'+empId);
  }
  async getInvestmentDoctors(investmentInitId:number){    
    return await  this.http.get(this.baseUrl + 'investment/investmentDoctors/'+investmentInitId).toPromise();
  }
  getInvestmentAprComment(investmentInitId:number,empId:string){    
    return this.http.get(this.baseUrl + 'investmentAprNoSbu/getInvestmentAprComment/'+investmentInitId+'/'+parseInt(empId));
  }
  // getInvestmentDetails(investmentInitId:number){    
  //   return this.http.get(this.baseUrl + 'investmentRec/investmentRecDetails/'+investmentInitId).toPromise();
  // }
  getInvestmentTargetedProds(investmentInitId:number,sbu:string){    
    return this.http.get(this.baseUrl + 'investmentRec/investmentRecProducts/'+investmentInitId+'/'+sbu);
  }
  getInvestmentAprDetails(investmentInitId:number,empId:number){    
    return this.http.get(this.baseUrl + 'investmentAprNoSbu/investmentAprDetails/'+investmentInitId+'/'+empId).toPromise();
  }
  getInvestmentDetails(investmentInitId:number,empId:number){    
    return this.http.get(this.baseUrl + 'investmentAprNoSbu/investmentRecDetails/'+investmentInitId+'/'+empId).toPromise();
  }
  getInvestmentAprProducts(investmentInitId:number,sbu:string){    
    return this.http.get(this.baseUrl + 'investmentAprNoSbu/investmentAprProducts/'+investmentInitId+'/'+sbu);
  }
  getInvestmentTargetedGroups(investmentInitId:number,empId:number){    
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
    // return this.http.get<IInvestmentInitPagination>(this.baseUrl + 'investmentAprNoSbu/investmentInits/'+empId+'/'+sbu, { observe: 'response', params })
    // //return this.http.get<IDonationPagination>(this.baseUrl + 'donation/donations', { observe: 'response', params })
    // .pipe(
    //   map(response => {
    //     this.investmentInits = [...this.investmentInits, ...response.body.data]; 
    //     this.investmentInitPagination = response.body;
    //     return this.investmentInitPagination;
    //   })
    // );
    return this.http.get(this.baseUrl + 'investmentAprNoSbu/investmentInits/'+empId+'/'+sbu);

  }
  getGenParams(){
    return this.genParams;
  }
  setGenParams(genParams: GenericParams) {
    this.genParams = genParams;
  }
  getInvestmentApproved(empId:number,sbu:string,userRole:string){ 
    // let params = new HttpParams();
    // if (this.genParams.search) {
    //   params = params.append('search', this.genParams.search);
    // }
    // params = params.append('sort', this.genParams.sort);
    // params = params.append('pageIndex', this.genParams.pageIndex.toString());
    // params = params.append('pageSize', this.genParams.pageSize.toString());
    // return this.http.get<IInvestmentInitPagination>(this.baseUrl + 'investmentAprNoSbu/investmentApproved/'+empId+'/'+sbu+'/'+userRole, { observe: 'response', params })
    // //return this.http.get<IDonationPagination>(this.baseUrl + 'donation/donations', { observe: 'response', params })
    // .pipe(
    //   map(response => {
    //     this.investmentInits = [...this.investmentInits, ...response.body.data]; 
    //     this.investmentInitPagination = response.body;
    //     return this.investmentInitPagination;
    //   })
    // );
    return this.http.get(this.baseUrl + 'investmentAprNoSbu/investmentApproved/'+empId+'/'+sbu+'/'+userRole);
  }
  insertInvestmentApr() {
    
    return this.http.post(this.baseUrl+ 'investmentAprNoSbu/insertAprCom', this.investmentAprCommentFormData);

  }
  
  updateInvestmentApr() {
    return this.http.post(this.baseUrl+ 'investmentAprNoSbu/updateAprCom',  this.investmentAprCommentFormData);
  }
  insertInvestAprNoSBU(empId:number,sbu:string,investmentTargetedProds:IInvestmentTargetedProd[]) {
    var investmentRecComment=this.investmentAprCommentFormData;
    var investmentApr=this.investmentDetailFormData;
    var investmentRecProducts=investmentTargetedProds;
    var investmentNoSBUAprInsertDto={investmentRecComment,investmentApr,investmentRecProducts}
   const headers = new HttpHeaders().set('Content-Type','application/json');
    return this.http.post(this.baseUrl+ 'investmentAprNoSbu/insertInvestAprNoSBU/'+empId+'/'+this.investmentAprCommentFormData.recStatus+'/'+sbu+'/'+this.investmentAprFormData.donationId, investmentNoSBUAprInsertDto,{headers:headers});
  }
  updateInvestAprNoSBU(empId:number,sbu:string,investmentTargetedProds:IInvestmentTargetedProd[]) {
    var investmentRecComment=this.investmentAprCommentFormData;
    var investmentApr=this.investmentDetailFormData;
    var investmentRecProducts=investmentTargetedProds;
    var investmentNoSBUAprInsertDto={investmentRecComment,investmentApr,investmentRecProducts}
   const headers = new HttpHeaders().set('Content-Type','application/json');
    return this.http.post(this.baseUrl+ 'investmentAprNoSbu/updateInvestAprNoSBU/'+empId+'/'+this.investmentAprCommentFormData.recStatus+'/'+sbu+'/'+this.investmentAprFormData.donationId, investmentNoSBUAprInsertDto,{headers:headers});
  }
  insertInvestmentDetail(empId:number,sbu:string) {
    
    return this.http.post(this.baseUrl+ 'investmentAprNoSbu/insertApr/'+empId+'/'+this.investmentAprCommentFormData.recStatus+'/'+sbu+'/'+this.investmentAprFormData.donationId, this.investmentDetailFormData);
  
  }
  
  getInvestmentRecDepot(initId:any) {
    debugger;
    return this.http.get(this.baseUrl+ 'investmentAprNoSbu/getInvestmentRecDepot/'+initId).toPromise();
  }

  getEmpLoc(initId:any) {
    debugger;
    return this.http.get(this.baseUrl+ 'investmentAprNoSbu/getEmpMarket/'+initId);
  }
  
 
  
  insertInvestmentTargetedProd(investmentTargetedProds:IInvestmentTargetedProd[]) {
    
    return this.http.post(this.baseUrl+ 'investmentAprNoSbu/insertAprProd', investmentTargetedProds,
    {responseType: 'text'});

  }
  
  removeInvestmentTargetedProd() {
    
    return this.http.post(this.baseUrl+ 'investment/removeInvestmentTargetedProd', this.investmentTargetedProdFormData,
    {responseType: 'text'});

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

