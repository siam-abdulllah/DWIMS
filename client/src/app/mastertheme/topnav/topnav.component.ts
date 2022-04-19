import { DOCUMENT } from '@angular/common';
import { Component, ElementRef, HostListener, Inject, OnInit, Renderer2 } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/account/account.service';
@Component({
  selector: 'app-topnav',
  templateUrl: './topnav.component.html',
  styles: ['./topnav.component.scss']
})
export class TopnavComponent implements OnInit {
  sbu: string;
  employeeName: string;
  desgination: string;
  displayName: string;
  constructor(private accountService: AccountService, private router: Router) { }

  ngOnInit() {
    this.displayName = localStorage.getItem('displayName');
    this.getEmployeeId();
  }
  getEmployeeId() {
    this.accountService.getEmployeeSbu(parseInt(this.accountService.getEmployeeId())).subscribe(
      (response) => {
        this.sbu = response.sbuName;
        this.employeeName = response.employeeName;
        this.desgination = response.designationName;
      },
      (error) => {
        console.log(error);
      }
    );

  }
  checkPermission() {
    // if(this.loginService.getUserRole()=="Importer")
    // {
    //   this.router.navigate(['/portal/editimporterinfo']);
    // }
    // else if(this.loginService.getUserRole()=="Admin" || "SA" || "Executive")
    // {
      this.router.navigate(['/portal/changePassword']);
    // }
    // else{
    //   console.log("Other");
    // }
  }
  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }
}
