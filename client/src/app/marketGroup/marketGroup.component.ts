
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
  marketGroupSearchModalRef: BsModalRef;
  
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
       debugger;
      this.marketGroupMsts = response.data;
      this.openMarketGroupSearchModal(this.marketGroupSearchModal);
     }, error => {
         console.log(error);
    });
  }
  openMarketGroupSearchModal(template: TemplateRef<any>) {
    this.marketGroupSearchModalRef = this.modalService.show(template, this.config);
  }
  getMarketGroups(){
     this.marketGroupService.getMarketGroups(this.marketGroupService.marketGroupFormData.id).subscribe(response => {
      this.marketGroupDtls = response as IMarketGroupDtl[];
     }, error => {
         console.log(error);
    });
  }
  onSubmit(form: NgForm) {
    
    if (this.marketGroupService.marketGroupFormData.id == 0)
      this.insertMarketGroup(form);
    else
      this.updateMarketGroup(form);
  }

  insertMarketGroup(form: NgForm) {
    this.marketGroupService.insertMarketGroup().subscribe(
      res => {
        debugger;
        this.marketGroupService.marketGroupFormData=res as IMarketGroupMst;
        this.toastr.success('Submitted successfully', 'Payment Detail Register')
      },
      err => { console.log(err); }
    );
  }

  updateMarketGroup(form: NgForm) {
    this.marketGroupService.updateMarketGroup().subscribe(
      res => {
        debugger;
        this.marketGroupService.marketGroupFormData=res as IMarketGroupMst;
        this.toastr.success('Submitted successfully', 'Payment Detail Register')
      },
      err => { console.log(err); }
    );
  
  }
  
addMarket() {
  debugger;
  
  //var e = (document.getElementById("marketCode") as HTMLInputElement).value;
  //var f = document.getElementById('marketCode');
  var e = (document.getElementById("marketCode")) as HTMLSelectElement;
    var sel = e.selectedIndex;
    var opt = e.options[sel];
    var selectedMarketCode = opt.value;
    var selectedMarketName = opt.innerHTML;
  if(this.marketGroupService.marketGroupFormData.id===0 || this.marketGroupService.marketGroupFormData.id===undefined)
  {
    alert("Please Insert group first!")
    return false;
  }
  this.marketGroupService.insertMarketGroupDtl(this.marketGroupService.marketGroupFormData.id,selectedMarketCode,selectedMarketName).subscribe(response => {
   debugger;
   this.getMarketGroups();
   }, error => {
     console.log(error);
  });
}
  selectMarketGroup(selectedRecord: IMarketGroupMst) {
   this.marketGroupService.marketGroupFormData = Object.assign({}, selectedRecord);
   this.marketGroupSearchModalRef.hide()
  }
  
  resetPage(form: NgForm) {
    form.reset();
    
  }
  
}
