
import { Market, IMarket } from '../shared/models/market';
import { MarketGroupMst, IMarketGroupMst } from '../shared/models/marketGroupMst';
import { MarketGroupDtl, IMarketGroupDtl } from '../shared/models/marketGroupDtl';
import { GenericParams } from '../shared/models/genericParams';
import { Component, ElementRef, OnInit, ViewChild , TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { MarketGroupService } from '../_services/marketGroup.service';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { BsModalRef, BsModalService} from 'ngx-bootstrap/modal';
@Component({
  selector: 'app-marketGroup',
  templateUrl: './marketGroup.component.html',
  styles: [
  ]
})
export class MarketGroupComponent implements OnInit {
  @ViewChild('marketGroupSearchModal', { static: false }) marketGroupSearchModal: TemplateRef<any>;
  openMarketGroupSearchModalRef: BsModalRef;
  
  markets: IMarket[];
  marketGroupMsts: IMarketGroupMst[];
  marketGroupDtls: IMarketGroupDtl[];
  totalCount = 0;
  config = {
    keyboard: false,
    class: 'modal-lg',
    ignoreBackdropClick: true
  };
  constructor(public marketGroupService: MarketGroupService, private router: Router,
    private toastr: ToastrService,private modalService: BsModalService) { }

  ngOnInit() {
    this.getMarket();
    
  }
  getMarket(){
     this.marketGroupService.getMarkets().subscribe(response => {
      this.markets = response as IMarket[];
     }, error => {
         console.log(error);
    });
  }
  getGroups(){
     this.marketGroupService.getGroups().subscribe(response => {
      this.marketGroupMsts = response as IMarketGroupMst[];
      this.openMarketGroupSearchModal(this.marketGroupSearchModal);
     }, error => {
         console.log(error);
    });
  }
  openMarketGroupSearchModal(template: TemplateRef<any>) {
    this.openMarketGroupSearchModalRef = this.modalService.show(template, this.config);
  }
  getMarketGroups(){
     this.marketGroupService.getMarketGroups(this.marketGroupService.marketGroupFormData.id).subscribe(response => {
      this.marketGroupDtls = response as IMarketGroupDtl[];
     }, error => {
         console.log(error);
    });
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
  selectMarketGroup(selectedRecord: IMarketGroupMst) {
    
   this.marketGroupService.marketGroupFormData = Object.assign({}, selectedRecord);
  }
  
  resetPage(form: NgForm) {
    form.reset();
    
  }
  
}
