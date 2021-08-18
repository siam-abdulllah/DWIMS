import { ApprovalCeiling, IApprovalCeiling } from './../shared/models/approvalCeiling';
import { GenericParams } from './../shared/models/genericParams';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
//import { MasterService } from '../master.service';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApprovalCeilingService } from '../_services/approval-ceiling.service';
import { IApprovalAuthority } from '../shared/models/approvalAuthority';
import { IDonation } from '../shared/models/donation';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';

@Component({
  selector: 'app-approval-ceiling',
  templateUrl: './approval-ceiling.component.html'
})
export class ApprovalCeilingComponent implements OnInit {

  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  genParams: GenericParams;
  approvalCeilings: IApprovalCeiling[];
  totalCount = 0;
  approvalAuthorities: IApprovalAuthority[];
  donations: IDonation[];
  bsConfig: Partial<BsDatepickerConfig>;
  bsValue: Date = new Date();
  //constructor(public approvalCeilingService: MasterService, private router: Router, private toastr: ToastrService) { }
  constructor(public approvalCeilingService: ApprovalCeilingService,private router: Router, private toastr: ToastrService) { }
  ngOnInit() {
    this.getApprovalAuthority();
    this.getApprovalCeiling();
    this. getDonation();
    this.bsConfig = Object.assign({}, { containerClass: 'theme-green' }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();
  }
  getApprovalAuthority(){
    this.approvalCeilingService.getApprovalAuthority().subscribe(response => {
      this.approvalAuthorities = response as IApprovalAuthority[];
     }, error => {
        console.log(error);
     });
  }
  getDonation(){
    this.approvalCeilingService.getDonations().subscribe(response => {
      this.donations = response as IDonation[];
    }, error => {
        console.log(error);
    });
  }
  getApprovalCeiling(){
    this.approvalCeilingService.getApprovalCeiling().subscribe(response => {
      debugger;
      this.approvalCeilings = response.data;
      this.totalCount = response.count;
    }, error => {
        console.log(error);
    });
  }

  onSubmit(form: NgForm) {
    debugger;
    if (this.approvalCeilingService.approvalCeilingFormData.id == 0)
      this.insertApprovalCeiling(form);
    else
      this.updateApprovalCeiling(form);
  }


  insertApprovalCeiling(form: NgForm) {
    this.approvalCeilingService.insertApprovalCeiling().subscribe(
      res => {
        debugger;
        this.resetForm(form);
        this.getApprovalCeiling();
        this.toastr.success('Data Saved successfully', 'Approval Ceiling')
      },
      err => { console.log(err); }
    );
  }

  updateApprovalCeiling(form: NgForm) {
    this.approvalCeilingService.updateApprovalCeiling().subscribe(
      res => {
        debugger;
        this.resetForm(form);
        this.getApprovalCeiling();
        this.toastr.info('Data Updated Successfully', 'Approval Ceiling')
      },
      err => { console.log(err); }
    );
  }

  populateForm(selectedRecord: IApprovalCeiling) {
    this.approvalCeilingService.approvalCeilingFormData = Object.assign({}, selectedRecord);
    this.approvalCeilingService.approvalCeilingFormData.investmentFrom=new Date(selectedRecord.investmentFrom);
      this.approvalCeilingService.approvalCeilingFormData.investmentTo=new Date(selectedRecord.investmentTo);
     
  }

  resetForm(form: NgForm) {
    form.form.reset();
    this.approvalCeilingService.approvalCeilingFormData = new ApprovalCeiling();
    this.approvalCeilings=[];
  }

}
