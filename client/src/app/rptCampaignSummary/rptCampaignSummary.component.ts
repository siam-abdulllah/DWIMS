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
import { ICampaignMst } from '../shared/models/campaign';
import * as XLSX from 'xlsx';
import { IDonation } from '../shared/models/donation';
import { ISBU } from '../shared/models/sbu';
import { ThisReceiver } from '@angular/compiler';


@Component({
  selector: 'rptCampaignSummary',
  templateUrl: './rptCampaignSummary.component.html',
  styles: [
  ]
})

export class RptCampaignSummaryComponent implements OnInit {
  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  genParams: GenericParams;
  donationTo: string;
  empId: string;
  isInvestmentInActive: boolean;
  searchText = '';
  numberPattern = "^[0-9]+(.[0-9]{1,10})?$";
  //totalCount = 0;
  donations: IDonation[];
  SBUs: ISBU[];
  reports: any;
  fromDate: Date;
  toDate: Date;
  marketCode: string;
  campaignId: any;
  donationId: any;
  doctorCode: any;
  societyName: any;
  bcdsName: any;
  sbu: any;
  institutionId: any;
  //societyId: any;
  //bcdsId: any;
  campaignMsts: ICampaignMst[];
  rptDepotLetter: IrptDepotLetterSearch[] = [];
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
    private SpinnerService: NgxSpinnerService, private accountService: AccountService) { }

  ngOnInit() {
    this.resetForm();
    //this.getEmployeeId();
    this.getCampaign();
    this.getDonation();
    this.getSBU();
    this.bsConfig = Object.assign({}, { containerClass: 'theme-blue' }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();
  }

  getEmployeeId() {
    this.empId = this.accountService.getEmployeeId();
    this.userRole = this.accountService.getUserRole();
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

  ViewDataDoc() {
    if ((this.campaignId == undefined || this.campaignId == "") && (this.institutionId == undefined || this.institutionId == "") && (this.doctorCode == undefined || this.doctorCode == "") && (this.marketCode == undefined || this.marketCode == "") && (this.sbu == undefined || this.sbu == "") && (this.donationId == undefined || this.donationId == ""))

      if (this.campaignId == "") {
        this.campaignId = 0;
      }

    if (this.institutionId == "") {
      this.institutionId = 0;
    }

    if (this.doctorCode == "") {
      this.doctorCode = 0;
    }

    if (this.sbu == "All") {
      this.sbu = "";
    }
    debugger;
    if (this.donationTo == 'Campaign') {
      const campaignSearchDto: ICampaignSearchDto = {
        campaignId: this.campaignId,
        fromDate: this.datePipe.transform(this.fromDate, 'yyyy-MM-dd HH:mm:ss'),
        toDate: this.datePipe.transform(this.toDate, 'yyyy-MM-dd HH:mm:ss'),

        //fromDate: this.fromDate,
        //toDate: this.toDate,
        donationId: this.donationId,
        sbu: this.sbu,
        institutionId: this.institutionId,
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
    else if (this.donationTo == 'Doctor') {
      const searchDto: ISearchDto = {
        doctorId: this.doctorCode,
        institutionId: 0,
        campaignId: 0,
        bcdsName: '',
        societyName: '',
        fromDate: this.datePipe.transform(this.fromDate, 'yyyy-MM-dd HH:mm:ss'),
        toDate: this.datePipe.transform(this.toDate, 'yyyy-MM-dd HH:mm:ss'),
        donationId: this.donationId,
        sbu: this.sbu,
        marketCode: this.marketCode,
      };
      this.reportService.getDoctorSummaryReport(searchDto).subscribe(response => {
        debugger;
        this.reports = response;
      }, error => {
        console.log(error);
      });
    }
    else if (this.donationTo == 'Institution') {
      const searchDto: ISearchDto = {
        doctorId: 0,
        institutionId: this.institutionId,
        campaignId: 0,
        bcdsName: '',
        societyName: '',
        fromDate: this.datePipe.transform(this.fromDate, 'yyyy-MM-dd HH:mm:ss'),
        toDate: this.datePipe.transform(this.toDate, 'yyyy-MM-dd HH:mm:ss'),
        donationId: this.donationId,
        sbu: this.sbu,
        marketCode: this.marketCode,
      };
      this.reportService.getInstitutionSummaryReport(searchDto).subscribe(response => {
        debugger;
        this.reports = response;
      }, error => {
        console.log(error);
      });
    }
    else if (this.donationTo == 'Bcds') {
      const searchDto: ISearchDto = {
        doctorId: 0,
        institutionId: 0,
        campaignId: 0,
        bcdsName: this.bcdsName,
        societyName: '',
        fromDate: this.datePipe.transform(this.fromDate, 'yyyy-MM-dd HH:mm:ss'),
        toDate: this.datePipe.transform(this.toDate, 'yyyy-MM-dd HH:mm:ss'),
        donationId: this.donationId,
        sbu: this.sbu,
        marketCode: this.marketCode,
      };
      this.reportService.getBcdsSummaryReport(searchDto).subscribe(response => {
        debugger;
        this.reports = response;
      }, error => {
        console.log(error);
      });
    }
    else if (this.donationTo == 'Society') {
      const searchDto: ISearchDto = {
        doctorId: 0,
        institutionId: 0,
        campaignId: 0,
        bcdsName: '',
        societyName: this.societyName,
        fromDate: this.datePipe.transform(this.fromDate, 'yyyy-MM-dd HH:mm:ss'),
        toDate: this.datePipe.transform(this.toDate, 'yyyy-MM-dd HH:mm:ss'),
        donationId: this.donationId,
        sbu: this.sbu,
        marketCode: this.marketCode,
      };
      this.reportService.getSocietySummaryReport(searchDto).subscribe(response => {
        debugger;
        this.reports = response;
      }, error => {
        console.log(error);
      });
    }
    else if (this.donationTo == 'All') {
      const searchDto: ISearchDto = {
        doctorId: this.doctorCode,
        institutionId: this.institutionId,
        campaignId: this.campaignId,
        bcdsName: this.bcdsName,
        societyName: this.societyName,
        fromDate: this.datePipe.transform(this.fromDate, 'yyyy-MM-dd HH:mm:ss'),
        toDate: this.datePipe.transform(this.toDate, 'yyyy-MM-dd HH:mm:ss'),
        donationId: this.donationId,
        sbu: this.sbu,
        marketCode: this.marketCode,
      };
      this.reportService.getSummaryReport(searchDto).subscribe(response => {
        debugger;
        this.reports = response;
      }, error => {
        console.log(error);
      });
    }
  }

  exportexcel(): void {
    /* pass here the table id */
    let element = document.getElementById('excel-table');
    const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(element);

    /* generate workbook and add the worksheet */
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');

    /* save to file */
    if(this.donationTo == 'All'){}
    else if (this.donationTo == 'Doctor') {}
    else if (this.donationTo == 'Institution'){}
    else if (this.donationTo == 'Campaign'){}
    else if (this.donationTo == 'Society'){}
    else if (this.donationTo == 'Bcds'){}
    XLSX.writeFile(wb, 'Campaign_summary_report.xlsx');

  }

  resetSearch() {
    this.searchText = '';
  }

  resetPage(form: NgForm) {
    form.form.reset();
  }

  resetForm() {
    this.donationTo = 'Campaign';
  }
}

interface ICampaignSearchDto {
  campaignId: number | null;
  fromDate: any;
  toDate: any;
  sbu: string;
  institutionId: number | null;
  donationId: number | null;
  doctorId: number | null;
  marketCode: string;
}
interface ISearchDto {
  doctorId: number | null;
  institutionId: number | null;
  campaignId: number | null;
  bcdsName: string;
  societyName: string;
  fromDate: any;
  toDate: any;
  sbu: string;
  donationId: number | null;
  marketCode: string;
}

