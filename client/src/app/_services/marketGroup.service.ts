import { MarketGroupPaginationMst, IMarketGroupPaginationMst } from './../shared/models/marketGroupPaginationMst';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { IMarketGroupMst,MarketGroupMst} from'../shared/models/marketGroupMst';
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
  marketGroupFormData: MarketGroupMst = new MarketGroupMst();
  
  
  
  baseUrl = environment.apiUrl;
  genParams = new GenericParams();

  
  constructor(private http: HttpClient, private router: Router) { }

  getGroups(){    
    return this.http.get(this.baseUrl + 'marketGroup/getGroups');
    
  }
  getMarketGroups(id:number){    
    return this.http.get(this.baseUrl + 'marketGroup/getMarketGroups/'+id);
    
  }
  
  getMarkets(){    
  return this.http.get(this.baseUrl + 'market/getMArkets');
 
}

insertMarketGroup() {
  debugger;
  return this.http.post(this.baseUrl+ 'marketGroup/insert', this.marketGroupFormData);

}
 updateMarketGroup() {
   return this.http.post(this.baseUrl+ 'marketGroup/update',  this.marketGroupFormData);
 }

}

