import { Component, ElementRef, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { environment } from '../../environments/environment';
import 'jspdf-autotable';
import * as jsPDF from 'jspdf';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { IrptDepotLetter } from '../shared/models/rptDepotLetter';
import { IrptDepotLetterSearch, rptDepotLetterSearch } from '../shared/models/rptInvestSummary';
import { ToastrService } from 'ngx-toastr';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from "ngx-spinner";
import { GenericParams } from '../shared/models/genericParams';
import { AccountService } from '../account/account.service';
import { DatePipe } from '@angular/common';
import { DepotPendingService } from '../_services/depotPending.service';
import { DepotPrintTrack,IDepotPrintTrack } from '../shared/models/depotPrintTrack';

@Component({
  selector: 'pendingPrintDepot',
  templateUrl: './pendingPrintDepot.component.html',
  styles: [
  ]
})

export class PendingPrintDepotComponent implements OnInit {
  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  @ViewChild('fromDate') fromDate: ElementRef;
  @ViewChild('toDate') toDate: ElementRef;
  genParams: GenericParams;
  empId: string;
  searchText = '';
  configs: any;
  numberPattern = "^[0-9]+(.[0-9]{1,10})?$";
  totalCount = 0;
  depotLetter :IrptDepotLetter[] = [];
  printTrack :IDepotPrintTrack[] = [];
  rptDepotLetter:any;
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
    public pendingService: DepotPendingService, private datePipe: DatePipe,
    private toastr: ToastrService, private modalService: BsModalService,
    private SpinnerService: NgxSpinnerService,private accountService: AccountService,) { }

  ngOnInit() {
    this.resetForm();
    this.getEmployeeId();
    this.bsConfig = Object.assign({}, { containerClass: 'theme-blue' }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();
    this.configs = {
      currentPage: 1,
      itemsPerPage: 10,
      totalItems:50,
      };
  }
  resetSearch(){
    this.searchText = '';
}
  getEmployeeId() {
    this.empId = this.accountService.getEmployeeId();
    this.userRole = this.accountService.getUserRole();
    this.ViewData();
  }

  ViewData() {

    var empId = parseInt(this.empId);
    this.pendingService.getPendingReport(empId,this.userRole).subscribe(response => {
      this.rptDepotLetter = response;
    }, error => {
      console.log(error);
    });
  }

  onPageChanged(event: any){
    const params = this.pendingService.getGenParams();
    if (params.pageIndex !== event)
    {
      params.pageIndex = event;
      this.pendingService.setGenParams(params);
      this.ViewData();
    }
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


  ViewReport(selectedRecord: IrptDepotLetterSearch)
  {
    this.pendingService.getRptDepotLetter(selectedRecord.id).subscribe(resp => {
      // this.reportInvestmentService.getInsSocietyBCDSWiseInvestment().subscribe(resp => {  
      this.depotLetter = resp as IrptDepotLetter[];
 
      if (this.rptDepotLetter.length <= 0) {
        this.toastr.warning('No Data Found', 'Report');
      }
      else
      {
        this.insertTracker(this.depotLetter);
      }   
    }, error => {
      console.log(error);
    });
  }


  insertTracker(r: IrptDepotLetter[]) {
    this.pendingService.depotPrintFormData.investmentInitId = r[0].id;
    this.pendingService.depotPrintFormData.depotName = r[0].depotName;
    this.pendingService.depotPrintFormData.depotId = "";
    this.pendingService.depotPrintFormData.employeeId = r[0].empId;
    this.pendingService.depotPrintFormData.remarks = "";
    this.pendingService.depotPrintFormData.printCount = 1;

    this.pendingService.insertTrackReport(this.pendingService.depotPrintFormData).subscribe(
      res => {
        debugger;
        this.toastr.success('Data Saved successfully', 'Report Tracker')
        this.getReport(this.depotLetter);
      },
      err => { console.log(err); }
    );
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
    pdf.text('In response to above letter reference, we are pleased to approve ' + r[0].donationTypeName + ' as cash for below '+ r[0].donationTo+'.', 65, 240); 
    pdf.text('Name: '+r[0].doctorName +', GP ID. '+ r[0].docId +' '+ r[0].address +'.', 65, 260 );
    pdf.text('Amount: '+ (r[0].proposedAmount).toLocaleString() + '/-  ('+ this.transform(r[0].proposedAmount)+') only.', 65, 279 );


    pdf.text('You are therefore advised to Collect the amount in cash from DIC, '+ r[0].depotName +' by showing this reference letter & Arrange to hand over' , 65, 320)
    pdf.text('the money to the mentioned '+ r[0].donationTo+' in prescence of RSD/DIC and respective Colleagues.' , 65, 340)

    pdf.text('We hope and believe that you will be able to keep good relationship with the mentioned '+ r[0].donationTo+' by using this opportunity.' , 65, 380)

    pdf.text('With best wishes' , 85, 430)


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

    if (typeof pdf.putTotalPages === 'function') {
      pdf.putTotalPages(totalPagesExp);
    }

    // pdf.save(title + '.pdf');
    pdf.setProperties({
      title: "Donation_Confirmation_Letter_"+ r[0].referenceNo
    });

    var blob = pdf.output("blob");
    window.open(URL.createObjectURL(blob));
    //this.loading = false;
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