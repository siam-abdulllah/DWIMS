import { ApprovalAuthConfigPagination, IApprovalAuthConfigPagination } from './../shared/models/approvalAuthConfigPagination';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { IApprovalAuthConfig,ApprovalAuthConfig} from'../shared/models/approvalAuthConfig';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { BehaviorSubject, ReplaySubject, of, from } from 'rxjs';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router';
import { GenericParams } from '../shared/models/genericParams';

@Injectable({
  providedIn: 'root'
})
export class ApprAuthConfigService {
  
  approvalAuthConfigs: IApprovalAuthConfig[]=[];
  approvalAuthConfigPagination = new ApprovalAuthConfigPagination();
  approvalAuthConfigFormData: ApprovalAuthConfig = new ApprovalAuthConfig();
  
  
  
  baseUrl = environment.apiUrl;
  genParams = new GenericParams();

  
  constructor(private http: HttpClient, private router: Router) { }

  getApprovalAuthConfigs(id:number){    
    return this.http.get(this.baseUrl + 'apprAuthConfig/employeesForConfigByAuthId/'+id);
    
  }
  
getApprovalAuthority(){    
  return this.http.get(this.baseUrl + 'approvalAuthority/approvalAuthoritiesForConfig');
 
}
getEmployees(){    
  return this.http.get(this.baseUrl + 'employee/employeesForConfig');
 
}

insertApprAuthConfig() {
  
  return this.http.post(this.baseUrl+ 'apprAuthConfig/insert', this.approvalAuthConfigFormData);

}
// updateApprAuthConfig() {
//   return this.http.post(this.baseUrl+ 'approvalAuthorityConfig/update',  this.approvalAuthConfigFormData);
// }

}

