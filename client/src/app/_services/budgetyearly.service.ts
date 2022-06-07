import { InvestmentInitPagination, IInvestmentInitPagination } from '../shared/models/investmentPagination';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import {
   InvestmentForm,IInvestmentForm,InvestmentMedicineProd ,InvestmentDetail, IInvestmentDetail,InvestmentTargetedProd
} from '../shared/models/investment';
import { InvestmentDoctor, IInvestmentDoctor, InvestmentInstitution, IInvestmentInstitution, InvestmentCampaign, IInvestmentCampaign } from '../shared/models/investment';
import { InvestmentBcds, IInvestmentBcds, InvestmentSociety, IInvestmentSociety } from '../shared/models/investment';

import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { BehaviorSubject, ReplaySubject, of, from } from 'rxjs';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router';
import { GenericParams } from '../shared/models/genericParams';
import { BudgetYearly, IBudgetYearly } from '../shared/models/budgetyearly';

@Injectable({
  providedIn: 'root'
})

export class BudgetYearlyService {
  budgetYearly: BudgetYearly = new BudgetYearly();
  baseUrl = environment.apiUrl;
  genParams = new GenericParams();


  constructor(private http: HttpClient, private router: Router) { }

  getBudgetYearly(){    
    debugger;
    return this.http.get(this.baseUrl + 'BgtYearly/getBudgetYearly');
  }
  getTotalExpense(deptId:number,year:number){    
    debugger;
    return this.http.get(this.baseUrl + 'BgtYearly/getTotalExpense/'+deptId+'/'+year);
  }
  getTotalPipeline(deptId:number,year:number){    
    debugger;
    return this.http.get(this.baseUrl + 'BgtYearly/getTotalPipeLine/'+deptId+'/'+year);
  }
  getBudgetAmount(deptId:number,year:number){    
    debugger;
    return this.http.get(this.baseUrl + 'BgtYearly/getBudgetAmount/'+deptId+'/'+year);
  }
  getTotalAllocated(deptId:number,year:number){    
    debugger;
    return this.http.get(this.baseUrl + 'BgtYearly/getTotalAllocated/'+deptId+'/'+year);
  }
  getGenParams(){
    return this.genParams;
  }
  submitBudgetYearly() {
    debugger;
    return this.http.post(this.baseUrl + 'BgtYearly/SaveBgtYearly', this.budgetYearly);
  }
 
  uploadBudgetYearlyFile(file:any) {
    debugger;
    return this.http.post(this.baseUrl + 'fileUpload/SaveFile', file);
  }
}

