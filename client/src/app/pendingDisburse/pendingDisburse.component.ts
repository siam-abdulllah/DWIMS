import { IMedDispSearch } from './../shared/models/medDispatch';
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
import { IInvestmentMedicineProd, InvestmentMedicineProd } from '../shared/models/investment';
import { IMedicineDispatchDtl, MedicineDispatchDtl } from '../shared/models/medDispatch';
import { IDepotInfo } from '../shared/models/depotInfo';
import { IDonation } from '../shared/models/donation';

@Component({
  selector: 'app-bcds-info',
  templateUrl: './pendingDisburse.component.html',
})
export class PendingDisburseComponent implements OnInit {

  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  @ViewChild('pendingListModal', { static: false }) pendingListModal: TemplateRef<any>;
  pendingListModalRef: BsModalRef;
  bsConfig: Partial<BsDatepickerConfig>;
  bsValue: Date = new Date();
  medDispatchForm: FormGroup;
  empId: string;
  emp: any;
  depots: IDepotInfo[];
  donations: IDonation[];
  genParams: GenericParams;
  numberPattern = "^[0-9]+(.[0-9]{1,10})?$";
  depotLetter :IrptDepotLetter[] = [];
  printTrack :IDepotPrintTrack[] = [];
  enableForm: boolean = true;
  investmentMedicineProds: MedicineDispatchDtl[];
  rptDepotLetter:any;
  searchText = '';
  config: any;
  totalCount = 0;
  userRole: any;
  constructor(public pendingService: MedDispatchService, private SpinnerService: NgxSpinnerService, private modalService: BsModalService, private accountService: AccountService, 
     private router: Router, private toastr: ToastrService, private datePipe: DatePipe,) {
   }



  // getPendingDispatch() {
  //   this.SpinnerService.show();
  //   //var empId = parseInt(this.empId);
  //   this.pendingService.mpoPendingPayment(rptMedDispSearchDto).subscribe(response => {
  //     this.SpinnerService.hide();
  //     this.rptDepotLetter = response;
  //     if(this.rptDepotLetter.length==0)
  //     {
  //       this.toastr.info('No Data Found');
  //     }
  //   }, error => {
  //     this.SpinnerService.hide();
  //     console.log(error);
  //   });
  // }

 
  ngOnInit() {
    this.resetPage();
    this.getEmployeeId();
  }

  getEmployeeId() {
    this.empId = this.accountService.getEmployeeId();
    this.userRole = this.accountService.getUserRole();

    var empId = parseInt(this.empId);
   
    this.pendingService.mpoPendingPayment(empId, this.userRole).subscribe(response => {
      this.SpinnerService.hide();
      this.rptDepotLetter = response;
      if(this.rptDepotLetter.length==0)
      {
        this.toastr.info('No Data Found');
      }
    }, error => {
      this.SpinnerService.hide();
      console.log(error);
    });
  }

  resetSearch(){
    this.searchText = '';
}
  reset() {
    this.investmentMedicineProds =[];
    this.medDispatchForm.setValue({
      fromDate: "",
      toDate: "",
      depotCode: "",
      donationId: "",
      disStatus: "",
    });
  }

  resetPage() {
  }
}