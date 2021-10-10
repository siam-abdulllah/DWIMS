import { DonationPagination, IDonationPagination } from './../shared/models/donationPagination';
import { SubCampaignPagination, ISubCampaignPagination } from './../shared/models/subCampaignPagination';
import { CampaignMstPagination, ICampaignMstPagination,CampaignDtlPagination, ICampaignDtlPagination,CampaignDtlProductPagination, ICampaignDtlProductPagination } from './../shared/models/campaignPagination';
import { ApprovalAuthorityPagination, IApprovalAuthorityPagination } from './../shared/models/approvalAuthorityPagination';
import { IBcdsInfo, BcdsInfo } from './../shared/models/bcdsInfo';
import { BcdsPagination, IBcdsPagination } from './../shared/models/bcdsPagination';

import { ISocietyInfo, SocietyInfo } from '../shared/models/societyInfo';
import { SocietyPagination, ISocietyPagination } from './../shared/models/societyPagination';

import { IClusterMstInfo, ClusterMstInfo, IClusterDtlInfo, ClusterDtlInfo } from '../shared/models/clusterInfo';
import { IEmployeeInfo, EmployeeInfo } from '../shared/models/employeeInfo';
import { EmployeePagination, IEmployeePagination } from './../shared/models/employeePagination';
import { IRole, IRoleResponse } from './../shared/models/role';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { IDonation,Donation} from'../shared/models/donation';
import { ISubCampaign,SubCampaign} from'../shared/models/subCampaign';
import { CampaignMst, ICampaignMst,CampaignDtl, ICampaignDtl,CampaignDtlProduct, ICampaignDtlProduct } from '../shared/models/campaign';

import { IApprovalAuthority,ApprovalAuthority} from'../shared/models/approvalAuthority';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { BehaviorSubject, ReplaySubject, of, from } from 'rxjs';
import { IUser, IUserResponse } from '../shared/models/user';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router';
import { GenericParams } from '../shared/models/genericParams';
import { IPagination, Pagination } from '../shared/models/pagination';
import { ClusterDtlPagination, ClusterMstPagination, IClusterDtlPagination, IClusterMstPagination } from '../shared/models/clusterMstPagination';

@Injectable({
  providedIn: 'root'
})
export class MasterService {
  //roles: IRole[] = [];
  donations: IDonation[]=[];
  donationPagination = new DonationPagination();
  approvalAuthoritys: IApprovalAuthority[]=[];
  approvalAuthorityPagination = new ApprovalAuthorityPagination();
  clusterMsts: IClusterMstInfo[]=[];
  clusterMstPagination = new ClusterMstPagination();
  clusterDtls: IClusterDtlInfo[]=[];
  clusterDtlPagination = new ClusterDtlPagination();
  subCampaigns: ISubCampaign[]=[];
  subCampaignPagination = new SubCampaignPagination();
  campaignMsts: ICampaignMst[]=[];
  campaignMstPagination = new CampaignMstPagination();
  campaignDtls: ICampaignDtl[]=[];
  campaignDtlProducts: ICampaignDtlProduct[]=[];
  campaignDtlPagination = new CampaignDtlPagination();
  campaignDtlProductPagination = new CampaignDtlProductPagination();
  
  baseUrl = environment.apiUrl;
  genParams = new GenericParams();

  donationFormData: Donation = new Donation();
  subCampaignFormData: SubCampaign = new SubCampaign();
  campaignMstFormData: CampaignMst = new CampaignMst();
  campaignDtlFormData: CampaignDtl = new CampaignDtl();
  campaignDtlProductFormData: CampaignDtlProduct = new CampaignDtlProduct();
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
  clusterMstFormData: ClusterMstInfo = new ClusterMstInfo();
  clusterDtlFormData: ClusterDtlInfo = new ClusterDtlInfo();
  constructor(private http: HttpClient, private router: Router) { }

