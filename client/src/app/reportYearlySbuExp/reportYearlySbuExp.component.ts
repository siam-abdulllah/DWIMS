
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
  selector: 'reportYearlySbuExp',
  templateUrl: './reportYearlySbuExp.component.html',
  styles: [
  ],
  providers: [DatePipe]
})
export class ReportYearlySbuExpComponent implements OnInit {
  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  @ViewChild('investmentInitSearchModal', { static: false }) investmentInitSearchModal: TemplateRef<any>;
  @ViewChild('submissionConfirmModal', { static: false }) submissionConfirmModal: TemplateRef<any>;

  convertedDate:string;
  empId: string;
  sbu: string;
  investmentSearchForm: FormGroup;
  investmentInitForm: NgForm;
  numberPattern = "^[0-9]+(.[0-9]{1,10})?$";
  yearlySbuExp :IYearlySBUWiseExpense[] = [];
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

   onOpenCalendar(e) {
      e.setViewMode('year');
      e.monthSelectHandler = (event: CalendarCellViewModel): void => {
        e.value = event.date;
        return;
      };
  }

/// ********************************************
/// Generate Yearly Budget And Expense
/// ********************************************

getYearlySBUWiseExpReport()  {
  const investmentReportSearchDto: IInvestmentReportSearchDto = {
    fromDate: this.investmentSearchForm.value.fromDate,
    toDate: this.investmentSearchForm.value.fromDate,
    sbu: null,
    donationType: null,
    userId: 0,
    investType: null,
    institutionId: null,
    societyId: null,
    bcdsId: null,
    doctorId: null,
    locationType: null,
    territoryCode: null,
    marketCode: null,
    regionCode: null,
    zoneCode: null,
    divisionCode: null,
    brandCode: null,
    campaignName: null,
    subCampaignName: null,
  };

  this.reportInvestmentService.GetSBUWiseExpenseReport(investmentReportSearchDto).subscribe(resp => {
    this.yearlySbuExp = resp as IYearlySBUWiseExpense[];
    debugger;
    if (this.yearlySbuExp.length <= 0) {
      this.toastr.warning('No Data Found', 'Report');
    }
  
    this.viewYearlySBUWiseExpReport();
  }, error => {
    console.log(error);
  });
}

/// ********************************************
/// Generate SBU Wise Yearly Expense
/// ********************************************

viewYearlySBUWiseExpReport() {

  if (this.yearlySbuExp.length <= 0) {
    return false;
  }

  const r =  this.yearlySbuExp as IYearlySBUWiseExpense[];

  let row: any[] = [];
  let rowD: any[] = [];
  let col = ['SBU', 'Yearly Budget' , 'Expense', 'Remaining', 'Usage (%)' ]; // initialization for headers

  debugger;
  for (const a of r) {
    row.push(a.sbuName);
    row.push((a.amount).toLocaleString());
    row.push((a.expense).toLocaleString());
    row.push((a.amount - a.expense).toLocaleString());
    row.push(Number(a.expense/a.amount * 100).toFixed(2));
    rowD.push(row);
    row = [];
  }
  //this.getReport(col, rowD, title, orgName, orgAddress);
  this.getReportLetter(col, rowD, 'Yearly SBU Wise Expense Report', 'Square Pharmaceuticals Ltd.', '48, Square Center, Mohakhali');
}

  getReportLetter(col: any[], rowD: any[], title: any, orgName: any, orgAddress: any) {
    const totalPagesExp = "{total_pages_count_string}";
    const pdf = new jsPDF('l', 'pt', [612, 800]);
    var pageWidth = pdf.internal.pageSize.width || pdf.internal.pageSize.getWidth();

    pdf.setTextColor(0, 0, 0);
    pdf.setFontSize(18);
    pdf.setFontType('bold');
    pdf.text( orgName,  pageWidth / 2, 50, {align: 'center'});
    pdf.setFontSize(12);
    pdf.setFontType('bold');
    pdf.text( orgAddress,  pageWidth / 2, 67, {align: 'center'});
    pdf.setFontSize(16);
    pdf.setFontType('bold');
    pdf.text( title ,  pageWidth / 2, 85, {align: 'center'});
    const pDate = this.datePipe.transform(new Date, "dd/MM/yyyy");
    pdf.setFontSize(11);
    pdf.setFontType('normal');
    pdf.text('Printing Date: ' + pDate, 60, 100);
    var pageContent = function (data) {
      // HEADER

      // FOOTER
      var str = "Page " + data.pageCount;
      // Total page number plugin only available in jspdf v1.0+
      if (typeof pdf.putTotalPages === 'function') {
        str = str + " of " + totalPagesExp;
      }
      pdf.setFontSize(9);
      var pageHeight = pdf.internal.pageSize.height || pdf.internal.pageSize.getHeight();
      pdf.text(str, data.settings.margin.left, pageHeight - 10); // showing current page number
    // pdf.text(title, 100, pageHeight - 10); 
    };
    pdf.autoTable(col, rowD,
      {
        theme: "grid",
        // table: { fillColor: 255, textColor: 0, fontStyle: 'normal', lineWidth: 0.1 },
        //head: { textColor: 0, fillColor: [211,211,211], fontStyle: 'bold', lineWidth: 0 },
        // body: {},
        // foot: { textColor: 255, fillColor: [26, 188, 156], fontStyle: 'bold', lineWidth: 0 },
        // alternateRow: {},
        headStyles: { fillColor: [192, 192, 192] },

        didDrawPage: pageContent,
        margin: { top: 110 },
        bodyStyles: { valign: 'middle', lineColor: [153, 153, 153] },
        styles: { overflow: 'linebreak', cellWidth: 'auto', fontSize: 9, textColor: 0 },
      });

    //for adding total number of pages // i.e 10 etc
    if (typeof pdf.putTotalPages === 'function') {
      pdf.putTotalPages(totalPagesExp);
    }

    // pdf.save(title + '.pdf');
    pdf.setProperties({
      title: title + ".pdf"
    });

    var blob = pdf.output("blob");
    window.open(URL.createObjectURL(blob));
    //this.loading = false;
  }
}

interface IInvestmentReportSearchDto {
  userId: number;
  fromDate: Date | undefined | null;
  toDate: Date | undefined | null;
  sbu: string;
  donationType: string;
  investType: string;
  institutionId: number | null;
  societyId: number | null;
  bcdsId: number | null;
  doctorId: number | null;
  locationType: string;
  territoryCode: string;
  marketCode: string;
  regionCode: string;
  zoneCode: string;
  divisionCode: string;
  brandCode: string,
  campaignName: string,
  subCampaignName: string,
}

export interface IYearlySBUWiseExpense {
  id: number;
  year : number; 
  sbuName  : string;
  amount : number;
  expense : number;
}

export class YearlySBUWiseExpense implements IYearlySBUWiseExpense {
  id: number = 0;
  year : number; 
  sbuName  : string;
  amount : number;
  expense : number;

}
