import {
  InvestmentRec, IInvestmentRec, InvestmentInit, IInvestmentInit,
  InvestmentTargetedProd, IInvestmentTargetedProd, InvestmentTargetedGroup, IInvestmentTargetedGroup, IInvestmentRecComment
} from '../shared/models/investmentRec';
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
import { MarketGroupMst, IMarketGroupMst } from '../shared/models/marketGroupMst';
import { MarketGroupDtl, IMarketGroupDtl } from '../shared/models/marketGroupDtl';
import { AccountService } from '../account/account.service';
import { IInvestmentDetailOld } from '../shared/models/investment';
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
  // genParams: GenericParams;
  // campaigns: ICampaign[]; 
  investmentRecs: IInvestmentRec[];
  investmentTargetedProds: IInvestmentTargetedProd[];
  investmentTargetedGroups: IInvestmentTargetedGroup[];
  investmentDetails: IInvestmentRec[];
  investmentDoctors: IInvestmentDoctor[];
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
  totalCount = 0;
  bsConfig: Partial<BsDatepickerConfig>;
  bsValue: Date = new Date();
  empId: string;
  sbu: string;
  investmentDetailsOld: IInvestmentDetailOld[];
  config = {
    keyboard: false,
    class: 'modal-lg',
    ignoreBackdropClick: true
  };
  constructor(private accountService: AccountService, public investmentRecService: InvestmentRecService, private router: Router,
    private toastr: ToastrService, private modalService: BsModalService, private datePipe: DatePipe, private SpinnerService: NgxSpinnerService) { }

  openInvestmentInitSearchModal(template: TemplateRef<any>) {
    this.InvestmentInitSearchModalRef = this.modalService.show(template, this.config);
  }
  openInvestmentRecSearchModal(template: TemplateRef<any>) {
    this.InvestmentRecSearchModalRef = this.modalService.show(template, this.config);
  }
  selectInvestmentInit(selectedRecord: IInvestmentInit) {
    //
    this.investmentRecService.investmentRecFormData = Object.assign({}, selectedRecord);
    this.investmentRecService.investmentDetailFormData.investmentInitId = selectedRecord.id;
    this.investmentRecService.investmentRecCommentFormData.investmentInitId = selectedRecord.id;
    this.isDonationValid = true;
    if (this.investmentRecService.investmentRecFormData.donationTo == "Doctor") {

      this.getInvestmentDoctor();
    }
    else if (this.investmentRecService.investmentRecFormData.donationTo == "Institution") {

      this.getInvestmentInstitution();
    }
    else if (this.investmentRecService.investmentRecFormData.donationTo == "Campaign") {
      //this.getCampaignMst();
      this.getInvestmentCampaign();
    }
    else if (this.investmentRecService.investmentRecFormData.donationTo == "Bcds") {
      this.getInvestmentBcds();
    }
    else if (this.investmentRecService.investmentRecFormData.donationTo == "Society") {
      this.getInvestmentSociety();
    }

    this.getInvestmentDetails();
    this.getInvestmentTargetedProd();
    this.getInvestmentTargetedGroup();
    if (this.sbu == this.investmentRecService.investmentRecFormData.sbu) {
      this.isInvOther = false;
      this.isValid = true;
      // this.getInvestmentTargetedProd();
    }
    else {
      this.isInvOther = true;
      this.isValid = false;
    }
    //this.isValid=true;
    this.InvestmentInitSearchModalRef.hide()
  }
  selectInvestmentRec(selectedRecord: IInvestmentInit) {
    //
    this.investmentRecService.investmentRecFormData = Object.assign({}, selectedRecord);
    this.investmentRecService.investmentDetailFormData.investmentInitId = selectedRecord.id;
    this.investmentRecService.investmentRecCommentFormData.investmentInitId = selectedRecord.id;
    this.isDonationValid = true;
    if (this.investmentRecService.investmentRecFormData.donationTo == "Doctor") {

      this.getInvestmentDoctor();
    }
    else if (this.investmentRecService.investmentRecFormData.donationTo == "Institution") {

      this.getInvestmentInstitution();
    }
    else if (this.investmentRecService.investmentRecFormData.donationTo == "Campaign") {
      //this.getCampaignMst();
      this.getInvestmentCampaign();
    }
    else if (this.investmentRecService.investmentRecFormData.donationTo == "Bcds") {
      this.getInvestmentBcds();
    }
    else if (this.investmentRecService.investmentRecFormData.donationTo == "Society") {
      this.getInvestmentSociety();
    }
    this.getInvestmentRecDetails();
    this.getInvestmentRecProducts();
    this.getInvestmentRecComment();
    this.getInvestmentTargetedGroup();
    if (this.sbu == this.investmentRecService.investmentRecFormData.sbu) {
      this.isInvOther = false;
      this.isValid = true;
      // this.getInvestmentTargetedProd();
    }
    else {
      this.isInvOther = true;
      this.isValid = false;
    }
    this.InvestmentRecSearchModalRef.hide()
  }
  getLastFiveInvestment(marketCode: string, toDayDate: string) {
    debugger;
    if (this.investmentRecService.investmentRecFormData.donationTo == "Doctor") {
      this.investmentRecService.getLastFiveInvestmentForDoc(this.investmentRecService.investmentRecFormData.donationType,this.investmentRecService.investmentDoctorFormData.doctorId,marketCode, toDayDate).subscribe(
        (response) => {
          this.investmentDetailsOld = response as IInvestmentDetailOld[];
        },
        (error) => {
          console.log(error);
        }
      );
    }
    else if (this.investmentRecService.investmentRecFormData.donationTo == "Institution") {
      this.investmentRecService.getLastFiveInvestmentForInstitute(this.investmentRecService.investmentRecFormData.donationType,this.investmentRecService.investmentInstitutionFormData.institutionId,marketCode, toDayDate).subscribe(
        (response) => {
          this.investmentDetailsOld = response as IInvestmentDetailOld[];
        },
        (error) => {
          console.log(error);
        }
      );
    }
    else if (this.investmentRecService.investmentRecFormData.donationTo == "Campaign") {
      this.investmentRecService.getLastFiveInvestmentForCampaign(this.investmentRecService.investmentRecFormData.donationType,this.investmentRecService.investmentCampaignFormData.campaignMstId,marketCode, toDayDate).subscribe(
        (response) => {
          this.investmentDetailsOld = response as IInvestmentDetailOld[];
        },
        (error) => {
          console.log(error);
        }
      );
    }
    else if (this.investmentRecService.investmentRecFormData.donationTo == "Bcds") {
      this.investmentRecService.getLastFiveInvestmentForBcds(this.investmentRecService.investmentRecFormData.donationType,this.investmentRecService.investmentBcdsFormData.bcdsId,marketCode, toDayDate).subscribe(
        (response) => {
          this.investmentDetailsOld = response as IInvestmentDetailOld[];
        },
        (error) => {
          console.log(error);
        }
      );
    }
    else if (this.investmentRecService.investmentRecFormData.donationTo == "Society") {
      
      this.investmentRecService.getLastFiveInvestmentForSociety(this.investmentRecService.investmentRecFormData.donationType,this.investmentRecService.investmentSocietyFormData.societyId,marketCode, toDayDate).subscribe(
        (response) => {
          this.investmentDetailsOld = response as IInvestmentDetailOld[];
        },
        (error) => {
          console.log(error);
        }
      );
    }
    
  }
  // getLastFiveInvestment(marketCode: string, toDayDate: string) {
  //    this.investmentRecService.getLastFiveInvestment(marketCode, toDayDate).subscribe(
  //     (response) => {

  //       this.investmentDetailsOld = response as IInvestmentDetailOld[];
  //     },
  //     (error) => {
  //       console.log(error);
  //     }
  //   );
  // }

  getCampaignMst() {
    this.investmentRecService.getCampaignMsts().subscribe(response => {
      //
      this.campaignMsts = response as ICampaignMst[];
    }, error => {
      console.log(error);
    });
  }
  getInvestmentInit() {
    this.SpinnerService.show(); 
    this.investmentRecService.getInvestmentInit(parseInt(this.empId), this.sbu).subscribe(response => {
      this.SpinnerService.hide(); 
      this.investmentRecs = response.data;     
      if (this.investmentRecs.length>0) {
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
  getInvestmentRecommended() {
    this.SpinnerService.show(); 
    this.investmentRecService.getInvestmentRecommended(parseInt(this.empId), this.sbu).subscribe(response => {
      this.SpinnerService.hide();
      this.investmentRecs = response.data;
      if (this.investmentRecs.length>0) {
        this.openInvestmentRecSearchModal(this.investmentRecSearchModal);
      }
      else {
        this.toastr.warning('No Data Found');
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

  getInvestmentCampaign() {
    this.investmentRecService.getInvestmentCampaigns(this.investmentRecService.investmentRecFormData.id).subscribe(response => {


      var data = response[0] as IInvestmentCampaign;
      if (data !== undefined) {
        this.investmentRecService.investmentCampaignFormData = data;
        this.investmentRecService.investmentCampaignFormData.campaignMstId = data.campaignDtl.mstId;
        this.investmentRecService.investmentCampaignFormData.subCampaignName = data.campaignDtl.subCampaignName;
        this.investmentRecService.investmentCampaignFormData.doctorName = data.doctorInfo.doctorName;
        this.investmentRecService.investmentCampaignFormData.institutionName = data.institutionInfo.institutionName;
        this.investmentRecService.investmentCampaignFormData.subCampStartDate = new DatePipe('en-US').transform(data.campaignDtl.subCampStartDate, 'dd/MM/yyyy');
        this.investmentRecService.investmentCampaignFormData.subCampEndDate = new DatePipe('en-US').transform(data.campaignDtl.subCampEndDate, 'dd/MM/yyyy')
        this.investmentRecService.getCampaignMsts().subscribe(response => {
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
      else {
        this.toastr.warning('No Data Found', 'Investment ');
      }

    }, error => {
      console.log(error);
    });

  }
  getInvestmentBcds() {
    this.investmentRecService.getInvestmentBcds(this.investmentRecService.investmentRecFormData.id).subscribe(response => {
      //

      var data = response[0] as IInvestmentBcds;
      if (data !== undefined) {
        debugger;
        this.investmentRecService.investmentBcdsFormData = data; 
        this.investmentRecService.investmentBcdsFormData.responsibleDoctorName = data.doctorInfo.doctorName;
        this.investmentRecService.investmentBcdsFormData.bcdsName = data.bcds.bcdsName;
        this.investmentRecService.investmentBcdsFormData.bcdsAddress = data.bcds.bcdsAddress;
        this.investmentRecService.investmentBcdsFormData.noOfMember = data.bcds.noOfMember;
        //this.onChangeBcdsInBcds();
      }
      else {
        this.toastr.warning('No Data Found', 'Investment ');
      }

    }, error => {
      console.log(error);
    });
  }
  getInvestmentSociety() {
    this.investmentRecService.getInvestmentSociety(this.investmentRecService.investmentRecFormData.id).subscribe(response => {
      //

      var data = response[0] as IInvestmentSociety;
      if (data !== undefined) {
        this.investmentRecService.investmentSocietyFormData = data;
        this.investmentRecService.investmentSocietyFormData.responsibleDoctorName = data.doctorInfo.doctorName;
        this.investmentRecService.investmentSocietyFormData.societyName = data.society.societyName;
        this.investmentRecService.investmentSocietyFormData.noOfMember = data.society.noOfMember;
        this.investmentRecService.investmentSocietyFormData.societyAddress = data.society.societyAddress;

        //this.onChangeSocietyInSociety();
      }
      else {
        this.toastr.warning('No Data Found', 'Investment ');
      }

    }, error => {
      console.log(error);
    });
  }
  getInvestmentInstitution() {
    this.investmentRecService.getInvestmentInstitutions(this.investmentRecService.investmentRecFormData.id).subscribe(response => {
      //

      var data = response[0] as IInvestmentInstitution;
      if (data !== undefined) {
        this.investmentRecService.investmentInstitutionFormData = data;
        this.investmentRecService.investmentInstitutionFormData.responsibleDoctorName = data.doctorInfo.doctorName;
        this.investmentRecService.investmentInstitutionFormData.institutionName = data.institutionInfo.institutionName;
        this.investmentRecService.investmentInstitutionFormData.address = data.institutionInfo.address;
        this.investmentRecService.investmentInstitutionFormData.institutionType = data.institutionInfo.institutionType;
        //this.onChangeInstitutionInInst();
      }
      else {
        this.toastr.warning('No Data Found', 'Investment ');
      }

    }, error => {
      console.log(error);
    });
  }
  getInvestmentDoctor() {
    this.investmentRecService.getInvestmentDoctors(this.investmentRecService.investmentRecFormData.id).subscribe(response => {
      //
      var data = response[0] as IInvestmentDoctor;
      if (data !== undefined) {
        this.investmentRecService.investmentDoctorFormData = data;
        this.investmentRecService.investmentDoctorFormData.doctorName = data.doctorInfo.doctorName;
        this.investmentRecService.investmentDoctorFormData.degree = data.doctorInfo.degree;
        this.investmentRecService.investmentDoctorFormData.designation = data.doctorInfo.designation;
        this.investmentRecService.investmentDoctorFormData.institutionName = data.institutionInfo.institutionName;
        this.investmentRecService.investmentDoctorFormData.address = data.institutionInfo.address;
        //this.onChangeDoctorInDoc();
        //this.onChangeInstitutionInDoc();
      }
      else {
        this.toastr.warning('No Data Found', 'Investment ');
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
      else {
        this.toastr.warning('No Data Found', 'Investment ');
      }

    }, error => {
      console.log(error);
    });
  }
  getInvestmentDetails() {
    this.investmentRecService.getInvestmentDetails(this.investmentRecService.investmentRecFormData.id).subscribe(response => {
      //
      var data = response[0] as IInvestmentRec;
      if (data !== undefined) {
        this.investmentRecService.investmentDetailFormData = data;
        this.investmentRecService.investmentDetailFormData.id = 0;
        this.investmentRecService.investmentDetailFormData.fromDate = new Date(data.fromDate);
        this.investmentRecService.investmentDetailFormData.toDate = new Date(data.toDate);
        let convertedDate = this.datePipe.transform(data.fromDate, 'ddMMyyyy');
        this.getLastFiveInvestment(this.investmentRecService.investmentRecFormData.marketCode, convertedDate);


      } else {
        this.toastr.warning('No Data Found', 'Investment ');
      }
    }, error => {
      console.log(error);
    });
  }
  getInvestmentTargetedProd() {
    this.investmentRecService.getInvestmentTargetedProds(this.investmentRecService.investmentRecFormData.id, this.sbu).subscribe(response => {
      //
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
  getInvestmentRecDetails() {
    //
    this.investmentRecService.getInvestmentRecDetails(this.investmentRecService.investmentRecFormData.id).subscribe(response => {

      var data = response[0] as IInvestmentRec;
      if (data !== undefined) {
        this.investmentRecService.investmentDetailFormData = data;
        this.investmentRecService.investmentDetailFormData.id = 0;
        this.investmentRecService.investmentDetailFormData.fromDate = new Date(data.fromDate);
        this.investmentRecService.investmentDetailFormData.toDate = new Date(data.toDate);
        let convertedDate = this.datePipe.transform(data.fromDate, 'ddMMyyyy');
        this.getLastFiveInvestment(this.investmentRecService.investmentRecFormData.marketCode, convertedDate);

      } else {
        //this.toastr.warning('No Data Found', 'Investment ');
        this.getInvestmentDetails();
      }
    }, error => {
      console.log(error);
    });
  }
  getInvestmentRecProducts() {
    //
    this.investmentRecService.getInvestmentRecProducts(this.investmentRecService.investmentRecFormData.id, this.sbu).subscribe(response => {

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
    this.investmentRecService.getInvestmentTargetedGroups(this.investmentRecService.investmentRecFormData.id).subscribe(response => {
      //
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

  ngOnInit() {
    this.resetForm();
    this.getEmployeeId();
    //this.getProduct();
    //this.getMarketGroupMsts();
    this.bsConfig = Object.assign({}, { containerClass: 'theme-blue'  }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();

  }

  changeDateInDetail() {
    //

    //this.printingDate=this.getDigitBanglaFromEnglish(this.datePipe.transform(value, "dd/MM/yyyy"));
    if (this.investmentRecService.investmentDetailFormData.fromDate == null || this.investmentRecService.investmentDetailFormData.fromDate == undefined) {

      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.toDate == null || this.investmentRecService.investmentDetailFormData.toDate == undefined) {

      return false;
    }
    let dateFrom = this.investmentRecService.investmentDetailFormData.fromDate;
    let dateTo = this.investmentRecService.investmentDetailFormData.toDate;
    //let dateFrom = new Date();
    //let dateTo = new Date();

    this.investmentRecService.investmentDetailFormData.totalMonth = dateTo.getMonth() - dateFrom.getMonth() + (12 * (dateTo.getFullYear() - dateFrom.getFullYear()));
    this.investmentRecService.investmentDetailFormData.totalMonth = this.investmentRecService.investmentDetailFormData.totalMonth + 1;
  }

  getProduct() {
    this.investmentRecService.getProduct(this.sbu).subscribe(response => {
      //
      this.products = response as IProduct[];
    }, error => {
      console.log(error);
    });
  }
  getEmployeeId() {
    //
    this.empId = this.accountService.getEmployeeId();
    this.investmentRecService.investmentRecCommentFormData.employeeId = parseInt(this.empId);
    this.getEmployeeSbu();
  }
  getEmployeeSbu() {
    //
    this.accountService.getEmployeeSbu(this.investmentRecService.investmentRecCommentFormData.employeeId).subscribe(
      (response) => {
        //
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
    if (this.investmentRecService.investmentDetailFormData.proposedAmount == null || this.investmentRecService.investmentDetailFormData.proposedAmount == undefined || this.investmentRecService.investmentDetailFormData.proposedAmount == "") {
      this.toastr.warning('Enter Proposed Amount First', 'Investment ', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.purpose == null || this.investmentRecService.investmentDetailFormData.purpose == undefined || this.investmentRecService.investmentDetailFormData.purpose == "") {
      this.toastr.warning('Enter Purpose First', 'Investment ', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.fromDate == null || this.investmentRecService.investmentDetailFormData.fromDate == undefined) {
      this.toastr.warning('Select From Date  First', 'Investment ', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.toDate == null || this.investmentRecService.investmentDetailFormData.toDate == undefined) {
      this.toastr.warning('Select To Date  First', 'Investment ', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.commitmentAllSBU == null || this.investmentRecService.investmentDetailFormData.commitmentAllSBU == undefined || this.investmentRecService.investmentDetailFormData.commitmentAllSBU == "") {
      this.toastr.warning('Enter Commitment All SBU First', 'Investment ', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.commitmentOwnSBU == null || this.investmentRecService.investmentDetailFormData.commitmentOwnSBU == undefined || this.investmentRecService.investmentDetailFormData.commitmentOwnSBU == "") {
      this.toastr.warning('Enter Commitment Own SBU First', 'Investment ', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.paymentMethod == null || this.investmentRecService.investmentDetailFormData.paymentMethod == undefined || this.investmentRecService.investmentDetailFormData.paymentMethod == "") {
      this.toastr.warning('Select Payment Method First', 'Investment ', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    this.investmentRecService.insertInvestmentRec().subscribe(
      res => {
        //
        this.investmentRecService.investmentRecCommentFormData = res as IInvestmentRecComment;
        //this.investmentRecService.investmentDoctorFormData.investmentInitId=this.investmentRecService.investmentRecFormData.id;
        //this.investmentRecService.investmentInstitutionFormData.investmentInitId=this.investmentRecService.investmentRecFormData.id;
        this.isValid = true;
        if (this.sbu == this.investmentRecService.investmentRecFormData.sbu) 
        { 
          this.insertInvestmentDetails();
         }
        this.insertInvestmentTargetedProd();
        this.toastr.success('Save successfully', 'Investment')
      },
      err => { 
        console.log(err); 
      }
    );
  }
  updateInvestmentRec() {
    if (this.investmentRecService.investmentDetailFormData.proposedAmount == null || this.investmentRecService.investmentDetailFormData.proposedAmount == undefined || this.investmentRecService.investmentDetailFormData.proposedAmount == "") {
      this.toastr.warning('Enter Proposed Amount First', 'Investment ', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.purpose == null || this.investmentRecService.investmentDetailFormData.purpose == undefined || this.investmentRecService.investmentDetailFormData.purpose == "") {
      this.toastr.warning('Enter Purpose First', 'Investment ', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.fromDate == null || this.investmentRecService.investmentDetailFormData.fromDate == undefined) {
      this.toastr.warning('Select From Date  First', 'Investment ', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.toDate == null || this.investmentRecService.investmentDetailFormData.toDate == undefined) {
      this.toastr.warning('Select To Date  First', 'Investment ', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.commitmentAllSBU == null || this.investmentRecService.investmentDetailFormData.commitmentAllSBU == undefined || this.investmentRecService.investmentDetailFormData.commitmentAllSBU == "") {
      this.toastr.warning('Enter Commitment All SBU First', 'Investment ', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.commitmentOwnSBU == null || this.investmentRecService.investmentDetailFormData.commitmentOwnSBU == undefined || this.investmentRecService.investmentDetailFormData.commitmentOwnSBU == "") {
      this.toastr.warning('Enter Commitment Own SBU First', 'Investment ', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    if (this.investmentRecService.investmentDetailFormData.paymentMethod == null || this.investmentRecService.investmentDetailFormData.paymentMethod == undefined || this.investmentRecService.investmentDetailFormData.paymentMethod == "") {
      this.toastr.warning('Select Payment Method First', 'Investment ', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    this.investmentRecService.updateInvestmentRec().subscribe(
      res => {
        //
        this.isValid = true;
        this.investmentRecService.investmentRecCommentFormData = res as IInvestmentRecComment;
        if (this.sbu == this.investmentRecService.investmentRecFormData.sbu) { this.insertInvestmentDetails(); }
        this.insertInvestmentTargetedProd();
        //this.investmentRecService.investmentDoctorFormData.investmentInitId=this.investmentRecService.investmentRecFormData.id;
        // this.investmentRecService.investmentInstitutionFormData.investmentInitId=this.investmentRecService.investmentRecFormData.id;
        this.toastr.info('Updated successfully', 'Investment ')
      },
      err => { console.log(err); }
    );
  }
  insertInvestmentDetails() {
    if (this.investmentRecService.investmentRecFormData.id == null || this.investmentRecService.investmentRecFormData.id == undefined || this.investmentRecService.investmentRecFormData.id == 0) {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment ', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    


    this.investmentRecService.investmentDetailFormData.investmentInitId = this.investmentRecService.investmentRecFormData.id;

    //if(this.investmentRecService.investmentDetailFormData.id==null || this.investmentRecService.investmentDetailFormData.id==undefined || this.investmentRecService.investmentDetailFormData.id==0)
    //{
    this.investmentRecService.insertInvestmentDetail(parseInt(this.empId), this.sbu).subscribe(
      res => {
        var data = res as IInvestmentRec;
        this.investmentRecService.investmentDetailFormData = data;
        //this.investmentRecService.investmentDoctorFormData.doctorName=String(data.doctorId);
        this.investmentRecService.investmentDetailFormData.fromDate = new Date(data.fromDate);
        this.investmentRecService.investmentDetailFormData.toDate = new Date(data.toDate);
        this.isDonationValid = true;
        this.toastr.success('Investment Details Save successfully', 'Investment Details');
      },
      err => { console.log(err); }
    );
    // }
    // else{
    //   this.investmentRecService.updateInvestmentDetail().subscribe(
    //     res => {
    //      var data=res as IInvestmentRec;
    //      this.investmentRecService.investmentDetailFormData=data;
    //      //this.investmentRecService.investmentDoctorFormData.doctorName=String(data.doctorId);
    //      this.investmentRecService.investmentDetailFormData.fromDate=new Date(data.fromDate);
    //     this.investmentRecService.investmentDetailFormData.toDate=new Date(data.toDate);
    //      this.isDonationValid=true;
    //      this.toastr.success('Save successfully', 'Investment ');
    //     },
    //     err => { console.log(err); }
    //   );
    // }

  }



  insertInvestmentTargetedProd() {
    //
    if (this.investmentRecService.investmentRecFormData.id == null || this.investmentRecService.investmentRecFormData.id == undefined || this.investmentRecService.investmentRecFormData.id == 0) {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment ', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    // if(this.investmentRecService.investmentDetailFormData.id==null || this.investmentRecService.investmentDetailFormData.id==undefined || this.investmentRecService.investmentDetailFormData.id==0)
    // {
    //   this.toastr.warning('Insert Investment Detail First', 'Investment ', {
    //     positionClass: 'toast-top-right' 
    //  });
    //  return false;
    // }

    // if(this.investmentRecService.investmentTargetedProdFormData.productId==null || this.investmentRecService.investmentTargetedProdFormData.productId==undefined || this.investmentRecService.investmentTargetedProdFormData.productId==0)
    // {
    //   this.toastr.warning('Select Product First', 'Investment ', {
    //     positionClass: 'toast-top-right' 
    //  });
    //  return false;
    // }
    debugger;
    if (this.investmentTargetedProds == undefined || this.investmentTargetedProds.length==0) {
      // this.toastr.warning('Select Product First', 'Investment ', {
      //   positionClass: 'toast-top-right'
      // });
      return false;
    }
    this.investmentRecService.investmentTargetedProdFormData.investmentInitId = this.investmentRecService.investmentRecFormData.id;
    //if(this.investmentRecService.investmentTargetedProdFormData.id==null || this.investmentRecService.investmentTargetedProdFormData.id==undefined || this.investmentRecService.investmentTargetedProdFormData.id==0)
    //{
    this.investmentRecService.insertInvestmentTargetedProd(this.investmentTargetedProds).subscribe(
      res => {
        //
        //this.investmentRecService.investmentTargetedProdFormData=new InvestmentTargetedProd();

        this.getInvestmentTargetedProd();

        this.isDonationValid = true;
        this.toastr.success('Targeted Product Save successfully', 'Investment Targeted Product');
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
          data.employeeId = parseInt(this.empId);
          data.investmentInitId = this.investmentRecService.investmentRecFormData.id;
          data.productId = this.investmentRecService.investmentTargetedProdFormData.productId;
          data.productInfo = this.products[i];
          //data.productInfo.push({ id: this.products[i].id, productName: this.products[i].productName,productCode: this.products[i].productCode});

          //data.productInfo.productName=this.products[i].productName;
          //data.productInfo.productCode=this.products[i].productCode;
          this.investmentTargetedProds.push(data);
          return false;
        }

      }


      // this.investmentTargetedProds.push(      
      //   { id: 0, investmentInitId: this.investmentRecService.investmentRecFormData.id,productId:0 });
    }

  }

  editInvestmentTargetedProd(selectedRecord: IInvestmentTargetedProd) {
    this.investmentRecService.investmentTargetedProdFormData = Object.assign({}, selectedRecord);
    // var e = (document.getElementById("marketCode")) as HTMLSelectElement;
    // var sel = e.selectedIndex;
    // var opt = e.options[sel];
    // var selectedMarketCode = opt.value;
    // var selectedMarketName = opt.innerHTML;

  }
  populateForm() {
    //this.investmentRecService.campaignFormData = Object.assign({}, selectedRecord);
  }
  resetPage(form: NgForm) {
    form.reset();
    this.investmentRecService.investmentRecFormData = new InvestmentInit();
    this.investmentTargetedProds = [];
    this.investmentTargetedGroups = [];
    this.isValid = false;
    this.isInvOther = false;
  }
  resetForm() {
    this.investmentRecService.investmentRecFormData = new InvestmentInit();
    this.investmentTargetedProds = [];
    this.investmentTargetedGroups = [];
    this.isValid = false;
    this.isInvOther = false;
  }

  removeInvestmentTargetedProd(selectedRecord: IInvestmentTargetedProd) {
    if (this.investmentTargetedProds.find(x => x.productId == selectedRecord.productId)) {
      this.investmentTargetedProds.splice(this.investmentTargetedProds.findIndex(x => x.productId == selectedRecord.productId), 1);
    }
    if (this.investmentRecService.investmentRecCommentFormData.id == null || this.investmentRecService.investmentRecCommentFormData.id == undefined || this.investmentRecService.investmentRecCommentFormData.id == 0) {
      return false;
    }
    this.investmentRecService.investmentTargetedProdFormData = Object.assign({}, selectedRecord);
    var c = confirm("Are you sure you want to delete that?");
    if (c == true) {
      this.investmentRecService.removeInvestmentTargetedProd().subscribe(
        res => {
          //
          this.toastr.success(res);
          //this.isDonationValid=false;
          this.investmentRecService.investmentTargetedProdFormData = new InvestmentTargetedProd();
          this.getInvestmentTargetedProd();
        },
        err => { console.log(err); }
      );
    }
  }
}

