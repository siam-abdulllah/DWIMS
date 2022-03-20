import { ChangePasswordAllService } from './../_services/changePasswordAll.service';
import { IRegApproval,RegApproval} from'../shared/models/regApproval';
import { GenericParams } from '../shared/models/genericParams';
import { Component, ElementRef, OnInit, ViewChild , TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
//import { regApprovalService } from '../master.service';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService} from 'ngx-bootstrap/modal';
import {FormGroup,FormBuilder,Validators,AsyncValidatorFn} from '@angular/forms';
import {RegApprovalService} from '../_services/regApproval.service';
import { AccountService } from '../account/account.service';
import { NgxSpinnerService } from 'ngx-spinner';
@Component({
  selector: 'app-regApproval',
  templateUrl: './changePasswordAll.component.html',
  styles: [
  ]
})
export class ChangePasswordAllComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  @ViewChild('regApprovalSearchModal', { static: false }) regApprovalSearchModal: TemplateRef<any>;
  RegApprovalSearchModalRef: BsModalRef;
  searchText = '';
  roleList = [];
  errors: string[];
  regApprovals: IRegApproval[];
  empId: string;
  userRole: any;
  totalCount = 0;
  bsConfig: Partial<BsDatepickerConfig>;
  bsValue: Date = new Date();
  config = {
    keyboard: false,
    class: 'modal-lg',
    ignoreBackdropClick: true
  };
    constructor( 
      private accountService: AccountService,
      private changePasswordService: ChangePasswordAllService,
      private router: Router,
      private toastr: ToastrService,
      private fb: FormBuilder,
      public regApprovalService: RegApprovalService,
      private modalService: BsModalService,
      private SpinnerService: NgxSpinnerService
    ) { }
    resetSearch() {
      this.searchText = '';
    }
  ngOnInit() {
    this.resetPage();
    this.getRoles();
    this.getEmployeeId();
    this.bsConfig = Object.assign({}, { containerClass: 'theme-blue'  }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();
  }
  openRegApprovalSearchModal(template: TemplateRef<any>) {
    this.RegApprovalSearchModalRef = this.modalService.show(template, this.config);
  }
    
  getEmployeeId() {
    this.empId = this.accountService.getEmployeeId();
    this.userRole = this.accountService.getUserRole();
  }
  selectRegApproval(selectedRecord: IRegApproval) {
    this.regApprovalService.regApprovalFormData = Object.assign({}, selectedRecord);
    this.getUserById();
    this.RegApprovalSearchModalRef.hide()
  }
 
  getRegApproved(){
    this.SpinnerService.show(); 
    this.regApprovalService.getRegApproved().subscribe(response => {
      this.SpinnerService.hide();
      this.regApprovals = response as IRegApproval[];
      if (this.regApprovals.length>0) {
this.openRegApprovalSearchModal(this.regApprovalSearchModal);
      }
      else {
        this.toastr.warning('No Data Found');
      }
      //this.totalCount = response.count;
    }, error => {
      this.SpinnerService.hide();
        console.log(error);
    });
  }
  

  changePasswordAll() {
    this.SpinnerService.show(); 
    var employeeSAPCode = this.regApprovalService.regApprovalFormData.employeeId.toString();
    this.changePasswordService.changePasswordAny(employeeSAPCode, this.empId ).subscribe(
      res => {
        this.SpinnerService.hide();
        this.toastr.info('Updated successfully', 'Change Password')
      },
      err => { 
        console.log(err);
        this.SpinnerService.hide();
       }
    );
  }

  populateForm() {
    //this.regApprovalService.campaignFormData = Object.assign({}, selectedRecord);
  }
  resetForm(form: NgForm) {
    form.form.reset();
    //this.regApprovalService.campaignFormData = new Campaign();
  }
  resetPage() {
    this.regApprovalService.regApprovalFormData = new RegApproval();
  }
  getRoles() {
    this.roleList = [];
    this.accountService.getRoles().subscribe(
      (response) => {
        if (response) {
          this.roleList = response;
        }
      },
      (error) => {
        this.errors = error.errors;
      }
    );
  }
  getUserById(){
    this.accountService.getUserById(this.regApprovalService.regApprovalFormData.userId).subscribe(
      (response) => {
        if (response) 
        {
          response.roles.forEach(element => {
            this.regApprovalService.regApprovalFormData.role = element;
                }); 
        }
      },
      (error) => {
        this.errors = error.errors;
      }
    );
  }
}
