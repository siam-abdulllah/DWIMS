
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
@Component({
  selector: 'app-regApproval',
  templateUrl: './regApproval.component.html',
  styles: [
  ]
})
export class RegApprovalComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  @ViewChild('regApprovalSearchModal', { static: false }) regApprovalSearchModal: TemplateRef<any>;
  RegApprovalSearchModalRef: BsModalRef;
  roleList = [];
  errors: string[];
  regApprovals: IRegApproval[];
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
      private router: Router,
      private toastr: ToastrService,
      private fb: FormBuilder,
      public regApprovalService: RegApprovalService,
      private modalService: BsModalService,
    ) { }

  ngOnInit() {
    this.getRoles();
    this.bsConfig = Object.assign({}, { containerClass: 'theme-green' }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();
  }
  openRegApprovalSearchModal(template: TemplateRef<any>) {
    this.RegApprovalSearchModalRef = this.modalService.show(template, this.config);
  }
    
  getEmployeeFormApproval(){
    this.regApprovalService.getEmployeeFormApproval().subscribe(response => {
      debugger;
      this.regApprovals = response as IRegApproval[];
      this.openRegApprovalSearchModal(this.regApprovalSearchModal);
      //this.totalCount = response.count;
    }, error => {
        console.log(error);
    });
  }
  getRegApproved(){
    this.regApprovalService.getRegApproved().subscribe(response => {
      debugger;
      this.regApprovals = response as IRegApproval[];
      this.openRegApprovalSearchModal(this.regApprovalSearchModal);
      //this.totalCount = response.count;
    }, error => {
        console.log(error);
    });
  }
  selectRegApproval(selectedRecord: IRegApproval) {
    this.regApprovalService.regApprovalFormData = Object.assign({}, selectedRecord);
    this.getUserById();
    this.RegApprovalSearchModalRef.hide()
  }
  onSubmit(form: NgForm) {
      this.updateRegApproval(form);
  }

  

  updateRegApproval(form: NgForm) {
    this.regApprovalService.updateRegApproval().subscribe(
      res => {
        debugger;
        this.resetForm(form);
        this.toastr.info('Updated successfully', 'User registration Approval')
      },
      err => { console.log(err); }
    );
  }

  populateForm() {
    //this.regApprovalService.campaignFormData = Object.assign({}, selectedRecord);
  }
  resetForm(form: NgForm) {
    form.form.reset();
    //this.regApprovalService.campaignFormData = new Campaign();
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
