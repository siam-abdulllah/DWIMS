
import {InvestmentInit, IInvestmentInit, InvestmentDetail, IInvestmentDetail,InvestmentTargetedProd, IInvestmentTargetedProd,  IInvestmentDetailOld} from '../shared/models/investment';
import { IInvestmentRcvComment, InvestmentRcvComment, InvestmentTargetedGroup, IInvestmentTargetedGroup,} from '../shared/models/investmentRcv';
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
import { RptInvestmentDetailService } from '../_services/report-investdetail.service';
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

@Component({
  selector: 'app-investmentDetail',
  templateUrl: './rptInvestmentDetail.component.html',
  styles: [
  ],
  providers: [DatePipe]
})
export class RptInvestmentDetailComponent implements OnInit {
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

  constructor(private accountService: AccountService, public investmentInitService: RptInvestmentDetailService, private router: Router,

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

  openInvestmentInitSearchModal(template: TemplateRef<any>) {
    this.InvestmentInitSearchModalRef = this.modalService.show(template, this.config);
  }
  getEmployeeLocation() {
    this.investmentInitService.getEmpLoc(this.investmentInitService.investmentInitFormData.id).subscribe(response => {
      var data = response as IEmployeeLocation[];
      if (data !== undefined) {
        this.empLocation = data;
      }
      // else {
      //   this.toastr.warning('No Data Found', 'Investment ');
      // }
    }, error => {
      console.log(error);
    });
  }

  GetData(id)
  {
    this.getInvestmentInit(id);
    this.investmentInitService.investmentInitFormData.id = id;
    this.investmentInitService.investmentRcvFormData.id = id;
    this.investmentInitService.investmentDetailFormData.investmentInitId= id;
    //this.investmentInitService.investmentInitFormData = Object.assign({}, selectedRecord);
    this.investmentInitService.investmentDoctorFormData.investmentInitId = id;
    this.investmentInitService.investmentInstitutionFormData.investmentInitId = id;
    this.investmentInitService.investmentCampaignFormData.investmentInitId = id;
    this.investmentInitService.investmentBcdsFormData.investmentInitId = id;
    this.investmentInitService.investmentSocietyFormData.investmentInitId = id;
    this.investmentInitService.investmentDetailFormData.investmentInitId = id;
    //this.isDonationValid = true;
    if (this.investmentInitService.investmentInitFormData.donationTo == "Doctor") {
      //this.getDoctor();
      //this.getInstitution();
      this.getInvestmentDoctor();
    }
    else if (this.investmentInitService.investmentInitFormData.donationTo == "Institution") {
      //this.getDoctor();
      //this.getInstitution();
      this.getInvestmentInstitution();
    }
    else if (this.investmentInitService.investmentInitFormData.donationTo == "Campaign") {
      //this.getCampaignMst();
      //this.getDoctor();
      //this.getInstitution();
      this.getInvestmentCampaign();
    }
    else if (this.investmentInitService.investmentInitFormData.donationTo == "Bcds") {
      //this.getBcds();
      this.getInvestmentBcds();
    }
    else if (this.investmentInitService.investmentInitFormData.donationTo == "Society") {
      //this.getSociety();
      this.getInvestmentSociety();

    }
    this.getInvestmentDetails();
    this.getInvestmentTargetedProd();
    //this.getInvestmentTargetedGroup();
    this.getInvestmentTargetedGroupStatus();

    this.getinvestmentRcvComment();

    if (parseInt(this.empId) == this.investmentInitService.investmentInitFormData.employeeId) {
      this.isInvOther = false;
      //this.isValid = true;
      // this.getInvestmentTargetedProd();
    }
    else {
      this.isInvOther = true;
      //this.isValid = false;
    }
    if (this.investmentInitService.investmentInitFormData.confirmation==true) {
      this.isSubmitted = true;
      //this.isValid = true;
      // this.getInvestmentTargetedProd();
    }
    else {
      this.isSubmitted  = false;
      //this.isValid = false;
    }
  }

  
  // selectInvestmentInit(selectedRecord: IInvestmentInit) {
  //   this.investmentInitService.investmentInitFormData = Object.assign({}, selectedRecord);
  //   this.investmentInitService.investmentDoctorFormData.investmentInitId = selectedRecord.id;
  //   this.investmentInitService.investmentInstitutionFormData.investmentInitId = selectedRecord.id;
  //   this.investmentInitService.investmentCampaignFormData.investmentInitId = selectedRecord.id;
  //   this.investmentInitService.investmentBcdsFormData.investmentInitId = selectedRecord.id;
  //   this.investmentInitService.investmentSocietyFormData.investmentInitId = selectedRecord.id;
  //   this.investmentInitService.investmentDetailFormData.investmentInitId = selectedRecord.id;
  //   this.isDonationValid = true;
  //   if (this.investmentInitService.investmentInitFormData.donationTo == "Doctor") {
  //     this.getDoctor();
  //     //this.getInstitution();
  //     //this.getInvestmentDoctor();
  //   }
  //   else if (this.investmentInitService.investmentInitFormData.donationTo == "Institution") {
  //     //this.getDoctor();
  //     this.getInstitution();
  //     //this.getInvestmentInstitution();
  //   }
  //   else if (this.investmentInitService.investmentInitFormData.donationTo == "Campaign") {
  //     this.getCampaignMst();
  //     //this.getDoctor();
  //     //this.getInstitution();
  //     //this.getInvestmentCampaign();
  //   }
  //   else if (this.investmentInitService.investmentInitFormData.donationTo == "Bcds") {
  //     this.getBcds();
  //     //this.getInvestmentBcds();
  //   }
  //   else if (this.investmentInitService.investmentInitFormData.donationTo == "Society") {
  //     this.getSociety();

  //   }
  //   this.getInvestmentDetails();
  //   this.getInvestmentTargetedProd();
  //   //this.getInvestmentTargetedGroup();
  //   this.getInvestmentTargetedGroupStatus();
  //   this.getinvestmentRcvComment();

  //   if (parseInt(this.empId) == this.investmentInitService.investmentInitFormData.employeeId) {
  //     this.isInvOther = false;
  //     //this.isValid = true;
  //     // this.getInvestmentTargetedProd();
  //   }
  //   else {
  //     this.isInvOther = true;
  //     //this.isValid = false;
  //   }
  //   if (this.investmentInitService.investmentInitFormData.confirmation==true) {
  //     this.isSubmitted = true;
  //     //this.isValid = true;
  //     // this.getInvestmentTargetedProd();
  //   }
  //   else {
  //     this.isSubmitted  = false;
  //     //this.isValid = false;
  //   }
    
  //   this.isValid = true;
  //   this.InvestmentInitSearchModalRef.hide()
  // }

    getInvestmentInit(id: string) {

  
    this.investmentInitService.getInvestmentInit(parseInt(id)).subscribe(response => {
      this.investmentInits = response.data;
      //debugger;
      if (this.investmentInits.length>0) {
        this.investmentInitService.investmentInitFormData.id =  this.investmentInits[0].id;
        this.investmentInitService.investmentInitFormData.proposeFor =  this.investmentInits[0].proposeFor;
        this.investmentInitService.investmentInitFormData.referenceNo =  this.investmentInits[0].referenceNo;
        this.investmentInitService.investmentInitFormData.donationTo =  this.investmentInits[0].donationTo;
        this.investmentInitService.investmentInitFormData.donationId =  this.investmentInits[0].donationId;
        
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
          this.getCampaignMst();
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
        this.getEmployeeLocation()
      
      }
      // else {
      //   this.toastr.warning('No Data Found');
      // }
     }, error => {
      this.SpinnerService.hide();
         console.log(error);
    });
  }


  getinvestmentRcvComment() {
    this.investmentInitService.getInvestmentRcvComment(this.investmentInitService.investmentRcvFormData.id).subscribe(response => {
      var data = response[0] as IInvestmentRcvComment;
      if (data !== undefined) {
        this.investmentInitService.investmentRcvCommentFormData = data;
      }
      else {
        //this.toastr.warning('No Data Found', 'Investment ');
      }

    }, error => {
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
        this.convertedDate = this.datePipe.transform(data.fromDate, 'ddMMyyyy');
        //this.getLastFiveInvestment(this.investmentInitService.investmentInitFormData.marketCode, this.convertedDate);

      } else {
       // this.toastr.warning('No Data Found', 'Investment');
      }
    }, error => {
      console.log(error);
    });
  }
  // getInvestmentCampaign() {
  //   this.investmentInitService.getInvestmentCampaigns(this.investmentInitService.investmentCampaignFormData.investmentInitId).subscribe(response => {
  //     var data = response[0] as IInvestmentCampaign;
  //     if (data !== undefined) {
  //       this.investmentInitService.investmentCampaignFormData = data;

