import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { AccountService } from '../account/account.service';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class SuperAdminRoleGuard implements CanActivate {
  constructor(
    private accountService: AccountService,
    private router: Router, private toastr: ToastrService) {}
    // canActivate(): boolean {
    //   if (!this.accountService.loggedIn()) {
    //       this.toastr.error('UnAuthorized Access!!!');
    //       this.router.navigate(['/login']);
    //       return false;
    //   }
    //   if (this.accountService.getUserRole() === 'Administrator') {
    //       // console.log(this.loginService.getUserRole());
    //       return true;
    //   }
    //   this.toastr.error('UnAuthorized Access!!!');
    //   this.router.navigate(['login']);
    //   return false;
    // }
    canActivate(
      next: ActivatedRouteSnapshot,
      state: RouterStateSnapshot): boolean {
      if (!this.accountService.loggedIn()) {
          this.toastr.error('UnAuthorized Access!!!');
          this.router.navigate(['/login']);
          return false;
      }
      // if (this.accountService.getUserRole() === 'Administrator') {
         
      //     return true;
      // }
      if (this.accountService.isMenuPermitted(state.url)) {
          // console.log(this.loginService.getUserRole());
          return true;
      }

      this.toastr.error('UnAuthorized Access!!!');
      this.router.navigate(['login']);
      return false;
    }
}