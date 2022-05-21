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
export class BgtEmployeeService {

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

  getDeptSbuWiseBudgetAmt(deptId: any, sbu: string, year: any ){    
    return this.http.get(this.baseUrl + 'bgtEmployee/getDeptSBUBudget/'+deptId+'/'+sbu+'/'+year);
  }

  getSBUWiseDonationLocation(donationId: any, deptId: any, year: any, authId: any ){    
    return this.http.get(this.baseUrl + 'bgtEmployee/getSBUWiseDonationLocation/'+donationId+'/'+deptId+'/'+year+'/'+authId);
  }

  insertBgtEmp(bgtEmpFormData: any) {
    return this.http.post(this.baseUrl+ 'bgtEmployee/insertBgtEmployee', bgtEmpFormData);
  }

  insertBgtOwn(bgtOwnFormData: any) {
    return this.http.post(this.baseUrl+ 'bgtEmployee/insertBgtOwn', bgtOwnFormData);
  }


  insertBgtOwnList(sbuData:any) {
    return this.http.post(this.baseUrl+ 'bgtEmployee/insertBgtOwn', sbuData);
  }

}
