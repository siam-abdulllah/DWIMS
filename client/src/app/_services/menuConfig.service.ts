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
import { MenuConfig } from '../shared/models/menuConfig';

@Injectable({
  providedIn: 'root'
})
export class MenuConfigService {
  removeMenuConfig(id: number) {
    return this.http.get(this.baseUrl + 'menuConfig/removeMenuConfig/'+id,{ responseType: 'text' });
    
  }

  menuHead: IMenuHead[] = [];
  //menuHeadPagination = new MenuHeadPagination();
  //menuHeadFormData: MenuHead = new MenuHead();
  subMenu: ISubMenu[] = [];
  //subMenuPagination = new SubMenuPagination();
  menuConfigFormData: MenuConfig = new MenuConfig();
  baseUrl = environment.apiUrl;
  genParams = new GenericParams();


  constructor(private http: HttpClient, private router: Router) { }

  getMenuHead() {
    return this.http.get(this.baseUrl + 'menuHead/menuHeadsForMenuConfig');
  }
  getSubMenu() {
    return this.http.get(this.baseUrl + 'subMenu/subMenusForMenuConfig/'+this.menuConfigFormData.menuHeadId);
    
    
  }
  getMenuConfig() {
    return this.http.get(this.baseUrl + 'menuConfig/menuConfigs/'+this.menuConfigFormData.menuHeadId+'/'+this.menuConfigFormData.roleId);
    
    
  }
  insertMenuConfig() {
    return this.http.post(this.baseUrl + 'menuConfig/insert', this.menuConfigFormData);
  }
  updateMenuConfig() {
    return this.http.post(this.baseUrl + 'menuConfig/update', this.menuConfigFormData);
  }
}

