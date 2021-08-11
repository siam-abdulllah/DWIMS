//import { BcdsInfo, IBcdsInfo } from './../../shared/models/bcdsInfo';
//import { GenericParams } from './../../shared/models/genericParams';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
//import { MasterService } from '../master.service';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-user-registration',
  templateUrl: './user-registration.component.html',
  styleUrls: ['./user-registration.component.scss']
})
export class UserRegistrationComponent implements OnInit {

  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  //genParams: GenericParams;
  //bcdsInfo: IBcdsInfo[];
  totalCount = 0;
  //constructor(public masterService: MasterService, private router: Router, private toastr: ToastrService) { }
  constructor(private router: Router, private toastr: ToastrService) { }
  ngOnInit() {
    // this.getBcds();
  }

  // getBcds(){
  //   this.masterService.getBcdsList().subscribe(response => {
  //     debugger;
  //     this.bcdsInfo = response.data;
  //     this.totalCount = response.count;
  //   }, error => {
  //       console.log(error);
  //   });
  // }

  // onSubmit(form: NgForm) {
  //   debugger;
  //   if (this.masterService.approvaltimelimitFormData.id == 0)
  //     this.insertBcds(form);
  //   else
  //     this.updateBcds(form);
  // }


  // insertBcds(form: NgForm) {
  //   this.masterService.insertBcds().subscribe(
  //     res => {
  //       debugger;
  //       this.resetForm(form);
  //       this.getBcds();
  //       this.toastr.success('Data Saved successfully', 'BCDS Information')
  //     },
  //     err => { console.log(err); }
  //   );
  // }

  // updateBcds(form: NgForm) {
  //   this.masterService.updateBcds().subscribe(
  //     res => {
  //       debugger;
  //       this.resetForm(form);
  //       this.getBcds();
  //       this.toastr.info('Data Updated Successfully', 'BCDS Information')
  //     },
  //     err => { console.log(err); }
  //   );
  // }

  // populateForm(selectedRecord: IBcdsInfo) {
  //   this.masterService.bcdsFormData = Object.assign({}, selectedRecord);
  // }
  // resetForm(form: NgForm) {
  //   form.form.reset();
  //   this.masterService.bcdsFormData = new BcdsInfo();
  // }

}
