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

  sbuWiseBudgets: ISBUWiseBudget[]= [];
  sbuwiseBudgetPagination = new SBUWiseBudgetPagination();
  sbuwiseBudgeFormData: SBUWiseBudget = new SBUWiseBudget();

  constructor(private http: HttpClient, private router: Router) { }

  getGenParams(){
    return this.genParams;
  }

   // tslint:disable-next-line: typedef
   setGenParams(genParams: GenericParams) {
    this.genParams = genParams;
  }


  getSBU(){    
    return this.http.get(this.baseUrl + 'employee/getSBU');
  }
  getSBUWiseBudget(){    
    let params = new HttpParams();
    
    if (this.genParams.search) {
      params = params.append('search', this.genParams.search);
    }
    params = params.append('sort', this.genParams.sort);
    params = params.append('pageIndex', this.genParams.pageIndex.toString());
    params = params.append('pageSize', this.genParams.pageSize.toString());

    return this.http.get<ISBUWiseBudgetPagination>(this.baseUrl + 'sBUWiseBudget/GetAllSBUBudget', { observe: 'response', params })
    .pipe(
      map(response => {
        this.sbuWiseBudgets = [...this.sbuWiseBudgets, ...response.body.data]; 
        this.sbuwiseBudgetPagination = response.body;
        return this.sbuwiseBudgetPagination;
      })
    ); 
  }

  removeSBUWiseBudget(sbuwiseBudgeFormData: ISBUWiseBudget) {
    return this.http.post(this.baseUrl+ 'sBUWiseBudget/removeSBUWiseBudget', sbuwiseBudgeFormData,
    {responseType: 'text'});
  }
  insertSBUWiseBudget() {
    return this.http.post(this.baseUrl+ 'sBUWiseBudget/CreateSBUWiseBudget', this.sbuwiseBudgeFormData);
  }
  updateSBUWiseBudget() {
    debugger;
    return this.http.post(this.baseUrl+ 'sBUWiseBudget/ModifySBUWiseBudget',  this.sbuwiseBudgeFormData);
}
getDonations(){    
  return this.http.get(this.baseUrl + 'donation/donationsForInvestment');
}
}