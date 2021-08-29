import { GenericParams } from './../shared/models/genericParams';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ReportInvestmentService } from '../_services/report-investment.service';
import { ToastrService } from 'ngx-toastr';

import { NgForm, FormGroup, FormControl, Validators } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';

import 'jspdf-autotable';
import * as jsPDF from 'jspdf';
import { DatePipe } from '@angular/common';

import { IDonation } from '../shared/models/donation';
import { IBcdsInfo } from '../shared/models/bcdsInfo';
import { ISocietyInfo } from '../shared/models/societyInfo';
import { IInstitution } from '../shared/models/institution';
import { ISBU } from '../shared/models/sbu';
import { IMarket, IRegion, ITerritory, IDivision, IZone } from '../shared/models/location';
import { Doctor, IDoctor } from '../shared/models/docotor';

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

  visSoc: boolean = true;
  visBcd: boolean = true;
  visIns: boolean = true;
  visDoc: boolean = true;

  visMarket: boolean = true;
  visZone: boolean = true;
  visTerritory: boolean = true;
  visRegion: boolean = true;
  visDivision: boolean = true;

  reports: IReportConfig[] = [];
  donations: IDonation[];
  bcds: IBcdsInfo[];
  society: ISocietyInfo[];
  doctor: IDoctor[];
  institutions: IInstitution[];
  SBUs: ISBU[];

  market: IMarket[];
  region: IRegion[];
  territory: ITerritory[];
  division: IDivision[];
  zone: IZone[];

  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  genParams: GenericParams;
  // approvalCeiling: IApprovalCeiling[];
  totalCount = 0;
  //constructor(public masterService: MasterService, private router: Router, private toastr: ToastrService) { }
  constructor(public reportInvestmentService: ReportInvestmentService, private router: Router, private toastr: ToastrService, private datePipe: DatePipe) {

  }

  createInvestmentSearchForm() {
    this.investmentSearchForm = new FormGroup({
      fromDate: new FormControl('', [Validators.required]),
      toDate: new FormControl('', [Validators.required]),
      donationTo: new FormControl('', [Validators.required]),
      societyId: new FormControl(''),
      bcdsId: new FormControl(''),
      institutionId: new FormControl(''),
      doctorId: new FormControl(''),
      donationType: new FormControl(''),
      sbu: new FormControl(''),

      locationType: new FormControl(''),
      marketCode: new FormControl(''),
      territoryCode: new FormControl(''),
      divisionCode: new FormControl(''),
      regionCode: new FormControl(''),
      zoneCode: new FormControl(''),
    });
  }

  ngOnInit() {
    this.getReportList();
    this.getDonation();
    this.getSBU();
    this.createInvestmentSearchForm();
    this.bsConfig = Object.assign({}, { containerClass: 'theme-blue' }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();
  }

  getReportList() {
    this.reportInvestmentService.getReportList().subscribe(response => {
  
      this.reports = response.data as IReportConfig[];
    }, error => {
      console.log(error);
    });
  }


  viewReport(rpt) {

    debugger;
    if(this.investmentSearchForm.value.fromDate == "" || this.investmentSearchForm.value.toDate == "")
    {
      this.toastr.error("Select Duration", "Error");
      return;
    }
debugger;
    if (rpt == "Institution/Society/BCDS wise Investment") {
      
       this.getDocSocInvestReport();
    }
  }

  onChangeLocationType() {

    this.visMarket = true;
    this.visZone = true;
    this.visTerritory = true;
    this.visRegion = true;
    this.visDivision = true;

    this.investmentSearchForm.value.marketCode = "";
    this.investmentSearchForm.value.territoryCode  =  "";
    this.investmentSearchForm.value.regionCode  =  "";
    this.investmentSearchForm.value.divisionCode =  "";
    this.investmentSearchForm.value.zoneCode =  "";

    if (this.investmentSearchForm.value.locationType == "Market") {
      this.visMarket = false;
      this.getMarket();
    }
    else if (this.investmentSearchForm.value.locationType == "Territory") {
      this.visTerritory = false;
      this.getTerritory();
    }
    else if (this.investmentSearchForm.value.locationType == "Region") {
      this.visRegion = false;
      this.getRegion();
    }
    else if (this.investmentSearchForm.value.locationType == "Zone") {
      this.visZone = false;
      this.getZone();
    }
    else if (this.investmentSearchForm.value.locationType == "Division") {
      this.visDivision = false;
      this.getDivision();
    }
  }

  onChangeDonationTo() {

    this.visSoc = true;
    this.visBcd = true;
    this.visIns = true;
    this.visDoc = true;

    this.investmentSearchForm.value.institutionId =  null;
    this.investmentSearchForm.value.societyId  =  null;
    this.investmentSearchForm.value.bcdsId  =  null;
    this.investmentSearchForm.value.doctorId =  null;

    if (this.investmentSearchForm.value.donationTo == "Institution") {
      this.visIns = false;
      this.getInstitution();
    }
    else if (this.investmentSearchForm.value.donationTo == "Bcds") {
      this.visBcd = false;
      this.getBcds();
    }
    else if (this.investmentSearchForm.value.donationTo == "Society") {
      this.visSoc = false;
      this.getSociety();
    }
    else if (this.investmentSearchForm.value.donationTo == "Doctor") {
      this.visDoc = false;
      this.getDoctor();
    }
  }

  getDoctor() {
    this.reportInvestmentService.getDoctors().subscribe(response => {
      this.doctor = response as IDoctor[];
    }, error => {
      console.log(error);
    });
  }


  getMarket() {
    this.reportInvestmentService.getMarket().subscribe(response => {
      this.market = response as IMarket[];
    }, error => {
      console.log(error);
    });
  }

  getTerritory() {
    this.reportInvestmentService.getTerritory().subscribe(response => {
      this.territory = response as ITerritory[];
    }, error => {
      console.log(error);
    });
  }

  getDivision() {
    this.reportInvestmentService.getDivision().subscribe(response => {
      this.division = response as IDivision[];
    }, error => {
      console.log(error);
    });
  }

  getRegion() {
    this.reportInvestmentService.getRegion().subscribe(response => {
      this.region = response as IRegion[];
    }, error => {
      console.log(error);
    });
  }

  getZone() {
    this.reportInvestmentService.getZone().subscribe(response => {
      this.zone = response as IZone[];
    }, error => {
      console.log(error);
    });
  }

  getDonation() {
    this.reportInvestmentService.getDonations().subscribe(response => {
      this.donations = response as IDonation[];
    }, error => {
      console.log(error);
    });
  }

  getSociety() {
    this.reportInvestmentService.getSociety().subscribe(response => {
      //debugger;
      this.society = response as ISocietyInfo[];
    }, error => {
      console.log(error);
    });
  }

  getBcds() {
    this.reportInvestmentService.getBcds().subscribe(response => {
      //debugger;
      this.bcds = response as IBcdsInfo[];
    }, error => {
      console.log(error);
    });
  }

  getInstitution() {
    this.reportInvestmentService.getInstitutions().subscribe(response => {
      this.institutions = response as IInstitution[];
    }, error => {
      console.log(error);
    });
  }

  getSBU() {
    this.reportInvestmentService.getSBU().subscribe(response => {
      this.SBUs = response as ISBU[];
    }, error => {
      console.log(error);
    });
  }

  getDocSocInvestReport() {
    debugger;
    //this.loading = true;
    // tslint:disable-next-line: radix
    // const impId = parseInt(this.loginService.getEmpOrImpName());
    const investmentReportSearchDto: IInvestmentReportSearchDto = {
      //importerId: impId,
      fromDate: this.investmentSearchForm.value.fromDate,
      toDate: this.investmentSearchForm.value.toDate,
      sbu: this.investmentSearchForm.value.sbu,
      userId: 0,
      donationType: this.investmentSearchForm.value.donationType,
      investType: this.investmentSearchForm.value.donationTo,
      institutionId: this.investmentSearchForm.value.institutionId,
      societyId: this.investmentSearchForm.value.societyId,
      bcdsId: this.investmentSearchForm.value.bcdsId,
      doctorId: this.investmentSearchForm.value.doctorId,

      locationType: this.investmentSearchForm.value.locationType,
      territoryCode: this.investmentSearchForm.value.territoryCode,
      marketCode: this.investmentSearchForm.value.marketCode,
      regionCode: this.investmentSearchForm.value.regionCode,
      zoneCode: this.investmentSearchForm.value.zoneCode,
      divisionCode: this.investmentSearchForm.value.divisionCode,
    };


debugger;
    this.reportInvestmentService.getInsSocietyBCDSWiseInvestment(investmentReportSearchDto).subscribe(resp => {
      // this.reportInvestmentService.getInsSocietyBCDSWiseInvestment().subscribe(resp => {  
      this.instSocDocInvestmentDto = resp as IInstSocDocInvestmentDto[];
      if (this.instSocDocInvestmentDto.length <= 0) {
        this.toastr.warning('No Data Found', 'Report');
      }
      if (this.instSocDocInvestmentDto.length > 0) {
        for (let p of this.instSocDocInvestmentDto) {
          var fD = this.datePipe.transform(p.fromDate, "dd/MM/yyyy")
          var tD = this.datePipe.transform(p.toDate, "dd/MM/yyyy")
        }
      }

      this.viewProformaSummaryReport();
    }, error => {
      console.log(error);
    });
  }

  viewProformaSummaryReport() {
    debugger;
    if (this.instSocDocInvestmentDto.length <= 0) {
      this.toastr.warning("No Data to Show Report", "Report");
      return false;
    }
    // const doc = new jsPDF();
    // doc.text("Hello there", 15, 15);
    // doc.save('first.pdf');

    const r = this.instSocDocInvestmentDto as IInstSocDocInvestmentDto[];

    let row: any[] = [];
    let rowD: any[] = [];
    let col = ['Name', 'Location', 'Donation \nType', 'Invested \nAmount.', 'Duration', 'Commitment',
      'Actual \nShare', 'Competitor \nShare']; // initialization for headers
    // let col = ['SL NO.','Name OF Importer','Products','PI No.','PI Date','Manufacturer',
    // 'Exporter', 'Country Of Origin','Pack Size','Approval Amount MT','Approval Amount Unit', 'Status'];
    let title = "Institute Wise Investment Report"; // title of report
    let slNO = 0;
    for (const a of r) {
      console.log(r);
      //row.push(++slNO);
      row.push(a.donationToName);
      row.push(a.locationName);
      row.push(a.donationType);
      //row.push(a.proformaDate);

      const convertedfDate = new Date(a.fromDate);
      let fd = '';
      fd += convertedfDate.getDate() + '/' + (convertedfDate.getMonth() + 1) + '/' + convertedfDate.getFullYear();

      const convertedtDate = new Date(a.toDate);
      let td = '';
      td += convertedtDate.getDate() + '/' + (convertedtDate.getMonth() + 1) + '/' + convertedtDate.getFullYear();
      row.push(fd + ' - ' + td);

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
  sbu: string;
  donationType: string;
  investType: string;
  institutionId: number | null;
  societyId: number | null;
  bcdsId: number | null;
  doctorId: number | null;
  locationType: string;
  territoryCode: string;
  marketCode: string;
  regionCode: string;
  zoneCode: string;
  divisionCode: string;
}


interface IInstSocDocInvestmentDto {
  id: number;
  donationType: string;
  investedAmount: number;
  commitment: number;
  actualShare: number;
  competitorShare: number;
  fromDate: Date | undefined | null;
  toDate: Date | undefined | null;
  donationToName: string;
  locationName: string;
}

export interface IReportConfig {
  id: number;
  reportName: string;
  reportFunc: string;
  reportCode: string;
}


export interface IReportConfigPagination {
  pageIndex: number;
  pageSize: number;
  count: number;
  data: IReportConfig[];
}

export class ReportConfigPagination implements IReportConfigPagination {
  pageIndex: number;
  pageSize: number;
  count: number;
  data: IReportConfig[] = [];
}