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



@Injectable({
  providedIn: 'root'
})
export class MedDispatchService {

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

  

  insertDispatch(medDispatchFormData: any) {
    debugger;
    return this.http.post(this.baseUrl+ 'medDispatch/createDispatch', medDispatchFormData);
  }

  insertMedicineDetail(medDispDtl:MedicineDispatchDtl[]) {
    debugger;
    return this.http.post(this.baseUrl+ 'medDispatch/insertMedicineDetail', medDispDtl);
  }

  getRptDepotLetter(initId:any) {
    return this.http.get(this.baseUrl+ 'reportInvestment/rptInvestDepo/'+initId);
  }

  getPendingDispatch(empId:number,userRole:string){    
    return this.http.get(this.baseUrl + 'medDispatch/pendingDispatch/'+ empId+'/'+userRole);
  }

  getRptMedDis(model: any){    
    return this.http.post(this.baseUrl + 'medDispatch/medDispReport/', model);
  }

  // removeInvestmentMedicineProd() {
  //   return this.http.post(this.baseUrl + 'medDispatch/removeInvestmentMedicineProd', this.investmentMedicineProdFormData,
  //     { responseType: 'text' });
  // }

  getDonations() {
    return this.http.get(this.baseUrl + 'donation/donationsForInvestment');
  }
  getDepot() {
    return this.http.get(this.baseUrl + 'employee/depotForInvestment');
  }
  getInvestmentMedicineProds(investmentInitId: number) {
    return this.http.get(this.baseUrl + 'medDispatch/getMedicineProds/' + investmentInitId );
  }

  getEmpDepot(empId: number) {
    return this.http.get(this.baseUrl + 'employee/getEmpDepot/' + empId );
  }
}