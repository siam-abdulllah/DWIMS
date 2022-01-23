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
import { MedDispatchService } from '../_services/medDispatch.service';
import { DepotPrintTrack,IDepotPrintTrack } from '../shared/models/depotPrintTrack';
import { IInvestmentMedicineProd, InvestmentMedicineProd } from '../shared/models/investmentRec';

@Component({
  selector: 'app-bcds-info',
  templateUrl: './medDispatch.component.html',
})
export class MedDispatchComponent implements OnInit {

  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  @ViewChild('pendingListModal', { static: false }) pendingListModal: TemplateRef<any>;
  pendingListModalRef: BsModalRef;
  bsConfig: Partial<BsDatepickerConfig>;
  bsValue: Date = new Date();
  medDispatchForm: FormGroup;
  empId: string;
  genParams: GenericParams;
  numberPattern = "^[0-9]+(.[0-9]{1,10})?$";
  depotLetter :IrptDepotLetter[] = [];
  printTrack :IDepotPrintTrack[] = [];
  investmentMedicineProds: IInvestmentMedicineProd[];
  rptDepotLetter:any;
  searchText = '';
  config: any;
  totalCount = 0;
  userRole: any;
  constructor(public pendingService: MedDispatchService, private SpinnerService: NgxSpinnerService, private modalService: BsModalService, private accountService: AccountService,  private router: Router, private toastr: ToastrService) {
   }

   createMedDispatchForm() {
    this.medDispatchForm = new FormGroup({
      referenceNo: new FormControl('', [Validators.required]),
      issueReference: new FormControl('', [Validators.required]),
      issueDate: new FormControl('', [Validators.required]),
      employeeName: new FormControl(''),
      doctorName: new FormControl(''),
      marketName: new FormControl(''),
      donationTypeName: new FormControl(''),
      proposeAmt: new FormControl(''),
      dispatchAmt: new FormControl(''),
      id: new FormControl(''),
      investmentInitId: new FormControl(''),
      searchText: new FormControl(''),
      remarks: new FormControl(''),
    });
  }

  getPendingDispatch() {
    this.SpinnerService.show();
    var empId = parseInt(this.empId);
    this.pendingService.getPendingDispatch(empId,this.userRole).subscribe(response => {
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

  getInvestmentMedicineProd() {
    this.pendingService.getInvestmentMedicineProds(this.medDispatchForm.value.investmentInitId).subscribe(response => {
      var data = response as IInvestmentMedicineProd[];
      debugger;
      if (data !== undefined && data.length>0) {
        this.investmentMedicineProds = data;
        let sum=0;
        for (let i = 0; i < this.investmentMedicineProds.length; i++) {
          sum=sum+this.investmentMedicineProds[i].tpVat;
        }

        this.medDispatchForm.patchValue({
          dispatchAmt: sum.toString(),
        });

      }
      else {
        this.medDispatchForm.patchValue({
          dispatchAmt: '0',
        });
        this.investmentMedicineProds =[];
      }
    }, error => {
      console.log(error);
    });
  }

  ngOnInit() {
    this.resetPage();
    this.getEmployeeId();
    this.createMedDispatchForm();
  }

  getEmployeeId() {
    this.empId = this.accountService.getEmployeeId();
    this.userRole = this.accountService.getUserRole();
  }

  insertTracker() {

    if(this.medDispatchForm.value.referenceNo == "" || this.medDispatchForm.value.referenceNo == null )
    {
      this.toastr.error('Select a proposal First');
      return;
    }

    if(this.medDispatchForm.value.issueReference == "" || this.medDispatchForm.value.issueReference == null || this.medDispatchForm.value.issueDate == "" || this.medDispatchForm.value.issueDate == null )
    {
      this.toastr.error('Enter Payment Reference No & Date');
      return;
    }

    if(this.medDispatchForm.value.dispatchAmt == "" || this.medDispatchForm.value.dispatchAmt == null || this.medDispatchForm.value.remarks == "" || this.medDispatchForm.value.remarks == null )
    {
      this.toastr.error('Enter Payment Dispatch Amount & Remarks');
      return;
    }

    this.pendingService.medDispatchFormData.investmentInitId = this.medDispatchForm.value.investmentInitId;
    this.pendingService.medDispatchFormData.issueReference = this.medDispatchForm.value.issueReference;
    this.pendingService.medDispatchFormData.issueDate = this.medDispatchForm.value.issueDate;
    this.pendingService.medDispatchFormData.depotName = "";
    this.pendingService.medDispatchFormData.depotCode = "";
    this.pendingService.medDispatchFormData.employeeId = parseInt(this.empId);
    this.pendingService.medDispatchFormData.remarks = this.medDispatchForm.value.remarks;
    this.pendingService.medDispatchFormData.dispatchAmt = this.medDispatchForm.value.dispatchAmt;
    this.pendingService.medDispatchFormData.proposeAmt = this.medDispatchForm.value.proposeAmt;


    this.pendingService.insertDispatch(this.pendingService.medDispatchFormData).subscribe(
      res => {
        debugger;
        this.toastr.success('Data Saved successfully', 'Report Tracker')
      },
      err => { console.log(err); }
    );
  }



  ViewData(selectedRecord: IrptDepotLetterSearch)
  {
    this.medDispatchForm.patchValue({
      referenceNo: selectedRecord.referenceNo,
      employeeName: selectedRecord.employeeName,
      doctorName:  selectedRecord.doctorName,
      donationTypeName: selectedRecord.donationTypeName,
      marketName:  selectedRecord.marketName,
      proposeAmt:  selectedRecord.proposedAmount,
      investmentInitId:  selectedRecord.id,
      // formControlName2: myValue2 (can be omitted)
    });
    this.getInvestmentMedicineProd();
    this.pendingListModalRef.hide();
  }


  removeInvestmentMedicineProd(selectedRecord: IInvestmentMedicineProd) {
    var c = confirm("Are you sure you want to delete that?");
    if (c == true) {
      this.pendingService.investmentMedicineProdFormData = Object.assign({}, selectedRecord);
      this.SpinnerService.show();
      debugger;
      this.pendingService.removeInvestmentMedicineProd().subscribe(
        res => {
          //this.isDonationValid=false;
          this.pendingService.investmentMedicineProdFormData = new InvestmentMedicineProd();
          this.getInvestmentMedicineProd();
          this.SpinnerService.hide();

          this.toastr.success(res);
        },
        err => {
          this.SpinnerService.hide();
          console.log(err);
        }
      );
    }
  }

  resetSearch(){
    this.searchText = '';
}

openPendingListModal(template: TemplateRef<any>) {
  this.pendingListModalRef = this.modalService.show(template, this.config);
}

  reset() {
    // this.searchText = '';
    // form.reset();
    // this.config = {
    //   currentPage: 1,
    //   itemsPerPage: 10,
    //   totalItems:50,
    //   };
    this.investmentMedicineProds =[];

  this.medDispatchForm.setValue({
      referenceNo: "",
      issueReference: "",
      issueDate: "",
      employeeName: "",
      doctorName: "",
      marketName: "",
      donationTypeName: "",
      proposeAmt: "",
      dispatchAmt: "",
      remarks: "",
      id: "",
      searchText: "",
      investmentInitId: "",
    });
  }

  resetPage() {

  }
}
