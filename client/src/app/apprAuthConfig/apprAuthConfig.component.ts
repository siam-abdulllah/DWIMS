import { Employee, IEmployee } from '../shared/models/employee';
import { ApprovalAuthConfig, IApprovalAuthConfig } from '../shared/models/approvalAuthConfig';
import { Market, IMarket } from '../shared/models/market';
import { GenericParams } from '../shared/models/genericParams';
import { Component, ElementRef, OnInit, ViewChild , TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { ApprAuthConfigService } from '../_services/apprAuthConfig.service';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApprovalAuthority, IApprovalAuthority } from '../shared/models/approvalAuthority';

@Component({
  selector: 'app-apprAuthConfig',
  templateUrl: './apprAuthConfig.component.html',
  styles: [
  ]
})
export class ApprAuthConfigComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  approvalAuthorities: IApprovalAuthority[];
  approvalAuthConfigs: IApprovalAuthConfig[]; 
  employees: IEmployee[];
  employeesForApprAuth: IEmployeeForApprAuth[];
  totalCount = 0;
 
  constructor(public apprAuthConfigService: ApprAuthConfigService, private router: Router,
    private toastr: ToastrService) { }

  ngOnInit() {
    this.resetForm();
    this.getApprovalAuthority();
    this.getEmployees();
    
  }
  resetPage(form: NgForm) {
    form.reset();
    this.apprAuthConfigService.approvalAuthConfigFormData = new ApprovalAuthConfig();
    this.employeesForApprAuth=[];
  }
  resetForm() {
    this.apprAuthConfigService.approvalAuthConfigFormData = new ApprovalAuthConfig();
    this.employeesForApprAuth=[];
  }
  getApprovalAuthConfigs(){

    this.apprAuthConfigService.getApprovalAuthConfigs(parseInt(this.apprAuthConfigService.approvalAuthConfigFormData.approvalAuthorityId)).subscribe(response => {
      debugger;
      this.employeesForApprAuth = response as IEmployeeForApprAuth[];
     }, error => {
        console.log(error);
     });
  }
  getApprovalAuthority(){
    this.apprAuthConfigService.getApprovalAuthority().subscribe(response => {
      this.approvalAuthorities = response as IApprovalAuthority[];
     }, error => {
        console.log(error);
     });
  }
  getEmployees(){
    this.apprAuthConfigService.getEmployees().subscribe(response => {
      debugger;
      this.employees = response as IEmployee[];
    //   this.totalCount = response.count;
     }, error => {
        console.log(error);
     });
  }
 
 
  onSubmit(form: NgForm) {
    debugger;
    // if (this.apprAuthConfigService.campaignFormData.id == 0)
      this.insertApprAuthConfig(form);
    // else
    //   this.updateCampaign(form);
  }

  insertApprAuthConfig(form: NgForm) {
    this.apprAuthConfigService.insertApprAuthConfig().subscribe(
      res => {
        debugger;
        const approvalAuthConfigMst = res as IApprovalAuthConfig;
        //this.resetForm(form);
        this.getApprovalAuthConfigs();
        this.toastr.success('Insert Successfully');
      },
      err => { 
        this.toastr.warning('Insert Failed');
        console.log(err); }
    );
  }

  


}
interface IEmployeeForApprAuth {
  employee:IEmployee;
   
}
