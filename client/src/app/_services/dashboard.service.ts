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
export class DashboardService {
  
//   regApprovals: IRegApproval[]=[];
//   regApprovalPagination = new RegApprovalPagination();
//   regApprovalFormData: RegApproval = new RegApproval();
  
  
  
  baseUrl = environment.apiUrl;
  genParams = new GenericParams();

  
  constructor(private http: HttpClient, private router: Router) { }

    
  getTotalApproved(role: string, empCode: string){    
    return this.http.get(this.baseUrl + 'dashboard/totalApproved/'+role+'/'+empCode);
  }

  getMyPending(role: string, empCode: string){    
    return this.http.get(this.baseUrl + 'dashboard/myPendingCount/'+role+'/'+empCode);
    //return this.http.get(this.baseUrl + 'dashboard/myPending/'+role+'/'+empCode);
  }

  getApprovalPending(role: string, empCode: string){    
    return this.http.get(this.baseUrl + 'dashboard/approvalPending/'+role+'/'+empCode);
  }
// updateRegApproval() {
//    return this.http.post(this.baseUrl+ 'account/updateRegApproval',  this.regApprovalFormData);
//   }
}

