import { IDonation } from '../shared/models/donation';
import { Component, ElementRef, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { environment } from '../../environments/environment';
import 'jspdf-autotable';
import * as jsPDF from 'jspdf';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { IrptDepotLetterSearch } from '../shared/models/rptInvestSummary';
import { ToastrService } from 'ngx-toastr';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from "ngx-spinner";
import { RptInvestSummaryService } from '../_services/report-investsummary.service';
import { GenericParams } from '../shared/models/genericParams';
import { AccountService } from '../account/account.service';
import { DatePipe } from '@angular/common';
import { CalendarCellViewModel } from 'ngx-bootstrap/datepicker/models';
import { IApprovalAuthority } from '../shared/models/approvalAuthority';
import { ISBU } from '../shared/models/sbu';
import { BDCurrencyPipe } from '../bdNumberPipe';
import * as XLSX from 'xlsx';

@Component({
  selector: 'rptCampExpBgt',
  templateUrl: './rptCampExpBgt.component.html',
  styles: [
  ]
})

export class RptCampExpBgtComponent implements OnInit {
  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  genParams: GenericParams;
  empId: string;
  isInvestmentInActive: boolean;
  searchText = '';
  donations: IDonation[];
  approvalAuthorities: IApprovalAuthority[];
  sbu: ISBU[];
  //configs: any;
  searchDto: IReportSearchDto;
  numberPattern = "^[0-9]+(.[0-9]{1,10})?$";
  //totalCount = 0;
  reports: any;
  employeeId: any;
  approvalAuthorityId: any;
  donationId: any;
  fromDate: Date;
  sbuCode: any;
  rptDepotLetter :IrptDepotLetterSearch[] = [];
  bsConfig: Partial<BsDatepickerConfig>;
  bsValue: Date = new Date();
  baseUrl = environment.apiUrl;
  config = {
    keyboard: false,
    class: 'modal-lg',
    ignoreBackdropClick: true
  };
  userRole: any;
  date: Date;

  constructor(private router: Router,
    public BDCurrency: BDCurrencyPipe,
    public reportService: RptInvestSummaryService, private datePipe: DatePipe,
    private toastr: ToastrService, private modalService: BsModalService,
    private SpinnerService: NgxSpinnerService,private accountService: AccountService,) { }

  ngOnInit() {
    this.resetForm();
    this.bsConfig = Object.assign({}, { containerClass: 'theme-blue' }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();
    this.sbuCode = 0;
    this.getEmployeeId();
    this.ViewDataDoc();
  }


  getEmployeeId() {
    this.empId = this.accountService.getEmployeeId();
    this.userRole = this.accountService.getUserRole();
  }

  ViewDataDoc() {
    
    if (this.userRole == 'Administrator') {
      this.reportService.getCampExpBgt(0).subscribe(response => {
        debugger;
        this.reports = response;
      }, error => {
        console.log(error);
      });  

    }
    else {
      this.reportService.getCampExpBgt(Number(this.empId)).subscribe(response => {
        debugger;
        this.reports = response;
      }, error => {
        console.log(error);
      });  
    }
}

  onOpenCalendar(e) {
    e.setViewMode('year');
    e.monthSelectHandler = (event: CalendarCellViewModel): void => {
      e.value = event.date;
      return;
    };
}

exportexcel(): void {
  /* pass here the table id */
  let element = document.getElementById('excel-table');
  const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(element);

  /* generate workbook and add the worksheet */
  const wb: XLSX.WorkBook = XLSX.utils.book_new();
  XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');

  XLSX.writeFile(wb, 'Campaign_Exp_vs_Bgt.xlsx');
}


   resetSearch(){
    this.searchText = '';
}

  resetPage(form: NgForm) {
    form.form.reset();
  }

  resetForm() {
    this.isInvestmentInActive=false;
  }
}

interface IReportSearchDto {
  employeeId: number;
  sbu: string;
  donationId: number;
  year: Date;
}