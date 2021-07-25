import { CampaignMst, ICampaignMst,CampaignDtl, ICampaignDtl,CampaignDtlProduct, ICampaignDtlProduct } from '../../shared/models/campaign';
import { SBU, ISBU } from '../../shared/models/sbu';
import { Brand, IBrand } from '../../shared/models/brand';
import { SubCampaign, ISubCampaign } from '../../shared/models/subCampaign';
import { Product, IProduct } from '../../shared/models/product';
import { GenericParams } from '../../shared/models/genericParams';
import { Component, ElementRef, OnInit, ViewChild , TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { MasterService } from '../master.service';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService} from 'ngx-bootstrap/modal';
@Component({
  selector: 'app-campaign',
  templateUrl: './campaign.component.html',
  styles: [
  ]
})
export class CampaignComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  @ViewChild('campaignModal', { static: false }) campaignModal: TemplateRef<any>;
  campaignModalRef: BsModalRef;
  genParams: GenericParams;
  campaignMsts: ICampaignMst[]; 
  campaigDtls: ICampaignDtl[]; 
  campaigDtlProducts: ICampaignDtlProduct[]; 
  SBUs: ISBU[];
  brands: IBrand[];
   
  subCampaigns: ISubCampaign[]; 
  products: IProduct[];
  totalCount = 0;
  bsConfig: Partial<BsDatepickerConfig>;
  bsValue: Date = new Date();
  constructor(public masterService: MasterService, private router: Router,
    private toastr: ToastrService) { }

  ngOnInit() {
    this.getSBU();
    this.getBrand();
    this.getSubCampaign();
    this.bsConfig = Object.assign({}, { containerClass: 'theme-green' }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();
  }
  getSBU(){
    this.masterService.getSBU().subscribe(response => {
      this.SBUs = response as ISBU[];
    }, error => {
        console.log(error);
    });
  }
  getBrand(){
    this.masterService.getBrand().subscribe(response => {
      this.brands = response as IBrand[];
    }, error => {
        console.log(error);
    });
  }
  getSubCampaign(){
    this.masterService.getSubCampaignForCamp().subscribe(response => {
      this.subCampaigns = response as ISubCampaign[];
    }, error => {
        console.log(error);
    });
  }
  getCampaign(){
    this.masterService.getCampaign().subscribe(response => {
      this.campaignMsts = response.data;
      this.totalCount = response.count;
    }, error => {
        console.log(error);
    });
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
        
        this.masterService.campaignMstFormData=res as ICampaignMst;
        this.toastr.success('Submitted successfully', 'Campaign Info')
      },
      err => { console.log(err); }
    );
  }

  updateCampaign(form: NgForm) {
    this.masterService.updateCampaignMst().subscribe(
      res => {
        this.masterService.campaignMstFormData=res as ICampaignMst;
        this.toastr.info('Updated successfully', 'Campaign Info')
      },
      err => { console.log(err); }
    );
  }

  populateForm(selectedRecord: ICampaignMst) {
    this.masterService.campaignMstFormData = Object.assign({}, selectedRecord);
  }
  addSubCampaign() {
    debugger;
    if(this.masterService.campaignMstFormData.id==0){
      return false;
    }
    if(this.masterService.campaignDtlFormData.subCampaignId==null ||this.masterService.campaignDtlFormData.subCampaignId==undefined){
      return false;
    }
    if(this.masterService.campaignDtlFormData.budget=="" ||this.masterService.campaignDtlFormData.budget==null ||this.masterService.campaignDtlFormData.budget==undefined){ return false;
    }
    if(this.masterService.campaignDtlFormData.subCampStartDate==null ||this.masterService.campaignDtlFormData.subCampStartDate==undefined){ return false;
    }
    if(this.masterService.campaignDtlFormData.subCampEndDate==null ||this.masterService.campaignDtlFormData.subCampEndDate==undefined){ return false;
    }
    //this.masterService.campaignDtlFormData.subCampStartDate = new Date(this.masterService.campaignDtlFormData.subCampStartDate);
    //this.masterService.campaignDtlFormData.subCampEndDate = new Date(this.masterService.campaignDtlFormData.subCampEndDate);
    var StartDate = this.masterService.campaignDtlFormData.subCampStartDate;
    var EndDate = this.masterService.campaignDtlFormData.subCampEndDate;

    this.masterService.campaignDtlFormData.subCampStartDate = new Date(StartDate.setDate(StartDate.getDate()+1));
    this.masterService.campaignDtlFormData.subCampEndDate = new Date(EndDate.setDate(EndDate.getDate() + 1));

    this.masterService.campaignDtlFormData.mstId=this.masterService.campaignMstFormData.id;
    if(this.masterService.campaignDtlFormData.id==0){
    this.masterService.insertCampaignDtl().subscribe(
      res => {
        this.masterService.campaignDtlFormData=new CampaignDtl();
        this.toastr.info('Insert successfully', 'Campaign Info')
      },
      err => { console.log(err); }
    );
    }
    else{
      this.masterService.updateCampaignDtl().subscribe(
        res => {
          this.masterService.campaignDtlFormData=new CampaignDtl();
          this.toastr.info('Updated successfully', 'Campaign Info')
        },
        err => { console.log(err); }
      );
    }
  }
  
  resetForm(form: NgForm) {
    form.form.reset();
    this.masterService.campaignMstFormData = new CampaignMst();
  }
}
