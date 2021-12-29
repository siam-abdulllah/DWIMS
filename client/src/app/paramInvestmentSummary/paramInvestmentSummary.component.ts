import { rptInvestSummaryPagination, IrptInvestSummaryPagination } from '../shared/models/rptInvestSummaryPagination';
import { IrptInvestSummary, rptInvestSummary } from '../shared/models/rptInvestSummary';
import { Component, ElementRef, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { environment } from '../../environments/environment';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from "ngx-spinner";
import { RptInvestSummaryService } from '../_services/report-investsummary.service';
import { GenericParams } from '../shared/models/genericParams';
import { AccountService } from '../account/account.service';
import { InvestmentInit } from '../shared/models/investmentRec';
import { DatePipe } from '@angular/common';


@Component({
  selector: 'paramInvestmentSummary',
  templateUrl: './paramInvestmentSummary.component.html',
  styles: [
  ]
})

export class ParamInvestSummaryComponent implements OnInit {
  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  @ViewChild('fromDate') fromDate: ElementRef;
  @ViewChild('toDate') toDate: ElementRef;
  genParams: GenericParams;
  empId: string;
  searchText = '';
  configs: any;
  searchDto: IReportSearchDto;
  numberPattern = "^[0-9]+(.[0-9]{1,10})?$";
  totalCount = 0;
  approveStatus : any;
  reports: IrptInvestSummary[] = [];
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
    public reportService: RptInvestSummaryService,
    private toastr: ToastrService, private modalService: BsModalService,
    private SpinnerService: NgxSpinnerService,private accountService: AccountService,) { }

  ngOnInit() {
    debugger;
    var url_string = window.location.href
    var url = new URL(url_string);
    var v=url.pathname.split("/");

    this.approveStatus = v[3];

    this.resetForm();
    this.getEmployeeId();
    this.bsConfig = Object.assign({}, { containerClass: 'theme-blue' }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();
    this.configs = {
      currentPage: 1,
      itemsPerPage: 10,
      totalItems:50,
      };
      this.GetData(v[3]);
  }


  GetData(param)
  {
    debugger;
    this.date=new Date();
    let latest_date =this.datepipe.transform(this.date, 'yyyy-MM-dd');

    this.reportService.rptInvestSummaryFormData.fromDate = new Date(new Date().getFullYear(), 0, 1);;
    this.reportService.rptInvestSummaryFormData.toDate = this.date;
    // if(param== "Approved")
    // {
    //   alert('Approved');
    // }
    // if(param== "Pending")
    // {
    //   alert('Pending');
    // }

    this.ViewData();
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
    const  searchDto: IParamReportSearchDto = {
      fromDate: this.reportService.rptInvestSummaryFormData.fromDate,
      toDate: this.reportService.rptInvestSummaryFormData.toDate,
      userRole:this.userRole,
      empId:this.empId,
      approveStatus:this.approveStatus
    };

    this.reportService.GetParamInvestmentSummaryReport(searchDto).subscribe(response => {
      const params = this.reportService.getGenParams();
      this.reports = response.data;
      this.totalCount = response.count;
      this.configs = {
        currentPage: params.pageIndex,
        itemsPerPage: params.pageSize,
        totalItems:this.totalCount,
        };
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
    this.router.navigate([]).then(result => {  window.open('/portal/rptInvestmentDetail/'+selectedRecord.id, '_blank'); });;
  }

  resetSearch(){
    this.searchText = '';
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
}

interface IReportSearchDto {
  fromDate: Date | undefined | null;
  toDate: Date | undefined | null;
  userRole:string;
  empId:string;
}

interface IParamReportSearchDto {
  fromDate: Date | undefined | null;
  toDate: Date | undefined | null;
  userRole:string;
  empId:string;
  approveStatus:string;
}