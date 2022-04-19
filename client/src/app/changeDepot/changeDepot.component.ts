import { ChangeDepotService } from './../_services/changeDepot.service';
import { Component, ElementRef, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { environment } from '../../environments/environment';
import 'jspdf-autotable';
import { Router } from '@angular/router';
import { FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import { IrptDepotLetter } from '../shared/models/rptDepotLetter';
import { ToastrService } from 'ngx-toastr';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from "ngx-spinner";
import { GenericParams } from '../shared/models/genericParams';
import { AccountService } from '../account/account.service';
import { DatePipe } from '@angular/common';
import { IDepotPrintTrack } from '../shared/models/depotPrintTrack';
import { IDepotInfo } from '../shared/models/depotInfo';
import { IChangeDepot, IChangeDepotSearch } from '../shared/models/changeDepot';

@Component({
  selector: 'app-bcds-info',
  templateUrl: './changeDepot.component.html',
})
export class ChangeDepotComponent implements OnInit {

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
  depots: IDepotInfo[];
  rptDepotLetter:any;
  searchText = '';
  config: any;
  totalCount = 0;
  userRole: any;
  constructor(public pendingService: ChangeDepotService, private SpinnerService: NgxSpinnerService, private modalService: BsModalService, private accountService: AccountService,  private router: Router, private toastr: ToastrService) {
   }

   createBillTrackForm() {
    this.billTrackForm = new FormGroup({
      referenceNo: new FormControl('', [Validators.required]),
      oldDepotName: new FormControl('', [Validators.required]),
      oldDepotCode: new FormControl('', [Validators.required]),
      depotCode: new FormControl('', [Validators.required]),
      employeeName: new FormControl(''),
      doctorName: new FormControl(''),
      marketName: new FormControl(''),
      donationTypeName: new FormControl(''),
      proposedAmount: new FormControl(''),
      remarks: new FormControl(''),
      id: new FormControl(''),
      investmentInitId: new FormControl(''),
      searchText: new FormControl(''),
    });
  }

  getDepot() {
    this.pendingService.getDepot().subscribe(response => {
      this.depots = response as IDepotInfo[];
    }, error => {
      console.log(error);
    });
  }

  getDepotList() {
    this.SpinnerService.show();
    var empId = parseInt(this.empId);
    this.pendingService.getDepotList(empId,this.userRole).subscribe(response => {
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
    this.getDepot();
  }

  getEmployeeId() {
    this.empId = this.accountService.getEmployeeId();
    this.userRole = this.accountService.getUserRole();
  }

  insertChange() {

    if(this.billTrackForm.value.referenceNo == "" || this.billTrackForm.value.referenceNo == null)
    {
      this.toastr.error('Select a proposal First');
      return;
    }

    if(this.billTrackForm.value.depotCode == "" || this.billTrackForm.value.depotCode == null)
    {
      this.toastr.error('Enter Payment Reference No & Date');
      return;
    }

    this.pendingService.changeDepotFormData.investmentInitId = this.billTrackForm.value.investmentInitId;
    this.pendingService.changeDepotFormData.depotCode = this.billTrackForm.value.depotCode;
    this.pendingService.changeDepotFormData.oldDepotName = this.billTrackForm.value.oldDepotName;
    this.pendingService.changeDepotFormData.oldDepotCode = this.billTrackForm.value.oldDepotCode;
    this.pendingService.changeDepotFormData.employeeId = parseInt(this.empId);
    this.pendingService.changeDepotFormData.remarks = this.billTrackForm.value.remarks;

    this.pendingService.insertChange(this.pendingService.changeDepotFormData).subscribe(
      res => {
        debugger;
        this.toastr.success('Data Saved successfully', 'Change Depot')
      },
      err => { console.log(err); }
    );
  }

  ViewData(selectedRecord: IChangeDepotSearch)
  {
    this.billTrackForm.patchValue({
      id: selectedRecord.id,
      referenceNo: selectedRecord.referenceNo,
      employeeName: selectedRecord.employeeName,
      doctorName:  selectedRecord.doctorName,
      donationTypeName: selectedRecord.donationTypeName,
      marketName:  selectedRecord.marketName,
      proposedAmount:  selectedRecord.proposedAmount,
      investmentInitId:  selectedRecord.investmentInitId,
      oldDepotName: selectedRecord.depotName,
      oldDepotCode: selectedRecord.depotCode,

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
      oldDepotCode: "",
      oldDepotName: "",
      depotCode: "",
      employeeName: "",
      doctorName: "",
      marketName: "",
      donationTypeName: "",
      proposedAmount: "",
      remarks: "",
      id: "",
      searchText: "",
      investmentInitId: "",
    });
  }

  resetPage() {

  }
}



