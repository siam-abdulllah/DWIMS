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
import { BgtOwnService } from '../_services/bgtOwn.service';
import { IApprovalAuthority } from '../shared/models/approvalAuthority';
import { ISBU } from '../shared/models/sbu';
import { DATE } from 'ngx-bootstrap/chronos/units/constants';
import { IEmployeeInfo } from '../shared/models/employeeInfo';

@Component({
  selector: 'bgtOwn',
  templateUrl: './bgtOwn.component.html',
})
export class BgtOwnComponent implements OnInit {

  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  @ViewChild('pendingListModal', { static: false }) pendingListModal: TemplateRef<any>;
  pendingListModalRef: BsModalRef;
  bsConfig: Partial<BsDatepickerConfig>;
  bsValue: Date = new Date();
  bgtOwn: FormGroup;
  empId: string;
  genParams: GenericParams;
  approvalAuthorities: IApprovalAuthority[];
  SBUs: ISBU[];
  employees: IEmployeeInfo[];
  numberPattern = "^[0-9]+(.[0-9]{1,10})?$";
  isValid: boolean = true;
  valShow: boolean = true;
  isHide: boolean = false;
  searchText = '';
  config: any;
  totalCount = 0;
  userRole: any;
  constructor(public bgtService: BgtOwnService, private SpinnerService: NgxSpinnerService, private modalService: BsModalService, private accountService: AccountService, 
     private router: Router, private toastr: ToastrService, private datePipe: DatePipe,) {
   }
   customSearchFnEmp(term: string, item: any) {
    term = term.toLocaleLowerCase();
    return item.employeeSAPCode.toLocaleLowerCase().indexOf(term) > -1 ||
      item.employeeName.toLocaleLowerCase().indexOf(term) > -1;
  }
  onChangeEmployee() {
    
  }
  onChangeSBU() {
    this.bgtService.getSbuWiseEmp(this.bgtOwn.value.sbu).subscribe(response => {    
      // this.bgtOwn.patchValue({
      //   sbuTotalBudget: response[0].count,
      // });
      
     }, error => {
        console.log(error);
     });
  }
   createbgtOwnForm() {
    this.bgtOwn = new FormGroup({
      deptId: new FormControl('', [Validators.required]),
      year: new FormControl('', [Validators.required]),
      authId: new FormControl('', [Validators.required]),
      sbu: new FormControl('1021', [Validators.required]),
      totalAmount: new FormControl(''),
      totalExpense: new FormControl(''),
      totalPipeline: new FormControl(''),
      segment: new FormControl(''),
      permEdit: new FormControl(''),
      permView: new FormControl(''),
      donationId: new FormControl(''),
      donationAmount: new FormControl(''),
      amtLimit: new FormControl(''),
      prevAllocate: new FormControl(''),
      remaining: new FormControl(''),
      employee: new FormControl(9999),
    });
  }

  getDeptSbuWiseBudgetAmt()
  {
    if(this.bgtOwn.value.deptId == "" || this.bgtOwn.value.deptId == null)
    {
      this.toastr.error('Select Department');
      this.bgtOwn.patchValue({
        year: "",
      });
      return;
    }
    if(this.bgtOwn.value.sbu == "" || this.bgtOwn.value.sbu == null)
    {
      this.toastr.error('Select SBU');
      this.bgtOwn.patchValue({
        year: "",
      });
      return;
    }

    var yr = new Date(this.bgtOwn.value.year);
    yr.getFullYear();

    this.bgtService.getDeptSbuWiseBudgetAmt(this.bgtOwn.value.deptId, this.bgtOwn.value.sbu, yr.getFullYear()).subscribe(response => {    
      // this.bgtOwn.patchValue({
      //   sbuTotalBudget: response[0].count,
      // });
      
     }, error => {
        console.log(error);
     });
  }

  getAuthPersonCount()
  {
    if(this.bgtOwn.value.authId == "" || this.bgtOwn.value.authId == null)
    {
      this.toastr.error('Select Authorization Level');
      return;
    }

    this.bgtService.getAuthPersonCount(this.bgtOwn.value.authId).subscribe(response => {    
      // this.bgtOwn.patchValue({
      //   ttlPerson: response[0].count,
      // });
      
     }, error => {
        console.log(error);
     });
  }


