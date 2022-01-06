
import { DashboardService } from './../_services/dashboard.service';
import { AccountService } from '../account/account.service';
import { IEmployeeInfo } from '../shared/models/employeeInfo';
import { Component, OnInit } from '@angular/core';
import {
  InvestmentRec, IInvestmentRec, InvestmentInit, IInvestmentInit,
  InvestmentTargetedProd, IInvestmentTargetedProd, InvestmentTargetedGroup, IInvestmentTargetedGroup, IInvestmentRecComment, InvestmentRecComment
} from '../shared/models/investmentRec';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styles: [
  ]
})
export class HomeComponent implements OnInit {
  employeeInfo: IEmployeeInfo[] = [];
  userRole: any;
  sbu: any;
  empId: any;
  totalApproved: any;
  myPending: any;
  aprPending: any;
  marketCode: string;
  constructor(private router: Router, private accountService: AccountService, public dashboardService: DashboardService) { }

  ngOnInit() {
    this.userRole = this.accountService.getUserRole();
    this.empId = this.accountService.getEmployeeId();
    this.getEmpSbu();
  }

  getEmpSbu(){
    this.accountService.getEmployeeSbu(this.empId).subscribe(
      (response) => {
        this.sbu = response.sbu;
        this.marketCode = response.marketCode;
        this.getTotalApproved();
      // this.getMyPending();
      },
      (error) => {
        console.log(error);
      });
  }
  
  getTotalApproved() {
    this.dashboardService.getTotalApproved(this.userRole,this.empId).subscribe(response => {
      var data = response;
      this.totalApproved = data;
      this.getMyPending();
    }, error => {
      console.log(error);
    });
  }

  getApproveDetail(){
      this.router.navigate([]).then(result => {  window.open('/portal/paramInvestmentSummary/Approved', '_blank'); });;
  }

  getMyPending() {
    this.dashboardService.getMyPending(this.sbu,this.userRole,this.empId).subscribe(response => {
      var data = response;
      this.myPending = data;
      if(this.userRole=='Administrator')
      {
        this.getAprPending();
      }
    }, error => {
      console.log(error);
    });
  }

  getAprPending() {
    this.dashboardService.getApprovalPending(this.userRole,this.empId).subscribe(response => {
      var data = response;
      this.aprPending = data;
    }, error => {
      console.log(error);
    });
  }

  getPendingDetail(){
    this.router.navigate([]).then(result => {  window.open('/portal/paramInvestmentSummary/Pending', '_blank'); });;
}
}
