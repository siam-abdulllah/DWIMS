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
    canActivate(): boolean {
      if (!this.accountService.loggedIn()) {
          this.toastr.error('UnAuthorized Access!!!');
          this.router.navigate(['/login']);
          return false;
      }
      if (this.accountService.getUserRole() === 'Administrator') {
          // console.log(this.loginService.getUserRole());
          return true;
      }
      this.toastr.error('UnAuthorized Access!!!');
      this.router.navigate(['login']);
      return false;
    }
  // canActivate(route: ActivatedRouteSnapshot,
  //   state: RouterStateSnapshot): boolean {
  //     debugger;
  //   if (!this.accountService.loggedIn()) {
  //       this.toastr.error('Unauthorized Access!!!');
  //       this.router.navigate(['/login']);
  //       return false;
  //   }
    
  //     this.accountService.eventPerm(state.url).subscribe(response => {
  //       debugger;
  //       if(response){
  //       return true;
  //     }
  //     else{
  //       this.toastr.error('Unauthorized Access!!!');
  //       this.router.navigate(['login']);
  //       return false;
  //     }
        
  //     }, error => {
  //         console.log(error);
  //     });
  // }
}