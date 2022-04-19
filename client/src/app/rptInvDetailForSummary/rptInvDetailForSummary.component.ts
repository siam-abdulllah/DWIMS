
import {InvestmentInit, IInvestmentInit, InvestmentDetail, IInvestmentDetail,InvestmentTargetedProd, IInvestmentTargetedProd,  IInvestmentDetailOld} from '../shared/models/investment';
import { IInvestmentRcvComment, InvestmentRcvComment, InvestmentTargetedGroup, IInvestmentTargetedGroup, IInvestmentRcvCommentRpt,} from '../shared/models/investmentRcv';
import { InvestmentInstitution, IInvestmentInstitution, InvestmentCampaign, IInvestmentCampaign } from '../shared/models/investmentRcv';
import { InvestmentBcds, IInvestmentBcds, InvestmentSociety, IInvestmentSociety } from '../shared/models/investmentRcv';
import { InvestmentDoctor, IInvestmentDoctor} from '../shared/models/investmentRec';
import { SubCampaign, ISubCampaign } from '../shared/models/subCampaign';
import { Donation, IDonation } from '../shared/models/donation';
import { Doctor, IDoctor } from '../shared/models/docotor';
import { Institution, IInstitution } from '../shared/models/institution';
import 'jspdf-autotable';
import * as jsPDF from 'jspdf';
//import { GenericParams } from '../shared/models/genericParams';
import { Component, ElementRef, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { RptInvDetailForSummaryService } from '../_services/rptInvDetailForSummary.service';
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
import { AccountService } from '../account/account.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { IEmployeeLocation } from '../shared/models/empLocation';
import { IInvestmentRcvInsert } from '../investmentRcv/investmentRcv.component';

@Component({
  selector: 'app-investmentDetail',
  templateUrl: './rptInvDetailForSummary.component.html',
  styles: [
  ],
  providers: [DatePipe]
})
export class RptInvestmentDetailSummaryComponent implements OnInit {
  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  @ViewChild('investmentInitSearchModal', { static: false }) investmentInitSearchModal: TemplateRef<any>;
  @ViewChild('submissionConfirmModal', { static: false }) submissionConfirmModal: TemplateRef<any>;
  InvestmentInitSearchModalRef: BsModalRef;
  submissionConfirmRef: BsModalRef;
  convertedDate:string;
  empId: string;
  sbu: string;
  marketCode: string;
  id: string;
  investmentRcvList: IInvestmentRcvCommentRpt[];
  empLocation: IEmployeeLocation[];
  investmentInits: IInvestmentInit[];
  investmentTargetedProds: IInvestmentTargetedProd[];
  investmentTargetedGroups: IInvestmentTargetedGroup[];
  investmentTargetedGroupss2: IInvestmentTargetedGroup[];
  investmentRcvFormData: InvestmentInit = new InvestmentInit();
  investmentDetailsOld: IInvestmentDetailOld[];
  investmentDoctors: IInvestmentDoctor[];
  isValid: boolean = false;
  isInvOther: boolean = false;
  isDonationValid: boolean = false;
  isSubmitted: boolean = false;
  investmentInitForm: NgForm;
  numberPattern = "^[0-9]+(.[0-9]{1,10})?$";
  bcds: IBcdsInfo[];
  investmentRcvCommentFormData: InvestmentRcvComment = new InvestmentRcvComment();
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
  userRole: any;
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

  constructor(private accountService: AccountService, public rptInvDetailForService: RptInvDetailForSummaryService, private router: Router,

    private toastr: ToastrService, private modalService: BsModalService, private datePipe: DatePipe, private SpinnerService: NgxSpinnerService) { }
  ngOnInit() {
    var url_string = window.location.href
    var url = new URL(url_string);
    var v=url.pathname.split("/");
    this.convertedDate = this.datePipe.transform(this.today, 'ddMMyyyy');
    ///this.selectInvestmentInit(1);
    this.resetPageLoad()
    this.getEmployeeId();
    this.getDonation();
    this.GetData(v[3]);
    this.bsConfig = Object.assign({}, { containerClass: 'theme-blue'  }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();
  }

 
  getEmployeeLocation() {
    this.rptInvDetailForService.getEmpLoc(this.rptInvDetailForService.investmentInitFormData.id).subscribe(response => {
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
    this.rptInvDetailForService.investmentInitFormData.id = id;
    this.rptInvDetailForService.investmentRcvFormData.id = id;
    this.rptInvDetailForService.investmentDetailFormData.investmentInitId= id;
    //this.rptInvDetailForService.investmentInitFormData = Object.assign({}, selectedRecord);
    this.rptInvDetailForService.investmentDoctorFormData.investmentInitId = id;
    this.rptInvDetailForService.investmentInstitutionFormData.investmentInitId = id;
    this.rptInvDetailForService.investmentCampaignFormData.investmentInitId = id;
    this.rptInvDetailForService.investmentBcdsFormData.investmentInitId = id;
    this.rptInvDetailForService.investmentSocietyFormData.investmentInitId = id;
    this.rptInvDetailForService.investmentDetailFormData.investmentInitId = id;
    //this.isDonationValid = true;
    if (this.rptInvDetailForService.investmentInitFormData.donationTo == "Doctor") {
      //this.getDoctor();
      //this.getInstitution();
      this.getInvestmentDoctor();
    }
    else if (this.rptInvDetailForService.investmentInitFormData.donationTo == "Institution") {
      //this.getDoctor();
      //this.getInstitution();
      this.getInvestmentInstitution();
    }
    else if (this.rptInvDetailForService.investmentInitFormData.donationTo == "Campaign") {
      //this.getCampaignMst();
      //this.getDoctor();
      //this.getInstitution();
      this.getInvestmentCampaign();
    }
    else if (this.rptInvDetailForService.investmentInitFormData.donationTo == "Bcds") {
      //this.getBcds();
      this.getInvestmentBcds();
    }
    else if (this.rptInvDetailForService.investmentInitFormData.donationTo == "Society") {
      //this.getSociety();
      this.getInvestmentSociety();

    }
    this.getInvestmentDetails();
    this.getInvestmentTargetedProd();
    //this.getInvestmentTargetedGroup();
    this.getInvestmentTargetedGroupStatus();

    this.getinvestmentRcvComment();

    if (parseInt(this.empId) == this.rptInvDetailForService.investmentInitFormData.employeeId) {
      this.isInvOther = false;
      //this.isValid = true;
      // this.getInvestmentTargetedProd();
    }
    else {
      this.isInvOther = true;
      //this.isValid = false;
    }
    if (this.rptInvDetailForService.investmentInitFormData.confirmation==true) {
      this.isSubmitted = true;
      //this.isValid = true;
      // this.getInvestmentTargetedProd();
    }
    else {
      this.isSubmitted  = false;
      //this.isValid = false;
    }
  }

  
  
    getInvestmentInit(id: string) {
    this.rptInvDetailForService.getInvestmentInit(parseInt(id)).subscribe(response => {
      this.investmentInits = response.data;
      if (this.investmentInits.length>0) {
        this.rptInvDetailForService.investmentInitFormData.id =  this.investmentInits[0].id;
        this.rptInvDetailForService.investmentInitFormData.proposeFor =  this.investmentInits[0].proposeFor;
        this.rptInvDetailForService.investmentInitFormData.referenceNo =  this.investmentInits[0].referenceNo;
        this.rptInvDetailForService.investmentInitFormData.donationTo =  this.investmentInits[0].donationTo;
        this.rptInvDetailForService.investmentInitFormData.donationId =  this.investmentInits[0].donationId;
        
        if (this.rptInvDetailForService.investmentInitFormData.donationTo == "Doctor") {
          this.getDoctor();
        }
        else if (this.rptInvDetailForService.investmentInitFormData.donationTo == "Institution") {
          this.getInstitution();
        }
        else if (this.rptInvDetailForService.investmentInitFormData.donationTo == "Campaign") {
          this.getCampaignMst();
        }
        else if (this.rptInvDetailForService.investmentInitFormData.donationTo == "Bcds") {
          this.getBcds();
        }
        else if (this.rptInvDetailForService.investmentInitFormData.donationTo == "Society") {
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
    this.rptInvDetailForService.getInvestmentRcvComment(this.rptInvDetailForService.investmentRcvFormData.id).subscribe(response => {
      var data = response as IInvestmentRcvCommentRpt[];
      if (data !== undefined) {
        //this.rptInvDetailForService.investmentRcvCommentFormData = data;
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
    this.rptInvDetailForService.getInvestmentDetails(this.rptInvDetailForService.investmentDetailFormData.investmentInitId,parseInt(this.empId),this.userRole).subscribe(response => {
      var data = response[0] as IInvestmentDetail;
      if (data !== undefined) {
        debugger;
        this.rptInvDetailForService.investmentDetailFormData = data;
        this.rptInvDetailForService.investmentDetailFormData.fromDate = new Date(data.fromDate);
        this.rptInvDetailForService.investmentDetailFormData.toDate = new Date(data.toDate);
        this.rptInvDetailForService.investmentDetailFormData.commitmentFromDate = new Date(data.commitmentFromDate);
        this.rptInvDetailForService.investmentDetailFormData.commitmentToDate = new Date(data.commitmentToDate);
        this.convertedDate = this.datePipe.transform(data.fromDate, 'ddMMyyyy');
        //this.getLastFiveInvestment(this.rptInvDetailForService.investmentInitFormData.marketCode, this.convertedDate);

      } else {
       // this.toastr.warning('No Data Found', 'Investment');
      }
    }, error => {
      console.log(error);
    });
  }
  // getInvestmentCampaign() {
  //   this.rptInvDetailForService.getInvestmentCampaigns(this.rptInvDetailForService.investmentCampaignFormData.investmentInitId).subscribe(response => {
  //     var data = response[0] as IInvestmentCampaign;
  //     if (data !== undefined) {
  //       this.rptInvDetailForService.investmentCampaignFormData = data;

  //       this.rptInvDetailForService.investmentCampaignFormData.campaignMstId = data.campaignDtl.mstId;
  //       this.rptInvDetailForService.investmentCampaignFormData.subCampStartDate = new DatePipe('en-US').transform(data.campaignDtl.subCampStartDate, 'dd/MM/yyyy');
  //       this.rptInvDetailForService.investmentCampaignFormData.subCampEndDate = new DatePipe('en-US').transform(data.campaignDtl.subCampEndDate, 'dd/MM/yyyy');

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
    this.rptInvDetailForService.getInvestmentCampaigns(this.rptInvDetailForService.investmentCampaignFormData.investmentInitId).subscribe(response => {
      var data = response[0] as IInvestmentCampaign;
      if (data !== undefined) {
        this.rptInvDetailForService.investmentCampaignFormData = data;
        this.rptInvDetailForService.investmentCampaignFormData.campaignMstId = data.campaignDtl.mstId;
        //this.rptInvDetailForService.investmentCampaignFormData.subCampaignName = data.campaignDtl.subCampaignName;
        this.rptInvDetailForService.investmentCampaignFormData.doctorName = data.doctorInfo.doctorName;
        this.rptInvDetailForService.investmentCampaignFormData.institutionName = data.institutionInfo.institutionName;
        this.rptInvDetailForService.investmentCampaignFormData.subCampStartDate = new DatePipe('en-US').transform(data.campaignDtl.subCampStartDate, 'dd/MM/yyyy');
        this.rptInvDetailForService.investmentCampaignFormData.subCampEndDate = new DatePipe('en-US').transform(data.campaignDtl.subCampEndDate, 'dd/MM/yyyy')
        this.rptInvDetailForService.getCampaignMsts(data.campaignDtl.mstId).subscribe(response => {
          this.campaignMsts = response as ICampaignMst[];
          debugger;
          for (let i = 0; i < this.campaignMsts.length; i++) {
            if (this.campaignMsts[i].id == this.rptInvDetailForService.investmentCampaignFormData.campaignDtl.mstId) {
              this.rptInvDetailForService.investmentCampaignFormData.campaignName = this.campaignMsts[i].campaignName;
            }
          }
          this.rptInvDetailForService.getCampaignDtls(data.campaignDtl.mstId).subscribe(response => {
            this.campaignDtls = response as ICampaignDtl[];
            for (let i = 0; i < this.campaignDtls.length; i++) {
              if (this.campaignDtls[i].id == data.campaignDtl.id) {
                this.rptInvDetailForService.investmentCampaignFormData.subCampaignName = this.campaignDtls[i].subCampaign.subCampaignName;
              }
            }
          }, error => {
            console.log(error);
          });
        }, error => {
          console.log(error);
        });


        this.rptInvDetailForService.getCampaignDtlProducts(data.campaignDtl.id).subscribe(response => {
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
    if (this.rptInvDetailForService.investmentDetailFormData.fromDate != null && this.rptInvDetailForService.investmentDetailFormData.toDate != null) {
      if (this.rptInvDetailForService.investmentDetailFormData.toDate > this.rptInvDetailForService.investmentDetailFormData.fromDate) {
      }
      else {
        form.controls.fromDate.setValue(null);
        form.controls.toDate.setValue(null);
        this.toastr.error('Select Appropriate Date Range', 'Error');
      }
    }
  }

  getInvestmentBcds() {
    this.rptInvDetailForService.getInvestmentBcds(this.rptInvDetailForService.investmentRcvFormData.id).subscribe(response => {
      var data = response[0] as IInvestmentBcds;
      if (data !== undefined) {
        this.rptInvDetailForService.investmentBcdsFormData = data;
        this.rptInvDetailForService.investmentBcdsFormData.responsibleDoctorName = data.doctorInfo.doctorName;
        this.rptInvDetailForService.investmentBcdsFormData.bcdsName = data.bcds.bcdsName;
        this.rptInvDetailForService.investmentBcdsFormData.bcdsAddress = data.bcds.bcdsAddress;
        this.rptInvDetailForService.investmentBcdsFormData.noOfMember = data.bcds.noOfMember;
      }
      else {
       // this.toastr.warning('No Data Found', 'Investment ');
      }

    }, error => {
      console.log(error);
    });
  }
  getInvestmentSociety() {
    this.rptInvDetailForService.getInvestmentSociety(this.rptInvDetailForService.investmentRcvFormData.id).subscribe(response => {
      var data = response[0] as IInvestmentSociety;
      if (data !== undefined) {
        this.rptInvDetailForService.investmentSocietyFormData = data;
        this.rptInvDetailForService.investmentSocietyFormData.responsibleDoctorName = data.doctorInfo.doctorName;
        this.rptInvDetailForService.investmentSocietyFormData.societyName = data.society.societyName;
        this.rptInvDetailForService.investmentSocietyFormData.noOfMember = data.society.noOfMember;
        this.rptInvDetailForService.investmentSocietyFormData.societyAddress = data.society.societyAddress;
      }
      else {
        //this.toastr.warning('No Data Found', 'Investment ');
      }
    }, error => {
      console.log(error);
    });
  }
  getInvestmentInstitution() {
    this.rptInvDetailForService.getInvestmentInstitutions(this.rptInvDetailForService.investmentRcvFormData.id).subscribe(response => {
      var data = response[0] as IInvestmentInstitution;
      if (data !== undefined) {
        this.rptInvDetailForService.investmentInstitutionFormData = data;
        this.rptInvDetailForService.investmentInstitutionFormData.responsibleDoctorName = data.doctorInfo.doctorName;
        this.rptInvDetailForService.investmentInstitutionFormData.institutionName = data.institutionInfo.institutionName;
        this.rptInvDetailForService.investmentInstitutionFormData.address = data.institutionInfo.address;
        this.rptInvDetailForService.investmentInstitutionFormData.institutionType = data.institutionInfo.institutionType;
      }
      else {
        //this.toastr.warning('No Data Found', 'Investment ');
      }

    }, error => {
      console.log(error);
    });
  }
  getInvestmentDoctor() {
    this.rptInvDetailForService.getInvestmentDoctors(this.rptInvDetailForService.investmentRcvFormData.id).subscribe(response => {
      var data = response[0] as IInvestmentDoctor;
      if (data !== undefined) {
        this.rptInvDetailForService.investmentDoctorFormData = data;
        this.rptInvDetailForService.investmentDoctorFormData.doctorName = data.doctorInfo.doctorName;
        this.rptInvDetailForService.investmentDoctorFormData.degree = data.doctorInfo.degree;
        this.rptInvDetailForService.investmentDoctorFormData.designation = data.doctorInfo.designation;
        this.rptInvDetailForService.investmentDoctorFormData.institutionName = data.institutionInfo.institutionName;
        this.rptInvDetailForService.investmentDoctorFormData.address = data.institutionInfo.address;
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
    this.rptInvDetailForService.getInvestmentTargetedProds(this.rptInvDetailForService.investmentInitFormData.id, this.sbu).subscribe(response => {
      
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
  //   this.rptInvDetailForService.getInvestmentTargetedGroups(this.rptInvDetailForService.investmentInitFormData.id).subscribe(response => {

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
    this.rptInvDetailForService.getInvestmentTargetedGroupStatus(this.rptInvDetailForService.investmentInitFormData.id).subscribe(response => {
      var data = response as IInvestmentTargetedGroup[];
      //debugger;
      if (data !== undefined) {
        //debugger;
        this.investmentTargetedGroupss2 = data;
      }

    }, error => {
      console.log(error);
    });
  }
  getEmployeeId() {
    this.empId = this.accountService.getEmployeeId();
    this.userRole = this.accountService.getUserRole();
    this.rptInvDetailForService.investmentInitFormData.employeeId = parseInt(this.empId);
    this.getMarketGroupMsts();
    this.getEmployeeSbu();

  }
  getEmployeeSbu() {
    this.accountService.getEmployeeSbu(this.rptInvDetailForService.investmentInitFormData.employeeId).subscribe(
      (response) => {
        this.sbu = response.sbu;
        this.rptInvDetailForService.investmentInitFormData.marketCode = response.marketCode;
        this.marketCode= response.marketCode;
        this.getProduct();
        //this.getLastFiveInvestment(this.rptInvDetailForService.investmentInitFormData.marketCode, this.todayDate);
      },
      (error) => {
        console.log(error);
      }
    );
  }
  onChangeProposeFor() {
    // if (this.rptInvDetailForService.investmentInitFormData.proposeFor == "BrandCampaign") {
    //   this.rptInvDetailForService.investmentInitFormData.donationTo = "Campaign";
    //   this.onChangeDonationTo();
    //   this.isDonationValid = true;
    // }
    // else {
    //   this.isDonationValid = false;
    // }
    if(this.rptInvDetailForService.investmentInitFormData.proposeFor == "BrandCampaign" && this.rptInvDetailForService.investmentInitFormData.donationTo != "Campaign" && this.rptInvDetailForService.investmentInitFormData.donationTo != null)
    {
      this.toastr.warning("For Brand Campaign, must select Campaign");
      this.rptInvDetailForService.investmentInitFormData.donationTo =null;
      return false;
    }
    if(this.rptInvDetailForService.investmentInitFormData.proposeFor == "Others" && this.rptInvDetailForService.investmentInitFormData.donationTo == "Campaign" && this.rptInvDetailForService.investmentInitFormData.donationTo != null)
    {
      this.toastr.warning("For Campaign, must select Brand Campaign");
      this.rptInvDetailForService.investmentInitFormData.donationTo =null;
      return false;
    }
  }
  onChangeDonationTo() {
    if(this.rptInvDetailForService.investmentInitFormData.proposeFor == "BrandCampaign" && this.rptInvDetailForService.investmentInitFormData.donationTo != "Campaign" && this.rptInvDetailForService.investmentInitFormData.donationTo != null)
    {
      this.toastr.warning("For Brand Campaign, must select Campaign");
      this.rptInvDetailForService.investmentInitFormData.proposeFor =null;
      return false;
    }
    if(this.rptInvDetailForService.investmentInitFormData.proposeFor == "Others" && this.rptInvDetailForService.investmentInitFormData.donationTo == "Campaign" && this.rptInvDetailForService.investmentInitFormData.donationTo != null)
    {
      this.toastr.warning("For Campaign, must select Brand Campaign");
      this.rptInvDetailForService.investmentInitFormData.proposeFor =null;
      return false;
    }
    if (this.rptInvDetailForService.investmentInitFormData.donationTo == "Doctor") {
      if (this.rptInvDetailForService.investmentDoctorFormData.id == null || this.rptInvDetailForService.investmentDoctorFormData.id == undefined || this.rptInvDetailForService.investmentDoctorFormData.id == 0) {
        this.rptInvDetailForService.investmentDoctorFormData = new InvestmentDoctor();
        this.getDoctor();
        this.getInstitution();
      }
    }
    else if (this.rptInvDetailForService.investmentInitFormData.donationTo == "Institution") {
      if (this.rptInvDetailForService.investmentInstitutionFormData.id == null || this.rptInvDetailForService.investmentInstitutionFormData.id == undefined || this.rptInvDetailForService.investmentInstitutionFormData.id == 0) {
        this.rptInvDetailForService.investmentDoctorFormData = new InvestmentDoctor();
        this.getDoctor();
        this.getInstitution();
      }
    }
    else if (this.rptInvDetailForService.investmentInitFormData.donationTo == "Campaign") {
      if (this.rptInvDetailForService.investmentCampaignFormData.id == null || this.rptInvDetailForService.investmentCampaignFormData.id == undefined || this.rptInvDetailForService.investmentCampaignFormData.id == 0) {
        this.rptInvDetailForService.investmentCampaignFormData = new InvestmentCampaign();
        this.getCampaignMst();
        this.getDoctor();
        this.getInstitution();
      }
    }
    else if (this.rptInvDetailForService.investmentInitFormData.donationTo == "Bcds") {
      if (this.rptInvDetailForService.investmentBcdsFormData.id == null || this.rptInvDetailForService.investmentBcdsFormData.id == undefined || this.rptInvDetailForService.investmentBcdsFormData.id == 0) {
        this.rptInvDetailForService.investmentBcdsFormData = new InvestmentBcds();
        this.getBcds();
      }
    }
    else if (this.rptInvDetailForService.investmentInitFormData.donationTo == "Society") {
      if (this.rptInvDetailForService.investmentSocietyFormData.id == null || this.rptInvDetailForService.investmentSocietyFormData.id == undefined || this.rptInvDetailForService.investmentSocietyFormData.id == 0) {
        this.rptInvDetailForService.investmentSocietyFormData = new InvestmentSociety();
        this.getSociety();
      }
    }
    if (this.rptInvDetailForService.investmentInitFormData.id != null && this.rptInvDetailForService.investmentInitFormData.id != undefined && this.rptInvDetailForService.investmentInitFormData.id != 0) {
      this.rptInvDetailForService.investmentDoctorFormData.investmentInitId = this.rptInvDetailForService.investmentInitFormData.id;
      this.rptInvDetailForService.investmentInstitutionFormData.investmentInitId = this.rptInvDetailForService.investmentInitFormData.id;
      this.rptInvDetailForService.investmentCampaignFormData.investmentInitId = this.rptInvDetailForService.investmentInitFormData.id;
      this.rptInvDetailForService.investmentBcdsFormData.investmentInitId = this.rptInvDetailForService.investmentInitFormData.id;
      this.rptInvDetailForService.investmentSocietyFormData.investmentInitId = this.rptInvDetailForService.investmentInitFormData.id;
    }
  }
  onChangeDoctorInDoc() {
    for (var i = 0; i < this.doctors.length; i++) {
      if (this.doctors[i].id == this.rptInvDetailForService.investmentDoctorFormData.doctorId) {
        this.rptInvDetailForService.investmentDoctorFormData.doctorName=this.doctors[i].doctorName;
        //this.rptInvDetailForService.investmentDoctorFormData.doctorCode = this.doctors[i].doctorCode;
        this.rptInvDetailForService.investmentDoctorFormData.degree = this.doctors[i].degree;
        this.rptInvDetailForService.investmentDoctorFormData.designation = this.doctors[i].designation;

        break;
      }
    }
    //this.getLastFiveInvestment(this.rptInvDetailForService.investmentInitFormData.marketCode, this.convertedDate);
  }
  onChangeInstitutionInDoc() {
    for (var i = 0; i < this.institutions.length; i++) {
      if (this.institutions[i].id == this.rptInvDetailForService.investmentDoctorFormData.institutionId) {
        //this.rptInvDetailForService.investmentDoctorFormData.address = this.institutions[i].address;

        break;
      }
    }
  }
  onChangeInstitutionInInst() {
    for (var i = 0; i < this.institutions.length; i++) {
      if (this.institutions[i].id == this.rptInvDetailForService.investmentInstitutionFormData.institutionId) {
        this.rptInvDetailForService.investmentInstitutionFormData.address = this.institutions[i].address;
        this.rptInvDetailForService.investmentInstitutionFormData.institutionType = this.institutions[i].institutionType;

        break;
      }
    }
    //this.getLastFiveInvestment(this.rptInvDetailForService.investmentInitFormData.marketCode, this.convertedDate);
  }
  onChangeCampaignInCamp() {

    this.rptInvDetailForService.getCampaignDtls(this.rptInvDetailForService.investmentCampaignFormData.campaignMstId).subscribe(response => {
      this.campaignDtls = response as ICampaignDtl[];
    }, error => {
      console.log(error);
    });
  }
  onChangeBcdsInBcds() {
    for (var i = 0; i < this.bcds.length; i++) {
      if (this.bcds[i].id == this.rptInvDetailForService.investmentBcdsFormData.bcdsId) {
        this.rptInvDetailForService.investmentBcdsFormData.bcdsAddress = this.bcds[i].bcdsAddress;
        this.rptInvDetailForService.investmentBcdsFormData.noOfMember = this.bcds[i].noOfMember;
        break;
      }
    }
  }
  onChangeSubCampaignInCamp() {
    for (var i = 0; i < this.campaignDtls.length; i++) {
      if (this.campaignDtls[i].id == this.rptInvDetailForService.investmentCampaignFormData.campaignDtlId) {
        this.rptInvDetailForService.investmentCampaignFormData.subCampStartDate = new DatePipe('en-US').transform(this.campaignDtls[i].subCampStartDate, 'dd/MM/yyyy');
        this.rptInvDetailForService.investmentCampaignFormData.subCampEndDate = new DatePipe('en-US').transform(this.campaignDtls[i].subCampEndDate, 'dd/MM/yyyy');
        break;
      }
    }
    this.rptInvDetailForService.getCampaignDtlProducts(this.rptInvDetailForService.investmentCampaignFormData.campaignDtlId).subscribe(response => {
      this.campaignDtlProducts = response as ICampaignDtlProduct[];
    }, error => {
      console.log(error);
    });

  }
  onChangeSocietyInSociety() {
    for (var i = 0; i < this.society.length; i++) {
      if (this.society[i].id == this.rptInvDetailForService.investmentSocietyFormData.societyId) {
        this.rptInvDetailForService.investmentSocietyFormData.societyAddress = this.society[i].societyAddress;
        this.rptInvDetailForService.investmentSocietyFormData.noOfMember = this.society[i].noOfMember;

        break;
      }
    }
    //this.getLastFiveInvestment(this.rptInvDetailForService.investmentInitFormData.marketCode, this.convertedDate);
  }
  onChangeMarketGroupInTargetedGroup() {
    if (this.investmentTargetedGroups == null || this.investmentTargetedGroups.length == 0) {
      for (let i = 0; i < this.marketGroupMsts.length; i++) {
        if (this.marketGroupMsts[i].id == this.rptInvDetailForService.investmentTargetedGroupFormData.marketGroupMstId) {
          var data = [];
          for (let j = 0; j < this.marketGroupMsts[i].marketGroupDtls.length; j++) {
            if (this.marketGroupMsts[i].marketGroupDtls[j].status == 'Active') {
              var marketGroupMstId = this.marketGroupMsts[i].marketGroupDtls[j].mstId;
              var marketCode = this.marketGroupMsts[i].marketGroupDtls[j].marketCode;
              var marketName = this.marketGroupMsts[i].marketGroupDtls[j].marketName;
              var sbu = this.marketGroupMsts[i].marketGroupDtls[j].sbu;
              var sbuName = this.marketGroupMsts[i].marketGroupDtls[j].sbuName;

              data.push({ id: 0, investmentInitId: this.rptInvDetailForService.investmentInitFormData.id, marketGroupMst: this.marketGroupMsts[i], marketGroupMstId: marketGroupMstId, marketCode: marketCode, marketName: marketName,sbu:sbu,sbuName:sbuName });
              //this.investmentTargetedGroups.push({id:0,investmentInitId:this.rptInvDetailForService.investmentInitFormData.id,marketGroup:null,marketGroupMstId:this.marketGroupMsts[i].marketGroupDtls[j].mstId,marketCode:this.marketGroupMsts[i].marketGroupDtls[j].marketCode,marketName:this.marketGroupMsts[i].marketGroupDtls[j].marketName});
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
    if (this.rptInvDetailForService.investmentDetailFormData.fromDate == null || this.rptInvDetailForService.investmentDetailFormData.fromDate == undefined) {

      return false;
    }
    if (this.rptInvDetailForService.investmentDetailFormData.toDate == null || this.rptInvDetailForService.investmentDetailFormData.toDate == undefined) {

      return false;
    }
    let dateFrom = this.rptInvDetailForService.investmentDetailFormData.fromDate;
    let dateTo = this.rptInvDetailForService.investmentDetailFormData.toDate;
    //let dateFrom = new Date();
    //let dateTo = new Date();

    this.rptInvDetailForService.investmentDetailFormData.totalMonth = dateTo.getMonth() - dateFrom.getMonth() + (12 * (dateTo.getFullYear() - dateFrom.getFullYear()));
    this.rptInvDetailForService.investmentDetailFormData.totalMonth = this.rptInvDetailForService.investmentDetailFormData.totalMonth + 1;
  }
  getDonation() {
    this.rptInvDetailForService.getDonations().subscribe(response => {
      this.donations = response as IDonation[];
    }, error => {
      console.log(error);
    });
  }
  getSubCampaign() {
    this.rptInvDetailForService.getSubCampaigns().subscribe(response => {
      this.subCampaigns = response as ISubCampaign[];
    }, error => {
      console.log(error);
    });
  }
  getDoctor() {
    this.SpinnerService.show(); 
    this.rptInvDetailForService.getDoctors(this.rptInvDetailForService.investmentInitFormData.marketCode).subscribe(response => {
      this.doctors = response as IDoctor[];
      this.rptInvDetailForService.getInstitutions(this.rptInvDetailForService.investmentInitFormData.marketCode).subscribe(response => {
        this.institutions = response as IInstitution[];
        if (this.rptInvDetailForService.investmentInitFormData.id != null && this.rptInvDetailForService.investmentInitFormData.id != undefined && this.rptInvDetailForService.investmentInitFormData.id != 0) {
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
    this.rptInvDetailForService.getInstitutions(this.rptInvDetailForService.investmentInitFormData.marketCode).subscribe(response => {
      this.institutions = response as IInstitution[];
      this.rptInvDetailForService.getDoctors(this.rptInvDetailForService.investmentInitFormData.marketCode).subscribe(response => {
        this.doctors = response as IDoctor[];
        if (this.rptInvDetailForService.investmentInitFormData.id != null && this.rptInvDetailForService.investmentInitFormData.id != undefined && this.rptInvDetailForService.investmentInitFormData.id != 0) {
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
    this.rptInvDetailForService.getCampaignMsts(parseInt(this.empId)).subscribe(response => {
      this.campaignMsts = response as ICampaignMst[];
      this.rptInvDetailForService.getDoctors(this.rptInvDetailForService.investmentInitFormData.marketCode).subscribe(response => {
        this.doctors = response as IDoctor[];
        this.rptInvDetailForService.getInstitutions(this.rptInvDetailForService.investmentInitFormData.marketCode).subscribe(response => {
          this.institutions = response as IInstitution[];
          if (this.rptInvDetailForService.investmentInitFormData.id != null && this.rptInvDetailForService.investmentInitFormData.id != undefined && this.rptInvDetailForService.investmentInitFormData.id != 0) {
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
    this.rptInvDetailForService.getSociety().subscribe(response => {
      this.society = response as ISocietyInfo[];
      this.rptInvDetailForService.getDoctors(this.rptInvDetailForService.investmentInitFormData.marketCode).subscribe(response => {
        this.doctors = response as IDoctor[];
        if (this.rptInvDetailForService.investmentInitFormData.id != null && this.rptInvDetailForService.investmentInitFormData.id != undefined && this.rptInvDetailForService.investmentInitFormData.id != 0) {
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
    this.rptInvDetailForService.getBcds().subscribe(response => {
      this.bcds = response as IBcdsInfo[];
      this.rptInvDetailForService.getDoctors(this.rptInvDetailForService.investmentInitFormData.marketCode).subscribe(response => {
        this.doctors = response as IDoctor[];
        if (this.rptInvDetailForService.investmentInitFormData.id != null && this.rptInvDetailForService.investmentInitFormData.id != undefined && this.rptInvDetailForService.investmentInitFormData.id != 0) {
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
    this.rptInvDetailForService.getMarkets().subscribe(response => {
      this.markets = response as IMarket[];
      this.SpinnerService.hide();
    }, error => {
      this.SpinnerService.hide();
      console.log(error);
    });
  }
  getProduct() {
    this.rptInvDetailForService.getProduct(this.sbu).subscribe(response => {
      this.products = response as IProduct[];
    }, error => {
      console.log(error);
    });
  }
  getMarketGroupMsts() {
    this.rptInvDetailForService.getMarketGroupMsts(this.empId).subscribe(response => {
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
    //this.rptInvDetailForService.campaignFormData = Object.assign({}, selectedRecord);
  }
  resetPage(form: NgForm) {
    window.close();
    // form.reset();
    // this.rptInvDetailForService.investmentInitFormData = new InvestmentInit();
    // this.rptInvDetailForService.investmentInitFormData.marketCode = this.marketCode;
    // this.isValid = false;
    // this.isSubmitted = false;
    // this.isInvOther = false;
    // this.isDonationValid = false;
    // this.investmentTargetedGroups = [];
    // this.investmentTargetedProds = [];
    // this.investmentDetailsOld = [];
  }
  resetPageLoad() {
    this.rptInvDetailForService.investmentInitFormData = new InvestmentInit();
    this.rptInvDetailForService.investmentInitFormData.marketCode = this.marketCode;
    this.isValid = false;
    this.isSubmitted = false;
    this.isInvOther = false;
    this.isDonationValid = false;
    this.investmentTargetedGroups = [];
    this.investmentTargetedProds = [];
    this.investmentDetailsOld = [];
  }
  

}

