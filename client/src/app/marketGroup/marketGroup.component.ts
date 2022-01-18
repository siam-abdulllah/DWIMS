import { Market, IMarket } from '../shared/models/market';
import { MarketGroupMst, IMarketGroupMst } from '../shared/models/marketGroupMst';
import { MarketGroupDtl, IMarketGroupDtl } from '../shared/models/marketGroupDtl';
import { GenericParams } from '../shared/models/genericParams';
import { Component, ElementRef, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { MarketGroupService } from '../_services/marketGroup.service';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { AccountService } from '../account/account.service';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-marketGroup',
  templateUrl: './marketGroup.component.html',
  styles: [
  ]
})
export class MarketGroupComponent implements OnInit {
  @ViewChild('marketGroupSearchModal', { static: false }) marketGroupSearchModal: TemplateRef<any>;
  marketGroupSearchModalRef: BsModalRef;
  empSbu: string;
  sbu: string;
  sbuName: string;
  markets: IMarket[];
  searchText = '';
  marketGroupMsts: IMarketGroupMst[];
  marketGroupDtls: IMarketGroupDtl[];
  totalCount = 0;
  empId: string;
  status: any;
  marketCode: string;
  config = {
    keyboard: false,
    class: 'modal-lg',
    ignoreBackdropClick: true
  };
  constructor(private accountService: AccountService, public marketGroupService: MarketGroupService, private router: Router,
    private toastr: ToastrService, private modalService: BsModalService, private SpinnerService: NgxSpinnerService) { }

  ngOnInit() {
    this.resetForm();

    this.getEmployeeId();
  }
  getMarket(empId: string) {
    this.marketGroupService.getMarkets(empId).subscribe(response => {
      this.markets = response as IMarket[];
    }, error => {
      console.log(error);
    });
  }
  getEmployeeId() {
    this.empId = this.accountService.getEmployeeId();
    this.getMarket(this.empId);
    //this.marketGroupService.marketGroupFormData.employeeId = parseInt(this.empId);
  }
  getEmployeeSbu() {
    this.accountService.getEmployeeSbu(parseInt(this.empId)).subscribe(
      (response) => {
        this.empSbu = response.sbu;;
        //this.getLastFiveInvestment(this.investmentInitService.investmentInitFormData.marketCode, this.todayDate);
      },
      (error) => {
        console.log(error);
      }
    );
  }
  getGroups() {
    this.SpinnerService.show();
    this.marketGroupService.getGroups(parseInt(this.empId)).subscribe(response => {
      this.SpinnerService.hide();
      this.marketGroupMsts = response.data;
      if (this.marketGroupMsts.length > 0) {
        this.openMarketGroupSearchModal(this.marketGroupSearchModal);
      }
      else {
        this.toastr.warning('No Data Found');
      }
    }, error => {
      this.SpinnerService.hide();
      console.log(error);
    });
  }

