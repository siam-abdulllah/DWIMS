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
import { BudgetEmpSbuMap } from '../shared/models/BudgetEmpSbuMap';

@Injectable({
  providedIn: 'root'
})
export class BudgetEmpSbuMapervice {
  budgetSbuMap: BudgetEmpSbuMap = new BudgetEmpSbuMap();
  baseUrl = environment.apiUrl;
  genParams = new GenericParams();

  constructor(private http: HttpClient, private router: Router) { }

  getEmployees(){    
    return this.http.get(this.baseUrl + 'employee/employeesForSbuMapping');
  }
  getSBU(){    
    return this.http.get(this.baseUrl + 'employee/getSBU');
  }

  getEmpSbuMappingListByDept(deptId:number) {
    return this.http.get(this.baseUrl + 'employee/getEmpSbuMappingListByDept/'+deptId);
  }
  getEmpSbuMappingListBySbu(sbu:string) {
    return this.http.get(this.baseUrl + 'employee/getEmpSbuMappingListBySbu/'+sbu);
  }
  getEmpSbuMappingList(deptId:number,sbu:string) {
    return this.http.get(this.baseUrl + 'employee/getEmpSbuMappingList/'+deptId+'/'+sbu);
  }
  removeEmpSbuMapping(selectedRecord:BudgetEmpSbuMap) {
    return this.http.post(this.baseUrl + 'employee/removeEmpSbuMapping', selectedRecord,
      { responseType: 'text' });

  }
  
  SaveEmpSbuMapping() {
    debugger;
    return this.http.post(this.baseUrl + 'employee/SaveEmpSbuMapping', this.budgetSbuMap);
  }
 
  
}

