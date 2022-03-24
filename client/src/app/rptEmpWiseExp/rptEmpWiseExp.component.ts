import { IDonation } from './../shared/models/donation';
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
import { IDoctor } from '../shared/models/docotor';
import { CalendarCellViewModel } from 'ngx-bootstrap/datepicker/models';
import { IApprovalAuthority } from '../shared/models/approvalAuthority';


@Component({
  selector: 'rptEmpWiseExp',
  templateUrl: './rptEmpWiseExp.component.html',
  styles: [
  ]
})

export class RptEmpWiseExpComponent implements OnInit {
  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  genParams: GenericParams;
  empId: string;
  isInvestmentInActive: boolean;
  searchText = '';
  donations: IDonation[];
  approvalAuthorities: IApprovalAuthority[];
  //configs: any;
  searchDto: IReportSearchDto;
  numberPattern = "^[0-9]+(.[0-9]{1,10})?$";
  //totalCount = 0;
  reports: any;
  employeeId: any;
  approvalAuthorityId: any;
  donationId: any;
  fromDate: Date;
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
    this.getDonation();
    this.getApprovalAuthority();
    this.bsConfig = Object.assign({}, { containerClass: 'theme-blue' }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();
  }

  getApprovalAuthority(){
    this.reportService.getApprovalAuthority().subscribe(response => {
      this.approvalAuthorities = response as IApprovalAuthority[];
     }, error => {
        console.log(error);
     });
  }

  getDonation() {
    this.reportService.getDonations().subscribe(response => {
      this.donations = response as IDonation[];
    }, error => {
      console.log(error);
    });
  }

  getEmployeeId() {
    this.empId = this.accountService.getEmployeeId();
    this.userRole = this.accountService.getUserRole();
  }

  ViewDataDoc() {
     debugger;
        if((this.fromDate==null) && (this.employeeId==undefined || this.employeeId=="") && (this.approvalAuthorityId==undefined || this.approvalAuthorityId=="") && (this.donationId==undefined || this.donationId==""))
        {
          this.toastr.warning('Please enter at least 1 parameter!');
         return false;
        }

        if(this.employeeId == "")
        {
          this.employeeId = 0;
        }
        
        const  searchDto: IReportSearchDto ={
          employeeId: this.employeeId,
          approvalAuthorityId: this.approvalAuthorityId,
          donationId: this.donationId,
          year: this.fromDate,

        }
      this.reportService.getEmpMonthlyExpense(searchDto).subscribe(response => {
        debugger;
        this.reports = response;
      }, error => {
        console.log(error);
      });  
  }

  onOpenCalendar(e) {
    e.setViewMode('year');
    e.monthSelectHandler = (event: CalendarCellViewModel): void => {
      e.value = event.date;
      return;
    };
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
  approvalAuthorityId: number;
  donationId: number;
  year: Date;

}