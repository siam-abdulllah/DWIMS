
import 'jspdf-autotable';
import * as jsPDF from 'jspdf';
//import { GenericParams } from '../shared/models/genericParams';
import { Component, ElementRef, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { DatePipe } from '@angular/common';
import { NgxSpinnerService } from 'ngx-spinner';
import { ReportInvestmentService } from '../_services/report-investment.service';
import { CalendarCellViewModel } from 'ngx-bootstrap/datepicker/models';

@Component({
  selector: 'reportSystemSummary',
  templateUrl: './reportSystemSummary.component.html',
  styles: [
  ],
  providers: [DatePipe]
})
export class ReportSystemSummaryComponent implements OnInit {
  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  @ViewChild('investmentInitSearchModal', { static: false }) investmentInitSearchModal: TemplateRef<any>;
  @ViewChild('submissionConfirmModal', { static: false }) submissionConfirmModal: TemplateRef<any>;

  convertedDate:string;
  empId: string;
  sbu: string;
  investmentSearchForm: FormGroup;
  investmentInitForm: NgForm;
  numberPattern = "^[0-9]+(.[0-9]{1,10})?$";
  rptSysSum :any;
  donationToVal: string;
  userRole: any;
  totalCount = 0;
  bsConfig: Partial<BsDatepickerConfig>;
  bsValue: Date = new Date();
  today = new Date();
  dd = String(this.today.getDate()).padStart(2, '0');
  mm = String(this.today.getMonth() + 1).padStart(2, '0'); //January is 0!
  yyyy = this.today.getFullYear();
  todayDate = this.dd + this.mm + this.yyyy;
  config = {
    keyboard: false,
    class: 'modal-lg',
    ignoreBackdropClick: true
  };

  constructor(public reportInvestmentService: ReportInvestmentService, private router: Router, private toastr: ToastrService, private datePipe: DatePipe, private SpinnerService: NgxSpinnerService) { }
  
  ngOnInit() {
      this.createInvestmentSearchForm();
      this.getSystemSummaryData();
    this.bsConfig = Object.assign({}, { containerClass: 'theme-blue' }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();
  }

  createInvestmentSearchForm() {
    this.investmentSearchForm = new FormGroup({
      fromDate: new FormControl('', [Validators.required]),
      donationType: new FormControl(''),
      sbu: new FormControl(''),
    });
  }

getSystemSummaryData()  {
   this.reportInvestmentService.getSystemSummary().subscribe(resp => {
    this.rptSysSum = resp;
  }, error => {
    console.log(error);
  });
}

}