import { InvestmentRec, IInvestmentRec, InvestmentInit, IInvestmentInit, InvestmentTargetedProd, IInvestmentTargetedProd, InvestmentTargetedGroup, IInvestmentTargetedGroup, IInvestmentMedicineProd, IInvestmentRecComment, InvestmentRecComment, InvestmentMedicineProd } from '../shared/models/investmentRec';
import { InvestmentDoctor, IInvestmentDoctor, InvestmentInstitution, IInvestmentInstitution, InvestmentCampaign, IInvestmentCampaign } from '../shared/models/investmentRec';
import { InvestmentBcds, IInvestmentBcds, InvestmentSociety, IInvestmentSociety } from '../shared/models/investmentRec';
import { SubCampaign, ISubCampaign } from '../shared/models/subCampaign';
import { Donation, IDonation } from '../shared/models/donation';
import { Doctor, IDoctor } from '../shared/models/docotor';
import { Institution, IInstitution } from '../shared/models/institution';
import { GenericParams } from '../shared/models/genericParams';
import { Component, ElementRef, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { InvestmentRecService } from '../_services/investmentRec.service';
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
import { MedicineProduct, IMedicineProduct } from '../shared/models/medicineProduct';
import { MarketGroupMst, IMarketGroupMst } from '../shared/models/marketGroupMst';
import { MarketGroupDtl, IMarketGroupDtl } from '../shared/models/marketGroupDtl';
import { AccountService } from '../account/account.service';
import { IInvestmentDetailOld, ILastFiveInvestmentDetail } from '../shared/models/investment';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-investmentRec',
  templateUrl: './investmentRec.component.html',
  styles: [
  ],
  providers: [DatePipe]
})
export class InvestmentRecComponent implements OnInit {
  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  @ViewChild('investmentInitSearchModal', { static: false }) investmentInitSearchModal: TemplateRef<any>;
  @ViewChild('investmentRecSearchModal', { static: false }) investmentRecSearchModal: TemplateRef<any>;
  InvestmentInitSearchModalRef: BsModalRef;
  InvestmentRecSearchModalRef: BsModalRef;
  investmentRecs: IInvestmentRec[];
  investmentInits: IInvestmentInit[];
  medicineProducts: IMedicineProduct[];
  investmentTargetedProds: IInvestmentTargetedProd[];
  investmentTargetedGroups: IInvestmentTargetedGroup[];
  investmentMedicineProd: IInvestmentMedicineProd[];
  investmentDetails: IInvestmentRec[];
  investmentDoctors: IInvestmentDoctor[];
  searchText = '';
  convertedDate: string;
  //configs: any;
  isValid: boolean = false;
  isInvOther: boolean = false;
  isDonationValid: boolean = false;
  numberPattern = "^[0-9]+(.[0-9]{1,10})?$";
  bcds: IBcdsInfo[];
  society: ISocietyInfo[];
  markets: IMarket[];
  products: IProduct[];
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
  minDate: Date;
  maxDate: Date;
  //totalCount = 0;
  bsConfig: Partial<BsDatepickerConfig>;
  bsValue: Date = new Date();
  isAdmin: boolean = false;
  empId: string;
  sbu: string;
  investmentDetailsOld: IInvestmentDetailOld[];
  lastFiveInvestmentDetail: ILastFiveInvestmentDetail[];
  config = {
    keyboard: false,
    class: 'modal-lg',
    ignoreBackdropClick: true
  };
  userRole: any;
  constructor(private accountService: AccountService, public investmentRecService: InvestmentRecService, private router: Router,
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
  getDonation() {
    this.investmentRecService.getDonations().subscribe(response => {
      this.donations = response as IDonation[];
    }, error => {
      console.log(error);
    });
  }
  openInvestmentInitSearchModal(template: TemplateRef<any>) {
    this.InvestmentInitSearchModalRef = this.modalService.show(template, this.config);
  }
  openInvestmentRecSearchModal(template: TemplateRef<any>) {
    this.InvestmentRecSearchModalRef = this.modalService.show(template, this.config);
  }
  async selectInvestmentInit(selectedRecord: IInvestmentInit) {
    this.resetForm();
    debugger;
    this.investmentRecService.investmentRecFormData = Object.assign({}, selectedRecord);
    this.investmentRecService.investmentDetailFormData.investmentInitId = selectedRecord.id;
    this.investmentRecService.investmentRecCommentFormData.investmentInitId = selectedRecord.id;
    this.investmentRecService.investmentMedicineProdFormData.investmentInitId = selectedRecord.id;
    this.isDonationValid = true;
    this.convertedDate = this.datePipe.transform(selectedRecord.setOn, 'ddMMyyyy');
    if (this.investmentRecService.investmentRecFormData.donationTo == "Doctor") {
      await this.getInvestmentDoctor();
    }
    else if (this.investmentRecService.investmentRecFormData.donationTo == "Institution") {
      await this.getInvestmentInstitution();
    }
    else if (this.investmentRecService.investmentRecFormData.donationTo == "Campaign") {
      this.getInvestmentCampaign();
    }
    else if (this.investmentRecService.investmentRecFormData.donationTo == "Bcds") {
      await this.getInvestmentBcds();
    }
    else if (this.investmentRecService.investmentRecFormData.donationTo == "Society") {
      await this.getInvestmentSociety();
    }
    this.getInvestmentDetails();
    this.getInvestmentTargetedProd();
    if (this.investmentRecService.investmentRecFormData.donationId == 4) {
      this.getInvestmentMedicineProd();
    }
    this.getInvestmentTargetedGroup();
    if (this.sbu == this.investmentRecService.investmentRecFormData.sbu) {
      this.isInvOther = false;
      this.isValid = true;
    }
    else {
      this.isInvOther = true;
      this.isValid = false;
    }
    this.InvestmentInitSearchModalRef.hide()
  }
  async selectInvestmentRec(selectedRecord: IInvestmentInit) {
    this.resetForm();
    this.investmentRecService.investmentRecFormData = Object.assign({}, selectedRecord);
    this.investmentRecService.investmentDetailFormData.investmentInitId = selectedRecord.id;
    this.investmentRecService.investmentRecCommentFormData.investmentInitId = selectedRecord.id;
    this.investmentRecService.investmentMedicineProdFormData.investmentInitId = selectedRecord.id;
    this.convertedDate = this.datePipe.transform(selectedRecord.setOn, 'ddMMyyyy');
    this.isDonationValid = true;
    if (this.investmentRecService.investmentRecFormData.donationTo == "Doctor") {
      await this.getInvestmentDoctor();
    }
    else if (this.investmentRecService.investmentRecFormData.donationTo == "Institution") {
      await this.getInvestmentInstitution();
    }
    else if (this.investmentRecService.investmentRecFormData.donationTo == "Campaign") {
      await this.getInvestmentCampaign();
    }
    else if (this.investmentRecService.investmentRecFormData.donationTo == "Bcds") {
      await this.getInvestmentBcds();
    }
    else if (this.investmentRecService.investmentRecFormData.donationTo == "Society") {
      await this.getInvestmentSociety();
    }
    this.getInvestmentRecProducts();
    this.getInvestmentRecComment();
    if (this.investmentRecService.investmentRecFormData.donationId == 4) {
      this.getInvestmentMedicineProd();
    }
    this.getInvestmentTargetedGroup();
    if (this.sbu == this.investmentRecService.investmentRecFormData.sbu) {
      await this.getInvestmentRecDetails();
      this.isInvOther = false;
      this.isValid = true;
    }
    else {
      await this.getInvestmentDetails();
      this.isInvOther = true;
      this.isValid = false;
    }
    this.InvestmentRecSearchModalRef.hide()
  }
  async getLastFiveInvestment(marketCode: string, toDayDate: string) {
    if (this.investmentRecService.investmentRecFormData.donationTo == "Doctor") {
      await this.investmentRecService.getLastFiveInvestmentForDoc(this.investmentRecService.investmentRecFormData.donationId, this.investmentRecService.investmentDoctorFormData.doctorId, marketCode, toDayDate).then(
        (response) => {
          this.lastFiveInvestmentDetail = response as ILastFiveInvestmentDetail[];
        },
        (error) => {
          console.log(error);
        }
      );
    }
    else if (this.investmentRecService.investmentRecFormData.donationTo == "Institution") {
      await this.investmentRecService.getLastFiveInvestmentForInstitute(this.investmentRecService.investmentRecFormData.donationId, this.investmentRecService.investmentInstitutionFormData.institutionId, marketCode, toDayDate).then(
        (response) => {
          this.lastFiveInvestmentDetail = response as ILastFiveInvestmentDetail[];
        },
        (error) => {
          console.log(error);
        }
      );
    }
    else if (this.investmentRecService.investmentRecFormData.donationTo == "Campaign") {
      await this.investmentRecService.getLastFiveInvestmentForCampaign(this.investmentRecService.investmentRecFormData.donationId, this.investmentRecService.investmentCampaignFormData.campaignMstId, marketCode, toDayDate).then(
        (response) => {
          this.lastFiveInvestmentDetail = response as ILastFiveInvestmentDetail[];
        },
        (error) => {
          console.log(error);
        }
      );
    }
    else if (this.investmentRecService.investmentRecFormData.donationTo == "Bcds") {
      await this.investmentRecService.getLastFiveInvestmentForBcds(this.investmentRecService.investmentRecFormData.donationId, this.investmentRecService.investmentBcdsFormData.bcdsId, marketCode, toDayDate).then(
        (response) => {
          this.lastFiveInvestmentDetail = response as ILastFiveInvestmentDetail[];
        },
        (error) => {
          console.log(error);
        }
      );
    }
    else if (this.investmentRecService.investmentRecFormData.donationTo == "Society") {
      await this.investmentRecService.getLastFiveInvestmentForSociety(this.investmentRecService.investmentRecFormData.donationId, this.investmentRecService.investmentSocietyFormData.societyId, marketCode, toDayDate).then(
        (response) => {
          this.lastFiveInvestmentDetail = response as ILastFiveInvestmentDetail[];
        },
        (error) => {
          console.log(error);
        }
      );
    }

  }
  getCampaignMst() {
    this.investmentRecService.getCampaignMsts(parseInt(this.empId)).subscribe(response => {
      this.campaignMsts = response as ICampaignMst[];
    }, error => {
      console.log(error);
    });
  }
  getInvestmentInit() {
    this.SpinnerService.show();
    this.investmentRecService.getInvestmentInit(parseInt(this.empId), this.sbu).subscribe(response => {
      this.SpinnerService.hide();
      this.investmentInits = response as IInvestmentInit[];

      if (this.investmentInits.length > 0) {
        this.openInvestmentInitSearchModal(this.investmentInitSearchModal);
      } else {
        this.toastr.warning('No Data Found');
      }
    }, error => {
      this.SpinnerService.hide();
      console.log(error);
    });
  }
  getInvestmentRecommended() {
    const params = this.investmentRecService.getGenParams();
    this.SpinnerService.show();
    this.investmentRecService.getInvestmentRecommended(parseInt(this.empId), this.sbu, this.userRole).subscribe(response => {
      this.SpinnerService.hide();
      this.investmentInits = response as IInvestmentInit[];
      // this.totalCount = response.count;
      // this.configs = {
      //   currentPage: params.pageIndex,
      //   itemsPerPage: params.pageSize,
      //   totalItems: this.totalCount,
      // };
      if (this.investmentInits.length > 0) {
        this.openInvestmentRecSearchModal(this.investmentRecSearchModal);
      }
      else {
        this.toastr.warning('No Data Found', 'Investment');
      }
    }, error => {
      this.SpinnerService.hide();
      console.log(error);
    });
  }
  dateCompare(form: NgForm) {
    if (this.investmentRecService.investmentDetailFormData.fromDate != null && this.investmentRecService.investmentDetailFormData.toDate != null) {
      if (this.investmentRecService.investmentDetailFormData.toDate > this.investmentRecService.investmentDetailFormData.fromDate) {
      }
      else {
        form.controls.fromDate.setValue(null);
        form.controls.toDate.setValue(null);
        this.toastr.error('Select Appropriate Date Range', 'Error')
      }
    }
  }
  dateCommitmentCompare(form: NgForm) {
    if (this.investmentRecService.investmentDetailFormData.commitmentFromDate != null && this.investmentRecService.investmentDetailFormData.commitmentToDate != null) {
      if (this.investmentRecService.investmentDetailFormData.commitmentToDate > this.investmentRecService.investmentDetailFormData.commitmentFromDate) {
      }
      else {
        form.controls.commitmentFromDate.setValue(null);
        form.controls.commitmentToDate.setValue(null);
        this.toastr.error('Select Appropriate Commitment Date Range');
      }
    }
  }
  getInvestmentCampaign() {
    this.investmentRecService.getInvestmentCampaigns(this.investmentRecService.investmentRecFormData.id).subscribe(response => {
      var data = response[0] as IInvestmentCampaign;
      if (data !== undefined) {
        debugger;
        this.investmentRecService.investmentCampaignFormData = data;
        this.investmentRecService.investmentCampaignFormData.campaignMstId = data.campaignDtl.mstId;
        //this.investmentRecService.investmentCampaignFormData.subCampaignName = data.campaignDtl.subCampaignName;
        //this.investmentRecService.investmentCampaignFormData.subCampaignName = data.campaignDtl.subCampaignName;
        this.investmentRecService.investmentCampaignFormData.doctorName = data.doctorInfo.doctorName;
        this.investmentRecService.investmentCampaignFormData.institutionName = data.institutionInfo.institutionName;
        this.investmentRecService.investmentCampaignFormData.subCampStartDate = new DatePipe('en-US').transform(data.campaignDtl.subCampStartDate, 'dd/MM/yyyy');
        this.investmentRecService.investmentCampaignFormData.subCampEndDate = new DatePipe('en-US').transform(data.campaignDtl.subCampEndDate, 'dd/MM/yyyy')
        this.investmentRecService.getCampaignMsts(this.investmentRecService.investmentRecFormData.employeeId).subscribe(response => {
          this.campaignMsts = response as ICampaignMst[];
          for (let i = 0; i < this.campaignMsts.length; i++) {
            if (this.campaignMsts[i].id == this.investmentRecService.investmentCampaignFormData.campaignDtl.mstId) {
              this.investmentRecService.investmentCampaignFormData.campaignName = this.campaignMsts[i].campaignName;
            }
          }
          this.investmentRecService.getCampaignDtls(data.campaignDtl.mstId).subscribe(response => {
            this.campaignDtls = response as ICampaignDtl[];
            for (let i = 0; i < this.campaignDtls.length; i++) {
              if (this.campaignDtls[i].id == data.campaignDtl.id) {
                this.investmentRecService.investmentCampaignFormData.subCampaignName = this.campaignDtls[i].subCampaign.subCampaignName;
              }
            }
          }, error => {
            console.log(error);
          });
        }, error => {
          console.log(error);
        });
        this.investmentRecService.getCampaignDtlProducts(data.campaignDtl.id).subscribe(response => {
          this.campaignDtlProducts = response as ICampaignDtlProduct[];
        }, error => {
          console.log(error);
        });
      }
    }, error => {
      console.log(error);
    });

  }
  async getInvestmentBcds() {
    await this.investmentRecService.getInvestmentBcds(this.investmentRecService.investmentRecFormData.id).then(response => {
      var data = response[0] as IInvestmentBcds;
      if (data !== undefined) {
        this.investmentRecService.investmentBcdsFormData = data;
        this.investmentRecService.investmentBcdsFormData.responsibleDoctorName = data.doctorInfo.doctorName;
        this.investmentRecService.investmentBcdsFormData.bcdsName = data.bcds.bcdsName;
        this.investmentRecService.investmentBcdsFormData.bcdsAddress = data.bcds.bcdsAddress;
        this.investmentRecService.investmentBcdsFormData.noOfMember = data.bcds.noOfMember;
      }
    }, error => {
      console.log(error);
    });
  }
  async getInvestmentSociety() {
    await this.investmentRecService.getInvestmentSociety(this.investmentRecService.investmentRecFormData.id).then(response => {
      var data = response[0] as IInvestmentSociety;
      if (data !== undefined) {
        this.investmentRecService.investmentSocietyFormData = data;
        this.investmentRecService.investmentSocietyFormData.responsibleDoctorName = data.doctorInfo.doctorName;
        this.investmentRecService.investmentSocietyFormData.societyName = data.society.societyName;
        this.investmentRecService.investmentSocietyFormData.noOfMember = data.society.noOfMember;
        this.investmentRecService.investmentSocietyFormData.societyAddress = data.society.societyAddress;
      }
    }, error => {
      console.log(error);
    });
  }
  async getInvestmentInstitution() {
    await this.investmentRecService.getInvestmentInstitutions(this.investmentRecService.investmentRecFormData.id).then(response => {
      var data = response[0] as IInvestmentInstitution;
      if (data !== undefined) {
        this.investmentRecService.investmentInstitutionFormData = data;
        this.investmentRecService.investmentInstitutionFormData.responsibleDoctorName = data.doctorInfo.doctorName;
        this.investmentRecService.investmentInstitutionFormData.institutionName = data.institutionInfo.institutionName;
        this.investmentRecService.investmentInstitutionFormData.address = data.institutionInfo.address;
        this.investmentRecService.investmentInstitutionFormData.institutionType = data.institutionInfo.institutionType;
      }

    }, error => {
      console.log(error);
    });
  }
  async getInvestmentDoctor() {
    await this.investmentRecService.getInvestmentDoctors(this.investmentRecService.investmentRecFormData.id).then(response => {
      var data = response[0] as IInvestmentDoctor;
      if (data !== undefined) {
        this.investmentRecService.investmentDoctorFormData = data;
        this.investmentRecService.investmentDoctorFormData.doctorName = data.doctorInfo.doctorCode + '-' + data.doctorInfo.doctorName;
        this.investmentRecService.investmentDoctorFormData.degree = data.doctorInfo.degree;
        this.investmentRecService.investmentDoctorFormData.designation = data.doctorInfo.designation;
        this.investmentRecService.investmentDoctorFormData.institutionName = data.institutionInfo.institutionCode + '-' + data.institutionInfo.institutionName;
        this.investmentRecService.investmentDoctorFormData.address = data.institutionInfo.address;
      }

    }, error => {
      console.log(error);
    });
  }
  getInvestmentRecComment() {
    this.investmentRecService.getInvestmentRecComment(this.investmentRecService.investmentRecFormData.id, this.empId).subscribe(response => {
      //
      var data = response[0] as IInvestmentRecComment;
      if (data !== undefined) {
        this.investmentRecService.investmentRecCommentFormData = data;
      }
    }, error => {
      console.log(error);
    });
  }
  async getInvestmentDetails() {
    await this.investmentRecService.getInvestmentDetails(this.investmentRecService.investmentRecFormData.id).then(async response => {
      var data = response[0] as IInvestmentRec;
      if (data !== undefined) {
        this.investmentRecService.investmentDetailFormData = data;
        this.investmentRecService.investmentDetailFormData.id = 0;
        this.investmentRecService.investmentDetailFormData.fromDate = new Date(data.fromDate);
        this.investmentRecService.investmentDetailFormData.toDate = new Date(data.toDate);
        this.investmentRecService.investmentDetailFormData.commitmentFromDate = new Date(data.commitmentFromDate);
        this.investmentRecService.investmentDetailFormData.commitmentToDate = new Date(data.commitmentToDate);
        //let convertedDate = this.datePipe.transform(data.fromDate, 'ddMMyyyy');
        await this.getLastFiveInvestment(this.investmentRecService.investmentRecFormData.marketCode, this.convertedDate);
      }
    }, error => {
      console.log(error);
    });
  }
  getInvestmentMedicineProd() {
    this.investmentRecService.getInvestmentMedicineProds(this.investmentRecService.investmentRecFormData.id, this.sbu).subscribe(response => {
      var data = response as IInvestmentMedicineProd[];
      if (data !== undefined) {
        this.investmentMedicineProd = data;
        let sum = 0;
        debugger;
        for (let i = 0; i < this.investmentMedicineProd.length; i++) {
          sum = sum + this.investmentMedicineProd[i].tpVat;
        }
        //this.investmentRecService.investmentDetailFormData.proposedAmount=sum.toString();
        this.investmentRecService.investmentDetailFormData.proposedAmount = ((Math.round(sum * 100) / 100).toFixed(2));
      }
      else {
        this.investmentRecService.investmentDetailFormData.proposedAmount = '';
        this.investmentMedicineProd = [];
        //this.toastr.warning('No Data Found', 'Investment');
      }
    }, error => {
      console.log(error);

    });
  }
  getInvestmentTargetedProd() {
    this.investmentRecService.getInvestmentTargetedProds(this.investmentRecService.investmentRecFormData.id, this.sbu).subscribe(response => {
      var data = response as IInvestmentTargetedProd[];
      if (data !== undefined) {
        this.investmentTargetedProds = data;
      }
    }, error => {
      console.log(error);
    });
  }
  async getInvestmentRecDetails() {
    this.investmentRecService.getInvestmentRecDetails(this.investmentRecService.investmentRecFormData.id, parseInt(this.empId)).subscribe(async response => {
      var data = response[0] as IInvestmentRec;
      debugger;
      if (data !== undefined) {
        this.investmentRecService.investmentDetailFormData = data;
        this.investmentRecService.investmentDetailFormData.id = 0;
        this.investmentRecService.investmentDetailFormData.fromDate = new Date(data.fromDate);
        this.investmentRecService.investmentDetailFormData.toDate = new Date(data.toDate);
        this.investmentRecService.investmentDetailFormData.commitmentFromDate = new Date(data.commitmentFromDate);
        this.investmentRecService.investmentDetailFormData.commitmentToDate = new Date(data.commitmentToDate);
        //let convertedDate = this.datePipe.transform(data.fromDate, 'ddMMyyyy');
        await this.getLastFiveInvestment(this.investmentRecService.investmentRecFormData.marketCode, this.convertedDate);
      } else {
        this.getInvestmentDetails();
      }
    }, error => {
      console.log(error);
    });
  }
  getInvestmentRecProducts() {
    this.investmentRecService.getInvestmentRecProducts(this.investmentRecService.investmentRecFormData.id, this.sbu).subscribe(response => {
      var data = response as IInvestmentTargetedProd[];
      if (data !== undefined) {
        this.investmentTargetedProds = data;
      }
    }, error => {
      console.log(error);
    });
  }
  getInvestmentTargetedGroup() {
    this.investmentRecService.getInvestmentTargetedGroups(this.investmentRecService.investmentRecFormData.id, parseInt(this.empId)).subscribe(response => {
      var data = response as IInvestmentTargetedGroup[];
      if (data !== undefined) {
        this.investmentTargetedGroups = data;
      }

    }, error => {
      console.log(error);
    });
  }
  resetTargetedGroup() {
    if(this.investmentRecService.investmentRecFormData.id!=null && this.investmentRecService.investmentRecFormData.id!=undefined && this.investmentRecService.investmentRecFormData.id!=0){
    this.investmentRecService.getInvestmentTargetedGroups(this.investmentRecService.investmentRecFormData.id, parseInt(this.empId)).subscribe(response => {
      var data = response as IInvestmentTargetedGroup[];
      if (data !== undefined) {
        this.investmentTargetedGroups = data;
      }
    }, error => {
      console.log(error);
    });
  }
  }
  changeDateInDetail() {
    if (this.investmentRecService.investmentDetailFormData.fromDate == null || this.investmentRecService.investmentDetailFormData.fromDate == undefined) {
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.toDate == null || this.investmentRecService.investmentDetailFormData.toDate == undefined) {
      return false;
    }
    let dateFrom = this.investmentRecService.investmentDetailFormData.fromDate;
    let dateTo = this.investmentRecService.investmentDetailFormData.toDate;
    this.investmentRecService.investmentDetailFormData.totalMonth = dateTo.getMonth() - dateFrom.getMonth() + (12 * (dateTo.getFullYear() - dateFrom.getFullYear()));
    this.investmentRecService.investmentDetailFormData.totalMonth = this.investmentRecService.investmentDetailFormData.totalMonth + 1;
  }
  changeCommitmentDateInDetail() {
    //this.printingDate=this.getDigitBanglaFromEnglish(this.datePipe.transform(value, "dd/MM/yyyy"));
    if (this.investmentRecService.investmentDetailFormData.commitmentFromDate == null || this.investmentRecService.investmentDetailFormData.commitmentFromDate == undefined) {
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.commitmentToDate == null || this.investmentRecService.investmentDetailFormData.commitmentToDate == undefined) {
      return false;
    }
    let dateFrom = this.investmentRecService.investmentDetailFormData.commitmentFromDate;
    let dateTo = this.investmentRecService.investmentDetailFormData.commitmentToDate;
    this.investmentRecService.investmentDetailFormData.commitmentTotalMonth = dateTo.getMonth() - dateFrom.getMonth() + (12 * (dateTo.getFullYear() - dateFrom.getFullYear()));
    this.investmentRecService.investmentDetailFormData.commitmentTotalMonth = this.investmentRecService.investmentDetailFormData.commitmentTotalMonth + 1;
  }
  changePaymentMethod() {
    if (this.investmentRecService.investmentDetailFormData.paymentMethod != 'Cheque') {
      this.investmentRecService.investmentDetailFormData.chequeTitle = "" ;
    }
   }
  getProduct() {
    this.investmentRecService.getProduct(this.sbu).subscribe(response => {
      this.products = response as IProduct[];
    }, error => {
      console.log(error);
    });
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
    this.investmentRecService.investmentRecCommentFormData.employeeId = parseInt(this.empId);
    this.getEmployeeSbu();
  }
  getEmployeeSbu() {
    this.accountService.getEmployeeSbu(this.investmentRecService.investmentRecCommentFormData.employeeId).subscribe(
      (response) => {
        this.sbu = response.sbu;
        this.getProduct();
      },
      (error) => {
        console.log(error);
      }
    );
  }
  onSubmit(form: NgForm) {
    if (this.investmentRecService.investmentRecCommentFormData.id == null || this.investmentRecService.investmentRecCommentFormData.id == undefined || this.investmentRecService.investmentRecCommentFormData.id == 0)
      this.insertInvestmentRec();
    else
      this.updateInvestmentRec();
  }
  insertInvestmentRec() {
    this.investmentRecService.investmentRecCommentFormData.employeeId = parseInt(this.empId);
    if (this.investmentRecService.investmentDetailFormData.proposedAmount == null || this.investmentRecService.investmentDetailFormData.proposedAmount == undefined || this.investmentRecService.investmentDetailFormData.proposedAmount == "") {
      this.toastr.warning('Enter Proposed Amount First', 'Investment ');
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.purpose == null || this.investmentRecService.investmentDetailFormData.purpose == undefined || this.investmentRecService.investmentDetailFormData.purpose == "") {
      this.toastr.warning('Enter Purpose First', 'Investment');
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.paymentFreq == null || this.investmentRecService.investmentDetailFormData.paymentFreq == undefined || this.investmentRecService.investmentDetailFormData.paymentFreq == "") {
      this.toastr.warning('Select Payment Frequency First', 'Investment Detail');
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.fromDate == null || this.investmentRecService.investmentDetailFormData.fromDate == undefined) {
      this.toastr.warning('Select Payment Dur. From Date  First', 'Investment Detail');
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.toDate == null || this.investmentRecService.investmentDetailFormData.toDate == undefined) {
      this.toastr.warning('Select Payment Dur. To Date  First', 'Investment Detail');
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.totalMonth  == null || this.investmentRecService.investmentDetailFormData.totalMonth  == undefined || this.investmentRecService.investmentDetailFormData.totalMonth==0) {
      this.toastr.warning('Invalid Payment Total Month', 'Investment Detail');
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.commitmentFromDate == null || this.investmentRecService.investmentDetailFormData.commitmentFromDate == undefined) {
      this.toastr.warning('Select Commitment From Date  First', 'Investment Detail');
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.commitmentToDate == null || this.investmentRecService.investmentDetailFormData.commitmentToDate == undefined) {
      this.toastr.warning('Select Commitment To Date  First', 'Investment Detail');
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.commitmentTotalMonth  == null || this.investmentRecService.investmentDetailFormData.commitmentTotalMonth  == undefined || this.investmentRecService.investmentDetailFormData.commitmentTotalMonth==0) {
      this.toastr.warning('Invalid Commitment Total Month', 'Investment Detail');
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.commitmentAllSBU == null || this.investmentRecService.investmentDetailFormData.commitmentAllSBU == undefined || this.investmentRecService.investmentDetailFormData.commitmentAllSBU == "") {
      this.toastr.warning('Enter Commitment All SBU First', 'Investment ');
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.commitmentOwnSBU == null || this.investmentRecService.investmentDetailFormData.commitmentOwnSBU == undefined || this.investmentRecService.investmentDetailFormData.commitmentOwnSBU == "") {
      this.toastr.warning('Enter Commitment Own SBU First', 'Investment ');
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.paymentMethod == null || this.investmentRecService.investmentDetailFormData.paymentMethod == undefined || this.investmentRecService.investmentDetailFormData.paymentMethod == "") {
      this.toastr.warning('Select Payment Method First', 'Investment');
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.paymentMethod == 'Cheque') {
      if (this.investmentRecService.investmentDetailFormData.chequeTitle == null || this.investmentRecService.investmentDetailFormData.chequeTitle == undefined || this.investmentRecService.investmentDetailFormData.chequeTitle == "") {
        this.toastr.warning('Enter  Cheque Title First', 'Investment Detail');
        return false;
      }
    }
    if (this.investmentRecService.investmentRecCommentFormData.recStatus == 'Not Recommended' || this.investmentRecService.investmentRecCommentFormData.recStatus == 'Cancelled') {
      if (this.investmentRecService.investmentRecCommentFormData.comments == null || this.investmentRecService.investmentRecCommentFormData.comments == undefined || this.investmentRecService.investmentRecCommentFormData.comments == "") {
        this.toastr.warning('Please Insert Comment For Not Recommendation / Cancellation', 'Investment');
        return false;
      }
    }
    if (this.investmentRecService.investmentTargetedProdFormData.productId != null && this.investmentRecService.investmentTargetedProdFormData.productId != undefined && this.investmentRecService.investmentTargetedProdFormData.productId != 0) {
      this.toastr.warning('Please Add the Selected Product First', 'Investment ');
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.paymentFreq == 'Quarterly') {
      if (this.investmentRecService.investmentDetailFormData.totalMonth  <3) {
        this.toastr.warning('Duration can not be less than 3 Month for Quarterly Investment ');
        return false;
      }
    }
    if (this.investmentRecService.investmentDetailFormData.paymentFreq == 'Half Yearly') {
      if (this.investmentRecService.investmentDetailFormData.totalMonth  <6) {
        this.toastr.warning('Duration can not be less than 6 Month for Half Yearly Investment');
        return false;
      }
    }
    this.SpinnerService.show();
    //if (this.sbu == this.investmentRecService.investmentRecFormData.sbu) {
      this.investmentRecService.investmentDetailFormData.fromDate  = this.datePipe.transform(this.investmentRecService.investmentDetailFormData.fromDate, 'yyyy-MM-dd HH:mm:ss');
      this.investmentRecService.investmentDetailFormData.toDate= this.datePipe.transform(this.investmentRecService.investmentDetailFormData.toDate, 'yyyy-MM-dd HH:mm:ss');
    
      this.investmentRecService.investmentDetailFormData.commitmentFromDate  = this.datePipe.transform(this.investmentRecService.investmentDetailFormData.commitmentFromDate, 'yyyy-MM-dd HH:mm:ss');
      this.investmentRecService.investmentDetailFormData.commitmentToDate= this.datePipe.transform(this.investmentRecService.investmentDetailFormData.commitmentToDate, 'yyyy-MM-dd HH:mm:ss');
    
      this.investmentRecService.investmentDetailFormData.investmentInitId = this.investmentRecService.investmentRecFormData.id;
      this.investmentRecService.insertRecommendForOwnSBU(parseInt(this.empId), this.sbu,this.investmentTargetedProds).subscribe(
        res => {
          this.investmentRecService.investmentRecCommentFormData = res as IInvestmentRecComment;
          this.isDonationValid = true;
          this.isValid = true;
          this.getInvestmentRecDetails();
          this.getInvestmentTargetedGroup();
          this.SpinnerService.hide();
          this.toastr.info('Save successfully')
          
        },
        err => { 
          this.investmentRecService.investmentDetailFormData.fromDate = new Date(this.investmentRecService.investmentDetailFormData.fromDate);
        this.investmentRecService.investmentDetailFormData.toDate = new Date(this.investmentRecService.investmentDetailFormData.toDate); 
        this.investmentRecService.investmentDetailFormData.commitmentFromDate = new Date(this.investmentRecService.investmentDetailFormData.commitmentFromDate);
        this.investmentRecService.investmentDetailFormData.commitmentToDate = new Date(this.investmentRecService.investmentDetailFormData.commitmentToDate);
        
          console.log(err);
          this.SpinnerService.hide();
         }
      );

    //}
    // else {
    //   this.investmentRecService.investmentDetailFormData.investmentInitId = this.investmentRecService.investmentRecFormData.id;
  
    //   this.investmentRecService.insertRecommendForOtherSBU(parseInt(this.empId), this.sbu,this.investmentTargetedProds).subscribe(
    //     res => {
    //       this.investmentRecService.investmentRecCommentFormData = res as IInvestmentRecComment;
    //       this.isDonationValid = true;
    //       this.isValid = true;
    //       this.getInvestmentRecDetails();
    //       this.getInvestmentTargetedGroup();
    //       this.SpinnerService.hide();
    //       this.toastr.info('Save successfully')
    //     },
    //     err => { 
    //       console.log(err);
    //       this.SpinnerService.hide();
    //     }
    //   );
    // }
  }
  updateInvestmentRec() {
    if (this.investmentRecService.investmentDetailFormData.proposedAmount == null || this.investmentRecService.investmentDetailFormData.proposedAmount == undefined || this.investmentRecService.investmentDetailFormData.proposedAmount == "") {
      this.toastr.warning('Enter Proposed Amount First', 'Investment');
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.purpose == null || this.investmentRecService.investmentDetailFormData.purpose == undefined || this.investmentRecService.investmentDetailFormData.purpose == "") {
      this.toastr.warning('Enter Purpose First', 'Investment');
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.paymentFreq == null || this.investmentRecService.investmentDetailFormData.paymentFreq == undefined || this.investmentRecService.investmentDetailFormData.paymentFreq == "") {
      this.toastr.warning('Select Payment Frequency First', 'Investment Detail');
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.fromDate == null || this.investmentRecService.investmentDetailFormData.fromDate == undefined) {
      this.toastr.warning('Select Payment Dur. From Date  First', 'Investment Detail');
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.toDate == null || this.investmentRecService.investmentDetailFormData.toDate == undefined) {
      this.toastr.warning('Select Payment Dur. To Date  First', 'Investment Detail');
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.totalMonth  == null || this.investmentRecService.investmentDetailFormData.totalMonth  == undefined || this.investmentRecService.investmentDetailFormData.totalMonth==0) {
      this.toastr.warning('Invalid Payment Total Month', 'Investment Detail');
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.commitmentFromDate == null || this.investmentRecService.investmentDetailFormData.commitmentFromDate == undefined) {
      this.toastr.warning('Select Commitment From Date  First', 'Investment Detail');
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.commitmentToDate == null || this.investmentRecService.investmentDetailFormData.commitmentToDate == undefined) {
      this.toastr.warning('Select Commitment To Date  First', 'Investment Detail');
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.commitmentTotalMonth  == null || this.investmentRecService.investmentDetailFormData.commitmentTotalMonth  == undefined || this.investmentRecService.investmentDetailFormData.commitmentTotalMonth==0) {
      this.toastr.warning('Invalid Commitment Total Month', 'Investment Detail');
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.commitmentAllSBU == null || this.investmentRecService.investmentDetailFormData.commitmentAllSBU == undefined || this.investmentRecService.investmentDetailFormData.commitmentAllSBU == "") {
      this.toastr.warning('Enter Commitment All SBU First', 'Investment');
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.commitmentOwnSBU == null || this.investmentRecService.investmentDetailFormData.commitmentOwnSBU == undefined || this.investmentRecService.investmentDetailFormData.commitmentOwnSBU == "") {
      this.toastr.warning('Enter Commitment Own SBU First', 'Investment');
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.paymentMethod == null || this.investmentRecService.investmentDetailFormData.paymentMethod == undefined || this.investmentRecService.investmentDetailFormData.paymentMethod == "") {
      this.toastr.warning('Select Payment Method First', 'Investment');
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.paymentMethod == 'Cheque') {
      if (this.investmentRecService.investmentDetailFormData.chequeTitle == null || this.investmentRecService.investmentDetailFormData.chequeTitle == undefined || this.investmentRecService.investmentDetailFormData.chequeTitle == "") {
        this.toastr.warning('Enter Cheque Title First', 'Investment Detail');
        return false;
      }
    }
    if (this.investmentRecService.investmentRecCommentFormData.recStatus == 'Not Recommended') {
      if (this.investmentRecService.investmentRecCommentFormData.comments == null || this.investmentRecService.investmentRecCommentFormData.comments == undefined || this.investmentRecService.investmentRecCommentFormData.comments == "") {

        this.toastr.warning('Please Insert Comment For Not Recommendation', 'Investment');
        return false;
      }
    }
    if (this.investmentRecService.investmentTargetedProdFormData.productId != null && this.investmentRecService.investmentTargetedProdFormData.productId != undefined && this.investmentRecService.investmentTargetedProdFormData.productId != 0) {
      this.toastr.warning('Please Add the Selected Product First', 'Investment ');
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.paymentFreq == 'Quarterly') {
      if (this.investmentRecService.investmentDetailFormData.totalMonth  <3) {
        this.toastr.warning('Duration can not be less than 3 Month for Quarterly Investment ');
        return false;
      }
    }
    if (this.investmentRecService.investmentDetailFormData.paymentFreq == 'Half Yearly') {
      if (this.investmentRecService.investmentDetailFormData.totalMonth  <6) {
        this.toastr.warning('Duration can not be less than 6 Month for Half Yearly Investment');
        return false;
      }
    }
    this.SpinnerService.show();
    //if (this.sbu == this.investmentRecService.investmentRecFormData.sbu) {
      this.investmentRecService.investmentDetailFormData.fromDate  = this.datePipe.transform(this.investmentRecService.investmentDetailFormData.fromDate, 'yyyy-MM-dd HH:mm:ss');
      this.investmentRecService.investmentDetailFormData.toDate= this.datePipe.transform(this.investmentRecService.investmentDetailFormData.toDate, 'yyyy-MM-dd HH:mm:ss');
    
      this.investmentRecService.investmentDetailFormData.commitmentFromDate  = this.datePipe.transform(this.investmentRecService.investmentDetailFormData.commitmentFromDate, 'yyyy-MM-dd HH:mm:ss');
      this.investmentRecService.investmentDetailFormData.commitmentToDate= this.datePipe.transform(this.investmentRecService.investmentDetailFormData.commitmentToDate, 'yyyy-MM-dd HH:mm:ss');
    
      this.investmentRecService.investmentDetailFormData.investmentInitId = this.investmentRecService.investmentRecFormData.id;
      this.investmentRecService.updateRecommendForOwnSBU(parseInt(this.empId), this.sbu,this.investmentTargetedProds).subscribe(
        res => {
          this.investmentRecService.investmentRecCommentFormData = res as IInvestmentRecComment;
          this.isDonationValid = true;
          this.isValid = true;
          this.getInvestmentRecDetails();
          this.getInvestmentTargetedGroup();
          this.SpinnerService.hide();
          this.toastr.info('Updated successfully', 'Investment ')
        },
        err => { 
          this.investmentRecService.investmentDetailFormData.fromDate = new Date(this.investmentRecService.investmentDetailFormData.fromDate);
          this.investmentRecService.investmentDetailFormData.toDate = new Date(this.investmentRecService.investmentDetailFormData.toDate); 
          this.investmentRecService.investmentDetailFormData.commitmentFromDate = new Date(this.investmentRecService.investmentDetailFormData.commitmentFromDate);
          this.investmentRecService.investmentDetailFormData.commitmentToDate = new Date(this.investmentRecService.investmentDetailFormData.commitmentToDate);
          console.log(err); 
          this.SpinnerService.hide();
        }
      );
    // }
    // else {
    //   this.investmentRecService.investmentDetailFormData.investmentInitId = this.investmentRecService.investmentRecFormData.id;
      
    //   this.investmentRecService.updateRecommendForOtherSBU(parseInt(this.empId), this.sbu,this.investmentTargetedProds).subscribe(
    //     res => {
    //       this.investmentRecService.investmentRecCommentFormData = res as IInvestmentRecComment;
    //       this.isDonationValid = true;
    //       this.isValid = true;
    //       this.getInvestmentRecDetails();
    //       this.getInvestmentTargetedGroup();
    //       this.SpinnerService.hide();
    //       this.toastr.info('Updated successfully', 'Investment ')
          
    //     },
    //     err => { 
    //       console.log(err); 
    //       this.SpinnerService.hide();}
    //   );

    // }
    // this.investmentRecService.updateInvestmentRec().subscribe(
    //   res => {
    //     //
    //     this.investmentRecService.investmentRecCommentFormData = res as IInvestmentRecComment;
    //     if (this.sbu == this.investmentRecService.investmentRecFormData.sbu) {
    //       this.insertInvestmentDetails();
    //     }
    //     this.isValid = true;
    //     this.insertInvestmentTargetedProd();
    //     this.getInvestmentTargetedGroup();
    //     this.SpinnerService.hide();
    //     this.toastr.info('Updated successfully', 'Investment ')
    //   },
    //   err => {
    //     console.log(err);
    //     this.SpinnerService.hide();
    //   }
    // );
  }
  // insertInvestmentDetails() {
  //   this.investmentRecService.investmentDetailFormData.investmentInitId = this.investmentRecService.investmentRecFormData.id;
  //   this.investmentRecService.insertInvestmentDetail(parseInt(this.empId), this.sbu).subscribe(
  //     res => {
  //       var data = res as IInvestmentRec;
  //       this.investmentRecService.investmentDetailFormData = data;
  //       this.investmentRecService.investmentDetailFormData.fromDate = new Date(data.fromDate);
  //       this.investmentRecService.investmentDetailFormData.toDate = new Date(data.toDate);
  //       this.investmentRecService.investmentDetailFormData.commitmentFromDate = new Date(data.commitmentFromDate);
  //       this.investmentRecService.investmentDetailFormData.commitmentToDate = new Date(data.commitmentToDate);
  //       this.isDonationValid = true;
  //     },
      
  //     err => {
  //       this.investmentRecService.investmentDetailFormData.fromDate  = this.datePipe.transform(this.investmentRecService.investmentDetailFormData.fromDate, 'yyyy-MM-dd HH:mm:ss');
  //       this.investmentRecService.investmentDetailFormData.toDate= this.datePipe.transform(this.investmentRecService.investmentDetailFormData.toDate, 'yyyy-MM-dd HH:mm:ss');
  //       this.investmentRecService.investmentDetailFormData.commitmentFromDate  = this.datePipe.transform(this.investmentRecService.investmentDetailFormData.commitmentFromDate, 'yyyy-MM-dd HH:mm:ss');
  //       this.investmentRecService.investmentDetailFormData.commitmentToDate= this.datePipe.transform(this.investmentRecService.investmentDetailFormData.commitmentToDate, 'yyyy-MM-dd HH:mm:ss');
  //       console.log(err); 
  //       }
  //   );
  // }

  insertInvestmentMedicineProd() {
    if (this.investmentRecService.investmentMedicineProdFormData.investmentInitId == null || this.investmentRecService.investmentMedicineProdFormData.investmentInitId == undefined || this.investmentRecService.investmentMedicineProdFormData.investmentInitId == 0) {
      this.toastr.warning('Insert Investment Initialisation First!');
      return false;
    }

    if (this.investmentRecService.investmentMedicineProdFormData.productId == null || this.investmentRecService.investmentMedicineProdFormData.productId == undefined || this.investmentRecService.investmentMedicineProdFormData.productId == 0) {
      this.toastr.warning('Select Product First');
      return false;
    }
    if (this.investmentMedicineProd !== undefined) {
      for (let i = 0; i < this.investmentMedicineProd.length; i++) {
        if (this.investmentMedicineProd[i].productId === this.investmentRecService.investmentMedicineProdFormData.productId) {
          this.toastr.warning("Product already exist!");
          return false;
        }
      }
    }
    this.investmentRecService.investmentMedicineProdFormData.employeeId = parseInt(this.empId);
    this.SpinnerService.show();
    this.investmentRecService.insertInvestmentMedicineProd().subscribe(
      res => {
        this.investmentRecService.investmentMedicineProdFormData = new InvestmentMedicineProd();
        this.getInvestmentMedicineProd();
        this.isDonationValid = true;
        this.toastr.success('Save successfully');
      },
      err => { console.log(err); }
    );
  }
  removeInvestmentMedicineProd(selectedRecord: IInvestmentMedicineProd) {
    var c = confirm("Are you sure you want to delete that?");
    if (c == true) {
      this.investmentRecService.investmentMedicineProdFormData = Object.assign({}, selectedRecord);
      this.SpinnerService.show();
      debugger;
      this.investmentRecService.removeInvestmentMedicineProd().subscribe(
        res => {
          //this.isDonationValid=false;
          this.investmentRecService.investmentMedicineProdFormData = new InvestmentMedicineProd();
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

  insertInvestmentTargetedProd() {
    if (this.investmentRecService.investmentRecFormData.id == null || this.investmentRecService.investmentRecFormData.id == undefined || this.investmentRecService.investmentRecFormData.id == 0) {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment ', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    if (this.investmentTargetedProds == undefined || this.investmentTargetedProds.length == 0) {
      return false;
    }
    this.investmentRecService.investmentTargetedProdFormData.investmentInitId = this.investmentRecService.investmentRecFormData.id;
    this.investmentRecService.insertInvestmentTargetedProd(this.investmentTargetedProds).subscribe(
      res => {
        this.getInvestmentRecProducts();
        this.isDonationValid = true;
        //this.toastr.success('Targeted Product Save successfully', 'Investment Targeted Product');
      },
      err => { console.log(err); }
    );
  }
  addInvestmentTargetedProd() {
    if (this.investmentRecService.investmentTargetedProdFormData.productId == null || this.investmentRecService.investmentTargetedProdFormData.productId == undefined || this.investmentRecService.investmentTargetedProdFormData.productId == 0) {
      this.toastr.warning('Select Product First', 'Investment ', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    if (this.investmentTargetedProds !== undefined) {
      for (let i = 0; i < this.investmentTargetedProds.length; i++) {
        if (this.investmentTargetedProds[i].productInfo.id == this.investmentRecService.investmentTargetedProdFormData.productId) {
          alert("product already exist !");
          return false;
        }

      }
      for (let i = 0; i < this.products.length; i++) {
        if (this.products[i].id == this.investmentRecService.investmentTargetedProdFormData.productId) {
          let data = new InvestmentTargetedProd();
          data.id = 0;
          data.sbu = this.sbu;
          data.employeeId = parseInt(this.empId);
          data.investmentInitId = this.investmentRecService.investmentRecFormData.id;
          data.productId = this.investmentRecService.investmentTargetedProdFormData.productId;
          data.productInfo = this.products[i];
          this.investmentTargetedProds.push(data);
          this.investmentRecService.investmentTargetedProdFormData = new InvestmentTargetedProd;
          return false;
        }

      }
    }
  }
  removeInvestmentTargetedProd(selectedRecord: IInvestmentTargetedProd) {
    debugger;
    if (this.investmentRecService.investmentRecCommentFormData.id == null || this.investmentRecService.investmentRecCommentFormData.id == undefined || this.investmentRecService.investmentRecCommentFormData.id == 0) {
      var c = confirm("Are you sure you want to remove this product?");
      if (c == true) {
        if (this.investmentTargetedProds.find(x => x.productId == selectedRecord.productId)) {
          this.investmentTargetedProds.splice(this.investmentTargetedProds.findIndex(x => x.productId == selectedRecord.productId), 1);
          this.toastr.success("Successfully Removed. Please Save the data.");
          return false;
        }
      }
    }
    else {
      var c = confirm("Are you sure you want to delete this  product?");
      if (c == true) {
        if (selectedRecord.id == 0) {
          if (this.investmentTargetedProds.find(x => x.productId == selectedRecord.productId)) {
            this.investmentTargetedProds.splice(this.investmentTargetedProds.findIndex(x => x.productId == selectedRecord.productId), 1);
            this.toastr.success("Successfully Removed");
            return false;
          }
        }
        if (this.investmentTargetedProds.find(x => x.productId == selectedRecord.productId)) {
          this.investmentTargetedProds.splice(this.investmentTargetedProds.findIndex(x => x.productId == selectedRecord.productId), 1);
        }
        // if (this.investmentRecService.investmentRecCommentFormData.id == null || this.investmentRecService.investmentRecCommentFormData.id == undefined || this.investmentRecService.investmentRecCommentFormData.id == 0) {
        //   //this.investmentRecService.investmentTargetedProdFormData = new InvestmentTargetedProd();
        //   return false;
        // }
        this.investmentRecService.investmentTargetedProdFormData = Object.assign({}, selectedRecord);
        this.investmentRecService.removeInvestmentTargetedProd().subscribe(
          res => {
            //
            this.toastr.success(res);
            this.investmentRecService.investmentTargetedProdFormData = new InvestmentTargetedProd();
            this.getInvestmentRecProducts();
          },
          err => {
            console.log(err);
          }
        );
      }

    }
  }
  populateForm() {
    //this.investmentRecService.campaignFormData = Object.assign({}, selectedRecord);
  }
  resetPage(form: NgForm) {
    window.location.reload();
    form.reset();
    this.investmentRecService.investmentRecFormData = new InvestmentInit();
    this.investmentRecService.investmentRecCommentFormData = new InvestmentRecComment();
    this.investmentRecService.investmentMedicineProdFormData = new InvestmentMedicineProd();
    this.investmentRecService.investmentDetailFormData = new InvestmentRec();
    this.investmentRecService.investmentTargetedProdFormData = new InvestmentTargetedProd();
    this.investmentRecService.investmentTargetedGroupFormData = new InvestmentTargetedGroup();
    this.investmentRecService.investmentDoctorFormData = new InvestmentDoctor();
    this.investmentRecService.investmentInstitutionFormData = new InvestmentInstitution();
    this.investmentRecService.investmentCampaignFormData = new InvestmentCampaign();
    this.investmentRecService.investmentBcdsFormData = new InvestmentBcds();
    this.investmentRecService.investmentSocietyFormData = new InvestmentSociety();
    this.investmentTargetedProds = [];
    this.investmentTargetedGroups = [];
    this.investmentDetailsOld = [];
    this.investmentMedicineProd = [];
    this.lastFiveInvestmentDetail = [];
    this.isAdmin = false;
    this.isValid = false;
    this.isInvOther = false;
    // this.configs = {
    //   currentPage: 1,
    //   itemsPerPage: 10,
    //   totalItems: 50,
    // };
  }
  resetForm() {
    this.investmentRecService.investmentRecFormData = new InvestmentInit();
    this.investmentRecService.investmentRecCommentFormData = new InvestmentRecComment();
    this.investmentRecService.investmentMedicineProdFormData = new InvestmentMedicineProd();
    this.investmentRecService.investmentDetailFormData = new InvestmentRec();
    this.investmentRecService.investmentTargetedProdFormData = new InvestmentTargetedProd();
    this.investmentRecService.investmentTargetedGroupFormData = new InvestmentTargetedGroup();
    this.investmentRecService.investmentDoctorFormData = new InvestmentDoctor();
    this.investmentRecService.investmentInstitutionFormData = new InvestmentInstitution();
    this.investmentRecService.investmentCampaignFormData = new InvestmentCampaign();
    this.investmentRecService.investmentBcdsFormData = new InvestmentBcds();
    this.investmentRecService.investmentSocietyFormData = new InvestmentSociety();
    this.investmentTargetedProds = [];
    this.investmentMedicineProd = [];
    this.investmentTargetedGroups = [];
    this.investmentDetailsOld = [];
    this.lastFiveInvestmentDetail = [];
    this.isAdmin = false;
    this.isValid = false;
    this.isInvOther = false;
    // this.configs = {
    //   currentPage: 1,
    //   itemsPerPage: 10,
    //   totalItems: 50,
    // };
  }


  // onPageChanged(event: any) {
  //   const params = this.investmentRecService.getGenParams();
  //   if (params.pageIndex !== event) {
  //     params.pageIndex = event;
  //     this.investmentRecService.setGenParams(params);
  //     this.getInvestmentRecommendedPgChange();
  //   }
  // }
  // getInvestmentRecommendedPgChange() {
  //   const params = this.investmentRecService.getGenParams();
  //   this.SpinnerService.show();
  //   this.investmentRecService.getInvestmentRecommended(parseInt(this.empId), this.sbu, this.userRole).subscribe(response => {
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

