import {Injectable} from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
// import { environment } from 'src/environments/environment.prod';
import { environment } from 'src/environments/environment';

@Injectable({
    providedIn: 'root'
})
export class ChangePasswordAllService {
    baseUrl = environment.apiUrl + 'account/';
    constructor( private http: HttpClient) {}
   
    verifyCurrentPassword(model: any) {
        return this.http.post(this.baseUrl + 'VerifyCurrentPassword', model);
    }
    verifyCurrentPasswordEmployee(model: any) {
        return this.http.post(this.baseUrl + 'VerifyCurrentPasswordEmployee', model);
    }
    changePassword(model: any) {
        return this.http.post(this.baseUrl + 'ChangePassword', model);
    }
    changePasswordEmployee(model: any) {
        return this.http.post(this.baseUrl + 'ChangePasswordEmployee', model);
    }
    changePasswordAdminSide(model: any) {
        
        return this.http.post(this.baseUrl + 'ChangePasswordAdminSide', model);
    }
    changePasswordAdminSideEmployee(model: any) {
        
        return this.http.post(this.baseUrl + 'ChangePasswordAdminSideEmployee', model);
    }

    changePasswordAny(employeeSAPCode: any, empId: any) {   
        debugger;
        return this.http.get(this.baseUrl + 'changePasswordAnySupport/'+employeeSAPCode+'/'+ empId);
    }
}
