import {
  InvestmentApr, IInvestmentApr, InvestmentInit, IInvestmentInit,
  InvestmentTargetedProd, IInvestmentTargetedProd, InvestmentTargetedGroup, IInvestmentTargetedGroup, IInvestmentAprComment, InvestmentAprComment
} from '../shared/models/investmentApr';
import { InvestmentDoctor, IInvestmentDoctor, InvestmentInstitution, IInvestmentInstitution, InvestmentCampaign, IInvestmentCampaign } from '../shared/models/investmentApr';
import { InvestmentBcds, IInvestmentBcds, InvestmentSociety, IInvestmentSociety } from '../shared/models/investmentApr';
import { SubCampaign, ISubCampaign } from '../shared/models/subCampaign';
import { Donation, IDonation } from '../shared/models/donation';
import { Doctor, IDoctor } from '../shared/models/docotor';
import { Institution, IInstitution } from '../shared/models/institution';
import { GenericParams } from '../shared/models/genericParams';
import { Component, ElementRef, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { InvestmentAprService } from '../_services/investmentApr.service';
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
  selector: 'app-investmentApr',
  templateUrl: './investmentApr.component.html',
  styles: [
  ],
  providers: [DatePipe]
})
export class InvestmentAprComponent implements OnInit {
  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  @ViewChild('investmentInitSearchModal', { static: false }) investmentInitSearchModal: TemplateRef<any>;
  @ViewChild('investmentAprSearchModal', { static: false }) investmentAprSearchModal: TemplateRef<any>;
  InvestmentInitSearchModalRef: BsModalRef;
  InvestmentAprSearchModalRef: BsModalRef;
  investmentDetailsOld: IInvestmentDetailOld[];
  lastFiveInvestmentDetail:ILastFiveInvestmentDetail[];
  investmentAprs: IInvestmentApr[];
  investmentInits: IInvestmentInit[];
  investmentTargetedProds: IInvestmentTargetedProd[];
  investmentTargetedGroups: IInvestmentTargetedGroup[];
  investmentDetails: IInvestmentApr[];
  investmentDoctors: IInvestmentDoctor[];
  isValid: boolean = false;
  isInvOther: boolean = false;
  isBudgetVisible: boolean = false;
  isDonationValid: boolean = false;
  searchText = '';
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
  campaignMsts: ICampaignMst[];
  campaignDtls: ICampaignDtl[];
  campaignDtlProducts: ICampaignDtlProduct[];
  marketGroupMsts: IMarketGroupMst[];
  donationToVal: string;
  totalCount = 0;
  bsConfig: Partial<BsDatepickerConfig>;
  bsValue: Date = new Date();
  isAdmin: boolean = false;
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
  constructor(private accountService: AccountService, public investmentAprService: InvestmentAprService, private router: Router,
    private toastr: ToastrService, private modalService: BsModalService, private datePipe: DatePipe, private SpinnerService: NgxSpinnerService) { }
  ngOnInit() {
    this.resetForm();
    this.getEmployeeId();
    this.getDonation();
    this.bsConfig = Object.assign({}, { containerClass: 'theme-blue' }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();
  }
  onSubmit(form: NgForm) {
    if (this.investmentAprService.investmentAprCommentFormData.id == null || this.investmentAprService.investmentAprCommentFormData.id == undefined || this.investmentAprService.investmentAprCommentFormData.id == 0)
      this.insertInvestmentApr();
    //this.insertInvestmentDetails();
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
  openInvestmentInitSearchModal(template: TemplateRef<any>) {
    this.InvestmentInitSearchModalRef = this.modalService.show(template, this.config);
  }
  openInvestmentAprSearchModal(template: TemplateRef<any>) {
    this.InvestmentAprSearchModalRef = this.modalService.show(template, this.config);
  }
  selectInvestmentInit(selectedAprord: IInvestmentInit) {
    this.resetForm();
    this.investmentAprService.investmentAprFormData = Object.assign({}, selectedAprord);
    var selectedDonation= this.donations.filter(res=>res.id==selectedAprord.donationId).map(ele=>ele.donationTypeName);
    this.donationName=selectedDonation[0];
    this.investmentAprService.investmentDetailFormData.investmentInitId = selectedAprord.id;
    this.investmentAprService.investmentAprCommentFormData.investmentInitId = selectedAprord.id;
    this.isDonationValid = true;
    this.convertedDate = this.datePipe.transform(selectedAprord.setOn, 'ddMMyyyy');
    if (this.investmentAprService.investmentAprFormData.donationTo == "Doctor") {
      this.getInvestmentDoctor();
    }
    else if (this.investmentAprService.investmentAprFormData.donationTo == "Institution") {
      this.getInvestmentInstitution();
    }
    else if (this.investmentAprService.investmentAprFormData.donationTo == "Campaign") {
      this.getInvestmentCampaign();
    }
    else if (this.investmentAprService.investmentAprFormData.donationTo == "Bcds") {
      this.getInvestmentBcds();
    }
    else if (this.investmentAprService.investmentAprFormData.donationTo == "Society") {
      this.getInvestmentSociety();
    }
    this.getInvestmentDetails();
    this.getInvestmentTargetedProd();
    this.getInvestmentTargetedGroup();
    if (this.sbu == this.investmentAprService.investmentAprFormData.sbu) {
      this.isInvOther = false;
      this.isValid = true;
      this.getBudget();
    }
    else {
      this.isInvOther = true;
      this.isValid = false;
    }
    
    this.InvestmentInitSearchModalRef.hide()
  }
  selectInvestmentApr(selectedAprord: IInvestmentInit) {
    this.investmentAprService.investmentAprFormData = Object.assign({}, selectedAprord);
    var selectedDonation= this.donations.filter(res=>res.id==selectedAprord.donationId).map(ele=>ele.donationTypeName);
    this.donationName=selectedDonation[0];
    this.investmentAprService.investmentDetailFormData.investmentInitId = selectedAprord.id;
    this.investmentAprService.investmentAprCommentFormData.investmentInitId = selectedAprord.id;
    this.isDonationValid = true;
    this.convertedDate = this.datePipe.transform(selectedAprord.setOn, 'ddMMyyyy');
    if (this.investmentAprService.investmentAprFormData.donationTo == "Doctor") {
      this.getInvestmentDoctor();
    }
    else if (this.investmentAprService.investmentAprFormData.donationTo == "Institution") {
      this.getInvestmentInstitution();
    }
    else if (this.investmentAprService.investmentAprFormData.donationTo == "Campaign") {
      this.getInvestmentCampaign();
    }
    else if (this.investmentAprService.investmentAprFormData.donationTo == "Bcds") {
      this.getInvestmentBcds();
    }
    else if (this.investmentAprService.investmentAprFormData.donationTo == "Society") {
      this.getInvestmentSociety();
    }
    this.getInvestmentAprDetails();
    this.getInvestmentAprProducts();
    this.getInvestmentAprComment();
    this.getInvestmentTargetedGroup();
    if (this.sbu == this.investmentAprService.investmentAprFormData.sbu) {
      this.isInvOther = false;
      this.isValid = true;
      this.getBudget();
    }
    else {
      this.isInvOther = true;
      this.isValid = false;
    }
   
    this.InvestmentAprSearchModalRef.hide()
  }
  getLastFiveInvestment(marketCode: string, toDayDate: string) {
    if (this.investmentAprService.investmentAprFormData.donationTo == "Doctor") {
      this.investmentAprService.getLastFiveInvestmentForDoc(this.investmentAprService.investmentAprFormData.donationId, this.investmentAprService.investmentDoctorFormData.doctorId, marketCode, toDayDate).then(
        (response) => {
          this.lastFiveInvestmentDetail = response as ILastFiveInvestmentDetail[];
        },
        (error) => {
          console.log(error);
        }
      );
    }
    else if (this.investmentAprService.investmentAprFormData.donationTo == "Institution") {
      this.investmentAprService.getLastFiveInvestmentForInstitute(this.investmentAprService.investmentAprFormData.donationId, this.investmentAprService.investmentInstitutionFormData.institutionId, marketCode, toDayDate).subscribe(
        (response) => {
          this.lastFiveInvestmentDetail = response as ILastFiveInvestmentDetail[];
        },
        (error) => {
          console.log(error);
        }
      );
    }
    else if (this.investmentAprService.investmentAprFormData.donationTo == "Campaign") {
      this.investmentAprService.getLastFiveInvestmentForCampaign(this.investmentAprService.investmentAprFormData.donationId, this.investmentAprService.investmentCampaignFormData.campaignMstId, marketCode, toDayDate).subscribe(
        (response) => {
          this.lastFiveInvestmentDetail = response as ILastFiveInvestmentDetail[];
        },
        (error) => {
          console.log(error);
        }
      );
    }
    else if (this.investmentAprService.investmentAprFormData.donationTo == "Bcds") {
      this.investmentAprService.getLastFiveInvestmentForBcds(this.investmentAprService.investmentAprFormData.donationId, this.investmentAprService.investmentBcdsFormData.bcdsId, marketCode, toDayDate).subscribe(
        (response) => {
          this.lastFiveInvestmentDetail = response as ILastFiveInvestmentDetail[];
        },
        (error) => {
          console.log(error);
        }
      );
    }
    else if (this.investmentAprService.investmentAprFormData.donationTo == "Society") {
      this.investmentAprService.getLastFiveInvestmentForSociety(this.investmentAprService.investmentAprFormData.donationId, this.investmentAprService.investmentSocietyFormData.societyId, marketCode, toDayDate).subscribe(
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
    this.investmentAprService.getCampaignMsts().subscribe(response => {
      this.campaignMsts = response as ICampaignMst[];
    }, error => {
      console.log(error);
    });
  }
  getInvestmentInit() {
    this.SpinnerService.show();
    this.investmentAprService.getInvestmentInit(parseInt(this.empId), this.sbu).subscribe(response => {
      this.SpinnerService.hide();
      this.investmentInits = response.data;
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
  getInvestmentApproved() {
    const params = this.investmentAprService.getGenParams();
    this.SpinnerService.show();
    this.investmentAprService.getInvestmentApproved(parseInt(this.empId), this.sbu,this.userRole).subscribe(response => {
      this.SpinnerService.hide();
      this.investmentInits = response.data;
      this.totalCount = response.count;
      debugger;
      this.configs = {
        currentPage: params.pageIndex,
        itemsPerPage: params.pageSize,
        totalItems: this.totalCount,
      };
      if (this.investmentInits.length > 0) {
        this.openInvestmentAprSearchModal(this.investmentAprSearchModal);
      }
      else {
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
        this.investmentAprService.getCampaignMsts().subscribe(response => {
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
      else {
        this.toastr.warning('No Data Found', 'Investment ');
      }
    }, error => {
      console.log(error);
    });

  }
  getInvestmentBcds() {
    this.investmentAprService.getInvestmentBcds(this.investmentAprService.investmentAprFormData.id).subscribe(response => {
      var data = response[0] as IInvestmentBcds;
      if (data !== undefined) {
        this.investmentAprService.investmentBcdsFormData = data;
        this.investmentAprService.investmentBcdsFormData.responsibleDoctorName = data.doctorInfo.doctorName;
        this.investmentAprService.investmentBcdsFormData.bcdsName = data.bcds.bcdsName;
        this.investmentAprService.investmentBcdsFormData.bcdsAddress = data.bcds.bcdsAddress;
        this.investmentAprService.investmentBcdsFormData.noOfMember = data.bcds.noOfMember;
      }
      else {
        this.toastr.warning('No Data Found', 'Investment ');
      }

    }, error => {
      console.log(error);
    });
  }
  getInvestmentSociety() {
    this.investmentAprService.getInvestmentSociety(this.investmentAprService.investmentAprFormData.id).subscribe(response => {
      var data = response[0] as IInvestmentSociety;
      if (data !== undefined) {
        this.investmentAprService.investmentSocietyFormData = data;
        this.investmentAprService.investmentSocietyFormData.responsibleDoctorName = data.doctorInfo.doctorName;
        this.investmentAprService.investmentSocietyFormData.societyName = data.society.societyName;
        this.investmentAprService.investmentSocietyFormData.noOfMember = data.society.noOfMember;
        this.investmentAprService.investmentSocietyFormData.societyAddress = data.society.societyAddress;
      }
      else {
        this.toastr.warning('No Data Found', 'Investment ');
      }
    }, error => {
      console.log(error);
    });
  }
  getInvestmentInstitution() {
    this.investmentAprService.getInvestmentInstitutions(this.investmentAprService.investmentAprFormData.id).subscribe(response => {
      var data = response[0] as IInvestmentInstitution;
      if (data !== undefined) {
        this.investmentAprService.investmentInstitutionFormData = data;
        this.investmentAprService.investmentInstitutionFormData.responsibleDoctorName = data.doctorInfo.doctorName;
        this.investmentAprService.investmentInstitutionFormData.institutionName = data.institutionInfo.institutionName;
        this.investmentAprService.investmentInstitutionFormData.address = data.institutionInfo.address;
        this.investmentAprService.investmentInstitutionFormData.institutionType = data.institutionInfo.institutionType;
      }
      else {
        this.toastr.warning('No Data Found', 'Investment ');
      }

    }, error => {
      console.log(error);
    });
  }
  getInvestmentDoctor() {
    this.investmentAprService.getInvestmentDoctors(this.investmentAprService.investmentAprFormData.id).then(response => {
      var data = response[0] as IInvestmentDoctor;
      if (data !== undefined) {
        this.investmentAprService.investmentDoctorFormData = data;
        this.investmentAprService.investmentDoctorFormData.doctorName = data.doctorInfo.doctorName;
        this.investmentAprService.investmentDoctorFormData.degree = data.doctorInfo.degree;
        this.investmentAprService.investmentDoctorFormData.designation = data.doctorInfo.designation;
        this.investmentAprService.investmentDoctorFormData.institutionName = data.institutionInfo.institutionName;
        this.investmentAprService.investmentDoctorFormData.address = data.institutionInfo.address;
      }
      else {
        this.toastr.warning('No Data Found', 'Investment ');
      }
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
      else {
        this.toastr.warning('No Data Found', 'Investment ');
      }

    }, error => {
      console.log(error);
    });
  }
  getInvestmentDetails() {
    this.investmentAprService.getInvestmentDetails(this.investmentAprService.investmentAprFormData.id).subscribe(response => {
      var data = response[0] as IInvestmentApr;
      if (data !== undefined) {
        this.investmentAprService.investmentDetailFormData = data;
        this.investmentAprService.investmentDetailFormData.id = 0;
        this.investmentAprService.investmentDetailFormData.fromDate = new Date(data.fromDate);
        this.investmentAprService.investmentDetailFormData.toDate = new Date(data.toDate);
        //let convertedDate = this.datePipe.transform(data.fromDate, 'ddMMyyyy');
        this.getLastFiveInvestment(this.investmentAprService.investmentAprFormData.marketCode, this.convertedDate);
      } else {
        this.toastr.warning('No Data Found', 'Investment');
      }
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
      else {
        this.toastr.warning('No Data Found', 'Investment ');
      }

    }, error => {
      console.log(error);
    });
  }
  getInvestmentAprDetails() {
    this.investmentAprService.getInvestmentAprDetails(this.investmentAprService.investmentAprFormData.id, parseInt(this.empId)).subscribe(response => {
      var data = response[0] as IInvestmentApr;
      if (data !== undefined) {
        this.investmentAprService.investmentDetailFormData = data;
        this.investmentAprService.investmentDetailFormData.id = 0;
        this.investmentAprService.investmentDetailFormData.fromDate = new Date(data.fromDate);
        this.investmentAprService.investmentDetailFormData.toDate = new Date(data.toDate);
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
      else {
        this.toastr.warning('No Data Found', 'Investment ');
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
      else {
        this.toastr.warning('No Data Found', 'Investment ');
      }
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


  getEmployeeId() {
    this.empId = this.accountService.getEmployeeId();
    this.userRole = this.accountService.getUserRole();
    if(this.userRole=='Administrator')
    {
      this.isAdmin=true;
    }
    else{
      this.isAdmin=false;
    }
    this.investmentAprService.investmentAprCommentFormData.employeeId = parseInt(this.empId);
    this.getEmployeeSbu();
  }
  getEmployeeSbu() {
    this.accountService.getEmployeeSbu(this.investmentAprService.investmentAprCommentFormData.employeeId).subscribe(
      (response) => {
        this.sbu = response.sbu;
        this.getProduct();
      },
      (error) => {
        console.log(error);
      });
  }
  getProduct() {
    this.investmentAprService.getProduct(this.sbu).subscribe(response => {
      this.products = response as IProduct[];
    }, error => {
      console.log(error);
    });
  }
  getBudget() {
    this.investmentAprService.getBudget(this.sbu, parseInt(this.empId), this.investmentAprService.investmentAprFormData.donationId).subscribe(response => {
      this.budgetCeiling = response[0] as IBudgetCeiling;
      this.isBudgetVisible = true;
    }, error => {
      console.log(error);
    });
  }
  insertInvestmentApr() {
    this.investmentAprService.investmentAprCommentFormData.employeeId = parseInt(this.empId);
    this.SpinnerService.show();
    this.investmentAprService.insertInvestmentApr().subscribe(
      res => {
        this.investmentAprService.investmentAprCommentFormData = res as IInvestmentAprComment;
        this.isValid = true;
        this.insertInvestmentTargetedProd();
        this.SpinnerService.hide();
        if (this.sbu != this.investmentAprService.investmentAprFormData.sbu) 
        { 
        this.toastr.success('Save successfully', 'Investment')
        }
      },
      err => { console.log(err); }
    );
  }
  updateInvestmentApr() {
    this.SpinnerService.show();
    this.investmentAprService.updateInvestmentApr().subscribe(
      res => {
        this.isValid = true;
        this.investmentAprService.investmentAprCommentFormData = res as IInvestmentAprComment;
        this.insertInvestmentTargetedProd();
        this.SpinnerService.hide();
        if (this.sbu != this.investmentAprService.investmentAprFormData.sbu) 
        { 
        this.toastr.success('Save successfully', 'Investment')
        }
      },
      err => { console.log(err); }
    );
  }
  insertInvestmentDetails() {
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
    if (this.investmentAprService.investmentDetailFormData.fromDate == null || this.investmentAprService.investmentDetailFormData.fromDate == undefined) {
      this.toastr.warning('Select From Date  First', 'Investment ');
      return false;
    }
    if (this.investmentAprService.investmentDetailFormData.toDate == null || this.investmentAprService.investmentDetailFormData.toDate == undefined) {
      this.toastr.warning('Select To Date  First', 'Investment ');
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

    this.investmentAprService.investmentDetailFormData.investmentInitId = this.investmentAprService.investmentAprFormData.id;

    this.SpinnerService.show();
    this.investmentAprService.insertInvestmentDetail(parseInt(this.empId), this.sbu).subscribe(
      res => {
        var data = res as IInvestmentApr;
        this.investmentAprService.investmentDetailFormData = data;
        this.investmentAprService.investmentDetailFormData.fromDate = new Date(data.fromDate);
        this.investmentAprService.investmentDetailFormData.toDate = new Date(data.toDate);
        this.isDonationValid = true;
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
      res => {if (this.sbu == this.investmentAprService.investmentAprFormData.sbu) 
        { 
        this.insertInvestmentDetails();
        }
        this.getInvestmentTargetedProd();
        this.getInvestmentTargetedGroup();
        this.isDonationValid = true;
        this.SpinnerService.hide();
        if (this.sbu != this.investmentAprService.investmentAprFormData.sbu) 
        { 
        this.toastr.success('Save successfully', 'Investment Product');
        }
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

  editInvestmentTargetedProd(selectedAprord: IInvestmentTargetedProd) {
    this.investmentAprService.investmentTargetedProdFormData = Object.assign({}, selectedAprord);
  }
  populateForm() {
  }
  resetPage(form: NgForm) {
    form.reset();
    this.investmentAprService.investmentAprFormData = new InvestmentInit();
    this.investmentAprService.investmentAprCommentFormData = new InvestmentAprComment();
    this.investmentTargetedProds = [];
    this.investmentTargetedGroups = [];
    this.investmentDetailsOld = [];
    this.lastFiveInvestmentDetail = [];
    this.isAdmin = false;
    this.isValid = false;
    this.isBudgetVisible = false;
    this.configs = {
      currentPage: 1,
      itemsPerPage: 10,
      totalItems: 50,
    };
  }
  resetForm() {
    this.investmentAprService.investmentAprFormData = new InvestmentInit();
    this.investmentAprService.investmentAprCommentFormData = new InvestmentAprComment();
    this.investmentTargetedProds = [];
    this.investmentTargetedGroups = [];
    this.investmentDetailsOld = [];
    this.lastFiveInvestmentDetail = [];
    this.isAdmin = false;
    this.isValid = false;
    this.isBudgetVisible = false;
    this.configs = {
      currentPage: 1,
      itemsPerPage: 10,
      totalItems: 50,
    };
  }

  removeInvestmentTargetedProd(selectedAprord: IInvestmentTargetedProd) {
    if (this.investmentTargetedProds.find(x => x.productId == selectedAprord.productId)) {
      this.investmentTargetedProds.splice(this.investmentTargetedProds.findIndex(x => x.productId == selectedAprord.productId), 1);
    }
    if (this.investmentAprService.investmentAprCommentFormData.id == null || this.investmentAprService.investmentAprCommentFormData.id == undefined || this.investmentAprService.investmentAprCommentFormData.id == 0) {
      return false;
    }
    this.investmentAprService.investmentTargetedProdFormData = Object.assign({}, selectedAprord);
    var c = confirm("Are you sure you want to delete that?");
    if (c == true) {
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
  onPageChanged(event: any) {
    const params = this.investmentAprService.getGenParams();
    if (params.pageIndex !== event) {
      params.pageIndex = event;
      this.investmentAprService.setGenParams(params);
      this.getInvestmentApprovedPgChange();
    }
  }
  getInvestmentApprovedPgChange() {
    const params = this.investmentAprService.getGenParams();
    this.SpinnerService.show();
    this.investmentAprService.getInvestmentApproved(parseInt(this.empId), this.sbu,this.userRole).subscribe(response => {
      this.SpinnerService.hide();
      this.investmentInits = response.data;
      this.totalCount = response.count;
      this.configs = {
        currentPage: params.pageIndex,
        itemsPerPage: params.pageSize,
        totalItems: this.totalCount,
      };
    
    }, error => {
      this.SpinnerService.hide();
      console.log(error);
    });
  }
  resetSearch() {
    this.searchText = '';
  }
}