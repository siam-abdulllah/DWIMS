import { Component, ElementRef, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from "ngx-spinner";
import { GenericParams } from '../shared/models/genericParams';
import { AccountService } from '../account/account.service';
import { CurrencyPipe, DatePipe, getLocaleDateTimeFormat } from '@angular/common';
import { BgtEmpDisburseService } from '../_services/bgtEmpDisburse.service';
import { IApprovalAuthority } from '../shared/models/approvalAuthority';
import { ISBU } from '../shared/models/sbu';
import { IDonation } from '../shared/models/donation';
import { BDCurrencyPipe } from '../bdNumberPipe';

@Component({
  selector: 'bgtEmpDisburse',
  templateUrl: './bgtEmpDisburse.component.html',
})
export class BgtEmpDisburseComponent implements OnInit {

  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  @ViewChild('pendingListModal', { static: false }) pendingListModal: TemplateRef<any>;
  pendingListModalRef: BsModalRef;
  bsConfig: Partial<BsDatepickerConfig>;
  donations: IDonation[];
  bsValue: Date = new Date();
  bgtEmpDisburse: FormGroup;
  empId: string;
  genParams: GenericParams;
  approvalAuthorities: IApprovalAuthority[];
  SBUs: ISBU[];
  numberPattern = "^[0-9]+(.[0-9]{1,10})?$";

totalBudget: number = 0;
totalAlloc: number = 0;


  donationTypeName: string;
  sbuData: SbuData[];
  config: any;
  totalCount = 0;
  userRole: any;
  constructor(public bgtService: BgtEmpDisburseService, public BDCurrency: BDCurrencyPipe, private SpinnerService: NgxSpinnerService, private modalService: BsModalService, private accountService: AccountService,
    private router: Router, private toastr: ToastrService, private datePipe: DatePipe, public _formBuilder: FormBuilder) {
  }

  createbgtEmpDisburseForm() {
    // this.bgtEmpDisburse = new FormGroup({
    this.bgtEmpDisburse = this._formBuilder.group({


      deptId: ['', Validators.required],
      year: ['', Validators.required],
      authId: ['', Validators.required],
      sbu: ['', Validators.required],
      ttlAmount: ['', Validators.pattern('^[0-9]+(.[0-9]{1,10})?$')],
      donationAmount: [''],
      amtLimit: [''],
      sbuTotalBudget: [''],
      ttlAllocate: [''],
      prevAllocate: [''],
      ttlPerson: [''],
      remaining: [''],
      donationAmt: [''],
      transLimit: [''],

      grdAmount: [''],
      grdLimit: [''],
      month: [''],
    });
  }

  getAllocatedAmount() {

    this.totalBudget = 0;
    this.totalAlloc = 0;

    var yr = new Date(this.bgtEmpDisburse.value.year);
    this.bgtService.getAuthBudget(this.bgtEmpDisburse.value.sbu, this.bgtEmpDisburse.value.deptId, yr.getFullYear(), this.bgtEmpDisburse.value.authId).subscribe(
      res => {

        this.totalBudget = res[0].count;
      },
      err => { console.log(err); }
    );

    for (var i = 0; i < this.sbuData.length; i++) { 
      this.totalAlloc = this.totalAlloc +  this.sbuData[i].total;
    }
  }

  getApprovalAuthority() {
    this.bgtService.getApprovalAuthority().subscribe(response => {
      this.approvalAuthorities = response as IApprovalAuthority[];
    }, error => {
      console.log(error);
    });
  }

  getSBU() {
    this.bgtService.getSBU().subscribe(response => {
      this.SBUs = response as ISBU[];
    }, error => {
      console.log(error);
    });
  }

  resetSegment() {
    this.bgtEmpDisburse.patchValue({
      remaining: "",
      ttlAllocate: "",
    });
  }

  ngOnInit() {
    this.createbgtEmpDisburseForm();
    this.reset();
    this.getEmployeeId();
    this.getSBU();
    this.getApprovalAuthority();
  }

  getEmployeeId() {
    this.empId = this.accountService.getEmployeeId();
    this.userRole = this.accountService.getUserRole();
  }

  generateData() {
      if (this.bgtEmpDisburse.value.deptId == "" || this.bgtEmpDisburse.value.deptId == null) {
      this.toastr.error('Select Department');
      return;
    }

    if (this.bgtEmpDisburse.value.year == "" || this.bgtEmpDisburse.value.year == null) {
      this.toastr.error('Select Year');
      return;
    }

    if (this.bgtEmpDisburse.value.sbu == "" || this.bgtEmpDisburse.value.sbu == null) {
      this.toastr.error('Select SBU');
      return;
    }

    if (this.bgtEmpDisburse.value.authId == "" || this.bgtEmpDisburse.value.authId == null) {
      this.toastr.error('Select Authorization Level');
      return;
    }

    if (this.bgtEmpDisburse.value.deptId == 1 ) {
      if(this.bgtEmpDisburse.value.authId == 8 || this.bgtEmpDisburse.value.authId == 15 ||  this.bgtEmpDisburse.value.authId == 16 || this.bgtEmpDisburse.value.authId == 17 || this.bgtEmpDisburse.value.authId == 18)
      {
      this.toastr.error('Wrong Department / Authorization Combination');
      return;
      }
    }

    if (this.bgtEmpDisburse.value.deptId == 2) {
      if(this.bgtEmpDisburse.value.authId != 8 && this.bgtEmpDisburse.value.authId != 15 &&  this.bgtEmpDisburse.value.authId != 16 && this.bgtEmpDisburse.value.authId != 17 && this.bgtEmpDisburse.value.authId != 18)
      {
      this.toastr.error('Wrong Department / Authorization Combination');
      return;
      }
     
    }

    var yr = new Date(this.bgtEmpDisburse.value.year);
    this.bgtService.getSBUWiseEmpDisburse(this.bgtEmpDisburse.value.sbu, this.bgtEmpDisburse.value.deptId, yr.getFullYear(), this.bgtEmpDisburse.value.authId).subscribe(
      res => {
        this.sbuData = res as ISbuData[];
        this.getAllocatedAmount();
      },
      err => { console.log(err); }
    );
  }

  saveData(selectedRow: ISbuData) {

      if(selectedRow.newAllocated == null)
      {
            this.toastr.error('Input is necessary!');
            return;
      }

      if( selectedRow.newAllocated > (selectedRow.total + this.totalBudget - this.totalAlloc))
      {
        this.toastr.error('No Sufficient Budget For Allocation');
        return;
      }
      if(selectedRow.newAllocated < selectedRow.expense || selectedRow.newAllocated < selectedRow.allocated )
      {
            this.toastr.error('Allocated Amount Should be Higher than Expense & Current Allocation');
            return;
      }
    
      var yr = new Date(this.bgtEmpDisburse.value.year);

      selectedRow.deptId = this.bgtEmpDisburse.value.deptId;
      selectedRow.year = yr.getFullYear();
      selectedRow.enteredBy = parseInt(this.empId);
    

    this.bgtService.insertBgtEmpDisburse(selectedRow).subscribe(
      res => {
        this.toastr.success('Budget Distributed successfully', 'Budget Distribution')
        this.generateData();
      },
      err => { console.log(err); }
    );
  }


  getTotal(selectedRow: ISbuData) {

    // if(selectedRow.newAllocated > selectedRow.total)
    // {
    //   selectedRow.newAllocated = 0;
    //   this.toastr.error('Budget Exceed !!');
    //   return;
    // }

    if(selectedRow.newAllocated < selectedRow.expense || selectedRow.newAllocated < selectedRow.allocated )
      {
          selectedRow.newAllocated = 0;
          this.toastr.error('Allocated Amount Should be Higher than Expense & Current Allocation');
          return;
      }

  }

  openPendingListModal(template: TemplateRef<any>) {
    this.pendingListModalRef = this.modalService.show(template, this.config);
  }

  reset() {

    this.totalBudget= 0;
    this.totalAlloc = 0;

    this.bgtEmpDisburse.setValue({
      deptId: "",
      year: "",
      authId: "",
      sbu: "",
      ttlAmount: "",
      donationAmount: "",
      amtLimit: "",
      sbuTotalBudget: "",
      ttlAllocate: "",
      prevAllocate: "",
      ttlPerson: "",
      remaining: "",
      donationAmt: "",
      transLimit: "",
      grdAmount: "",
      grdLimit: "",
      month: "",
    });

    this.sbuData = [];
  }
}

interface ISbuData {
  compId: number;
  sbu: string;
  year: number;
  authId: number;
  deptId: string;
  employeeId: number;
  employeeName: string;
  code: string;
  compoCode: string;
  total: number;
  allocated: number;
  expense: number | null;
  newAllocated: number;
  enteredBy: number;
}

export class SbuData implements ISbuData {
  compId: number;
  sbu: string;
  year: number;
  authId: number;
  deptId: string;
  employeeId: number;
  employeeName: string;
  code: string;
  compoCode: string;
  total: number;
  allocated: number;
  expense: number | null;
  newAllocated: number;
  enteredBy: number;
} 