  resetSearch() {
    this.searchText = '';
  }
  openMarketGroupSearchModal(template: TemplateRef<any>) {
    this.marketGroupSearchModalRef = this.modalService.show(template, this.config);
  }
  getMarketGroups() {
    this.marketGroupService.getMarketGroups(this.marketGroupService.marketGroupFormData.id).subscribe(response => {
      debugger;
      this.marketGroupDtls = response.data;
      //this.checkFiveMarket();
      if (this.marketGroupService.marketGroupFormData.status == "Active") {
        if (this.marketGroupDtls.length != 4) {
          this.toastr.warning("Four market must be added in Market Group");
              this.marketGroupService.marketGroupFormData.status = "Inactive";
              if (this.marketGroupService.marketGroupFormData.id != 0){
              this.marketGroupService.updateMarketGroup(parseInt(this.empId)).subscribe(
                res => {
                  debugger;
                  this.marketGroupService.marketGroupFormData = res as IMarketGroupMst;
                  //this.status= "Inactive";
                  //event.target.value= "Inactive";
                },
                err => { console.log(err); }
              );
            }
              return false;
        }
      }
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
    this.marketGroupService.insertMarketGroup(parseInt(this.empId)).subscribe(
      res => {
        debugger;
        this.marketGroupService.marketGroupFormData = res as IMarketGroupMst;
        this.toastr.success('Saved successfully', 'Market Group')
      },
      err => { console.log(err); }
    );
  }

  updateMarketGroup(form: NgForm) {
    this.marketGroupService.updateMarketGroup(parseInt(this.empId)).subscribe(
      res => {
        debugger;
        this.marketGroupService.marketGroupFormData = res as IMarketGroupMst;
        this.toastr.success('Updated successfully', 'Market Group')
      },
      err => { console.log(err); }
    );

  }

  addMarket() {
    debugger;

    //var e = (document.getElementById("marketCode") as HTMLInputElement).value;
    //var f = document.getElementById('marketCode');
    if (this.marketGroupService.marketGroupFormData.id === 0 || this.marketGroupService.marketGroupFormData.id === undefined) {
      this.toastr.warning("Please Insert Market Group first!")
      return false;
    } if (this.marketGroupService.marketGroupFormData.marketCode == "" || this.marketGroupService.marketGroupFormData.marketCode === undefined || this.marketGroupService.marketGroupFormData.marketCode === null) {
      this.toastr.warning("Please  Select Market first!")
      return false;
    }
    
    var e = (document.getElementById("marketCode")) as HTMLSelectElement;
    //var sel = e.selectedIndex;
    //var opt = e.options[sel];
    var selectedMarketCode = this.marketGroupService.marketGroupFormData.marketCode;
    if (this.marketGroupDtls !== undefined) {
      for (let i = 0; i < this.marketGroupDtls.length; i++) {
        if (this.marketGroupDtls[i].marketCode === selectedMarketCode) {
          this.toastr.warning("Market already exist in this Group!");
          return false;
        }
      }
    }
    let isMarketAvailable = false;
    for (let i = 0; i < this.markets.length; i++) {
      if (this.markets[i].marketCode === selectedMarketCode) {
        this.sbu = this.markets[i].sbu;
        this.sbuName = this.markets[i].sbuName;
        var selectedMarketName = this.markets[i].marketName;
        isMarketAvailable = true;
        break;
      }
    }
    if (!isMarketAvailable) {
      this.toastr.warning("Market is not found!");
      return false;
    }
    this.marketGroupService.insertMarketGroupDtl(this.marketGroupService.marketGroupFormData.id, selectedMarketCode, selectedMarketName, this.sbu, this.sbuName).subscribe(response => {
      debugger;
      this.getMarketGroups();
    }, error => {
      console.log(error);
    });
  }
  selectMarketGroup(selectedRecord: IMarketGroupMst) {
    debugger;
    this.marketGroupService.marketGroupFormData = Object.assign({}, selectedRecord);
    this.getMarketGroups();
    this.marketGroupSearchModalRef.hide()
  }
  removeMarketGroups(selectedRecord: IMarketGroupDtl) {
    debugger;
    var c = confirm("Are you sure you want to remove that?");
    if (c == true) {
      this.marketGroupService.removeMarketGroups(selectedRecord).subscribe(response => {
        debugger;
        this.getMarketGroups();
      }, error => {
        console.log(error);
      });
    }
  }

  checkFiveMarket(event) {
    debugger;
    if (this.marketGroupService.marketGroupFormData.status == "Active") {
      if (this.marketGroupDtls.length == 4) {
        // for (let i = 0; i < this.marketGroupDtls.length; i++) {
        //   if (this.empSbu == this.marketGroupDtls[i].sbu) {
        //     this.toastr.warning("Own SBU cannot be added in Market Group");
        //     this.marketGroupService.marketGroupFormData.status = "Inactive";
        //     return false;
        //   }
        //   let count = 0;
        //   for (let j = 0; j < this.marketGroupDtls.length; j++) {
        //     if (this.marketGroupDtls[i].sbu == this.marketGroupDtls[j].sbu) {
        //       count = count + 1;
        //     }
        //   }
        //   if(count>1)
        //   {
        //     this.toastr.warning("Same SBU cannot be added multiple in Market Group");
        //     this.marketGroupService.marketGroupFormData.status = "Inactive";
        //     return false;
        //   }
        // }
      }
      else{
        this.toastr.warning("Five market must be added in Market Group");
            this.marketGroupService.marketGroupFormData.status = "Inactive";
            if (this.marketGroupService.marketGroupFormData.id != 0){
            this.marketGroupService.updateMarketGroup(parseInt(this.empId)).subscribe(
              res => {
                debugger;
                this.marketGroupService.marketGroupFormData = res as IMarketGroupMst;
                //this.status= "Inactive";
                event.target.value= "Inactive";
              },
              err => { console.log(err); }
            );
          }
            return false;
      }
    }
  }
  resetPage(form: NgForm) {
    debugger;
    //form.reset();
    this.marketGroupService.marketGroupFormData = new MarketGroupMst();
    this.marketGroupService.marketGroupFormData.status = "Inactive";
    this.marketGroupDtls = [];
  }
  resetForm() {
    debugger;
    this.marketGroupService.marketGroupFormData = new MarketGroupMst();
    this.marketGroupService.marketGroupFormData.status = "Inactive";
    this.marketGroupDtls = [];
  }

}
