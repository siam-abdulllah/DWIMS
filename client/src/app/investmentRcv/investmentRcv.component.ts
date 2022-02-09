import {
  InvestmentRcv, IInvestmentRcv, InvestmentInit, IInvestmentInit,
  InvestmentTargetedProd, IInvestmentTargetedProd, InvestmentTargetedGroup, IInvestmentTargetedGroup, IInvestmentRcvComment, InvestmentRcvComment
} from '../shared/models/investmentRcv';
import { InvestmentDoctor, IInvestmentDoctor, InvestmentInstitution, IInvestmentInstitution, InvestmentCampaign, IInvestmentCampaign } from '../shared/models/investmentRcv';
import { InvestmentBcds, IInvestmentBcds, InvestmentSociety, IInvestmentSociety } from '../shared/models/investmentRcv';
import { SubCampaign, ISubCampaign } from '../shared/models/subCampaign';
import { Donation, IDonation } from '../shared/models/donation';
import { Doctor, IDoctor } from '../shared/models/docotor';
import { Institution, IInstitution } from '../shared/models/institution';
import { GenericParams } from '../shared/models/genericParams';
import { Component, ElementRef, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { InvestmentRcvService } from '../_services/investmentRcv.service';
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
import { IInvestmentDetailOld, ILastFiveInvestmentDetail } from '../shared/models/investment';
import { NgxSpinnerService } from 'ngx-spinner';
import { IBudgetCeiling } from '../shared/models/budgetCeiling';

@Component({
  selector: 'app-investmentRcv',
  templateUrl: './investmentRcv.component.html',
  styles: [
  ],
  providers: [DatePipe]
})
export class InvestmentRcvComponent implements OnInit {
  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  @ViewChild('investmentInitSearchModal', { static: false }) investmentInitSearchModal: TemplateRef<any>;
  @ViewChild('investmentRcvSearchModal', { static: false }) investmentRcvSearchModal: TemplateRef<any>;
  InvestmentInitSearchModalRef: BsModalRef;
  investmentRcvSearchModalRef: BsModalRef;
  investmentDetailsOld: IInvestmentDetailOld[];
  lastFiveInvestmentDetail:ILastFiveInvestmentDetail[];
  investmentRcvs: IInvestmentRcv[];
  investmentTargetedProds: IInvestmentTargetedProd[];
  investmentTargetedGroups: IInvestmentTargetedGroup[];
  investmentDetails: IInvestmentRcv[];
  investmentDoctors: IInvestmentDoctor[];
  isValid: boolean = false;
  isInvOther: boolean = false;
  isBudgetVisible: boolean = false;
  isDonationValid: boolean = false;
  configs: any;
  numberPattern = "^[0-9]+(.[0-9]{1,10})?$";
  bcds: IBcdsInfo[];
  society: ISocietyInfo[];
  markets: IMarket[];
  products: IProduct[];
  budgetCeiling: IBudgetCeiling;
  campaignDtlproducts: IProduct[];
  subCampaigns: ISubCampaign[];
  doctors: IDoctor[];
  institutions: IInstitution[];
  donations: IDonation[];
  searchText = '';
  campaignMsts: ICampaignMst[];
  campaignDtls: ICampaignDtl[];
  campaignDtlProducts: ICampaignDtlProduct[];
  marketGroupMsts: IMarketGroupMst[];
  investmentInits: IInvestmentInit[];
  donationToVal: string;
  totalCount = 0;
  bsConfig: Partial<BsDatepickerConfig>;
  bsValue: Date = new Date();
  empId: string;
  sbu: string;
  config = {
    keyboard: false,
    class: 'modal-lg',
    ignoreBackdropClick: true
  };
  userRole: any;

  constructor(private accountService: AccountService, public investmentRcvService: InvestmentRcvService, private router: Router,
    private toastr: ToastrService, private modalService: BsModalService, private datePipe: DatePipe, private SpinnerService: NgxSpinnerService) { }
  ngOnInit() {
    this.resetForm();
    this.getEmployeeId();
    this.getDonation();
    this.bsConfig = Object.assign({}, { containerClass: 'theme-blue' }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();
  }
  onSubmit(form: NgForm) {
    if (this.investmentRcvService.investmentRcvCommentFormData.id == null || this.investmentRcvService.investmentRcvCommentFormData.id == undefined || this.investmentRcvService.investmentRcvCommentFormData.id == 0)
      this.insertinvestmentRcv();
    //this.insertInvestmentDetails();
    else
      this.updateinvestmentRcv();
  } getDonation() {
    this.investmentRcvService.getDonations().subscribe(response => {
      this.donations = response as IDonation[];
    }, error => {
      console.log(error);
    });
  }
  openInvestmentInitSearchModal(template: TemplateRef<any>) {
    this.InvestmentInitSearchModalRef = this.modalService.show(template, this.config);
  }
  openinvestmentRcvSearchModal(template: TemplateRef<any>) {
    this.investmentRcvSearchModalRef = this.modalService.show(template, this.config);
 
  }
  selectInvestmentInit(selectedAprord: IInvestmentInit) {
    this.resetForm();
    this.investmentRcvService.investmentRcvFormData = Object.assign({}, selectedAprord);
    this.investmentRcvService.investmentDetailFormData.investmentInitId = selectedAprord.id;
    this.investmentRcvService.investmentRcvCommentFormData.investmentInitId = selectedAprord.id;
    this.isDonationValid = true;
    if (this.investmentRcvService.investmentRcvFormData.donationTo == "Doctor") {
      this.getInvestmentDoctor();
    }
    else if (this.investmentRcvService.investmentRcvFormData.donationTo == "Institution") {
      this.getInvestmentInstitution();
    }
    else if (this.investmentRcvService.investmentRcvFormData.donationTo == "Campaign") {
      this.getInvestmentCampaign();
    }
    else if (this.investmentRcvService.investmentRcvFormData.donationTo == "Bcds") {
      this.getInvestmentBcds();
    }
    else if (this.investmentRcvService.investmentRcvFormData.donationTo == "Society") {
      this.getInvestmentSociety();
    }
    this.getInvestmentDetails();
    this.getInvestmentTargetedProd();
    this.getInvestmentTargetedGroup();
    if (this.sbu == this.investmentRcvService.investmentRcvFormData.sbu) {
      this.isInvOther = false;
      this.isValid = true;
    }
    else {
      this.isInvOther = true;
      this.isValid = false;
    }
    //this.getBudget();
    this.InvestmentInitSearchModalRef.hide()
  }
  selectinvestmentRcv(selectedAprord: IInvestmentInit) {
    this.investmentRcvService.investmentRcvFormData = Object.assign({}, selectedAprord);
    this.investmentRcvService.investmentDetailFormData.investmentInitId = selectedAprord.id;
    this.investmentRcvService.investmentRcvCommentFormData.investmentInitId = selectedAprord.id;
    this.isDonationValid = true;
    if (this.investmentRcvService.investmentRcvFormData.donationTo == "Doctor") {
      this.getInvestmentDoctor();
    }
    else if (this.investmentRcvService.investmentRcvFormData.donationTo == "Institution") {
      this.getInvestmentInstitution();
    }
    else if (this.investmentRcvService.investmentRcvFormData.donationTo == "Campaign") {
      this.getInvestmentCampaign();
    }
    else if (this.investmentRcvService.investmentRcvFormData.donationTo == "Bcds") {
      this.getInvestmentBcds();
    }
    else if (this.investmentRcvService.investmentRcvFormData.donationTo == "Society") {
      this.getInvestmentSociety();
    }
    this.getinvestmentRcvDetails();
    this.getinvestmentRcvProducts();
    this.getinvestmentRcvComment();
  
    if (this.sbu == this.investmentRcvService.investmentRcvFormData.sbu) {
      this.isInvOther = false;
      this.isValid = true;
    }
    else {
      this.isInvOther = true;
      this.isValid = false;
    }
    //this.getBudget();
    this.closeSearchModalInvestRcv()
  }
  getLastFiveInvestment(marketCode: string, toDayDate: string) {
    if (this.investmentRcvService.investmentRcvFormData.donationTo == "Doctor") {
      this.investmentRcvService.getLastFiveInvestmentForDoc(this.investmentRcvService.investmentRcvFormData.donationId, this.investmentRcvService.investmentDoctorFormData.doctorId, marketCode, toDayDate).subscribe(
        (response) => {
          this.lastFiveInvestmentDetail = response as ILastFiveInvestmentDetail[];
        },
        (error) => {
          console.log(error);
        }
      );
    }
    else if (this.investmentRcvService.investmentRcvFormData.donationTo == "Institution") {
      this.investmentRcvService.getLastFiveInvestmentForInstitute(this.investmentRcvService.investmentRcvFormData.donationId, this.investmentRcvService.investmentInstitutionFormData.institutionId, marketCode, toDayDate).subscribe(
        (response) => {
          this.lastFiveInvestmentDetail = response as ILastFiveInvestmentDetail[];
        },
        (error) => {
          console.log(error);
        }
      );
    }
    else if (this.investmentRcvService.investmentRcvFormData.donationTo == "Campaign") {
      this.investmentRcvService.getLastFiveInvestmentForCampaign(this.investmentRcvService.investmentRcvFormData.donationId, this.investmentRcvService.investmentCampaignFormData.campaignMstId, marketCode, toDayDate).subscribe(
        (response) => {
          this.lastFiveInvestmentDetail = response as ILastFiveInvestmentDetail[];
        },
        (error) => {
          console.log(error);
        }
      );
    }
    else if (this.investmentRcvService.investmentRcvFormData.donationTo == "Bcds") {
      this.investmentRcvService.getLastFiveInvestmentForBcds(this.investmentRcvService.investmentRcvFormData.donationId, this.investmentRcvService.investmentBcdsFormData.bcdsId, marketCode, toDayDate).subscribe(
        (response) => {
          this.lastFiveInvestmentDetail = response as ILastFiveInvestmentDetail[];
        },
        (error) => {
          console.log(error);
        }
      );
    }
    else if (this.investmentRcvService.investmentRcvFormData.donationTo == "Society") {
      this.investmentRcvService.getLastFiveInvestmentForSociety(this.investmentRcvService.investmentRcvFormData.donationId, this.investmentRcvService.investmentSocietyFormData.societyId, marketCode, toDayDate).subscribe(
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
  //   this.investmentRcvService.getCampaignMsts().subscribe(response => {
  //     this.campaignMsts = response as ICampaignMst[];
  //   }, error => {
  //     console.log(error);
  //   });
  // }
  getInvestmentInit() {
    const params = this.investmentRcvService.getGenParams();
    this.SpinnerService.show();
    this.investmentRcvService.getInvestmentInit(parseInt(this.empId), this.sbu).subscribe(response => {
      this.SpinnerService.hide();
      this.investmentInits = response.data;
      this.totalCount = response.count;
      this.configs = {
        currentPage: params.pageIndex,
        itemsPerPage: params.pageSize,
        totalItems: this.totalCount,
      };
    
      if (this.investmentInits.length > 0) {
        if (params.pageIndex == 1) {
          this.openInvestmentInitSearchModal(this.investmentInitSearchModal);
        }
      }
      else {
        this.toastr.warning('No Data Found');
      }
    }, error => {
      this.SpinnerService.hide();
      console.log(error);
    });
  }
  getInvestmentApproved() {
    this.SpinnerService.show();
    this.investmentRcvService.getInvestmentApproved(parseInt(this.empId), this.sbu).subscribe(response => {
      this.SpinnerService.hide();
      this.investmentInits = response.data;
      if (this.investmentInits.length > 0) {
        this.openinvestmentRcvSearchModal(this.investmentRcvSearchModal);
      }
      else {
        this.toastr.warning('No Data Found');
      }
    }, error => {
      this.SpinnerService.hide();
      console.log(error);
    });
  }

  resetSearch(){
    this.searchText = '';
}

  getInvestmentCampaign() {
    this.investmentRcvService.getInvestmentCampaigns(this.investmentRcvService.investmentRcvFormData.id).subscribe(response => {
      var data = response[0] as IInvestmentCampaign;
      if (data !== undefined) {
        this.investmentRcvService.investmentCampaignFormData = data;
        this.investmentRcvService.investmentCampaignFormData.campaignMstId = data.campaignDtl.mstId;
        this.investmentRcvService.investmentCampaignFormData.subCampaignName = data.campaignDtl.subCampaignName;
        this.investmentRcvService.investmentCampaignFormData.doctorName = data.doctorInfo.doctorName;
        this.investmentRcvService.investmentCampaignFormData.institutionName = data.institutionInfo.institutionName;
        this.investmentRcvService.investmentCampaignFormData.subCampStartDate = new DatePipe('en-US').transform(data.campaignDtl.subCampStartDate, 'dd/MM/yyyy');
        this.investmentRcvService.investmentCampaignFormData.subCampEndDate = new DatePipe('en-US').transform(data.campaignDtl.subCampEndDate, 'dd/MM/yyyy')
        this.investmentRcvService.getCampaignMsts(this.investmentRcvService.investmentRcvFormData.employeeId).subscribe(response => {
          this.campaignMsts = response as ICampaignMst[];
          for (let i = 0; i < this.campaignMsts.length; i++) {
            if (this.campaignMsts[i].id == this.investmentRcvService.investmentCampaignFormData.campaignDtl.mstId) {
              this.investmentRcvService.investmentCampaignFormData.campaignName = this.campaignMsts[i].campaignName;
            }
          }
          this.investmentRcvService.getCampaignDtls(data.campaignDtl.mstId).subscribe(response => {
            this.campaignDtls = response as ICampaignDtl[];
            for (let i = 0; i < this.campaignDtls.length; i++) {
              if (this.campaignDtls[i].id == data.campaignDtl.id) {
                this.investmentRcvService.investmentCampaignFormData.subCampaignName = this.campaignDtls[i].subCampaign.subCampaignName;
              }
            }
          }, error => {
            console.log(error);
          });
        }, error => {
          console.log(error);
        });


        this.investmentRcvService.getCampaignDtlProducts(data.campaignDtl.id).subscribe(response => {
          this.campaignDtlProducts = response as ICampaignDtlProduct[];
        }, error => {
          console.log(error);
        });
      }
      else {
        this.toastr.warning('No Data Found', 'Investment ');
      }
    }, error => {
      console.log(error);
    });

  }
  getInvestmentBcds() {
    this.investmentRcvService.getInvestmentBcds(this.investmentRcvService.investmentRcvFormData.id).subscribe(response => {
      var data = response[0] as IInvestmentBcds;
      if (data !== undefined) {
        this.investmentRcvService.investmentBcdsFormData = data;
        this.investmentRcvService.investmentBcdsFormData.responsibleDoctorName = data.doctorInfo.doctorName;
        this.investmentRcvService.investmentBcdsFormData.bcdsName = data.bcds.bcdsName;
        this.investmentRcvService.investmentBcdsFormData.bcdsAddress = data.bcds.bcdsAddress;
        this.investmentRcvService.investmentBcdsFormData.noOfMember = data.bcds.noOfMember;
      }
      else {
        this.toastr.warning('No Data Found', 'Investment ');
      }

    }, error => {
      console.log(error);
    });
  }
  getInvestmentSociety() {
    this.investmentRcvService.getInvestmentSociety(this.investmentRcvService.investmentRcvFormData.id).subscribe(response => {
      var data = response[0] as IInvestmentSociety;
      if (data !== undefined) {
        this.investmentRcvService.investmentSocietyFormData = data;
        this.investmentRcvService.investmentSocietyFormData.responsibleDoctorName = data.doctorInfo.doctorName;
        this.investmentRcvService.investmentSocietyFormData.societyName = data.society.societyName;
        this.investmentRcvService.investmentSocietyFormData.noOfMember = data.society.noOfMember;
        this.investmentRcvService.investmentSocietyFormData.societyAddress = data.society.societyAddress;
      }
      else {
        this.toastr.warning('No Data Found', 'Investment ');
      }
    }, error => {
      console.log(error);
    });
  }
  getInvestmentInstitution() {
    this.investmentRcvService.getInvestmentInstitutions(this.investmentRcvService.investmentRcvFormData.id).subscribe(response => {
      var data = response[0] as IInvestmentInstitution;
      if (data !== undefined) {
        this.investmentRcvService.investmentInstitutionFormData = data;
        this.investmentRcvService.investmentInstitutionFormData.responsibleDoctorName = data.doctorInfo.doctorName;
        this.investmentRcvService.investmentInstitutionFormData.institutionName = data.institutionInfo.institutionName;
        this.investmentRcvService.investmentInstitutionFormData.address = data.institutionInfo.address;
        this.investmentRcvService.investmentInstitutionFormData.institutionType = data.institutionInfo.institutionType;
      }
      else {
        this.toastr.warning('No Data Found', 'Investment ');
      }

    }, error => {
      console.log(error);
    });
  }
  getInvestmentDoctor() {
    this.investmentRcvService.getInvestmentDoctors(this.investmentRcvService.investmentRcvFormData.id).subscribe(response => {
      var data = response[0] as IInvestmentDoctor;
      if (data !== undefined) {
        this.investmentRcvService.investmentDoctorFormData = data;
        this.investmentRcvService.investmentDoctorFormData.doctorName = data.doctorInfo.doctorName;
        this.investmentRcvService.investmentDoctorFormData.degree = data.doctorInfo.degree;
        this.investmentRcvService.investmentDoctorFormData.designation = data.doctorInfo.designation;
        this.investmentRcvService.investmentDoctorFormData.institutionName = data.institutionInfo.institutionName;
        this.investmentRcvService.investmentDoctorFormData.address = data.institutionInfo.address;
      }
      else {
        this.toastr.warning('No Data Found', 'Investment ');
      }
    }, error => {
      console.log(error);
    });
  }
  getinvestmentRcvComment() {
    this.investmentRcvService.getInvestmentRcvComment(this.investmentRcvService.investmentRcvFormData.id).subscribe(response => {
      var data = response[0] as IInvestmentRcvComment;
      if (data !== undefined) {
        debugger;
        this.investmentRcvService.investmentRcvCommentFormData = data;
      }
      else {
        this.toastr.warning('No Data Found', 'Investment ');
      }

    }, error => {
      console.log(error);
    });
  }
  getInvestmentDetails() {
    this.investmentRcvService.getInvestmentDetails(this.investmentRcvService.investmentRcvFormData.id).subscribe(response => {
      var data = response[0] as IInvestmentRcv;
      if (data !== undefined) {
        this.investmentRcvService.investmentDetailFormData = data;
        this.investmentRcvService.investmentDetailFormData.id = 0;
        this.investmentRcvService.investmentDetailFormData.fromDate = new Date(data.fromDate);
        this.investmentRcvService.investmentDetailFormData.toDate = new Date(data.toDate);
        let convertedDate = this.datePipe.transform(data.fromDate, 'ddMMyyyy');
        this.getLastFiveInvestment(this.investmentRcvService.investmentRcvFormData.marketCode, convertedDate);
      } else {
        this.toastr.warning('No Data Found', 'Investment');
      }
    }, error => {
      console.log(error);
    });
  }
  getInvestmentTargetedProd() {
    this.investmentRcvService.getInvestmentTargetedProds(this.investmentRcvService.investmentRcvFormData.id, this.sbu).subscribe(response => {
      var data = response as IInvestmentTargetedProd[];
      if (data !== undefined) {
        this.investmentTargetedProds = data;

      }
      else {
        this.toastr.warning('No Data Found', 'Investment ');
      }

    }, error => {
      console.log(error);
    });
  }
  getinvestmentRcvDetails() {
    this.investmentRcvService.getInvestmentRcvDetails(this.investmentRcvService.investmentRcvFormData.id, parseInt(this.empId)).subscribe(response => {
      var data = response[0] as IInvestmentRcv;
      if (data !== undefined) {
        this.investmentRcvService.investmentDetailFormData = data;
        this.investmentRcvService.investmentDetailFormData.id = 0;
        this.investmentRcvService.investmentDetailFormData.fromDate = new Date(data.fromDate);
        this.investmentRcvService.investmentDetailFormData.toDate = new Date(data.toDate);
        let convertedDate = this.datePipe.transform(data.fromDate, 'ddMMyyyy');
        this.getLastFiveInvestment(this.investmentRcvService.investmentRcvFormData.marketCode, convertedDate);
      } else {
        this.getInvestmentDetails();
      }
    }, error => {
      console.log(error);
    });
  }
  getinvestmentRcvProducts() {
    this.investmentRcvService.getInvestmentRcvProducts(this.investmentRcvService.investmentRcvFormData.id, this.sbu).subscribe(response => {
      var data = response as IInvestmentTargetedProd[];
      if (data !== undefined) {
        this.investmentTargetedProds = data;
      }
      else {
        this.toastr.warning('No Data Found', 'Investment ');
      }
    }, error => {
      console.log(error);
    });
  }
  getInvestmentTargetedGroup() {
    this.investmentRcvService.getInvestmentTargetedGroups(this.investmentRcvService.investmentRcvFormData.id, parseInt(this.empId)).subscribe(response => {
      var data = response as IInvestmentTargetedGroup[];
      if (data !== undefined) {
        this.investmentTargetedGroups = data;
      }
      else {
        this.toastr.warning('No Data Found', 'Investment ');
      }
    }, error => {
      console.log(error);
    });
  }



  changeDateInDetail() {
    if (this.investmentRcvService.investmentDetailFormData.fromDate == null || this.investmentRcvService.investmentDetailFormData.fromDate == undefined) {
      return false;
    }
    if (this.investmentRcvService.investmentDetailFormData.toDate == null || this.investmentRcvService.investmentDetailFormData.toDate == undefined) {
      return false;
    }
    let dateFrom = this.investmentRcvService.investmentDetailFormData.fromDate;
    let dateTo = this.investmentRcvService.investmentDetailFormData.toDate;
    this.investmentRcvService.investmentDetailFormData.totalMonth = dateTo.getMonth() - dateFrom.getMonth() + (12 * (dateTo.getFullYear() - dateFrom.getFullYear()));
    this.investmentRcvService.investmentDetailFormData.totalMonth = this.investmentRcvService.investmentDetailFormData.totalMonth + 1;
  }

  dateCompare(form: NgForm) {
    if (this.investmentRcvService.investmentDetailFormData.fromDate != null && this.investmentRcvService.investmentDetailFormData.toDate != null) {
      if (this.investmentRcvService.investmentDetailFormData.toDate > this.investmentRcvService.investmentDetailFormData.fromDate) {
      }
      else {
        form.controls.fromDate.setValue(null);
        form.controls.toDate.setValue(null);
        this.toastr.error('Select Appropriate Date Range', 'Error')
      }
    }
  }


  getEmployeeId() {
    this.empId = this.accountService.getEmployeeId();
    this.userRole = this.accountService.getUserRole();
    this.investmentRcvService.investmentRcvCommentFormData.employeeId = parseInt(this.empId);
    this.getEmployeeSbu();
  }
  getEmployeeSbu() {
    this.accountService.getEmployeeSbu(this.investmentRcvService.investmentRcvCommentFormData.employeeId).subscribe(
      (response) => {
        this.sbu = response.sbu;
        this.getProduct();
      },
      (error) => {
        console.log(error);
      });
  }
  getProduct() {
    this.investmentRcvService.getProduct(this.sbu).subscribe(response => {
      this.products = response as IProduct[];
    }, error => {
      console.log(error);
    });
  }
  // getBudget() {
  //   this.investmentRcvService.getBudget(this.sbu, parseInt(this.empId), this.investmentRcvService.investmentRcvFormData.donationId).subscribe(response => {
  //     this.budgetCeiling = response[0] as IBudgetCeiling;
  //     this.isBudgetVisible = true;
  //   }, error => {
  //     console.log(error);
  //   });
  // }
  insertinvestmentRcv() {

    const investRecvhDto: IInvestmentRcvInsert = {
      id: 0,
      investmentInitId: this.investmentRcvService.investmentRcvCommentFormData.investmentInitId,
      chequeTitle: this.investmentRcvService.investmentDetailFormData.chequeTitle,
      paymentMethod: this.investmentRcvService.investmentDetailFormData.paymentMethod,
      commitmentAllSBU: this.investmentRcvService.investmentDetailFormData.commitmentAllSBU,
      commitmentOwnSBU: this.investmentRcvService.investmentDetailFormData.commitmentOwnSBU,
      totalMonth: this.investmentRcvService.investmentDetailFormData.totalMonth,
      proposedAmount: this.investmentRcvService.investmentDetailFormData.proposedAmount,
      purpose: this.investmentRcvService.investmentDetailFormData.purpose,
      marketCode: this.investmentRcvService.investmentDetailFormData.marketCode,
      sbu: this.investmentRcvService.investmentDetailFormData.sbu,
      fromDate: this.investmentRcvService.investmentDetailFormData.fromDate,
      toDate: this.investmentRcvService.investmentDetailFormData.toDate,
      receiveStatus: this.investmentRcvService.investmentRcvCommentFormData.receiveStatus,
      comments: this.investmentRcvService.investmentRcvCommentFormData.comments,
      employeeId: parseInt(this.empId),
      sbuName: '',
      zoneCode: '',
      zoneName: '',
      regionCode: '',
      regionName: '',
      territoryCode: '',
      territoryName: '',
      marketName: '',
      marketGroupCode: '',
      marketGroupName: '',
      priority: 0
    };

    this.investmentRcvService.investmentRcvCommentFormData.employeeId = parseInt(this.empId);
    this.SpinnerService.show();
    this.investmentRcvService.insertInvestmentRcv(investRecvhDto).subscribe(
      res => {
        this.investmentRcvService.investmentRcvCommentFormData = res as IInvestmentRcvComment;
        this.isValid = true;
        //this.insertInvestmentTargetedProd();
        this.SpinnerService.hide();
        if (this.investmentRcvService.investmentRcvFormData.id > 0) 
        { 
        this.toastr.success('Save Successfully', 'Investment')
        }
      },
      err => { console.log(err); }
    );
  }
  updateinvestmentRcv() {

    const investRecvhDto: IInvestmentRcvInsert = {
      id: this.investmentRcvService.investmentRcvCommentFormData.id,
      investmentInitId: this.investmentRcvService.investmentRcvCommentFormData.investmentInitId,
      chequeTitle: this.investmentRcvService.investmentDetailFormData.chequeTitle,
      paymentMethod: this.investmentRcvService.investmentDetailFormData.paymentMethod,
      commitmentAllSBU: this.investmentRcvService.investmentDetailFormData.commitmentAllSBU,
      commitmentOwnSBU: this.investmentRcvService.investmentDetailFormData.commitmentOwnSBU,
      totalMonth: this.investmentRcvService.investmentDetailFormData.totalMonth,
      proposedAmount: this.investmentRcvService.investmentDetailFormData.proposedAmount,
      purpose: this.investmentRcvService.investmentDetailFormData.purpose,
      marketCode: this.investmentRcvService.investmentDetailFormData.marketCode,
      sbu: this.investmentRcvService.investmentDetailFormData.sbu,
      fromDate: this.investmentRcvService.investmentDetailFormData.fromDate,
      toDate: this.investmentRcvService.investmentDetailFormData.toDate,
      receiveStatus: this.investmentRcvService.investmentRcvCommentFormData.receiveStatus,
      comments: this.investmentRcvService.investmentRcvCommentFormData.comments,
      employeeId: parseInt(this.empId),
      sbuName: '',
      zoneCode: '',
      zoneName: '',
      regionCode: '',
      regionName: '',
      territoryCode: '',
      territoryName: '',
      marketName: '',
      marketGroupCode: '',
      marketGroupName: '',
      priority: 0
    };

    this.SpinnerService.show();
    this.investmentRcvService.updateInvestmentRcv(investRecvhDto).subscribe(
      res => {
        this.isValid = true;
        this.investmentRcvService.investmentRcvCommentFormData = res as IInvestmentRcvComment;
        //this.insertInvestmentTargetedProd();
        this.SpinnerService.hide();
        debugger;
        if (this.investmentRcvService.investmentRcvFormData.id > 0) 
        { 
        this.toastr.success('Updated Successfully', 'Investment Received')
        }
      },
      err => { console.log(err); }
    );
  }
  insertInvestmentDetails() {
    if (this.investmentRcvService.investmentRcvFormData.id == null || this.investmentRcvService.investmentRcvFormData.id == undefined || this.investmentRcvService.investmentRcvFormData.id == 0) {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment ');
      return false;
    }
    if (this.investmentRcvService.investmentDetailFormData.proposedAmount == null || this.investmentRcvService.investmentDetailFormData.proposedAmount == undefined || this.investmentRcvService.investmentDetailFormData.proposedAmount == "") {
      this.toastr.warning('Enter Proposed Amount First', 'Investment ');
      return false;
    }
    if (this.investmentRcvService.investmentDetailFormData.purpose == null || this.investmentRcvService.investmentDetailFormData.purpose == undefined || this.investmentRcvService.investmentDetailFormData.purpose == "") {
      this.toastr.warning('Enter Purpose First', 'Investment ');
      return false;
    }
    if (this.investmentRcvService.investmentDetailFormData.fromDate == null || this.investmentRcvService.investmentDetailFormData.fromDate == undefined) {
      this.toastr.warning('Select From Date  First', 'Investment ');
      return false;
    }
    if (this.investmentRcvService.investmentDetailFormData.toDate == null || this.investmentRcvService.investmentDetailFormData.toDate == undefined) {
      this.toastr.warning('Select To Date  First', 'Investment ');
      return false;
    }
    if (this.investmentRcvService.investmentDetailFormData.commitmentAllSBU == null || this.investmentRcvService.investmentDetailFormData.commitmentAllSBU == undefined || this.investmentRcvService.investmentDetailFormData.commitmentAllSBU == "") {
      this.toastr.warning('Enter Commitment All SBU First', 'Investment ');
      return false;
    }
    if (this.investmentRcvService.investmentDetailFormData.commitmentOwnSBU == null || this.investmentRcvService.investmentDetailFormData.commitmentOwnSBU == undefined || this.investmentRcvService.investmentDetailFormData.commitmentOwnSBU == "") {
      this.toastr.warning('Enter Commitment Own SBU First', 'Investment ');
      return false;
    }
    if (this.investmentRcvService.investmentDetailFormData.paymentMethod == null || this.investmentRcvService.investmentDetailFormData.paymentMethod == undefined || this.investmentRcvService.investmentDetailFormData.paymentMethod == "") {
      this.toastr.warning('Select Payment Method First', 'Investment ');
      return false;
    }

    this.investmentRcvService.investmentDetailFormData.investmentInitId = this.investmentRcvService.investmentRcvFormData.id;

    this.SpinnerService.show();
    this.investmentRcvService.insertInvestmentDetail(parseInt(this.empId), this.sbu).subscribe(
      res => {
        var data = res as IInvestmentRcv;
        this.investmentRcvService.investmentDetailFormData = data;
        this.investmentRcvService.investmentDetailFormData.fromDate = new Date(data.fromDate);
        this.investmentRcvService.investmentDetailFormData.toDate = new Date(data.toDate);
        this.isDonationValid = true;
        this.toastr.success('Save successfully', 'Investment');
        this.SpinnerService.hide();
      },
      err => {
        console.log(err);
        this.investmentRcvService.investmentRcvCommentFormData.id = 0;
        this.SpinnerService.hide();
      }
    );
  }

  // insertInvestmentTargetedProd() {
  //   if (this.investmentRcvService.investmentRcvFormData.id == null || this.investmentRcvService.investmentRcvFormData.id == undefined || this.investmentRcvService.investmentRcvFormData.id == 0) {
  //     this.toastr.warning('Insert Investment Initialisation First', 'Investment Product');
  //     return false;
  //   }
  //   if (this.investmentTargetedProds !== undefined) {
  //     for (let i = 0; i < this.investmentTargetedProds.length; i++) {
  //       if (this.investmentTargetedProds[i].productInfo.id == this.investmentRcvService.investmentTargetedProdFormData.productId) {
  //         this.toastr.warning("Product already exist !");
  //         return false;
  //       }
  //     }
  //   }
  //   else {
  //     this.toastr.warning('Select Product First', 'Investment Product');
  //     return false;
  //   }
  //   this.investmentRcvService.investmentTargetedProdFormData.investmentInitId = this.investmentRcvService.investmentRcvFormData.id;
  //   this.SpinnerService.show();
  //   this.investmentRcvService.insertInvestmentTargetedProd(this.investmentTargetedProds).subscribe(
  //     res => {if (this.sbu == this.investmentRcvService.investmentRcvFormData.sbu) 
  //       { 
  //       this.insertInvestmentDetails();
  //       }
  //       this.getInvestmentTargetedProd();
  //       this.getInvestmentTargetedGroup();
  //       this.isDonationValid = true;
  //       this.SpinnerService.hide();
  //       if (this.sbu != this.investmentRcvService.investmentRcvFormData.sbu) 
  //       { 
  //       this.toastr.success('Save successfully', 'Investment Product');
  //       }
  //     },
  //     err => {
  //       console.log(err);
  //       this.SpinnerService.hide();
  //     }
  //   );
  // }
  addInvestmentTargetedProd() {
    if (this.investmentRcvService.investmentTargetedProdFormData.productId == null || this.investmentRcvService.investmentTargetedProdFormData.productId == undefined || this.investmentRcvService.investmentTargetedProdFormData.productId == 0) {
      this.toastr.warning('Select Product First', 'Investment ');
      return false;
    }
    if (this.investmentTargetedProds !== undefined) {
      for (let i = 0; i < this.investmentTargetedProds.length; i++) {
        if (this.investmentTargetedProds[i].productInfo.id == this.investmentRcvService.investmentTargetedProdFormData.productId) {
          this.toastr.warning("Product already exist !");
          return false;
        }

      }
      for (let i = 0; i < this.products.length; i++) {
        if (this.products[i].id == this.investmentRcvService.investmentTargetedProdFormData.productId) {
          let data = new InvestmentTargetedProd();
          data.employeeId = parseInt(this.empId);
          data.investmentInitId = this.investmentRcvService.investmentRcvFormData.id;
          data.productId = this.investmentRcvService.investmentTargetedProdFormData.productId;
          data.productInfo = this.products[i];
          this.investmentTargetedProds.push(data);
          return false;
        }
      }
    }
  }

  editInvestmentTargetedProd(selectedAprord: IInvestmentTargetedProd) {
    this.investmentRcvService.investmentTargetedProdFormData = Object.assign({}, selectedAprord);
  }
  populateForm() {
  }
  resetPage(form: NgForm) {
    window.location.reload();
    form.reset();
    this.investmentRcvService.investmentRcvFormData = new InvestmentInit();
    this.investmentRcvService.investmentRcvCommentFormData = new InvestmentRcvComment();
    this.investmentRcvService.investmentRcvFormData = new InvestmentInit();
    this.investmentRcvService.investmentDetailFormData = new InvestmentRcv();
    this.investmentRcvService.investmentRcvCommentFormData = new InvestmentRcvComment();
    this.investmentRcvService.investmentTargetedProdFormData = new InvestmentTargetedProd();
    this.investmentRcvService.investmentTargetedGroupFormData = new InvestmentTargetedGroup();
    this.investmentRcvService.investmentDoctorFormData = new InvestmentDoctor();
    this.investmentRcvService.investmentInstitutionFormData = new InvestmentInstitution();
    this.investmentRcvService.investmentCampaignFormData = new InvestmentCampaign();
    this.investmentRcvService.investmentBcdsFormData = new InvestmentBcds();
    this.investmentRcvService.investmentSocietyFormData = new InvestmentSociety();
    this.investmentTargetedProds = [];
    this.investmentTargetedGroups = [];
    this.investmentDetailsOld = [];
    this.lastFiveInvestmentDetail = [];
    this.isValid = false;
    this.isBudgetVisible = false;
    this.configs = {
      currentPage: 1,
      itemsPerPage: 20,
      totalItems: 50,
    };
  }
  resetForm() {
    this.investmentRcvService.investmentRcvFormData = new InvestmentInit();
    this.investmentRcvService.investmentRcvCommentFormData = new InvestmentRcvComment();
    this.investmentRcvService.investmentRcvFormData = new InvestmentInit();
    this.investmentRcvService.investmentDetailFormData = new InvestmentRcv();
    this.investmentRcvService.investmentRcvCommentFormData = new InvestmentRcvComment();
    this.investmentRcvService.investmentTargetedProdFormData = new InvestmentTargetedProd();
    this.investmentRcvService.investmentTargetedGroupFormData = new InvestmentTargetedGroup();
    this.investmentRcvService.investmentDoctorFormData = new InvestmentDoctor();
    this.investmentRcvService.investmentInstitutionFormData = new InvestmentInstitution();
    this.investmentRcvService.investmentCampaignFormData = new InvestmentCampaign();
    this.investmentRcvService.investmentBcdsFormData = new InvestmentBcds();
    this.investmentRcvService.investmentSocietyFormData = new InvestmentSociety();
    this.investmentTargetedProds = [];
    this.investmentTargetedGroups = [];
    this.investmentDetailsOld = [];
    this.lastFiveInvestmentDetail = [];
    this.isValid = false;
    this.isBudgetVisible = false;
    this.configs = {
      currentPage: 1,
      itemsPerPage: 20,
      totalItems: 50,
    };
  }

  onPageChanged(event: any) {
    const params = this.investmentRcvService.getGenParams();
    if (params.pageIndex !== event) {
      params.pageIndex = event;
      this.investmentRcvService.setGenParams(params);
      this.getInvestmentInit();
    }
  }
  
  closeSearchModalInvestRcv()
  {
    const params = this.investmentRcvService.getGenParams();
    params.pageIndex = 1;
    this.investmentRcvSearchModalRef.hide()
  }


  removeInvestmentTargetedProd(selectedAprord: IInvestmentTargetedProd) {
    if (this.investmentTargetedProds.find(x => x.productId == selectedAprord.productId)) {
      this.investmentTargetedProds.splice(this.investmentTargetedProds.findIndex(x => x.productId == selectedAprord.productId), 1);
    }
    if (this.investmentRcvService.investmentRcvCommentFormData.id == null || this.investmentRcvService.investmentRcvCommentFormData.id == undefined || this.investmentRcvService.investmentRcvCommentFormData.id == 0) {
      return false;
    }
    this.investmentRcvService.investmentTargetedProdFormData = Object.assign({}, selectedAprord);
    var c = confirm("Are you sure you want to delete that?");
    if (c == true) {
      this.investmentRcvService.removeInvestmentTargetedProd().subscribe(
        res => {
          this.toastr.success(res);
          this.investmentRcvService.investmentTargetedProdFormData = new InvestmentTargetedProd();
          this.getInvestmentTargetedProd();
        },
        err => { console.log(err); }
      );
    }
  }
}



export interface IInvestmentRcvInsert {
  id: number;
  investmentInitId: number;
  employeeId : number;
  sbu: string;
  sbuName: string;
  zoneCode: string;
  zoneName: string;
  regionCode: string;
  regionName: string;
  territoryCode: string;
  territoryName: string;
  marketCode: string;
  marketName: string;
  marketGroupCode: string;
  marketGroupName: string;
  comments: string;
  chequeTitle: string;
  paymentMethod: string;
  commitmentAllSBU: string;
  commitmentOwnSBU: string;
  fromDate: Date;
  toDate: Date;
  totalMonth: number;
  proposedAmount: string;
  priority: number;
  purpose: string;
  receiveStatus: string;
}

export class InvestmentRcvInsert implements IInvestmentRcvInsert {
  id: number;
  investmentInitId: number;
  employeeId : number;
  sbu: string;
  sbuName: string;
  zoneCode: string;
  zoneName: string;
  regionCode: string;
  regionName: string;
  territoryCode: string;
  territoryName: string;
  marketCode: string;
  marketName: string;
  marketGroupCode: string;
  marketGroupName: string;
  comments: string;
  chequeTitle: string;
  paymentMethod: string;
  commitmentAllSBU: string;
  commitmentOwnSBU: string;
  fromDate: Date;
  toDate: Date;
  totalMonth: number;
  proposedAmount: string;
  priority: number;
  purpose: string;
  receiveStatus: string;
}