
import { InvestmentInit, IInvestmentInit, InvestmentDetail, IInvestmentDetail, InvestmentTargetedProd, IInvestmentTargetedProd, InvestmentTargetedGroup, IInvestmentTargetedGroup, IInvestmentDetailOld, ILastFiveInvestmentDetail, IInvestmentMedicineProd, InvestmentMedicineProd } from '../shared/models/investment';
import { InvestmentDoctor, IInvestmentDoctor, InvestmentInstitution, IInvestmentInstitution, InvestmentCampaign, IInvestmentCampaign } from '../shared/models/investment';
import { InvestmentBcds, IInvestmentBcds, InvestmentSociety, IInvestmentSociety } from '../shared/models/investment';
import { SubCampaign, ISubCampaign } from '../shared/models/subCampaign';
import { Donation, IDonation } from '../shared/models/donation';
import { Doctor, IDoctor } from '../shared/models/docotor';
import { Institution, IInstitution } from '../shared/models/institution';
//import { GenericParams } from '../shared/models/genericParams';
import { Component, ElementRef, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { InvestmentInitService } from '../_services/investment.service';
import { FormGroup, NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Product, IProduct } from '../shared/models/product';
import { Market, IMarket } from '../shared/models/market';
import { CampaignMst, ICampaignMst, CampaignDtl, ICampaignDtl, CampaignDtlProduct, ICampaignDtlProduct } from '../shared/models/campaign';
import { DatePipe } from '@angular/common';
import { IBcdsInfo } from '../shared/models/bcdsInfo';
import { ISocietyInfo } from '../shared/models/societyInfo';
import { MarketGroupMst, IMarketGroupMst } from '../shared/models/marketGroupMst';
import { MarketGroupDtl, IMarketGroupDtl } from '../shared/models/marketGroupDtl';
import { AccountService } from '../account/account.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { BreadcrumbModule } from 'xng-breadcrumb';
import { IMedicineProduct } from '../shared/models/medicineProduct';

@Component({
  selector: 'app-investmentInit',
  templateUrl: './investmentInit.component.html',
  styles: [
  ],
  providers: [DatePipe]
})
export class InvestmentInitComponent implements OnInit {
  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  @ViewChild('investmentInitSearchModal', { static: false }) investmentInitSearchModal: TemplateRef<any>;
  @ViewChild('submissionConfirmModal', { static: false }) submissionConfirmModal: TemplateRef<any>;
  InvestmentInitSearchModalRef: BsModalRef;
  submissionConfirmRef: BsModalRef;
  convertedDate: string;
  empId: string;
  searchText = '';
  //configs: any;
  minDate: Date;
  maxDate: Date;
  degree: any;
  designation: any;
  docInstaddress: any;
  instaddress: any;
  userRole: string;
  sbu: string;
  marketCode: string;
  investmentInits: IInvestmentInit[];
  investmentMedicineProds: IInvestmentMedicineProd[];
  investmentTargetedProds: IInvestmentTargetedProd[];
  investmentTargetedGroups: IInvestmentTargetedGroup[];
  investmentDetailsOld: IInvestmentDetailOld[];
  lastFiveInvestmentDetail: ILastFiveInvestmentDetail[];
  investmentDoctors: IInvestmentDoctor[];
  isValid: boolean = false;
  isInvOther: boolean = false;
  isAdmin: boolean = false;
  isDonationValid: boolean = false;
  isSubmitted: boolean = false;
  investmentInitForm: NgForm;
  numberPattern = "^[0-9]+(.[0-9]{1,10})?$";
  bcds: IBcdsInfo[];
  society: ISocietyInfo[];
  markets: IMarket[];
  products: IProduct[];
  medicineProducts: IMedicineProduct[];
  campaignDtlproducts: IProduct[];
  subCampaigns: ISubCampaign[];
  doctors: IDoctor[];
  institutions: IInstitution[];
  donations: IDonation[];
  campaignMsts: ICampaignMst[];
  campaignDtls: ICampaignDtl[];
  campaignDtlProducts: ICampaignDtlProduct[];
  marketGroupMsts: IMarketGroupMst[];
  donationToVal: string;
  totalCount = 0;
  bsConfig: Partial<BsDatepickerConfig>;
  bsValue: Date = new Date();
  today = new Date();
  dd = String(this.today.getDate()).padStart(2, '0');
  mm = String(this.today.getMonth() + 1).padStart(2, '0'); //January is 0!
  yyyy = this.today.getFullYear();
  todayDate = this.dd + this.mm + this.yyyy;
  config = {
    keyboard: false,
    class: 'modal-lg',
    ignoreBackdropClick: true
  };
  institutionType: string;
  constructor(private accountService: AccountService, public investmentInitService: InvestmentInitService, private router: Router,
    private toastr: ToastrService, private modalService: BsModalService, private datePipe: DatePipe, private SpinnerService: NgxSpinnerService) { }
  ngOnInit() {
    this.convertedDate = this.datePipe.transform(this.today, 'ddMMyyyy');
    this.resetPageLoad()
    this.getEmployeeId();
    this.getDonation();
    this.bsConfig = Object.assign({}, { containerClass: 'theme-blue' }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();
    const currentDate = new Date();
    this.minDate = new Date(currentDate.getFullYear(), 0, 1);
    this.maxDate = new Date(currentDate.getFullYear(), 11, 31);
  }
  openInvestmentInitSearchModal(template: TemplateRef<any>) {
    this.InvestmentInitSearchModalRef = this.modalService.show(template, this.config);
  }
  selectInvestmentInit(selectedRecord: IInvestmentInit) {
    this.resetPageLoad();
    this.investmentInitService.investmentInitFormData = Object.assign({}, selectedRecord);
    this.investmentInitService.investmentDoctorFormData.investmentInitId = selectedRecord.id;
    this.investmentInitService.investmentInstitutionFormData.investmentInitId = selectedRecord.id;
    this.investmentInitService.investmentCampaignFormData.investmentInitId = selectedRecord.id;
    this.investmentInitService.investmentBcdsFormData.investmentInitId = selectedRecord.id;
    this.investmentInitService.investmentSocietyFormData.investmentInitId = selectedRecord.id;
    this.investmentInitService.investmentDetailFormData.investmentInitId = selectedRecord.id;
    this.convertedDate = this.datePipe.transform(selectedRecord.setOn, 'ddMMyyyy');
    this.isDonationValid = true;
    if (this.investmentInitService.investmentInitFormData.donationId == 4) {
      this.getInvestmentMedicineProd();
    }
    if (this.investmentInitService.investmentInitFormData.donationTo == "Doctor") {
      this.getDoctor();
      //this.getInstitution();
      //this.getInvestmentDoctor();
    }
    else if (this.investmentInitService.investmentInitFormData.donationTo == "Institution") {
      //this.getDoctor();
      this.getInstitution();
      //this.getInvestmentInstitution();
    }
    else if (this.investmentInitService.investmentInitFormData.donationTo == "Campaign") {
      this.getCampaignMst(this.investmentInitService.investmentInitFormData.employeeId);
      //this.getDoctor();
      //this.getInstitution();
      //this.getInvestmentCampaign();
    }
    else if (this.investmentInitService.investmentInitFormData.donationTo == "Bcds") {
      this.getBcds();
      //this.getInvestmentBcds();
    }
    else if (this.investmentInitService.investmentInitFormData.donationTo == "Society") {
      this.getSociety();

    }
    this.getInvestmentDetails();
    this.getInvestmentTargetedProd();
    this.getInvestmentTargetedGroup();
    if (parseInt(this.empId) == this.investmentInitService.investmentInitFormData.employeeId) {
      this.isInvOther = false;
      //this.isValid = true;
      // this.getInvestmentTargetedProd();
    }
    else {
      this.isInvOther = true;
      //this.isValid = false;
    }
    if (this.investmentInitService.investmentInitFormData.confirmation == true) {
      this.isSubmitted = true;
      //this.isValid = true;
      // this.getInvestmentTargetedProd();
    }
    else {
      this.isSubmitted = false;
      //this.isValid = false;
    }

    this.isValid = true;
    this.InvestmentInitSearchModalRef.hide()
  }
  getInvestmentInit() {
    const params = this.investmentInitService.getGenParams();
    this.SpinnerService.show();
    this.investmentInitService.getInvestmentInit(parseInt(this.empId), this.sbu, this.userRole).subscribe(response => {
      this.SpinnerService.hide();
      this.investmentInits = response as IInvestmentInit[];
      //this.totalCount = response.count;
      // this.configs = {
      //   currentPage: params.pageIndex,
      //   itemsPerPage: params.pageSize,
      //   totalItems: this.totalCount,
      // };
      if (this.investmentInits.length > 0) {
          this.openInvestmentInitSearchModal(this.investmentInitSearchModal);
      }
      else {
        this.toastr.warning('No Data Found');
      }
    }, error => {
      this.SpinnerService.hide();
      console.log(error);
    });
  }
  getInvestmentDetails() {
    this.investmentInitService.getInvestmentDetails(this.investmentInitService.investmentDetailFormData.investmentInitId).subscribe(response => {
      var data = response[0] as IInvestmentDetail;
      if (data !== undefined) {
        this.investmentInitService.investmentDetailFormData = data;
        this.investmentInitService.investmentDetailFormData.fromDate = new Date(data.fromDate);
        this.investmentInitService.investmentDetailFormData.toDate = new Date(data.toDate);
        this.investmentInitService.investmentDetailFormData.commitmentFromDate = new Date(data.commitmentFromDate);
        this.investmentInitService.investmentDetailFormData.commitmentToDate = new Date(data.commitmentToDate);
        //this.convertedDate = this.datePipe.transform(data.fromDate, 'ddMMyyyy');
        //this.getLastFiveInvestment(this.investmentInitService.investmentInitFormData.marketCode, this.convertedDate);

      } else {
        // this.toastr.warning('No Data Found', 'Investment');
      }
    }, error => {
      console.log(error);
    });
  }
  getInvestmentCampaign() {
    this.investmentInitService.getInvestmentCampaigns(this.investmentInitService.investmentCampaignFormData.investmentInitId).subscribe(response => {
      var data = response[0] as IInvestmentCampaign;
      if (data !== undefined) {
        this.investmentInitService.investmentCampaignFormData = data;
        debugger;
        this.investmentInitService.investmentCampaignFormData.campaignMstId = data.campaignDtl.mstId;
        this.investmentInitService.investmentCampaignFormData.subCampStartDate = new DatePipe('en-US').transform(data.campaignDtl.subCampStartDate, 'dd/MM/yyyy');
        this.investmentInitService.investmentCampaignFormData.subCampEndDate = new DatePipe('en-US').transform(data.campaignDtl.subCampEndDate, 'dd/MM/yyyy');

        this.onChangeCampaignInCamp();
        //this.onChangeSubCampaignInCamp();
      }
      else {
        this.toastr.warning('No Data Found', 'Investment');
      }

    }, error => {
      console.log(error);
    });
  }
  dateCompare(form: NgForm) {
    if (this.investmentInitService.investmentDetailFormData.fromDate != null && this.investmentInitService.investmentDetailFormData.toDate != null) {
      if (this.investmentInitService.investmentDetailFormData.toDate > this.investmentInitService.investmentDetailFormData.fromDate) {
      }
      else {
        form.controls.fromDate.setValue(null);
        form.controls.toDate.setValue(null);
        this.toastr.error('Select Appropriate Payment Date Range');
      }
    }
  }
  dateCommitmentCompare(form: NgForm) {
    if (this.investmentInitService.investmentDetailFormData.commitmentFromDate != null && this.investmentInitService.investmentDetailFormData.commitmentToDate != null) {
      if (this.investmentInitService.investmentDetailFormData.commitmentToDate > this.investmentInitService.investmentDetailFormData.commitmentFromDate) {
      }
      else {
        form.controls.commitmentFromDate.setValue(null);
        form.controls.commitmentToDate.setValue(null);
        this.toastr.error('Select Appropriate Commitment Date Range');
      }
    }
  }
  getInvestmentBcds() {
    this.investmentInitService.getInvestmentBcds(this.investmentInitService.investmentBcdsFormData.investmentInitId).subscribe(response => {
      var data = response[0] as IInvestmentBcds;
      if (data !== undefined) {
        this.investmentInitService.investmentBcdsFormData = data;
        this.onChangeBcdsInBcds();
        this.getLastFiveInvestment(this.investmentInitService.investmentInitFormData.marketCode, this.convertedDate);
      }
      else {
        //this.toastr.warning('No Data Found', 'Investment');
      }
    }, error => {
      console.log(error);
    });
  }
  getInvestmentSociety() {
    this.investmentInitService.getInvestmentSociety(this.investmentInitService.investmentSocietyFormData.investmentInitId).subscribe(response => {
      var data = response[0] as IInvestmentSociety;
      if (data !== undefined) {
        this.investmentInitService.investmentSocietyFormData = data;
        this.onChangeSocietyInSociety();
        this.getLastFiveInvestment(this.investmentInitService.investmentInitFormData.marketCode, this.convertedDate);
      }
      else {
        //this.toastr.warning('No Data Found', 'Investment');
      }

    }, error => {
      console.log(error);
    });
  }
  getInvestmentInstitution() {
    this.investmentInitService.getInvestmentInstitutions(this.investmentInitService.investmentInstitutionFormData.investmentInitId).subscribe(response => {
      var data = response[0] as IInvestmentInstitution;
      if (data !== undefined) {
        this.investmentInitService.investmentInstitutionFormData = data;
        this.onChangeInstitutionInInst();
        this.getLastFiveInvestment(this.investmentInitService.investmentInitFormData.marketCode, this.convertedDate);
      }
      else {
        //this.toastr.warning('No Data Found', 'Investment');
      }
    }, error => {
      console.log(error);
    });
  }
  getInvestmentDoctor() {
    this.investmentInitService.getInvestmentDoctors(this.investmentInitService.investmentDoctorFormData.investmentInitId).subscribe(response => {
      var data = response[0] as IInvestmentDoctor;
      if (data !== undefined) {
        this.investmentInitService.investmentDoctorFormData = data;
        //this.investmentInitService.investmentDoctorFormData.doctorName = String(data.doctorId);
        this.onChangeDoctorInDoc();
        this.onChangeInstitutionInDoc();
        this.getLastFiveInvestment(this.investmentInitService.investmentInitFormData.marketCode, this.convertedDate);
      }
      else {
        //this.toastr.warning('No Data Found', 'Investment');
      }
    }, error => {
      console.log(error);
    });
  }
  // #region Medicine Prod 
  getInvestmentMedicineProd() {
    this.investmentInitService.getInvestmentMedicineProds(this.investmentInitService.investmentInitFormData.id, this.sbu).subscribe(response => {
      var data = response as IInvestmentMedicineProd[];
      debugger;
      if (data !== undefined && data.length>0) {
        this.investmentMedicineProds = data;
        let sum=0;
        for (let i = 0; i < this.investmentMedicineProds.length; i++) {
          sum=sum+this.investmentMedicineProds[i].tpVat;
        }
        //this.investmentInitService.investmentDetailFormData.proposedAmount=sum.toString();
        this.investmentInitService.investmentDetailFormData.proposedAmount=((Math.round(sum * 100) / 100).toFixed(2));
      }
      else {
        this.investmentInitService.investmentDetailFormData.proposedAmount='';
        this.investmentMedicineProds =[];
      }
    }, error => {
      console.log(error);
    });
  }
  insertInvestmentMedicineProd() {
    if (this.investmentInitService.investmentInitFormData.id == null || this.investmentInitService.investmentInitFormData.id == undefined || this.investmentInitService.investmentInitFormData.id == 0) {
      this.toastr.warning('Insert Investment Initialisation First!');
      return false;
    }
    if (this.investmentInitService.investmentMedicineProdFormData.productId == null || this.investmentInitService.investmentMedicineProdFormData.productId == undefined || this.investmentInitService.investmentMedicineProdFormData.productId == 0) {
      this.toastr.warning('Select Product First');
      return false;
    }
    if (this.investmentInitService.investmentMedicineProdFormData.boxQuantity == null || this.investmentInitService.investmentMedicineProdFormData.boxQuantity == undefined || this.investmentInitService.investmentMedicineProdFormData.boxQuantity == 0) {
      this.toastr.warning('Insert Box Quantity First');
      return false;
    }
    if (this.investmentMedicineProds !== undefined) {
      for (let i = 0; i < this.investmentMedicineProds.length; i++) {
        if (this.investmentMedicineProds[i].medicineProduct.id === this.investmentInitService.investmentMedicineProdFormData.productId) {
          this.toastr.warning("Product already exist!");
          return false;
        }
      }
    }
    if (this.isSubmitted == true && parseInt(this.empId) == this.investmentInitService.investmentInitFormData.employeeId) {
      this.toastr.warning("Investment already submitted");
      return false;
    }
    else {
      this.investmentInitService.investmentMedicineProdFormData.investmentInitId = this.investmentInitService.investmentInitFormData.id;
      this.investmentInitService.investmentMedicineProdFormData.employeeId = parseInt(this.empId);
        this.SpinnerService.show();
        this.investmentInitService.insertInvestmentMedicineProd().subscribe(
          res => {
            this.investmentInitService.investmentMedicineProdFormData = new InvestmentMedicineProd();
            this.getInvestmentMedicineProd();
            this.isDonationValid = true;
            this.toastr.success('Save successfully', 'Investment  Product');
          },
          err => { console.log(err); }
        );
    }
  }
  removeInvestmentMedicineProd(selectedRecord: IInvestmentMedicineProd) {
    if (this.isSubmitted == true && parseInt(this.empId) == this.investmentInitService.investmentInitFormData.employeeId) {
      this.toastr.warning("Investment already submitted");
      return false;
    }
    var c = confirm("Are you sure you want to delete that?");
    if (c == true) {
      this.investmentInitService.investmentMedicineProdFormData = Object.assign({}, selectedRecord);
      this.SpinnerService.show();
      debugger;
      this.investmentInitService.removeInvestmentMedicineProd().subscribe(
        res => {
          //this.isDonationValid=false;
          this.investmentInitService.investmentMedicineProdFormData = new InvestmentMedicineProd();
          this.getInvestmentMedicineProd();
          this.SpinnerService.hide();

          this.toastr.success(res);
        },
        err => {
          this.SpinnerService.hide();
          console.log(err);
        }
      );
    }
  }
  // #endregion Medicine Prod

// #region Targeted Prod 
  getInvestmentTargetedProd() {
    this.investmentInitService.getInvestmentTargetedProds(this.investmentInitService.investmentInitFormData.id, this.sbu).subscribe(response => {

      var data = response as IInvestmentTargetedProd[];
      if (data !== undefined) {
        this.investmentTargetedProds = data;

      }
      else {
        //this.toastr.warning('No Data Found', 'Investment');
      }

    }, error => {
      console.log(error);

    });
  }
  removeInvestmentTargetedProd(selectedRecord: IInvestmentTargetedProd) {
    if (this.isSubmitted == true && parseInt(this.empId) == this.investmentInitService.investmentInitFormData.employeeId) {
      this.toastr.warning("Investment already submitted");
      return false;
    }
    var c = confirm("Are you sure you want to delete that?");
    if (c == true) {
      this.investmentInitService.investmentTargetedProdFormData = Object.assign({}, selectedRecord);
      this.SpinnerService.show();
      debugger;
      this.investmentInitService.removeInvestmentTargetedProd().subscribe(
        res => {
          //this.isDonationValid=false;
          this.investmentInitService.investmentTargetedProdFormData = new InvestmentTargetedProd();
          this.getInvestmentTargetedProd();
          this.SpinnerService.hide();

          this.toastr.success(res);
        },
        err => {
          this.SpinnerService.hide();
          console.log(err);
        }
      );
    }
  }
  // #endregion Targeted Prod 
  // #region Targeted Group 
  getInvestmentTargetedGroup() {
    this.investmentInitService.getInvestmentTargetedGroups(this.investmentInitService.investmentInitFormData.id).subscribe(response => {

      var data = response as IInvestmentTargetedGroup[];
      if (data !== undefined) {
        this.investmentTargetedGroups = data;

      }
      else {
        //this.toastr.warning('No Data Found', 'Investment');
      }

    }, error => {
      console.log(error);
    });
  }
  // removeInvestmentIndTargetedGroup(selectedRecord: IInvestmentTargetedGroup) {
  //   if (this.isSubmitted == true && parseInt(this.empId) == this.investmentInitService.investmentInitFormData.employeeId) {
  //     this.toastr.warning("Investment already submitted");
  //     return false;
  //   }
  //   if (this.isSubmitted == true && parseInt(this.empId) == this.investmentInitService.investmentInitFormData.employeeId) {
  //     this.toastr.warning("Investment already submitted");
  //     return false;
  //   }
  //   var c = confirm("Are you sure you want to delete that?");
  //   if (c == true) {
  //     if(selectedRecord.id==0 || selectedRecord.id==undefined || selectedRecord.id==null)
  //     {
  //       this.investmentTargetedGroups.splice(this.investmentTargetedGroups.findIndex(x => x.marketCode == selectedRecord.marketCode), 1);
  //       this.toastr.success("Successfully Removed. Please Save the data.");
  //       return false;
      
  //     }
  //     this.investmentInitService.investmentTargetedGroupFormData = Object.assign({}, selectedRecord);
  //     this.SpinnerService.show();
  //     debugger;
  //     this.investmentInitService.removeInvestmentIndTargetedGroup(this.investmentInitService.investmentTargetedGroupFormData).subscribe(
  //       res => {
  //         //this.isDonationValid=false;
  //         this.investmentInitService.investmentTargetedGroupFormData = new InvestmentTargetedGroup();
  //         this.getInvestmentTargetedGroup();
  //         this.SpinnerService.hide();
  //         this.toastr.success(res);
  //       },
  //       err => {
  //         this.SpinnerService.hide();
  //         console.log(err);
  //       }
  //     );
  //   }
  // }
  removeInvestmentTargetedGroup() {
    //this.investmentInitService.investmentTargetedProdFormData = Object.assign({}, selectedRecord);
    if (this.investmentTargetedGroups != null && this.investmentTargetedGroups.length > 0) {
      var c = confirm("Are you sure you want to delete that?");
      if (c == true) {
        if (this.isSubmitted == true) {
          this.toastr.warning('This Investment has already been submitted', 'Investment');
          return false;
        }
        this.SpinnerService.show();
        this.investmentInitService.removeInvestmentTargetedGroup(this.investmentTargetedGroups).subscribe(
          res => {
            //this.isDonationValid=false;
            this.investmentInitService.investmentTargetedGroupFormData = new InvestmentTargetedGroup();
            this.getInvestmentTargetedGroup();
            this.SpinnerService.hide();
            this.toastr.success(res);
          },
          err => {
            this.SpinnerService.hide();
            console.log(err);
            this.toastr.warning(err.error, 'Investment Group');
          }
        );
      }
    }
    else {
      this.toastr.warning('No Market Group Found', 'Investment Group');
    }
  }
  // #endregion Targeted Group 
  getEmployeeId() {
    this.empId = this.accountService.getEmployeeId();
    this.userRole = this.accountService.getUserRole();
    if (this.userRole == 'Administrator') {
      this.isAdmin = true;
    }
    else {
      this.isAdmin = false;
    }
    this.investmentInitService.investmentInitFormData.employeeId = parseInt(this.empId);
    this.getMarketGroupMsts();
    this.getEmployeeSbu();

  }
  
  // #region Last Five Investment 
  async  getLastFiveInvestment(marketCode: string, toDayDate: string) {
    if (this.investmentInitService.investmentInitFormData.donationTo == "Doctor") {
      await this.investmentInitService.getLastFiveInvestmentForDoc(this.investmentInitService.investmentInitFormData.donationId, this.investmentInitService.investmentDoctorFormData.doctorId, marketCode, toDayDate).then(
        (response) => {
          this.lastFiveInvestmentDetail = response as ILastFiveInvestmentDetail[];
        },
        (error) => {
          console.log(error);
        }
      );
    }
    else if (this.investmentInitService.investmentInitFormData.donationTo == "Institution") {
      await this.investmentInitService.getLastFiveInvestmentForInstitute(this.investmentInitService.investmentInitFormData.donationId, this.investmentInitService.investmentInstitutionFormData.institutionId, marketCode, toDayDate).then(
        (response) => {
          this.lastFiveInvestmentDetail = response as ILastFiveInvestmentDetail[];
        },
        (error) => {
          console.log(error);
        }
      );
    }
    else if (this.investmentInitService.investmentInitFormData.donationTo == "Campaign") {
      await this.investmentInitService.getLastFiveInvestmentForCampaign(this.investmentInitService.investmentInitFormData.donationId, this.investmentInitService.investmentCampaignFormData.campaignMstId, marketCode, toDayDate).then(
        (response) => {
          this.lastFiveInvestmentDetail = response as ILastFiveInvestmentDetail[];
        },
        (error) => {
          console.log(error);
        }
      );
    }
    else if (this.investmentInitService.investmentInitFormData.donationTo == "Bcds") {
      await this.investmentInitService.getLastFiveInvestmentForBcds(this.investmentInitService.investmentInitFormData.donationId, this.investmentInitService.investmentBcdsFormData.bcdsId, marketCode, toDayDate).then(
        (response) => {
          this.lastFiveInvestmentDetail = response as ILastFiveInvestmentDetail[];
        },
        (error) => {
          console.log(error);
        }
      );
    }
    else if (this.investmentInitService.investmentInitFormData.donationTo == "Society") {

      await this.investmentInitService.getLastFiveInvestmentForSociety(this.investmentInitService.investmentInitFormData.donationId, this.investmentInitService.investmentSocietyFormData.societyId, marketCode, toDayDate).then(
        (response) => {
          this.lastFiveInvestmentDetail = response as ILastFiveInvestmentDetail[];
        },
        (error) => {
          console.log(error);
        }
      );
    }

  }
  // #endregion Last Five Investment  
  getEmployeeSbu() {
    this.accountService.getEmployeeSbu(this.investmentInitService.investmentInitFormData.employeeId).subscribe(
      (response) => {
        this.sbu = response.sbu;
        this.investmentInitService.investmentInitFormData.marketCode = response.marketCode;
        this.marketCode = response.marketCode;
        this.getProduct();
        this.getMedicineProds();
        //this.getLastFiveInvestment(this.investmentInitService.investmentInitFormData.marketCode, this.todayDate);
      },
      (error) => {
        console.log(error);
      }
    );
  }
  onChangeProposeFor() {
    // if (this.investmentInitService.investmentInitFormData.proposeFor == "BrandCampaign") {
    //   this.investmentInitService.investmentInitFormData.donationTo = "Campaign";
    //   this.onChangeDonationTo();
    //   this.isDonationValid = true;
    // }
    // else {
    //   this.isDonationValid = false;
    // }
    if (this.investmentInitService.investmentInitFormData.proposeFor == "BrandCampaign" && this.investmentInitService.investmentInitFormData.donationTo != "Campaign" && this.investmentInitService.investmentInitFormData.donationTo != null) {
      this.toastr.warning("For Brand Campaign, must select Campaign");
      this.investmentInitService.investmentInitFormData.donationTo = null;
      return false;
    }
    if (this.investmentInitService.investmentInitFormData.proposeFor == "Others" && this.investmentInitService.investmentInitFormData.donationTo == "Campaign" && this.investmentInitService.investmentInitFormData.donationTo != null) {
      this.toastr.warning("For Campaign, must select Brand Campaign");
      this.investmentInitService.investmentInitFormData.donationTo = null;
      return false;
    }
  }
  onChangeDonationTo() {
    if (this.investmentInitService.investmentInitFormData.proposeFor == "BrandCampaign" && this.investmentInitService.investmentInitFormData.donationTo != "Campaign" && this.investmentInitService.investmentInitFormData.donationTo != null) {
      this.toastr.warning("For Brand Campaign, must select Campaign");
      this.investmentInitService.investmentInitFormData.proposeFor = null;
      return false;
    }
    if (this.investmentInitService.investmentInitFormData.proposeFor == "Others" && this.investmentInitService.investmentInitFormData.donationTo == "Campaign" && this.investmentInitService.investmentInitFormData.donationTo != null) {
      this.toastr.warning("For Campaign, must select Brand Campaign");
      this.investmentInitService.investmentInitFormData.proposeFor = null;
      return false;
    }
    if (this.investmentInitService.investmentInitFormData.donationTo == "Doctor") {
      if (this.investmentInitService.investmentDoctorFormData.id == null || this.investmentInitService.investmentDoctorFormData.id == undefined || this.investmentInitService.investmentDoctorFormData.id == 0) {
        this.investmentInitService.investmentDoctorFormData = new InvestmentDoctor();
        this.getDoctor();
        this.getInstitution();
      }
    }
    else if (this.investmentInitService.investmentInitFormData.donationTo == "Institution") {
      if (this.investmentInitService.investmentInstitutionFormData.id == null || this.investmentInitService.investmentInstitutionFormData.id == undefined || this.investmentInitService.investmentInstitutionFormData.id == 0) {
        this.investmentInitService.investmentDoctorFormData = new InvestmentDoctor();
        this.getDoctor();
        this.getInstitution();
      }
    }
    else if (this.investmentInitService.investmentInitFormData.donationTo == "Campaign") {
      if (this.investmentInitService.investmentCampaignFormData.id == null || this.investmentInitService.investmentCampaignFormData.id == undefined || this.investmentInitService.investmentCampaignFormData.id == 0) {
        this.investmentInitService.investmentCampaignFormData = new InvestmentCampaign();
        this.getCampaignMst(parseInt(this.empId));
        this.getDoctor();
        this.getInstitution();
      }
    }
    else if (this.investmentInitService.investmentInitFormData.donationTo == "Bcds") {
      if (this.investmentInitService.investmentBcdsFormData.id == null || this.investmentInitService.investmentBcdsFormData.id == undefined || this.investmentInitService.investmentBcdsFormData.id == 0) {
        this.investmentInitService.investmentBcdsFormData = new InvestmentBcds();
        this.getBcds();
      }
    }
    else if (this.investmentInitService.investmentInitFormData.donationTo == "Society") {
      if (this.investmentInitService.investmentSocietyFormData.id == null || this.investmentInitService.investmentSocietyFormData.id == undefined || this.investmentInitService.investmentSocietyFormData.id == 0) {
        this.investmentInitService.investmentSocietyFormData = new InvestmentSociety();
        this.getSociety();
      }
    }
    if (this.investmentInitService.investmentInitFormData.id != null && this.investmentInitService.investmentInitFormData.id != undefined && this.investmentInitService.investmentInitFormData.id != 0) {
      this.investmentInitService.investmentDoctorFormData.investmentInitId = this.investmentInitService.investmentInitFormData.id;
      this.investmentInitService.investmentInstitutionFormData.investmentInitId = this.investmentInitService.investmentInitFormData.id;
      this.investmentInitService.investmentCampaignFormData.investmentInitId = this.investmentInitService.investmentInitFormData.id;
      this.investmentInitService.investmentBcdsFormData.investmentInitId = this.investmentInitService.investmentInitFormData.id;
      this.investmentInitService.investmentSocietyFormData.investmentInitId = this.investmentInitService.investmentInitFormData.id;
    }
  }
  onChangeDoctorInDoc() {
    for (var i = 0; i < this.doctors.length; i++) {
      if (this.doctors[i].id == this.investmentInitService.investmentDoctorFormData.doctorId) {
        //this.investmentInitService.investmentDoctorFormData.doctorName=this.doctors[i].doctorName;
        this.investmentInitService.investmentDoctorFormData.doctorCode = this.doctors[i].doctorCode;
        this.degree = this.doctors[i].degree;
        this.designation = this.doctors[i].designation;
        break;
      }
    }
    this.getLastFiveInvestment(this.investmentInitService.investmentInitFormData.marketCode, this.convertedDate);
  }
  onChangeInstitutionInDoc() {
    for (var i = 0; i < this.institutions.length; i++) {
      if (this.institutions[i].id == this.investmentInitService.investmentDoctorFormData.institutionId) {
        this.docInstaddress = this.institutions[i].address;

        break;
      }
    }
  }
  onChangeInstitutionInInst() {
    for (var i = 0; i < this.institutions.length; i++) {
      if (this.institutions[i].id == this.investmentInitService.investmentInstitutionFormData.institutionId) {
        this.instaddress = this.institutions[i].address;
        this.institutionType = this.institutions[i].institutionType;
        debugger;
        break;
      }
    }
    this.getLastFiveInvestment(this.investmentInitService.investmentInitFormData.marketCode, this.convertedDate);
  }
  onChangeCampaignInCamp() {
    this.investmentInitService.getCampaignDtls(this.investmentInitService.investmentCampaignFormData.campaignMstId).subscribe(response => {
      this.campaignDtls = response as ICampaignDtl[];
      this.onChangeSubCampaignInCamp();
    }, error => {
      console.log(error);
    });
  }
  onChangeSubCampaignInCamp() {
    for (var i = 0; i < this.campaignDtls.length; i++) {
      if (this.campaignDtls[i].id == this.investmentInitService.investmentCampaignFormData.campaignDtlId) {
        this.investmentInitService.investmentCampaignFormData.subCampStartDate = new DatePipe('en-US').transform(this.campaignDtls[i].subCampStartDate, 'dd/MM/yyyy');
        this.investmentInitService.investmentCampaignFormData.subCampEndDate = new DatePipe('en-US').transform(this.campaignDtls[i].subCampEndDate, 'dd/MM/yyyy');
        break;
      }
    }
    this.investmentInitService.getCampaignDtlProducts(this.investmentInitService.investmentCampaignFormData.campaignDtlId).subscribe(response => {
      this.campaignDtlProducts = response as ICampaignDtlProduct[];
    }, error => {
      console.log(error);
    });

  }
  onChangeSocietyInSociety() {
    for (var i = 0; i < this.society.length; i++) {
      if (this.society[i].id == this.investmentInitService.investmentSocietyFormData.societyId) {
        this.investmentInitService.investmentSocietyFormData.societyAddress = this.society[i].societyAddress;
        this.investmentInitService.investmentSocietyFormData.noOfMember = this.society[i].noOfMember;

        break;
      }
    }
    this.getLastFiveInvestment(this.investmentInitService.investmentInitFormData.marketCode, this.convertedDate);
  }
  onChangeBcdsInBcds() {
    for (var i = 0; i < this.bcds.length; i++) {
      if (this.bcds[i].id == this.investmentInitService.investmentBcdsFormData.bcdsId) {
        this.investmentInitService.investmentBcdsFormData.bcdsAddress = this.bcds[i].bcdsAddress;
        this.investmentInitService.investmentBcdsFormData.noOfMember = this.bcds[i].noOfMember;
        break;
      }
    }
    this.getLastFiveInvestment(this.investmentInitService.investmentInitFormData.marketCode, this.convertedDate);
  }
  onChangeMarketGroupInTargetedGroup() {
    if (this.investmentTargetedGroups == null || this.investmentTargetedGroups.length == 0) {
      for (let i = 0; i < this.marketGroupMsts.length; i++) {
        if (this.marketGroupMsts[i].id == this.investmentInitService.investmentTargetedGroupFormData.marketGroupMstId) {
          var data = [];
          for (let j = 0; j < this.marketGroupMsts[i].marketGroupDtls.length; j++) {
            if (this.marketGroupMsts[i].marketGroupDtls[j].status == 'Active') {
              var marketGroupMstId = this.marketGroupMsts[i].marketGroupDtls[j].mstId;
              var marketCode = this.marketGroupMsts[i].marketGroupDtls[j].marketCode;
              var marketName = this.marketGroupMsts[i].marketGroupDtls[j].marketName;
              var sbu = this.marketGroupMsts[i].marketGroupDtls[j].sbu;
              var sbuName = this.marketGroupMsts[i].marketGroupDtls[j].sbuName;

              data.push({ id: 0, investmentInitId: this.investmentInitService.investmentInitFormData.id, marketGroupMst: this.marketGroupMsts[i], marketGroupMstId: marketGroupMstId, marketCode: marketCode, marketName: marketName, sbu: sbu, sbuName: sbuName });
              //this.investmentTargetedGroups.push({id:0,investmentInitId:this.investmentInitService.investmentInitFormData.id,marketGroup:null,marketGroupMstId:this.marketGroupMsts[i].marketGroupDtls[j].mstId,marketCode:this.marketGroupMsts[i].marketGroupDtls[j].marketCode,marketName:this.marketGroupMsts[i].marketGroupDtls[j].marketName});
            }
          }
          this.investmentTargetedGroups = data;
          break
        }
      }
    }
    else {
      this.investmentInitService.investmentTargetedGroupFormData.marketGroupMstId=0;
      this.toastr.warning('Already Market Group Exist', 'Investment', {
        positionClass: 'toast-top-right'
      });
    }
  }
  changeDateInDetail() {
    //this.printingDate=this.getDigitBanglaFromEnglish(this.datePipe.transform(value, "dd/MM/yyyy"));
    if (this.investmentInitService.investmentDetailFormData.fromDate == null || this.investmentInitService.investmentDetailFormData.fromDate == undefined) {

      return false;
    }
    if (this.investmentInitService.investmentDetailFormData.toDate == null || this.investmentInitService.investmentDetailFormData.toDate == undefined) {

      return false;
    }
    let dateFrom = this.investmentInitService.investmentDetailFormData.fromDate;
    let dateTo = this.investmentInitService.investmentDetailFormData.toDate;
    //let dateFrom = new Date();
    //let dateTo = new Date();

    this.investmentInitService.investmentDetailFormData.totalMonth = dateTo.getMonth() - dateFrom.getMonth() + (12 * (dateTo.getFullYear() - dateFrom.getFullYear()));
    this.investmentInitService.investmentDetailFormData.totalMonth = this.investmentInitService.investmentDetailFormData.totalMonth + 1;
  }
  changeCommitmentDateInDetail() {
    //this.printingDate=this.getDigitBanglaFromEnglish(this.datePipe.transform(value, "dd/MM/yyyy"));
    if (this.investmentInitService.investmentDetailFormData.commitmentFromDate == null || this.investmentInitService.investmentDetailFormData.commitmentFromDate == undefined) {

      return false;
    }
    if (this.investmentInitService.investmentDetailFormData.commitmentToDate == null || this.investmentInitService.investmentDetailFormData.commitmentToDate == undefined) {

      return false;
    }
    let dateFrom = this.investmentInitService.investmentDetailFormData.commitmentFromDate;
    let dateTo = this.investmentInitService.investmentDetailFormData.commitmentToDate;
    //let dateFrom = new Date();
    //let dateTo = new Date();

    this.investmentInitService.investmentDetailFormData.commitmentTotalMonth = dateTo.getMonth() - dateFrom.getMonth() + (12 * (dateTo.getFullYear() - dateFrom.getFullYear()));
    this.investmentInitService.investmentDetailFormData.commitmentTotalMonth = this.investmentInitService.investmentDetailFormData.commitmentTotalMonth + 1;
  }
  changePaymentMethod() {
    if (this.investmentInitService.investmentDetailFormData.paymentMethod != 'Cheque') {
      this.investmentInitService.investmentDetailFormData.chequeTitle = "" ;
    }
   }
  getDonation() {
    this.investmentInitService.getDonations().subscribe(response => {
      this.donations = response as IDonation[];
    }, error => {
      console.log(error);
    });
  }
  getSubCampaign() {
    this.investmentInitService.getSubCampaigns().subscribe(response => {
      this.subCampaigns = response as ISubCampaign[];
    }, error => {
      console.log(error);
    });
  }
  getDoctor() {
    this.SpinnerService.show();
    this.investmentInitService.getDoctors(this.investmentInitService.investmentInitFormData.marketCode).subscribe(response => {
      this.doctors = response as IDoctor[];
      this.investmentInitService.getInstitutions(this.investmentInitService.investmentInitFormData.marketCode).subscribe(response => {
        this.institutions = response as IInstitution[];
        if (this.investmentInitService.investmentInitFormData.id != null && this.investmentInitService.investmentInitFormData.id != undefined && this.investmentInitService.investmentInitFormData.id != 0) {
          this.getInvestmentDoctor();
        }
        this.SpinnerService.hide();
      }, error => {
        this.SpinnerService.hide();
        console.log(error);
      });

    }, error => {
      this.SpinnerService.hide();
      console.log(error);
    });
  }
  customSearchFnDoc(term: string, item: any) {
    term = term.toLocaleLowerCase();
    return item.doctorCode.toLocaleLowerCase().indexOf(term) > -1 ||
      item.doctorName.toLocaleLowerCase().indexOf(term) > -1;
  }
  getInstitution() {
    this.SpinnerService.show();
    this.investmentInitService.getInstitutions(this.investmentInitService.investmentInitFormData.marketCode).subscribe(response => {
      this.institutions = response as IInstitution[];
      this.investmentInitService.getDoctors(this.investmentInitService.investmentInitFormData.marketCode).subscribe(response => {
        this.doctors = response as IDoctor[];
        if (this.investmentInitService.investmentInitFormData.id != null && this.investmentInitService.investmentInitFormData.id != undefined && this.investmentInitService.investmentInitFormData.id != 0) {
          this.getInvestmentInstitution();
        }
        this.SpinnerService.hide();
      }, error => {
        this.SpinnerService.hide();
        console.log(error);
      });
    }, error => {
      this.SpinnerService.hide();
      console.log(error);
    });
  }
  customSearchFnIns(term: string, item: any) {
    term = term.toLocaleLowerCase();
    return item.institutionCode.toLocaleLowerCase().indexOf(term) > -1 ||
      item.institutionName.toLocaleLowerCase().indexOf(term) > -1;
  }
  getCampaignMst(empId:number) {
    this.SpinnerService.show();
    this.investmentInitService.getCampaignMsts(empId).subscribe(response => {
      this.campaignMsts = response as ICampaignMst[];
      debugger;
      this.investmentInitService.getDoctors(this.investmentInitService.investmentInitFormData.marketCode).subscribe(response => {
        this.doctors = response as IDoctor[];
        this.investmentInitService.getInstitutions(this.investmentInitService.investmentInitFormData.marketCode).subscribe(response => {
          this.institutions = response as IInstitution[];
          if (this.investmentInitService.investmentInitFormData.id != null && this.investmentInitService.investmentInitFormData.id != undefined && this.investmentInitService.investmentInitFormData.id != 0) {
            this.getInvestmentCampaign();
          }
          this.SpinnerService.hide();
        }, error => {
          this.SpinnerService.hide();
          console.log(error);
        });

      }, error => {
        this.SpinnerService.hide();
        console.log(error);
      });
    }, error => {
      this.SpinnerService.hide();
      console.log(error);
    });
  }
  getSociety() {
    this.SpinnerService.show();
    this.investmentInitService.getSociety().subscribe(response => {
      this.society = response as ISocietyInfo[];
      this.investmentInitService.getDoctors(this.investmentInitService.investmentInitFormData.marketCode).subscribe(response => {
        this.doctors = response as IDoctor[];
        this.investmentInitService.investmentSocietyFormData.responsibleDoctorId=900000;
        if (this.investmentInitService.investmentInitFormData.id != null && this.investmentInitService.investmentInitFormData.id != undefined && this.investmentInitService.investmentInitFormData.id != 0) {
          this.getInvestmentSociety();
        }
        this.SpinnerService.hide();
      }, error => {
        this.SpinnerService.hide();
        console.log(error);
      });
    }, error => {
      this.SpinnerService.hide();
      console.log(error);
    });
  }
  getBcds() {
    this.SpinnerService.show();
    this.investmentInitService.getBcds().subscribe(response => {
      this.bcds = response as IBcdsInfo[];
      this.investmentInitService.getDoctors(this.investmentInitService.investmentInitFormData.marketCode).subscribe(response => {
        this.doctors = response as IDoctor[];
        this.investmentInitService.investmentBcdsFormData.responsibleDoctorId=900000;
        if (this.investmentInitService.investmentInitFormData.id != null && this.investmentInitService.investmentInitFormData.id != undefined && this.investmentInitService.investmentInitFormData.id != 0) {
          this.getInvestmentBcds();
        }
        this.SpinnerService.hide();
      }, error => {
        this.SpinnerService.hide();
        console.log(error);
      });
    }, error => {
      this.SpinnerService.hide();
      console.log(error);
    });
  }
  getMarket() {
    this.SpinnerService.show();
    this.investmentInitService.getMarkets().subscribe(response => {
      this.markets = response as IMarket[];
      this.SpinnerService.hide();
    }, error => {
      this.SpinnerService.hide();
      console.log(error);
    });
  }
  getProduct() {
    this.SpinnerService.show();
    this.investmentInitService.getProduct(this.sbu).subscribe(response => {
      this.products = response as IProduct[];
      this.SpinnerService.hide();
    }, error => {
      this.SpinnerService.hide();
      console.log(error);
    });
  }
  getMedicineProds() {
    this.SpinnerService.show();
    this.investmentInitService.getMedicineProduct().subscribe(response => {
      this.medicineProducts = response as IMedicineProduct[];
      this.SpinnerService.hide();
    }, error => {
      this.SpinnerService.hide();
      console.log(error);
    });
  }
  customSearchFnProd(term: string, item: any) {
    term = term.toLocaleLowerCase();
    return item.productCode.toLocaleLowerCase().indexOf(term) > -1 ||
      item.productName.toLocaleLowerCase().indexOf(term) > -1;
  }
  getMarketGroupMsts() {
    this.SpinnerService.show();
    this.investmentInitService.getMarketGroupMsts(this.empId).subscribe(response => {
      this.marketGroupMsts = response as IMarketGroupMst[];
      this.SpinnerService.hide();
    }, error => {
      this.SpinnerService.hide();
      console.log(error);
    });
  }
  onSubmit(form: NgForm) {
    if (this.investmentInitService.investmentInitFormData.id == 0)
      this.insertInvestmentInit();
    else
      if (parseInt(this.empId) == this.investmentInitService.investmentInitFormData.employeeId || this.isAdmin == true) {
        if (this.isSubmitted == true && this.isAdmin == false) {
          this.toastr.warning('This Investment has already been submitted');
          return false;
        }
        this.updateInvestmentInit();
      }
      else {
        this.updateInvestmentInitOther();
      }
  }
  confirmSubmission() {
    this.openSubmissionConfirmModal(this.submissionConfirmModal);
  }
  openSubmissionConfirmModal(template: TemplateRef<any>) {
    this.submissionConfirmRef = this.modalService.show(template, {
      keyboard: false,
      class: 'modal-md',
      ignoreBackdropClick: true
    });
  }
  confirmSubmit() {
    this.submissionConfirmRef.hide();
    this.submitInvestment();
  }
  declineSubmit() {
    this.submissionConfirmRef.hide();
  }
  submitInvestment() {
    if (this.investmentInitService.investmentInitFormData.donationTo == "Doctor") {
      if (this.investmentInitService.investmentDoctorFormData.id == null || this.investmentInitService.investmentDoctorFormData.id == undefined || this.investmentInitService.investmentDoctorFormData.id == 0) {
        this.toastr.warning('Insert Doctor Information First', 'Investment Doctor');
        return false;
      }
    }
    else if (this.investmentInitService.investmentInitFormData.donationTo == "Institution") {
      if (this.investmentInitService.investmentInstitutionFormData.id == null || this.investmentInitService.investmentInstitutionFormData.id == undefined || this.investmentInitService.investmentInstitutionFormData.id == 0) {
        this.toastr.warning('Insert Institution Information First', 'Investment Institution');
        return false;
      }
    }
    else if (this.investmentInitService.investmentInitFormData.donationTo == "Campaign") {
      if (this.investmentInitService.investmentCampaignFormData.id == null || this.investmentInitService.investmentCampaignFormData.id == undefined || this.investmentInitService.investmentCampaignFormData.id == 0) {
        this.toastr.warning('Insert Campaign Information First', 'Investment Campaign');
        return false;
      }
    }
    else if (this.investmentInitService.investmentInitFormData.donationTo == "Bcds") {
      if (this.investmentInitService.investmentBcdsFormData.id == null || this.investmentInitService.investmentBcdsFormData.id == undefined || this.investmentInitService.investmentBcdsFormData.id == 0) {
        this.toastr.warning('Insert Bcds Information First', 'Investment');
        return false;
      }
    }
    else if (this.investmentInitService.investmentInitFormData.donationTo == "Society") {
      if (this.investmentInitService.investmentSocietyFormData.id == null || this.investmentInitService.investmentSocietyFormData.id == undefined || this.investmentInitService.investmentSocietyFormData.id == 0) {
        this.toastr.warning('Insert Society Information First', 'Investment Society');
        return false;
      }
    }
    if (this.investmentInitService.investmentDetailFormData.id == null || this.investmentInitService.investmentDetailFormData.id == undefined || this.investmentInitService.investmentDetailFormData.id == 0) {
      this.toastr.warning('Insert Investment Detail First', 'Investment Detail');
      return false;
    }
    debugger;
    if (this.investmentTargetedGroups.length>0) {
        for (let a = 0; a < this.investmentTargetedGroups.length; a++) {
          if(this.investmentTargetedGroups[a].id == null || this.investmentTargetedGroups[a].id == undefined || this.investmentTargetedGroups[a].id == 0){
            this.toastr.warning('Insert Investment Targeted Group First');
            return false;
        }
      }
    }
    else{
      this.toastr.warning('Insert Investment Targeted Group First');
      return false;
    }
    this.SpinnerService.show();
    this.investmentInitService.submitInvestment().subscribe(
      res => {
        this.investmentInitService.investmentInitFormData = res as IInvestmentInit;
        //this.investmentInitService.investmentDoctorFormData.investmentInitId = this.investmentInitService.investmentInitFormData.id;
        //this.investmentInitService.investmentInstitutionFormData.investmentInitId = this.investmentInitService.investmentInitFormData.id;
        this.isValid = true;
        this.isSubmitted = true;
        this.isInvOther = true;
        this.toastr.success('Submitted successfully', 'Investment')
      },
      err => { 
        console.log(err); 
      }
    );
  }
  insertInvestmentInit() {
    this.SpinnerService.show();
    this.investmentInitService.investmentInitFormData.employeeId = parseInt(this.empId);
    this.investmentInitService.insertInvestmentInit().subscribe(
      res => {
        this.investmentInitService.investmentInitFormData = res as IInvestmentInit;
        //this.investmentInitService.investmentDoctorFormData.investmentInitId = this.investmentInitService.investmentInitFormData.id;
        //this.investmentInitService.investmentInstitutionFormData.investmentInitId = this.investmentInitService.investmentInitFormData.id;
        this.isValid = true;
        this.toastr.success('Saved successfully', 'Investment')
      },
      err => { console.log(err); }
    );
  }
  updateInvestmentInit() {
    this.SpinnerService.show();
    this.investmentInitService.updateInvestmentInit().subscribe(
      res => {
        this.isValid = true;
        this.investmentInitService.investmentDoctorFormData.investmentInitId = this.investmentInitService.investmentInitFormData.id;
        this.investmentInitService.investmentInstitutionFormData.investmentInitId = this.investmentInitService.investmentInitFormData.id;
        this.toastr.info('Updated successfully', 'Investment')
      },
      err => {
        console.log(err);
        //this.toastr.info(err);
      }
    );
  }
  updateInvestmentInitOther() {
    this.SpinnerService.show();
    this.investmentInitService.updateInvestmentInitOther(parseInt(this.empId)).subscribe(
      res => {
        this.getInvestmentTargetedGroup();
        //this.isValid = true;
        //this.investmentInitService.investmentDoctorFormData.investmentInitId = this.investmentInitService.investmentInitFormData.id;
        //this.investmentInitService.investmentInstitutionFormData.investmentInitId = this.investmentInitService.investmentInitFormData.id;
        this.toastr.info('Saved successfully', 'Investment')
      },
      err => { console.log(err); }
    );
  }
  insertInvestmentDetails() {
    debugger;
    if (this.isSubmitted == true) {
      this.toastr.warning('This Investment has already been submitted', 'Investment');
      return false;
    }
    if (this.investmentInitService.investmentInitFormData.id == null || this.investmentInitService.investmentInitFormData.id == undefined || this.investmentInitService.investmentInitFormData.id == 0) {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment Detail');
      return false;
    }
    if (this.investmentInitService.investmentDetailFormData.proposedAmount == null || this.investmentInitService.investmentDetailFormData.proposedAmount == undefined || this.investmentInitService.investmentDetailFormData.proposedAmount == "") {
      this.toastr.warning('Enter Proposed Amount First', 'Investment Detail');
      return false;
    }
    if (this.investmentInitService.investmentDetailFormData.purpose == null || this.investmentInitService.investmentDetailFormData.purpose == undefined || this.investmentInitService.investmentDetailFormData.purpose == "") {
      this.toastr.warning('Enter Purpose First', 'Investment Detail');
      return false;
    }
    if (this.investmentInitService.investmentDetailFormData.paymentFreq == null || this.investmentInitService.investmentDetailFormData.paymentFreq == undefined || this.investmentInitService.investmentDetailFormData.paymentFreq == "") {
      this.toastr.warning('Select Payment Frequency First', 'Investment Detail');
      return false;
    }
    if (this.investmentInitService.investmentDetailFormData.fromDate == null || this.investmentInitService.investmentDetailFormData.fromDate == undefined) {
      this.toastr.warning('Select Payment Dur. From Date  First', 'Investment Detail');
      return false;
    }
    if (this.investmentInitService.investmentDetailFormData.toDate == null || this.investmentInitService.investmentDetailFormData.toDate == undefined) {
      this.toastr.warning('Select Payment Dur. To Date  First', 'Investment Detail');
      return false;
    }
    if (this.investmentInitService.investmentDetailFormData.totalMonth  == null || this.investmentInitService.investmentDetailFormData.totalMonth  == undefined || this.investmentInitService.investmentDetailFormData.totalMonth==0) {
      this.toastr.warning('Invalid Payment Total Month', 'Investment Detail');
      return false;
    }
    if (this.investmentInitService.investmentDetailFormData.commitmentFromDate == null || this.investmentInitService.investmentDetailFormData.commitmentFromDate == undefined) {
      this.toastr.warning('Select Commitment From Date  First', 'Investment Detail');
      return false;
    }
    if (this.investmentInitService.investmentDetailFormData.commitmentToDate == null || this.investmentInitService.investmentDetailFormData.commitmentToDate == undefined) {
      this.toastr.warning('Select Commitment To Date  First', 'Investment Detail');
      return false;
    }
    if (this.investmentInitService.investmentDetailFormData.commitmentTotalMonth  == null || this.investmentInitService.investmentDetailFormData.commitmentTotalMonth  == undefined || this.investmentInitService.investmentDetailFormData.commitmentTotalMonth==0) {
      this.toastr.warning('Invalid Commitment Total Month', 'Investment Detail');
      return false;
    }
    if (this.investmentInitService.investmentDetailFormData.commitmentAllSBU == null || this.investmentInitService.investmentDetailFormData.commitmentAllSBU == undefined || this.investmentInitService.investmentDetailFormData.commitmentAllSBU == "") {
      this.toastr.warning('Enter Commitment All SBU First', 'Investment Detail');
      return false;
    }
    if (this.investmentInitService.investmentDetailFormData.commitmentOwnSBU == null || this.investmentInitService.investmentDetailFormData.commitmentOwnSBU == undefined || this.investmentInitService.investmentDetailFormData.commitmentOwnSBU == "") {
      this.toastr.warning('Enter Commitment Own SBU First', 'Investment Detail');
      return false;
    }
    if (this.investmentInitService.investmentDetailFormData.paymentMethod == null || this.investmentInitService.investmentDetailFormData.paymentMethod == undefined || this.investmentInitService.investmentDetailFormData.paymentMethod == "") {
      this.toastr.warning('Select Payment Method First', 'Investment Detail');
      return false;
    }
    if (this.investmentInitService.investmentDetailFormData.paymentMethod == 'Cheque') {
      if (this.investmentInitService.investmentDetailFormData.chequeTitle == null || this.investmentInitService.investmentDetailFormData.chequeTitle == undefined || this.investmentInitService.investmentDetailFormData.chequeTitle == "") {
        this.toastr.warning('Enter  Cheque Title First', 'Investment Detail');
        return false;
      }
    }
    if (this.investmentInitService.investmentDetailFormData.paymentFreq == 'Quarterly') {
      if (this.investmentInitService.investmentDetailFormData.totalMonth  <3) {
        this.toastr.warning('Duration can not be less than 3 Month for Quarterly Investment ');
        return false;
      }
    }
    if (this.investmentInitService.investmentDetailFormData.paymentFreq == 'Half Yearly') {
      if (this.investmentInitService.investmentDetailFormData.totalMonth  <6) {
        this.toastr.warning('Duration can not be less than 6 Month for Half Yearly Investment');
        return false;
      }
    }
    this.investmentInitService.investmentDetailFormData.investmentInitId = this.investmentInitService.investmentInitFormData.id;
    if (this.investmentInitService.investmentDetailFormData.id == null || this.investmentInitService.investmentDetailFormData.id == undefined || this.investmentInitService.investmentDetailFormData.id == 0) {
      this.SpinnerService.show();
      this.investmentInitService.investmentDetailFormData.fromDate  = this.datePipe.transform(this.investmentInitService.investmentDetailFormData.fromDate, 'yyyy-MM-dd HH:mm:ss');
      this.investmentInitService.investmentDetailFormData.toDate= this.datePipe.transform(this.investmentInitService.investmentDetailFormData.toDate, 'yyyy-MM-dd HH:mm:ss');
      this.investmentInitService.investmentDetailFormData.commitmentFromDate  = this.datePipe.transform(this.investmentInitService.investmentDetailFormData.commitmentFromDate, 'yyyy-MM-dd HH:mm:ss');
      this.investmentInitService.investmentDetailFormData.commitmentToDate= this.datePipe.transform(this.investmentInitService.investmentDetailFormData.commitmentToDate, 'yyyy-MM-dd HH:mm:ss');
    
      this.investmentInitService.insertInvestmentDetail().subscribe(
        res => {
          var data = res as IInvestmentDetail;
          debugger;
          this.investmentInitService.investmentDetailFormData = data;
          //this.investmentInitService.investmentDoctorFormData.doctorName=String(data.doctorId);
          this.investmentInitService.investmentDetailFormData.fromDate = new Date(data.fromDate);
          this.investmentInitService.investmentDetailFormData.toDate = new Date(data.toDate);
          this.investmentInitService.investmentDetailFormData.commitmentFromDate = new Date(data.commitmentFromDate);
          this.investmentInitService.investmentDetailFormData.commitmentToDate = new Date(data.commitmentToDate);
          this.isDonationValid = true;
          this.toastr.success('Save successfully', 'Investment Detail');
        },
        err => { 
          this.investmentInitService.investmentDetailFormData.fromDate = new Date(this.investmentInitService.investmentDetailFormData.fromDate);
          this.investmentInitService.investmentDetailFormData.toDate = new Date(this.investmentInitService.investmentDetailFormData.toDate);
          this.investmentInitService.investmentDetailFormData.commitmentFromDate = new Date(this.investmentInitService.investmentDetailFormData.commitmentFromDate);
          this.investmentInitService.investmentDetailFormData.commitmentToDate = new Date(this.investmentInitService.investmentDetailFormData.commitmentToDate);
          console.log(err); 
        }
      );
    }
    else {
      this.SpinnerService.show();
      this.investmentInitService.investmentDetailFormData.fromDate  = this.datePipe.transform(this.investmentInitService.investmentDetailFormData.fromDate, 'yyyy-MM-dd HH:mm:ss');
      this.investmentInitService.investmentDetailFormData.toDate= this.datePipe.transform(this.investmentInitService.investmentDetailFormData.toDate, 'yyyy-MM-dd HH:mm:ss');
      this.investmentInitService.investmentDetailFormData.commitmentFromDate  = this.datePipe.transform(this.investmentInitService.investmentDetailFormData.commitmentFromDate, 'yyyy-MM-dd HH:mm:ss');
      this.investmentInitService.investmentDetailFormData.commitmentToDate= this.datePipe.transform(this.investmentInitService.investmentDetailFormData.commitmentToDate, 'yyyy-MM-dd HH:mm:ss');
    
      this.investmentInitService.updateInvestmentDetail().subscribe(
        res => {
          var data = res as IInvestmentDetail;
          this.investmentInitService.investmentDetailFormData = data;
          //this.investmentInitService.investmentDoctorFormData.doctorName=String(data.doctorId);
          this.investmentInitService.investmentDetailFormData.fromDate = new Date(data.fromDate);
          this.investmentInitService.investmentDetailFormData.toDate = new Date(data.toDate);
          this.investmentInitService.investmentDetailFormData.commitmentFromDate = new Date(data.commitmentFromDate);
          this.investmentInitService.investmentDetailFormData.commitmentToDate = new Date(data.commitmentToDate);
          this.isDonationValid = true;
          this.toastr.success('Save successfully', 'Investment Detail');
        },
        err => { 
         // this.toastr.warning(err.error.message);
         this.investmentInitService.investmentDetailFormData.fromDate = new Date(this.investmentInitService.investmentDetailFormData.fromDate);
         this.investmentInitService.investmentDetailFormData.toDate = new Date(this.investmentInitService.investmentDetailFormData.toDate);
         this.investmentInitService.investmentDetailFormData.commitmentFromDate = new Date(this.investmentInitService.investmentDetailFormData.commitmentFromDate);
         this.investmentInitService.investmentDetailFormData.commitmentToDate = new Date(this.investmentInitService.investmentDetailFormData.commitmentToDate);
         console.log(err); 
        }
      );
    }

  }
  insertInvestmentDoctor() {
    if (this.isSubmitted == true) {
      this.toastr.warning('This Investment has already been submitted', 'Investment');
      return false;
    }
    if (this.investmentInitService.investmentInitFormData.id == null || this.investmentInitService.investmentInitFormData.id == undefined || this.investmentInitService.investmentInitFormData.id == 0) {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment Doctor');
      return false;
    }
    // if(this.investmentInitService.investmentInitFormData.donationTo!=="Doctor")
    // {
    //   this.updateInvestmentInit();
    // }
    if (this.investmentInitService.investmentDoctorFormData.doctorId == null || this.investmentInitService.investmentDoctorFormData.doctorId == undefined || this.investmentInitService.investmentDoctorFormData.doctorId == 0) {
      this.toastr.warning('Select Doctor First', 'Investment Doctor');
      return false;
    }
    if (this.investmentInitService.investmentDoctorFormData.institutionId == null || this.investmentInitService.investmentDoctorFormData.institutionId == undefined || this.investmentInitService.investmentDoctorFormData.institutionId == 0) {
      this.toastr.warning('Select Institute First', 'Investment Doctor');
      return false;
    }
    this.investmentInitService.investmentDoctorFormData.investmentInitId = this.investmentInitService.investmentInitFormData.id;
    this.SpinnerService.show();
    this.investmentInitService.insertInvestmentDoctor().subscribe(
      res => {
        var data = res as IInvestmentDoctor;
        this.investmentInitService.investmentDoctorFormData = data;
        this.investmentInitService.investmentDoctorFormData.doctorName = String(data.doctorId);
        this.onChangeDoctorInDoc();
        this.onChangeInstitutionInDoc();
        this.updateInvestmentInit();
        this.getLastFiveInvestment(this.investmentInitService.investmentInitFormData.marketCode, this.todayDate);
        this.isDonationValid = true;
        this.toastr.success('Save successfully', 'Investment Doctor');
      },
      err => { console.log(err); }
    );
  }
  insertInvestmentInstitution() {
    if (this.isSubmitted == true) {
      this.toastr.warning('This Investment has already been submitted', 'Investment');
      return false;
    }
    if (this.investmentInitService.investmentInitFormData.id == null || this.investmentInitService.investmentInitFormData.id == undefined || this.investmentInitService.investmentInitFormData.id == 0) {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment Institution');
      return false;
    }
    // if(this.investmentInitService.investmentInitFormData.donationTo!=="Institution")
    // {

    // }
    if (this.investmentInitService.investmentInstitutionFormData.responsibleDoctorId == null || this.investmentInitService.investmentInstitutionFormData.responsibleDoctorId == undefined || this.investmentInitService.investmentInstitutionFormData.responsibleDoctorId == 0) {
      this.toastr.warning('Select Institution First', 'Investment Institution');
      return false;
    }
    if (this.investmentInitService.investmentInstitutionFormData.institutionId == null || this.investmentInitService.investmentInstitutionFormData.institutionId == undefined || this.investmentInitService.investmentInstitutionFormData.institutionId == 0) {
      this.toastr.warning('Select Institute First', 'Investment Institution');
      return false;
    }
    this.investmentInitService.investmentInstitutionFormData.investmentInitId = this.investmentInitService.investmentInitFormData.id;
    this.SpinnerService.show();
    this.investmentInitService.insertInvestmentInstitution().subscribe(
      res => {
        this.investmentInitService.investmentInstitutionFormData = res as IInvestmentInstitution;
        this.onChangeInstitutionInInst();
        this.updateInvestmentInit();
        this.isDonationValid = true;
        this.getLastFiveInvestment(this.investmentInitService.investmentInitFormData.marketCode, this.todayDate);
        this.toastr.success('Save successfully', 'Investment Institution');
      },
      err => { 
        debugger;
        console.log(err); }
    );
  }
  insertInvestmentCampaign() {
    if (this.isSubmitted == true) {
      this.toastr.warning('This Investment has already been submitted', 'Investment');
      return false;
    }
    if (this.investmentInitService.investmentInitFormData.id == null || this.investmentInitService.investmentInitFormData.id == undefined || this.investmentInitService.investmentInitFormData.id == 0) {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment Campaign');
      return false;
    }
    // if(this.investmentInitService.investmentInitFormData.donationTo!=="Institution")
    // {

    // }
    if (this.investmentInitService.investmentCampaignFormData.campaignMstId == null || this.investmentInitService.investmentCampaignFormData.campaignMstId == undefined || this.investmentInitService.investmentCampaignFormData.campaignMstId == 0) {
      this.toastr.warning('Select Campaign First', 'Investment Campaign');
      return false;
    }
    if (this.investmentInitService.investmentCampaignFormData.campaignDtlId == null || this.investmentInitService.investmentCampaignFormData.campaignDtlId == undefined || this.investmentInitService.investmentCampaignFormData.campaignDtlId == 0) {
      this.toastr.warning('Select Sub-Campaign First', 'Investment Campaign');
      return false;
    }
    if (this.investmentInitService.investmentCampaignFormData.doctorId == null || this.investmentInitService.investmentCampaignFormData.doctorId == undefined || this.investmentInitService.investmentCampaignFormData.doctorId == 0) {
      this.toastr.warning('Select Doctor First', 'Investment Campaign');
      return false;
    }
    if (this.investmentInitService.investmentCampaignFormData.institutionId == null || this.investmentInitService.investmentCampaignFormData.institutionId == undefined || this.investmentInitService.investmentCampaignFormData.institutionId == 0) {
      this.toastr.warning('Select Institute First', 'Investment Campaign');
      return false;
    }
    this.investmentInitService.investmentCampaignFormData.investmentInitId = this.investmentInitService.investmentInitFormData.id;

    var tempMstId = this.investmentInitService.investmentCampaignFormData.campaignMstId;
    this.SpinnerService.show();
    this.investmentInitService.insertInvestmentCampaign().subscribe(
      res => {
        this.investmentInitService.investmentCampaignFormData = res as IInvestmentCampaign;
        this.investmentInitService.investmentCampaignFormData.campaignMstId = tempMstId;
        this.onChangeCampaignInCamp();
        //this.onChangeSubCampaignInCamp();
        this.updateInvestmentInit();

        this.isDonationValid = true;
        this.toastr.success('Save successfully', 'Investment Campaign');
      },
      err => { console.log(err); }
    );
  }
  insertInvestmentSociety() {
    if (this.isSubmitted == true) {
      this.toastr.warning('This Investment has already been submitted', 'Investment');
      return false;
    }
    if (this.investmentInitService.investmentInitFormData.id == null || this.investmentInitService.investmentInitFormData.id == undefined || this.investmentInitService.investmentInitFormData.id == 0) {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment Society');
      return false;
    }
    // if(this.investmentInitService.investmentInitFormData.donationTo!=="Institution")
    // {

    // }
    if (this.investmentInitService.investmentSocietyFormData.societyId == null || this.investmentInitService.investmentSocietyFormData.societyId == undefined || this.investmentInitService.investmentSocietyFormData.societyId == 0) {
      this.toastr.warning('Select Society First', 'Investment Society');
      return false;
    }



    this.investmentInitService.investmentSocietyFormData.investmentInitId = this.investmentInitService.investmentInitFormData.id;
    this.SpinnerService.show();
    this.investmentInitService.insertInvestmentSociety().subscribe(
      res => {

        this.investmentInitService.investmentSocietyFormData = res as IInvestmentSociety;
        this.onChangeSocietyInSociety();
        this.updateInvestmentInit();

        this.isDonationValid = true;
        this.getLastFiveInvestment(this.investmentInitService.investmentInitFormData.marketCode, this.todayDate);
        this.toastr.success('Save successfully', 'Investment Society');
      },
      err => { console.log(err); }
    );
  }
  insertInvestmentBcds() {
    if (this.isSubmitted == true) {
      this.toastr.warning('This Investment has already been submitted', 'Investment');
      return false;
    }
    if (this.investmentInitService.investmentInitFormData.id == null || this.investmentInitService.investmentInitFormData.id == undefined || this.investmentInitService.investmentInitFormData.id == 0) {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment Bcds');
      return false;
    }
    // if(this.investmentInitService.investmentInitFormData.donationTo!=="Institution")
    // {

    // }
    if (this.investmentInitService.investmentBcdsFormData.bcdsId == null || this.investmentInitService.investmentBcdsFormData.bcdsId == undefined || this.investmentInitService.investmentBcdsFormData.bcdsId == 0) {
      this.toastr.warning('Select Bcds First', 'Investment Bcds');
      return false;
    }
    this.investmentInitService.investmentBcdsFormData.investmentInitId = this.investmentInitService.investmentInitFormData.id;
    this.SpinnerService.show();
    this.investmentInitService.insertInvestmentBcds().subscribe(
      res => {

        this.investmentInitService.investmentBcdsFormData = res as IInvestmentBcds;
        this.onChangeBcdsInBcds();
        this.updateInvestmentInit();

        this.isDonationValid = true;
        this.getLastFiveInvestment(this.investmentInitService.investmentInitFormData.marketCode, this.todayDate);
        this.toastr.success('Save successfully', 'Investment Bcds');
      },
      err => { console.log(err); }
    );
  }
  insertInvestmentTargetedProd() {
    if (this.investmentInitService.investmentInitFormData.id == null || this.investmentInitService.investmentInitFormData.id == undefined || this.investmentInitService.investmentInitFormData.id == 0) {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment Product');
      return false;
    }

    if (this.investmentInitService.investmentTargetedProdFormData.productId == null || this.investmentInitService.investmentTargetedProdFormData.productId == undefined || this.investmentInitService.investmentTargetedProdFormData.productId == 0) {
      this.toastr.warning('Select Product First', 'Investment Product');
      return false;
    }
    if (this.investmentTargetedProds !== undefined) {
      for (let i = 0; i < this.investmentTargetedProds.length; i++) {
        if (this.investmentTargetedProds[i].productInfo.id === this.investmentInitService.investmentTargetedProdFormData.productId) {
          this.toastr.warning("Product already exist !");
          return false;
        }
      }
    }
    this.investmentInitService.investmentTargetedProdFormData.investmentInitId = this.investmentInitService.investmentInitFormData.id;
    this.investmentInitService.investmentTargetedProdFormData.employeeId = parseInt(this.empId);
    for (let i = 0; i < this.products.length; i++) {
      if (this.investmentInitService.investmentTargetedProdFormData.productId == this.products[i].id) {
        this.investmentInitService.investmentTargetedProdFormData.sbu = this.products[i].sbu;
      }
    }
    if (this.isSubmitted == true && parseInt(this.empId) == this.investmentInitService.investmentInitFormData.employeeId) {
      this.toastr.warning("Investment already submitted");
      return false;
    }
    else {
      if (this.investmentInitService.investmentTargetedProdFormData.id == null || this.investmentInitService.investmentTargetedProdFormData.id == undefined || this.investmentInitService.investmentTargetedProdFormData.id == 0) {
        this.SpinnerService.show();
        this.investmentInitService.insertInvestmentTargetedProd().subscribe(
          res => {
            this.investmentInitService.investmentTargetedProdFormData = new InvestmentTargetedProd();

            this.getInvestmentTargetedProd();

            this.isDonationValid = true;
            this.toastr.success('Save successfully', 'Investment  Product');
          },
          err => { console.log(err); }
        );
      }
      else {
        this.SpinnerService.show();
        this.investmentInitService.updateInvestmentTargetedProd().subscribe(
          res => {
            this.investmentInitService.investmentTargetedProdFormData = new InvestmentTargetedProd();
            this.getInvestmentTargetedProd();
            this.isDonationValid = true;
            this.toastr.success('Update successfully', 'Investment  Product');
          },
          err => { console.log(err); }
        );
      }
    }
  }
  insertInvestmentTargetedGroup() {
    debugger;
    if (this.investmentInitService.investmentInitFormData.id == null || this.investmentInitService.investmentInitFormData.id == undefined || this.investmentInitService.investmentInitFormData.id == 0) {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment Group');
      return false;
    }

    if (this.investmentInitService.investmentTargetedGroupFormData.marketGroupMstId == null || this.investmentInitService.investmentTargetedGroupFormData.marketGroupMstId == undefined || this.investmentInitService.investmentTargetedGroupFormData.marketGroupMstId == 0) {
      this.toastr.warning('Select Market Group First', 'Investment Group');
      return false;
    }
    if (this.investmentTargetedGroups != null && this.investmentTargetedGroups.length > 0) {
      this.investmentInitService.investmentTargetedGroupFormData.investmentInitId = this.investmentInitService.investmentInitFormData.id;
      debugger;
      this.SpinnerService.show();
      //this.investmentInitService.insertInvestmentTargetedGroup(this.investmentTargetedGroups,this.investmentInitService.investmentInitFormData.id).subscribe(
      this.investmentInitService.insertInvestmentTargetedGroup(this.investmentInitService.investmentTargetedGroupFormData.marketGroupMstId,this.investmentInitService.investmentInitFormData.id).subscribe(
        res => {
          this.investmentInitService.investmentTargetedProdFormData = new InvestmentTargetedProd();
          this.getInvestmentTargetedGroup();
          this.isDonationValid = true;
          this.SpinnerService.hide();
          this.toastr.success(res);
        },
        err => {
          this.SpinnerService.hide();
          console.log(err);
        }
      );
    }
    else {
      this.toastr.warning('Select Market Group First', 'Investment Group');
    }
  }
  // editInvestmentTargetedProd(selectedRecord: IInvestmentTargetedProd) {
  //   this.investmentInitService.investmentTargetedProdFormData = Object.assign({}, selectedRecord);
  //   // var e = (document.getElementById("marketCode")) as HTMLSelectElement;
  //   // var sel = e.selectedIndex;
  //   // var opt = e.options[sel];
  //   // var selectedMarketCode = opt.value;
  //   // var selectedMarketName = opt.innerHTML;

  // }
  populateForm() {
    //this.investmentInitService.campaignFormData = Object.assign({}, selectedRecord);
  }
  resetPage(form: NgForm) {
    window.location.reload();
    form.reset();
    this.investmentInitService.investmentInitFormData = new InvestmentInit();
    this.investmentInitService.investmentInitFormData = new InvestmentInit();
    this.investmentInitService.investmentDetailFormData = new InvestmentDetail();
    this.investmentInitService.investmentTargetedProdFormData = new InvestmentTargetedProd();
    this.investmentInitService.investmentTargetedGroupFormData = new InvestmentTargetedGroup();
    this.investmentInitService.investmentDoctorFormData = new InvestmentDoctor();
    this.investmentInitService.investmentInstitutionFormData = new InvestmentInstitution();
    this.investmentInitService.investmentCampaignFormData = new InvestmentCampaign();
    this.investmentInitService.investmentBcdsFormData = new InvestmentBcds();
    this.investmentInitService.investmentSocietyFormData = new InvestmentSociety();
    this.investmentInitService.investmentMedicineProdFormData = new InvestmentMedicineProd();
    this.investmentInitService.investmentInitFormData.marketCode = this.marketCode;
    this.isValid = false;
    this.isAdmin = false;
    this.isSubmitted = false;
    this.isInvOther = false;
    this.isDonationValid = false;
    this.investmentTargetedGroups = [];
    this.investmentTargetedProds = [];
    this.lastFiveInvestmentDetail = [];
    // this.configs = {
    //   currentPage: 1,
    //   itemsPerPage: 10,
    //   totalItems: 50,
    // };
  }
  resetPageLoad() {
    //window.location.reload();
    this.investmentInitService.investmentInitFormData = new InvestmentInit();
    this.investmentInitService.investmentInitFormData = new InvestmentInit();
    this.investmentInitService.investmentDetailFormData = new InvestmentDetail();
    this.investmentInitService.investmentTargetedProdFormData = new InvestmentTargetedProd();
    this.investmentInitService.investmentTargetedGroupFormData = new InvestmentTargetedGroup();
    this.investmentInitService.investmentDoctorFormData = new InvestmentDoctor();
    this.investmentInitService.investmentInstitutionFormData = new InvestmentInstitution();
    this.investmentInitService.investmentCampaignFormData = new InvestmentCampaign();
    this.investmentInitService.investmentBcdsFormData = new InvestmentBcds();
    this.investmentInitService.investmentSocietyFormData = new InvestmentSociety();
    this.investmentInitService.investmentMedicineProdFormData = new InvestmentMedicineProd();
    this.degree='';
    this.designation='';
    this.docInstaddress='';
    this.instaddress='';
    this.investmentInitService.investmentInitFormData.marketCode = this.marketCode;
    this.isAdmin = false;
    this.isValid = false;
    this.isSubmitted = false;
    this.isInvOther = false;
    this.isDonationValid = false;
    this.investmentTargetedGroups = [];
    this.investmentTargetedProds = [];
    this.lastFiveInvestmentDetail = [];
    // this.configs = {
    //   currentPage: 1,
    //   itemsPerPage: 10,
    //   totalItems: 50,
    // };
  }
  removeInvestmentDoctor() {
    var c = confirm("Are you sure you want to delete that?");
    if (c == true) {
      if (this.isSubmitted == true) {
        this.toastr.warning('This Investment has already been submitted', 'Investment');
        return false;
      }
      this.SpinnerService.show();
      this.investmentInitService.removeInvestmentDoctor().subscribe(
        res => {
          this.SpinnerService.hide();
          this.toastr.success(res);
          this.isDonationValid = false;
          this.investmentInitService.investmentDoctorFormData = new InvestmentDoctor();
        },
        err => {
          this.SpinnerService.hide();
          console.log(err);
        }
      );
    }
  }
  removeInvestmentInstitution() {
    var c = confirm("Are you sure you want to delete that?");
    if (c == true) {
      if (this.isSubmitted == true) {
        this.toastr.warning('This Investment has already been submitted', 'Investment');
        return false;
      }
      this.SpinnerService.show();
      this.investmentInitService.removeInvestmentInstitution().subscribe(
        res => {
          this.SpinnerService.hide();
          this.toastr.success(res);
          this.isDonationValid = false;
          this.investmentInitService.investmentInstitutionFormData = new InvestmentInstitution();
        },
        err => {
          this.SpinnerService.hide();
          console.log(err);
        }
      );
    }
  }
  removeInvestmentCampaign() {
    var c = confirm("Are you sure you want to delete that?");
    if (c == true) {
      if (this.isSubmitted == true) {
        this.toastr.warning('This Investment has already been submitted', 'Investment');
        return false;
      }
      this.SpinnerService.show();
      this.investmentInitService.removeInvestmentCampaign().subscribe(
        res => {
          this.SpinnerService.hide();
          this.toastr.success(res);
          this.isDonationValid = false;
          this.investmentInitService.investmentCampaignFormData = new InvestmentCampaign();
        },
        err => {
          this.SpinnerService.hide();
          console.log(err);
        }
      );
    }
  }
  removeInvestmentSociety() {
    var c = confirm("Are you sure you want to delete that?");
    if (c == true) {
      if (this.isSubmitted == true) {
        this.toastr.warning('This Investment has already been submitted', 'Investment');
        return false;
      }
      this.SpinnerService.show();
      this.investmentInitService.removeInvestmentSociety().subscribe(
        res => {
          this.SpinnerService.hide();
          this.toastr.success(res);
          this.isDonationValid = false;
          this.investmentInitService.investmentSocietyFormData = new InvestmentSociety();
        },
        err => {
          this.SpinnerService.hide();
          console.log(err);
        }
      );
    }
  }
  removeInvestmentBcds() {
    var c = confirm("Are you sure you want to delete that?");
    if (c == true) {
      if (this.isSubmitted == true) {
        this.toastr.warning('This Investment has already been submitted', 'Investment');
        return false;
      }
      this.SpinnerService.show();
      this.investmentInitService.removeInvestmentBcds().subscribe(
        res => {
          this.isDonationValid = false;
          this.investmentInitService.investmentBcdsFormData = new InvestmentBcds();
          this.SpinnerService.hide();
          this.toastr.success(res);
        },
        err => {
          this.SpinnerService.hide();
          console.log(err);
        }
      );
    }
  }
  
  
 
  // onPageChanged(event: any) {
  //   const params = this.investmentInitService.getGenParams();
  //   if (params.pageIndex !== event) {
  //     params.pageIndex = event;
  //     this.investmentInitService.setGenParams(params);
  //     this.getInvestmentInitPgChange();
  //   }
  // }
  // getInvestmentInitPgChange() {
  //   const params = this.investmentInitService.getGenParams();
  //   this.SpinnerService.show();
  //   this.investmentInitService.getInvestmentInit(parseInt(this.empId), this.sbu, this.userRole).subscribe(response => {
  //     this.SpinnerService.hide();
  //     this.investmentInits = response.data;
  //     this.totalCount = response.count;
  //     this.configs = {
  //       currentPage: params.pageIndex,
  //       itemsPerPage: params.pageSize,
  //       totalItems: this.totalCount,
  //     };
  //   }, error => {
  //     this.SpinnerService.hide();
  //     console.log(error);
  //   });
  // }
  resetSearch() {
    this.searchText = '';
  }
}

