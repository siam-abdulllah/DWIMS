import { Campaign, ICampaign } from '../../shared/models/campaign';
import { SBU, ISBU } from '../../shared/models/sbu';
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
  campaigns: ICampaign[]; 
  SBUs: ISBU[]; 
  subCampaigns: ISubCampaign[]; 
  products: IProduct[];
  totalCount = 0;
  bsConfig: Partial<BsDatepickerConfig>;
  bsValue: Date = new Date();
  constructor(public masterService: MasterService, private router: Router,
    private toastr: ToastrService) { }

  ngOnInit() {
    this.getSBU();
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
    this.masterService.getSBU().subscribe(response => {
      this.SBUs = response as ISBU[];
    }, error => {
        console.log(error);
    });
  }
  getCampaign(){
    this.masterService.getCampaign().subscribe(response => {
      this.campaigns = response.data;
      this.totalCount = response.count;
    }, error => {
        console.log(error);
    });
  }
  onSubmit(form: NgForm) {
    debugger;
    if (this.masterService.campaignFormData.id == 0)
      this.insertCampaign(form);
    else
      this.updateCampaign(form);
  }

  insertCampaign(form: NgForm) {
    this.masterService.insertCampaign().subscribe(
      res => {
        debugger;
        this.resetForm(form);
        this.getCampaign();
        this.toastr.success('Submitted successfully', 'Payment Detail Register')
      },
      err => { console.log(err); }
    );
  }

  updateCampaign(form: NgForm) {
    this.masterService.updateSubCampaign().subscribe(
      res => {
        debugger;
        this.resetForm(form);
        this.getCampaign();
        this.toastr.info('Updated successfully', 'Payment Detail Register')
      },
      err => { console.log(err); }
    );
  }

  populateForm(selectedRecord: ICampaign) {
    this.masterService.campaignFormData = Object.assign({}, selectedRecord);
  }
  resetForm(form: NgForm) {
    form.form.reset();
    this.masterService.campaignFormData = new Campaign();
  }
}
