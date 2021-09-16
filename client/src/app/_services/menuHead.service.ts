import { MenuHeadPagination, IMenuHeadPagination } from './../shared/models/menuHeadPagination';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { IMenuHead,MenuHead} from'../shared/models/menuHead';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router';
import { GenericParams } from '../shared/models/genericParams';

@Injectable({
  providedIn: 'root'
})
export class MenuHeadService {
  
  menuHead: IMenuHead[]=[];
  menuHeadPagination = new MenuHeadPagination();
  menuHeadFormData: MenuHead = new MenuHead();
  baseUrl = environment.apiUrl;
  genParams = new GenericParams();

  
  constructor(private http: HttpClient, private router: Router) { }

  getMenuHead(){   
    //return this.http.get(this.baseUrl + 'menuHead/menuHead/'+id);
    let params = new HttpParams();
    
    if (this.genParams.search) {
      params = params.append('search', this.genParams.search);
    }
    params = params.append('sort', this.genParams.sort);
    params = params.append('pageIndex', this.genParams.pageNumber.toString());
    params = params.append('pageSize', this.genParams.pageSize.toString());
    return this.http.get<IMenuHeadPagination>(this.baseUrl + 'menuHead/menuHeads', { observe: 'response', params })
    //return this.http.get<IDonationPagination>(this.baseUrl + 'donation/donations', { observe: 'response', params })
    .pipe(
      map(response => {
        this.menuHead = [...this.menuHead, ...response.body.data]; 
        this.menuHeadPagination = response.body;
        return this.menuHeadPagination;
      })
    ); 
    
  }
  


insertMenuHead() {
  return this.http.post(this.baseUrl+ 'menuHead/insert', this.menuHeadFormData);

}
updateMenuHead() {
  return this.http.post(this.baseUrl+ 'menuHead/update',  this.menuHeadFormData);
}

}

