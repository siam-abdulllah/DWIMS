import {
  InvestmentApr, IInvestmentApr, InvestmentInit, IInvestmentInit,
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
  //totalCount = 0;
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
  investmentInit: InvestmentInit;
  constructor(private accountService: AccountService, public investmentCancelService: InvestmentCancelService, private router: Router,
    private toastr: ToastrService, private modalService: BsModalService, private datePipe: DatePipe, private SpinnerService: NgxSpinnerService) { }
  ngOnInit() {
    //this.resetForm();
    this.getEmployeeId();
    this.getDonation();
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

  GetData(id)
  {
    this.getInvestmentInit(id);
    this.investmentCancelService.investmentCancelFormData.id = id;
    this.investmentCancelService.investmentRcvFormData.id = id;
    this.investmentCancelService.investmentDetailFormData.investmentInitId= id;
    //this.investmentCancelService.investmentCancelFormData = Object.assign({}, selectedRecord);
    this.investmentCancelService.investmentDoctorFormData.investmentInitId = id;
    this.investmentCancelService.investmentInstitutionFormData.investmentInitId = id;
    this.investmentCancelService.investmentCampaignFormData.investmentInitId = id;
    this.investmentCancelService.investmentBcdsFormData.investmentInitId = id;
    this.investmentCancelService.investmentSocietyFormData.investmentInitId = id;
    this.investmentCancelService.investmentDetailFormData.investmentInitId = id;
    //this.isDonationValid = true;
    if (this.investmentCancelService.investmentCancelFormData.donationTo == "Doctor") {
      //this.getDoctor();
      //this.getInstitution();
      this.getInvestmentDoctor();
    }
    else if (this.investmentCancelService.investmentCancelFormData.donationTo == "Institution") {
      //this.getDoctor();
      //this.getInstitution();
      this.getInvestmentInstitution();
    }
    else if (this.investmentCancelService.investmentCancelFormData.donationTo == "Campaign") {
      //this.getCampaignMst();
      //this.getDoctor();
      //this.getInstitution();
      this.getInvestmentCampaign();
    }
    else if (this.investmentCancelService.investmentCancelFormData.donationTo == "Bcds") {
      //this.getBcds();
      this.getInvestmentBcds();
    }
    else if (this.investmentCancelService.investmentCancelFormData.donationTo == "Society") {
      //this.getSociety();
      this.getInvestmentSociety();

    }
    this.getInvestmentDetails();
    this.getInvestmentTargetedProd();
    //this.getInvestmentTargetedGroup();
    this.getInvestmentTargetedGroupStatus();

    this.getinvestmentRcvComment();

    if (parseInt(this.empId) == this.investmentCancelService.investmentCancelFormData.employeeId) {
      this.isInvOther = false;
      //this.isValid = true;
      // this.getInvestmentTargetedProd();
    }
    else {
      this.isInvOther = true;
      //this.isValid = false;
    }
    // if (this.investmentCancelService.investmentCancelFormData.confirmation==true) {
    //   this.isSubmitted = true;
    //   //this.isValid = true;
    //   // this.getInvestmentTargetedProd();
    // }
    // else {
    //   this.isSubmitted  = false;
    //   //this.isValid = false;
    // }
  }

  
  
    getInvestmentInit(id: string) {
    this.investmentCancelService.getInvestmentInit(parseInt(id)).subscribe(response => {
      this.investmentInits = response.data;
      if (this.investmentInits.length>0) {
        this.investmentCancelService.investmentCancelFormData.id =  this.investmentInits[0].id;
        this.investmentCancelService.investmentCancelFormData.proposeFor =  this.investmentInits[0].proposeFor;
        this.investmentCancelService.investmentCancelFormData.referenceNo =  this.investmentInits[0].referenceNo;
        this.investmentCancelService.investmentCancelFormData.donationTo =  this.investmentInits[0].donationTo;
        this.investmentCancelService.investmentCancelFormData.donationId =  this.investmentInits[0].donationId;
        
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
    this.investmentCancelService.getInvestmentRcvComment(this.investmentCancelService.investmentRcvFormData.id).subscribe(response => {
      var data = response as IInvestmentRcvCommentRpt[];
      if (data !== undefined) {
        //this.investmentCancelService.investmentRcvCommentFormData = data;
        this.investmentRcvList=data;
      }
      else {
        //this.toastr.warning('No Data Found', 'Investment ');
        this.investmentRcvList=[];
      }

    }, error => {
      console.log(error);
    });
  }
  getInvestmentDetails() {
    this.investmentCancelService.getInvestmentDetails(this.investmentCancelService.investmentDetailFormData.investmentInitId,parseInt(this.empId),this.userRole).subscribe(response => {
      var data = response[0] as IInvestmentDetail;
      if (data !== undefined) {
        debugger;
        this.investmentCancelService.investmentDetailFormData = data;
        this.investmentCancelService.investmentDetailFormData.fromDate = new Date(data.fromDate);
        this.investmentCancelService.investmentDetailFormData.toDate = new Date(data.toDate);
        this.investmentCancelService.investmentDetailFormData.commitmentFromDate = new Date(data.commitmentFromDate);
        this.investmentCancelService.investmentDetailFormData.commitmentToDate = new Date(data.commitmentToDate);
        this.convertedDate = this.datePipe.transform(data.fromDate, 'ddMMyyyy');
        //this.getLastFiveInvestment(this.investmentCancelService.investmentCancelFormData.marketCode, this.convertedDate);

      } else {
       // this.toastr.warning('No Data Found', 'Investment');
      }
    }, error => {
      console.log(error);
    });
  }
  // getInvestmentCampaign() {
  //   this.investmentCancelService.getInvestmentCampaigns(this.investmentCancelService.investmentCampaignFormData.investmentInitId).subscribe(response => {
  //     var data = response[0] as IInvestmentCampaign;
  //     if (data !== undefined) {
  //       this.investmentCancelService.investmentCampaignFormData = data;

  //       this.investmentCancelService.investmentCampaignFormData.campaignMstId = data.campaignDtl.mstId;
  //       this.investmentCancelService.investmentCampaignFormData.subCampStartDate = new DatePipe('en-US').transform(data.campaignDtl.subCampStartDate, 'dd/MM/yyyy');
  //       this.investmentCancelService.investmentCampaignFormData.subCampEndDate = new DatePipe('en-US').transform(data.campaignDtl.subCampEndDate, 'dd/MM/yyyy');

  //       this.onChangeCampaignInCamp();
  //       this.onChangeSubCampaignInCamp();
  //     }
  //     else {
  //     }

  //   }, error => {
  //     console.log(error);
  //   });
  // }
  getInvestmentCampaign() {
    this.investmentCancelService.getInvestmentCampaigns(this.investmentCancelService.investmentCampaignFormData.investmentInitId).subscribe(response => {
      var data = response[0] as IInvestmentCampaign;
      if (data !== undefined) {
        this.investmentCancelService.investmentCampaignFormData = data;
        this.investmentCancelService.investmentCampaignFormData.campaignMstId = data.campaignDtl.mstId;
        //this.investmentCancelService.investmentCampaignFormData.subCampaignName = data.campaignDtl.subCampaignName;
        this.investmentCancelService.investmentCampaignFormData.doctorName = data.doctorInfo.doctorName;
        this.investmentCancelService.investmentCampaignFormData.institutionName = data.institutionInfo.institutionName;
        this.investmentCancelService.investmentCampaignFormData.subCampStartDate = new DatePipe('en-US').transform(data.campaignDtl.subCampStartDate, 'dd/MM/yyyy');
        this.investmentCancelService.investmentCampaignFormData.subCampEndDate = new DatePipe('en-US').transform(data.campaignDtl.subCampEndDate, 'dd/MM/yyyy')
        this.investmentCancelService.getCampaignMsts(data.campaignDtl.mstId).subscribe(response => {
          this.campaignMsts = response as ICampaignMst[];
          debugger;
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
      // else {
      //   this.toastr.warning('No Data Found', 'Investment ');
      // }
    }, error => {
      console.log(error);
    });

  }
  dateCompare(form: NgForm) {
    if (this.investmentCancelService.investmentDetailFormData.fromDate != null && this.investmentCancelService.investmentDetailFormData.toDate != null) {
      if (this.investmentCancelService.investmentDetailFormData.toDate > this.investmentCancelService.investmentDetailFormData.fromDate) {
      }
      else {
        form.controls.fromDate.setValue(null);
        form.controls.toDate.setValue(null);
        this.toastr.error('Select Appropriate Date Range', 'Error');
      }
    }
  }

  getInvestmentBcds() {
    this.investmentCancelService.getInvestmentBcds(this.investmentCancelService.investmentRcvFormData.id).subscribe(response => {
      var data = response[0] as IInvestmentBcds;
      if (data !== undefined) {
        this.investmentCancelService.investmentBcdsFormData = data;
        this.investmentCancelService.investmentBcdsFormData.responsibleDoctorName = data.doctorInfo.doctorName;
        this.investmentCancelService.investmentBcdsFormData.bcdsName = data.bcds.bcdsName;
        this.investmentCancelService.investmentBcdsFormData.bcdsAddress = data.bcds.bcdsAddress;
        this.investmentCancelService.investmentBcdsFormData.noOfMember = data.bcds.noOfMember;
      }
      else {
       // this.toastr.warning('No Data Found', 'Investment ');
      }

    }, error => {
      console.log(error);
    });
  }
  getInvestmentSociety() {
    this.investmentCancelService.getInvestmentSociety(this.investmentCancelService.investmentRcvFormData.id).subscribe(response => {
      var data = response[0] as IInvestmentSociety;
      if (data !== undefined) {
        this.investmentCancelService.investmentSocietyFormData = data;
        this.investmentCancelService.investmentSocietyFormData.responsibleDoctorName = data.doctorInfo.doctorName;
        this.investmentCancelService.investmentSocietyFormData.societyName = data.society.societyName;
        this.investmentCancelService.investmentSocietyFormData.noOfMember = data.society.noOfMember;
        this.investmentCancelService.investmentSocietyFormData.societyAddress = data.society.societyAddress;
      }
      else {
        //this.toastr.warning('No Data Found', 'Investment ');
      }
    }, error => {
      console.log(error);
    });
  }
  getInvestmentInstitution() {
    this.investmentCancelService.getInvestmentInstitutions(this.investmentCancelService.investmentRcvFormData.id).subscribe(response => {
      var data = response[0] as IInvestmentInstitution;
      if (data !== undefined) {
        this.investmentCancelService.investmentInstitutionFormData = data;
        this.investmentCancelService.investmentInstitutionFormData.responsibleDoctorName = data.doctorInfo.doctorName;
        this.investmentCancelService.investmentInstitutionFormData.institutionName = data.institutionInfo.institutionName;
        this.investmentCancelService.investmentInstitutionFormData.address = data.institutionInfo.address;
        this.investmentCancelService.investmentInstitutionFormData.institutionType = data.institutionInfo.institutionType;
      }
      else {
        //this.toastr.warning('No Data Found', 'Investment ');
      }

    }, error => {
      console.log(error);
    });
  }
  getInvestmentDoctor() {
    this.investmentCancelService.getInvestmentDoctors(this.investmentCancelService.investmentRcvFormData.id).subscribe(response => {
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
        //this.toastr.warning('No Data Found', 'Investment ');
      }
    }, error => {
      console.log(error);
    });
  }
  getInvestmentTargetedProd() {
    debugger;
    this.investmentCancelService.getInvestmentTargetedProds(this.investmentCancelService.investmentCancelFormData.id, this.sbu).subscribe(response => {
      
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
  // getInvestmentTargetedGroup() {
  //   this.investmentCancelService.getInvestmentTargetedGroups(this.investmentCancelService.investmentCancelFormData.id).subscribe(response => {

  //     var data = response as IInvestmentTargetedGroup[];
  //     if (data !== undefined) {
  //       this.investmentTargetedGroups = data;

  //     }
  //     else {
  //       //this.toastr.warning('No Data Found', 'Investment');
  //     }

  //   }, error => {
  //     console.log(error);
  //   });
  // }

  getInvestmentTargetedGroupStatus() {
    this.investmentCancelService.getInvestmentTargetedGroupStatus(this.investmentCancelService.investmentCancelFormData.id).subscribe(response => {
      var data = response as IInvestmentTargetedGroup[];
      //debugger;
      if (data !== undefined) {
        //debugger;
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
        this.marketCode= response.marketCode;
        this.getProduct();
        //this.getLastFiveInvestment(this.investmentCancelService.investmentCancelFormData.marketCode, this.todayDate);
      },
      (error) => {
        console.log(error);
      }
    );
  }
  onChangeProposeFor() {
    // if (this.investmentCancelService.investmentCancelFormData.proposeFor == "BrandCampaign") {
    //   this.investmentCancelService.investmentCancelFormData.donationTo = "Campaign";
    //   this.onChangeDonationTo();
    //   this.isDonationValid = true;
    // }
    // else {
    //   this.isDonationValid = false;
    // }
    if(this.investmentCancelService.investmentCancelFormData.proposeFor == "BrandCampaign" && this.investmentCancelService.investmentCancelFormData.donationTo != "Campaign" && this.investmentCancelService.investmentCancelFormData.donationTo != null)
    {
      this.toastr.warning("For Brand Campaign, must select Campaign");
      this.investmentCancelService.investmentCancelFormData.donationTo =null;
      return false;
    }
    if(this.investmentCancelService.investmentCancelFormData.proposeFor == "Others" && this.investmentCancelService.investmentCancelFormData.donationTo == "Campaign" && this.investmentCancelService.investmentCancelFormData.donationTo != null)
    {
      this.toastr.warning("For Campaign, must select Brand Campaign");
      this.investmentCancelService.investmentCancelFormData.donationTo =null;
      return false;
    }
  }
  onChangeDonationTo() {
    if(this.investmentCancelService.investmentCancelFormData.proposeFor == "BrandCampaign" && this.investmentCancelService.investmentCancelFormData.donationTo != "Campaign" && this.investmentCancelService.investmentCancelFormData.donationTo != null)
    {
      this.toastr.warning("For Brand Campaign, must select Campaign");
      this.investmentCancelService.investmentCancelFormData.proposeFor =null;
      return false;
    }
    if(this.investmentCancelService.investmentCancelFormData.proposeFor == "Others" && this.investmentCancelService.investmentCancelFormData.donationTo == "Campaign" && this.investmentCancelService.investmentCancelFormData.donationTo != null)
    {
      this.toastr.warning("For Campaign, must select Brand Campaign");
      this.investmentCancelService.investmentCancelFormData.proposeFor =null;
      return false;
    }
    if (this.investmentCancelService.investmentCancelFormData.donationTo == "Doctor") {
      if (this.investmentCancelService.investmentDoctorFormData.id == null || this.investmentCancelService.investmentDoctorFormData.id == undefined || this.investmentCancelService.investmentDoctorFormData.id == 0) {
        this.investmentCancelService.investmentDoctorFormData = new InvestmentDoctor();
        this.getDoctor();
        this.getInstitution();
      }
    }
    else if (this.investmentCancelService.investmentCancelFormData.donationTo == "Institution") {
      if (this.investmentCancelService.investmentInstitutionFormData.id == null || this.investmentCancelService.investmentInstitutionFormData.id == undefined || this.investmentCancelService.investmentInstitutionFormData.id == 0) {
        this.investmentCancelService.investmentDoctorFormData = new InvestmentDoctor();
        this.getDoctor();
        this.getInstitution();
      }
    }
    else if (this.investmentCancelService.investmentCancelFormData.donationTo == "Campaign") {
      if (this.investmentCancelService.investmentCampaignFormData.id == null || this.investmentCancelService.investmentCampaignFormData.id == undefined || this.investmentCancelService.investmentCampaignFormData.id == 0) {
        this.investmentCancelService.investmentCampaignFormData = new InvestmentCampaign();
        this.getCampaignMst();
        this.getDoctor();
        this.getInstitution();
      }
    }
    else if (this.investmentCancelService.investmentCancelFormData.donationTo == "Bcds") {
      if (this.investmentCancelService.investmentBcdsFormData.id == null || this.investmentCancelService.investmentBcdsFormData.id == undefined || this.investmentCancelService.investmentBcdsFormData.id == 0) {
        this.investmentCancelService.investmentBcdsFormData = new InvestmentBcds();
        this.getBcds();
      }
    }
    else if (this.investmentCancelService.investmentCancelFormData.donationTo == "Society") {
      if (this.investmentCancelService.investmentSocietyFormData.id == null || this.investmentCancelService.investmentSocietyFormData.id == undefined || this.investmentCancelService.investmentSocietyFormData.id == 0) {
        this.investmentCancelService.investmentSocietyFormData = new InvestmentSociety();
        this.getSociety();
      }
    }
    if (this.investmentCancelService.investmentCancelFormData.id != null && this.investmentCancelService.investmentCancelFormData.id != undefined && this.investmentCancelService.investmentCancelFormData.id != 0) {
      this.investmentCancelService.investmentDoctorFormData.investmentInitId = this.investmentCancelService.investmentCancelFormData.id;
      this.investmentCancelService.investmentInstitutionFormData.investmentInitId = this.investmentCancelService.investmentCancelFormData.id;
      this.investmentCancelService.investmentCampaignFormData.investmentInitId = this.investmentCancelService.investmentCancelFormData.id;
      this.investmentCancelService.investmentBcdsFormData.investmentInitId = this.investmentCancelService.investmentCancelFormData.id;
      this.investmentCancelService.investmentSocietyFormData.investmentInitId = this.investmentCancelService.investmentCancelFormData.id;
    }
  }
  onChangeDoctorInDoc() {
    for (var i = 0; i < this.doctors.length; i++) {
      if (this.doctors[i].id == this.investmentCancelService.investmentDoctorFormData.doctorId) {
        this.investmentCancelService.investmentDoctorFormData.doctorName=this.doctors[i].doctorName;
        //this.investmentCancelService.investmentDoctorFormData.doctorCode = this.doctors[i].doctorCode;
        this.investmentCancelService.investmentDoctorFormData.degree = this.doctors[i].degree;
        this.investmentCancelService.investmentDoctorFormData.designation = this.doctors[i].designation;

        break;
      }
    }
    //this.getLastFiveInvestment(this.investmentCancelService.investmentCancelFormData.marketCode, this.convertedDate);
  }
  onChangeInstitutionInDoc() {
    for (var i = 0; i < this.institutions.length; i++) {
      if (this.institutions[i].id == this.investmentCancelService.investmentDoctorFormData.institutionId) {
        //this.investmentCancelService.investmentDoctorFormData.address = this.institutions[i].address;

        break;
      }
    }
  }
  onChangeInstitutionInInst() {
    for (var i = 0; i < this.institutions.length; i++) {
      if (this.institutions[i].id == this.investmentCancelService.investmentInstitutionFormData.institutionId) {
        this.investmentCancelService.investmentInstitutionFormData.address = this.institutions[i].address;
        this.investmentCancelService.investmentInstitutionFormData.institutionType = this.institutions[i].institutionType;

        break;
      }
    }
    //this.getLastFiveInvestment(this.investmentCancelService.investmentCancelFormData.marketCode, this.convertedDate);
  }
  onChangeCampaignInCamp() {

    this.investmentCancelService.getCampaignDtls(this.investmentCancelService.investmentCampaignFormData.campaignMstId).subscribe(response => {
      this.campaignDtls = response as ICampaignDtl[];
    }, error => {
      console.log(error);
    });
  }
  onChangeBcdsInBcds() {
    for (var i = 0; i < this.bcds.length; i++) {
      if (this.bcds[i].id == this.investmentCancelService.investmentBcdsFormData.bcdsId) {
        this.investmentCancelService.investmentBcdsFormData.bcdsAddress = this.bcds[i].bcdsAddress;
        this.investmentCancelService.investmentBcdsFormData.noOfMember = this.bcds[i].noOfMember;
        break;
      }
    }
  }
  onChangeSubCampaignInCamp() {
    for (var i = 0; i < this.campaignDtls.length; i++) {
      if (this.campaignDtls[i].id == this.investmentCancelService.investmentCampaignFormData.campaignDtlId) {
        this.investmentCancelService.investmentCampaignFormData.subCampStartDate = new DatePipe('en-US').transform(this.campaignDtls[i].subCampStartDate, 'dd/MM/yyyy');
        this.investmentCancelService.investmentCampaignFormData.subCampEndDate = new DatePipe('en-US').transform(this.campaignDtls[i].subCampEndDate, 'dd/MM/yyyy');
        break;
      }
    }
    this.investmentCancelService.getCampaignDtlProducts(this.investmentCancelService.investmentCampaignFormData.campaignDtlId).subscribe(response => {
      this.campaignDtlProducts = response as ICampaignDtlProduct[];
    }, error => {
      console.log(error);
    });

  }
  onChangeSocietyInSociety() {
    for (var i = 0; i < this.society.length; i++) {
      if (this.society[i].id == this.investmentCancelService.investmentSocietyFormData.societyId) {
        this.investmentCancelService.investmentSocietyFormData.societyAddress = this.society[i].societyAddress;
        this.investmentCancelService.investmentSocietyFormData.noOfMember = this.society[i].noOfMember;

        break;
      }
    }
    //this.getLastFiveInvestment(this.investmentCancelService.investmentCancelFormData.marketCode, this.convertedDate);
  }
  onChangeMarketGroupInTargetedGroup() {
    if (this.investmentTargetedGroups == null || this.investmentTargetedGroups.length == 0) {
      for (let i = 0; i < this.marketGroupMsts.length; i++) {
        if (this.marketGroupMsts[i].id == this.investmentCancelService.investmentTargetedGroupFormData.marketGroupMstId) {
          var data = [];
          for (let j = 0; j < this.marketGroupMsts[i].marketGroupDtls.length; j++) {
            if (this.marketGroupMsts[i].marketGroupDtls[j].status == 'Active') {
              var marketGroupMstId = this.marketGroupMsts[i].marketGroupDtls[j].mstId;
              var marketCode = this.marketGroupMsts[i].marketGroupDtls[j].marketCode;
              var marketName = this.marketGroupMsts[i].marketGroupDtls[j].marketName;
              var sbu = this.marketGroupMsts[i].marketGroupDtls[j].sbu;
              var sbuName = this.marketGroupMsts[i].marketGroupDtls[j].sbuName;

              data.push({ id: 0, investmentInitId: this.investmentCancelService.investmentCancelFormData.id, marketGroupMst: this.marketGroupMsts[i], marketGroupMstId: marketGroupMstId, marketCode: marketCode, marketName: marketName,sbu:sbu,sbuName:sbuName });
              //this.investmentTargetedGroups.push({id:0,investmentInitId:this.investmentCancelService.investmentCancelFormData.id,marketGroup:null,marketGroupMstId:this.marketGroupMsts[i].marketGroupDtls[j].mstId,marketCode:this.marketGroupMsts[i].marketGroupDtls[j].marketCode,marketName:this.marketGroupMsts[i].marketGroupDtls[j].marketName});
            }
          }
          this.investmentTargetedGroups = data;
          break
        }
      }
    }
    else {
      this.toastr.warning('Already Market Group Exist', 'Investment', {
        positionClass: 'toast-top-right'
      });
    }
  }
  changeDateInDetail() {
    //this.printingDate=this.getDigitBanglaFromEnglish(this.datePipe.transform(value, "dd/MM/yyyy"));
    if (this.investmentCancelService.investmentDetailFormData.fromDate == null || this.investmentCancelService.investmentDetailFormData.fromDate == undefined) {

      return false;
    }
    if (this.investmentCancelService.investmentDetailFormData.toDate == null || this.investmentCancelService.investmentDetailFormData.toDate == undefined) {

      return false;
    }
    let dateFrom = this.investmentCancelService.investmentDetailFormData.fromDate;
    let dateTo = this.investmentCancelService.investmentDetailFormData.toDate;
    //let dateFrom = new Date();
    //let dateTo = new Date();

    this.investmentCancelService.investmentDetailFormData.totalMonth = dateTo.getMonth() - dateFrom.getMonth() + (12 * (dateTo.getFullYear() - dateFrom.getFullYear()));
    this.investmentCancelService.investmentDetailFormData.totalMonth = this.investmentCancelService.investmentDetailFormData.totalMonth + 1;
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
  }
  // printPDF()
  // {
  //   var data = document.getElementById('content');  //Id of the table

  //     let pdf = new jsPDF('p', 'mm', 'a4'); // A4 size page of PDF  
  //     let position = 0;  
      
  //     pdf.addHTML(data, () => {
  //     //pdf.save('web.pdf');
  //     var blob = pdf.output("blob");
  //     window.open(URL.createObjectURL(blob));
  //     });
  // }
  populateForm() {
    //this.investmentCancelService.campaignFormData = Object.assign({}, selectedRecord);
  }
  resetPage(form: NgForm) {
    window.close();
    // form.reset();
    // this.investmentCancelService.investmentCancelFormData = new InvestmentInit();
    // this.investmentCancelService.investmentCancelFormData.marketCode = this.marketCode;
    // this.isValid = false;
    // this.isSubmitted = false;
    // this.isInvOther = false;
    // this.isDonationValid = false;
    // this.investmentTargetedGroups = [];
    // this.investmentTargetedProds = [];
    // this.investmentDetailsOld = [];
  }
  resetPageLoad() {
    this.investmentCancelService.investmentCancelFormData = new InvestmentInit();
    this.investmentCancelService.investmentCancelFormData.marketCode = this.marketCode;
    this.isDonationValid = false;
    this.investmentTargetedGroups = [];
    this.investmentTargetedProds = [];
    this.investmentDetailsOld = [];
  }
  ViewDataDoc() {
    debugger;
   // this.reportService.IsInvestmentInActiveDoc(this. investmentCancelService.investmentCancelFormData.referenceNo,this.doctorId,this.doctorName).subscribe(response => {
   //   debugger;
     // if(1==0)
     // {
     //   this.isInvestmentInActive=true;
     // }
     // else{
       //this.isInvestmentInActive=false;
       if((this.investmentCancelService.investmentCancelFormData.referenceNo==undefined || this. investmentCancelService.investmentCancelFormData.referenceNo==""))
       {
         this.toastr.warning('Please enter ReferenceNo!');
        return false;
       }
       
       if(this. investmentCancelService.investmentCancelFormData.referenceNo!=undefined && this. investmentCancelService.investmentCancelFormData.referenceNo!="")
       {
         if(this. investmentCancelService.investmentCancelFormData.referenceNo.length!=11)
         {
           this.toastr.warning('Please enter must be 11 character in Reference No ');
           return false;
         }
       }
      //  if(this.doctorName!=undefined && this.doctorName!="")
      //  {
      //    if(this.doctorName.length<4)
      //    {
      //      this.toastr.warning('Please enter minimum 4 character in Doctor Name! ');
      //      return false;
      //    }
      //  }
     this.investmentCancelService.GetInvestmentSummarySingleDoc(this.investmentCancelService.investmentCancelFormData.referenceNo).subscribe(response => {
       debugger;
       this.investmentInit = response as InvestmentInit;
       this.GetData(this.investmentInit[0].id);
     }, error => {
       console.log(error);
     });
 }
}
export interface IInvestmentDetailTracker {
  id: number;
  investmentInitId: number;
  chequeTitle: string;
  paymentMethod: string;
  paymentFreq: string;
  commitmentAllSBU: string;
  commitmentOwnSBU: string;
  shareAllSBU: string;
  shareOwnSBU: string;
  totalMonth: number;
  commitmentTotalMonth: number;
  proposedAmount: string;
  purpose: string;
  fromDate: Date;
  toDate: Date;
  commitmentFromDate: any;
  commitmentToDate: any;
}