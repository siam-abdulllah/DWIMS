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
import { ICampaignMst } from '../shared/models/campaign';
import * as XLSX from 'xlsx';
import { IDonation } from '../shared/models/donation';
import { ISBU } from '../shared/models/sbu';



@Component({
  selector: 'rptCampaignSummary',
  templateUrl: './rptCampaignSummary.component.html',
  styles: [
  ]
})

export class RptCampaignSummaryComponent implements OnInit {
  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  genParams: GenericParams;
  empId: string;
  isInvestmentInActive: boolean;
  searchText = '';
  numberPattern = "^[0-9]+(.[0-9]{1,10})?$";
  //totalCount = 0;
  reports: any;
  fromDate: Date;
  toDate: Date;
  marketCode: string;
  campaignId: any;
  doctorCode: any;
  donationId: any;
  sbu: any;
  institutionId: any;
  campaignMsts: ICampaignMst[];
  donations: IDonation[];
  SBUs: ISBU[];
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
    this.getSBU();
    //this.getEmployeeId();
    this.getCampaign();
    this.getDonation();
    this.bsConfig = Object.assign({}, { containerClass: 'theme-blue' }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();
  }

  getEmployeeId() {
    this.empId = this.accountService.getEmployeeId();
    this.userRole = this.accountService.getUserRole();
  }

  getDonation() {
    this.reportService.getDonations().subscribe(response => {
      this.donations = response as IDonation[];
    }, error => {
      console.log(error);
    });
  }
 
  getSBU() {
    this.reportService.getSBU().subscribe(response => {
      this.SBUs = response as ISBU[];
    }, error => {
      console.log(error);
    });
  }

  getCampaign() {
    this.SpinnerService.show();
    this.reportService.getCampaignMsts().subscribe(response => {
      this.campaignMsts = response as ICampaignMst[];
      this.SpinnerService.hide();
    }, error => {
      this.SpinnerService.hide();
      console.log(error);
    });
  }

  ViewDataDoc() {
        if( (this.campaignId==undefined || this.campaignId=="") && (this.institutionId==undefined || this.institutionId=="") && (this.doctorCode==undefined || this.doctorCode=="") && (this.marketCode==undefined || this.marketCode=="") && (this.sbu==undefined || this.sbu=="") && (this.donationId==undefined || this.donationId=="")  )
        {
          this.toastr.warning('Please enter at least 1 parameter!');
          return false;
        }
    
        if(this.campaignId == "")
        {
          this.campaignId = 0;
        }

        if(this.institutionId == "")
        {
          this.institutionId = 0;
        }

        if(this.doctorCode == "")
        {
          this.doctorCode = 0;
        }

        debugger;
        const campaignSearchDto: ICampaignSearchDto = {
          campaignId: this.campaignId,
          fromDate: this.fromDate,
          toDate: this.toDate,
          sbu: this.sbu,
          institutionId: this.institutionId,
          donationId: this.donationId,
          doctorId: this.doctorCode,
          marketCode: this.marketCode,
        };

      this.reportService.getCampaignSummaryReport(campaignSearchDto).subscribe(response => {
        debugger;
        this.reports = response;
      }, error => {
        console.log(error);
      });
  }

  exportexcel(): void
  {
    /* pass here the table id */
    let element = document.getElementById('excel-table');
    const ws: XLSX.WorkSheet =XLSX.utils.table_to_sheet(element);
 
    /* generate workbook and add the worksheet */
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');
 
    /* save to file */  
    XLSX.writeFile(wb, 'Campaign_summary_report.xlsx');
 
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

interface ICampaignSearchDto {
  campaignId: number | null;
  fromDate: Date | undefined | null;
  toDate: Date | undefined | null;
  sbu: string;
  institutionId: number | null;
  doctorId: number | null;
  donationId: number | null;
  marketCode: string;
}

