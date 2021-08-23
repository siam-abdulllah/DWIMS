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

  getMarket(){    
    return this.http.get(this.baseUrl + 'employee/getMarket');
  }

  getTerritory(){    
    return this.http.get(this.baseUrl + 'employee/getTerritory');
  }

  getZone(){    
    return this.http.get(this.baseUrl + 'employee/getZone');
  }

  getDivision(){    
    return this.http.get(this.baseUrl + 'employee/getDivision');
  }

  getRegion(){    
    return this.http.get(this.baseUrl + 'employee/getRegion');
  }
  
  getSBU(){    
    return this.http.get(this.baseUrl + 'employee/getSBU');
  }
  getInstitutions(){    
    return this.http.get(this.baseUrl + 'institution/institutionsForInvestment');
  }
  getBcds(){    
    return this.http.get(this.baseUrl + 'bcds/bcdsForInvestment');
  }
  getSociety(){    
    return this.http.get(this.baseUrl + 'society/societyForInvestment');
  }
  getDonations(){    
    return this.http.get(this.baseUrl + 'donation/donationsForInvestment');
  }


  
  getInsSocietyBCDSWiseInvestment(model: any) {
    //getInsSocietyBCDSWiseInvestment() {
     return this.http.post(this.baseUrl + 'reportInvestment/GetInsSocietyBCDSWiseInvestment',model);
  }


  getDoctorWiseLeadership(model: any) {
    return this.http.post(this.baseUrl + 'reportInvestment/GetDoctorWiseLeadership', model);
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

