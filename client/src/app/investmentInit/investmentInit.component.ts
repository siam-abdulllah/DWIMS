
import { InvestmentInit, IInvestmentInit,
  InvestmentDetail,IInvestmentDetail,InvestmentTargetedProd,IInvestmentTargetedProd } from '../shared/models/investment';
import { SubCampaign, ISubCampaign } from '../shared/models/subCampaign';
import { Donation, IDonation } from '../shared/models/donation';
import { GenericParams } from '../shared/models/genericParams';
import { Component, ElementRef, OnInit, ViewChild , TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { InvestmentInitService } from '../_services/investment.service';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService} from 'ngx-bootstrap/modal';
@Component({
  selector: 'app-investmentInit',
  templateUrl: './investmentInit.component.html',
  styles: [
  ]
})
export class InvestmentInitComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
 // @ViewChild('campaignModal', { static: false }) campaignModal: TemplateRef<any>;
  // campaignModalRef: BsModalRef;
  // genParams: GenericParams;
  // campaigns: ICampaign[]; 
  // subCampaigns: ISubCampaign[]; 
  donations: IDonation[];
  totalCount = 0;
  bsConfig: Partial<BsDatepickerConfig>;
  bsValue: Date = new Date();
  constructor(public investmentInitService: InvestmentInitService, private router: Router,
    private toastr: ToastrService) { }

  ngOnInit() {
    this.getDonation();
    this.bsConfig = Object.assign({}, { containerClass: 'theme-green' }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();
  }
  getDonation(){
    this.investmentInitService.getdonations().subscribe(response => {
      this.donations = response as IDonation[];
    }, error => {
        console.log(error);
    });
  }
  onSubmit(form: NgForm) {
    // if (this.investmentInitService.campaignFormData.id == 0)
    //   this.insertCampaign(form);
    // else
    //   this.updateCampaign(form);
  }
  insertInvestment(form: NgForm) {
    // this.investmentInitService.insertCampaign().subscribe(
    //   res => {
    //     debugger;
    //     this.resetForm(form);
    //     this.getCampaign();
    //     this.toastr.success('Submitted successfully', 'Payment Detail Register')
    //   },
    //   err => { console.log(err); }
    // );
  }
  updateInvestment(form: NgForm) {
    // this.investmentInitService.updateSubCampaign().subscribe(
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
    //this.investmentInitService.campaignFormData = Object.assign({}, selectedRecord);
  }
  resetForm(form: NgForm) {
    form.form.reset();
    //this.investmentInitService.campaignFormData = new Campaign();
  }
}
