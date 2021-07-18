import { DonationPagination, IDonationPagination } from './../shared/models/donationPagination';
import { SubCampaignPagination, ISubCampaignPagination } from './../shared/models/subCampaignPagination';
import { CampaignPagination, ICampaignPagination } from './../shared/models/campaignPagination';
import { ApprovalAuthorityPagination, IApprovalAuthorityPagination } from './../shared/models/approvalAuthorityPagination';
import { IBcdsInfo, BcdsInfo } from './../shared/models/bcdsInfo';
import { BcdsPagination, IBcdsPagination } from './../shared/models/bcdsPagination';

import { ISocietyInfo, SocietyInfo } from '../shared/models/societyInfo';
import { SocietyPagination, ISocietyPagination } from './../shared/models/societyPagination';

import { IEmployeeInfo, EmployeeInfo } from '../shared/models/employeeInfo';
import { EmployeePagination, IEmployeePagination } from './../shared/models/employeePagination';
import { IRole, IRoleResponse } from './../shared/models/role';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { IDonation,Donation} from'../shared/models/donation';
import { ISubCampaign,SubCampaign} from'../shared/models/subCampaign';
import { ICampaign,Campaign} from'../shared/models/campaign';
import { IApprovalAuthority,ApprovalAuthority} from'../shared/models/approvalAuthority';
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
export class MasterService {
  //roles: IRole[] = [];
  donations: IDonation[]=[];
  donationPagination = new DonationPagination();
  approvalAuthoritys: IApprovalAuthority[]=[];
  approvalAuthorityPagination = new ApprovalAuthorityPagination();
  subCampaigns: ISubCampaign[]=[];
  subCampaignPagination = new SubCampaignPagination();
  campaigns: ICampaign[]=[];
  campaignPagination = new CampaignPagination();
  
  baseUrl = environment.apiUrl;
  genParams = new GenericParams();

  donationFormData: Donation = new Donation();
  subCampaignFormData: SubCampaign = new SubCampaign();
  campaignFormData: Campaign = new Campaign();
  approvalAuthorityFormData: ApprovalAuthority = new ApprovalAuthority();
  
  bcdsInfo: IBcdsInfo[]= [];
  bcdspagination = new BcdsPagination();
  bcdsFormData: BcdsInfo = new BcdsInfo();

  societyInfo: ISocietyInfo[]= [];
  societypagination = new SocietyPagination();
  societyFormData: SocietyInfo = new SocietyInfo();

  employeeInfo: IEmployeeInfo[]= [];
  employeePagination = new EmployeePagination();
  employeeFormData: EmployeeInfo = new EmployeeInfo();
  constructor(private http: HttpClient, private router: Router) { }

  getSBU(){    
    return this.http.get(this.baseUrl + 'employee/getSBU');
    
  }
  getCampaign(){    
    let params = new HttpParams();
    debugger;
    if (this.genParams.search) {
      params = params.append('search', this.genParams.search);
    }
    params = params.append('sort', this.genParams.sort);
    params = params.append('pageIndex', this.genParams.pageNumber.toString());
    params = params.append('pageSize', this.genParams.pageSize.toString());

    return this.http.get<ICampaignPagination>(this.baseUrl + 'campaign/campaigns', { observe: 'response', params })
    .pipe(
      map(response => {
        this.campaigns = [...this.campaigns, ...response.body.data]; 
        this.campaignPagination = response.body;
        return this.campaignPagination;
      })
    );
    
  }
  insertCampaign() {
    return this.http.post(this.baseUrl+ 'campaign/insert', this.campaignFormData);
    // return this.http.post(this.baseUrl + 'account/register', values).pipe(
    //   map((user: IUser) => {
    //     if (user) {
    //       // localStorage.setItem('token', user.token);
    //       // this.currentUserSource.next(user);
    //       return user;
    //     }
    //   })
    // );
  }
  updateCampaign() {
    return this.http.post(this.baseUrl+ 'campaign/update',  this.campaignFormData);
}
 
