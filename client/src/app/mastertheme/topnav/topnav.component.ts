import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/account/account.service';
@Component({
  selector: 'app-topnav',
  templateUrl: './topnav.component.html',
  styles: [
  ]
})
export class TopnavComponent implements OnInit {
  sbu:string;
  employeeName:string;
  desgination:string;
  displayName:string;
  constructor(private accountService: AccountService,private router: Router) { }

  ngOnInit() {
    this.displayName = localStorage.getItem('displayName');
    this.getEmployeeId();
  }
  getEmployeeId() {

    this.accountService.getEmployeeSbu(parseInt(this.accountService.getEmployeeId())).subscribe(
      (response) => {
        this.sbu = response.sbu;
        this.employeeName = response.employeeName;
        this.desgination = response.designationName;
      },
      (error) => {
        console.log(error);
      }
    );

  }
  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }
}
