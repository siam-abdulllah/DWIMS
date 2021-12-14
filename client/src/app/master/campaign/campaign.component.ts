import { CampaignMst, ICampaignMst, CampaignDtl, ICampaignDtl, CampaignDtlProduct, ICampaignDtlProduct } from '../../shared/models/campaign';
import { SBU, ISBU } from '../../shared/models/sbu';
import { Brand, IBrand } from '../../shared/models/brand';
import { SubCampaign, ISubCampaign } from '../../shared/models/subCampaign';
import { Product, IProduct } from '../../shared/models/product';
import { GenericParams } from '../../shared/models/genericParams';
import { Component, ElementRef, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { MasterService } from '../master.service';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from "ngx-spinner";
@Component({
  selector: 'app-campaign',
  templateUrl: './campaign.component.html',
  styles: [
  ]
})
export class CampaignComponent implements OnInit {
  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  @ViewChild('campaignMstSearchModal', { static: false }) campaignMstSearchModal: TemplateRef<any>;
  @ViewChild('productSearchModal', { static: false }) productSearchModal: TemplateRef<any>;
  @ViewChild('subCampStartDate') SubCampStartDate: ElementRef;
  @ViewChild('subCampEndDate') SubCampEndDate: ElementRef;
  campaignMstSearchodalRef: BsModalRef;
  productSearchModalRef: BsModalRef;
  genParams: GenericParams;
  campaignMsts: ICampaignMst[];
  searchText = '';
  configs: any;
  campaignDtls: ICampaignDtl[];
  campaignDtlProducts: ICampaignDtlProduct[];
  SBUs: ISBU[];
  brands: IBrand[];
  numberPattern = "^[0-9]+(.[0-9]{1,10})?$";
  subCampaigns: ISubCampaign[];
  products: IProduct[];
  totalCount = 0;
  bsConfig: Partial<BsDatepickerConfig>;
  bsValue: Date = new Date();
  config = {
    keyboard: false,
    class: 'modal-lg',
    ignoreBackdropClick: true
  };
  constructor(public masterService: MasterService, private router: Router,
    private toastr: ToastrService, private modalService: BsModalService,
    private SpinnerService: NgxSpinnerService) { }

  ngOnInit() {
    this.resetForm();
    this.getSBU();
    //this.getBrand();
    this.getSubCampaign();
    //this.getProduct();
    this.bsConfig = Object.assign({}, { containerClass: 'theme-blue' }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();
  }
  getSBU() {
    this.masterService.getSBU().subscribe(response => {
      this.SBUs = response as ISBU[];
    }, error => {
      console.log(error);
    });
  }
  getBrand() {
    this.masterService.getBrand(this.masterService.campaignMstFormData.sbu).subscribe(response => {
      this.brands = response as IBrand[];
    }, error => {
      console.log(error);
    });
  }
  getProduct() {
    this.SpinnerService.show();
    this.masterService.getProduct(this.masterService.campaignMstFormData.brandCode, this.masterService.campaignMstFormData.sbu).subscribe(response => {
      debugger;
      this.products = response as IProduct[];
    }, error => {
      console.log(error);
    });
    this.SpinnerService.hide();
  }
  getSubCampaign() {
    this.masterService.getSubCampaignForCamp().subscribe(response => {
      this.subCampaigns = response as ISubCampaign[];
    }, error => {
      console.log(error);
    });
  }
  getCampaign() {
    const params = this.masterService.getGenParams();
    this.SpinnerService.show();
    this.masterService.getCampaign().subscribe(response => {
      this.SpinnerService.hide();
      this.campaignMsts = response.data;
      this.totalCount = response.count;
      this.configs = {
        currentPage: params.pageIndex,
        itemsPerPage: params.pageSize,
        totalItems: this.totalCount,
      };

      if (this.campaignMsts.length > 0) {
        if (params.pageIndex == 1) {
          this.openCampaignMstSearchModal(this.campaignMstSearchModal);
        }
      }
      else {
        this.toastr.warning('No Data Found');
      }
    }, error => {
      this.SpinnerService.hide();
      console.log(error);
    });
  }


  openCampaignMstSearchModal(template: TemplateRef<any>) {
    this.campaignMstSearchodalRef = this.modalService.show(template, this.config);
  }

  dateCompare() {
    if (this.masterService.campaignDtlFormData.subCampStartDate != null && this.masterService.campaignDtlFormData.subCampEndDate != null) {
      if (this.masterService.campaignDtlFormData.subCampEndDate > this.masterService.campaignDtlFormData.subCampStartDate) {
        return true;
      }
      else {
        //form.controls.subCampStartDate.setValue(null);
        //form.controls.subCampEndDate.setValue(null);
        this.toastr.error('Select Appropriate Date Range', 'Error');
        return false;
      }
    }
  }


  getCampaignDtl() {
    this.campaignDtls = [];
    this.masterService.getCampaignDtl(this.masterService.campaignMstFormData.id).subscribe(response => {
      debugger;
      this.campaignDtls = response.data;
      // for(var i=0;this.campaignDtls.length<1;i++)
      // {
      //   const sDate = new Date(this.campaignDtls[i].subCampStartDate);
      //   this.campaignDtls[i].subCampStartDate=new Date(sDate.getDate() + '/' + (sDate.getMonth() + 1) + '/' + sDate.getFullYear());
      //   const eDate = new Date(this.campaignDtls[i].subCampEndDate);
      //   this.campaignDtls[i].subCampEndDate=new Date(eDate.getDate() + '/' + (eDate.getMonth() + 1) + '/' + eDate.getFullYear());
      //  }
      this.totalCount = response.count;
    }, error => {
      console.log(error);
    });
  }
  getCampaignDtlProduct() {
    this.campaignDtlProducts = [];
    this.masterService.getCampaignDtlProduct(this.masterService.campaignDtlFormData.id).subscribe(response => {
      debugger;
      this.campaignDtlProducts = response.data;
      this.totalCount = response.count;
    }, error => {
      console.log(error);
    });
  }
  getCampaignDtlProductClick(selectedRecord: ICampaignDtl) {
    this.masterService.campaignDtlFormData = Object.assign({}, selectedRecord);
    this.masterService.campaignDtlFormData.subCampStartDate = new Date(selectedRecord.subCampStartDate);
    this.masterService.campaignDtlFormData.subCampEndDate = new Date(selectedRecord.subCampEndDate);

    this.getCampaignDtlProduct();
  }
  onSubmit(form: NgForm) {
    debugger;
    if (this.masterService.campaignMstFormData.id == 0)
      this.insertCampaign(form);
    else
      this.updateCampaign(form);
  }

  insertCampaign(form: NgForm) {

    this.masterService.insertCampaignMst().subscribe(
      res => {
        debugger;
        //this.resetForm(form);
        //this.getCampaign();

        this.masterService.campaignMstFormData = res as ICampaignMst;
        this.toastr.success('Submitted successfully', 'Campaign Info')
      },
      err => { console.log(err); }
    );
  }

  updateCampaign(form: NgForm) {

    this.masterService.updateCampaignMst().subscribe(
      res => {
        this.masterService.campaignMstFormData = res as ICampaignMst;
        this.toastr.info('Updated successfully', 'Campaign Info')
      },
      err => { console.log(err); }
    );
  }

  populateForm(selectedRecord: ICampaignMst) {
    this.masterService.campaignMstFormData = Object.assign({}, selectedRecord);
  }
  populateDtlsForm(selectedRecord: ICampaignDtl) {
    this.masterService.campaignDtlFormData = Object.assign({}, selectedRecord);
    this.masterService.campaignDtlFormData.subCampStartDate = new Date(selectedRecord.subCampStartDate);
    this.masterService.campaignDtlFormData.subCampEndDate = new Date(selectedRecord.subCampEndDate);


  }
  addSubCampaign() {
    debugger;
    if (this.masterService.campaignMstFormData.id == 0) {
      this.toastr.warning('Please Insert Campaign Data First', 'Campaign');
      return false;
    }
    if (this.masterService.campaignDtlFormData.subCampaignId == null || this.masterService.campaignDtlFormData.subCampaignId == undefined) {
      this.toastr.warning('Please Select Sub-Campaign First', 'Campaign');
      return false;
    }
    if (this.masterService.campaignDtlFormData.budget == "" || this.masterService.campaignDtlFormData.budget == null || this.masterService.campaignDtlFormData.budget == undefined) {
      this.toastr.warning('Select Budget First', 'Campaign');
      return false;
    }
    if (this.masterService.campaignDtlFormData.subCampStartDate == null || this.masterService.campaignDtlFormData.subCampStartDate == undefined) {
      this.toastr.warning('Select Sub-Campaign Start Date ', 'Campaign');
      return false;
    }
    if (this.masterService.campaignDtlFormData.subCampEndDate == null || this.masterService.campaignDtlFormData.subCampEndDate == undefined) {
      this.toastr.warning('Select Sub-Campaign End Date', 'Campaign');
      return false;
    }

    //this.masterService.campaignDtlFormData.subCampStartDate = new Date(this.masterService.campaignDtlFormData.subCampStartDate);
    //this.masterService.campaignDtlFormData.subCampEndDate = new Date(this.masterService.campaignDtlFormData.subCampEndDate);
    var StartDate = this.masterService.campaignDtlFormData.subCampStartDate;
    var EndDate = this.masterService.campaignDtlFormData.subCampEndDate;

    //this.masterService.campaignDtlFormData.subCampStartDate = new Date(StartDate.setDate(StartDate.getDate()+1));
    //this.masterService.campaignDtlFormData.subCampEndDate = new Date(EndDate.setDate(EndDate.getDate() + 1));

    this.masterService.campaignDtlFormData.mstId = this.masterService.campaignMstFormData.id;
    if (this.masterService.campaignDtlFormData.id == 0) {
      for (var i = 0; i < this.campaignDtls.length; i++) {
        if (this.campaignDtls[i].subCampaignId == this.masterService.campaignDtlFormData.subCampaignId) {
          this.toastr.warning(' Sub-Campaign Already Exist', 'Campaign');
          return false;
        }
      }
      if (this.dateCompare()) {
        this.masterService.insertCampaignDtl().subscribe(
          res => {
            this.masterService.campaignDtlFormData = new CampaignDtl();
            this.getCampaignDtl();
            this.toastr.info('Insert successfully', 'Campaign Info')
          },
          err => { console.log(err); }
        );
      }
    }
    else {
      if (this.dateCompare()) {
        this.masterService.updateCampaignDtl().subscribe(
          res => {
            this.masterService.campaignDtlFormData = new CampaignDtl();
            this.getCampaignDtl();
            this.toastr.info('Updated successfully', 'Campaign Info')
          },
          err => { console.log(err); }
        );
      }
    }
  }
  addProduct(selectedRecord: ICampaignDtl) {
    if (this.masterService.campaignMstFormData.id == 0) {
      this.toastr.warning('Please Insert Campaign Data First', 'Campaign');
      return false;
    }
    if (this.masterService.campaignDtlFormData.id == 0) {
      this.toastr.warning('Please Insert Sub-Campaign Data First', 'Campaign');
      return false;
    }
    if (this.masterService.campaignDtlProductFormData.productId == 0 || this.masterService.campaignDtlProductFormData.productId == null || this.masterService.campaignDtlProductFormData.productId == undefined) {
      this.toastr.warning('Please Select Product', 'Campaign');
      return false;
    }
    this.masterService.campaignDtlProductFormData.dtlId = this.masterService.campaignDtlFormData.id;
    if (this.campaignDtlProducts.length > 0) {
      for (var i = 0; i < this.campaignDtlProducts.length; i++) {
        if (this.campaignDtlProducts[i].productId == this.masterService.campaignDtlProductFormData.productId) {
          this.toastr.warning('Product already exist', 'Campaign');
          return false;
        }
      }
    }
    if (this.masterService.campaignDtlProductFormData.id == 0) {
      this.masterService.insertCampaignDtlProduct().subscribe(
        res => {
          this.masterService.campaignDtlProductFormData = new CampaignDtlProduct();
          this.getCampaignDtlProduct();
          this.toastr.info('Insert successfully', 'Campaign Info');
          this.productSearchModalRef.hide()
        },
        err => { console.log(err); }
      );
    }
    else {
      this.masterService.updateCampaignDtl().subscribe(
        res => {
          this.masterService.campaignDtlFormData = new CampaignDtl();
          this.getCampaignDtl();
          this.toastr.info('Updated successfully', 'Campaign Info')
        },
        err => { console.log(err); }
      );
    }
  }
  removeProduct(selectedRecord: ICampaignDtlProduct) {
    var result = confirm("Do you want to delete?");
    if (result) {
      this.masterService.removeDtlProduct(selectedRecord).subscribe(
        res => {
          this.masterService.campaignDtlProductFormData = new CampaignDtlProduct();
          this.getCampaignDtlProduct();
          this.toastr.info('Deleted successfully', 'Campaign Info');
          this.productSearchModalRef.hide()
        },
        err => { console.log(err); }
      );
    }
  }


  onPageChanged(event: any) {
    const params = this.masterService.getGenParams();
    if (params.pageIndex !== event) {
      params.pageIndex = event;
      this.masterService.setGenParams(params);
      this.getCampaign();
    }
  }

  onSearch() {
    const params = this.masterService.getGenParams();
    params.search = this.searchTerm.nativeElement.value;
    params.pageIndex = 1;
    this.masterService.setGenParams(params);
    this.getCampaign();
  }

  resetSearch() {
    this.searchText = '';
  }


  showProductModal(selectedRecord: ICampaignDtl) {
    this.masterService.campaignDtlFormData = Object.assign({}, selectedRecord);
    this.masterService.campaignDtlFormData.subCampStartDate = new Date(selectedRecord.subCampStartDate);
    this.masterService.campaignDtlFormData.subCampEndDate = new Date(selectedRecord.subCampEndDate);
    this.getCampaignDtlProduct();
    this.openProductSearchModal(this.productSearchModal);
  }
  openProductSearchModal(template: TemplateRef<any>) {
    this.productSearchModalRef = this.modalService.show(template, this.config);
  }
  selectCampaignMst(selectedRecord: ICampaignMst) {
    this.masterService.campaignMstFormData = Object.assign({}, selectedRecord);
    this.getBrand();
    this.getProduct();
    this.getCampaignDtl();
    this.campaignMstSearchodalRef.hide()
  }
  resetPage(form: NgForm) {
    form.form.reset();
    this.masterService.campaignMstFormData = new CampaignMst();
    this.masterService.campaignDtlFormData = new CampaignDtl();
    this.masterService.campaignDtlProductFormData = new CampaignDtlProduct();
    this.campaignMsts = [];
    this.campaignDtls = [];
    this.campaignDtlProducts = [];
    this.configs = {
      currentPage: 1,
      itemsPerPage: 10,
      totalItems: 50,
    };
  }
  resetForm() {
    this.masterService.campaignMstFormData = new CampaignMst();
    this.masterService.campaignDtlFormData = new CampaignDtl();
    this.masterService.campaignDtlProductFormData = new CampaignDtlProduct();
    this.campaignMsts = [];
    this.campaignDtls = [];
    this.campaignDtlProducts = [];
    this.configs = {
      currentPage: 1,
      itemsPerPage: 10,
      totalItems: 50,
    };
  }
}
