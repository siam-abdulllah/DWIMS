import { InvestmentInitPagination, IInvestmentInitPagination } from '../shared/models/investmentPagination';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { InvestmentInit, IInvestmentInit,InvestmentDetail,IInvestmentDetail,InvestmentTargetedProd,IInvestmentTargetedProd } from '../shared/models/investment';
import { InvestmentDoctor, IInvestmentDoctor,InvestmentInstitution,IInvestmentInstitution,InvestmentCampaign,IInvestmentCampaign } from '../shared/models/investment';
import { InvestmentBcds, IInvestmentBcds,InvestmentSociety,IInvestmentSociety } from '../shared/models/investment';

import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { BehaviorSubject, ReplaySubject, of, from } from 'rxjs';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router';
import { GenericParams } from '../shared/models/genericParams';

@Injectable({
  providedIn: 'root'
})
export class InvestmentInitService {
  investmentInits: IInvestmentInit[]=[];
  investmentInitPagination = new InvestmentInitPagination();
  investmentInitFormData: InvestmentInit = new InvestmentInit();
  investmentDetailFormData: InvestmentDetail = new InvestmentDetail();
  investmentTargetedProdFormData: InvestmentTargetedProd = new InvestmentTargetedProd();
  investmentDoctorFormData: InvestmentDoctor = new InvestmentDoctor();
  investmentInstitutionFormData: InvestmentInstitution = new InvestmentInstitution();
  investmentCampaignFormData: InvestmentCampaign = new InvestmentCampaign();
  investmentBcdsFormData: InvestmentBcds = new InvestmentBcds();
  investmentSocietyFormData: InvestmentSociety = new InvestmentSociety();
  baseUrl = environment.apiUrl;
  genParams = new GenericParams();

  
  constructor(private http: HttpClient, private router: Router) { }

  getDonations(){    
    return this.http.get(this.baseUrl + 'donation/donationsForInvestment');
  }
  getMarkets(){    
    return this.http.get(this.baseUrl + 'employee/marketForInvestment');
  }
  getProduct(){    
    return this.http.get(this.baseUrl + 'product/getProductForInvestment');
  }
  getApprovalAuthority(){    
    return this.http.get(this.baseUrl + 'approvalAuthority/approvalAuthoritiesForConfig');
  }
  getEmployees(){    
    return this.http.get(this.baseUrl + 'employee/employeesForInvestment');
  }
  getInstitutions(){    
    return this.http.get(this.baseUrl + 'institution/institutionsForInvestment');
  }
  getDoctors(){    
    return this.http.get(this.baseUrl + 'doctor/doctorsForInvestment');
  }
  getInvestmentDoctors(investmentInitId:number){    
    return this.http.get(this.baseUrl + 'investment/investmentDoctors/'+investmentInitId);
  }
  getInvestmentInstitutions(investmentInitId:number){    
    return this.http.get(this.baseUrl + 'investment/investmentInstitutions/'+investmentInitId);
  }
  getInvestmentInit(){    
    let params = new HttpParams();
    if (this.genParams.search) {
      params = params.append('search', this.genParams.search);
    }
    params = params.append('sort', this.genParams.sort);
    params = params.append('pageIndex', this.genParams.pageNumber.toString());
    params = params.append('pageSize', this.genParams.pageSize.toString());
    return this.http.get<IInvestmentInitPagination>(this.baseUrl + 'investment/investmentInits', { observe: 'response', params })
    //return this.http.get<IDonationPagination>(this.baseUrl + 'donation/donations', { observe: 'response', params })
    .pipe(
      map(response => {
        this.investmentInits = [...this.investmentInits, ...response.body.data]; 
        this.investmentInitPagination = response.body;
        return this.investmentInitPagination;
      })
    );
    
  }
  insertInvestmentInit() {
    debugger;
    return this.http.post(this.baseUrl+ 'investment/insertInit', this.investmentInitFormData);

  }
  
  updateInvestmentInit() {
    return this.http.post(this.baseUrl+ 'investment/updateInit',  this.investmentInitFormData);
  }
  insertInvestmentDoctor() {
    debugger;
    return this.http.post(this.baseUrl+ 'investment/insertInvestmentDoctor', this.investmentDoctorFormData);

  }
  insertInvestmentInstitution() {
    debugger;
    return this.http.post(this.baseUrl+ 'investment/insertInvestmentInstitution', this.investmentInstitutionFormData);

  }
}

