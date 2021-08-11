import { GenericParams } from './../shared/models/genericParams';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ReportInvestmentService } from '../_services/report-investment.service';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

import { FormGroup, FormControl, Validators } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';

import 'jspdf-autotable';
import * as jsPDF from 'jspdf';
import { DatePipe } from '@angular/common';


@Component({
  selector: 'app-report-investment',
  templateUrl: './report-investment.component.html',
  styleUrls: []
})
export class ReportInvestmentComponent implements OnInit {
  investmentSearchForm: FormGroup;
  bsConfig: Partial<BsDatepickerConfig>;
  bsValue: Date = new Date();
  investmentSearchDto: IInvestmentReportSearchDto;
  instSocDocInvestmentDto: IInstSocDocInvestmentDto[] = [];

  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  genParams: GenericParams;
  // approvalCeiling: IApprovalCeiling[];
  totalCount = 0;
  //constructor(public masterService: MasterService, private router: Router, private toastr: ToastrService) { }
  constructor(public reportInvestmentService: ReportInvestmentService, private router: Router, private toastr: ToastrService, private datePipe: DatePipe) { 

  }
  
  ngOnInit() {
    this.bsConfig = Object.assign({}, { containerClass: 'theme-blue' }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();
  }

  getDocSocInvestReport() {
    //this.loading = true;
    // tslint:disable-next-line: radix
    // const impId = parseInt(this.loginService.getEmpOrImpName());
    const investmentReportSearchDto: IInvestmentReportSearchDto = {
      //importerId: impId,
      fromDate: this.investmentSearchForm.value.fromDate,
      toDate: this.investmentSearchForm.value.toDate,
      SBU: "",
      userId: 0,
      location: "",
      donationType: "",
      name: "",
    };
    
    this.reportInvestmentService.getInsSocietyBCDSWiseInvestment(investmentReportSearchDto).subscribe(resp => {
      this.instSocDocInvestmentDto = resp as IInstSocDocInvestmentDto[];
      if (this.instSocDocInvestmentDto.length <= 0) {
        //this.alertify.warning('No Data Found');
      }
      if (this.instSocDocInvestmentDto.length) {
        for (let p of this.instSocDocInvestmentDto) {   
            var fD = this.datePipe.transform(p.fromDate, "dd/MM/yyyy")             
            var tD = this.datePipe.transform(p.toDate, "dd/MM/yyyy")
        }
      }

    }, error => {
      console.log(error);
    });
  }



  viewProformaSummaryReport() {
    if (this.instSocDocInvestmentDto.length <= 0) {
      //this.alertify.warning('No Data to Show Report');
      return false;
    }
    // const doc = new jsPDF();
    // doc.text("Hello there", 15, 15);
    // doc.save('first.pdf');
    let orgName;
    let orgAddress
    const r = this.instSocDocInvestmentDto as IInstSocDocInvestmentDto[];
  
    let row: any[] = [];
    let rowD: any[] = [];
    let col = ['Name', 'Location', 'Donation \nType',  'Invested \nAmount.', 'Duration', 'Commitment',
      'Actual \nShare', 'Competitor \nShare']; // initialization for headers
    // let col = ['SL NO.','Name OF Importer','Products','PI No.','PI Date','Manufacturer',
    // 'Exporter', 'Country Of Origin','Pack Size','Approval Amount MT','Approval Amount Unit', 'Status'];
    let title = "Proforma Invoice Summary Report"; // title of report
    let slNO = 0;
    for (const a of r) {
      console.log(r);
      //row.push(++slNO);
      row.push(a.name);
      row.push(a.location);
      row.push(a.donationType);
      //row.push(a.proformaDate);

      const convertedfDate = new Date(a.fromDate);
      let fd = '';
      fd += convertedfDate.getDate() + '/' + (convertedfDate.getMonth() + 1) + '/' + convertedfDate.getFullYear();

      const convertedtDate = new Date(a.toDate);
      let td = '';
      td += convertedtDate.getDate() + '/' + (convertedtDate.getMonth() + 1) + '/' + convertedtDate.getFullYear();
      row.push(fd+ ' - '+ td);

      row.push(a.commitment);
      row.push(a.actualShare);
      row.push(a.competitorShare);

      rowD.push(row);
      row = [];
    }
    //this.getReport(col, rowD, title, orgName, orgAddress);
    this.getReport(col, rowD, 'Institute Wise Investment Report', 'Square Pharmaceuticals Ltd.', '48, Square Center, Mohakhali');
  }

  getReport(col: any[], rowD: any[], title: any, orgName: any, orgAddress: any) {
    const totalPagesExp = "{total_pages_count_string}";
    const pdf = new jsPDF('l', 'pt', 'a4');
    pdf.setTextColor(0, 0, 0);
    pdf.setFontSize(11);
    pdf.setFontType('bold');
    pdf.text('Organization Name', 40, 60);
    pdf.setFontType('normal');
    pdf.text(': ' + orgName, 150, 60);
    pdf.setFontType('bold');
    pdf.text('Address', 40, 80);
    pdf.setFontType('normal');
    pdf.text(': ' + orgAddress, 150, 80);

    pdf.setFontType('bold');
    pdf.text('Report Name', 40, 100);
    pdf.setFontType('normal');
    pdf.text(': Proforma Invoice Report', 150, 100);
    const pDate = this.datePipe.transform(new Date, "dd/MM/yyyy");
    pdf.text('Printing Date: ' + pDate, 680, 100);
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

        styles: { overflow: 'linebreak', cellWidth: 'wrap', fontSize: 9, textColor: 0 },

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




interface IUserIdDto {
  userId: number;
}
interface IInvestmentReportSearchDto {
  userId: number;
  fromDate: Date | undefined | null;
  toDate: Date | undefined | null;
  SBU: string;
  location: string;
  donationType: string;
  name: string;
}


interface IInstSocDocInvestmentDto {
  id: number;
  location: string;
  donationType: string;
  name: string;
  investedAmount: number;
  commitment: number;
  actualShare: number;
  competitorShare: number;
  fromDate: Date | undefined | null;
  toDate: Date | undefined | null;
}