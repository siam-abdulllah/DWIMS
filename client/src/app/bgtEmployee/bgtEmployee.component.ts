import { Component, ElementRef, OnInit, ViewChild, TemplateRef } from '@angular/core';
import 'jspdf-autotable';
import * as jsPDF from 'jspdf';
import { Router } from '@angular/router';
import { FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from "ngx-spinner";
import { GenericParams } from '../shared/models/genericParams';
import { AccountService } from '../account/account.service';
import { DatePipe, getLocaleDateTimeFormat } from '@angular/common';
import { BgtEmployeeService } from '../_services/bgtEmployee.service';
import { IApprovalAuthority } from '../shared/models/approvalAuthority';
import { ISBU } from '../shared/models/sbu';
import { DATE } from 'ngx-bootstrap/chronos/units/constants';

@Component({
  selector: 'bgtEmployee',
  templateUrl: './bgtEmployee.component.html',
})
export class BgtEmployeeComponent implements OnInit {

  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  @ViewChild('pendingListModal', { static: false }) pendingListModal: TemplateRef<any>;
  pendingListModalRef: BsModalRef;
  bsConfig: Partial<BsDatepickerConfig>;
  bsValue: Date = new Date();
  bgtEmployee: FormGroup;
  empId: string;
  genParams: GenericParams;
  approvalAuthorities: IApprovalAuthority[];
  SBUs: ISBU[];
  numberPattern = "^[0-9]+(.[0-9]{1,10})?$";
  isValid: boolean = true;
  valShow: boolean = true;
  isHide: boolean = false;
  searchText = '';
  config: any;
  totalCount = 0;
  userRole: any;
  constructor(public bgtService: BgtEmployeeService, private SpinnerService: NgxSpinnerService, private modalService: BsModalService, private accountService: AccountService, 
     private router: Router, private toastr: ToastrService, private datePipe: DatePipe,) {
   }

   createbgtEmployeeForm() {
    this.bgtEmployee = new FormGroup({
      deptId: new FormControl('', [Validators.required]),
      year: new FormControl('', [Validators.required]),
      authId: new FormControl('', [Validators.required]),
      sbu: new FormControl('', [Validators.required]),
      ttlAmount: new FormControl(''),
      segment: new FormControl(''),
      permEdit: new FormControl(''),
      permView: new FormControl(''),
      donationId: new FormControl(''),
      donationAmount: new FormControl(''),
      amtLimit: new FormControl(''),
      sbuTotalBudget: new FormControl(''),
      ttlAllocate: new FormControl(''),
      prevAllocate: new FormControl(''),
      ttlPerson: new FormControl(''),
      remaining: new FormControl(''),
    });
  }

  getDeptSbuWiseBudgetAmt()
  {
    if(this.bgtEmployee.value.deptId == "" || this.bgtEmployee.value.deptId == null)
    {
      this.toastr.error('Select Department');
      this.bgtEmployee.patchValue({
        year: "",
      });
      return;
    }

    if(this.bgtEmployee.value.sbu == "" || this.bgtEmployee.value.sbu == null)
    {
      this.toastr.error('Select SBU');
      this.bgtEmployee.patchValue({
        year: "",
      });
      return;
    }

    var yr = new Date(this.bgtEmployee.value.year);
    yr.getFullYear();

    this.bgtService.getDeptSbuWiseBudgetAmt(this.bgtEmployee.value.deptId, this.bgtEmployee.value.sbu, yr.getFullYear()).subscribe(response => {
        this.bgtEmployee.patchValue({
          sbuTotalBudget: response[0].count,
        });    
     }, error => {
        console.log(error);
     });


     this.bgtService.getPrevAllocate(this.bgtEmployee.value.deptId, this.bgtEmployee.value.sbu, yr.getFullYear()).subscribe(response => {    
      this.bgtEmployee.patchValue({
        prevAllocate: response[0].count,
      });
      
     }, error => {
        console.log(error);
     });

  }

  getAuthPersonCount()
  {
    if(this.bgtEmployee.value.authId == "" || this.bgtEmployee.value.authId == null)
    {
      this.toastr.error('Select Authorization Level');
      return;
    }

    this.bgtService.getAuthPersonCount(this.bgtEmployee.value.authId, this.bgtEmployee.value.sbu).subscribe(response => {    
      this.bgtEmployee.patchValue({
        ttlPerson: response[0].count,
      });
      
     }, error => {
        console.log(error);
     });
  }


  getAllocatedAmount()
  {

    if(this.bgtEmployee.value.segment == "" || this.bgtEmployee.value.segment == null)
    {
      this.toastr.error('Select Segmentation');
      return;
    }

    if(this.bgtEmployee.value.segment == "Monthly")
    {
      const d = new Date();
      
      var remMonth = 12 - d.getMonth() - 1;
      var ttlAloc = this.bgtEmployee.value.ttlPerson * this.bgtEmployee.value.ttlAmount * remMonth;

      this.bgtEmployee.patchValue({
        ttlAllocate: ttlAloc,
      });
    }
    else if(this.bgtEmployee.value.segment == "Yearly")
    {
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

  getApprovalAuthority(){
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


  ngOnInit() {
    this.createbgtEmployeeForm();
    this.reset();
    this.getEmployeeId();
    
    this.getSBU();
    this.getApprovalAuthority();

    this.bgtEmployee.value.permEdit = true;
    this.bgtEmployee.value.permView = true;
  }

  getEmployeeId() {
    this.empId = this.accountService.getEmployeeId();
    this.userRole = this.accountService.getUserRole();
  }

  insertbgtEmployee() {

    if(this.bgtEmployee.value.remaining < 0)
    {
      this.toastr.error('There is not enough budget left to be allocated');
      return;
    }

    if(this.bgtEmployee.value.deptId == "" || this.bgtEmployee.value.deptId == null)
    {
      this.toastr.error('Select Department');
      return;
    }
    if(this.bgtEmployee.value.sbu == "" || this.bgtEmployee.value.sbu == null)
    {
      this.toastr.error('Select SBU');
      return;
    }
    if(this.bgtEmployee.value.year == "" || this.bgtEmployee.value.year == null)
    {
      this.toastr.error('Enter Year');
      return;
    }
    if(this.bgtEmployee.value.ttlAmount == "" || this.bgtEmployee.value.ttlAmount == null)
    {
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
        this.isValid = false;
      },
      err => { console.log(err); }
    );
  }

  private formatDate(date) {
    const d = new Date(date);
    let month = '' + (d.getMonth() + 1);
    let day = '' + d.getDate();
    const year = d.getFullYear();
    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;
    return [day, month, year ].join('-');
  }

  resetSearch(){
    this.searchText = '';
}

openPendingListModal(template: TemplateRef<any>) {
  this.pendingListModalRef = this.modalService.show(template, this.config);
}

  reset() {
    this.isValid = true;
    this.valShow = true;
    this.isHide = false;
    this.bgtEmployee.setValue({
      deptId: "",
      year: "",
      authId: "",
      sbu: "",
      ttlAmount: "",
      segment: "",
      permEdit:"",
      permView: "",
      donationId: "",
      donationAmount: "",
      amtLimit: "",
      sbuTotalBudget: "",
      ttlAllocate: "",
      prevAllocate: "",
      ttlPerson: "",
      remaining: "",
    });
  }

}
