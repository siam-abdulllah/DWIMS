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
import { MedicineDispatch, MedicineDispatchDtl} from '../shared/models/medDispatch';
import { BgtEmpInsertDto } from '../shared/models/bgtEmployee';



@Injectable({
  providedIn: 'root'
})
export class BgtOwnService {
  bgtEmpFormData: BgtEmpInsertDto = new BgtEmpInsertDto();
  medDispatchFormData: MedicineDispatch = new MedicineDispatch();
  investmentMedicineProdFormData: InvestmentMedicineProd = new InvestmentMedicineProd();
  rptDepotLetter: IrptDepotLetterSearch[]=[];
  medDispDtl: MedicineDispatchDtl = new MedicineDispatchDtl();
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

  getApprovalAuthority(){    
    return this.http.get(this.baseUrl + 'approvalAuthority/approvalAuthoritiesForConfig');
  }
  getSBU(){    
    return this.http.get(this.baseUrl + 'employee/getSBU');
  }
  getDeptSbuWiseBudgetAmt(deptId: any, sbu: string, year: any ){    
    return this.http.get(this.baseUrl + 'BgtOwn/getDeptSBUBudget/'+deptId+'/'+sbu+'/'+year);
  }
  getAllEmp(){    
    return this.http.get(this.baseUrl + 'BgtOwn/getAllEmp');
  }
  getSbuWiseEmp(sbu: any ){    
    return this.http.get(this.baseUrl + 'BgtOwn/getSbuWiseEmp/'+sbu);
  }
  getEmpWiseData(employeeId: any ){    
    return this.http.get(this.baseUrl + 'BgtOwn/getEmpWiseData/'+employeeId);
  }
  getEmpWiseSBU(employeeId: any ){    
    return this.http.get(this.baseUrl + 'BgtOwn/getEmpWiseSBU/'+employeeId);
  }
  getEmpWiseBgt(employeeId: any, sbu: string , year: any,compId:any,deptId:any ){    
    return this.http.get(this.baseUrl + 'BgtOwn/getEmpWiseBgt/'+employeeId+'/'+sbu+'/'+year+'/'+compId+'/'+deptId);
  }
  getEmpWiseTotExp(employeeId: any, sbu: string , year: any,compId:any,deptId:any  ){    
    return this.http.get(this.baseUrl + 'BgtOwn/getEmpWiseTotExp/'+employeeId+'/'+sbu+'/'+year+'/'+compId+'/'+deptId);
  }
  getEmpWiseTotPipe(employeeId: any, sbu: string , year: any ,compId:any,deptId:any ){    
    return this.http.get(this.baseUrl + 'BgtOwn/getEmpWiseTotPipe/'+employeeId+'/'+sbu+'/'+year+'/'+compId+'/'+deptId);
  }

  getAuthPersonCount(authId: any ){    
    return this.http.get(this.baseUrl + 'BgtOwn/getAuthPersonCount/'+authId);
  }
  getDonations() {
    return this.http.get(this.baseUrl + 'donation/donationsForInvestment');
  }
  insertBgtEmp(bgtEmpFormData: any) {
    return this.http.post(this.baseUrl+ 'bgtEmployee/insertBgtEmployee', bgtEmpFormData);
  }
}