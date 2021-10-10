import { IApprovalTimeLimit, ApprovalTimeLimit } from './../shared/models/approvalTimeLimit';
import { ApprovalTimeLimitPagination, IApprovalTimeLimitPagination } from './../shared/models/approvalTimeLimitPagination';
import { IRole, IRoleResponse } from './../shared/models/role';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { BehaviorSubject, ReplaySubject, of, from } from 'rxjs';
import { IUser, IUserResponse } from '../shared/models/user';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router';
import { GenericParams } from '../shared/models/genericParams';
import { IPagination, Pagination } from '../shared/models/pagination';

@Injectable({
  providedIn: 'root'
})
export class ApprovalTimeLimitService {
  roles: IRole[] = [];
  baseUrl = environment.apiUrl;
  genParams = new GenericParams();

  approvalTimeLimit: IApprovalTimeLimit[]= [];
  approvalTimeLimitPagination = new ApprovalTimeLimitPagination();
  approvalTimeLimitFormData: ApprovalTimeLimit = new ApprovalTimeLimit();

  constructor(private http: HttpClient, private router: Router) { }

  getApprovalAuthority(){    
    return this.http.get(this.baseUrl + 'approvalAuthority/approvalAuthoritiesForConfig');
   
  }
  getApprovalTimeLimit(){    
    let params = new HttpParams();
    
    if (this.genParams.search) {
      params = params.append('search', this.genParams.search);
    }
    params = params.append('sort', this.genParams.sort);
    params = params.append('pageIndex', this.genParams.pageIndex.toString());
    params = params.append('pageSize', this.genParams.pageSize.toString());

    return this.http.get<IApprovalTimeLimitPagination>(this.baseUrl + 'approvalTimeLimit/GetAllApprovalTime', { observe: 'response', params })
    .pipe(
      map(response => {
        this.approvalTimeLimit = [...this.approvalTimeLimit, ...response.body.data]; 
        this.approvalTimeLimitPagination = response.body;
        return this.approvalTimeLimitPagination;
      })
    ); 
  }

  insertApprovalTimeLimit() {
    
    //this.approvalTimeLimitFormData.approvalAuthorityId=this.approvalTimeLimitFormData.approvalAuthorityId;
    return this.http.post(this.baseUrl+ 'approvalTimeLimit/CreateApprovalTimeLimit', this.approvalTimeLimitFormData);
  }
  updateApprovalTimeLimit() {
    return this.http.post(this.baseUrl+ 'approvalTimeLimit/ModifyApprovalTimeLimit',  this.approvalTimeLimitFormData);
}

}