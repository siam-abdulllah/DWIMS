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

@Component({
  selector: 'app-bcds-info',
  templateUrl: './pendingMedDispatch.component.html',
})
export class PendingMedDispatchComponent implements OnInit {

  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  @ViewChild('pendingListModal', { static: false }) pendingListModal: TemplateRef<any>;
  pendingListModalRef: BsModalRef;
  bsConfig: Partial<BsDatepickerConfig>;
  bsValue: Date = new Date();
  medDispatchForm: FormGroup;
  empId: string;
  genParams: GenericParams;
  numberPattern = "^[0-9]+(.[0-9]{1,10})?$";
  depotLetter :IrptDepotLetter[] = [];
  printTrack :IDepotPrintTrack[] = [];
  investmentMedicineProds: MedicineDispatchDtl[];
  rptDepotLetter:any;
  searchText = '';
  config: any;
  totalCount = 0;
  userRole: any;
  constructor(public pendingService: MedDispatchService, private SpinnerService: NgxSpinnerService, private modalService: BsModalService, private accountService: AccountService, 
     private router: Router, private toastr: ToastrService, private datePipe: DatePipe,) {
   }

   createMedDispatchForm() {
    this.medDispatchForm = new FormGroup({
      referenceNo: new FormControl('', [Validators.required]),
      issueReference: new FormControl('', [Validators.required]),
      issueDate: new FormControl('', [Validators.required]),
      employeeName: new FormControl(''),
      doctorName: new FormControl(''),
      marketName: new FormControl(''),
      donationTypeName: new FormControl(''),
      proposeAmt: new FormControl(''),
      dispatchAmt: new FormControl(''),
      id: new FormControl(''),
      investmentInitId: new FormControl(''),
      searchText: new FormControl(''),
      remarks: new FormControl(''),


      productName: new FormControl(''),
      productId: new FormControl(''),
      originVal:  new FormControl(''),
      originQty:new FormControl(''),
      dispVal:  new FormControl(''),
      dispQty:  new FormControl(''),
    });
  }

  getPendingDispatch() {
    this.SpinnerService.show();
    var empId = parseInt(this.empId);
    this.pendingService.getPendingDispatch(empId,this.userRole).subscribe(response => {
      this.SpinnerService.hide();
      this.rptDepotLetter = response;
    }, error => {
      this.SpinnerService.hide();
      console.log(error);
    });
  }

  getInvestmentMedicineProd(initId: number, aprBy: any) {
    this.pendingService.getInvestmentMedicineProds(initId).subscribe(response => {
      var data = response as IMedicineDispatchDtl[];
      if (data !== undefined && data.length>0) {
        this.investmentMedicineProds = data;
        let row: any[] = [];
        let rowD: any[] = [];
        let col = ['Medicine Name', 'Quantity [Box]', 'Amount'];
        for (const a of this.investmentMedicineProds) {
          //row.push('Product');
          row.push(a.productName);
          row.push(a.dispatchQuantity);
          row.push((a.dispatchTpVat).toLocaleString());
          rowD.push(row);
          row = [];
        }        
        this.getReport(col, rowD, this.depotLetter, aprBy);
      }
      else {
        this.investmentMedicineProds =[];
      }

      
    }, error => {
      console.log(error);
    });
  }

  ngOnInit() {
    this.resetPage();
    this.getEmployeeId();
    this.createMedDispatchForm();
    this.getPendingDispatch();
  }

  getEmployeeId() {
    this.empId = this.accountService.getEmployeeId();
    this.userRole = this.accountService.getUserRole();
  }

  resetSearch(){
    this.searchText = '';
}


  reset() {

    this.investmentMedicineProds =[];
    this.medDispatchForm.setValue({
      referenceNo: "",
      issueReference: "",
      issueDate: "",
      employeeName: "",
      doctorName: "",
      marketName: "",
      donationTypeName: "",
      proposeAmt: "",
      dispatchAmt: "",
      remarks: "",
      id: "",
      searchText: "",
      investmentInitId: "",
      
      productName:"",
      productId:"",
      originVal:  "",
      originQty:"",
      dispVal:  "",
      dispQty: "",
    });
  }

  ViewReport(selectedRecord: IMedDispSearch)
  {
    this.pendingService.getRptDepotLetter(selectedRecord.id).subscribe(resp => {
      // this.reportInvestmentService.getInsSocietyBCDSWiseInvestment().subscribe(resp => {  
      this.depotLetter = resp as IrptDepotLetter[];
      if (this.rptDepotLetter.length <= 0) {
        this.toastr.warning('No Data Found', 'Report');
      }
      else
      {
        //this.getReport(this.depotLetter);
        this.getInvestmentMedicineProd(selectedRecord.id, selectedRecord.approvedBy);
      }   
    }, error => {
      console.log(error);
    });
  }

  getReport(col: any[], rowD: any[], r: IrptDepotLetter[], aprby: any) {
    const totalPagesExp = "{total_pages_count_string}";
    const pdf = new jsPDF('l', 'pt', [842, 595]);

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
    pdf.text('To: '+ r[0].employeeName + ' (Id:' +r[0].empId+ ') '+ r[0].designationName + ' ' + r[0].marketName  , 65, 120);
    pdf.text('Ref.: ' + r[0].referenceNo, 680, 140);
    pdf.text('Approved By: '+ aprby,  65, 140);
    pdf.setLineWidth(0.5);    
    pdf.line(65, 150, 790, 150);  

    pdf.text('Subject:', 65, 190);
    pdf.setFontType('bold');
    pdf.text('Regarding Cash '+ r[0].donationTypeName, 110, 190);

    pdf.setFontType('normal');
    pdf.text('In response to above letter reference, we are pleased to approve ' + r[0].donationTypeName + ' as cash for below '+ r[0].donationTo+'.', 65, 220); 
    pdf.text('Name: '+r[0].doctorName +', GP ID. '+ r[0].docId +' '+ r[0].address +'.', 65, 240 );
    pdf.text('Amount: '+ (r[0].proposedAmount).toLocaleString() + '/-  ('+ this.transform(r[0].proposedAmount)+') only.', 65, 259 );
    pdf.text('You are therefore advised to Collect the amount in cash from DIC, '+ r[0].depotName +' by showing this reference letter & Arrange to hand over' , 65, 300)
    pdf.text('the money to the mentioned '+ r[0].donationTo+' in prescence of RSM/DIC and respective Colleagues.' , 65, 320)
 // initialization for headers
    let slNO = 0;

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
        margin: { top: 410 },
        bodyStyles: { valign: 'middle', lineColor: [153, 153, 153] },
        styles: { overflow: 'linebreak', cellWidth: 'auto', fontSize: 9, textColor: 0 },
      });

    pdf.text('We hope and believe that you will be able to keep good relationship with the mentioned '+ r[0].donationTo+' by using this opportunity.' , 65, 345)

    pdf.text('With best wishes' , 85, 380)


    var pageContent = function (data) {
      // HEADER

      // FOOTER
      var str = "Page " + data.pageCount;
      // Total page number plugin only available in jspdf v1.0+
      if (typeof pdf.putTotalPages === 'function') {
        str = str + " of " + totalPagesExp;
      }
      pdf.setFontSize(9);
      //var pageHeight = pdf.internal.pageSize.height || pdf.internal.pageSize.getHeight();
      var pageHeight = 874;
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


  resetPage() {

  }
}
