import { ChangeDepot } from './../shared/models/changeDepot';
import { IRole, IRoleResponse } from '../shared/models/role';
import { DepotLetterSearchPagination } from '../shared/models/rptInvestSummary';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { BehaviorSubject, ReplaySubject, of, from } from 'rxjs';
import { IUser, IUserResponse } from '../shared/models/user';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router';
import { GenericParams } from '../shared/models/genericParams';



@Injectable({
  providedIn: 'root'
})
export class ChangeDepotService {
  pagination = new DepotLetterSearchPagination();
  changeDepotFormData: ChangeDepot = new ChangeDepot();
 

  roles: IRole[] = [];
  baseUrl = environment.apiUrl;
  genParams = new GenericParams();

  constructor(private http: HttpClient, private router: Router) { }
  getGenParams(){
    return this.genParams;
  }

  getDepotList(empId:number,userRole:string){    
    return this.http.get(this.baseUrl + 'changeDepot/invListForDepotChange/'+ empId+'/'+userRole);
  }

   setGenParams(genParams: GenericParams) {
    this.genParams = genParams;
  }

  insertChange(changeFormData: any) {
    return this.http.post(this.baseUrl+ 'changeDepot/createChangeDepot', changeFormData);
  }

  getRptDepotLetter(initId:any) {
    return this.http.get(this.baseUrl+ 'reportInvestment/rptInvestDepo/'+initId);
  }

  getDepot() {
    return this.http.get(this.baseUrl + 'employee/depotForInvestment');
  }

  getEmpDepot(empId: number) {
    return this.http.get(this.baseUrl + 'employee/getEmpDepot/' + empId );
  }
}