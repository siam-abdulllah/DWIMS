import { IReportConfig, IReportConfigPagination, ReportConfigPagination } from '../report-investment/report-investment.component';
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
export class DepotPendingService {
  reportConfig: IReportConfig[]=[];

  roles: IRole[] = [];
  baseUrl = environment.apiUrl;
  genParams = new GenericParams();
  reportpagination = new ReportConfigPagination();

  constructor(private http: HttpClient, private router: Router) { }
  getGenParams(){
    return this.genParams;
  }

   // tslint:disable-next-line: typedef
   setGenParams(genParams: GenericParams) {
    this.genParams = genParams;
  }


}

