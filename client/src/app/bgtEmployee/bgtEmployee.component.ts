import { Component, ElementRef, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { environment } from '../../environments/environment';
import 'jspdf-autotable';
import * as jsPDF from 'jspdf';
import { Router } from '@angular/router';
import { FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import { IrptDepotLetter } from '../shared/models/rptDepotLetter';
import { IrptDepotLetterSearch, rptDepotLetterSearch } from '../shared/models/rptInvestSummary';
import { ToastrService } from 'ngx-toastr';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from "ngx-spinner";
import { GenericParams } from '../shared/models/genericParams';
import { AccountService } from '../account/account.service';
import { DatePipe } from '@angular/common';
import { BgtEmployeeService } from '../_services/bgtEmployee.service';
import { DepotPrintTrack,IDepotPrintTrack } from '../shared/models/depotPrintTrack';
import { IInvestmentMedicineProd, InvestmentMedicineProd } from '../shared/models/investment';
import { IMedicineDispatchDtl, MedicineDispatchDtl, IMedDispSearch } from '../shared/models/medDispatch';
import { IApprovalAuthority } from '../shared/models/approvalAuthority';
import { ISBU } from '../shared/models/sbu';

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
      amount: new FormControl(''),
      segment: new FormControl(''),
      permEdit: new FormControl(''),
      permView: new FormControl(''),
      donationId: new FormControl(''),
      donationAmount: new FormControl(''),
      amtLimit: new FormControl(''),
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
  }

  getEmployeeId() {
    this.empId = this.accountService.getEmployeeId();
    this.userRole = this.accountService.getUserRole();
  }

  insertbgtEmployee() {

alert(this.bgtEmployee.value.permEdit);

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
    if(this.bgtEmployee.value.amount == "" || this.bgtEmployee.value.amount == null)
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

  // ViewData(selectedRecord: IMedDispSearch)
  // {
  //   this.medDispatchForm.patchValue({
  //     payRefNo: selectedRecord.payRefNo,
  //     referenceNo: selectedRecord.referenceNo,
  //     employeeName: selectedRecord.employeeName,
  //     doctorName:  selectedRecord.doctorName,
  //     donationTypeName: selectedRecord.donationTypeName,
  //     marketName:  selectedRecord.marketName,
  //     proposeAmt:  selectedRecord.proposedAmount,
  //     investmentInitId:  selectedRecord.investmentInitId,
  //     approvedBy:  selectedRecord.approvedBy,
  //     approvedDate:  this.formatDate(selectedRecord.approvedDate),
  //     depotName: selectedRecord.depotName,
  //     // formControlName2: myValue2 (can be omitted)
  //   });
  //   this.isValid = true;
  //   this.isHide = false;
    
  //   this.getInvestmentMedicineProd();
  //   this.pendingListModalRef.hide();
  // }


// updateData()
// {
//   debugger;
//   if(this.medDispatchForm.value.dispQty == null || this.medDispatchForm.value.dispQty == 0)
//   {
//     this.toastr.warning('Invalid Quantity', 'Medicine Dispatch');
//     return;
//   }
//   if(this.medDispatchForm.value.dispQty > this.medDispatchForm.value.originQty)
//   {
//     this.toastr.warning('Dispatch Quantity Can not be greater than Initial Quantity', 'Medicine Dispatch');
//     return;
//   }

//   this.medDispatchForm.value.dispatchTpVat = (this.medDispatchForm.value.originVal / this.medDispatchForm.value.originQty * this.medDispatchForm.value.dispQty);

//   let data = new MedicineDispatchDtl();
//   data.id = 0;
//   data.investmentInitId = this.medDispatchForm.value.investmentInitId;
//   data.employeeId = parseInt(this.empId);
//   data.productId = this.medDispatchForm.value.productId;
//   data.productName = this.medDispatchForm.value.productName;
//   data.boxQuantity = this.medDispatchForm.value.originQty;
//   data.tpVat = this.medDispatchForm.value.originVal;
//   data.dispatchQuantity = this.medDispatchForm.value.dispQty;
//   data.dispatchTpVat = (this.medDispatchForm.value.originVal / this.medDispatchForm.value.originQty * this.medDispatchForm.value.dispQty);
//   this.investmentMedicineProds.push(data);

  
//   if ( this.investmentMedicineProds.length>0) {
//     let sum=0;
//     for (let i = 0; i < this.investmentMedicineProds.length; i++) {
//       sum=sum+this.investmentMedicineProds[i].dispatchTpVat;
//     }
//     this.medDispatchForm.patchValue({
//       dispatchAmt: sum.toLocaleString(),
//     });
//   }
//   else {
//     this.medDispatchForm.patchValue({
//       dispatchAmt: '0',
//     });
//     this.investmentMedicineProds =[];
//   }

//   this.valShow = true;
//   this.isHide = false;
// }

  // removeInvestmentMedicineProd(selectedRecord: IMedicineDispatchDtl) {
  //   var c = confirm("Are you sure you want to delete that?");
  //   if (c == true) {
  //       const index = this.investmentMedicineProds.indexOf(selectedRecord);
  //       this.investmentMedicineProds.splice(index, 1);

  //       if ( this.investmentMedicineProds.length>0) {
  //         let sum=0;
  //         for (let i = 0; i < this.investmentMedicineProds.length; i++) {
  //           sum=sum+this.investmentMedicineProds[i].dispatchTpVat;
  //         }
  
  //         this.medDispatchForm.patchValue({
  //           dispatchAmt: sum.toLocaleString(),
  //         });
  
  //       }
  //       else {
  //         this.medDispatchForm.patchValue({
  //           dispatchAmt: '0',
  //         });
  //         this.investmentMedicineProds =[];
  //       }
  //   }
// }

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
      amount: "",
      segment: "",
      permEdit: "",
      permView: "",
      donationId: "",
      donationAmount: "",
      amtLimit: "",
    });
  }

}
