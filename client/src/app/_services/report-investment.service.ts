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
export class ReportInvestmentService {
  roles: IRole[] = [];
  baseUrl = environment.apiUrl;
  genParams = new GenericParams();

  constructor(private http: HttpClient, private router: Router) { }

  getInsSocietyBCDSWiseInvestment(model: any) {
     return this.http.post(this.baseUrl + 'GetDateWiseProformaByImporter', model);
  }

  // getImporterWiseCurrentYearProforma(model: any) {
  //   return this.http.post(this.baseUrl + 'GetImporterWiseCurrentYearProforma', model);
  // }
  // getCurrentYearProformaInfo(model: any) {
  //   return this.http.post(this.baseUrl + 'GetCurrentYearProformaInfo', model);
  // }
  // getDateWiseProformaInfos(model: any) {
  //   return this.http.post(this.baseUrl + 'GetDateWiseProformaInfos', model);
  // }


}

