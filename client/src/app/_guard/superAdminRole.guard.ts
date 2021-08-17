import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AccountService } from '../account/account.service';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class SuperAdminRoleGuard implements CanActivate {
  constructor(
    private accountService: AccountService,
    private router: Router, private toastr: ToastrService) {}
  canActivate(): boolean {
    debugger;
    if (!this.accountService.loggedIn()) {
        this.toastr.error('UnAuthorized Access!!!');
        this.router.navigate(['/login']);
        return false;
    }
    if (this.accountService.getUserRole() === 'Administrator') {
        // console.log(this.accountService.getUserRole());
        return true;
    }
    this.toastr.error('UnAuthorized Access!!!');
    this.router.navigate(['login']);
    return false;
  }
}