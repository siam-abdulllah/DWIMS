import { rptInvestSummaryPagination, IrptInvestSummaryPagination } from '../shared/models/rptInvestSummaryPagination';
import { IrptInvestSummary, rptInvestSummary } from '../shared/models/rptInvestSummary';
import { Component, ElementRef, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { environment } from '../../environments/environment';
import 'jspdf-autotable';
import * as jsPDF from 'jspdf';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { IrptDepotLetter } from './../shared/models/rptInvestSummary';
import { ToastrService } from 'ngx-toastr';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from "ngx-spinner";
import { RptInvestSummaryService } from '../_services/report-investsummary.service';
import { GenericParams } from './../shared/models/genericParams';
import { AccountService } from '../account/account.service';
import { InvestmentInit } from '../shared/models/investmentRec';
import { DatePipe } from '@angular/common';


@Component({
  selector: 'rptInvestmentSummary',
  templateUrl: './rptInvestmentSummary.component.html',
  styles: [
  ]
})

export class RptInvestSummaryComponent implements OnInit {
  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  @ViewChild('fromDate') fromDate: ElementRef;
  @ViewChild('toDate') toDate: ElementRef;
  genParams: GenericParams;
  empId: string;
  searchText = '';
  configs: any;
  searchDto: IReportSearchDto;
  numberPattern = "^[0-9]+(.[0-9]{1,10})?$";
  totalCount = 0;
  reports: IrptInvestSummary[] = [];
  rptDepotLetter :IrptDepotLetter[] = [];
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
    public datepipe: DatePipe,
    public reportService: RptInvestSummaryService, private datePipe: DatePipe,
    private toastr: ToastrService, private modalService: BsModalService,
    private SpinnerService: NgxSpinnerService,private accountService: AccountService,) { }

  ngOnInit() {

    // var url_string = window.location.href
    // var url = new URL(url_string);
    // var v=url.pathname.split("/");

    this.resetForm();
    this.getEmployeeId();
    this.bsConfig = Object.assign({}, { containerClass: 'theme-blue' }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();
    this.configs = {
      currentPage: 1,
      itemsPerPage: 10,
      totalItems:50,
      };
      //this.GetData(v[3]);
  }


  // GetData(param)
  // {

  //   this.date=new Date();
  //   let latest_date =this.datepipe.transform(this.date, 'yyyy-MM-dd');

  //   this.reportService.rptInvestSummaryFormData.fromDate = new Date(new Date().getFullYear(), 0, 1);;
  //   this.reportService.rptInvestSummaryFormData.toDate = this.date;
  //   if(param== "Approved")
  //   {
  //     alert('Approved');
  //   }
  //   if(param== "Pending")
  //   {
  //     alert('Pending');
  //   }
  // }

  getEmployeeId() {
    this.empId = this.accountService.getEmployeeId();
    this.userRole = this.accountService.getUserRole();
  }
  dateCompare() {
    if (this.reportService.rptInvestSummaryFormData.fromDate != null && this.reportService.rptInvestSummaryFormData.toDate != null) {
      if (this.reportService.rptInvestSummaryFormData.toDate > this.reportService.rptInvestSummaryFormData.fromDate) {
        return true;
      }
      else {
        this.toastr.error('Select Appropriate Date Range', 'Error');
        return false;
      }
    }
  }

  ViewData() {
    const  searchDto: IReportSearchDto = {
      fromDate: this.reportService.rptInvestSummaryFormData.fromDate,
      toDate: this.reportService.rptInvestSummaryFormData.toDate,
      userRole:this.userRole,
      empId:this.empId
    };

    this.reportService.GetInvestmentSummaryReport(searchDto).subscribe(response => {
      const params = this.reportService.getGenParams();
      this.reports = response.data;
      this.totalCount = response.count;
      this.configs = {
        currentPage: params.pageIndex,
        itemsPerPage: params.pageSize,
        totalItems:this.totalCount,
        };
    }, error => {
      console.log(error);
    });
  }

  onPageChanged(event: any){
    const params = this.reportService.getGenParams();
    if (params.pageIndex !== event)
    {
      params.pageIndex = event;
      this.reportService.setGenParams(params);
      this.ViewData();
    }
  }

  getSummaryDetail(selectedRecord: InvestmentInit){
  debugger;
    // this.router.navigate(
    //   ['rptInvestmentDetail'],
    //   { queryParams: { id: selectedRecord.id } }
    // );
    //this.router.navigate(['./rptInvestmentDetail'], {relativeTo: this.router});
    //this.router.navigate( ['/','rptInvestmentDetail', selectedRecord.id]);
    this.router.navigate([]).then(result => {  window.open('/portal/rptInvestmentDetail/'+selectedRecord.id, '_blank'); });;
    

  }

  resetSearch(){
    this.searchText = '';
}

  resetPage(form: NgForm) {
    form.form.reset();
    this.configs = {
      currentPage: 1,
      itemsPerPage: 10,
      totalItems:50,
      };
  }

  resetForm() {
    this.configs = {
      currentPage: 1,
      itemsPerPage: 10,
      totalItems:50,
      };
  }


  ViewReport()
  {
    this.reportService.getRptDepotLetter(186).subscribe(resp => {
      // this.reportInvestmentService.getInsSocietyBCDSWiseInvestment().subscribe(resp => {  
      this.rptDepotLetter = resp as IrptDepotLetter[];
      debugger;
      if (this.rptDepotLetter.length <= 0) {
        this.toastr.warning('No Data Found', 'Report');
      }
      else
      {
        this.getReport(this.rptDepotLetter);
      }   
    }, error => {
      console.log(error);
    });
  }


  getReport(r: IrptDepotLetter[]) {
    const totalPagesExp = "{total_pages_count_string}";
    
    
    const pdf = new jsPDF('l', 'pt', 'a4');

    var pageHeight = pdf.internal.pageSize.height || pdf.internal.pageSize.getHeight();
    var pageWidth = pdf.internal.pageSize.width || pdf.internal.pageSize.getWidth();

    pdf.setTextColor(0, 0, 0);
    pdf.setFontSize(24);
    pdf.setFontType('bold');
    pdf.text('Square Pharmaceuticals Ltd.', pageWidth / 2, 50, {align: 'center'});
    pdf.setFontSize(14);
    pdf.setFontType('normal');
    pdf.text('Inter-department communication', pageWidth / 2, 75, {align: 'center'});
    pdf.setFontSize(12);
    //const pDate = this.datePipe.transform(new Date, "dd/MM/yyyy");
    pdf.text('From: Sales Department', 65, 100);
    pdf.text('Place: Dhaka', 680, 100);
      const pDate = this.datePipe.transform(r[0].setOn, "dd/MM/yyyy");
    pdf.text('Date: ' + pDate, 680, 120);
    pdf.text('To: '+ r[0].employeeName + ' (Id:' +r[0].empId+ ') '+ r[0].designationName + ' ' + r[0].marketName  , 65, 140);
    pdf.text('Ref.: ' + r[0].referenceNo, 680, 140);

    pdf.setLineWidth(0.5);    
    pdf.line(65, 150, 790, 150);  

    pdf.text('Subject:', 65, 190);
    pdf.setFontType('bold');
    pdf.text('Regarding Cash '+ r[0].donationTypeName, 110, 190);

    pdf.setFontType('normal');
    pdf.text('In response to above letter reference, we are pleased to approve ' + (r[0].proposedAmount).toLocaleString() + '/-  only as cash donation for Dr. '+ r[0].doctorName +',', 65, 240); 
    pdf.text('GP ID. '+ r[0].docId +' '+ r[0].address +'.', 65, 260 );

    pdf.text('You are therefore advised to Collect the amount in cash from DIC, '+ r[0].depotName +' by showing this reference letter & Arrange to hand over the' , 65, 300)
    pdf.text('money to the mentioned Doctor in prescence of RSD/DIC and respective Colleagues.' , 65, 320)

    pdf.text('We hope and believe that you will be able to keep good relationship with the mentioned Doctor by using this opportunity.' , 65, 360)

    pdf.text('With best wishes' , 85, 410)


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
    // pdf.autoTable(col, rowD,
    //   {
    //     theme: "grid",
    //     // table: { fillColor: 255, textColor: 0, fontStyle: 'normal', lineWidth: 0.1 },
    //     //head: { textColor: 0, fillColor: [211,211,211], fontStyle: 'bold', lineWidth: 0 },
    //     // body: {},
    //     // foot: { textColor: 255, fillColor: [26, 188, 156], fontStyle: 'bold', lineWidth: 0 },
    //     // alternateRow: {},
    //     headStyles: { fillColor: [192, 192, 192] },

    //     didDrawPage: pageContent,
    //     margin: { top: 110 },
    //     bodyStyles: { valign: 'middle', lineColor: [153, 153, 153] },
    //     styles: { overflow: 'linebreak', cellWidth: 'auto', fontSize: 9, textColor: 0 },
    //   });

    //for adding total number of pages // i.e 10 etc
    if (typeof pdf.putTotalPages === 'function') {
      pdf.putTotalPages(totalPagesExp);
    }

    // pdf.save(title + '.pdf');
    pdf.setProperties({
      title: "Donation_Confirmation_Letter.pdf"
    });

    var blob = pdf.output("blob");
    window.open(URL.createObjectURL(blob));
    //this.loading = false;
  }
}

interface IReportSearchDto {
  fromDate: Date | undefined | null;
  toDate: Date | undefined | null;
  userRole:string;
  empId:string;
}