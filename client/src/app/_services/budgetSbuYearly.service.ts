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
import { ApprovalAuthDetails, ApprovalAuthDetailsModel, BudgetSbuYearly, BudgetYearly, IBudgetYearly, SbuDetails } from '../shared/models/budgetyearly';

@Injectable({
  providedIn: 'root'
})
export class BudgetSbuYearlyService {
  budgetSbuYearly: BudgetSbuYearly = new BudgetSbuYearly();
  budgetEmployee: ApprovalAuthDetails = new ApprovalAuthDetails();
  approvalAuthDetailsModel: ApprovalAuthDetailsModel = new ApprovalAuthDetailsModel();
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
  getAppAuthDetails(sbuCode:string,deptId:number){    
    return this.http.get(this.baseUrl + 'BgtSbuYearly/getAppAuthDetails/'+sbuCode+'/'+deptId);
  }
  getCampaignBgtDetails(sbuCode:string,deptId:number){    
    return this.http.get(this.baseUrl + 'BgtSbuYearly/getCampaignDetails/'+sbuCode+'/'+deptId);
  }
  
  getbgtEmployeeForSbu(sbuName:string,deptId:number,year:number,comId:number){    
    return this.http.get(this.baseUrl + 'BgtSbuYearly/getBudgetEmpForSbu/'+sbuName+'/'+deptId+'/'+year+'/'+comId);
  }
  
  getAllSbuBgtList(deptId:number,compId:number,year:number) {
    debugger;
    return this.http.get(this.baseUrl + 'BgtSbuYearly/getAllSbuBgtList/'+deptId+'/'+compId+'/'+year);
  }
  getSbuWisePipeLineExpense(deptId:number,compId:number,year:number) {
    debugger;
    return this.http.get(this.baseUrl + 'BgtSbuYearly/getAllSbuBgtList/'+deptId+'/'+compId+'/'+year);
  }
  getTotalExpense(deptId:number,year:number){    
    debugger;
    return this.http.get(this.baseUrl + 'BgtYearly/getTotalExpense/'+deptId+'/'+year);
  }
  getAllPipelineExpenseList(deptId:number,compId:number,year:number) {
    debugger;
    return this.http.get(this.baseUrl + 'BgtSbuYearly/getAllPipelineExpenseList/'+deptId+'/'+compId+'/'+year);
  }
  getAllAuthExpenseList(sbu:string,deptId:number,year:number,compId:number) {
    debugger;
    return this.http.get(this.baseUrl + 'BgtSbuYearly/getAllAuthExpenseList/'+sbu+'/'+deptId+'/'+year+'/'+compId);
  }
  updateSbuBudgetYearly() {
    debugger;
    return this.http.post(this.baseUrl + 'BgtSbuYearly/updateSbuBudgetYearly', this.budgetSbuYearly);
  }
  updateBgtEmployee() {
    debugger;
    return this.http.post(this.baseUrl + 'BgtSbuYearly/updateBgtEmployee', this.budgetEmployee);
  }
  saveAuthSbuDetails() {
    debugger;
    return this.http.post(this.baseUrl + 'BgtSbuYearly/saveAuthSbuDetails', this.approvalAuthDetailsModel);
  }
  submitSbuBudgetYearly() {
    debugger;
    return this.http.post(this.baseUrl + 'BgtSbuYearly/SaveSbuBgtYearly', this.budgetSbuYearly);
  }
 
  
}

