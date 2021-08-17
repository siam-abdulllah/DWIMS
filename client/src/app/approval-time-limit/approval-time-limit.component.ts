import { ApprovalTimeLimit, IApprovalTimeLimit } from './../shared/models/approvalTimeLimit';
import { GenericParams } from './../shared/models/genericParams';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ApprovalTimeLimitService } from '../_services/approval-time-limit.service';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-approval-time-limit',
  templateUrl: './approval-time-limit.component.html'
})
export class ApprovalTimeLimitComponent implements OnInit {

  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  genParams: GenericParams;
  approvalTimeLimit: IApprovalTimeLimit[];
  totalCount = 0;
  constructor(public approvalTimeService: ApprovalTimeLimitService, private router: Router, private toastr: ToastrService) { }
  //constructor(private router: Router, private toastr: ToastrService) { }
  ngOnInit() {
     this.getApprovalTimeLimit();
  }

  getApprovalTimeLimit(){
    this.approvalTimeService.getApprovalTimeLimit().subscribe(response => {
      debugger;
      this.approvalTimeLimit = response.data;
      this.totalCount = response.count;
    }, error => {
        console.log(error);
    });
  }

  onSubmit(form: NgForm) {
    debugger;
    if (this.approvalTimeService.approvalTimeLimitFormData.id == 0)
      this.insertApprovalTimeLimit(form);
    else
      this.updateApprovalTimeLimit(form);
  }


  insertApprovalTimeLimit(form: NgForm) {
    this.approvalTimeService.insertApprovalTimeLimit().subscribe(
      res => {
        debugger;
        this.resetForm(form);
        this.getApprovalTimeLimit();
        this.toastr.success('Data Saved successfully', 'Approval Time Limit')
      },
      err => { console.log(err); }
    );
  }

  updateApprovalTimeLimit(form: NgForm) {
    this.approvalTimeService.updateApprovalTimeLimit().subscribe(
      res => {
        debugger;
        this.resetForm(form);
        this.getApprovalTimeLimit();
        this.toastr.info('Data Updated Successfully', 'Approval Time Limit')
      },
      err => { console.log(err); }
    );
  }

  populateForm(selectedRecord: IApprovalTimeLimit) {
    this.approvalTimeService.approvalTimeLimitFormData = Object.assign({}, selectedRecord);
  }

  resetForm(form: NgForm) {
    form.form.reset();
    this.approvalTimeService.approvalTimeLimitFormData = new ApprovalTimeLimit();
  }

}
