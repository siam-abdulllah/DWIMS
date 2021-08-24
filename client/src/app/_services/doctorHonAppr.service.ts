import { DoctorHonApprPagination , IDoctorHonApprPagination} from '../shared/models/doctorHonApprPagination';
import { IDoctorHonAppr, DoctorHonAppr } from '../shared/models/doctorHonAppr';
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
export class DoctorHonApprService {
  roles: IRole[] = [];
  baseUrl = environment.apiUrl;
  genParams = new GenericParams();

  doctorHonAppr: IDoctorHonAppr[]= [];
  doctorHonApprPagination = new DoctorHonApprPagination();
  doctorHonApprFormData: DoctorHonAppr = new DoctorHonAppr();

  constructor(private http: HttpClient, private router: Router) { }
  getApprovalAuthority(){    
    return this.http.get(this.baseUrl + 'approvalAuthority/approvalAuthoritiesForConfig');
  }
  getDonations(){    
    return this.http.get(this.baseUrl + 'donation/donationsForInvestment');
  }
  getDoctorHonAppr(fDate:string){   
     
    let params = new HttpParams();
    
    if (this.genParams.search) {
      params = params.append('search', this.genParams.search);
    }
    params = params.append('sort', this.genParams.sort);
    params = params.append('pageIndex', this.genParams.pageNumber.toString());
    params = params.append('pageSize', this.genParams.pageSize.toString());

    return this.http.get<IDoctorHonApprPagination>(this.baseUrl + 'doctorHonAppr/GetAllData/'+fDate, { observe: 'response', params })
    .pipe(
      map(response => {
        this.doctorHonAppr = [...this.doctorHonAppr, ...response.body.data]; 
        this.doctorHonApprPagination = response.body;
        return this.doctorHonApprPagination;
      })
    ); 
  }

  insertDocHonAppr(doctorHonApprFormData:IDoctorHonAppr) {
    return this.http.post(this.baseUrl+ 'doctorHonAppr/insertDocHonAppr', doctorHonApprFormData);
  }
  updateDocHonAppr(doctorHonApprFormData:IDoctorHonAppr) {
    return this.http.post(this.baseUrl+ 'doctorHonAppr/updateDocHonAppr', doctorHonApprFormData);
  }
  

}