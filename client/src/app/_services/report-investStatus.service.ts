import { rptInvestStatusPagination, IrptInvestStatusPagination } from '../shared/models/rptInvestStatusPagination';
import { IrptInvestStatus, rptInvestStatus } from '../shared/models/rptInvestStatus';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { BehaviorSubject, ReplaySubject, of, from } from 'rxjs';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router';
import { GenericParams } from '../shared/models/genericParams';

@Injectable({
  providedIn: 'root'
})
export class RptInvestStatusService {
  
  rptInvestStatus: IrptInvestStatus[]=[];
  rptInvestStatusPagination = new rptInvestStatusPagination();
  rptInvestStatusFormData: rptInvestStatus = new rptInvestStatus();

  baseUrl = environment.apiUrl;
  genParams = new GenericParams();
  constructor(private http: HttpClient, private router: Router) { }
 
  getGenParams(){
    return this.genParams;
  }

  setGenParams(genParams: GenericParams) {
    this.genParams = genParams;
  }
  getDoctors() {
    return this.http.get(this.baseUrl + 'doctor/doctorsForReport');
  }

  GetInvestmentSummaryReport(model: any){ 
    let params = new HttpParams();
    
    if (this.genParams.search) {
      params = params.append('search', this.genParams.search);
    }
    params = params.append('sort', this.genParams.sort);
    params = params.append('pageIndex', this.genParams.pageIndex.toString());
    params = params.append('pageSize', this.genParams.pageSize.toString());

    return this.http.post<IrptInvestStatusPagination>(this.baseUrl + 'reportInvestment/GetInvestmentSummaryReport', model, { observe: 'response', params })
    .pipe(
      map(response => {
        this.rptInvestStatus = [...this.rptInvestStatus, ...response.body.data]; 
        this.rptInvestStatusPagination = response.body;
        return this.rptInvestStatusPagination;
      })
    );   
  }


  GetInvestmentSummarySingle(referenceNo: string){ 
    return this.http.get(this.baseUrl+ 'reportInvestment/getInvestmentSummarySingle/'+referenceNo);  
  }
  GetInvestmentSummarySingleDoc(referenceNo: string,doctorId:any,doctorName:string){ 
    debugger;
    if(referenceNo==undefined || referenceNo=="")
    {
      referenceNo="undefined";
    }
    if(doctorId==undefined || doctorId=="")
    {
      doctorId=0;
    }
    if(doctorName==undefined || doctorName=="")
    {
      doctorName="undefined";
    }
    return this.http.get(this.baseUrl+ 'reportInvestment/getInvestmentSummarySingleDoc/'+referenceNo+'/'+doctorName+'/'+doctorId);  
  }
  IsInvestmentInActive(referenceNo: string){ 
    return this.http.get(this.baseUrl+ 'reportInvestment/isInvestmentInActive/'+referenceNo);  
  }
  IsInvestmentInActiveDoc(referenceNo: string,doctorId:number,doctorName:string){ 
    return this.http.get(this.baseUrl+ 'reportInvestment/IsInvestmentInActiveDoc/'+referenceNo+'/'+doctorId+'/'+doctorName);  
  }

  GetParamInvestmentSummaryReport(model: any){ 
    let params = new HttpParams();
    
    if (this.genParams.search) {
      params = params.append('search', this.genParams.search);
    }
    params = params.append('sort', this.genParams.sort);
    params = params.append('pageIndex', this.genParams.pageIndex.toString());
    params = params.append('pageSize', this.genParams.pageSize.toString());

    return this.http.post<IrptInvestStatusPagination>(this.baseUrl + 'reportInvestment/GetParamInvestmentSummaryReport', model, { observe: 'response', params })
    .pipe(
      map(response => {
        this.rptInvestStatus = [...this.rptInvestStatus, ...response.body.data]; 
        this.rptInvestStatusPagination = response.body;
        return this.rptInvestStatusPagination;
      })
    );   
  }

  getRptDepotLetter(initId:any) {
    return this.http.get(this.baseUrl+ 'reportInvestment/rptInvestDepo/'+initId);
  }


  getEmpMonthlyExpense(model: any) {
    return this.http.post(this.baseUrl+ 'reportInvestment/GetEmpMonthlyExpense/', model);
  }

  getApprovalAuthority(){    
    return this.http.get(this.baseUrl + 'approvalAuthority/approvalAuthoritiesForConfig');
  }

  getDonations() {
    return this.http.get(this.baseUrl + 'donation/donationsForInvestment');
  }

  getCampaignMsts() {
    return this.http.get(this.baseUrl + 'reportInvestment/campaignMsts/');
  }

  getCampaignSummaryReport(model: any) {
    return this.http.post(this.baseUrl+ 'reportInvestment/GetCampaignSummaryReport/', model);
  }

}

