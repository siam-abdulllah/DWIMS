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
import { DepotPendingService } from '../_services/depotPending.service';
import { DepotPrintTrack,IDepotPrintTrack } from '../shared/models/depotPrintTrack';

@Component({
  selector: 'app-bcds-info',
  templateUrl: './chequeTrack.component.html',
})
export class ChequeTrackComponent implements OnInit {

  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  @ViewChild('pendingListModal', { static: false }) pendingListModal: TemplateRef<any>;
  pendingListModalRef: BsModalRef;
  bsConfig: Partial<BsDatepickerConfig>;
  bsValue: Date = new Date();
  billTrackForm: FormGroup;
  empId: string;
  genParams: GenericParams;
  numberPattern = "^[0-9]+(.[0-9]{1,10})?$";
  depotLetter :IrptDepotLetter[] = [];
  printTrack :IDepotPrintTrack[] = [];
  rptDepotLetter:any;
  searchText = '';
  config: any;
  totalCount = 0;
  userRole: any;
  constructor(public pendingService: DepotPendingService, private SpinnerService: NgxSpinnerService, private modalService: BsModalService, private accountService: AccountService,  private router: Router, private toastr: ToastrService) {
   }

   createBillTrackForm() {
    this.billTrackForm = new FormGroup({
      referenceNo: new FormControl('', [Validators.required]),
      paymentRefNo: new FormControl('', [Validators.required]),
      payRefNo: new FormControl('', [Validators.required]),
      paymentDate: new FormControl('', [Validators.required]),
      employeeName: new FormControl(''),
      doctorName: new FormControl(''),
      marketName: new FormControl(''),
      chequeNo: new FormControl(''),
      bankName: new FormControl(''),
      donationTypeName: new FormControl(''),
      proposedAmount: new FormControl(''),
      id: new FormControl(''),
      investmentInitId: new FormControl(''),
      searchText: new FormControl(''),
    });
  }

  getPendingList() {
    this.SpinnerService.show();
    var empId = parseInt(this.empId);
    this.pendingService.getPendingChequeReport(empId).subscribe(response => {
      this.SpinnerService.hide();
      this.rptDepotLetter = response;
      if (this.rptDepotLetter.length > 0) {
        this.openPendingListModal(this.pendingListModal);
      }
      else {
        this.toastr.warning('No Data Found');
      }
    }, error => {
      this.SpinnerService.hide();
      console.log(error);
    });
  }

  ngOnInit() {
    this.resetPage();
    this.getEmployeeId();
    this.createBillTrackForm();
  }

  getEmployeeId() {
    this.empId = this.accountService.getEmployeeId();
    this.userRole = this.accountService.getUserRole();
  }

  insertTracker() {

    if(this.billTrackForm.value.payRefNo == "" || this.billTrackForm.value.payRefNo == null)
    {
      this.toastr.error('Select Investment First');
      return;
    }

    if(this.billTrackForm.value.paymentRefNo == "" || this.billTrackForm.value.paymentRefNo == null || this.billTrackForm.value.paymentDate == "" || this.billTrackForm.value.paymentDate == null )
    {
      this.toastr.error('Enter Payment Reference No & Date');
      return;
    }

    this.pendingService.depotPrintFormData.investmentInitId = this.billTrackForm.value.investmentInitId;
    this.pendingService.depotPrintFormData.paymentRefNo = this.billTrackForm.value.paymentRefNo;
    this.pendingService.depotPrintFormData.payRefNo = this.billTrackForm.value.payRefNo;
    this.pendingService.depotPrintFormData.sapRefNo = this.billTrackForm.value.paymentRefNo;
    this.pendingService.depotPrintFormData.paymentDate = this.billTrackForm.value.paymentDate;
    this.pendingService.depotPrintFormData.depotName = "";
    this.pendingService.depotPrintFormData.depotId = "";
    this.pendingService.depotPrintFormData.employeeId = parseInt(this.empId);
    this.pendingService.depotPrintFormData.remarks = "";
    this.pendingService.depotPrintFormData.printCount = 1;
    this.pendingService.depotPrintFormData.chequeNo = this.billTrackForm.value.chequeNo;
    this.pendingService.depotPrintFormData.bankName = this.billTrackForm.value.bankName;

    this.pendingService.insertTrackReport(this.pendingService.depotPrintFormData).subscribe(
      res => {
        this.toastr.success('Data Saved successfully', 'Report Tracker')
      },
      err => { this.toastr.error('Data Already Exists', 'Error') }
    );
  }

  ViewData(selectedRecord: IrptDepotLetterSearch)
  {
    this.billTrackForm.patchValue({
      payRefNo: selectedRecord.payRefNo,
      referenceNo: selectedRecord.referenceNo,
      employeeName: selectedRecord.employeeName,
      doctorName:  selectedRecord.doctorName,
      donationTypeName: selectedRecord.donationTypeName,
      marketName:  selectedRecord.marketName,
      proposedAmount:  selectedRecord.proposedAmount,
      investmentInitId:  selectedRecord.investmentInitId,
      // formControlName2: myValue2 (can be omitted)
    });
    this.pendingListModalRef.hide();
  }

  resetSearch(){
    this.searchText = '';
}

openPendingListModal(template: TemplateRef<any>) {
  this.pendingListModalRef = this.modalService.show(template, this.config);
}

  reset() {

  this.billTrackForm.setValue({
      referenceNo: "",
      paymentRefNo: "",
      payRefNo: "",
      paymentDate: "",
      employeeName: "",
      doctorName: "",
      marketName: "",
      donationTypeName: "",
      proposedAmount: "",
      id: "",
      chequeNo: "",
      bankName: "",
      searchText: "",
      investmentInitId: "",
    });
  }

  resetPage() {

  }
}
