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
import { DatePipe } from '@angular/common';
import { NgxSpinnerService } from "ngx-spinner"; 

@Component({
  selector: 'app-approval-ceiling',
  templateUrl: './approval-ceiling.component.html'
})
export class ApprovalCeilingComponent implements OnInit {

  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  genParams: GenericParams;
  approvalCeilings: IApprovalCeiling[];
  totalCount = 0;
  searchText = '';
  config: any;
  approvalAuthorities: IApprovalAuthority[];
  donations: IDonation[];
  bsConfig: Partial<BsDatepickerConfig>;
  numberPattern="^[0-9]+(.[0-9]{1,10})?$";
  bsValue: Date = new Date();
  //constructor(public approvalCeilingService: MasterService, private router: Router, private toastr: ToastrService) { }
  constructor(public approvalCeilingService: ApprovalCeilingService,private router: Router, private toastr: ToastrService,
    private datePipe: DatePipe,private SpinnerService: NgxSpinnerService) { }
  ngOnInit() {
    this.resetPage();
    this.getApprovalAuthority();
    this.getApprovalCeiling();
    this. getDonation();
    this.bsConfig = Object.assign({}, { containerClass: 'theme-blue'  }, { dateInputFormat: 'DD/MM/YYYY' });
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
    const params = this.approvalCeilingService.getGenParams();
    this.approvalCeilingService.getApprovalCeiling().subscribe(response => {
      debugger;
      this.approvalCeilings = response.data;
      this.totalCount = response.count;
      this.config = {
        currentPage: params.pageIndex,
        itemsPerPage: params.pageSize,
        totalItems:this.totalCount,
        };
    }, error => {
      
        console.log(error);
    });
  }

  onSubmit(form: NgForm) {
    debugger;
    this.approvalCeilingService.approvalCeilingFormData.investmentFrom = this.datePipe.transform(this.approvalCeilingService.approvalCeilingFormData.investmentFrom, 'yyyy-MM-dd HH:mm:ss');
    this.approvalCeilingService.approvalCeilingFormData.investmentTo = this.datePipe.transform(this.approvalCeilingService.approvalCeilingFormData.investmentTo, 'yyyy-MM-dd HH:mm:ss');
   if (this.approvalCeilingService.approvalCeilingFormData.id == 0)
      this.insertApprovalCeiling(form);
    else
      this.updateApprovalCeiling(form);
  }

  dateCompare() {
    if (this.approvalCeilingService.approvalCeilingFormData.investmentFrom != null && this.approvalCeilingService.approvalCeilingFormData.investmentTo != null) {
      if (this.approvalCeilingService.approvalCeilingFormData.investmentTo > this.approvalCeilingService.approvalCeilingFormData.investmentFrom) {
return true;
      }
      else {
        this.toastr.error('Select Appropriate Date Range', 'Error');
        return false;
      }
    }
  }

  insertApprovalCeiling(form: NgForm) {
    if(this.dateCompare()){
    this.SpinnerService.show(); 
    this.approvalCeilingService.insertApprovalCeiling().subscribe(
      res => {
        debugger;
        this.resetForm(form);
        this.getApprovalCeiling();
        this.SpinnerService.hide(); 
        this.toastr.success('Data Saved successfully', 'Approval Ceiling')
      },
      err => {
        debugger;
        this.approvalCeilingService.approvalCeilingFormData.investmentFrom = new Date(this.approvalCeilingService.approvalCeilingFormData.investmentFrom);
        this.approvalCeilingService.approvalCeilingFormData.investmentTo = new Date(this.approvalCeilingService.approvalCeilingFormData.investmentTo);
        this.SpinnerService.hide(); 
        this.toastr.error(err.errors[0], 'Approval Ceiling')
        console.log(err);
      }
    );
  }
  }

  updateApprovalCeiling(form: NgForm) {
    if(this.dateCompare()){
    this.SpinnerService.show(); 
    this.approvalCeilingService.updateApprovalCeiling().subscribe(
      res => {
        debugger;
        this.resetForm(form);
        this.getApprovalCeiling();
        this.SpinnerService.hide(); 
        this.toastr.info('Data Updated Successfully', 'Approval Ceiling')
      },
      err => {  
        this.approvalCeilingService.approvalCeilingFormData.investmentFrom = new Date(this.approvalCeilingService.approvalCeilingFormData.investmentFrom);
        this.approvalCeilingService.approvalCeilingFormData.investmentTo = new Date(this.approvalCeilingService.approvalCeilingFormData.investmentTo);
        this.SpinnerService.hide();  
        this.toastr.error(err.errors[0], 'Approval Ceiling')
      console.log(err); }
    );
      }
  }

  populateForm(selectedRecord: IApprovalCeiling) {
    this.approvalCeilingService.approvalCeilingFormData = Object.assign({}, selectedRecord);
    this.approvalCeilingService.approvalCeilingFormData.investmentFrom=new Date(selectedRecord.investmentFrom);
      this.approvalCeilingService.approvalCeilingFormData.investmentTo=new Date(selectedRecord.investmentTo);
     
  }

  resetForm(form: NgForm) {
    form.form.reset();
    this.approvalCeilingService.approvalCeilingFormData = new ApprovalCeiling();
    //this.approvalCeilings=[];
    this.searchText = '';
  }
  resetPage() {
    this.approvalCeilingService.approvalCeilingFormData = new ApprovalCeiling();
    this.approvalCeilings=[];
    this.searchText = '';
    this.config = {
      currentPage: 1,
      itemsPerPage: 10,
      totalItems:50,
      };
  }
  resetSearch(){
    this.searchText = '';
}
onPageChanged(event: any){
  const params = this.approvalCeilingService.getGenParams();
  if (params.pageIndex !== event)
  {
    params.pageIndex = event;
    this.approvalCeilingService.setGenParams(params);
    this.getApprovalCeiling();
  }
}

}
