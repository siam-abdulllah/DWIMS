import { ApprovalCeilingPagination , IApprovalCeilingPagination} from './../shared/models/approvalCeilingPagination';
import { IApprovalCeiling, ApprovalCeiling } from './../shared/models/approvalCeiling';
import { IRole, IRoleResponse } from '../shared/models/role';
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
export class ApprovalCeilingService {
  roles: IRole[] = [];
  baseUrl = environment.apiUrl;
  genParams = new GenericParams();

  approvalCeiling: IApprovalCeiling[]= [];
  approvalCeilingPagination = new ApprovalCeilingPagination();
  approvalCeilingFormData: ApprovalCeiling = new ApprovalCeiling();

  constructor(private http: HttpClient, private router: Router) { }


  getApprovalCeiling(){    
    let params = new HttpParams();
    debugger;
    if (this.genParams.search) {
      params = params.append('search', this.genParams.search);
    }
    params = params.append('sort', this.genParams.sort);
    params = params.append('pageIndex', this.genParams.pageNumber.toString());
    params = params.append('pageSize', this.genParams.pageSize.toString());

    return this.http.get<IApprovalCeilingPagination>(this.baseUrl + 'ApprovalCeiling/GetAllData', { observe: 'response', params })
    .pipe(
      map(response => {
        this.approvalCeiling = [...this.approvalCeiling, ...response.body.data]; 
        this.approvalCeilingPagination = response.body;
        return this.approvalCeilingPagination;
      })
    ); 
  }

  insertApprovalCeiling() {
    return this.http.post(this.baseUrl+ 'ApprovalCeiling/CreateApprovalCeiling', this.approvalCeilingFormData);
  }
  updateApprovalCeiling() {
    return this.http.post(this.baseUrl+ 'ApprovalCeiling/ModifyApprovalCeiling',  this.approvalCeilingFormData);
}

}