  //       this.investmentInitService.investmentCampaignFormData.campaignMstId = data.campaignDtl.mstId;
  //       this.investmentInitService.investmentCampaignFormData.subCampStartDate = new DatePipe('en-US').transform(data.campaignDtl.subCampStartDate, 'dd/MM/yyyy');
  //       this.investmentInitService.investmentCampaignFormData.subCampEndDate = new DatePipe('en-US').transform(data.campaignDtl.subCampEndDate, 'dd/MM/yyyy');

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
    this.investmentInitService.getInvestmentCampaigns(this.investmentInitService.investmentCampaignFormData.investmentInitId).subscribe(response => {
      var data = response[0] as IInvestmentCampaign;
      if (data !== undefined) {
        this.investmentInitService.investmentCampaignFormData = data;
        this.investmentInitService.investmentCampaignFormData.campaignMstId = data.campaignDtl.mstId;
        //this.investmentInitService.investmentCampaignFormData.subCampaignName = data.campaignDtl.subCampaignName;
        this.investmentInitService.investmentCampaignFormData.doctorName = data.doctorInfo.doctorName;
        this.investmentInitService.investmentCampaignFormData.institutionName = data.institutionInfo.institutionName;
        this.investmentInitService.investmentCampaignFormData.subCampStartDate = new DatePipe('en-US').transform(data.campaignDtl.subCampStartDate, 'dd/MM/yyyy');
        this.investmentInitService.investmentCampaignFormData.subCampEndDate = new DatePipe('en-US').transform(data.campaignDtl.subCampEndDate, 'dd/MM/yyyy')
        this.investmentInitService.getCampaignMsts(data.campaignDtl.mstId).subscribe(response => {
          this.campaignMsts = response as ICampaignMst[];
          debugger;
          for (let i = 0; i < this.campaignMsts.length; i++) {
            if (this.campaignMsts[i].id == this.investmentInitService.investmentCampaignFormData.campaignDtl.mstId) {
              this.investmentInitService.investmentCampaignFormData.campaignName = this.campaignMsts[i].campaignName;
            }
          }
          this.investmentInitService.getCampaignDtls(data.campaignDtl.mstId).subscribe(response => {
            this.campaignDtls = response as ICampaignDtl[];
            for (let i = 0; i < this.campaignDtls.length; i++) {
              if (this.campaignDtls[i].id == data.campaignDtl.id) {
                this.investmentInitService.investmentCampaignFormData.subCampaignName = this.campaignDtls[i].subCampaign.subCampaignName;
              }
            }
          }, error => {
            console.log(error);
          });
        }, error => {
          console.log(error);
        });


        this.investmentInitService.getCampaignDtlProducts(data.campaignDtl.id).subscribe(response => {
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
    if (this.investmentInitService.investmentDetailFormData.fromDate != null && this.investmentInitService.investmentDetailFormData.toDate != null) {
      if (this.investmentInitService.investmentDetailFormData.toDate > this.investmentInitService.investmentDetailFormData.fromDate) {
      }
      else {
        form.controls.fromDate.setValue(null);
        form.controls.toDate.setValue(null);
        this.toastr.error('Select Appropriate Date Range', 'Error');
      }
    }
  }

  getInvestmentBcds() {
    this.investmentInitService.getInvestmentBcds(this.investmentInitService.investmentRcvFormData.id).subscribe(response => {
      var data = response[0] as IInvestmentBcds;
      if (data !== undefined) {
        this.investmentInitService.investmentBcdsFormData = data;
        this.investmentInitService.investmentBcdsFormData.responsibleDoctorName = data.doctorInfo.doctorName;
        this.investmentInitService.investmentBcdsFormData.bcdsName = data.bcds.bcdsName;
        this.investmentInitService.investmentBcdsFormData.bcdsAddress = data.bcds.bcdsAddress;
        this.investmentInitService.investmentBcdsFormData.noOfMember = data.bcds.noOfMember;
      }
      else {
       // this.toastr.warning('No Data Found', 'Investment ');
      }

    }, error => {
      console.log(error);
    });
  }
  getInvestmentSociety() {
    this.investmentInitService.getInvestmentSociety(this.investmentInitService.investmentRcvFormData.id).subscribe(response => {
      var data = response[0] as IInvestmentSociety;
      if (data !== undefined) {
        this.investmentInitService.investmentSocietyFormData = data;
        this.investmentInitService.investmentSocietyFormData.responsibleDoctorName = data.doctorInfo.doctorName;
        this.investmentInitService.investmentSocietyFormData.societyName = data.society.societyName;
        this.investmentInitService.investmentSocietyFormData.noOfMember = data.society.noOfMember;
        this.investmentInitService.investmentSocietyFormData.societyAddress = data.society.societyAddress;
      }
      else {
        //this.toastr.warning('No Data Found', 'Investment ');
      }
    }, error => {
      console.log(error);
    });
  }
  getInvestmentInstitution() {
    this.investmentInitService.getInvestmentInstitutions(this.investmentInitService.investmentRcvFormData.id).subscribe(response => {
      var data = response[0] as IInvestmentInstitution;
      if (data !== undefined) {
        this.investmentInitService.investmentInstitutionFormData = data;
        this.investmentInitService.investmentInstitutionFormData.responsibleDoctorName = data.doctorInfo.doctorName;
        this.investmentInitService.investmentInstitutionFormData.institutionName = data.institutionInfo.institutionName;
        this.investmentInitService.investmentInstitutionFormData.address = data.institutionInfo.address;
        this.investmentInitService.investmentInstitutionFormData.institutionType = data.institutionInfo.institutionType;
      }
      else {
        //this.toastr.warning('No Data Found', 'Investment ');
      }

    }, error => {
      console.log(error);
    });
  }
  getInvestmentDoctor() {
    this.investmentInitService.getInvestmentDoctors(this.investmentInitService.investmentRcvFormData.id).subscribe(response => {
      var data = response[0] as IInvestmentDoctor;
      if (data !== undefined) {
        this.investmentInitService.investmentDoctorFormData = data;
        this.investmentInitService.investmentDoctorFormData.doctorName = data.doctorInfo.doctorName;
        this.investmentInitService.investmentDoctorFormData.degree = data.doctorInfo.degree;
        this.investmentInitService.investmentDoctorFormData.designation = data.doctorInfo.designation;
        this.investmentInitService.investmentDoctorFormData.institutionName = data.institutionInfo.institutionName;
        this.investmentInitService.investmentDoctorFormData.address = data.institutionInfo.address;
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
  // getInvestmentTargetedGroup() {
  //   this.investmentInitService.getInvestmentTargetedGroups(this.investmentInitService.investmentInitFormData.id).subscribe(response => {

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
    this.investmentInitService.getInvestmentTargetedGroupStatus(this.investmentInitService.investmentInitFormData.id).subscribe(response => {
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
    this.investmentInitService.investmentInitFormData.employeeId = parseInt(this.empId);
    this.getMarketGroupMsts();
    this.getEmployeeSbu();

  }
  getEmployeeSbu() {
    this.accountService.getEmployeeSbu(this.investmentInitService.investmentInitFormData.employeeId).subscribe(
      (response) => {
        this.sbu = response.sbu;
        this.investmentInitService.investmentInitFormData.marketCode = response.marketCode;
        this.marketCode= response.marketCode;
        this.getProduct();
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
    if(this.investmentInitService.investmentInitFormData.proposeFor == "BrandCampaign" && this.investmentInitService.investmentInitFormData.donationTo != "Campaign" && this.investmentInitService.investmentInitFormData.donationTo != null)
    {
      this.toastr.warning("For Brand Campaign, must select Campaign");
      this.investmentInitService.investmentInitFormData.donationTo =null;
      return false;
    }
    if(this.investmentInitService.investmentInitFormData.proposeFor == "Others" && this.investmentInitService.investmentInitFormData.donationTo == "Campaign" && this.investmentInitService.investmentInitFormData.donationTo != null)
    {
      this.toastr.warning("For Campaign, must select Brand Campaign");
      this.investmentInitService.investmentInitFormData.donationTo =null;
      return false;
    }
  }
  onChangeDonationTo() {
    if(this.investmentInitService.investmentInitFormData.proposeFor == "BrandCampaign" && this.investmentInitService.investmentInitFormData.donationTo != "Campaign" && this.investmentInitService.investmentInitFormData.donationTo != null)
    {
      this.toastr.warning("For Brand Campaign, must select Campaign");
      this.investmentInitService.investmentInitFormData.proposeFor =null;
      return false;
    }
    if(this.investmentInitService.investmentInitFormData.proposeFor == "Others" && this.investmentInitService.investmentInitFormData.donationTo == "Campaign" && this.investmentInitService.investmentInitFormData.donationTo != null)
    {
      this.toastr.warning("For Campaign, must select Brand Campaign");
      this.investmentInitService.investmentInitFormData.proposeFor =null;
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
        this.getCampaignMst();
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
        this.investmentInitService.investmentDoctorFormData.doctorName=this.doctors[i].doctorName;
        //this.investmentInitService.investmentDoctorFormData.doctorCode = this.doctors[i].doctorCode;
        this.investmentInitService.investmentDoctorFormData.degree = this.doctors[i].degree;
        this.investmentInitService.investmentDoctorFormData.designation = this.doctors[i].designation;

        break;
      }
    }
    //this.getLastFiveInvestment(this.investmentInitService.investmentInitFormData.marketCode, this.convertedDate);
  }
  onChangeInstitutionInDoc() {
    for (var i = 0; i < this.institutions.length; i++) {
      if (this.institutions[i].id == this.investmentInitService.investmentDoctorFormData.institutionId) {
        //this.investmentInitService.investmentDoctorFormData.address = this.institutions[i].address;

        break;
      }
    }
  }
  onChangeInstitutionInInst() {
    for (var i = 0; i < this.institutions.length; i++) {
      if (this.institutions[i].id == this.investmentInitService.investmentInstitutionFormData.institutionId) {
        this.investmentInitService.investmentInstitutionFormData.address = this.institutions[i].address;
        this.investmentInitService.investmentInstitutionFormData.institutionType = this.institutions[i].institutionType;

        break;
      }
    }
    //this.getLastFiveInvestment(this.investmentInitService.investmentInitFormData.marketCode, this.convertedDate);
  }
  onChangeCampaignInCamp() {

    this.investmentInitService.getCampaignDtls(this.investmentInitService.investmentCampaignFormData.campaignMstId).subscribe(response => {
      this.campaignDtls = response as ICampaignDtl[];
    }, error => {
      console.log(error);
    });
  }
  onChangeBcdsInBcds() {
    for (var i = 0; i < this.bcds.length; i++) {
      if (this.bcds[i].id == this.investmentInitService.investmentBcdsFormData.bcdsId) {
        this.investmentInitService.investmentBcdsFormData.bcdsAddress = this.bcds[i].bcdsAddress;
        this.investmentInitService.investmentBcdsFormData.noOfMember = this.bcds[i].noOfMember;
        break;
      }
    }
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
    //this.getLastFiveInvestment(this.investmentInitService.investmentInitFormData.marketCode, this.convertedDate);
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

              data.push({ id: 0, investmentInitId: this.investmentInitService.investmentInitFormData.id, marketGroupMst: this.marketGroupMsts[i], marketGroupMstId: marketGroupMstId, marketCode: marketCode, marketName: marketName,sbu:sbu,sbuName:sbuName });
              //this.investmentTargetedGroups.push({id:0,investmentInitId:this.investmentInitService.investmentInitFormData.id,marketGroup:null,marketGroupMstId:this.marketGroupMsts[i].marketGroupDtls[j].mstId,marketCode:this.marketGroupMsts[i].marketGroupDtls[j].marketCode,marketName:this.marketGroupMsts[i].marketGroupDtls[j].marketName});
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
  getCampaignMst() {
    this.SpinnerService.show();
    this.investmentInitService.getCampaignMsts(parseInt(this.empId)).subscribe(response => {
      this.campaignMsts = response as ICampaignMst[];
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
    this.investmentInitService.getProduct(this.sbu).subscribe(response => {
      this.products = response as IProduct[];
    }, error => {
      console.log(error);
    });
  }
  getMarketGroupMsts() {
    this.investmentInitService.getMarketGroupMsts(this.empId).subscribe(response => {
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
    //this.investmentInitService.campaignFormData = Object.assign({}, selectedRecord);
  }
  resetPage(form: NgForm) {
    window.close();
    // form.reset();
    // this.investmentInitService.investmentInitFormData = new InvestmentInit();
    // this.investmentInitService.investmentInitFormData.marketCode = this.marketCode;
    // this.isValid = false;
    // this.isSubmitted = false;
    // this.isInvOther = false;
    // this.isDonationValid = false;
    // this.investmentTargetedGroups = [];
    // this.investmentTargetedProds = [];
    // this.investmentDetailsOld = [];
  }
  resetPageLoad() {
    this.investmentInitService.investmentInitFormData = new InvestmentInit();
    this.investmentInitService.investmentInitFormData.marketCode = this.marketCode;
    this.isValid = false;
    this.isSubmitted = false;
    this.isInvOther = false;
    this.isDonationValid = false;
    this.investmentTargetedGroups = [];
    this.investmentTargetedProds = [];
    this.investmentDetailsOld = [];
  }
  

}

