import { rptInvestSummaryPagination, IrptInvestSummaryPagination } from '../shared/models/rptInvestSummaryPagination';
import { IrptInvestSummary, rptInvestSummary } from '../shared/models/rptInvestSummary';
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
import { InvestmentInit } from '../shared/models/investmentRec';
import { DatePipe } from '@angular/common';
import { IDoctor } from '../shared/models/docotor';


@Component({
  selector: 'rptInvSummarySingle',
  templateUrl: './rptInvSummarySingle.component.html',
  styles: [
  ]
})

export class RptInvSummarySingleComponent implements OnInit {
  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  @ViewChild('fromDate') fromDate: ElementRef;
  @ViewChild('toDate') toDate: ElementRef;
  genParams: GenericParams;
  empId: string;
  isInvestmentInActive: boolean;
  searchText = '';
  doctors: IDoctor[];
  //configs: any;
  searchDto: IReportSearchDto;
  numberPattern = "^[0-9]+(.[0-9]{1,10})?$";
  //totalCount = 0;
  reports: any;
  referenceNoDoc: string;
  doctorId: any;
  doctorName: any;
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
    this.resetForm();
    this.getEmployeeId();
    this.bsConfig = Object.assign({}, { containerClass: 'theme-blue' }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();
  }

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
    this.reportService.IsInvestmentInActive(this.reportService.rptInvestSummaryFormData.referenceNo).subscribe(response => {
      debugger;
      if(response[0].count==0)
      {
        this.isInvestmentInActive=true;
      }
      else{
        this.isInvestmentInActive=false;
      this.reportService.GetInvestmentSummarySingle(this.reportService.rptInvestSummaryFormData.referenceNo).subscribe(response => {
        debugger;
        this.reports = response;
      }, error => {
        console.log(error);
      });
      }
    }, error => {
      console.log(error);
    });
  
    
  }
  ViewDataDoc() {
     debugger;
    // this.reportService.IsInvestmentInActiveDoc(this.referenceNoDoc,this.doctorId,this.doctorName).subscribe(response => {
    //   debugger;
      // if(1==0)
      // {
      //   this.isInvestmentInActive=true;
      // }
      // else{
        //this.isInvestmentInActive=false;
        if((this.referenceNoDoc==undefined || this.referenceNoDoc=="") && (this.doctorId==undefined || this.doctorId=="") && (this.doctorName==undefined || this.doctorName==""))
        {
          this.toastr.warning('Please enter at least 1 parameter!');
         return false;
        }
        
        if(this.referenceNoDoc!=undefined && this.referenceNoDoc!="")
        {
          if(this.referenceNoDoc.length!=11)
          {
            this.toastr.warning('Please enter must be 11 character in Reference No ');
            return false;
          }
        }
        if(this.doctorName!=undefined && this.doctorName!="")
        {
          if(this.doctorName.length<4)
          {
            this.toastr.warning('Please enter minimum 4 character in Doctor Name! ');
            return false;
          }
        }
      this.reportService.GetInvestmentSummarySingleDoc(this.referenceNoDoc,this.doctorId,this.doctorName).subscribe(response => {
        debugger;
        this.reports = response;
      }, error => {
        console.log(error);
      });
     // }
    // }, error => {
    //   console.log(error);
    // });
  
    
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
    this.isInvestmentInActive=false;
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
  customSearchFnDoc(term: string, item: any) {
    term = term.toLocaleLowerCase();
    return item.doctorCode.toLocaleLowerCase().indexOf(term) > -1 ||
      item.doctorName.toLocaleLowerCase().indexOf(term) > -1;
  }
  // getDoctor() {
  //   this.reportService.getDoctors().subscribe(response => {
  //     this.doctors = response as IDoctor[];
  //   }, error => {
  //     this.SpinnerService.hide();
  //     console.log(error);
  //   });
  // }


}

interface IReportSearchDto {
  referenceNo: string;
}