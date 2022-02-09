import { rptInvestSummaryPagination, IrptInvestSummaryPagination } from '../shared/models/rptInvestSummaryPagination';
import { IrptInvestSummary, rptInvestSummary } from '../shared/models/rptInvestSummary';
import { Component, ElementRef, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { environment } from '../../environments/environment';
import 'jspdf-autotable';
import * as jsPDF from 'jspdf';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { IrptDepotLetterSearch } from './../shared/models/rptInvestSummary';
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
  //configs: any;
  searchDto: IReportSearchDto;
  numberPattern = "^[0-9]+(.[0-9]{1,10})?$";
  //totalCount = 0;
  reports: IrptInvestSummary[] = [];
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
    // this.configs = {
    //   currentPage: 1,
    //   itemsPerPage: 10,
    //   totalItems:50,
    //   };
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
      debugger;
      this.reports = response.data;
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
    this.router.navigate([]).then(result => {  window.open('/portal/rptInvDtlSummary/'+selectedRecord.id, '_blank'); });;
    

  }

  resetSearch(){
    this.searchText = '';
}

  resetPage(form: NgForm) {
    form.form.reset();
  }

  resetForm() {
  }


  ViewReport()
  {
    this.reportService.getRptDepotLetter(185).subscribe(resp => {
      // this.reportInvestmentService.getInsSocietyBCDSWiseInvestment().subscribe(resp => {  
      this.rptDepotLetter = resp as IrptDepotLetterSearch[];
      debugger;
      if (this.rptDepotLetter.length <= 0) {
        this.toastr.warning('No Data Found', 'Report');
      }
      else
      {
        //this.getReport(this.rptDepotLetter);
      }   
    }, error => {
      console.log(error);
    });
  }

  transform(value: any): any {
    if (value) {
      value = parseFloat(value).toFixed(2);
      let amounth = value.toString().split(".");
      let price: any = amounth[0];
      let pointer: any = amounth.length > 0 ? amounth[1] : null;
      var singleDigit = ["Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine"],
        doubleDigit = ["Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen"],
        tensPlace = ["", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety"],
        handle_tens = function (digit: any, prevdigit: any) {
          return 0 == digit ? "" : " " + (1 == digit ? doubleDigit[prevdigit] : tensPlace[digit])
        },
        handle_utlc = function (digit: any, nextdigit: any, denom: any) {
          return (0 != digit && 1 != nextdigit ? " " + singleDigit[digit] : "") + (0 != nextdigit || digit > 0 ? " " + denom : "")
        };
      var rupees = "",
        digitIndex = 0,
        digit = 0,
        nextDigit = 0,
        words = [],
        paisaWords = [],
        paisa = "";
      if (price += "", isNaN(parseFloat(price))) rupees = "";
      else if (parseFloat(price) > 0 && price.length <= 10) {
        for (digitIndex = price.length - 1; digitIndex >= 0; digitIndex--)
          switch (digit = price[digitIndex] - 0, nextDigit = digitIndex > 0 ? price[digitIndex - 1] - 0 : 0, price.length - digitIndex - 1) {
            case 0:
              words.push(handle_utlc(digit, nextDigit, ""));
              break;
            case 1:
              words.push(handle_tens(digit, price[digitIndex + 1]));
              break;
            case 2:
              words.push(0 != digit ? " " + singleDigit[digit] + " Hundred" + (0 != price[digitIndex + 1] && 0 != price[digitIndex + 2] ? " and" : "") : "");
              break;
            case 3:
              words.push(handle_utlc(digit, nextDigit, "Thousand"));
              break;
            case 4:
              words.push(handle_tens(digit, price[digitIndex + 1]));
              break;
            case 5:
              words.push(handle_utlc(digit, nextDigit, "Lakh"));
              break;
            case 6:
              words.push(handle_tens(digit, price[digitIndex + 1]));
              break;
            case 7:
              words.push(handle_utlc(digit, nextDigit, "Crore"));
              break;
            case 8:
              words.push(handle_tens(digit, price[digitIndex + 1]));
              break;
            case 9:
              words.push(0 != digit ? " " + singleDigit[digit] + " Hundred" + (0 != price[digitIndex + 1] || 0 != price[digitIndex + 2] ? " and" : " Crore") : "")
          }
        rupees = words.reverse().join("")
      } else rupees = "";
      if (rupees)
        rupees = `${rupees} BDT `
      if (pointer != "00") {
        digitIndex = 0;
        digit = 0;
        nextDigit = 0;
        for (digitIndex = pointer.length - 1; digitIndex >= 0; digitIndex--)
          switch (digit = pointer[digitIndex] - 0, nextDigit = digitIndex > 0 ? pointer[digitIndex - 1] - 0 : 0, pointer.length - digitIndex - 1) {
            case 0:
              paisaWords.push(handle_utlc(digit, nextDigit, ""));
              break;
            case 1:
              paisaWords.push(handle_tens(digit, pointer[digitIndex + 1]));
              break;
          }
        paisa = paisaWords.reverse().join("");
        if (rupees)
          rupees = `${rupees} and ${paisa} Paisa`
        else
          rupees = `${paisa} Paisa`
      }
      return rupees
    }
  }


}

interface IReportSearchDto {
  fromDate: Date | undefined | null;
  toDate: Date | undefined | null;
  userRole:string;
  empId:string;
}