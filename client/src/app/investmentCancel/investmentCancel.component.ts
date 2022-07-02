import {
  InvestmentApr, IInvestmentApr,
  InvestmentTargetedProd, IInvestmentTargetedProd, InvestmentTargetedGroup, IInvestmentTargetedGroup, IInvestmentAprComment, InvestmentAprComment, IInvestmentInitForApr, InvestmentInitForApr
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
import { IInvestmentDetail, IInvestmentDetailOld, IInvestmentMedicineProd, ILastFiveInvestmentDetail, InvestmentMedicineProd } from '../shared/models/investment';
import { NgxSpinnerService } from 'ngx-spinner';
import { IBudgetCeiling } from '../shared/models/budgetCeiling';
import { IDepotInfo } from '../shared/models/depotInfo';
import { IInvestmentRecDepot, InvestmentRecDepot } from '../shared/models/InvestmentRecDepot';
import { IMedicineProduct } from '../shared/models/medicineProduct';
import { IBudgetCeilingForCampaign } from '../shared/models/budgetCeilingForCampaign';
import { InvestmentCancelService } from '../_services/investmentCancel.service';
import { IEmployeeLocation } from '../shared/models/empLocation';
import { IInvestmentRcvCommentRpt } from '../shared/models/investmentRcv';

@Component({
  selector: 'app-investmentCancel',
  templateUrl: './investmentCancel.component.html',
  styles: [
  ],
  providers: [DatePipe]
})
export class InvestmentCancelComponent implements OnInit {
  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  @ViewChild('investmentInitSearchModal', { static: false }) investmentInitSearchModal: TemplateRef<any>;
  @ViewChild('investmentAprSearchModal', { static: false }) investmentAprSearchModal: TemplateRef<any>;
  InvestmentInitSearchModalRef: BsModalRef;
  InvestmentAprSearchModalRef: BsModalRef;
  investmentMedicineProds: IInvestmentMedicineProd[];
  investmentDetailsOld: IInvestmentDetailOld[];
  lastFiveInvestmentDetail: ILastFiveInvestmentDetail[];
  investmentAprs: IInvestmentApr[];
  investmentTargetedProds: IInvestmentTargetedProd[];
  investmentTargetedGroups: IInvestmentTargetedGroup[];
  investmentDetails: IInvestmentApr[];
  investmentDoctors: IInvestmentDoctor[];
  empLocation: IEmployeeLocation[];
  investmentRcvList: IInvestmentRcvCommentRpt[];
  apprDetail: IInvestmentDetailTracker[];
  isValid: boolean = false;
  isSaveButtonDisable: boolean = false;
  isInvOther: boolean = false;
  isDeleted: boolean = false;
  isBudgetVisible: boolean = false;
  isBudgetForCampaignVisible: boolean = false;
  isDonationValid: boolean = false;
  searchText = '';
  configs: any;
  numberPattern = "^[0-9]+(.[0-9]{1,10})?$";
  depots: IDepotInfo[];
  bcds: IBcdsInfo[];
  society: ISocietyInfo[];
  markets: IMarket[];
  products: IProduct[];
  medicineProducts: IMedicineProduct[];
  budgetCeiling: IBudgetCeiling;
  budgetCeilingForCampaign: IBudgetCeilingForCampaign;
  campaignDtlproducts: IProduct[];
  subCampaigns: ISubCampaign[];
  doctors: IDoctor[];
  institutions: IInstitution[];
  donations: IDonation[];
  campaignMsts: ICampaignMst[];
  campaignDtls: ICampaignDtl[];
  campaignDtlProducts: ICampaignDtlProduct[];
  marketGroupMsts: IMarketGroupMst[];
  investmentInits: IInvestmentInit[];
  donationToVal: string;
  bsConfig: Partial<BsDatepickerConfig>;
  bsValue: Date = new Date();
  isAdmin: boolean = false;
  isDepotRequire: boolean = false;
  empId: string;
  donationName: string;
  sbu: string;
  remainingSBUShow: boolean;
  config = {
    keyboard: false,
    class: 'modal-lg',
    ignoreBackdropClick: true
  };
  minDate: Date;
  maxDate: Date;
  userRole: any;
  convertedDate: string;
  marketCode: string;
  //isDeleted:boolean;
  investmentInit: InvestmentInit;
  constructor(private accountService: AccountService, public investmentCancelService: InvestmentCancelService, private router: Router,
    private toastr: ToastrService, private modalService: BsModalService, private datePipe: DatePipe, private SpinnerService: NgxSpinnerService) { }
  ngOnInit() {
    //this.isDeleted=false;
    this.getEmployeeId();
    //this.getDonation();
    this.bsConfig = Object.assign({}, { containerClass: 'theme-blue' }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();
    const currentDate = new Date();
    this.minDate = new Date(currentDate.getFullYear(), 0, 1);
    this.maxDate = new Date(currentDate.getFullYear(), 11, 31);
  }
  getEmployeeLocation() {
    this.investmentCancelService.getEmpLoc(this.investmentCancelService.investmentCancelFormData.id).subscribe(response => {
      var data = response as IEmployeeLocation[];
      if (data !== undefined) {
        this.empLocation = data;
      }
    }, error => {
      console.log(error);
    });
  }

  GetData(id) {
    //this.getInvestmentInit(id);
    //this.investmentCancelService.investmentCancelFormData.id = id;
    this.investmentCancelService.investmentDetailFormData.investmentInitId = id;
    this.investmentCancelService.investmentDoctorFormData.investmentInitId = id;
    this.investmentCancelService.investmentInstitutionFormData.investmentInitId = id;
    this.investmentCancelService.investmentCampaignFormData.investmentInitId = id;
    this.investmentCancelService.investmentBcdsFormData.investmentInitId = id;
    this.investmentCancelService.investmentSocietyFormData.investmentInitId = id;
    this.investmentCancelService.investmentDetailFormData.investmentInitId = id;
    if (this.investmentCancelService.investmentCancelFormData.donationTo == "Doctor") {
      this.getInvestmentDoctor();
    }
    else if (this.investmentCancelService.investmentCancelFormData.donationTo == "Institution") {
      this.getInvestmentInstitution();
    }
    else if (this.investmentCancelService.investmentCancelFormData.donationTo == "Campaign") {
      this.getInvestmentCampaign();
    }
    else if (this.investmentCancelService.investmentCancelFormData.donationTo == "Bcds") {
      this.getInvestmentBcds();
    }
    else if (this.investmentCancelService.investmentCancelFormData.donationTo == "Society") {
      this.getInvestmentSociety();
    }
    this.getInvestmentDetails();
    debugger;
    this.getInvestmentDetailTracker();
    this.getInvestmentTargetedProd();
    this.getInvestmentTargetedGroupStatus();
    this.getinvestmentRcvComment();
    this.getEmployeeLocation();
    // if (parseInt(this.empId) == this.investmentCancelService.investmentCancelFormData.employeeId) {
    //   this.isInvOther = false;
    // }
    // else {
    //   this.isInvOther = true;
    // }
  }

  getInvestmentInit(id: string) {
    this.investmentCancelService.getInvestmentInit(parseInt(id)).subscribe(response => {
      this.investmentInits = response as IInvestmentInit[];
      if (this.investmentInits.length > 0) {
        this.investmentCancelService.investmentCancelFormData.id = this.investmentInits[0].id;
        this.investmentCancelService.investmentCancelFormData.proposeFor = this.investmentInits[0].proposeFor;
        this.investmentCancelService.investmentCancelFormData.referenceNo = this.investmentInits[0].referenceNo;
        this.investmentCancelService.investmentCancelFormData.donationTo = this.investmentInits[0].donationTo;
        this.investmentCancelService.investmentCancelFormData.donationTypeName = this.investmentInits[0].donationTypeName;

        if (this.investmentCancelService.investmentCancelFormData.donationTo == "Doctor") {
          this.getDoctor();
        }
        else if (this.investmentCancelService.investmentCancelFormData.donationTo == "Institution") {
          this.getInstitution();
        }
        else if (this.investmentCancelService.investmentCancelFormData.donationTo == "Campaign") {
          this.getCampaignMst();
        }
        else if (this.investmentCancelService.investmentCancelFormData.donationTo == "Bcds") {
          this.getBcds();
        }
        else if (this.investmentCancelService.investmentCancelFormData.donationTo == "Society") {
          this.getSociety();
        }
        this.getInvestmentDetails();
        this.getEmployeeLocation()

      }
    }, error => {
      this.SpinnerService.hide();
      console.log(error);
    });
  }


  getinvestmentRcvComment() {
    this.investmentCancelService.getInvestmentRcvComment(this.investmentCancelService.investmentCancelFormData.id).subscribe(response => {
      var data = response as IInvestmentRcvCommentRpt[];
      if (data !== undefined) {
        this.investmentRcvList = data;
      }
      else {
        this.investmentRcvList = [];
      }
    }, error => {
      console.log(error);
    });
  }
  getInvestmentDetails() {
    this.investmentCancelService.getInvestmentDetails(this.investmentCancelService.investmentDetailFormData.investmentInitId, parseInt(this.empId), this.userRole).subscribe(response => {
      var data = response[0] as IInvestmentDetail;
      if (data !== undefined) {

        this.investmentCancelService.investmentDetailFormData = data;
        this.investmentCancelService.investmentDetailFormData.fromDate = new Date(data.fromDate);
        this.investmentCancelService.investmentDetailFormData.toDate = new Date(data.toDate);
        this.investmentCancelService.investmentDetailFormData.commitmentFromDate = new Date(data.commitmentFromDate);
        this.investmentCancelService.investmentDetailFormData.commitmentToDate = new Date(data.commitmentToDate);
        this.convertedDate = this.datePipe.transform(data.fromDate, 'ddMMyyyy');

      } else {
      }
    }, error => {
      console.log(error);
    });
  }
  getInvestmentDetailTracker() {
    this.investmentCancelService.getInvestmentDetailTracker(this.investmentCancelService.investmentDetailFormData.investmentInitId).subscribe(response => {
      var data = response as IInvestmentDetailTracker[];
      if (data !== undefined) {

        this.apprDetail = data;


      } else {
      }
    }, error => {
      console.log(error);
    });
  }

  getInvestmentCampaign() {
    this.investmentCancelService.getInvestmentCampaigns(this.investmentCancelService.investmentCampaignFormData.investmentInitId).subscribe(response => {
      var data = response[0] as IInvestmentCampaign;
      if (data !== undefined) {
        this.investmentCancelService.investmentCampaignFormData = data;
        this.investmentCancelService.investmentCampaignFormData.campaignMstId = data.campaignDtl.mstId;
        this.investmentCancelService.investmentCampaignFormData.doctorName = data.doctorInfo.doctorName;
        this.investmentCancelService.investmentCampaignFormData.institutionName = data.institutionInfo.institutionName;
        this.investmentCancelService.investmentCampaignFormData.subCampStartDate = new DatePipe('en-US').transform(data.campaignDtl.subCampStartDate, 'dd/MM/yyyy');
        this.investmentCancelService.investmentCampaignFormData.subCampEndDate = new DatePipe('en-US').transform(data.campaignDtl.subCampEndDate, 'dd/MM/yyyy')
        this.investmentCancelService.getCampaignMsts(data.campaignDtl.mstId).subscribe(response => {
          this.campaignMsts = response as ICampaignMst[];

          for (let i = 0; i < this.campaignMsts.length; i++) {
            if (this.campaignMsts[i].id == this.investmentCancelService.investmentCampaignFormData.campaignDtl.mstId) {
              this.investmentCancelService.investmentCampaignFormData.campaignName = this.campaignMsts[i].campaignName;
            }
          }
          this.investmentCancelService.getCampaignDtls(data.campaignDtl.mstId).subscribe(response => {
            this.campaignDtls = response as ICampaignDtl[];
            for (let i = 0; i < this.campaignDtls.length; i++) {
              if (this.campaignDtls[i].id == data.campaignDtl.id) {
                this.investmentCancelService.investmentCampaignFormData.subCampaignName = this.campaignDtls[i].subCampaign.subCampaignName;
              }
            }
          }, error => {
            console.log(error);
          });
        }, error => {
          console.log(error);
        });


        this.investmentCancelService.getCampaignDtlProducts(data.campaignDtl.id).subscribe(response => {
          this.campaignDtlProducts = response as ICampaignDtlProduct[];
        }, error => {
          console.log(error);
        });
      }
    }, error => {
      console.log(error);
    });

  }

  getInvestmentBcds() {
    this.investmentCancelService.getInvestmentBcds(this.investmentCancelService.investmentCancelFormData.id).subscribe(response => {
      var data = response[0] as IInvestmentBcds;
      if (data !== undefined) {
        this.investmentCancelService.investmentBcdsFormData = data;
        this.investmentCancelService.investmentBcdsFormData.responsibleDoctorName = data.doctorInfo.doctorName;
        this.investmentCancelService.investmentBcdsFormData.bcdsName = data.bcds.bcdsName;
        this.investmentCancelService.investmentBcdsFormData.bcdsAddress = data.bcds.bcdsAddress;
        this.investmentCancelService.investmentBcdsFormData.noOfMember = data.bcds.noOfMember;
      }
      else {
      }

    }, error => {
      console.log(error);
    });
  }
  getInvestmentSociety() {
    this.investmentCancelService.getInvestmentSociety(this.investmentCancelService.investmentCancelFormData.id).subscribe(response => {
      var data = response[0] as IInvestmentSociety;
      if (data !== undefined) {
        this.investmentCancelService.investmentSocietyFormData = data;
        this.investmentCancelService.investmentSocietyFormData.responsibleDoctorName = data.doctorInfo.doctorName;
        this.investmentCancelService.investmentSocietyFormData.societyName = data.society.societyName;
        this.investmentCancelService.investmentSocietyFormData.noOfMember = data.society.noOfMember;
        this.investmentCancelService.investmentSocietyFormData.societyAddress = data.society.societyAddress;
      }
      else {
      }
    }, error => {
      console.log(error);
    });
  }
  getInvestmentInstitution() {
    this.investmentCancelService.getInvestmentInstitutions(this.investmentCancelService.investmentCancelFormData.id).subscribe(response => {
      var data = response[0] as IInvestmentInstitution;
      if (data !== undefined) {
        this.investmentCancelService.investmentInstitutionFormData = data;
        this.investmentCancelService.investmentInstitutionFormData.responsibleDoctorName = data.doctorInfo.doctorName;
        this.investmentCancelService.investmentInstitutionFormData.institutionName = data.institutionInfo.institutionName;
        this.investmentCancelService.investmentInstitutionFormData.address = data.institutionInfo.address;
        this.investmentCancelService.investmentInstitutionFormData.institutionType = data.institutionInfo.institutionType;
      }
      else {
      }

    }, error => {
      console.log(error);
    });
  }
  getInvestmentDoctor() {
    this.investmentCancelService.getInvestmentDoctors(this.investmentCancelService.investmentCancelFormData.id).subscribe(response => {
      var data = response[0] as IInvestmentDoctor;
      if (data !== undefined) {
        this.investmentCancelService.investmentDoctorFormData = data;
        this.investmentCancelService.investmentDoctorFormData.doctorName = data.doctorInfo.doctorName;
        this.investmentCancelService.investmentDoctorFormData.degree = data.doctorInfo.degree;
        this.investmentCancelService.investmentDoctorFormData.designation = data.doctorInfo.designation;
        this.investmentCancelService.investmentDoctorFormData.institutionName = data.institutionInfo.institutionName;
        this.investmentCancelService.investmentDoctorFormData.address = data.institutionInfo.address;
      }
      else {
      }
    }, error => {
      console.log(error);
    });
  }
  getInvestmentTargetedProd() {

    this.investmentCancelService.getInvestmentTargetedProds(this.investmentCancelService.investmentCancelFormData.id, this.sbu).subscribe(response => {

      var data = response as IInvestmentTargetedProd[];
      if (data !== undefined) {
        this.investmentTargetedProds = data;

      }
      else {
      }

    }, error => {
      console.log(error);

    });
  }
  getInvestmentTargetedGroupStatus() {
    this.investmentCancelService.getInvestmentTargetedGroupStatus(this.investmentCancelService.investmentCancelFormData.id).subscribe(response => {
      var data = response as IInvestmentTargetedGroup[];
      if (data !== undefined) {
        this.investmentTargetedGroups = data;
      }
    }, error => {
      console.log(error);
    });
  }
  getEmployeeId() {
    this.empId = this.accountService.getEmployeeId();
    this.userRole = this.accountService.getUserRole();
    this.investmentCancelService.investmentCancelFormData.employeeId = parseInt(this.empId);
    this.getMarketGroupMsts();
    this.getEmployeeSbu();

  }
  getEmployeeSbu() {
    this.accountService.getEmployeeSbu(this.investmentCancelService.investmentCancelFormData.employeeId).subscribe(
      (response) => {
        this.sbu = response.sbu;
        this.investmentCancelService.investmentCancelFormData.marketCode = response.marketCode;
        this.marketCode = response.marketCode;
        this.getProduct();
        //this.getLastFiveInvestment(this.investmentCancelService.investmentCancelFormData.marketCode, this.todayDate);
      },
      (error) => {
        console.log(error);
      }
    );
  }

  getDonation() {
    this.investmentCancelService.getDonations().subscribe(response => {
      this.donations = response as IDonation[];
    }, error => {
      console.log(error);
    });
  }
  getSubCampaign() {
    this.investmentCancelService.getSubCampaigns().subscribe(response => {
      this.subCampaigns = response as ISubCampaign[];
    }, error => {
      console.log(error);
    });
  }
  getDoctor() {
    this.SpinnerService.show();
    this.investmentCancelService.getDoctors(this.investmentCancelService.investmentCancelFormData.marketCode).subscribe(response => {
      this.doctors = response as IDoctor[];
      this.investmentCancelService.getInstitutions(this.investmentCancelService.investmentCancelFormData.marketCode).subscribe(response => {
        this.institutions = response as IInstitution[];
        if (this.investmentCancelService.investmentCancelFormData.id != null && this.investmentCancelService.investmentCancelFormData.id != undefined && this.investmentCancelService.investmentCancelFormData.id != 0) {
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
  getInstitution() {
    this.SpinnerService.show();
    this.investmentCancelService.getInstitutions(this.investmentCancelService.investmentCancelFormData.marketCode).subscribe(response => {
      this.institutions = response as IInstitution[];
      this.investmentCancelService.getDoctors(this.investmentCancelService.investmentCancelFormData.marketCode).subscribe(response => {
        this.doctors = response as IDoctor[];
        if (this.investmentCancelService.investmentCancelFormData.id != null && this.investmentCancelService.investmentCancelFormData.id != undefined && this.investmentCancelService.investmentCancelFormData.id != 0) {
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
  getCampaignMst() {
    this.SpinnerService.show();
    this.investmentCancelService.getCampaignMsts(parseInt(this.empId)).subscribe(response => {
      this.campaignMsts = response as ICampaignMst[];
      this.investmentCancelService.getDoctors(this.investmentCancelService.investmentCancelFormData.marketCode).subscribe(response => {
        this.doctors = response as IDoctor[];
        this.investmentCancelService.getInstitutions(this.investmentCancelService.investmentCancelFormData.marketCode).subscribe(response => {
          this.institutions = response as IInstitution[];
          if (this.investmentCancelService.investmentCancelFormData.id != null && this.investmentCancelService.investmentCancelFormData.id != undefined && this.investmentCancelService.investmentCancelFormData.id != 0) {
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
    this.investmentCancelService.getSociety().subscribe(response => {
      this.society = response as ISocietyInfo[];
      this.investmentCancelService.getDoctors(this.investmentCancelService.investmentCancelFormData.marketCode).subscribe(response => {
        this.doctors = response as IDoctor[];
        if (this.investmentCancelService.investmentCancelFormData.id != null && this.investmentCancelService.investmentCancelFormData.id != undefined && this.investmentCancelService.investmentCancelFormData.id != 0) {
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
    this.investmentCancelService.getBcds().subscribe(response => {
      this.bcds = response as IBcdsInfo[];
      this.investmentCancelService.getDoctors(this.investmentCancelService.investmentCancelFormData.marketCode).subscribe(response => {
        this.doctors = response as IDoctor[];
        if (this.investmentCancelService.investmentCancelFormData.id != null && this.investmentCancelService.investmentCancelFormData.id != undefined && this.investmentCancelService.investmentCancelFormData.id != 0) {
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
    this.investmentCancelService.getMarkets().subscribe(response => {
      this.markets = response as IMarket[];
      this.SpinnerService.hide();
    }, error => {
      this.SpinnerService.hide();
      console.log(error);
    });
  }
  getProduct() {
    this.investmentCancelService.getProduct(this.sbu).subscribe(response => {
      this.products = response as IProduct[];
    }, error => {
      console.log(error);
    });
  }
  getMarketGroupMsts() {
    this.investmentCancelService.getMarketGroupMsts(this.empId).subscribe(response => {
      this.marketGroupMsts = response as IMarketGroupMst[];
    }, error => {
      console.log(error);
    });
  }
  onSubmit(form: NgForm) {

    if (this.apprDetail.length > 0) {
      this.toastr.warning('Investment can not be deleted, Payement existed!')
    }
    else {

    }
  }

  resetPage(form: NgForm) {
    window.location.reload();
  }
  resetPageLoad() {
    window.location.reload();
    this.investmentCancelService.investmentCancelFormData = new InvestmentInit();
    this.investmentCancelService.investmentCancelFormData.marketCode = this.marketCode;
    this.investmentTargetedGroups = [];
    this.investmentTargetedProds = [];
    this.investmentDetailsOld = [];
    this.isDeleted=false;
  }
  ViewData() {
    if ((this.investmentCancelService.investmentCancelFormData.referenceNo == undefined || this.investmentCancelService.investmentCancelFormData.referenceNo == "")) {
      this.toastr.warning('Please enter ReferenceNo!');
      return false;
    }
    if (this.investmentCancelService.investmentCancelFormData.referenceNo != undefined && this.investmentCancelService.investmentCancelFormData.referenceNo != "") {
      if (this.investmentCancelService.investmentCancelFormData.referenceNo.length != 11) {
        this.toastr.warning('Please enter must be 11 character in Reference No ');
        return false;
      }
    }
    this.investmentCancelService.GetInvestmentSummarySingleDoc(this.investmentCancelService.investmentCancelFormData.referenceNo).subscribe(response => {

      this.investmentInit = response as InvestmentInit;
      if (this.investmentInit[0].dataStatus == 0) {
        this.isDeleted=true;
      }
      else{
        this.isDeleted=false;
      }
      this.investmentCancelService.investmentCancelFormData.id = this.investmentInit[0].id;
      this.investmentCancelService.investmentCancelFormData.proposeFor = this.investmentInit[0].proposeFor;
      this.investmentCancelService.investmentCancelFormData.referenceNo = this.investmentInit[0].referenceNo.substring(0,11);
      this.investmentCancelService.investmentCancelFormData.donationTo = this.investmentInit[0].donationTo;
      this.investmentCancelService.investmentCancelFormData.donationTypeName = this.investmentInit[0].donationTypeName;

      this.GetData(this.investmentInit[0].id);
    }, error => {
      console.log(error);
    });
  }
  removeInvestmentDetail(selectedRecord: IInvestmentDetailTracker) {

    var c = confirm("Are you sure you want to delete that?");
    if (c == true) {
      this.SpinnerService.show();
      //this.investmentCancelService.removeInvestmentDetal(selectedRecord.id).subscribe(
      this.investmentCancelService.isInvestmentDetailExist(selectedRecord.id, parseInt(this.empId)).subscribe(
        res => {

          var message = res as string;
          if (message == 'Not Existed') {
            this.investmentCancelService.removeInvestmentDetail(selectedRecord.id, parseInt(this.empId)).subscribe(
              res => {
                var message = res as string;
                if (message === "Deleted") {
                  this.getInvestmentDetailTracker();
                  //this.ViewData();
                  this.toastr.success('Removed Successfully');
                }
                else{
                  this.toastr.warning(message);
                }
              
                this.SpinnerService.hide();

               
              },
              err => {
                this.SpinnerService.hide();
                console.log(err);
              }
            );
          }
          else {
            this.SpinnerService.hide();
            this.toastr.warning('Already disbursed');
          }
        },
        err => {
          this.SpinnerService.hide();
          console.log(err);
        }
      );

    }
  }



  cancelInvestment() {
    if (this.investmentInit[0].dataStatus == 0) {
      this.toastr.warning("Alreday Inactive!");
      return;
    }
   
    var c = confirm("Are you sure you want to delete that?");
    if (c == true) {
      this.SpinnerService.show();
      {
        this.investmentCancelService.cancelInv(this.investmentCancelService.investmentCancelFormData.id, parseInt(this.empId)).subscribe(
          res => {
            var message = res as string;
            debugger;

            //alert(message);
             if (message === "Deleted") { 
              this.ViewData();
              this.toastr.success('Removed Successfully'); 
             }
            else {
               this.toastr.warning('Already disbursed');
             }

          },
          err => {
            // if (err.error.message == 'Deleted') {
            //   this.toastr.success('Deleted Successfully');
            // }
            // else {
            //   this.toastr.warning(err.error.message);
            // }
            this.SpinnerService.hide();
            console.log(err);
            this.toastr.warning(err.error.text);
          }
        );
      }

      err => {
        this.SpinnerService.hide();
        console.log(err);
      }

    }
  }
}
export interface IInvestmentDetailTracker {
  id: number;
  investmentInitId: number;
  employeeId: number;
  donationId: number;
  month: string;
  year: string;
  approvedAmount: string;
  paidStatus: string;
  paymentRefNo: string;
  fromDate: Date;
  toDate: Date;
}
export interface IInvestmentInit {
  id: number;
  dataStatus: number;
  referenceNo: string;
  proposeFor: string;
  donationTo: string;
  donationTypeName: string;
  marketCode: string;
  sbu: string;
  employeeId: number;
  employee: IEmployee;
  setOn: Date;
}
export class InvestmentInit implements IInvestmentInit {
  id: number = 0;
  dataStatus: number = 1;
  referenceNo: string;
  proposeFor: string = null;
  donationTypeName: string = null;
  donationTo: string = null;
  marketCode: string;
  sbu: string;
  employeeId: number;
  employee: IEmployee;
  setOn: Date;
}
export interface IEmployee {
  id: number;
  employeeName: string;
  employeeCode: string;
  SBU: string;
  designationName: string;
  remarks: string;
  status: string;

}
