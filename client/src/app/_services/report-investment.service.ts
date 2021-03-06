import { IReportConfig, IReportConfigPagination, ReportConfigPagination } from './../report-investment/report-investment.component';
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
  reportConfig: IReportConfig[]=[];

  roles: IRole[] = [];
  baseUrl = environment.apiUrl;
  genParams = new GenericParams();
  reportpagination = new ReportConfigPagination();

  constructor(private http: HttpClient, private router: Router) { }


  getReportList(){    
    let params = new HttpParams();
    if (this.genParams.search) {
      params = params.append('search', this.genParams.search);
    }
    params = params.append('sort', this.genParams.sort);
    params = params.append('pageIndex', this.genParams.pageIndex.toString());
    params = params.append('pageSize', this.genParams.pageSize.toString());
  
    return this.http.get<IReportConfigPagination>(this.baseUrl + 'reportInvestment/getReportList', { observe: 'response', params })
    .pipe(
      map(response => {
        this.reportConfig = [...this.reportConfig, ...response.body.data]; 
        this.reportpagination = response.body;
        return this.reportpagination;
      })
    );
  }

  
  getGenParams(){
    return this.genParams;
  }

   // tslint:disable-next-line: typedef
   setGenParams(genParams: GenericParams) {
    this.genParams = genParams;
  }


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
  getDoctors(){    
    return this.http.get(this.baseUrl + 'doctor/doctorsForInvestment');
  }
  getBrand(){    
    return this.http.get(this.baseUrl + 'product/getAllBrand/');
  }
  getCampaignMsts(){    
    return this.http.get(this.baseUrl + 'campaign/getCampaignForReport');
  }
  getSubCampaign(){    
    return this.http.get(this.baseUrl + 'subCampaign/subCampaignsForCamp');
  }
  getInsSocietyBCDSWiseInvestment(model: any) {
     return this.http.post(this.baseUrl + 'reportInvestment/GetInsSocietyBCDSWiseInvestment',model);
  }

  GetDoctorLocationWiseInvestment(model: any) {
    return this.http.post(this.baseUrl + 'reportInvestment/GetDoctorLocationWiseInvestment', model);
 }

 GetDoctorCampaingWiseInvestment(model: any) {
  return this.http.post(this.baseUrl + 'reportInvestment/GetDoctorCampaingWiseInvestment', model);
}

GetSBUWiseExpSummaryReport(model: any) {
  return this.http.post(this.baseUrl + 'reportInvestment/GetSBUWiseExpSummaryReport', model);
}

GetYearlyBudgetReport(model: any) {
  return this.http.post(this.baseUrl + 'reportInvestment/GetYearlyBudgetReport', model);
}

GetSBUWiseExpenseReport(model: any) {
  return this.http.post(this.baseUrl + 'reportInvestment/GetSBUWiseExpenseReport', model);
}

GetEmpWiseExpSummaryReport(model: any) {
  return this.http.post(this.baseUrl + 'reportInvestment/GetEmpWiseExpSummaryReport', model);
}

getSystemSummary(){    
  return this.http.get(this.baseUrl + 'reportInvestment/getSystemSummaryData');
}

}

