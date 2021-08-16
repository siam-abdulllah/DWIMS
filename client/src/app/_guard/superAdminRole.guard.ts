import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AccountService } from '../account/account.service';
import { AlertifyService } from '../_services/alertify.service';

@Injectable({
  providedIn: 'root'
})
export class SuperAdminRoleGuard implements CanActivate {
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
    if (this.accountService.getUserRole() === 'SA') {
        // console.log(this.accountService.getUserRole());
        return true;
    }
    this.alertfy.error('UnAuthorized Access!!!');
    this.router.navigate(['login']);
    return false;
  }
}