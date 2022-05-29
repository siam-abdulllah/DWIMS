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
import { BgtEmployeeService } from '../_services/bgtEmployee.service';
import { IApprovalAuthority } from '../shared/models/approvalAuthority';
import { ISBU } from '../shared/models/sbu';
import { IDonation } from '../shared/models/donation';

@Component({
  selector: 'bgtEmployee',
  templateUrl: './bgtEmployee.component.html',
})
export class BgtEmployeeComponent implements OnInit {

  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  @ViewChild('pendingListModal', { static: false }) pendingListModal: TemplateRef<any>;
  pendingListModalRef: BsModalRef;
  bsConfig: Partial<BsDatepickerConfig>;
  donations: IDonation[];
  bsValue: Date = new Date();
  bgtEmployee: FormGroup;
  empId: string;
  genParams: GenericParams;
  approvalAuthorities: IApprovalAuthority[];
  SBUs: ISBU[];
  numberPattern = "^[0-9]+(.[0-9]{1,10})?$";
  btnShow: boolean = true;
  donationTypeName: string;
  sbuData: SbuData[];
  searchText = '';
  config: any;
  totalCount = 0;
  userRole: any;
  constructor(public bgtService: BgtEmployeeService, private SpinnerService: NgxSpinnerService, private modalService: BsModalService, private accountService: AccountService,
    private router: Router, private toastr: ToastrService, private datePipe: DatePipe, public _formBuilder: FormBuilder) {
  }

  createbgtEmployeeForm() {
    // this.bgtEmployee = new FormGroup({
    this.bgtEmployee = this._formBuilder.group({
      // deptId: new FormControl('', [Validators.required]),
      // year: new FormControl('', [Validators.required]),
      // authId: new FormControl('', [Validators.required]),
      // sbu: new FormControl('', [Validators.required]),
      // ttlAmount: new FormControl('', [Validators.pattern('^[0-9]+(.[0-9]{1,10})?$')]),
      // segment: new FormControl(''),
      // permEdit: new FormControl(''),
      // permView: new FormControl(''),
      // donationId: new FormControl(''),
      // donationAmount: new FormControl(''),
      // amtLimit: new FormControl(''),
      // sbuTotalBudget: new FormControl(''),
      // ttlAllocate: new FormControl(''),
      // prevAllocate: new FormControl(''),
      // ttlPerson: new FormControl(''),
      // remaining: new FormControl(''),
      // donationAmt: new FormControl(''),
      // transLimit: new FormControl(''),

      deptId: ['', Validators.required],
      year: ['', Validators.required],
      authId: ['', Validators.required],
      sbu: ['', Validators.required],
      ttlAmount: ['', Validators.pattern('^[0-9]+(.[0-9]{1,10})?$')],
      segment: [''],
      permEdit: [''],
      permView: [''],
      donationId: [''],
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
    });
  }

  getDonation() {
    this.bgtService.getDonations().subscribe(response => {
      this.donations = response as IDonation[];
    }, error => {
      console.log(error);
    });
  }

  getAllocatedAmount() {

    if (this.bgtEmployee.value.segment == "" || this.bgtEmployee.value.segment == null) {
      this.toastr.error('Select Segmentation');
      return;
    }

    if (this.bgtEmployee.value.ttlAmount == "" || this.bgtEmployee.value.ttlAmount == null) {
      this.toastr.error('Enter Amount');
      return;
    }

    if (this.bgtEmployee.value.segment == "Monthly") {
      const d = new Date();

      var remMonth = 12 - d.getMonth() - 1;
      var ttlAloc = this.bgtEmployee.value.ttlPerson * this.bgtEmployee.value.ttlAmount * remMonth;

      this.bgtEmployee.patchValue({
        ttlAllocate: ttlAloc,
      });
    }
    else if (this.bgtEmployee.value.segment == "Yearly") {
      var ttlAloc = this.bgtEmployee.value.ttlPerson * this.bgtEmployee.value.ttlAmount;

      this.bgtEmployee.patchValue({
        ttlAllocate: ttlAloc,
      });
    }

    var rem = this.bgtEmployee.value.sbuTotalBudget - this.bgtEmployee.value.ttlAllocate - this.bgtEmployee.value.prevAllocate;
    this.bgtEmployee.patchValue({
      remaining: rem,
    });
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
    this.bgtEmployee.patchValue({
      remaining: "",
      segment: "",
      ttlAllocate: "",
    });
  }