  getAllocatedAmount()
  {

    if(this.bgtOwn.value.segment == "" || this.bgtOwn.value.segment == null)
    {
      this.toastr.error('Select Segmentation');
      return;
    }

    if(this.bgtOwn.value.segment == "Monthly")
    {
      const d = new Date();
      
      var remMonth = 12 - d.getMonth() - 1;
      // var ttlAloc = this.bgtOwn.value.ttlPerson * this.bgtOwn.value.ttlAmount * remMonth;

      // this.bgtOwn.patchValue({
      //   ttlAllocate: ttlAloc,
      // });
    }
    else if(this.bgtOwn.value.segment == "Yearly")
    {
      // var ttlAloc = this.bgtOwn.value.ttlPerson * this.bgtOwn.value.ttlAmount;

      // this.bgtOwn.patchValue({
      //   ttlAllocate: ttlAloc,
      // });
    }
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
    this.createbgtOwnForm();
    this.reset();
    this.getEmployeeId();
    
    this.getSBU();
    this.getApprovalAuthority();
  }

  getEmployeeId() {
    this.empId = this.accountService.getEmployeeId();
    this.userRole = this.accountService.getUserRole();
  }

  insertbgtOwn() {

alert(this.bgtOwn.value.permEdit);

    if(this.bgtOwn.value.deptId == "" || this.bgtOwn.value.deptId == null)
    {
      this.toastr.error('Select Department');
      return;
    }
    if(this.bgtOwn.value.sbu == "" || this.bgtOwn.value.sbu == null)
    {
      this.toastr.error('Select SBU');
      return;
    }
    if(this.bgtOwn.value.year == "" || this.bgtOwn.value.year == null)
    {
      this.toastr.error('Enter Year');
      return;
    }
    if(this.bgtOwn.value.amount == "" || this.bgtOwn.value.amount == null)
    {
      this.toastr.error('Amount Can not be 0');
      return;
    }


    // this.pendingService.medDispatchFormData.investmentInitId = this.medDispatchForm.value.investmentInitId;
    // this.pendingService.medDispatchFormData.issueReference = this.medDispatchForm.value.issueReference;
    // this.pendingService.medDispatchFormData.issueDate = this.medDispatchForm.value.issueDate;
    // this.pendingService.medDispatchFormData.sapRefNo = this.medDispatchForm.value.issueReference;
    // this.pendingService.medDispatchFormData.payRefNo = this.medDispatchForm.value.payRefNo;
    // this.pendingService.medDispatchFormData.depotName = "";
    // this.pendingService.medDispatchFormData.depotCode = "";
    // this.pendingService.medDispatchFormData.employeeId = parseInt(this.empId);
    // this.pendingService.medDispatchFormData.remarks = this.medDispatchForm.value.remarks;
    // this.pendingService.medDispatchFormData.dispatchAmt = this.medDispatchForm.value.dispatchAmt;
    // this.pendingService.medDispatchFormData.proposeAmt = this.medDispatchForm.value.proposeAmt;


    // this.pendingService.insertDispatch(this.pendingService.medDispatchFormData).subscribe(
    //   res => {
    //     this.SaveMedicineDetail();
    //     this.toastr.success('Data Saved successfully', 'Medicine Dispatch') 
    //     this.isValid = false;
    //   },
    //   err => { console.log(err); }
    // );
  }

  // modifyData(selectedRecord: IMedicineDispatchDtl)
  // {
  //   this.valShow = false;
  //   this.isHide = true;
  //   this.medDispatchForm.patchValue({
  //     productName: selectedRecord.productName,
  //     productId: selectedRecord.productId,
  //     originVal:  selectedRecord.tpVat,
  //     originQty: selectedRecord.boxQuantity,
  //     dispVal:  selectedRecord.dispatchTpVat,
  //     dispQty:  selectedRecord.dispatchQuantity,
  //     // formControlName2: myValue2 (can be omitted)
  //   });

  //   const index = this.investmentMedicineProds.indexOf(selectedRecord);
  //   this.investmentMedicineProds.splice(index, 1);
  // }

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
    this.bgtOwn.setValue({
      deptId: "",
      year: "",
      authId: "",
      sbu: "1021",
      totalAmount: "",
      totalExpense: "",
      totalPipeline: "",
      segment: "",
      permEdit:"",
      permView: "",
      donationId: "",
      donationAmount: "",
      amtLimit: "",
      prevAllocate: "",
      remaining: "",
      employee: 9999,
    });
  }

}