  getGenParams(){
    return this.genParams;
  }

   // tslint:disable-next-line: typedef
   setGenParams(genParams: GenericParams) {
    this.genParams = genParams;
  }


  getSBU(){    
    return this.http.get(this.baseUrl + 'employee/getSBU');
  }
  getRegion(){    
    return this.http.get(this.baseUrl + 'employee/getRegion');
  }
  getBrand(sbu:string){    
    return this.http.get(this.baseUrl + 'product/getBrand/'+sbu);
  }
  getProduct(brandCode:string){    
    return this.http.get(this.baseUrl + 'product/getProduct/'+brandCode);
  }
  getSubCampaignForCamp(){    
    return this.http.get(this.baseUrl + 'subCampaign/subCampaignsForCamp');
  }
  removeDtlProduct(selectedRecord:ICampaignDtlProduct){    
    return this.http.post(this.baseUrl + 'campaign/removeDtlProduct',selectedRecord);
  }
  
  getCampaignDtl(mstId:number){ 
    let params = new HttpParams();
    
    if (this.genParams.search) {
      params = params.append('search', this.genParams.search);
    }
    params = params.append('sort', this.genParams.sort);
    params = params.append('pageIndex', this.genParams.pageIndex.toString());
    params = params.append('pageSize', this.genParams.pageSize.toString());

    return this.http.get<ICampaignDtlPagination>(this.baseUrl + 'campaign/campaignDtls/'+mstId, { observe: 'response', params })
    .pipe(
      map(response => {
        this.campaignDtls = [...this.campaignDtls, ...response.body.data]; 
        this.campaignDtlPagination = response.body;
        return this.campaignDtlPagination;
      })
    );   
    
    
  }
  getCampaignDtlProduct(dtlId:number){ 
    let params = new HttpParams();
    
    if (this.genParams.search) {
      params = params.append('search', this.genParams.search);
    }
    params = params.append('sort', this.genParams.sort);
    params = params.append('pageIndex', this.genParams.pageIndex.toString());
    params = params.append('pageSize', this.genParams.pageSize.toString());

    return this.http.get<ICampaignDtlProductPagination>(this.baseUrl + 'campaign/campaignDtlProducts/'+dtlId, { observe: 'response', params })
    .pipe(
      map(response => {
        this.campaignDtlProducts = [...this.campaignDtlProducts, ...response.body.data]; 
        this.campaignDtlProductPagination = response.body;
        return this.campaignDtlProductPagination;
      })
    );   
    
    
  }
  getCampaign(){    
    let params = new HttpParams();
    
    if (this.genParams.search) {
      params = params.append('search', this.genParams.search);
    }
    params = params.append('sort', this.genParams.sort);
    params = params.append('pageIndex', this.genParams.pageIndex.toString());
    params = params.append('pageSize', this.genParams.pageSize.toString());

    return this.http.get<ICampaignMstPagination>(this.baseUrl + 'campaign/campaignMsts', { observe: 'response', params })
    .pipe(
      map(response => {
        this.campaignMsts = [...this.campaignMsts, ...response.body.data]; 
        this.campaignMstPagination = response.body;
        return this.campaignMstPagination;
      })
    );
    
  }
  insertCampaignMst() {
    return this.http.post(this.baseUrl+ 'campaign/insertMst', this.campaignMstFormData);
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
  insertCampaignDtl() {
    return this.http.post(this.baseUrl+ 'campaign/insertDtl', this.campaignDtlFormData);
  }
  insertCampaignDtlProduct() {
    return this.http.post(this.baseUrl+ 'campaign/insertDtlProduct', this.campaignDtlProductFormData);
  }
  updateCampaignMst() {
    return this.http.post(this.baseUrl+ 'campaign/updateMst',  this.campaignMstFormData);
  }
  updateCampaignDtl() {
  return this.http.post(this.baseUrl+ 'campaign/updateDtl',  this.campaignDtlFormData);
  }
  updateCampaignDtlProduct() {
  return this.http.post(this.baseUrl+ 'campaign/updateDtlProduct',  this.campaignDtlProductFormData);
  }
 
  getSubCampaign(){    
    let params = new HttpParams();
    
    if (this.genParams.search) {
      params = params.append('search', this.genParams.search);
    }
    params = params.append('sort', this.genParams.sort);
    params = params.append('pageIndex', this.genParams.pageIndex.toString());
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
  }
  updateSubCampaign() {
    return this.http.post(this.baseUrl+ 'subCampaign/update',  this.subCampaignFormData);
}

getDonation(){    
  let params = new HttpParams();
  if (this.genParams.search) {
    params = params.append('search', this.genParams.search);
  }
  params = params.append('sort', this.genParams.sort);
  params = params.append('pageIndex', this.genParams.pageIndex.toString());
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
  
  if (this.genParams.search) {
    params = params.append('search', this.genParams.search);
  }
  params = params.append('sort', this.genParams.sort);
  params = params.append('pageIndex', this.genParams.pageIndex.toString());
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

getBcdsList(){    
  let params = new HttpParams();
  if (this.genParams.search) {
    params = params.append('search', this.genParams.search);
  }
  
  params = params.append('sort', this.genParams.sort);
  params = params.append('pageIndex', this.genParams.pageIndex.toString());
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

getSocietyList(){    
  let params = new HttpParams();
  if (this.genParams.search) {
    params = params.append('search', this.genParams.search);
  }
  params = params.append('sort', this.genParams.sort);
  params = params.append('pageIndex', this.genParams.pageIndex.toString());
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

getEmployeeList(){    
  let params = new HttpParams();
  if (this.genParams.search) {
    params = params.append('search', this.genParams.search);
  }
  params = params.append('sort', this.genParams.sort);
  params = params.append('pageIndex', this.genParams.pageIndex.toString());
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
getClusterMstList(){    
  let params = new HttpParams();
  
  if (this.genParams.search) {
    params = params.append('search', this.genParams.search);
  }
  params = params.append('sort', this.genParams.sort);
  params = params.append('pageIndex', this.genParams.pageIndex.toString());
  params = params.append('pageSize', this.genParams.pageSize.toString());

  return this.http.get<IClusterMstPagination>(this.baseUrl + 'cluster/clusterMsts', { observe: 'response', params })
  .pipe(
    map(response => {
      this.clusterMsts = [...this.clusterMsts, ...response.body.data]; 
      this.clusterMstPagination = response.body;
      return this.clusterMstPagination;
    })
  );
  
}
getClusterDtlList(mstId:number){    
  let params = new HttpParams();
  
  if (this.genParams.search) {
    params = params.append('search', this.genParams.search);
  }
  params = params.append('sort', this.genParams.sort);
  params = params.append('pageIndex', this.genParams.pageIndex.toString());
  params = params.append('pageSize', this.genParams.pageSize.toString());

  return this.http.get<IClusterDtlPagination>(this.baseUrl + 'cluster/clusterDtls/'+mstId, { observe: 'response', params })
  .pipe(
    map(response => {
      this.clusterDtls = [...this.clusterDtls, ...response.body.data]; 
      this.clusterDtlPagination = response.body;
      return this.clusterDtlPagination;
    })
  );
  
}
insertClusterMst() {
  return this.http.post(this.baseUrl+ 'cluster/insertMst', this.clusterMstFormData);
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
insertClusterDtl() {
  return this.http.post(this.baseUrl+ 'cluster/insertDtl', this.clusterDtlFormData);
}
updateClusterMst() {
  return this.http.post(this.baseUrl+ 'cluster/updateMst',  this.clusterMstFormData);
}
updateClusterDtl() {
return this.http.post(this.baseUrl+ 'cluster/updateDtl',  this.clusterDtlFormData);
}
}