  ngOnInit() {
    this.createbgtEmployeeForm();
    this.reset();
    this.getEmployeeId();

    this.getDonation();
    this.getSBU();
    this.getApprovalAuthority();
  }

  getEmployeeId() {
    this.empId = this.accountService.getEmployeeId();
    this.userRole = this.accountService.getUserRole();
  }

  insertbgtEmployee() {

    if (this.bgtEmployee.value.remaining < 0) {
      this.toastr.error('There is not enough budget left to be allocated');
      return;
    }

    if (this.bgtEmployee.value.deptId == "" || this.bgtEmployee.value.deptId == null) {
      this.toastr.error('Select Department');
      return;
    }
    if (this.bgtEmployee.value.sbu == "" || this.bgtEmployee.value.sbu == null) {
      this.toastr.error('Select SBU');
      return;
    }
    if (this.bgtEmployee.value.year == "" || this.bgtEmployee.value.year == null) {
      this.toastr.error('Enter Year');
      return;
    }
    if (this.bgtEmployee.value.ttlAmount == "" || this.bgtEmployee.value.ttlAmount == null) {
      this.toastr.error('Amount Can not be 0');
      return;
    }

    var yr = new Date(this.bgtEmployee.value.year);

    this.bgtService.bgtEmpFormData.deptId = this.bgtEmployee.value.deptId;
    this.bgtService.bgtEmpFormData.year = yr.getFullYear();
    this.bgtService.bgtEmpFormData.sbu = this.bgtEmployee.value.sbu;
    this.bgtService.bgtEmpFormData.authId = this.bgtEmployee.value.authId;
    this.bgtService.bgtEmpFormData.amount = this.bgtEmployee.value.ttlAmount;
    this.bgtService.bgtEmpFormData.segment = this.bgtEmployee.value.segment;
    this.bgtService.bgtEmpFormData.permEdit = this.bgtEmployee.value.permEdit;
    this.bgtService.bgtEmpFormData.permView = this.bgtEmployee.value.permView;
    this.bgtService.bgtEmpFormData.enteredBy = parseInt(this.empId);

    this.bgtService.insertBgtEmp(this.bgtService.bgtEmpFormData).subscribe(
      res => {
        this.toastr.success('Master Budget Data Saved successfully', 'Budget Dispatch')
      },
      err => { console.log(err); }
    );
  }

  generateData() {
    if (this.bgtEmployee.value.deptId == "" || this.bgtEmployee.value.deptId == null) {
      this.toastr.error('Select Department');
      return;
    }

    if (this.bgtEmployee.value.year == "" || this.bgtEmployee.value.year == null) {
      this.toastr.error('Select Year');
      return;
    }

    if (this.bgtEmployee.value.donationId == "" || this.bgtEmployee.value.donationId == null) {
      this.toastr.error('Select Donation Type');
      return;
    }

    if (this.bgtEmployee.value.authId == "" || this.bgtEmployee.value.authId == null) {
      this.toastr.error('Select Authorization Level');
      return;
    }

    if (this.bgtEmployee.value.deptId == 1 && this.bgtEmployee.value.authId == 8) {
      this.toastr.error('Wrong Department / Authorization Combination');
      return;
    }

    if (this.bgtEmployee.value.deptId == 2 && this.bgtEmployee.value.authId != 8) {
      this.toastr.error('Wrong Department / Authorization Combination');
      return;
    }

    this.donationTypeName = this.donations.filter(v => v.id == this.bgtEmployee.get('donationId').value)[0].donationTypeName;

    var yr = new Date(this.bgtEmployee.value.year);
    this.bgtService.getSBUWiseDonationLocation(this.bgtEmployee.value.donationId, this.bgtEmployee.value.deptId, yr.getFullYear(), this.bgtEmployee.value.authId).subscribe(
      res => {
        this.sbuData = res as ISbuData[];
        this.btnShow = false;
      },
      err => { console.log(err); }
    );
  }

