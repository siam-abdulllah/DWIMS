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
import { BudgetSbuYearly, BudgetYearly, IBudgetYearly, SbuDetails } from '../shared/models/budgetyearly';

@Injectable({
  providedIn: 'root'
})
export class BudgetSbuYearlyService {
  budgetSbuYearly: BudgetSbuYearly = new BudgetSbuYearly();
  sbuDetailsYearly: SbuDetails = new SbuDetails();
  baseUrl = environment.apiUrl;
  genParams = new GenericParams();


  constructor(private http: HttpClient, private router: Router) { }
  getSBU(){    
    return this.http.get(this.baseUrl + 'employee/getSBU');
  }
  getYearlyBudget(deptId:number,year:number){    
    return this.http.get(this.baseUrl + 'BgtSbuYearly/getYearlyBudget/'+deptId+'/'+year);
  }
  getAllSbuBgtList(deptId:number,compId:number,year:number) {
    debugger;
    return this.http.get(this.baseUrl + 'BgtSbuYearly/getAllSbuBgtList/'+deptId+'/'+compId+'/'+year);
  }
  getSbuWisePipeLineExpense(deptId:number,compId:number,year:number) {
    debugger;
    return this.http.get(this.baseUrl + 'BgtSbuYearly/getAllSbuBgtList/'+deptId+'/'+compId+'/'+year);
  }
  getAllPipelineExpenseList(deptId:number,compId:number,year:number) {
    debugger;
    return this.http.get(this.baseUrl + 'BgtSbuYearly/getAllPipelineExpenseList/'+deptId+'/'+compId+'/'+year);
  }
  submitSbuBudgetYearly() {
    debugger;
    return this.http.post(this.baseUrl + 'BgtSbuYearly/SaveSbuBgtYearly', this.budgetSbuYearly);
  }
 
  
}

