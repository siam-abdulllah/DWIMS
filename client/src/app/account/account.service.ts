import { UserPagination, IUserPagination } from './../shared/models/usersPagination';
import { IRole, IRoleResponse } from './../shared/models/role';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { BehaviorSubject, ReplaySubject, of } from 'rxjs';
import { IUser, IUserResponse } from '../shared/models/user';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router';
import { IAddress } from '../shared/models/address';
import { GenericParams } from '../shared/models/genericParams';
import { IPagination, Pagination } from '../shared/models/pagination';
import { IEmployeeInfo } from '../shared/models/employeeInfo';
import { JwtHelperService } from '@auth0/angular-jwt';
import { IMenuConfig, MenuConfig } from '../shared/models/menuConfig';
@Injectable({
  providedIn: 'root'
})
export class AccountService {
  jwtHelper = new JwtHelperService();
  roles: IRole[] = [];
  users: IUserResponse[] = [];
  pagination = new UserPagination();
  baseUrl = environment.apiUrl;
  genParams = new GenericParams();

  private currentUserSource = new ReplaySubject<IUser>(1);

  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private router: Router) { }

  // tslint:disable-next-line: typedef
  loadCurrentUser(token: string) {
    if (token === null) {
      this.currentUserSource.next(null);
      return of(null);
    }

    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);

    return this.http.get(this.baseUrl + 'account', { headers }).pipe(
      map((user: IUser) => {
        if (user) {
          localStorage.setItem('token', user.token);
          this.currentUserSource.next(user);

        }
      })
    );
  }

  // tslint:disable-next-line: typedef
  login(values: any) {
    debugger;
    return this.http.post(this.baseUrl + 'account/login', values).pipe(
      map((user: IUser) => {
        if (user) {
          localStorage.setItem('token', user.token);
          localStorage.setItem('empID', String(user.employeeId));
          localStorage.setItem('displayName', String(user.displayName));
          localStorage.setItem('menu', JSON.stringify(user.menuList));
          this.currentUserSource.next(user);
          //this.setEmployeeInfo(user.employeeId);
          //const empID = localStorage.getItem('empID');
          //const token = localStorage.getItem('token');
          //const r =  this.jwtHelper.decodeToken(token);
          //alert(r);
        }
      })
    );
  }
  loggedIn() {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }
  getUserRole() {
    const token = localStorage.getItem('token');
    const r = this.jwtHelper.decodeToken(token);
    return r.role;
  }
  isMenuPermitted(url: string) {
    debugger;
    const menu = JSON.parse(localStorage.getItem("menu"));
    for (let i = 0; i < menu.length; i++) {

      if (menu[i].url == url) {
        return true;
      }
    }
    return false;
  }
  // eventPerm() {
  //   debugger;
  //   const token = localStorage.getItem('token');
  //   const r =  this.jwtHelper.decodeToken(token);
  //   var menuConf=new MenuConfig();
  //   menuConf.roleName=r.role;
  //     return this.http.get(this.baseUrl + 'menuConfig/menuConfigsForSecurity/'+r.role).pipe(
  //       map((menuConfig: IMenuConfig) => {
  //         if (menuConfig) {
  //           localStorage.setItem('menu', JSON.stringify(menuConfig));
  //           debugger;
  //           // localStorage.setItem('token', user.token);
  //           // this.currentUserSource.next(user);
  //           return true;
  //         }
  //       })
  //     );


  // }
  getEmployeeId() {
    const employeeId = localStorage.getItem('empID');
    return employeeId;

  }
  getEmployeeSbu(employeeId: number) {
    return this.http.get(this.baseUrl + 'employee/getEmployeeSbuById/' + employeeId).pipe(
      map((employeeInfo: IEmployeeInfo) => {
        if (employeeInfo) {
          // localStorage.setItem('token', user.token);
          // this.currentUserSource.next(user);
          return employeeInfo;
        }
      })
    );

  }
  setEmployeeInfo(employeeId: number) {
    return this.http.get(this.baseUrl + 'employee/getEmployeeSbuById/' + employeeId).pipe(
      map((employeeInfo: IEmployeeInfo) => {
        if (employeeInfo) {
          //localStorage.setItem('employeeName', employeeInfo.employeeName);
          //localStorage.setItem('designation', employeeInfo.employeeName);

          return employeeInfo;
        }
      })
    );

  }

  // tslint:disable-next-line: typedef
  register(values: any) {
    return this.http.post(this.baseUrl + 'account/register', values).pipe(
      map((user: IUser) => {
        if (user) {
          // localStorage.setItem('token', user.token);
          // this.currentUserSource.next(user);
          return user;
        }
      })
    );
  }
  employeeValidateById(employeeSAPCode: string) {
    return this.http.get(this.baseUrl + 'employee/employeeValidateById/' + employeeSAPCode).pipe(
      map((employeeInfo: IEmployeeInfo) => {
        if (employeeInfo) {
          // localStorage.setItem('token', user.token);
          // this.currentUserSource.next(user);
          return employeeInfo;
        }
      })
    );
  }

  updateRegisterUser(values: any) {
    return this.http.post(this.baseUrl + 'account/updateRegisterUser', values).pipe(
      map((user: IUser) => {
        if (user) {
          // localStorage.setItem('token', user.token);
          // this.currentUserSource.next(user);
          return user;
        }
      })
    );
  }

  // tslint:disable-next-line: typedef
  logout() {
    localStorage.removeItem('token');
    this.currentUserSource.next(null);
    this.router.navigateByUrl('/');
  }

  // tslint:disable-next-line: typedef
  checkEmailExists(email: string) {
    return this.http.get(this.baseUrl + 'account/emailexists?email=' + email);
  }

  // tslint:disable-next-line: typedef
  getUserAddress() {
    return this.http.get<IAddress>(this.baseUrl + 'account/address');
  }

  // tslint:disable-next-line: typedef
  updateUserAddress(address: IAddress) {
    return this.http.put<IAddress>(this.baseUrl + 'account/address', address);
  }

  getRoles() {
    return this.http.get<IRoleResponse>(this.baseUrl + 'role/getRoles', { observe: 'response' })
      .pipe(
        map(response => {
          this.roles = [...this.roles, ...response.body.data];
          return this.roles;
        })
      );
  }

  getUsers() {
    let params = new HttpParams();
    if (this.genParams.search) {
      params = params.append('search', this.genParams.search);
    }
    params = params.append('sort', this.genParams.sort);
    params = params.append('pageIndex', this.genParams.pageNumber.toString());
    params = params.append('pageSize', this.genParams.pageSize.toString());

    return this.http.get<IUserPagination>(this.baseUrl + 'account/users', { observe: 'response', params })
      .pipe(
        map(response => {
          this.users = [...this.users, ...response.body.data];
          this.pagination = response.body;
          return this.pagination;
        })
      );

  }

  getGenParams() {
    return this.genParams;
  }
  // tslint:disable-next-line: typedef
  setGenParams(genParams: GenericParams) {
    this.genParams = genParams;
  }

  getUserById(id: any) {
    let params = new HttpParams();
    params = params.append('id', id);
    return this.http.get<IUserResponse>(this.baseUrl + 'account/getUserById', { observe: 'response', params })
      .pipe(
        map(response => {
          return response.body;
        })
      );

  }

}

