import { RegApprovalPagination, IRegApprovalPagination } from './../shared/models/regApprovalPagination';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { IRegApproval,RegApproval} from'../shared/models/regApproval';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { BehaviorSubject, ReplaySubject, of, from } from 'rxjs';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router';
import { GenericParams } from '../shared/models/genericParams';

@Injectable({
  providedIn: 'root'
})
export class RegApprovalService {
  
  regApprovals: IRegApproval[]=[];
  regApprovalPagination = new RegApprovalPagination();
  regApprovalFormData: RegApproval = new RegApproval();
  
  
  
  baseUrl = environment.apiUrl;
  genParams = new GenericParams();

  
  constructor(private http: HttpClient, private router: Router) { }

  getEmployeeFormApproval(){ 
    return this.http.get(this.baseUrl + 'employee/employeeForApproval');   
  }
  getRegApproved(){ 
    return this.http.get(this.baseUrl + 'employee/employeeApproved');   
  }
    
    
  // getregApprovals(id:number){    
  //   //return this.http.get(this.baseUrl + 'regApproval/regApprovalDtls/'+id);
  //   let params = new HttpParams();
  //   debugger;
  //   if (this.genParams.search) {
  //     params = params.append('search', this.genParams.search);
  //   }
  //   params = params.append('sort', this.genParams.sort);
  //   params = params.append('pageIndex', this.genParams.pageNumber.toString());
  //   params = params.append('pageSize', this.genParams.pageSize.toString());
  //   return this.http.get<IregApprovalPaginationDtl>(this.baseUrl + 'regApproval/regApprovalDtls/'+id, { observe: 'response', params })
  //   //return this.http.get<IDonationPagination>(this.baseUrl + 'donation/donations', { observe: 'response', params })
  //   .pipe(
  //     map(response => {
  //       this.regApprovalDtls = [...this.regApprovalDtls, ...response.body.data]; 
  //       this.regApprovalPaginationDtl = response.body;
  //       return this.regApprovalPaginationDtl;
  //     })
  //   );
  // }
  
  getMarkets(){    
  return this.http.get(this.baseUrl + 'employee/marketForGroup');
 
}


updateRegApproval() {
   return this.http.post(this.baseUrl+ 'account/updateRegApproval',  this.regApprovalFormData);
 }
 

}

