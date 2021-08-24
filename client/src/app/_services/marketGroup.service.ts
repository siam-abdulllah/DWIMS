import { MarketGroupPaginationMst, IMarketGroupPaginationMst } from './../shared/models/marketGroupPaginationMst';
import { MarketGroupPaginationDtl, IMarketGroupPaginationDtl } from './../shared/models/marketGroupPaginationDtl';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { IMarketGroupMst,MarketGroupMst} from'../shared/models/marketGroupMst';
import { IMarketGroupDtl,MarketGroupDtl} from'../shared/models/marketGroupDtl';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { BehaviorSubject, ReplaySubject, of, from } from 'rxjs';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router';
import { GenericParams } from '../shared/models/genericParams';

@Injectable({
  providedIn: 'root'
})
export class MarketGroupService {
  
  marketGroupMsts: IMarketGroupMst[]=[];
  marketGroupPaginationMst = new MarketGroupPaginationMst();
  marketGroupDtls: IMarketGroupDtl[]=[];
  marketGroupPaginationDtl = new MarketGroupPaginationDtl();
  marketGroupFormData: MarketGroupMst = new MarketGroupMst();
  
  
  
  baseUrl = environment.apiUrl;
  genParams = new GenericParams();

  
  constructor(private http: HttpClient, private router: Router) { }

  getGroups(){    
    let params = new HttpParams();
    
    if (this.genParams.search) {
      params = params.append('search', this.genParams.search);
    }
    params = params.append('sort', this.genParams.sort);
    params = params.append('pageIndex', this.genParams.pageNumber.toString());
    params = params.append('pageSize', this.genParams.pageSize.toString());
    return this.http.get<IMarketGroupPaginationMst>(this.baseUrl + 'marketGroup/marketGroupMsts', { observe: 'response', params })
    //return this.http.get<IDonationPagination>(this.baseUrl + 'donation/donations', { observe: 'response', params })
    .pipe(
      map(response => {
        this.marketGroupMsts = [...this.marketGroupMsts, ...response.body.data]; 
        this.marketGroupPaginationMst = response.body;
        return this.marketGroupPaginationMst;
      })
    );
    
  }
    
    
  getMarketGroups(id:number){    
    //return this.http.get(this.baseUrl + 'marketGroup/marketGroupDtls/'+id);
    let params = new HttpParams();
    
    if (this.genParams.search) {
      params = params.append('search', this.genParams.search);
    }
    params = params.append('sort', this.genParams.sort);
    params = params.append('pageIndex', this.genParams.pageNumber.toString());
    params = params.append('pageSize', this.genParams.pageSize.toString());
    return this.http.get<IMarketGroupPaginationDtl>(this.baseUrl + 'marketGroup/marketGroupDtls/'+id, { observe: 'response', params })
    //return this.http.get<IDonationPagination>(this.baseUrl + 'donation/donations', { observe: 'response', params })
    .pipe(
      map(response => {
        this.marketGroupDtls = [...this.marketGroupDtls, ...response.body.data]; 
        this.marketGroupPaginationDtl = response.body;
        return this.marketGroupPaginationDtl;
      })
    );
  }
  
  getMarkets(){    
  return this.http.get(this.baseUrl + 'employee/marketForGroup');
 
}

insertMarketGroup() {
  
  return this.http.post(this.baseUrl+ 'marketGroup/insertMst', this.marketGroupFormData);

}
insertMarketGroupDtl(id:number,marketCode:string,marketName:string,sbu:string) {
  
  var Indata = {'mstId': id, 'marketCode': marketCode, 'marketName':marketName, 'sbu':sbu};
  return this.http.post(this.baseUrl+ 'marketGroup/insertDtl', Indata);

}
 updateMarketGroup() {
   return this.http.post(this.baseUrl+ 'marketGroup/updateMst',  this.marketGroupFormData);
 }
 removeMarketGroups(selectedRecord: IMarketGroupDtl) {
   
   return this.http.post(this.baseUrl+ 'marketGroup/updateDtl', selectedRecord);
 }

}

