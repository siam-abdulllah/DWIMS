import { IEmployeeLocation } from './../shared/models/empLocation';
import {
  InvestmentApr, IInvestmentApr, InvestmentInit, IInvestmentInit,
  InvestmentTargetedProd, IInvestmentTargetedProd, InvestmentTargetedGroup, IInvestmentTargetedGroup, IInvestmentAprComment, InvestmentAprComment
} from '../shared/models/investmentApr';
import { InvestmentDoctor, IInvestmentDoctor, InvestmentInstitution, IInvestmentInstitution, InvestmentCampaign, IInvestmentCampaign } from '../shared/models/investmentApr';
import { InvestmentBcds, IInvestmentBcds, InvestmentSociety, IInvestmentSociety } from '../shared/models/investmentApr';
import { SubCampaign, ISubCampaign } from '../shared/models/subCampaign';
import { Donation, IDonation } from '../shared/models/donation';
import { Doctor, IDoctor } from '../shared/models/docotor';
import { EmployeeLocation } from '../shared/models/emplocation';
import { Institution, IInstitution } from '../shared/models/institution';
import { GenericParams } from '../shared/models/genericParams';
import { Component, ElementRef, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { InvestmentAprNoSbuService } from '../_services/investmentAprNoSbu.service';
import { NgForm } from '@angular/forms';
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
import { IInvestmentDetailOld, IInvestmentMedicineProd, ILastFiveInvestmentDetail, InvestmentMedicineProd } from '../shared/models/investment';
import { NgxSpinnerService } from 'ngx-spinner';
import { IBudgetCeiling } from '../shared/models/budgetCeiling';
import { IDepotInfo } from '../shared/models/depotInfo';
import { IInvestmentRecDepot, InvestmentRecDepot } from '../shared/models/InvestmentRecDepot';
import { IMedicineProduct } from '../shared/models/medicineProduct';
import { BDCurrencyPipe } from '../bdNumberPipe';

@Component({
  selector: 'app-investmentAprNoSbu',
  templateUrl: './investmentAprNoSbu.component.html',
  styles: [
  ],
  providers: [DatePipe]
})
export class InvestmentAprNoSbuComponent implements OnInit {
  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  @ViewChild('investmentInitSearchModal', { static: false }) investmentInitSearchModal: TemplateRef<any>;
  @ViewChild('investmentAprSearchModal', { static: false }) investmentAprSearchModal: TemplateRef<any>;
  InvestmentInitSearchModalRef: BsModalRef;
  InvestmentAprSearchModalRef: BsModalRef;
  investmentMedicineProds: IInvestmentMedicineProd[];
  investmentDetailsOld: IInvestmentDetailOld[];
  lastFiveInvestmentDetail: ILastFiveInvestmentDetail[];
  investmentAprs: IInvestmentApr[];
  investmentInits: IInvestmentInit[];
  empLocation: IEmployeeLocation[];
  investmentTargetedProds: IInvestmentTargetedProd[];
  investmentTargetedGroups: IInvestmentTargetedGroup[];
  investmentDetails: IInvestmentApr[];
  investmentDoctors: IInvestmentDoctor[];
  isValid: boolean = false;
  isInvOther: boolean = false;
  isBudgetVisible: boolean = false;
  isDonationValid: boolean = false;
  searchText = '';
  minDate: Date;
  maxDate: Date;
  //configs: any;
  numberPattern = "^[0-9]+(.[0-9]{1,10})?$";
  depots: IDepotInfo[];
  bcds: IBcdsInfo[];
  society: ISocietyInfo[];
  markets: IMarket[];
  products: IProduct[];
  medicineProducts: IMedicineProduct[];
  budgetCeiling: IBudgetCeiling;
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
  isAdmin: boolean = false;
  isDepotRequire: boolean = true;
  empId: string;
  donationName: string;
  sbu: string;
  config = {
    keyboard: false,
    class: 'modal-lg',
    ignoreBackdropClick: true
  };
  userRole: any;
  convertedDate: string;
  constructor(private accountService: AccountService, public BDCurrency: BDCurrencyPipe, public investmentAprService: InvestmentAprNoSbuService, private router: Router,
    private toastr: ToastrService, private modalService: BsModalService, private datePipe: DatePipe, private SpinnerService: NgxSpinnerService) { }
  ngOnInit() {
    this.resetForm();
    this.getEmployeeId();
    this.getDonation();
    this.bsConfig = Object.assign({}, { containerClass: 'theme-blue' }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();
    const currentDate = new Date();
    this.minDate = new Date(currentDate.getFullYear(), 0, 1);
    this.maxDate = new Date(currentDate.getFullYear(), 11, 31);
  }
  onSubmit(form: NgForm) {
    if (this.investmentAprService.investmentAprCommentFormData.id == null || this.investmentAprService.investmentAprCommentFormData.id == undefined || this.investmentAprService.investmentAprCommentFormData.id == 0)
      this.insertInvestmentApr();
    else
      this.updateInvestmentApr();
  }
  getDonation() {
    this.investmentAprService.getDonations().subscribe(response => {
      this.donations = response as IDonation[];
    }, error => {
      console.log(error);
    });
  }
  getDepot() {
    this.investmentAprService.getDepot().subscribe(response => {
      this.depots = response as IDepotInfo[];
    }, error => {
      console.log(error);
    });
  }
  openInvestmentInitSearchModal(template: TemplateRef<any>) {
    this.InvestmentInitSearchModalRef = this.modalService.show(template, this.config);
  }
  openInvestmentAprSearchModal(template: TemplateRef<any>) {
    this.InvestmentAprSearchModalRef = this.modalService.show(template, this.config);
  }
  async selectInvestmentInit(selectedAprord: IInvestmentInit) {
    this.resetForm();
    this.investmentAprService.investmentAprFormData = Object.assign({}, selectedAprord);
    var selectedDonation = this.donations.filter(res => res.id == selectedAprord.donationId).map(ele => ele.donationTypeName);
    this.donationName = selectedDonation[0];
    this.investmentAprService.investmentDetailFormData.investmentInitId = selectedAprord.id;
    this.investmentAprService.investmentAprCommentFormData.investmentInitId = selectedAprord.id;
    this.isDonationValid = true;
    this.convertedDate = this.datePipe.transform(selectedAprord.setOn, 'ddMMyyyy');
    if (this.investmentAprService.investmentAprFormData.donationTo == "Doctor") {
      await this.getInvestmentDoctor();
    }
    else if (this.investmentAprService.investmentAprFormData.donationTo == "Institution") {
      await this.getInvestmentInstitution();
    }
    else if (this.investmentAprService.investmentAprFormData.donationTo == "Campaign") {
      await this.getInvestmentCampaign();
    }
    else if (this.investmentAprService.investmentAprFormData.donationTo == "Bcds") {
      await this.getInvestmentBcds();
    }
    else if (this.investmentAprService.investmentAprFormData.donationTo == "Society") {
      await this.getInvestmentSociety();
    }
    await this.getInvestmentDetails();
    if (this.investmentAprService.investmentAprFormData.donationId == 4) {
      this.getInvestmentMedicineProd();

    }
    await this.getEmployeeLocation();
    await this.getInvestmentTargetedProd();
    await this.getInvestmentTargetedGroup();
    //if (this.sbu == this.investmentAprService.investmentAprFormData.sbu) {
    this.isInvOther = false;
    this.isValid = true;
    this.getBudget();

    this.InvestmentInitSearchModalRef.hide()
  }
  async selectInvestmentApr(selectedAprord: IInvestmentInit) {
    this.resetForm();
    this.investmentAprService.investmentAprFormData = Object.assign({}, selectedAprord);
    var selectedDonation = this.donations.filter(res => res.id == selectedAprord.donationId).map(ele => ele.donationTypeName);
    this.donationName = selectedDonation[0];
    this.investmentAprService.investmentDetailFormData.investmentInitId = selectedAprord.id;
    this.investmentAprService.investmentAprCommentFormData.investmentInitId = selectedAprord.id;
    this.isDonationValid = true;
    this.convertedDate = this.datePipe.transform(selectedAprord.setOn, 'ddMMyyyy');
    if (this.investmentAprService.investmentAprFormData.donationTo == "Doctor") {
      await this.getInvestmentDoctor();
    }
    else if (this.investmentAprService.investmentAprFormData.donationTo == "Institution") {
      await this.getInvestmentInstitution();
    }
    else if (this.investmentAprService.investmentAprFormData.donationTo == "Campaign") {
      await this.getInvestmentCampaign();
    }
    else if (this.investmentAprService.investmentAprFormData.donationTo == "Bcds") {
      await this.getInvestmentBcds();
    }
    else if (this.investmentAprService.investmentAprFormData.donationTo == "Society") {
      await this.getInvestmentSociety();
    }
    await this.getInvestmentAprDetails();
    if (this.investmentAprService.investmentAprFormData.donationId == 4) {
      this.getInvestmentMedicineProd();
    }
    await this.getInvestmentAprProducts();
    await this.getEmployeeLocation();
    await this.getInvestmentAprComment();
    await this.getInvestmentTargetedGroup();
    //if (this.sbu == this.investmentAprService.investmentAprFormData.sbu) {
    this.isInvOther = false;
    this.isValid = true;
    await this.getBudget();

    this.InvestmentAprSearchModalRef.hide()
  }
  async getLastFiveInvestment(marketCode: string, toDayDate: string) {
    if (this.investmentAprService.investmentAprFormData.donationTo == "Doctor") {
      await this.investmentAprService.getLastFiveInvestmentForDoc(this.investmentAprService.investmentAprFormData.donationId, this.investmentAprService.investmentDoctorFormData.doctorId, marketCode, toDayDate).then(
        (response) => {
          this.lastFiveInvestmentDetail = response as ILastFiveInvestmentDetail[];
        },
        (error) => {
          console.log(error);
        }
      );
    }
    else if (this.investmentAprService.investmentAprFormData.donationTo == "Institution") {
      await this.investmentAprService.getLastFiveInvestmentForInstitute(this.investmentAprService.investmentAprFormData.donationId, this.investmentAprService.investmentInstitutionFormData.institutionId, marketCode, toDayDate).then(
        (response) => {
          this.lastFiveInvestmentDetail = response as ILastFiveInvestmentDetail[];
        },
        (error) => {
          console.log(error);
        }
      );
    }
    else if (this.investmentAprService.investmentAprFormData.donationTo == "Campaign") {
      await this.investmentAprService.getLastFiveInvestmentForCampaign(this.investmentAprService.investmentAprFormData.donationId, this.investmentAprService.investmentCampaignFormData.campaignMstId, marketCode, toDayDate).then(
        (response) => {
          this.lastFiveInvestmentDetail = response as ILastFiveInvestmentDetail[];
        },
        (error) => {
          console.log(error);
        }
      );
    }
    else if (this.investmentAprService.investmentAprFormData.donationTo == "Bcds") {
      await this.investmentAprService.getLastFiveInvestmentForBcds(this.investmentAprService.investmentAprFormData.donationId, this.investmentAprService.investmentBcdsFormData.bcdsId, marketCode, toDayDate).then(
        (response) => {
          this.lastFiveInvestmentDetail = response as ILastFiveInvestmentDetail[];
        },
        (error) => {
          console.log(error);
        }
      );
    }
    else if (this.investmentAprService.investmentAprFormData.donationTo == "Society") {
      await this.investmentAprService.getLastFiveInvestmentForSociety(this.investmentAprService.investmentAprFormData.donationId, this.investmentAprService.investmentSocietyFormData.societyId, marketCode, toDayDate).then(
        (response) => {
          this.lastFiveInvestmentDetail = response as ILastFiveInvestmentDetail[];
        },
        (error) => {
          console.log(error);
        }
      );
    }

  }
  // getCampaignMst() {
  //   this.investmentAprService.getCampaignMsts().subscribe(response => {
  //     this.campaignMsts = response as ICampaignMst[];
  //   }, error => {
  //     console.log(error);
  //   });
  // }
  getInvestmentInit() {
    this.SpinnerService.show();
    this.investmentAprService.getInvestmentInit(parseInt(this.empId), this.sbu).subscribe(response => {
      this.SpinnerService.hide();
      this.investmentInits = response as IInvestmentInit[];
      if (this.investmentInits.length > 0) {
        this.openInvestmentInitSearchModal(this.investmentInitSearchModal);
      }
      else {
        debugger;
        this.toastr.warning('No Data Found');
      }
    }, error => {
      this.SpinnerService.hide();
      console.log(error);
    });
  }
  getInvestmentApproved() {
    const params = this.investmentAprService.getGenParams();
    this.SpinnerService.show();
    this.investmentAprService.getInvestmentApproved(parseInt(this.empId), this.sbu, this.userRole).subscribe(response => {
      this.SpinnerService.hide();
      this.investmentInits = response as IInvestmentInit[];;
      //this.totalCount = response.count;
      debugger;
      // this.configs = {
      //   currentPage: params.pageIndex,
      //   itemsPerPage: params.pageSize,
      //   totalItems: this.totalCount,
      // };
      if (this.investmentInits.length > 0) {
        this.openInvestmentAprSearchModal(this.investmentAprSearchModal);
      }
      else {
        debugger;
        this.toastr.warning('No Data Found');
      }
    }, error => {
      this.SpinnerService.hide();
      console.log(error);
    });
  }

  getInvestmentCampaign() {
    this.investmentAprService.getInvestmentCampaigns(this.investmentAprService.investmentAprFormData.id).subscribe(response => {
      var data = response[0] as IInvestmentCampaign;
      if (data !== undefined) {
        this.investmentAprService.investmentCampaignFormData = data;
        this.investmentAprService.investmentCampaignFormData.campaignMstId = data.campaignDtl.mstId;
        this.investmentAprService.investmentCampaignFormData.subCampaignName = data.campaignDtl.subCampaignName;
        this.investmentAprService.investmentCampaignFormData.doctorName = data.doctorInfo.doctorName;
        this.investmentAprService.investmentCampaignFormData.institutionName = data.institutionInfo.institutionName;
        this.investmentAprService.investmentCampaignFormData.subCampStartDate = new DatePipe('en-US').transform(data.campaignDtl.subCampStartDate, 'dd/MM/yyyy');
        this.investmentAprService.investmentCampaignFormData.subCampEndDate = new DatePipe('en-US').transform(data.campaignDtl.subCampEndDate, 'dd/MM/yyyy')
        this.investmentAprService.getCampaignMsts(this.investmentAprService.investmentAprFormData.employeeId).subscribe(response => {
          this.campaignMsts = response as ICampaignMst[];
          for (let i = 0; i < this.campaignMsts.length; i++) {
            if (this.campaignMsts[i].id == this.investmentAprService.investmentCampaignFormData.campaignDtl.mstId) {
              this.investmentAprService.investmentCampaignFormData.campaignName = this.campaignMsts[i].campaignName;
            }
          }
          this.investmentAprService.getCampaignDtls(data.campaignDtl.mstId).subscribe(response => {
            this.campaignDtls = response as ICampaignDtl[];
            for (let i = 0; i < this.campaignDtls.length; i++) {
              if (this.campaignDtls[i].id == data.campaignDtl.id) {
                this.investmentAprService.investmentCampaignFormData.subCampaignName = this.campaignDtls[i].subCampaign.subCampaignName;
              }
            }
          }, error => {
            console.log(error);
          });
        }, error => {
          console.log(error);
        });
        this.investmentAprService.getCampaignDtlProducts(data.campaignDtl.id).subscribe(response => {
          this.campaignDtlProducts = response as ICampaignDtlProduct[];
        }, error => {
          console.log(error);
        });
      }
      // else {
      //   this.toastr.warning('No Data Found', 'Investment ');
      // }
    }, error => {
      console.log(error);
    });

  }
  async getInvestmentBcds() {
    await this.investmentAprService.getInvestmentBcds(this.investmentAprService.investmentAprFormData.id).then(response => {
      var data = response[0] as IInvestmentBcds;
      if (data !== undefined) {
        this.investmentAprService.investmentBcdsFormData = data;
        this.investmentAprService.investmentBcdsFormData.responsibleDoctorName = data.doctorInfo.doctorName;
        this.investmentAprService.investmentBcdsFormData.bcdsName = data.bcds.bcdsName;
        this.investmentAprService.investmentBcdsFormData.bcdsAddress = data.bcds.bcdsAddress;
        this.investmentAprService.investmentBcdsFormData.noOfMember = data.bcds.noOfMember;
      }
      // else {
      //   this.toastr.warning('No Data Found', 'Investment ');
      // }

    }, error => {
      console.log(error);
    });
  }
  async getInvestmentSociety() {
    await this.investmentAprService.getInvestmentSociety(this.investmentAprService.investmentAprFormData.id).then(response => {
      var data = response[0] as IInvestmentSociety;
      if (data !== undefined) {
        this.investmentAprService.investmentSocietyFormData = data;
        this.investmentAprService.investmentSocietyFormData.responsibleDoctorName = data.doctorInfo.doctorName;
        this.investmentAprService.investmentSocietyFormData.societyName = data.society.societyName;
        this.investmentAprService.investmentSocietyFormData.noOfMember = data.society.noOfMember;
        this.investmentAprService.investmentSocietyFormData.societyAddress = data.society.societyAddress;
      }
      // else {
      //   this.toastr.warning('No Data Found', 'Investment ');
      // }
    }, error => {
      console.log(error);
    });
  }
  async getInvestmentRecDepot() {
    debugger;
    await this.investmentAprService.getInvestmentRecDepot(this.investmentAprService.investmentAprFormData.id).then(response => {
      debugger;
      this.investmentAprService.investmentDepotFormData = response as IInvestmentRecDepot;
    }, error => {
      console.log(error);
    });
  }
  async getInvestmentInstitution() {
    await this.investmentAprService.getInvestmentInstitutions(this.investmentAprService.investmentAprFormData.id).then(response => {
      var data = response[0] as IInvestmentInstitution;
      if (data !== undefined) {
        this.investmentAprService.investmentInstitutionFormData = data;
        this.investmentAprService.investmentInstitutionFormData.responsibleDoctorName = data.doctorInfo.doctorName;
        this.investmentAprService.investmentInstitutionFormData.institutionName = data.institutionInfo.institutionName;
        this.investmentAprService.investmentInstitutionFormData.address = data.institutionInfo.address;
        this.investmentAprService.investmentInstitutionFormData.institutionType = data.institutionInfo.institutionType;
      }
      // else {
      //   this.toastr.warning('No Data Found', 'Investment ');
      // }

    }, error => {
      console.log(error);
    });
  }
  async getInvestmentDoctor() {
    await this.investmentAprService.getInvestmentDoctors(this.investmentAprService.investmentAprFormData.id).then(response => {
      var data = response[0] as IInvestmentDoctor;
      if (data !== undefined) {
        this.investmentAprService.investmentDoctorFormData = data;
        this.investmentAprService.investmentDoctorFormData.doctorName = data.doctorInfo.doctorName;
        this.investmentAprService.investmentDoctorFormData.degree = data.doctorInfo.degree;
        this.investmentAprService.investmentDoctorFormData.designation = data.doctorInfo.designation;
        this.investmentAprService.investmentDoctorFormData.institutionName = data.institutionInfo.institutionName;
        this.investmentAprService.investmentDoctorFormData.address = data.institutionInfo.address;
      }
      // else {
      //   this.toastr.warning('No Data Found', 'Investment ');
      // }
    }, error => {
      console.log(error);
    });
  }
  getInvestmentAprComment() {
    this.investmentAprService.getInvestmentAprComment(this.investmentAprService.investmentAprFormData.id, this.empId).subscribe(response => {
      var data = response[0] as IInvestmentAprComment;
      if (data !== undefined) {
        this.investmentAprService.investmentAprCommentFormData = data;
      }
      // else {
      //   this.toastr.warning('No Data Found', 'Investment ');
      // }

    }, error => {
      console.log(error);
    });
  }
  async getInvestmentDetails() {
    await this.investmentAprService.getInvestmentDetails(this.investmentAprService.investmentAprFormData.id, parseInt(this.empId)).then(response => {
      var data = response[0] as IInvestmentApr;
      if (data !== undefined) {
        this.investmentAprService.investmentDetailFormData = data;
        this.investmentAprService.investmentDetailFormData.id = 0;
        this.investmentAprService.investmentDetailFormData.fromDate = new Date(data.fromDate);
        this.investmentAprService.investmentDetailFormData.toDate = new Date(data.toDate);
        this.investmentAprService.investmentDetailFormData.commitmentFromDate = new Date(this.investmentAprService.investmentDetailFormData.commitmentFromDate);
        this.investmentAprService.investmentDetailFormData.commitmentToDate = new Date(this.investmentAprService.investmentDetailFormData.commitmentToDate);
       
        if (data.paymentMethod == 'Cash') {
          this.getInvestmentRecDepot();
          this.isDepotRequire = false;
        }
        else {
          this.isDepotRequire = false;
        }
        //let convertedDate = this.datePipe.transform(data.fromDate, 'ddMMyyyy');
        this.getLastFiveInvestment(this.investmentAprService.investmentAprFormData.marketCode, this.convertedDate);
      }
      // else {
      //   this.toastr.warning('No Data Found', 'Investment');
      // }
    }, error => {
      console.log(error);
    });
  }
  getInvestmentTargetedProd() {
    this.investmentAprService.getInvestmentTargetedProds(this.investmentAprService.investmentAprFormData.id, this.sbu).subscribe(response => {
      var data = response as IInvestmentTargetedProd[];
      if (data !== undefined) {
        this.investmentTargetedProds = data;

      }
      // else {
      //   this.toastr.warning('No Data Found', 'Investment ');
      // }

    }, error => {
      console.log(error);
    });
  }
  async getInvestmentAprDetails() {
    await this.investmentAprService.getInvestmentAprDetails(this.investmentAprService.investmentAprFormData.id, parseInt(this.empId)).then(response => {
      var data = response[0] as IInvestmentApr;
      if (data !== undefined) {
        this.investmentAprService.investmentDetailFormData = data;
        this.investmentAprService.investmentDetailFormData.id = 0;
        this.investmentAprService.investmentDetailFormData.fromDate = new Date(data.fromDate);
        this.investmentAprService.investmentDetailFormData.toDate = new Date(data.toDate);
        this.investmentAprService.investmentDetailFormData.commitmentFromDate = new Date(this.investmentAprService.investmentDetailFormData.commitmentFromDate);
        this.investmentAprService.investmentDetailFormData.commitmentToDate = new Date(this.investmentAprService.investmentDetailFormData.commitmentToDate);
       
        if (data.paymentMethod == 'Cash') {
          this.getInvestmentRecDepot();
          this.isDepotRequire = false;
        }
        else {
          this.isDepotRequire = false;
        }
        //let convertedDate = this.datePipe.transform(data.fromDate, 'ddMMyyyy');
        this.getLastFiveInvestment(this.investmentAprService.investmentAprFormData.marketCode, this.convertedDate);
      } else {
        this.getInvestmentDetails();
      }
    }, error => {
      console.log(error);
    });
  }
  getInvestmentAprProducts() {
    this.investmentAprService.getInvestmentAprProducts(this.investmentAprService.investmentAprFormData.id, this.sbu).subscribe(response => {
      var data = response as IInvestmentTargetedProd[];
      if (data !== undefined) {
        this.investmentTargetedProds = data;
      }
      // else {
      //   this.toastr.warning('No Data Found', 'Investment ');
      // }
    }, error => {
      console.log(error);
    });
  }
  getEmployeeLocation() {
    this.investmentAprService.getEmpLoc(this.investmentAprService.investmentAprFormData.id).subscribe(response => {
      var data = response as IEmployeeLocation[];
      if (data !== undefined) {
        this.empLocation = data;
      }
    }, error => {
      console.log(error);
    });
  }
  getInvestmentTargetedGroup() {
    this.investmentAprService.getInvestmentTargetedGroups(this.investmentAprService.investmentAprFormData.id, parseInt(this.empId)).subscribe(response => {
      var data = response as IInvestmentTargetedGroup[];
      if (data !== undefined) {
        this.investmentTargetedGroups = data;
      }
      // else {
      //   this.toastr.warning('No Data Found', 'Investment ');
      // }
    }, error => {
      console.log(error);
    });
  }
  changeDateInDetail() {
    if (this.investmentAprService.investmentDetailFormData.fromDate == null || this.investmentAprService.investmentDetailFormData.fromDate == undefined) {
      return false;
    }
    if (this.investmentAprService.investmentDetailFormData.toDate == null || this.investmentAprService.investmentDetailFormData.toDate == undefined) {
      return false;
    }
    let dateFrom = this.investmentAprService.investmentDetailFormData.fromDate;
    let dateTo = this.investmentAprService.investmentDetailFormData.toDate;
    this.investmentAprService.investmentDetailFormData.totalMonth = dateTo.getMonth() - dateFrom.getMonth() + (12 * (dateTo.getFullYear() - dateFrom.getFullYear()));
    this.investmentAprService.investmentDetailFormData.totalMonth = this.investmentAprService.investmentDetailFormData.totalMonth + 1;
  }
  changeCommitmentDateInDetail() {
    //this.printingDate=this.getDigitBanglaFromEnglish(this.datePipe.transform(value, "dd/MM/yyyy"));
    if (this.investmentAprService.investmentDetailFormData.commitmentFromDate == null || this.investmentAprService.investmentDetailFormData.commitmentFromDate == undefined) {

      return false;
    }
    if (this.investmentAprService.investmentDetailFormData.commitmentToDate == null || this.investmentAprService.investmentDetailFormData.commitmentToDate == undefined) {

      return false;
    }
    let dateFrom = this.investmentAprService.investmentDetailFormData.commitmentFromDate;
    let dateTo = this.investmentAprService.investmentDetailFormData.commitmentToDate;
    //let dateFrom = new Date();
    //let dateTo = new Date();

    this.investmentAprService.investmentDetailFormData.commitmentTotalMonth = dateTo.getMonth() - dateFrom.getMonth() + (12 * (dateTo.getFullYear() - dateFrom.getFullYear()));
    this.investmentAprService.investmentDetailFormData.commitmentTotalMonth = this.investmentAprService.investmentDetailFormData.commitmentTotalMonth + 1;
  }
  dateCompare(form: NgForm) {
    if (this.investmentAprService.investmentDetailFormData.fromDate != null && this.investmentAprService.investmentDetailFormData.toDate != null) {
      if (this.investmentAprService.investmentDetailFormData.toDate > this.investmentAprService.investmentDetailFormData.fromDate) {
      }
      else {
        form.controls.fromDate.setValue(null);
        form.controls.toDate.setValue(null);
        this.toastr.error('Select Appropriate Date Range', 'Error')
      }
    }
  }
  dateCommitmentCompare(form: NgForm) {
    if (this.investmentAprService.investmentDetailFormData.commitmentFromDate != null && this.investmentAprService.investmentDetailFormData.commitmentToDate != null) {
      if (this.investmentAprService.investmentDetailFormData.commitmentToDate > this.investmentAprService.investmentDetailFormData.commitmentFromDate) {
      }
      else {
        form.controls.commitmentFromDate.setValue(null);
        form.controls.commitmentToDate.setValue(null);
        this.toastr.error('Select Appropriate Commitment Date Range');
      }
    }
  }
  getEmployeeId() {
    this.empId = this.accountService.getEmployeeId();
    this.userRole = this.accountService.getUserRole();
    if (this.userRole == 'Administrator') {
      this.isAdmin = true;
    }
    else {
      this.isAdmin = false;
    }
    this.getDepot();
    this.isDepotRequire = false;
    this.investmentAprService.investmentAprCommentFormData.employeeId = parseInt(this.empId);
    this.getEmployeeSbu();
  }
  getEmployeeSbu() {
    this.accountService.getEmployeeSbu(this.investmentAprService.investmentAprCommentFormData.employeeId).subscribe(
      (response) => {
        this.sbu = response.sbu;
        this.getProduct();
        this.getMedicineProds();
      },
      (error) => {
        console.log(error);
      });
  }
  changePaymentMethod() {
    if (this.investmentAprService.investmentDetailFormData.paymentMethod != 'Cheque') {
      this.investmentAprService.investmentDetailFormData.chequeTitle = "";
    }
   }
  getProduct() {
    this.investmentAprService.getProduct(this.sbu).subscribe(response => {
      this.products = response as IProduct[];
    }, error => {
      console.log(error);
    });
  }
  getMedicineProds() {
    this.SpinnerService.show();
    this.investmentAprService.getMedicineProduct().subscribe(response => {
      this.medicineProducts = response as IMedicineProduct[];
      this.SpinnerService.hide();
    }, error => {
      this.SpinnerService.hide();
      console.log(error);
    });
  }
  getBudget() {
    this.investmentAprService.getBudget(this.investmentAprService.investmentAprFormData.sbu, parseInt(this.empId), this.investmentAprService.investmentAprFormData.donationId).subscribe(response => {
      this.budgetCeiling = response[0] as IBudgetCeiling;
      this.isBudgetVisible = true;
    }, error => {
      console.log(error);
    });
  }
  insertInvestmentApr() {
    if (this.investmentAprService.investmentAprFormData.id == null || this.investmentAprService.investmentAprFormData.id == undefined || this.investmentAprService.investmentAprFormData.id == 0) {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment ');
      return false;
    }
    if (this.investmentAprService.investmentDetailFormData.proposedAmount == null || this.investmentAprService.investmentDetailFormData.proposedAmount == undefined || this.investmentAprService.investmentDetailFormData.proposedAmount == "") {
      this.toastr.warning('Enter Proposed Amount First', 'Investment ');
      return false;
    }
    if (this.investmentAprService.investmentDetailFormData.purpose == null || this.investmentAprService.investmentDetailFormData.purpose == undefined || this.investmentAprService.investmentDetailFormData.purpose == "") {
      this.toastr.warning('Enter Purpose First', 'Investment ');
      return false;
    }
    if (this.investmentAprService.investmentDetailFormData.paymentFreq == null || this.investmentAprService.investmentDetailFormData.paymentFreq == undefined || this.investmentAprService.investmentDetailFormData.paymentFreq == "") {
      this.toastr.warning('Select Payment Frequency First', 'Investment Detail');
      return false;
    }
    if (this.investmentAprService.investmentDetailFormData.fromDate == null || this.investmentAprService.investmentDetailFormData.fromDate == undefined) {
      this.toastr.warning('Select Payment Dur. From Date  First', 'Investment Detail');
      return false;
    }
    if (this.investmentAprService.investmentDetailFormData.toDate == null || this.investmentAprService.investmentDetailFormData.toDate == undefined) {
      this.toastr.warning('Select Payment Dur. To Date  First', 'Investment Detail');
      return false;
    }
    if (this.investmentAprService.investmentDetailFormData.totalMonth  == null || this.investmentAprService.investmentDetailFormData.totalMonth  == undefined || this.investmentAprService.investmentDetailFormData.totalMonth==0) {
      this.toastr.warning('Invalid Payment Total Month', 'Investment Detail');
      return false;
    }
    if (this.investmentAprService.investmentDetailFormData.commitmentFromDate == null || this.investmentAprService.investmentDetailFormData.commitmentFromDate == undefined) {
      this.toastr.warning('Select Commitment From Date  First', 'Investment Detail');
      return false;
    }
    if (this.investmentAprService.investmentDetailFormData.commitmentToDate == null || this.investmentAprService.investmentDetailFormData.commitmentToDate == undefined) {
      this.toastr.warning('Select Commitment To Date  First', 'Investment Detail');
      return false;
    }
    if (this.investmentAprService.investmentDetailFormData.commitmentTotalMonth  == null || this.investmentAprService.investmentDetailFormData.commitmentTotalMonth  == undefined || this.investmentAprService.investmentDetailFormData.commitmentTotalMonth==0) {
      this.toastr.warning('Invalid Commitment Total Month', 'Investment Detail');
      return false;
    }
    if (this.investmentAprService.investmentDetailFormData.commitmentAllSBU == null || this.investmentAprService.investmentDetailFormData.commitmentAllSBU == undefined || this.investmentAprService.investmentDetailFormData.commitmentAllSBU == "") {
      this.toastr.warning('Enter Commitment All SBU First', 'Investment ');
      return false;
    }
    if (this.investmentAprService.investmentDetailFormData.commitmentOwnSBU == null || this.investmentAprService.investmentDetailFormData.commitmentOwnSBU == undefined || this.investmentAprService.investmentDetailFormData.commitmentOwnSBU == "") {
      this.toastr.warning('Enter Commitment Own SBU First', 'Investment ');
      return false;
    }
    if (this.investmentAprService.investmentDetailFormData.paymentMethod == null || this.investmentAprService.investmentDetailFormData.paymentMethod == undefined || this.investmentAprService.investmentDetailFormData.paymentMethod == "") {
      this.toastr.warning('Select Payment Method First', 'Investment ');
      return false;
    }
    if (this.investmentAprService.investmentDetailFormData.paymentMethod == 'Cheque') {
      if (this.investmentAprService.investmentDetailFormData.chequeTitle == null || this.investmentAprService.investmentDetailFormData.chequeTitle == undefined || this.investmentAprService.investmentDetailFormData.chequeTitle == "") {
        this.toastr.warning('Enter  Cheque Title First', 'Investment Detail');
        return false;
      }
    }
    if (this.userRole == 'RSM' || this.userRole == 'Administrator') {
      if (this.investmentAprService.investmentDetailFormData.paymentMethod == 'Cash') {
        if (this.investmentAprService.investmentDepotFormData.depotCode == null || this.investmentAprService.investmentDepotFormData.depotCode == undefined || this.investmentAprService.investmentDepotFormData.depotCode == "") {
          this.toastr.warning('Select Depot First', 'Investment');
          return false;
        }
      }
    }
    if (this.investmentAprService.investmentAprCommentFormData.recStatus == 'Not Recommended') {
      if (this.investmentAprService.investmentAprCommentFormData.comments == null || this.investmentAprService.investmentAprCommentFormData.comments == undefined || this.investmentAprService.investmentAprCommentFormData.comments == "") {

        this.toastr.warning('Please Insert Comment For Not Recommendation', 'Investment');
        return false;
      }
    }
    if (this.investmentAprService.investmentAprFormData.id == null || this.investmentAprService.investmentAprFormData.id == undefined || this.investmentAprService.investmentAprFormData.id == 0) {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment Product');
      return false;
    }

    if (this.investmentTargetedProds !== undefined) {
      for (let i = 0; i < this.investmentTargetedProds.length; i++) {
        if (this.investmentTargetedProds[i].productInfo.id == this.investmentAprService.investmentTargetedProdFormData.productId) {
          this.toastr.warning("Product already exist !");
          return false;
        }
      }
    }
    else {
      this.toastr.warning('Select Product First', 'Investment Product');
      return false;
    }
if (this.investmentAprService.investmentDetailFormData.paymentFreq == 'Quarterly') {
      if (this.investmentAprService.investmentDetailFormData.totalMonth  <3) {
        this.toastr.warning('Duration can not be less than 3 Month for Quarterly Investment ');
        return false;
      }
    }
    if (this.investmentAprService.investmentDetailFormData.paymentFreq == 'Half Yearly') {
      if (this.investmentAprService.investmentDetailFormData.totalMonth  <6) {
        this.toastr.warning('Duration can not be less than 6 Month for Half Yearly Investment');
        return false;
      }
    }
    this.investmentAprService.investmentDetailFormData.fromDate  = this.datePipe.transform(this.investmentAprService.investmentDetailFormData.fromDate, 'yyyy-MM-dd HH:mm:ss');
    this.investmentAprService.investmentDetailFormData.toDate= this.datePipe.transform(this.investmentAprService.investmentDetailFormData.toDate, 'yyyy-MM-dd HH:mm:ss');
  
    this.investmentAprService.investmentDetailFormData.commitmentFromDate  = this.datePipe.transform(this.investmentAprService.investmentDetailFormData.commitmentFromDate, 'yyyy-MM-dd HH:mm:ss');
    this.investmentAprService.investmentDetailFormData.commitmentToDate= this.datePipe.transform(this.investmentAprService.investmentDetailFormData.commitmentToDate, 'yyyy-MM-dd HH:mm:ss');
  

    this.investmentAprService.insertInvestAprNoSBU(parseInt(this.empId), this.investmentAprService.investmentAprFormData.sbu,this.investmentTargetedProds).subscribe(
      res => {
        this.getInvestmentAprDetails();
        this.getInvestmentTargetedProd();
        this.getInvestmentTargetedGroup();
        this.toastr.success('Save successfully', 'Investment');
        this.isDonationValid = true;
        this.SpinnerService.hide();
      },
      err => {
        this.investmentAprService.investmentDetailFormData.fromDate = new Date(this.investmentAprService.investmentDetailFormData.fromDate);
        this.investmentAprService.investmentDetailFormData.toDate = new Date(this.investmentAprService.investmentDetailFormData.toDate);
        this.investmentAprService.investmentDetailFormData.commitmentFromDate = new Date(this.investmentAprService.investmentDetailFormData.commitmentFromDate);
        this.investmentAprService.investmentDetailFormData.commitmentToDate = new Date(this.investmentAprService.investmentDetailFormData.commitmentToDate);
       console.log(err);
        this.SpinnerService.hide();
      }
    );
    
    
    // this.investmentAprService.investmentDetailFormData.investmentInitId = this.investmentAprService.investmentAprFormData.id;
    // this.SpinnerService.show();
    // this.investmentAprService.insertInvestmentDetail(parseInt(this.empId), this.investmentAprService.investmentAprFormData.sbu).subscribe(
    //   res => {
    //     var data = res as IInvestmentApr;
    //     this.investmentAprService.investmentDetailFormData = data;
    //     this.investmentAprService.investmentDetailFormData.fromDate = new Date(data.fromDate);
    //     this.investmentAprService.investmentDetailFormData.toDate = new Date(data.toDate);
    //     this.isDonationValid = true;
    //     this.investmentAprService.investmentAprCommentFormData.employeeId = parseInt(this.empId);
    //     this.investmentAprService.insertInvestmentApr().subscribe(
    //       res => {
    //         this.investmentAprService.investmentAprCommentFormData = res as IInvestmentAprComment;
    //         this.isValid = true;
    //         this.investmentAprService.investmentTargetedProdFormData.investmentInitId = this.investmentAprService.investmentAprFormData.id;
    //         this.investmentAprService.insertInvestmentTargetedProd(this.investmentTargetedProds).subscribe(
    //           res => {
    //             this.getInvestmentTargetedProd();
    //             this.getInvestmentTargetedGroup();
    //             this.toastr.success('Save successfully', 'Investment');
    //             this.isDonationValid = true;
    //             this.SpinnerService.hide();
    //           },
    //           err => {
    //             console.log(err);
    //             this.SpinnerService.hide();
    //           }
    //         );
    //         this.SpinnerService.hide();
    //       },
    //       err => { console.log(err); }
    //     );
    //     this.getLastFiveInvestment(this.investmentAprService.investmentAprFormData.marketCode, this.convertedDate);
    //     this.SpinnerService.hide();
    //   },
    //   err => {
    //     console.log(err);
    //     this.investmentAprService.investmentAprCommentFormData.id = 0;
    //     this.SpinnerService.hide();
    //   }
    // );
  }
  updateInvestmentApr() {
    if (this.investmentAprService.investmentAprFormData.id == null || this.investmentAprService.investmentAprFormData.id == undefined || this.investmentAprService.investmentAprFormData.id == 0) {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment ');
      return false;
    }
    if (this.investmentAprService.investmentDetailFormData.proposedAmount == null || this.investmentAprService.investmentDetailFormData.proposedAmount == undefined || this.investmentAprService.investmentDetailFormData.proposedAmount == "") {
      this.toastr.warning('Enter Proposed Amount First', 'Investment ');
      return false;
    }
    if (this.investmentAprService.investmentDetailFormData.purpose == null || this.investmentAprService.investmentDetailFormData.purpose == undefined || this.investmentAprService.investmentDetailFormData.purpose == "") {
      this.toastr.warning('Enter Purpose First', 'Investment ');
      return false;
    }
    if (this.investmentAprService.investmentDetailFormData.paymentFreq == null || this.investmentAprService.investmentDetailFormData.paymentFreq == undefined || this.investmentAprService.investmentDetailFormData.paymentFreq == "") {
      this.toastr.warning('Select Payment Frequency First', 'Investment Detail');
      return false;
    }
    if (this.investmentAprService.investmentDetailFormData.fromDate == null || this.investmentAprService.investmentDetailFormData.fromDate == undefined) {
      this.toastr.warning('Select Payment Dur. From Date  First', 'Investment Detail');
      return false;
    }
    if (this.investmentAprService.investmentDetailFormData.toDate == null || this.investmentAprService.investmentDetailFormData.toDate == undefined) {
      this.toastr.warning('Select Payment Dur. To Date  First', 'Investment Detail');
      return false;
    }
    if (this.investmentAprService.investmentDetailFormData.totalMonth  == null || this.investmentAprService.investmentDetailFormData.totalMonth  == undefined || this.investmentAprService.investmentDetailFormData.totalMonth==0) {
      this.toastr.warning('Invalid Payment Total Month', 'Investment Detail');
      return false;
    }
    if (this.investmentAprService.investmentDetailFormData.commitmentFromDate == null || this.investmentAprService.investmentDetailFormData.commitmentFromDate == undefined) {
      this.toastr.warning('Select Commitment From Date  First', 'Investment Detail');
      return false;
    }
    if (this.investmentAprService.investmentDetailFormData.commitmentToDate == null || this.investmentAprService.investmentDetailFormData.commitmentToDate == undefined) {
      this.toastr.warning('Select Commitment To Date  First', 'Investment Detail');
      return false;
    }
    if (this.investmentAprService.investmentDetailFormData.commitmentTotalMonth  == null || this.investmentAprService.investmentDetailFormData.commitmentTotalMonth  == undefined || this.investmentAprService.investmentDetailFormData.commitmentTotalMonth==0) {
      this.toastr.warning('Invalid Commitment Total Month', 'Investment Detail');
      return false;
    }
    if (this.investmentAprService.investmentDetailFormData.commitmentAllSBU == null || this.investmentAprService.investmentDetailFormData.commitmentAllSBU == undefined || this.investmentAprService.investmentDetailFormData.commitmentAllSBU == "") {
      this.toastr.warning('Enter Commitment All SBU First', 'Investment ');
      return false;
    }
    if (this.investmentAprService.investmentDetailFormData.commitmentOwnSBU == null || this.investmentAprService.investmentDetailFormData.commitmentOwnSBU == undefined || this.investmentAprService.investmentDetailFormData.commitmentOwnSBU == "") {
      this.toastr.warning('Enter Commitment Own SBU First', 'Investment ');
      return false;
    }
    if (this.investmentAprService.investmentDetailFormData.paymentMethod == null || this.investmentAprService.investmentDetailFormData.paymentMethod == undefined || this.investmentAprService.investmentDetailFormData.paymentMethod == "") {
      this.toastr.warning('Select Payment Method First', 'Investment ');
      return false;
    }
    if (this.investmentAprService.investmentDetailFormData.paymentMethod == 'Cheque') {
      if (this.investmentAprService.investmentDetailFormData.chequeTitle == null || this.investmentAprService.investmentDetailFormData.chequeTitle == undefined || this.investmentAprService.investmentDetailFormData.chequeTitle == "") {
        this.toastr.warning('Enter  Cheque Title First', 'Investment Detail');
        return false;
      }
    }
    if (this.userRole == 'RSM' || this.userRole == 'Administrator') {
      if (this.investmentAprService.investmentDetailFormData.paymentMethod == 'Cash') {
        if (this.investmentAprService.investmentDepotFormData.depotCode == null || this.investmentAprService.investmentDepotFormData.depotCode == undefined || this.investmentAprService.investmentDepotFormData.depotCode == "") {
          this.toastr.warning('Select Depot First', 'Investment');
          return false;
        }
      }
    }
    if (this.investmentAprService.investmentAprCommentFormData.recStatus == 'Not Recommended') {
      if (this.investmentAprService.investmentAprCommentFormData.comments == null || this.investmentAprService.investmentAprCommentFormData.comments == undefined || this.investmentAprService.investmentAprCommentFormData.comments == "") {

        this.toastr.warning('Please Insert Comment For Not Recommendation', 'Investment');
        return false;
      }
    }
    if (this.investmentAprService.investmentAprFormData.id == null || this.investmentAprService.investmentAprFormData.id == undefined || this.investmentAprService.investmentAprFormData.id == 0) {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment Product');
      return false;
    }
    if (this.investmentTargetedProds !== undefined) {
      for (let i = 0; i < this.investmentTargetedProds.length; i++) {
        if (this.investmentTargetedProds[i].productInfo.id == this.investmentAprService.investmentTargetedProdFormData.productId) {
          this.toastr.warning("Product already exist !");
          return false;
        }
      }
    }
    else {
      this.toastr.warning('Select Product First', 'Investment Product');
      return false;
    }
    if (this.investmentAprService.investmentDetailFormData.paymentFreq == 'Quarterly') {
      if (this.investmentAprService.investmentDetailFormData.totalMonth  <3) {
        this.toastr.warning('Duration can not be less than 3 Month for Quarterly Investment ');
        return false;
      }
    }
    if (this.investmentAprService.investmentDetailFormData.paymentFreq == 'Half Yearly') {
      if (this.investmentAprService.investmentDetailFormData.totalMonth  <6) {
        this.toastr.warning('Duration can not be less than 6 Month for Half Yearly Investment');
        return false;
      }
    }
    this.investmentAprService.investmentDetailFormData.investmentInitId = this.investmentAprService.investmentAprFormData.id;
    this.investmentAprService.investmentDetailFormData.fromDate  = this.datePipe.transform(this.investmentAprService.investmentDetailFormData.fromDate, 'yyyy-MM-dd HH:mm:ss');
    this.investmentAprService.investmentDetailFormData.toDate= this.datePipe.transform(this.investmentAprService.investmentDetailFormData.toDate, 'yyyy-MM-dd HH:mm:ss');
  
    this.investmentAprService.investmentDetailFormData.commitmentFromDate  = this.datePipe.transform(this.investmentAprService.investmentDetailFormData.commitmentFromDate, 'yyyy-MM-dd HH:mm:ss');
    this.investmentAprService.investmentDetailFormData.commitmentToDate= this.datePipe.transform(this.investmentAprService.investmentDetailFormData.commitmentToDate, 'yyyy-MM-dd HH:mm:ss');
  
    this.SpinnerService.show();
    this.investmentAprService.updateInvestAprNoSBU(parseInt(this.empId), this.investmentAprService.investmentAprFormData.sbu,this.investmentTargetedProds).subscribe(
      res => {
        this.getInvestmentAprDetails();
        this.getInvestmentTargetedProd();
        this.getInvestmentTargetedGroup();
        this.toastr.success('Update successfully', 'Investment');
        this.isDonationValid = true;
        this.SpinnerService.hide();
      },
      err => {
        this.investmentAprService.investmentDetailFormData.fromDate = new Date(this.investmentAprService.investmentDetailFormData.fromDate);
        this.investmentAprService.investmentDetailFormData.toDate = new Date(this.investmentAprService.investmentDetailFormData.toDate);
        this.investmentAprService.investmentDetailFormData.commitmentFromDate = new Date(this.investmentAprService.investmentDetailFormData.commitmentFromDate);
        this.investmentAprService.investmentDetailFormData.commitmentToDate = new Date(this.investmentAprService.investmentDetailFormData.commitmentToDate);
       console.log(err);
        this.SpinnerService.hide();
      }
    );
    // this.investmentAprService.insertInvestmentDetail(parseInt(this.empId), this.investmentAprService.investmentAprFormData.sbu).subscribe(
    //   res => {
    //     var data = res as IInvestmentApr;
    //     this.investmentAprService.investmentDetailFormData = data;
    //     this.investmentAprService.investmentDetailFormData.fromDate = new Date(data.fromDate);
    //     this.investmentAprService.investmentDetailFormData.toDate = new Date(data.toDate);
    //     this.isDonationValid = true;
    //     this.investmentAprService.updateInvestmentApr().subscribe(
    //       res => {
    //         this.isValid = true;
    //         this.investmentAprService.investmentAprCommentFormData = res as IInvestmentAprComment;
    //         this.investmentAprService.investmentTargetedProdFormData.investmentInitId = this.investmentAprService.investmentAprFormData.id;
    //         this.investmentAprService.insertInvestmentTargetedProd(this.investmentTargetedProds).subscribe(
    //           res => {
    //             this.getInvestmentTargetedProd();
    //             this.getInvestmentTargetedGroup();
    //             this.isDonationValid = true;
    //             this.SpinnerService.hide();
    //           },
    //           err => {
    //             console.log(err);
    //             this.SpinnerService.hide();
    //           }
    //         );
    //         this.SpinnerService.hide();
    //         if (this.sbu != this.investmentAprService.investmentAprFormData.sbu) {
    //           this.toastr.success('Save successfully', 'Investment')
    //         }
    //       },
    //       err => { console.log(err); }
    //     );
    //     this.getLastFiveInvestment(this.investmentAprService.investmentAprFormData.marketCode, this.convertedDate);
    //     this.toastr.success('Save successfully', 'Investment');
    //     this.SpinnerService.hide();
    //   },
    //   err => {
    //     console.log(err);
    //     this.investmentAprService.investmentAprCommentFormData.id = 0;
    //     this.SpinnerService.hide();
    //   }
    // );
  }
  insertInvestmentDetails() {
    this.investmentAprService.investmentDetailFormData.investmentInitId = this.investmentAprService.investmentAprFormData.id;
    this.SpinnerService.show();
    this.investmentAprService.insertInvestmentDetail(parseInt(this.empId), this.investmentAprService.investmentAprFormData.sbu).subscribe(
      res => {
        var data = res as IInvestmentApr;
        this.investmentAprService.investmentDetailFormData = data;
        this.investmentAprService.investmentDetailFormData.fromDate = new Date(data.fromDate);
        this.investmentAprService.investmentDetailFormData.toDate = new Date(data.toDate);
        this.isDonationValid = true;
        this.getLastFiveInvestment(this.investmentAprService.investmentAprFormData.marketCode, this.convertedDate);
        this.toastr.success('Save successfully', 'Investment');
        this.SpinnerService.hide();
      },
      err => {
        console.log(err);
        this.investmentAprService.investmentAprCommentFormData.id = 0;
        this.SpinnerService.hide();
      }
    );
  }
  // insertInvestmentRecDepot() {
  //   if (this.userRole == 'RSM' || this.userRole == 'Administrator') {
  //     if (this.investmentAprService.investmentDetailFormData.paymentMethod == 'Cash') {
  //       this.investmentAprService.investmentDepotFormData.investmentInitId = this.investmentAprService.investmentAprFormData.id;
  //       for (let i = 0; i < this.depots.length; i++) {
  //         if (this.depots[i].depotCode == this.investmentAprService.investmentDepotFormData.depotCode) {
  //           this.investmentAprService.investmentDepotFormData.depotName = this.depots[i].depotName;
  //           this.investmentAprService.investmentDepotFormData.employeeId = parseInt(this.empId);
  //           break;
  //         }
  //       }
  //       this.SpinnerService.show();
  //       this.investmentAprService.insertInvestmentRecDepot().subscribe(
  //         res => {
  //           debugger;
  //           this.investmentAprService.investmentDepotFormData = res as IInvestmentRecDepot;
  //           this.SpinnerService.hide();

  //         },
  //         err => {
  //           console.log(err);
  //           this.SpinnerService.hide();
  //         }
  //       );
  //     }

  //   }
  // }
  insertInvestmentTargetedProd() {
    if (this.investmentAprService.investmentAprFormData.id == null || this.investmentAprService.investmentAprFormData.id == undefined || this.investmentAprService.investmentAprFormData.id == 0) {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment Product');
      return false;
    }
    if (this.investmentTargetedProds !== undefined) {
      for (let i = 0; i < this.investmentTargetedProds.length; i++) {
        if (this.investmentTargetedProds[i].productInfo.id == this.investmentAprService.investmentTargetedProdFormData.productId) {
          this.toastr.warning("Product already exist !");
          return false;
        }
      }
    }
    else {
      this.toastr.warning('Select Product First', 'Investment Product');
      return false;
    }
    this.investmentAprService.investmentTargetedProdFormData.investmentInitId = this.investmentAprService.investmentAprFormData.id;
    this.SpinnerService.show();
    this.investmentAprService.insertInvestmentTargetedProd(this.investmentTargetedProds).subscribe(
      res => {
        // if (this.sbu == this.investmentAprService.investmentAprFormData.sbu) 
        // { 
        this.insertInvestmentDetails();
        //}
        this.getInvestmentTargetedProd();
        this.getInvestmentTargetedGroup();
        this.isDonationValid = true;
        this.SpinnerService.hide();
        // if (this.sbu != this.investmentAprService.investmentAprFormData.sbu) 
        // { 
        // this.toastr.success('Save successfully', 'Investment Product');
        // }
      },
      err => {
        console.log(err);
        this.SpinnerService.hide();
      }
    );
  }
  addInvestmentTargetedProd() {
    if (this.investmentAprService.investmentTargetedProdFormData.productId == null || this.investmentAprService.investmentTargetedProdFormData.productId == undefined || this.investmentAprService.investmentTargetedProdFormData.productId == 0) {
      this.toastr.warning('Select Product First', 'Investment ');
      return false;
    }
    if (this.investmentTargetedProds !== undefined) {
      for (let i = 0; i < this.investmentTargetedProds.length; i++) {
        if (this.investmentTargetedProds[i].productInfo.id == this.investmentAprService.investmentTargetedProdFormData.productId) {
          this.toastr.warning("Product already exist !");
          return false;
        }

      }
      for (let i = 0; i < this.products.length; i++) {
        if (this.products[i].id == this.investmentAprService.investmentTargetedProdFormData.productId) {
          let data = new InvestmentTargetedProd();

          data.sbu = this.sbu;
          data.employeeId = parseInt(this.empId);
          data.investmentInitId = this.investmentAprService.investmentAprFormData.id;
          data.productId = this.investmentAprService.investmentTargetedProdFormData.productId;
          data.productInfo = this.products[i];
          this.investmentTargetedProds.push(data);
          return false;
        }
      }
    }
  }

  // editInvestmentTargetedProd(selectedAprord: IInvestmentTargetedProd) {
  //   this.investmentAprService.investmentTargetedProdFormData = Object.assign({}, selectedAprord);
  // }
  removeInvestmentTargetedProd(selectedAprord: IInvestmentTargetedProd) {

    if (this.investmentAprService.investmentAprCommentFormData.id == null || this.investmentAprService.investmentAprCommentFormData.id == undefined || this.investmentAprService.investmentAprCommentFormData.id == 0) {
      var c = confirm("Are you sure you want to remove this product?");
      if (c == true) {
        if (this.investmentTargetedProds.find(x => x.productId == selectedAprord.productId)) {
          this.investmentTargetedProds.splice(this.investmentTargetedProds.findIndex(x => x.productId == selectedAprord.productId), 1);
          this.toastr.success("Successfully Removed. Please Save the data.");
          return false;
        }
      }
    }
    else {
      var c = confirm("Are you sure you want to delete that?");
      if (c == true) {
        // if (this.investmentAprService.investmentAprCommentFormData.id == null || this.investmentAprService.investmentAprCommentFormData.id == undefined || this.investmentAprService.investmentAprCommentFormData.id == 0) {
        //   this.toastr.warning("Please Save Data First!") 
        //   return false;
        // }
        if (selectedAprord.id == 0) {
          if (this.investmentTargetedProds.find(x => x.productId == selectedAprord.productId)) {
            this.investmentTargetedProds.splice(this.investmentTargetedProds.findIndex(x => x.productId == selectedAprord.productId), 1);
            this.toastr.success("Successfully Removed");
            return false;
          }
        }
        this.investmentAprService.investmentTargetedProdFormData = Object.assign({}, selectedAprord);
        if (this.investmentTargetedProds.find(x => x.productId == selectedAprord.productId)) {
          this.investmentTargetedProds.splice(this.investmentTargetedProds.findIndex(x => x.productId == selectedAprord.productId), 1);
        }

        this.investmentAprService.removeInvestmentTargetedProd().subscribe(
          res => {
            this.toastr.success(res);
            this.investmentAprService.investmentTargetedProdFormData = new InvestmentTargetedProd();
            this.getInvestmentTargetedProd();
          },
          err => { console.log(err); }
        );
      }
    }
  }
  populateForm() {
  }
  resetPage(form: NgForm) {
    window.location.reload();
    form.reset();
    this.investmentAprService.investmentAprFormData = new InvestmentInit();
    this.investmentAprService.investmentAprCommentFormData = new InvestmentAprComment();
    this.investmentAprService.investmentDepotFormData = new InvestmentRecDepot();
    this.investmentAprService.investmentDetailFormData = new InvestmentApr();
    this.investmentAprService.investmentTargetedProdFormData = new InvestmentTargetedProd();
    this.investmentAprService.investmentTargetedGroupFormData = new InvestmentTargetedGroup();
    this.investmentAprService.investmentDoctorFormData = new InvestmentDoctor();
    this.investmentAprService.investmentInstitutionFormData = new InvestmentInstitution();
    this.investmentAprService.investmentCampaignFormData = new InvestmentCampaign();
    this.investmentAprService.investmentBcdsFormData = new InvestmentBcds();
    this.investmentAprService.investmentSocietyFormData = new InvestmentSociety();
    this.investmentTargetedProds = [];
    this.investmentTargetedGroups = [];
    this.investmentDetailsOld = [];
    this.lastFiveInvestmentDetail = [];
    // this.isAdmin = false;
    this.isDepotRequire = false;
    this.isValid = false;
    this.isBudgetVisible = false;
    // this.configs = {
    //   currentPage: 1,
    //   itemsPerPage: 10,
    //   totalItems: 50,
    // };
  }
  resetForm() {
    this.investmentAprService.investmentAprFormData = new InvestmentInit();
    this.investmentAprService.investmentAprCommentFormData = new InvestmentAprComment();
    this.investmentAprService.investmentDepotFormData = new InvestmentRecDepot();
    this.investmentAprService.investmentDetailFormData = new InvestmentApr();
    this.investmentAprService.investmentTargetedProdFormData = new InvestmentTargetedProd();
    this.investmentAprService.investmentTargetedGroupFormData = new InvestmentTargetedGroup();
    this.investmentAprService.investmentDoctorFormData = new InvestmentDoctor();
    this.investmentAprService.investmentInstitutionFormData = new InvestmentInstitution();
    this.investmentAprService.investmentCampaignFormData = new InvestmentCampaign();
    this.investmentAprService.investmentBcdsFormData = new InvestmentBcds();
    this.investmentAprService.investmentSocietyFormData = new InvestmentSociety();
    this.investmentTargetedProds = [];
    this.investmentTargetedGroups = [];
    this.investmentDetailsOld = [];
    this.lastFiveInvestmentDetail = [];
    //this.isAdmin = false;
    this.isDepotRequire = false;
    this.isValid = false;
    this.isBudgetVisible = false;
    // this.configs = {
    //   currentPage: 1,
    //   itemsPerPage: 10,
    //   totalItems: 50,
    // };
  }
  // #region Medicine Prod 
  getInvestmentMedicineProd() {
    this.investmentAprService.getInvestmentMedicineProds(this.investmentAprService.investmentAprFormData.id, this.sbu).subscribe(response => {
      var data = response as IInvestmentMedicineProd[];
      debugger;
      if (data !== undefined && data.length > 0) {
        this.investmentMedicineProds = data;
        let sum = 0;
        for (let i = 0; i < this.investmentMedicineProds.length; i++) {
          sum = sum + this.investmentMedicineProds[i].tpVat;
        }
        this.investmentAprService.investmentDetailFormData.proposedAmount = sum.toString();
      }
      else {
        this.investmentAprService.investmentDetailFormData.proposedAmount = '';
        this.investmentMedicineProds = [];
      }
    }, error => {
      console.log(error);
    });
  }
  insertInvestmentMedicineProd() {
    if (this.investmentAprService.investmentAprFormData.id == null || this.investmentAprService.investmentAprFormData.id == undefined || this.investmentAprService.investmentAprFormData.id == 0) {
      this.toastr.warning('Insert Investment Initialisation First!');
      return false;
    }
    if (this.investmentAprService.investmentMedicineProdFormData.productId == null || this.investmentAprService.investmentMedicineProdFormData.productId == undefined || this.investmentAprService.investmentMedicineProdFormData.productId == 0) {
      this.toastr.warning('Select Product First');
      return false;
    }
    if (this.investmentAprService.investmentMedicineProdFormData.boxQuantity == null || this.investmentAprService.investmentMedicineProdFormData.boxQuantity == undefined || this.investmentAprService.investmentMedicineProdFormData.boxQuantity == 0) {
      this.toastr.warning('Insert Box Quantity First');
      return false;
    }
    if (this.investmentMedicineProds !== undefined) {
      for (let i = 0; i < this.investmentMedicineProds.length; i++) {
        if (this.investmentMedicineProds[i].medicineProduct.id === this.investmentAprService.investmentMedicineProdFormData.productId) {
          this.toastr.warning("Product already exist!");
          return false;
        }
      }
    }
    this.investmentAprService.investmentMedicineProdFormData.investmentInitId = this.investmentAprService.investmentAprFormData.id;
    this.investmentAprService.investmentMedicineProdFormData.employeeId = parseInt(this.empId);
    this.SpinnerService.show();
    this.investmentAprService.insertInvestmentMedicineProd().subscribe(
      res => {
        this.investmentAprService.investmentMedicineProdFormData = new InvestmentMedicineProd();
        this.getInvestmentMedicineProd();
        this.isDonationValid = true;
        this.toastr.success('Save successfully', 'Investment  Product');
      },
      err => { console.log(err); }
    );
  }
  removeInvestmentMedicineProd(selectedRecord: IInvestmentMedicineProd) {
    var c = confirm("Are you sure you want to delete that?");
    if (c == true) {
      this.investmentAprService.investmentMedicineProdFormData = Object.assign({}, selectedRecord);
      this.SpinnerService.show();
      this.investmentAprService.removeInvestmentMedicineProd().subscribe(
        res => {
          this.investmentAprService.investmentMedicineProdFormData = new InvestmentMedicineProd();
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
  // onPageChanged(event: any) {
  //   const params = this.investmentAprService.getGenParams();
  //   if (params.pageIndex !== event) {
  //     params.pageIndex = event;
  //     this.investmentAprService.setGenParams(params);
  //     this.getInvestmentApprovedPgChange();
  //   }
  // }
  // getInvestmentApprovedPgChange() {
  //   const params = this.investmentAprService.getGenParams();
  //   this.SpinnerService.show();
  //   this.investmentAprService.getInvestmentApproved(parseInt(this.empId), this.investmentAprService.investmentAprFormData.sbu ,this.userRole).subscribe(response => {
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
  getSummaryDetail() {
    this.router.navigate([]).then(result => { window.open('/portal/rptInvestmentDetail/' + this.investmentAprService.investmentAprFormData.id, '_blank'); });
  }

}