  saveData() {
    if (this.sbuData.length == 0) {
      this.toastr.error('No Data to Save');
      return;
    }

    for (var i = 0; i < this.sbuData.length; i++) {
      if(this.sbuData[i].amount > 0 && this.sbuData[i].expense > this.sbuData[i].ttlAmt)
      {
            this.toastr.error('Insufficient allocation for SBU: '+ this.sbuData[i].sbu);
            return;
      }

      if(this.sbuData[i].ttlAmt == null)
      {
            this.toastr.error('Input is necessary for SBU: '+ this.sbuData[i].sbu);
            return;
      }

      if(this.sbuData[i].amount > 0 && (this.sbuData[i].limit == 0 || this.sbuData[i].limit == null))
      {
            this.toastr.error('Transaction limit needs to be set for SBU: '+ this.sbuData[i].sbu);
            return;
      }
    }

    for (var i = 0; i < this.sbuData.length; i++) {
      var yr = new Date(this.bgtEmployee.value.year);

      this.sbuData[i].deptId = this.bgtEmployee.value.deptId;
      this.sbuData[i].donationId = this.bgtEmployee.value.donationId;
      this.sbuData[i].segment = this.bgtEmployee.value.segment;
      this.sbuData[i].year = yr.getFullYear();
      this.sbuData[i].enteredBy = parseInt(this.empId);
    }

    this.bgtService.insertBgtOwnList(this.sbuData).subscribe(
      res => {
        this.toastr.success('Budget Distributed successfully', 'Budget Distribution')
        this.btnShow = false;
      },
      err => { console.log(err); }
    );
  }


  getTotal(selectedRow: ISbuData) {
    debugger;
    if (this.bgtEmployee.value.segment == "Monthly") {
      const d = new Date();
      var remMonth = 12 - d.getMonth();
      var ttl = selectedRow.totalPerson * selectedRow.amount * remMonth;

      var rem = selectedRow.sbuAmount + selectedRow.donationTypeAllocated - selectedRow.expense - selectedRow.totalAllocated - ttl;
      if (rem >= 0) {
        selectedRow.ttlAmt = ttl;
      }
      else {
        selectedRow.amount = 0;
        selectedRow.ttlAmt = 0;
        this.toastr.error('Insuficient Budget', 'Budget Distribution');
        return;
      }
    }
    else if (this.bgtEmployee.value.segment == "Yearly") {
      var ttl = selectedRow.totalPerson * selectedRow.amount;
      var rem = selectedRow.sbuAmount + selectedRow.donationTypeAllocated - selectedRow.expense - selectedRow.totalAllocated - ttl;

      if (rem >= 0) {
        selectedRow.ttlAmt = ttl;
      }
      else {
        selectedRow.amount = 0;
        selectedRow.ttlAmt = 0;
        this.toastr.error('Insuficient Budget', 'Budget Distribution');
        return;
      }
    }
  }

  resetSearch() {
    this.searchText = '';
  }


  openPendingListModal(template: TemplateRef<any>) {
    this.pendingListModalRef = this.modalService.show(template, this.config);
  }

  reset() {
    this.btnShow = true;
    this.bgtEmployee.setValue({
      deptId: "",
      year: "",
      authId: "",
      sbu: "",
      ttlAmount: "",
      segment: "",
      permEdit: "",
      permView: "",
      donationId: "",
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
    });

    this.sbuData = [];
  }
}

interface ISbuData {
  sbu: string;
  sbuAmount: number | null;
  expense: number | null;
  year: number;
  segment: string;
  deptId: string;
  approvalAuthorityName: string;
  donationId: number | null;
  donationTypeName: string;
  authId: number;
  totalPerson: number;
  totalLoc: number;
  amount: number;
  limit: number;
  ttlAmt: number;
  donationTypeAllocated: number;
  totalAllocated: number;
  enteredBy: number;
}

export class SbuData implements ISbuData {
  sbu: string;
  sbuAmount: number | null;
  expense: number | null;
  year: number;
  segment: string;
  deptId: string;
  approvalAuthorityName: string;
  donationId: number | null;
  donationTypeName: string;
  authId: number;
  totalPerson: number;
  totalLoc: number;
  amount: number;
  limit: number;
  ttlAmt: number;
  donationTypeAllocated: number;
  totalAllocated: number;
  enteredBy: number;
} 