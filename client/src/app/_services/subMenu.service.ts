import { MenuHeadPagination, IMenuHeadPagination } from './../shared/models/menuHeadPagination';
import { SubMenuPagination, ISubMenuPagination } from './../shared/models/subMenuPagination';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { IMenuHead, MenuHead } from '../shared/models/menuHead';
import { ISubMenu, SubMenu } from '../shared/models/subMenu';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router';
import { GenericParams } from '../shared/models/genericParams';

@Injectable({
  providedIn: 'root'
})
export class SubMenuService {

  menuHead: IMenuHead[] = [];
  //menuHeadPagination = new MenuHeadPagination();
  //menuHeadFormData: MenuHead = new MenuHead();
  subMenu: ISubMenu[] = [];
  subMenuPagination = new SubMenuPagination();
  subMenuFormData: SubMenu = new SubMenu();
  baseUrl = environment.apiUrl;
  genParams = new GenericParams();


  constructor(private http: HttpClient, private router: Router) { }

  getMenuHead() {
    return this.http.get(this.baseUrl + 'menuHead/menuHeadsForSubMenu');
  }
  getSubMenu() {
    let params = new HttpParams();
    if (this.genParams.search) {
      params = params.append('search', this.genParams.search);
    }
    params = params.append('sort', this.genParams.sort);
    params = params.append('pageIndex', this.genParams.pageNumber.toString());
    params = params.append('pageSize', this.genParams.pageSize.toString());
    return this.http.get<ISubMenuPagination>(this.baseUrl + 'subMenu/subMenus/'+this.subMenuFormData.menuHeadId, { observe: 'response', params })
      //return this.http.get<IDonationPagination>(this.baseUrl + 'donation/donations', { observe: 'response', params })
      .pipe(
        map(response => {
          this.subMenu = [...this.subMenu, ...response.body.data];
          this.subMenuPagination = response.body;
          return this.subMenuPagination;
        })
      );
  }
  insertSubMenu() {
    return this.http.post(this.baseUrl + 'subMenu/insert', this.subMenuFormData);
  }
  updateSubMenu() {
    return this.http.post(this.baseUrl + 'subMenu/update', this.subMenuFormData);
  }
}

