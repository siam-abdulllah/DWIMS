import { rptInvestSummaryPagination, IrptInvestSummaryPagination } from '../shared/models/rptInvestSummaryPagination';
import { IrptInvestSummary, rptInvestSummary } from '../shared/models/rptInvestSummary';
import { Component, ElementRef, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from "ngx-spinner";
import { RptInvestSummaryService } from '../_services/report-investsummary.service';
import { GenericParams } from '../shared/models/genericParams';

@Component({
  selector: 'rptInvestmentSummary',
  templateUrl: './rptInvestmentSummary.component.html',
  styles: [
  ]
})

export class RptInvestSummaryComponent implements OnInit {
  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  @ViewChild('fromDate') fromDate: ElementRef;
  @ViewChild('toDate') toDate: ElementRef;
  genParams: GenericParams;
  searchText = '';
  configs: any;
  searchDto: IReportSearchDto;
  numberPattern = "^[0-9]+(.[0-9]{1,10})?$";
  totalCount = 0;
  reports: IrptInvestSummary[] = [];
  bsConfig: Partial<BsDatepickerConfig>;
  bsValue: Date = new Date();
  config = {
    keyboard: false,
    class: 'modal-lg',
    ignoreBackdropClick: true
  };

  constructor(private router: Router,
    public reportService: RptInvestSummaryService,
    private toastr: ToastrService, private modalService: BsModalService,
    private SpinnerService: NgxSpinnerService) { }

  ngOnInit() {
    this.resetForm();
    this.bsConfig = Object.assign({}, { containerClass: 'theme-blue' }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();
    this.configs = {
      currentPage: 1,
      itemsPerPage: 10,
      totalItems:50,
      };
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
    const  searchDto: IReportSearchDto = {
      fromDate: this.reportService.rptInvestSummaryFormData.fromDate,
      toDate: this.reportService.rptInvestSummaryFormData.toDate,
    };

    this.reportService.GetInvestmentSummaryReport(searchDto).subscribe(response => {
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
}