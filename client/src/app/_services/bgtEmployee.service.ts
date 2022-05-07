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
import { InvestmentMedicineProd } from '../shared/models/investmentRec';
import { BgtEmpInsertDto} from '../shared/models/bgtEmployee';



@Injectable({
  providedIn: 'root'
})
export class BgtEmployeeService {

  bgtEmpFormData: BgtEmpInsertDto = new BgtEmpInsertDto();
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

  getDonations() {
    return this.http.get(this.baseUrl + 'donation/donationsForInvestment');
  }

  getApprovalAuthority(){    
    return this.http.get(this.baseUrl + 'bgtEmployee/approvalAuthoritiesForConfig');
  }
  
  getSBU(){    
    return this.http.get(this.baseUrl + 'employee/getSBU');
  }

  getDeptSbuWiseBudgetAmt(deptId: any, sbu: string, year: any ){    
    return this.http.get(this.baseUrl + 'bgtEmployee/getDeptSBUBudget/'+deptId+'/'+sbu+'/'+year);
  }

  getPrevAllocate(deptId: any, sbu: string, year: any ){    
    return this.http.get(this.baseUrl + 'bgtEmployee/getPrevAlloc/'+deptId+'/'+sbu+'/'+year);
  }

 

  getAuthPersonCount(authId: any, sbu: string ){    
    return this.http.get(this.baseUrl + 'bgtEmployee/getAuthPersonCount/'+authId+'/'+sbu);
  }

  insertBgtEmp(bgtEmpFormData: any) {
    return this.http.post(this.baseUrl+ 'bgtEmployee/insertBgtEmployee', bgtEmpFormData);
  }

}