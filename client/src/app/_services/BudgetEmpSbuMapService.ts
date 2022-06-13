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
  getEmpWiseData(employeeId: any ){    
    return this.http.get(this.baseUrl + 'bgtOwn/getEmpWiseData/'+employeeId);
  }
  getEmpSbuMappingListByEmp(empId: any,authId: any,sbu:any,deptId ){    
    return this.http.get(this.baseUrl + 'bgtOwn/getEmpSbuMappingListByEmp/'+empId+'/'+authId+'/'+sbu+'/'+deptId);
  }
  getEmployees(){    
    return this.http.get(this.baseUrl + 'employee/employeesForSbuMapping');
  }
  getApprovalAuthority(){    
    return this.http.get(this.baseUrl + 'approvalAuthority/approvalAuthoritiesForConfig');
   
  }
  getSBU(){    
    return this.http.get(this.baseUrl + 'employee/getSBU');
  }

  getEmpSbuMappingListByDept(deptId:number) {
    return this.http.get(this.baseUrl + 'empSbuMap/getEmpSbuMappingListByDept/'+deptId);
  }
  getEmpSbuMappingListBySbu(sbu:string) {
    return this.http.get(this.baseUrl + 'empSbuMap/getEmpSbuMappingListBySbu/'+sbu);
  }
  getEmpSbuMappingList(deptId:number,sbu:string) {
    return this.http.get(this.baseUrl + 'empSbuMap/getEmpSbuMappingList/'+deptId+'/'+sbu);
  }
  removeEmpSbuMapping(selectedRecord:BudgetEmpSbuMap) {
    return this.http.post(this.baseUrl + 'empSbuMap/removeEmpSbuMapping', selectedRecord,
      { responseType: 'text' });

  }
  
  SaveEmpSbuMapping() {
    debugger;
    return this.http.post(this.baseUrl + 'empSbuMap/SaveEmpSbuMapping', this.budgetSbuMap);
  }
 
  
}

