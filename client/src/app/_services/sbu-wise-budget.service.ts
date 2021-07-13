import { ISBUWiseBudget,SBUWiseBudget } from './../shared/models/sbuWiseBudget';
import { ISBUWiseBudgetPagination, SBUWiseBudgetPagination } from './../shared/models/sbuWiseBudgetPagination';
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
export class SBUWiseBudgetService {
  roles: IRole[] = [];
  baseUrl = environment.apiUrl;
  genParams = new GenericParams();

  sbuWiseBudget: ISBUWiseBudget[]= [];
  sbuwiseBudgetPagination = new SBUWiseBudgetPagination();
  sbuwiseBudgeFormData: SBUWiseBudget = new SBUWiseBudget();

  constructor(private http: HttpClient, private router: Router) { }


  getSBUWiseBudget(){    
    let params = new HttpParams();
    debugger;
    if (this.genParams.search) {
      params = params.append('search', this.genParams.search);
    }
    params = params.append('sort', this.genParams.sort);
    params = params.append('pageIndex', this.genParams.pageNumber.toString());
    params = params.append('pageSize', this.genParams.pageSize.toString());

    return this.http.get<ISBUWiseBudgetPagination>(this.baseUrl + 'SBUWiseBudget/GetAllSBUBudget', { observe: 'response', params })
    .pipe(
      map(response => {
        this.sbuWiseBudget = [...this.sbuWiseBudget, ...response.body.data]; 
        this.sbuwiseBudgetPagination = response.body;
        return this.sbuwiseBudgetPagination;
      })
    ); 
  }

  insertSBUWiseBudget() {
    return this.http.post(this.baseUrl+ 'SBUWiseBudget/CreateSBUWiseBudget', this.sbuwiseBudgeFormData);
  }
  updateSBUWiseBudget() {
    return this.http.post(this.baseUrl+ 'SBUWiseBudget/ModifySBUWiseBudget',  this.sbuwiseBudgeFormData);
}

}