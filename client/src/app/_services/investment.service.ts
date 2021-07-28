import { InvestmentInitPagination, IInvestmentInitPagination } from '../shared/models/investmentPagination';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { IInvestmentInit,InvestmentInit} from'../shared/models/investment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { BehaviorSubject, ReplaySubject, of, from } from 'rxjs';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router';
import { GenericParams } from '../shared/models/genericParams';

@Injectable({
  providedIn: 'root'
})
export class InvestmentInitService {
  investmentinits: IInvestmentInit[]=[];
  investmentinitPagination = new InvestmentInitPagination();
  investmentinitFormData: InvestmentInit = new InvestmentInit();
  baseUrl = environment.apiUrl;
  genParams = new GenericParams();

  
  constructor(private http: HttpClient, private router: Router) { }

  getDonations(){    
    return this.http.get(this.baseUrl + 'donation/donations');
    
  }
  getMarkets(){    
    return this.http.get(this.baseUrl + 'employee/marketForGroup');
   
  }
  getProduct(){    
    return this.http.get(this.baseUrl + 'product/getProduct');
    
  }
getApprovalAuthority(){    
  return this.http.get(this.baseUrl + 'approvalAuthority/approvalAuthoritiesForConfig');
 
}
getEmployees(){    
  return this.http.get(this.baseUrl + 'employee/employeesForConfig');
 
}

insertInvestmentInit() {
  debugger;
  return this.http.post(this.baseUrl+ 'investmentInit/insert', this.investmentinitFormData);

}
updateInvestmentInit() {
  return this.http.post(this.baseUrl+ 'approvalAuthorityConfig/update',  this.investmentinitFormData);
}

}

