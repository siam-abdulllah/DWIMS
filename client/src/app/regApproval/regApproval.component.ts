
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
  // genParams: GenericParams;
  // campaigns: ICampaign[]; 
  // subCampaigns: ISubCampaign[]; 
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
      private router: Router,
      private toastr: ToastrService,
      private fb: FormBuilder,
      public regApprovalService: RegApprovalService,
      private modalService: BsModalService,
    ) { }

  ngOnInit() {
    //this.getCampaign();
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
  SBUs = [
    {id: 1, name: 'Chittagong/Chattogram' },
    {id: 2, name: 'Sonamasjid'},
    {id: 3, name: 'Benapole'},
    {id: 4, name: 'Mongla'},
    {id: 5, name: 'Hili'},
    {id: 6, name: 'Darshana'},
    {id: 7, name: 'Shahjalal International Airport'},
    {id: 8, name: 'Banglabandha'},
    {id: 9, name: 'Birol'},
    {id: 10, name: 'Rohanpur'},
    {id: 11, name: 'Vomra'},
    {id: 12, name: 'Burimari'}
  ];
}
