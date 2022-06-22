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
import { BgtEmpInsertDto, BgtOwnInsertDto} from '../shared/models/bgtEmployee';
import { SbuData} from '../bgtEmployee/bgtEmployee.component';




@Injectable({
  providedIn: 'root'
})
export class BgtEmpDisburseService {

  bgtEmpFormData: BgtEmpInsertDto = new BgtEmpInsertDto();
  bgtOwnFormData: BgtOwnInsertDto = new BgtOwnInsertDto();
  rptDepotLetter: IrptDepotLetterSearch[]=[];
  pagination = new DepotLetterSearchPagination();
 
  sbuData:SbuData[] = [];
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

  getDonations() {
    return this.http.get(this.baseUrl + 'donation/donationsForInvestment');
  }

  getApprovalAuthority(){    
    return this.http.get(this.baseUrl + 'bgtEmployee/approvalAuthoritiesForConfig');
  }
  
  getSBU(){    
    return this.http.get(this.baseUrl + 'employee/getSBU');
  }

  getSBUWiseEmpDisburse(sbu: string, deptId: any, year: any, authId: any ){    
    return this.http.get(this.baseUrl + 'bgtEmpDisburse/getSBUWiseEmpDisburse/'+sbu+'/'+deptId+'/'+year+'/'+authId);
  }

  getAuthBudget(sbu: string, deptId: any, year: any, authId: any ){    
    return this.http.get(this.baseUrl + 'bgtEmpDisburse/getAuthBudget/'+sbu+'/'+deptId+'/'+year+'/'+authId);
  }

  insertBgtEmpDisburse(bgtEmpFormData: any) {
    return this.http.post(this.baseUrl+ 'bgtEmpDisburse/insertBgtEmpDetails', bgtEmpFormData);
  }

  insertBgtOwnList(sbuData:any) {
    return this.http.post(this.baseUrl+ 'bgtEmployee/insertBgtOwn', sbuData);
  }

}
