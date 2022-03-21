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
import { RptDocLocService } from '../_services/report-DocLoc.service';


@Component({
  selector: 'rptDocLocMap',
  templateUrl: './rptDocLocMap.component.html',
  styles: [
  ]
})

export class RptDocLocMapComponent implements OnInit {
  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  genParams: GenericParams;
  empId: string;
  isInvestmentInActive: boolean;
  searchText = '';
  numberPattern = "^[0-9]+(.[0-9]{1,10})?$";
  //totalCount = 0;
  reports: any;
  marketCode: string;
  marketName: string;
  doctorCode: any;
  doctorName: string;
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
    public reportService: RptDocLocService, private datePipe: DatePipe,
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
 
  ViewDataDoc() {
     debugger;

        if((this.doctorCode==undefined || this.doctorCode=="") && (this.marketCode==undefined || this.marketCode=="") && (this.doctorName==undefined || this.doctorName=="") && (this.marketName==undefined || this.marketName==""))
        {
          this.toastr.warning('Please enter at least 1 parameter!');
         return false;
        }
      
        if(this.doctorName!=undefined && this.doctorName!="")
        {
          if(this.doctorName.length<4)
          {
            this.toastr.warning('Please enter minimum 4 character in Doctor Name! ');
            return false;
          }
        }

        if(this.marketName!=undefined && this.marketName!="")
        {
          if(this.marketName.length<4)
          {
            this.toastr.warning('Please enter minimum 4 character in Market Name! ');
            return false;
          }
        }

      this.reportService.GetDocLocMapInd(this.doctorName,this.doctorCode,this.marketName, this.marketCode).subscribe(response => {
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


}

