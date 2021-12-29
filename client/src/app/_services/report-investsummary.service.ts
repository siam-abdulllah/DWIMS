import { rptInvestSummaryPagination, IrptInvestSummaryPagination } from '../shared/models/rptInvestSummaryPagination';
import { IrptInvestSummary, rptInvestSummary } from '../shared/models/rptInvestSummary';
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
export class RptInvestSummaryService {
  
  rptInvestSummary: IrptInvestSummary[]=[];
  rptInvestSummaryPagination = new rptInvestSummaryPagination();
  rptInvestSummaryFormData: rptInvestSummary = new rptInvestSummary();

  baseUrl = environment.apiUrl;
  genParams = new GenericParams();
  constructor(private http: HttpClient, private router: Router) { }
 
  getGenParams(){
    return this.genParams;
  }

  setGenParams(genParams: GenericParams) {
    this.genParams = genParams;
  }


  GetInvestmentSummaryReport(model: any){ 
    let params = new HttpParams();
    
    if (this.genParams.search) {
      params = params.append('search', this.genParams.search);
    }
    params = params.append('sort', this.genParams.sort);
    params = params.append('pageIndex', this.genParams.pageIndex.toString());
    params = params.append('pageSize', this.genParams.pageSize.toString());

    return this.http.post<IrptInvestSummaryPagination>(this.baseUrl + 'reportInvestment/GetInvestmentSummaryReport', model, { observe: 'response', params })
    .pipe(
      map(response => {
        this.rptInvestSummary = [...this.rptInvestSummary, ...response.body.data]; 
        this.rptInvestSummaryPagination = response.body;
        return this.rptInvestSummaryPagination;
      })
    );   
  }


  GetParamInvestmentSummaryReport(model: any){ 
    let params = new HttpParams();
    
    if (this.genParams.search) {
      params = params.append('search', this.genParams.search);
    }
    params = params.append('sort', this.genParams.sort);
    params = params.append('pageIndex', this.genParams.pageIndex.toString());
    params = params.append('pageSize', this.genParams.pageSize.toString());

    return this.http.post<IrptInvestSummaryPagination>(this.baseUrl + 'reportInvestment/GetParamInvestmentSummaryReport', model, { observe: 'response', params })
    .pipe(
      map(response => {
        this.rptInvestSummary = [...this.rptInvestSummary, ...response.body.data]; 
        this.rptInvestSummaryPagination = response.body;
        return this.rptInvestSummaryPagination;
      })
    );   
  }

}

