import { IRole, IRoleResponse } from '../shared/models/role';
import { IrptDepotLetterSearch, rptDepotLetterSearch, DepotLetterSearchPagination } from '../shared/models/rptInvestSummary';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { BehaviorSubject, ReplaySubject, of, from } from 'rxjs';
import { IUser, IUserResponse } from '../shared/models/user';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router';
import { GenericParams } from '../shared/models/genericParams';
import { DepotPrintTrack } from '../shared/models/depotPrintTrack';



@Injectable({
  providedIn: 'root'
})
export class DepotPendingService {

  depotPrintFormData: DepotPrintTrack = new DepotPrintTrack();
  rptDepotLetter: IrptDepotLetterSearch[]=[];
  pagination = new DepotLetterSearchPagination();
 

  roles: IRole[] = [];
  baseUrl = environment.apiUrl;
  genParams = new GenericParams();

  constructor(private http: HttpClient, private router: Router) { }
  getGenParams(){
    return this.genParams;
  }

   // tslint:disable-next-line: typedef
   setGenParams(genParams: GenericParams) {
    this.genParams = genParams;
  }

  // getPendingReport(empId:number){    
  //   let params = new HttpParams();
  //   debugger;
  //   if (this.genParams.search) {
  //     params = params.append('search', this.genParams.search);
  //   }
  //   params = params.append('sort', this.genParams.sort);
  //   params = params.append('pageIndex', this.genParams.pageIndex.toString());
  //   params = params.append('pageSize', this.genParams.pageSize.toString());
  //   return this.http.get<IDepotLetterPagination>(this.baseUrl + 'depotPrinting/pendingForPrint/'+ empId, { observe: 'response', params })
  //   .pipe(
  //     map(response => {
  //       this.rptDepotLetter = [...this.rptDepotLetter, ...response.body.data]; 
  //       this.pagination = response.body;
  //       return this.pagination;
  //     })
  //   );
  // }

  getPendingReport(empId:number){    
    return this.http.get(this.baseUrl + 'depotPrinting/pendingForPrint/'+ empId);
  }

  getRptDepotLetter(initId:any) {
    return this.http.get(this.baseUrl+ 'reportInvestment/rptInvestDepo/'+initId);
  }

  insertTrackReport(depotPrintFormData: any) {
    debugger;
    return this.http.post(this.baseUrl+ 'depotPrintTrack/createTrackRecord', depotPrintFormData);
  }
  
}

