
import { Component, ElementRef, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from "ngx-spinner";

import { GenericParams } from './../shared/models/genericParams';

@Component({
  selector: 'app-campaign',
  templateUrl: './campaign.component.html',
  styles: [
  ]
})
export class CampaignComponent implements OnInit {
  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  // @ViewChild('campaignMstSearchModal', { static: false }) campaignMstSearchModal: TemplateRef<any>;
  // @ViewChild('productSearchModal', { static: false }) productSearchModal: TemplateRef<any>;
  @ViewChild('StartDate') StartDate: ElementRef;
  @ViewChild('EndDate') EndDate: ElementRef;
  campaignMstSearchodalRef: BsModalRef;
  productSearchModalRef: BsModalRef;
  genParams: GenericParams;
  searchText = '';
  configs: any;
  numberPattern = "^[0-9]+(.[0-9]{1,10})?$";
  totalCount = 0;
  bsConfig: Partial<BsDatepickerConfig>;
  bsValue: Date = new Date();
  config = {
    keyboard: false,
    class: 'modal-lg',
    ignoreBackdropClick: true
  };
  constructor(private router: Router,
    private toastr: ToastrService, private modalService: BsModalService,
    private SpinnerService: NgxSpinnerService) { }

  ngOnInit() {
    this.resetForm();

    //this.getProduct();
    this.bsConfig = Object.assign({}, { containerClass: 'theme-blue' }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();
  }

  
  openCampaignMstSearchModal(template: TemplateRef<any>) {
    this.campaignMstSearchodalRef = this.modalService.show(template, this.config);
  }

  // dateCompare() {
  //   if (this.masterService.campaignDtlFormData.StartDate != null && this.masterService.campaignDtlFormData.EndDate != null) {
  //     if (this.masterService.campaignDtlFormData.EndDate > this.masterService.campaignDtlFormData.StartDate) {
  //       return true;
  //     }
  //     else {
  //       //form.controls.StartDate.setValue(null);
  //       //form.controls.EndDate.setValue(null);
  //       this.toastr.error('Select Appropriate Date Range', 'Error');
  //       return false;
  //     }
  //   }
  // }



  // onPageChanged(event: any){
  //   const params = this.masterService.getGenParams();
  //   if (params.pageIndex !== event)
  //   {
  //     params.pageIndex = event;
  //     this.masterService.setGenParams(params);
  //     this.getCampaign();
  //   }
  // }
  
  // onSearch(){
  //   const params = this.masterService.getGenParams();
  //   params.search = this.searchTerm.nativeElement.value;
  //   params.pageIndex = 1;
  //   this.masterService.setGenParams(params);
  //   this.getCampaign();
  // }

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
