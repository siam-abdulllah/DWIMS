
import { SubCampaign, ISubCampaign } from '../shared/models/subCampaign';
import { Market, IMarket } from '../shared/models/market';
import { GenericParams } from '../shared/models/genericParams';
import { Component, ElementRef, OnInit, ViewChild , TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
//import { MasterService } from '../master.service';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService} from 'ngx-bootstrap/modal';
@Component({
  selector: 'app-regApproval',
  templateUrl: './regApproval.component.html',
  styles: [
  ]
})
export class RegApprovalComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
 // @ViewChild('campaignModal', { static: false }) campaignModal: TemplateRef<any>;
  // campaignModalRef: BsModalRef;
  // genParams: GenericParams;
  // campaigns: ICampaign[]; 
  // subCampaigns: ISubCampaign[]; 
  markets: IMarket[];
  totalCount = 0;
  bsConfig: Partial<BsDatepickerConfig>;
  bsValue: Date = new Date();
  //constructor(public masterService: MasterService, private router: Router,
    constructor( private router: Router,
    private toastr: ToastrService) { }

  ngOnInit() {
    //this.getCampaign();
    this.bsConfig = Object.assign({}, { containerClass: 'theme-green' }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();
  }
  getCampaign(){
    // this.masterService.getCampaign().subscribe(response => {
    //   this.campaigns = response.data;
    //   this.totalCount = response.count;
    // }, error => {
    //     console.log(error);
    // });
  }
  onSubmit(form: NgForm) {
    
    // if (this.masterService.campaignFormData.id == 0)
    //   this.insertCampaign(form);
    // else
    //   this.updateCampaign(form);
  }

  insertCampaign(form: NgForm) {
    // this.masterService.insertCampaign().subscribe(
    //   res => {
    //     debugger;
    //     this.resetForm(form);
    //     this.getCampaign();
    //     this.toastr.success('Submitted successfully', 'Payment Detail Register')
    //   },
    //   err => { console.log(err); }
    // );
  }

  updateCampaign(form: NgForm) {
    // this.masterService.updateSubCampaign().subscribe(
    //   res => {
    //     debugger;
    //     this.resetForm(form);
    //     this.getCampaign();
    //     this.toastr.info('Updated successfully', 'Payment Detail Register')
    //   },
    //   err => { console.log(err); }
    // );
  }

  populateForm() {
    //this.masterService.campaignFormData = Object.assign({}, selectedRecord);
  }
  resetForm(form: NgForm) {
    form.form.reset();
    //this.masterService.campaignFormData = new Campaign();
  }
  SBUs = [
    {id: 1, name: 'Chittagong/Chattogram' },
    {id: 2, name: 'Sonamasjid'},
    {id: 3, name: 'Benapole'},
    {id: 4, name: 'Mongla'},
    {id: 5, name: 'Hili'},
    {id: 6, name: 'Darshana'},
    {id: 7, name: 'Shahjalal International Airport'},
    {id: 8, name: 'Banglabandha'},
    {id: 9, name: 'Birol'},
    {id: 10, name: 'Rohanpur'},
    {id: 11, name: 'Vomra'},
    {id: 12, name: 'Burimari'}
  ];
}
