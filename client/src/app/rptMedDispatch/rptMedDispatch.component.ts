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
  templateUrl: './rptMedDispatch.component.html',
})
export class RptMedDispatchComponent implements OnInit {

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

   getDepot() {
    this.pendingService.getDepot().subscribe(response => {
      this.depots = response as IDepotInfo[];
    }, error => {
      console.log(error);
    });
  }

  getDonation() {
    this.pendingService.getDonations().subscribe(response => {
      this.donations = response as IDonation[];
    }, error => {
      console.log(error);
    });
  }

   createMedDispatchForm() {
    this.medDispatchForm = new FormGroup({
      fromDate: new FormControl('', [Validators.required]),
      toDate: new FormControl('', [Validators.required]),
      depotCode: new FormControl('', [Validators.required]),
      donationId: new FormControl(''),
      disStatus: new FormControl(''),
    });
  }

  getPendingDispatch() {

    if(this.medDispatchForm.value.fromDate == "" || this.medDispatchForm.value.fromDate == null  || this.medDispatchForm.value.toDate == "" || this.medDispatchForm.value.toDate == null)
    {
      this.toastr.error('Select Date Range');
      return;
    }

    if(this.medDispatchForm.value.depotCode == "" || this.medDispatchForm.value.depotCode == null )
    {
      this.toastr.error('Select a Depot');
      return;
    }
    if(this.medDispatchForm.value.donationId == "" || this.medDispatchForm.value.donationId == null )
    {
      this.toastr.error('Select a Donation Type');
      return;
    }
  
    if(this.userRole == "SIC" || this.userRole == "DIC")
    {
      var dptCode = this.emp.depotCode.padStart(4,"0")
    }
    else
    {
      var dptCode = this.medDispatchForm.value.depotCode.padStart(4,"0");
    }

    const rptMedDispSearchDto: IRptMedDispSearchDto = {
      fromDate: this.medDispatchForm.value.fromDate,
      toDate: this.medDispatchForm.value.toDate,
      depotCode: dptCode,
      donationId: this.medDispatchForm.value.donationId,
      disStatus: this.medDispatchForm.value.disStatus,
    };

    this.SpinnerService.show();
    //var empId = parseInt(this.empId);
    this.pendingService.getRptMedDis(rptMedDispSearchDto).subscribe(response => {
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

 
  ngOnInit() {
    this.resetPage();
    this.getEmployeeId();
    this.createMedDispatchForm();
    this.getDepot();
    this.getDonation();

  }

  getEmployeeId() {
    this.empId = this.accountService.getEmployeeId();
    this.userRole = this.accountService.getUserRole();
    this.pendingService.getEmpDepot(parseInt(this.empId)).subscribe(response => {
      this.emp = response;
      if (this.userRole == 'DIC' || this.userRole == 'SIC') {
        //this.medDispatchForm.get('depotCode').disable();
        this.enableForm = true;
        this.medDispatchForm.patchValue({
          depotCode: this.emp.depotCode,
        });
      }
      else
      {
        //this.medDispatchForm.get('depotCode').enable();
        this.enableForm = false;
      }
      debugger;
    }, error => {
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


interface IRptMedDispSearchDto {
  fromDate: Date | undefined | null;
  toDate: Date | undefined | null;
  disStatus: string;
  donationId: string;
  depotCode: string;
}
