import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
//import { AccountService } from '../_services/login.service';

import { AccountService } from '../account/account.service';
import { AlertifyService } from '../_services/alertify.service';

@Injectable({
  providedIn: 'root'
})
export class AdminRoleGuard implements CanActivate {
  constructor(
    private accountService: AccountService,
    private router: Router,
    private alertfy: AlertifyService) {}
  canActivate(): boolean {
    if (!this.accountService.loggedIn()) {
        this.alertfy.error('UnAuthorized Access!!!');
        this.router.navigate(['/login']);
        return false;
    }
    if (this.accountService.getUserRole() === 'Admin' || this.accountService.getUserRole() === 'SA') {
      //  console.log(this.AccountService.getUserRole());
        return true;
    }
    this.alertfy.error('UnAuthorized Access!!!');
    this.router.navigate(['login']);
    return false;
  }
}
