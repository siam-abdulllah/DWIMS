import { Institution } from './../shared/models/institution';
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

import { IDocLocWiseInvestment } from './../shared/models/rptDocLocWiseInvestment';
import { IDocCampWiseInvestment } from '../shared/models/rptDocCampWiseInvestment';
import { IInsSocBcdsInvestment } from '../shared/models/rptInsSocBcdsInvestment';
import { ISBUWiseExpSummaryReport } from '../shared/models/rptSBUWiseExpSummaryReport';

import { ICampaignMst } from '../shared/models/campaign';
import { ISubCampaign } from '../shared/models/subCampaign';
import { IDonation } from '../shared/models/donation';
import { IBcdsInfo } from '../shared/models/bcdsInfo';
import { ISocietyInfo } from '../shared/models/societyInfo';
import { IInstitution } from '../shared/models/institution';
import { IProduct } from './../shared/models/product';
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
  insSocBcdsInvestment: IInsSocBcdsInvestment[] = [];
  docCampWiseInvestment: IDocCampWiseInvestment[] = [];
  docLocWiseInvestment: IDocLocWiseInvestment[] = [];
  sBUWiseExpSummaryReport :ISBUWiseExpSummaryReport[] = [];
  searchText = '';
  visSoc: boolean = true;
  visBcd: boolean = true;
  visIns: boolean = true;
  visDoc: boolean = true;
  config: any;
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
  products: IProduct[];
  campaignMst: ICampaignMst[];
  subCampaigns: ISubCampaign[];

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
      searchText: new FormControl(''),
      doctorId: new FormControl(''),
      donationType: new FormControl(''),
      sbu: new FormControl(''),

      locationType: new FormControl(''),
      marketCode: new FormControl(''),
      territoryCode: new FormControl(''),
      divisionCode: new FormControl(''),
      regionCode: new FormControl(''),
      zoneCode: new FormControl(''),

      brandCode: new FormControl(''),
      campaignName: new FormControl(''),
      subCampaignName: new FormControl(''),
    });
  }

  ngOnInit() {
    this.getReportList();
    this.getDonation();
    this.getSBU();
    this.getSubCampaign();
    this.getCampaign();
    this.createInvestmentSearchForm();
    this.bsConfig = Object.assign({}, { containerClass: 'theme-blue' }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();
    this.config = {
      currentPage: 1,
      itemsPerPage: 10,
      totalItems:50,
      };
  }

  getReportList() {
    this.reportInvestmentService.getReportList().subscribe(response => {
      const params = this.reportInvestmentService.getGenParams();
      this.reports = response.data;
      this.totalCount = response.count;
      this.config = {
        currentPage: params.pageIndex,
        itemsPerPage: params.pageSize,
        totalItems:this.totalCount,
        };
    }, error => {
      console.log(error);
    });
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

  onPageChanged(event: any){
    debugger;
    const params = this.reportInvestmentService.getGenParams();
    if (params.pageIndex !== event)
    {
      params.pageIndex = event;
      this.reportInvestmentService.setGenParams(params);
      this.getReportList();
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

  getCampaign() {
    this.reportInvestmentService.getCampaignMsts().subscribe(response => {
      this.campaignMst = response as ICampaignMst[];
    }, error => {
      console.log(error);
    });
  }

  getSubCampaign() {
    this.reportInvestmentService.getSubCampaign().subscribe(response => {
      this.subCampaigns = response as ISubCampaign[];
    }, error => {
      console.log(error);
    });
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
      this.society = response as ISocietyInfo[];
    }, error => {
      console.log(error);
    });
  }

  getBcds() {
    this.reportInvestmentService.getBcds().subscribe(response => {
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

  viewReport(rpt) {

    debugger;
    if(this.investmentSearchForm.value.fromDate == "" || this.investmentSearchForm.value.toDate == "")
    {
      this.toastr.error("Select Duration", "Error");
      return;
    }


    if (rpt == "Institution/Society/BCDS Wise Investment") {
      this.getDocSocInvestReport();
    }
    if (rpt == "SBU wise Invested Doctor's ROI") {
      this.getSbuWiseDocROIReport();
    }
    if (rpt == "Donation Category wise Investment") {
      this.getDonationWiseInvestmentReport();
    }
    if (rpt == "Location Category wise Investment") {
      this.getDonationWiseInvestmentReport();
    }
    if (rpt == "Doctor wise Commitment vs Return") {
      this.getDoctorWiseCommitvsReturnReport();
    }
    if (rpt == "Highest Deviation Report") {
      this.getHighestDeviationReport();
    }
    if (rpt == "Doctor Wise Leadership Analysis") {
      this.getDocWiseLeadershipReport();
    }
    if (rpt == "Invested Doctor Potentiality Analysis") {
      this.getDocWisePotentialReport();
    }
    if (rpt == "Brand Wise Investment Report") {
      this.getBrandWiseInvestmentReport();
    }
    if (rpt == "Brand Doctor Wise Investment Report") {
      this.getBrandDocWiseInvestmentReport();
    }
    if (rpt == "Campaign / Sub Campaign Wise Investment Report") {
      this.getCampSubCampWiseInvestmentReport();
    }
    if (rpt == "SBU wise Investment And Commitment vs Share") {
      this.getSBUWiseInvestCommitShareIndivDocReport();
    }
    if (rpt == "SBU wise Budget And Expense Summary") {
      this.getSBUWiseBudgetInvest();
    }
  }

/// ********************************************
/// Generate SBU wise Invested Doctor's ROI
/// ********************************************

getSbuWiseDocROIReport()  {
  const investmentReportSearchDto: IInvestmentReportSearchDto = {
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
    brandCode: this.investmentSearchForm.value.brandCode,
    campaignName: this.investmentSearchForm.value.campaignName,
    subCampaignName: this.investmentSearchForm.value.subCampaignName,
  };
debugger;
  this.reportInvestmentService.GetDoctorLocationWiseInvestment(investmentReportSearchDto).subscribe(resp => {
    // this.reportInvestmentService.getInsSocietyBCDSWiseInvestment().subscribe(resp => {  
    this.docLocWiseInvestment = resp as IDocLocWiseInvestment[];
    if (this.docLocWiseInvestment.length <= 0) {
      this.toastr.warning('No Data Found', 'Report');
    }
    if (this.docLocWiseInvestment.length > 0) {
      for (let p of this.docLocWiseInvestment) {
        var fD = this.datePipe.transform(p.fromDate, "dd/MM/yyyy")
        var tD = this.datePipe.transform(p.toDate, "dd/MM/yyyy")
      }
    }
    this.viewSbuWiseDocROIReport();
  }, error => {
    console.log(error);
  });
}

/// ********************************************
/// View SBU wise Invested Doctor's ROI
/// ********************************************

viewSbuWiseDocROIReport() {
  debugger;
  if (this.docLocWiseInvestment.length <= 0) {
    return false;
  }

  const r =  this.docLocWiseInvestment as IDocLocWiseInvestment[];

  let row: any[] = [];
  let rowD: any[] = [];
  let col = ['SBU Name', 'Location', 'Invested \nAmount', 'Duration', 'Commitment',
    'Actual \nShare']; // initialization for headers
  // let col = ['SL NO.','Name OF Importer','Products','PI No.','PI Date','Manufacturer',
  // 'Exporter', 'Country Of Origin','Pack Size','Approval Amount MT','Approval Amount Unit', 'Status'];
  let slNO = 0;
  for (const a of r) {
    console.log(r);
    //row.push(++slNO);
    row.push(a.sbuName);
    row.push(a.marketName +", " + a.territoryName +", \n" + a.regionName +", " + a.divisionName +", \n" + a.zoneName);
    row.push(a.investedAmt);

    const convertedfDate = new Date(a.fromDate);
    let fd = '';
    fd += convertedfDate.getDate() + '/' + (convertedfDate.getMonth() + 1) + '/' + convertedfDate.getFullYear();

    const convertedtDate = new Date(a.toDate);
    let td = '';
    td += convertedtDate.getDate() + '/' + (convertedtDate.getMonth() + 1) + '/' + convertedtDate.getFullYear();
    row.push(fd + ' - ' + td);

    row.push(a.commitment);
    row.push(a.actualShare);

    rowD.push(row);
    row = [];
  }
  //this.getReport(col, rowD, title, orgName, orgAddress);
  this.getReport(col, rowD, 'SBU wise Invested Doctors ROI Report', 'Square Pharmaceuticals Ltd.', '48, Square Center, Mohakhali');
}

/// ********************************************
/// Generate Donation Category wise Investment
/// ********************************************

getDonationWiseInvestmentReport()  {
  const investmentReportSearchDto: IInvestmentReportSearchDto = {
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
    brandCode: this.investmentSearchForm.value.brandCode,
    campaignName: this.investmentSearchForm.value.campaignName,
    subCampaignName: this.investmentSearchForm.value.subCampaignName,
  };

  this.reportInvestmentService.GetDoctorLocationWiseInvestment(investmentReportSearchDto).subscribe(resp => {
    // this.reportInvestmentService.getInsSocietyBCDSWiseInvestment().subscribe(resp => {  
    this.docLocWiseInvestment = resp as IDocLocWiseInvestment[];
    if (this.docLocWiseInvestment.length <= 0) {
      this.toastr.warning('No Data Found', 'Report');
    }
    if (this.docLocWiseInvestment.length > 0) {
      for (let p of this.docLocWiseInvestment) {
        var fD = this.datePipe.transform(p.fromDate, "dd/MM/yyyy")
        var tD = this.datePipe.transform(p.toDate, "dd/MM/yyyy")
      }
    }
    this.viewDonationWiseInvestmentReport();
  }, error => {
    console.log(error);
  });
}

/// ********************************************
/// View Donation Category wise Investment
/// ********************************************

viewDonationWiseInvestmentReport() {
  debugger;
  if (this.docLocWiseInvestment.length <= 0) {
    return false;
  }

  const r =  this.docLocWiseInvestment as IDocLocWiseInvestment[];

  let row: any[] = [];
  let rowD: any[] = [];
  let col = ['SBU Name', 'Location', 'Doctor Code', 'Doctor Name' , 'Institution' , 'Invested \nAmount.', 'Duration', 'Commitment',
    'Actual \nShare']; // initialization for headers
  // let col = ['SL NO.','Name OF Importer','Products','PI No.','PI Date','Manufacturer',
  // 'Exporter', 'Country Of Origin','Pack Size','Approval Amount MT','Approval Amount Unit', 'Status'];
  let slNO = 0;
  for (const a of r) {
    console.log(r);
    //row.push(++slNO);
    row.push(a.sbuName);
    row.push(a.marketName + ", " + a.territoryName + ", \n" + a.regionName + ", " + a.divisionName + ", \n" + a.zoneName);
    row.push(a.doctorId);
    row.push(a.doctorName);
    row.push(a.institutionName);
    row.push(a.investedAmt);

    const convertedfDate = new Date(a.fromDate);
    let fd = '';
    fd += convertedfDate.getDate() + '/' + (convertedfDate.getMonth() + 1) + '/' + convertedfDate.getFullYear();

    const convertedtDate = new Date(a.toDate);
    let td = '';
    td += convertedtDate.getDate() + '/' + (convertedtDate.getMonth() + 1) + '/' + convertedtDate.getFullYear();
    row.push(fd + ' - ' + td);

    row.push(a.commitment);
    row.push(a.actualShare);

    rowD.push(row);
    row = [];
  }
  //this.getReport(col, rowD, title, orgName, orgAddress);
  this.getReport(col, rowD, 'Category Wise Investment Report', 'Square Pharmaceuticals Ltd.', '48, Square Center, Mohakhali');
}

/// ********************************************
/// Generate Doctor wise Commitment vs. Return (actual share)
/// ********************************************

getDoctorWiseCommitvsReturnReport()  {
  const investmentReportSearchDto: IInvestmentReportSearchDto = {
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
    brandCode: this.investmentSearchForm.value.brandCode,
    campaignName: this.investmentSearchForm.value.campaignName,
    subCampaignName: this.investmentSearchForm.value.subCampaignName,
  };

  this.reportInvestmentService.GetDoctorLocationWiseInvestment(investmentReportSearchDto).subscribe(resp => {
    // this.reportInvestmentService.getInsSocietyBCDSWiseInvestment().subscribe(resp => {  
    this.docLocWiseInvestment = resp as IDocLocWiseInvestment[];
    if (this.docLocWiseInvestment.length <= 0) {
      this.toastr.warning('No Data Found', 'Report');
    }
    if (this.docLocWiseInvestment.length > 0) {
      for (let p of this.docLocWiseInvestment) {
        var fD = this.datePipe.transform(p.fromDate, "dd/MM/yyyy")
        var tD = this.datePipe.transform(p.toDate, "dd/MM/yyyy")
      }
    }
    this.viewDoctorWiseCommitvsReturnReport();
  }, error => {
    console.log(error);
  });
}

/// ********************************************
/// View Doctor wise Commitment vs. Return (actual share)
/// ********************************************

viewDoctorWiseCommitvsReturnReport() {
  debugger;
  if (this.docLocWiseInvestment.length <= 0) {
    return false;
  }

  const r =  this.docLocWiseInvestment as IDocLocWiseInvestment[];

  let row: any[] = [];
  let rowD: any[] = [];
  let col = ['SBU Name', 'Location', 'Doctor Code', 'Doctor Name' , 'Institution' , 'Donation Type' ,'Invested \nAmount.', 'Duration', 'Commitment',
    'Actual \nShare', 'Competitator \nShare']; // initialization for headers
  // let col = ['SL NO.','Name OF Importer','Products','PI No.','PI Date','Manufacturer',
  // 'Exporter', 'Country Of Origin','Pack Size','Approval Amount MT','Approval Amount Unit', 'Status'];
  let slNO = 0;
  for (const a of r) {
    console.log(r);
    //row.push(++slNO);
    row.push(a.sbuName);
    row.push(a.marketName +", " + a.territoryName+", \n" + a.regionName+", " + a.divisionName+", \n" + a.zoneName);
    row.push(a.doctorId);
    row.push(a.doctorName);
    row.push(a.institutionName);
    row.push(a.donationType);
    row.push(a.investedAmt);

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
  this.getReport(col, rowD, 'Doctor wise Commitment vs. Return', 'Square Pharmaceuticals Ltd.', '48, Square Center, Mohakhali');
}

/// ********************************************
/// Generate Institution/Society/BCDS wise Investment
/// ********************************************

  getDocSocInvestReport()  {
    const investmentReportSearchDto: IInvestmentReportSearchDto = {
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
      brandCode: this.investmentSearchForm.value.brandCode,
      campaignName: this.investmentSearchForm.value.campaignName,
      subCampaignName: this.investmentSearchForm.value.subCampaignName,
    };

    this.reportInvestmentService.getInsSocietyBCDSWiseInvestment(investmentReportSearchDto).subscribe(resp => {
      // this.reportInvestmentService.getInsSocietyBCDSWiseInvestment().subscribe(resp => {  
      this.insSocBcdsInvestment = resp as IInsSocBcdsInvestment[];
      if (this.insSocBcdsInvestment.length <= 0) {
        this.toastr.warning('No Data Found', 'Report');
      }
      if (this.insSocBcdsInvestment.length > 0) {
        for (let p of this.insSocBcdsInvestment) {
          var fD = this.datePipe.transform(p.fromDate, "dd/MM/yyyy")
          var tD = this.datePipe.transform(p.toDate, "dd/MM/yyyy")
        }
      }
      this.viewDocSocInvestReport();
    }, error => {
      console.log(error);
    });
  }

/// ********************************************
/// View Institution/Society/BCDS wise Investment
/// ********************************************

  viewDocSocInvestReport() {
    debugger;
    if (this.insSocBcdsInvestment.length <= 0) {
      return false;
    }
  
    const r = this.insSocBcdsInvestment as IInsSocBcdsInvestment[];

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
      row.push(a.institutionName + a.societyName + a.bcdsName);
      row.push(a.marketName + ", " + a.territoryName + ", \n" + a.regionName + ", " + a.divisionName + ", \n" + a.zoneName);
      row.push(a.donationType);
      row.push(a.investedAmt);

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

/// ********************************************
/// Generate Highest Deviation Report
/// ********************************************

getHighestDeviationReport()  {
  const investmentReportSearchDto: IInvestmentReportSearchDto = {
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
    brandCode: this.investmentSearchForm.value.brandCode,
    campaignName: this.investmentSearchForm.value.campaignName,
    subCampaignName: this.investmentSearchForm.value.subCampaignName,
  };

  this.reportInvestmentService.GetDoctorLocationWiseInvestment(investmentReportSearchDto).subscribe(resp => {
    // this.reportInvestmentService.getInsSocietyBCDSWiseInvestment().subscribe(resp => {  
    this.docLocWiseInvestment = resp as IDocLocWiseInvestment[];
    if (this.docLocWiseInvestment.length <= 0) {
      this.toastr.warning('No Data Found', 'Report');
    }
    if (this.docLocWiseInvestment.length > 0) {
      for (let p of this.docLocWiseInvestment) {
        var fD = this.datePipe.transform(p.fromDate, "dd/MM/yyyy")
        var tD = this.datePipe.transform(p.toDate, "dd/MM/yyyy")
      }
    }
    this.viewHighestDeviationReport();
  }, error => {
    console.log(error);
  });
}

/// ********************************************
/// View Highest Deviation Report
/// ********************************************

viewHighestDeviationReport() {
  debugger;
  if (this.docLocWiseInvestment.length <= 0) {
    return false;
  }

  const r =  this.docLocWiseInvestment as IDocLocWiseInvestment[];

  let row: any[] = [];
  let rowD: any[] = [];
  let col = ['SBU Name', 'Location', 'Doctor Code', 'Doctor Name' , 'Institution' , 'Donation Type' ,'Invested \nAmount.', 'Duration', 'Commitment',
    'Actual \nShare', 'Deviation']; // initialization for headers
  // let col = ['SL NO.','Name OF Importer','Products','PI No.','PI Date','Manufacturer',
  // 'Exporter', 'Country Of Origin','Pack Size','Approval Amount MT','Approval Amount Unit', 'Status'];
  let slNO = 0;
  for (const a of r) {
    console.log(r);
    //row.push(++slNO);
    row.push(a.sbuName);
    row.push(a.marketName + ", " + a.territoryName + ", \n" + a.regionName + ", " + a.divisionName + ", \n" + a.zoneName);
    row.push(a.doctorId);
    row.push(a.doctorName);
    row.push(a.institutionName);
    row.push(a.donationType);
    row.push(a.investedAmt);

    const convertedfDate = new Date(a.fromDate);
    let fd = '';
    fd += convertedfDate.getDate() + '/' + (convertedfDate.getMonth() + 1) + '/' + convertedfDate.getFullYear();

    const convertedtDate = new Date(a.toDate);
    let td = '';
    td += convertedtDate.getDate() + '/' + (convertedtDate.getMonth() + 1) + '/' + convertedtDate.getFullYear();
    row.push(fd + ' - ' + td);

    row.push(a.commitment);
    row.push(a.actualShare);
    row.push(a.deviation);

    rowD.push(row);
    row = [];
  }
  //this.getReport(col, rowD, title, orgName, orgAddress);
  this.getReport(col, rowD, 'Highest Deviation - Commitment vs Return', 'Square Pharmaceuticals Ltd.', '48, Square Center, Mohakhali');
}

/// ********************************************
/// Generate Doctor Wise Leadership Analysis
/// ********************************************

getDocWiseLeadershipReport()  {
  const investmentReportSearchDto: IInvestmentReportSearchDto = {
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
    brandCode: this.investmentSearchForm.value.brandCode,
    campaignName: this.investmentSearchForm.value.campaignName,
    subCampaignName: this.investmentSearchForm.value.subCampaignName,
  };

  this.reportInvestmentService.GetDoctorLocationWiseInvestment(investmentReportSearchDto).subscribe(resp => {
    // this.reportInvestmentService.getInsSocietyBCDSWiseInvestment().subscribe(resp => {  
    this.docLocWiseInvestment = resp as IDocLocWiseInvestment[];
    if (this.docLocWiseInvestment.length <= 0) {
      this.toastr.warning('No Data Found', 'Report');
    }
    if (this.docLocWiseInvestment.length > 0) {
      for (let p of this.docLocWiseInvestment) {
        var fD = this.datePipe.transform(p.fromDate, "dd/MM/yyyy")
        var tD = this.datePipe.transform(p.toDate, "dd/MM/yyyy")
      }
    }
    this.viewDocWiseLeadershipReport();
  }, error => {
    console.log(error);
  });
}

/// ********************************************
/// View Doctor Wise Leadership Analysis
/// ********************************************

viewDocWiseLeadershipReport() {
  debugger;
  if (this.docLocWiseInvestment.length <= 0) {
    return false;
  }

  const r =  this.docLocWiseInvestment as IDocLocWiseInvestment[];

  let row: any[] = [];
  let rowD: any[] = [];
  let col = ['SBU Name', 'Location', 'Doctor Code', 'Doctor Name' , 'Institution' , 'Donation Type' ,'Invested \nAmount.', 'Duration', 'Commitment',
    'Actual \nShare', 'Competitor \nShare', 'Leader vs \nNon Leader']; // initialization for headers
  // let col = ['SL NO.','Name OF Importer','Products','PI No.','PI Date','Manufacturer',
  // 'Exporter', 'Country Of Origin','Pack Size','Approval Amount MT','Approval Amount Unit', 'Status'];
  let slNO = 0;
  for (const a of r) {
    console.log(r);
    //row.push(++slNO);
    row.push(a.sbuName);
    row.push(a.marketName + ", " + a.territoryName + ", \n" + a.regionName + ", " + a.divisionName + ", \n" + a.zoneName);
    row.push(a.doctorId);
    row.push(a.doctorName);
    row.push(a.institutionName);
    row.push(a.donationType);
    row.push(a.investedAmt);

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
    row.push(a.leaderNonLeader);

    rowD.push(row);
    row = [];
  }
  //this.getReport(col, rowD, title, orgName, orgAddress);
  this.getReport(col, rowD, 'Doctor Wise Leadership Analysis Report', 'Square Pharmaceuticals Ltd.', '48, Square Center, Mohakhali');
}

/// ********************************************
/// Generate Invested Doctor Potentiality Analysis
/// ********************************************

getDocWisePotentialReport()  {
  const investmentReportSearchDto: IInvestmentReportSearchDto = {
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
    brandCode: this.investmentSearchForm.value.brandCode,
    campaignName: this.investmentSearchForm.value.campaignName,
    subCampaignName: this.investmentSearchForm.value.subCampaignName,
  };

  this.reportInvestmentService.GetDoctorLocationWiseInvestment(investmentReportSearchDto).subscribe(resp => {
    // this.reportInvestmentService.getInsSocietyBCDSWiseInvestment().subscribe(resp => {  
    this.docLocWiseInvestment = resp as IDocLocWiseInvestment[];
    if (this.docLocWiseInvestment.length <= 0) {
      this.toastr.warning('No Data Found', 'Report');
    }
    if (this.docLocWiseInvestment.length > 0) {
      for (let p of this.docLocWiseInvestment) {
        var fD = this.datePipe.transform(p.fromDate, "dd/MM/yyyy")
        var tD = this.datePipe.transform(p.toDate, "dd/MM/yyyy")
      }
    }
    this.viewDocWisePotentialReport();
  }, error => {
    console.log(error);
  });
}

/// ********************************************
/// View Invested Doctor Potentiality Analysis
/// ********************************************

viewDocWisePotentialReport() {
  debugger;
  if (this.docLocWiseInvestment.length <= 0) {
    return false;
  }

  const r =  this.docLocWiseInvestment as IDocLocWiseInvestment[];

  let row: any[] = [];
  let rowD: any[] = [];
  let col = ['SBU Name', 'Location', 'Doctor Code', 'Doctor Name' , 'Institution' , 'Donation Type' ,'Invested \nAmount.', 'Duration', 'Commitment',
    'Actual \nShare', 'Number of \nTransaction', 'No of \Patient']; // initialization for headers
  // let col = ['SL NO.','Name OF Importer','Products','PI No.','PI Date','Manufacturer',
  // 'Exporter', 'Country Of Origin','Pack Size','Approval Amount MT','Approval Amount Unit', 'Status'];
  let slNO = 0;
  for (const a of r) {
    console.log(r);
    //row.push(++slNO);
    row.push(a.sbuName);
    row.push(a.marketName + ", " + a.territoryName + ", \n" + a.regionName + ", " + a.divisionName + ", \n" + a.zoneName);
    row.push(a.doctorId);
    row.push(a.doctorName);
    row.push(a.institutionName);
    row.push(a.donationType);
    row.push(a.investedAmt);

    const convertedfDate = new Date(a.fromDate);
    let fd = '';
    fd += convertedfDate.getDate() + '/' + (convertedfDate.getMonth() + 1) + '/' + convertedfDate.getFullYear();

    const convertedtDate = new Date(a.toDate);
    let td = '';
    td += convertedtDate.getDate() + '/' + (convertedtDate.getMonth() + 1) + '/' + convertedtDate.getFullYear();
    row.push(fd + ' - ' + td);

    row.push(a.commitment);
    row.push(a.actualShare);
    row.push(a.noOfPresc);
    row.push(a.noOfPatient);

    rowD.push(row);
    row = [];
  }
  //this.getReport(col, rowD, title, orgName, orgAddress);
  this.getReport(col, rowD, 'Doctor Wise Potentiality Analysis Report', 'Square Pharmaceuticals Ltd.', '48, Square Center, Mohakhali');
}

/// ********************************************
/// Generate Brand Wise Investment Report
/// ********************************************

getBrandWiseInvestmentReport()  {
  const investmentReportSearchDto: IInvestmentReportSearchDto = {
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
    brandCode: this.investmentSearchForm.value.brandCode,
    campaignName: this.investmentSearchForm.value.campaignName,
    subCampaignName: this.investmentSearchForm.value.subCampaignName,
  };

  this.reportInvestmentService.GetDoctorCampaingWiseInvestment(investmentReportSearchDto).subscribe(resp => {
    // this.reportInvestmentService.getInsSocietyBCDSWiseInvestment().subscribe(resp => {  
    this.docCampWiseInvestment = resp as IDocCampWiseInvestment[];
    if (this.docCampWiseInvestment.length <= 0) {
      this.toastr.warning('No Data Found', 'Report');
    }
    if (this.docCampWiseInvestment.length > 0) {
      for (let p of this.docCampWiseInvestment) {
        var fD = this.datePipe.transform(p.fromDate, "dd/MM/yyyy")
        var tD = this.datePipe.transform(p.toDate, "dd/MM/yyyy")
      }
    }
    this.viewBrandWiseInvestmentReport();
  }, error => {
    console.log(error);
  });
}

/// ********************************************
/// View Brand Wise Investment Report
/// ********************************************

viewBrandWiseInvestmentReport() {
  debugger;
  if (this.docCampWiseInvestment.length <= 0) {
    return false;
  }

  const r =  this.docCampWiseInvestment as IDocCampWiseInvestment[];

  let row: any[] = [];
  let rowD: any[] = [];
  let col = ['SBU Name', 'Location', 'Brand Name' , 'Campaign' , 'Sub-Campaign' ,'Investment \nPrev. Yr', 'Investment \nCurnt. Yr']; // initialization for headers
  // let col = ['SL NO.','Name OF Importer','Products','PI No.','PI Date','Manufacturer',
  // 'Exporter', 'Country Of Origin','Pack Size','Approval Amount MT','Approval Amount Unit', 'Status'];
  let slNO = 0;
  for (const a of r) {
    console.log(r);
    //row.push(++slNO);
    row.push(a.sbuName);
    row.push(a.marketName + ", " + a.territoryName + ", \n" + a.regionName + ", " + a.divisionName + ", \n" + a.zoneName);
    row.push(a.brand);
    row.push(a.campaign);
    row.push(a.subCampaign);
    row.push(a.investmentPast);
    row.push(a.investmentPresent);

    rowD.push(row);
    row = [];
  }
  //this.getReport(col, rowD, title, orgName, orgAddress);
  this.getReport(col, rowD, 'Brand Wise Investment Report', 'Square Pharmaceuticals Ltd.', '48, Square Center, Mohakhali');
}

/// ********************************************
/// Generate Brand Doctor Wise Investment Report
/// ********************************************

getBrandDocWiseInvestmentReport()  {
  const investmentReportSearchDto: IInvestmentReportSearchDto = {
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
    brandCode: this.investmentSearchForm.value.brandCode,
    campaignName: this.investmentSearchForm.value.campaignName,
    subCampaignName: this.investmentSearchForm.value.subCampaignName,
  };

  this.reportInvestmentService.GetDoctorCampaingWiseInvestment(investmentReportSearchDto).subscribe(resp => {
    // this.reportInvestmentService.getInsSocietyBCDSWiseInvestment().subscribe(resp => {  
    this.docCampWiseInvestment = resp as IDocCampWiseInvestment[];
    if (this.docCampWiseInvestment.length <= 0) {
      this.toastr.warning('No Data Found', 'Report');
    }
    if (this.docCampWiseInvestment.length > 0) {
      for (let p of this.docCampWiseInvestment) {
        var fD = this.datePipe.transform(p.fromDate, "dd/MM/yyyy")
        var tD = this.datePipe.transform(p.toDate, "dd/MM/yyyy")
      }
    }
    this.viewBrandDocWiseInvestmentReport();
  }, error => {
    console.log(error);
  });
}

/// ********************************************
/// View Brand Doctor Wise Investment Report
/// ********************************************

viewBrandDocWiseInvestmentReport() {
  debugger;
  if (this.docCampWiseInvestment.length <= 0) {
    return false;
  }

  const r =  this.docCampWiseInvestment as IDocCampWiseInvestment[];

  let row: any[] = [];
  let rowD: any[] = [];
  let col = ['SBU Name', 'Location', 'Brand' , 'Camp.' , 'Sub Camp.' , 'Doctor Code','Doctor Name','Institution',
     'Doc \nCategory' , 'No Of \nPatient' ,'Invst \nPrev. Yr', 'Invst \nCurnt. Yr', 'Duration', 'Commit.', 'Act \nShare' ]; // initialization for headers

  let slNO = 0;
  for (const a of r) {
    console.log(r);
    //row.push(++slNO);
    row.push(a.sbuName);
    row.push(a.marketName + ", " + a.territoryName + ", \n" + a.regionName + ", " + a.divisionName + ", \n" + a.zoneName);
    row.push(a.brand);
    row.push(a.campaign);
    row.push(a.subCampaign);

    row.push(a.doctorId);
    row.push(a.doctorName);
    row.push(a.institutionName);
    row.push(a.doctorCategory);
    row.push(a.noOfPatient);
    row.push(a.investmentPast);
    row.push(a.investmentPresent);

    const convertedfDate = new Date(a.fromDate);
    let fd = '';
    fd += convertedfDate.getDate() + '/' + (convertedfDate.getMonth() + 1) + '/' + convertedfDate.getFullYear();

    const convertedtDate = new Date(a.toDate);
    let td = '';
    td += convertedtDate.getDate() + '/' + (convertedtDate.getMonth() + 1) + '/' + convertedtDate.getFullYear();
    row.push(fd + ' - ' + td);

    row.push(a.commitment);
    row.push(a.actualShare);

    rowD.push(row);
    row = [];
  }
  //this.getReport(col, rowD, title, orgName, orgAddress);
  this.getReport(col, rowD, 'Brand Doctor Wise Investment Report', 'Square Pharmaceuticals Ltd.', '48, Square Center, Mohakhali');
}


/// ********************************************
/// Generate Campaign / Sub Campaign Wise Investment Report
/// ********************************************

getCampSubCampWiseInvestmentReport()  {
  const investmentReportSearchDto: IInvestmentReportSearchDto = {
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
    brandCode: this.investmentSearchForm.value.brandCode,
    campaignName: this.investmentSearchForm.value.campaignName,
    subCampaignName: this.investmentSearchForm.value.subCampaignName,
  };

  this.reportInvestmentService.GetDoctorCampaingWiseInvestment(investmentReportSearchDto).subscribe(resp => {
    // this.reportInvestmentService.getInsSocietyBCDSWiseInvestment().subscribe(resp => {  
    this.docCampWiseInvestment = resp as IDocCampWiseInvestment[];
    if (this.docCampWiseInvestment.length <= 0) {
      this.toastr.warning('No Data Found', 'Report');
    }
    if (this.docCampWiseInvestment.length > 0) {
      for (let p of this.docCampWiseInvestment) {
        var fD = this.datePipe.transform(p.fromDate, "dd/MM/yyyy")
        var tD = this.datePipe.transform(p.toDate, "dd/MM/yyyy")
      }
    }
    this.viewCampSubCampWiseInvestmentReport();
  }, error => {
    console.log(error);
  });
}

/// ********************************************
/// View Campaign / Sub Campaign Wise Investment Report
/// ********************************************

viewCampSubCampWiseInvestmentReport() {
  debugger;
  if (this.docCampWiseInvestment.length <= 0) {
    return false;
  }

  const r =  this.docCampWiseInvestment as IDocCampWiseInvestment[];

  let row: any[] = [];
  let rowD: any[] = [];
  let col = ['SBU Name', 'Location', 'Campaign' , 'Sub-Campaign' , 'Doctor Code','Doctor Name','Institution',
     'Doctor \nCategory' , 'No Of \nPatient' ,'Investment \nPrev. Yr', 'Investment \nCurnt. Yr', 'Duration', 'Commitment', 'Actual \nShare' ]; // initialization for headers

  let slNO = 0;
  for (const a of r) {
    console.log(r);
    //row.push(++slNO);
    row.push(a.sbuName);
    row.push(a.marketName + ", " + a.territoryName + ", \n" + a.regionName + ", " + a.divisionName + ", \n" + a.zoneName);
    row.push(a.campaign);
    row.push(a.subCampaign);

    row.push(a.doctorId);
    row.push(a.doctorName);
    row.push(a.institutionName);
    row.push(a.doctorCategory);
    row.push(a.noOfPatient);
    row.push(a.investmentPast);
    row.push(a.investmentPresent);

    const convertedfDate = new Date(a.fromDate);
    let fd = '';
    fd += convertedfDate.getDate() + '/' + (convertedfDate.getMonth() + 1) + '/' + convertedfDate.getFullYear();

    const convertedtDate = new Date(a.toDate);
    let td = '';
    td += convertedtDate.getDate() + '/' + (convertedtDate.getMonth() + 1) + '/' + convertedtDate.getFullYear();
    row.push(fd + ' - ' + td);

    row.push(a.commitment);
    row.push(a.actualShare);

    rowD.push(row);
    row = [];
  }
  //this.getReport(col, rowD, title, orgName, orgAddress);
  this.getReport(col, rowD, 'Campaign / Sub Campaign Wise Investment Report', 'Square Pharmaceuticals Ltd.', '48, Square Center, Mohakhali');
}

/// ********************************************
/// Generate SBU wise Investment And Commitment vs Share (Individual Doctor)
/// ********************************************

getSBUWiseInvestCommitShareIndivDocReport()  {
  const investmentReportSearchDto: IInvestmentReportSearchDto = {
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
    brandCode: this.investmentSearchForm.value.brandCode,
    campaignName: this.investmentSearchForm.value.campaignName,
    subCampaignName: this.investmentSearchForm.value.subCampaignName,
  };

  this.reportInvestmentService.GetDoctorCampaingWiseInvestment(investmentReportSearchDto).subscribe(resp => {
    // this.reportInvestmentService.getInsSocietyBCDSWiseInvestment().subscribe(resp => {  
    this.docCampWiseInvestment = resp as IDocCampWiseInvestment[];
    if (this.docCampWiseInvestment.length <= 0) {
      this.toastr.warning('No Data Found', 'Report');
    }
    if (this.docCampWiseInvestment.length > 0) {
      for (let p of this.docCampWiseInvestment) {
        var fD = this.datePipe.transform(p.fromDate, "dd/MM/yyyy")
        var tD = this.datePipe.transform(p.toDate, "dd/MM/yyyy")
      }
    }
    this.viewSBUWiseInvestCommitShareIndivDocReport();
  }, error => {
    console.log(error);
  });
}



/// ********************************************
/// View SBU wise Budget And Expense
/// ********************************************

viewSBUWiseInvestCommitShareIndivDocReport() {
  debugger;
  if (this.docCampWiseInvestment.length <= 0) {
    return false;
  }

  const r =  this.docCampWiseInvestment as IDocCampWiseInvestment[];

  let row: any[] = [];
  let rowD: any[] = [];
  let col = ['SBU Name', 'Brand','Campaign' , 'Sub-Campaign' , 'Doctor Code','Doctor Name','Institution',
     'Doctor \nCategory' , 'No Of \nPatient' ,'Investment \nPrev. Yr', 'Investment \nCurnt. Yr', 'Commitment \n(No of Rx)', 'Territory', 'Region' , 'Zone' ]; // initialization for headers

  let slNO = 0;
  for (const a of r) {
    console.log(r);
    //row.push(++slNO);
    row.push(a.sbuName);
    row.push(a.brand);
    row.push(a.campaign);
    row.push(a.subCampaign);

    row.push(a.doctorId);
    row.push(a.doctorName);
    row.push(a.institutionName);
    row.push(a.doctorCategory);
    row.push(a.noOfPatient);
    row.push(a.investmentPast);
    row.push(a.investmentPresent);

    row.push(a.noOfPresc);
    row.push(a.territoryName );
    row.push(a.regionName);
    row.push(a.zoneName);

    rowD.push(row);
    row = [];
  }
  //this.getReport(col, rowD, title, orgName, orgAddress);
  this.getReport(col, rowD, 'SBU wise Investment And Commitment vs Share (Individual Doctor)', 'Square Pharmaceuticals Ltd.', '48, Square Center, Mohakhali');
}

/// ********************************************
/// Generate SBU wise Investment And Commitment vs Share (Individual Doctor)
/// ********************************************

getDocWiseInvestAllPortfolioReport()  {
  const investmentReportSearchDto: IInvestmentReportSearchDto = {
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
    brandCode: this.investmentSearchForm.value.brandCode,
    campaignName: this.investmentSearchForm.value.campaignName,
    subCampaignName: this.investmentSearchForm.value.subCampaignName,
  };

  this.reportInvestmentService.GetDoctorCampaingWiseInvestment(investmentReportSearchDto).subscribe(resp => {
    // this.reportInvestmentService.getInsSocietyBCDSWiseInvestment().subscribe(resp => {  
    this.docCampWiseInvestment = resp as IDocCampWiseInvestment[];
    if (this.docCampWiseInvestment.length <= 0) {
      this.toastr.warning('No Data Found', 'Report');
    }
    if (this.docCampWiseInvestment.length > 0) {
      for (let p of this.docCampWiseInvestment) {
        var fD = this.datePipe.transform(p.fromDate, "dd/MM/yyyy")
        var tD = this.datePipe.transform(p.toDate, "dd/MM/yyyy")
      }
    }
    this.viewDocWiseInvestAllPortfolioReport();
  }, error => {
    console.log(error);
  });
}

/// ********************************************
/// View Doctor Wise Invest in All Portfolio (Not Fixed, Column Missing)
/// ********************************************

viewDocWiseInvestAllPortfolioReport() {
  debugger;
  if (this.docCampWiseInvestment.length <= 0) {
    return false;
  }

  const r =  this.docCampWiseInvestment as IDocCampWiseInvestment[];

  let row: any[] = [];
  let rowD: any[] = [];
  let col = ['Brand', 'Doctor Code','Doctor Name', 'Campaign' , 'Sub-Campaign' , 'Investment Amount', 
  'Rx Share \n(All)', 'Rx Share \n(Brand)', 'Investment \nPrev. Yr', 'Investment \nCurnt. Yr', 'Commitment \n(No of Rx)', 'Territory', 'Region' , 'Zone' ]; // initialization for headers

  let slNO = 0;
  for (const a of r) {
    console.log(r);

    row.push(a.brand);
    row.push(a.doctorId);
    row.push(a.doctorName);
    row.push(a.campaign);
    row.push(a.subCampaign);
    row.push(a.investmentPresent);

    row.push(a);
    row.push(a.investmentPresent);

    row.push(a.noOfPatient);
    row.push(a.investmentPast);
    row.push(a.investmentPresent);

    row.push(a.noOfPresc);
    row.push(a.territoryName );
    row.push(a.regionName);
    row.push(a.zoneName);

    rowD.push(row);
    row = [];
  }
  //this.getReport(col, rowD, title, orgName, orgAddress);
  this.getReport(col, rowD, 'Doctor Wise Invest in All Portfolio', 'Square Pharmaceuticals Ltd.', '48, Square Center, Mohakhali');
}

/// ********************************************
/// Generate SBU wise Budget And Expense
/// ********************************************

getSBUWiseBudgetInvest()  {
  const investmentReportSearchDto: IInvestmentReportSearchDto = {
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
    brandCode: this.investmentSearchForm.value.brandCode,
    campaignName: this.investmentSearchForm.value.campaignName,
    subCampaignName: this.investmentSearchForm.value.subCampaignName,
  };

  this.reportInvestmentService.GetSBUWiseExpSummaryReport(investmentReportSearchDto).subscribe(resp => {
    // this.reportInvestmentService.getInsSocietyBCDSWiseInvestment().subscribe(resp => {  
    this.sBUWiseExpSummaryReport = resp as ISBUWiseExpSummaryReport[];
    debugger;
    if (this.sBUWiseExpSummaryReport.length <= 0) {
      this.toastr.warning('No Data Found', 'Report');
    }
  
    this.viewSBUWiseBudgetInvest();
  }, error => {
    console.log(error);
  });
}

/// ********************************************
/// View SBU wise Budget And Expense
/// ********************************************

viewSBUWiseBudgetInvest() {

  if (this.sBUWiseExpSummaryReport.length <= 0) {
    return false;
  }
  debugger;
  const r =  this.sBUWiseExpSummaryReport as ISBUWiseExpSummaryReport[];

  let row: any[] = [];
  let rowD: any[] = [];
  let col = ['SBU Name', 'Donation Type','Budget' , 'Expense', 'Remaining' ]; // initialization for headers

  let slNO = 0;

  for (const a of r) {
 
    row.push(a.sbuName);
    row.push(a.donationTypeName);
    row.push(a.budget);
    row.push(a.expense);
    row.push(a.budget - a.expense);

    rowD.push(row);
    row = [];
  }
  //this.getReport(col, rowD, title, orgName, orgAddress);
  this.getReport(col, rowD, 'SBU wise Budget And Expense Summary Report', 'Square Pharmaceuticals Ltd.', '48, Square Center, Mohakhali');
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
    pdf.text(': ' + title , 150, 100);
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
     // pdf.text(title, 100, pageHeight - 10); 
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
        styles: { overflow: 'linebreak', cellWidth: 'auto', fontSize: 9, textColor: 0 },
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
  brandCode: string,
  campaignName: string,
  subCampaignName: string,
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