  getSubCampaign(){    
    let params = new HttpParams();
    debugger;
    if (this.genParams.search) {
      params = params.append('search', this.genParams.search);
    }
    params = params.append('sort', this.genParams.sort);
    params = params.append('pageIndex', this.genParams.pageNumber.toString());
    params = params.append('pageSize', this.genParams.pageSize.toString());

    return this.http.get<ISubCampaignPagination>(this.baseUrl + 'subCampaign/subCampaigns', { observe: 'response', params })
    .pipe(
      map(response => {
        this.subCampaigns = [...this.subCampaigns, ...response.body.data]; 
        this.subCampaignPagination = response.body;
        return this.subCampaignPagination;
      })
    );
    
  }
  insertSubCampaign() {
    return this.http.post(this.baseUrl+ 'subCampaign/insert', this.subCampaignFormData);
    // return this.http.post(this.baseUrl + 'account/register', values).pipe(
    //   map((user: IUser) => {
    //     if (user) {
    //       // localStorage.setItem('token', user.token);
    //       // this.currentUserSource.next(user);
    //       return user;
    //     }
    //   })
    // );
  }
  updateSubCampaign() {
    return this.http.post(this.baseUrl+ 'subCampaign/update',  this.subCampaignFormData);
}

getDonation(){    
  let params = new HttpParams();
  debugger;
  if (this.genParams.search) {
    params = params.append('search', this.genParams.search);
  }
  params = params.append('sort', this.genParams.sort);
  params = params.append('pageIndex', this.genParams.pageNumber.toString());
  params = params.append('pageSize', this.genParams.pageSize.toString());

  return this.http.get<IDonationPagination>(this.baseUrl + 'donation/donations', { observe: 'response', params })
  .pipe(
    map(response => {
      this.donations = [...this.donations, ...response.body.data]; 
      this.donationPagination = response.body;
      return this.donationPagination;
    })
  );
  
}
insertDonation() {
  return this.http.post(this.baseUrl+ 'donation/insert', this.donationFormData);

}
updateDonation() {
  return this.http.post(this.baseUrl+ 'donation/update',  this.donationFormData);
}


getApprovalAuthority(){    
  let params = new HttpParams();
  debugger;
  if (this.genParams.search) {
    params = params.append('search', this.genParams.search);
  }
  params = params.append('sort', this.genParams.sort);
  params = params.append('pageIndex', this.genParams.pageNumber.toString());
  params = params.append('pageSize', this.genParams.pageSize.toString());

  return this.http.get<IApprovalAuthorityPagination>(this.baseUrl + 'ApprovalAuthority/approvalAuthorities', { observe: 'response', params })
  .pipe(
    map(response => {
      this.approvalAuthoritys = [...this.approvalAuthoritys, ...response.body.data]; 
      this.approvalAuthorityPagination = response.body;
      return this.approvalAuthorityPagination;
    })
  );
  
}
insertApprovalAuthority() {
  return this.http.post(this.baseUrl+ 'approvalAuthority/insert', this.approvalAuthorityFormData);

}
updateApprovalAuthority() {
  return this.http.post(this.baseUrl+ 'approvalAuthority/update',  this.approvalAuthorityFormData);
}

//#region BCDS Info

getBcdsList(){    
  let params = new HttpParams();
  //debugger;
  if (this.genParams.search) {
    params = params.append('search', this.genParams.search);
  }
  params = params.append('sort', this.genParams.sort);
  params = params.append('pageIndex', this.genParams.pageNumber.toString());
  params = params.append('pageSize', this.genParams.pageSize.toString());

  return this.http.get<IBcdsPagination>(this.baseUrl + 'Bcds/GetAllBCDS', { observe: 'response', params })
  .pipe(
    map(response => {
      this.bcdsInfo = [...this.bcdsInfo, ...response.body.data]; 
      this.bcdspagination = response.body;
      return this.bcdspagination;
    })
  );
}

insertBcds() {
  return this.http.post(this.baseUrl+ 'Bcds/CreateBCDS', this.bcdsFormData);
}

updateBcds() {
  return this.http.post(this.baseUrl+ 'Bcds/ModifyBCDS',  this.bcdsFormData);
}

//#endregion
getSocietyList(){    
  let params = new HttpParams();
  //debugger;
  if (this.genParams.search) {
    params = params.append('search', this.genParams.search);
  }
  params = params.append('sort', this.genParams.sort);
  params = params.append('pageIndex', this.genParams.pageNumber.toString());
  params = params.append('pageSize', this.genParams.pageSize.toString());

  return this.http.get<ISocietyPagination>(this.baseUrl + 'Society/GetAllSociety', { observe: 'response', params })
  .pipe(
    map(response => {
      this.societyInfo = [...this.societyInfo, ...response.body.data]; 
      this.societypagination = response.body;
      return this.societypagination;
    })
  );
}

insertSociety() {
  return this.http.post(this.baseUrl+ 'Society/CreateSociety', this.societyFormData);
}

updateSociety() {
  return this.http.post(this.baseUrl+ 'Society/ModifySociety',  this.societyFormData);
}
//#endregion


getEmployeeList(){    
  let params = new HttpParams();
  //debugger;
  if (this.genParams.search) {
    params = params.append('search', this.genParams.search);
  }
  params = params.append('sort', this.genParams.sort);
  params = params.append('pageIndex', this.genParams.pageNumber.toString());
  params = params.append('pageSize', this.genParams.pageSize.toString());

  return this.http.get<IEmployeePagination>(this.baseUrl + 'Employee/GetAllEmployee', { observe: 'response', params })
  .pipe(
    map(response => {
      this.employeeInfo = [...this.employeeInfo, ...response.body.data]; 
      this.employeePagination = response.body;
      return this.employeePagination;
    })
  );
}

}

