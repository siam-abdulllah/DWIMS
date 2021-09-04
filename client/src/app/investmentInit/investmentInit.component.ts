
import {
  InvestmentInit, IInvestmentInit, InvestmentDetail, IInvestmentDetail,
  InvestmentTargetedProd, IInvestmentTargetedProd, InvestmentTargetedGroup, IInvestmentTargetedGroup, IInvestmentDetailOld
} from '../shared/models/investment';
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
import { FormGroup,NgForm } from '@angular/forms';
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
  InvestmentInitSearchModalRef: BsModalRef;
  empId: string;
  sbu: string;
  investmentInits: IInvestmentInit[];
  investmentTargetedProds: IInvestmentTargetedProd[];
  investmentTargetedGroups: IInvestmentTargetedGroup[];
  investmentDetailsOld: IInvestmentDetailOld[];
  investmentDoctors: IInvestmentDoctor[];
  isValid: boolean = false;
  isInvOther: boolean = false;
  isDonationValid: boolean = false;
  investmentInitForm: NgForm;
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
  constructor(private accountService: AccountService, public investmentInitService: InvestmentInitService, private router: Router,
    private toastr: ToastrService, private modalService: BsModalService, private datePipe: DatePipe) { }
  ngOnInit() {
   // this.resetPage(this.investmentInitForm)
    this.getEmployeeId();
    this.getDonation();
    //this.getProduct();
    //this.getMarketGroupMsts();
    this.bsConfig = Object.assign({}, { containerClass: 'theme-green' }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();
  }
  openInvestmentInitSearchModal(template: TemplateRef<any>) {
    this.InvestmentInitSearchModalRef = this.modalService.show(template, this.config);
  }
  selectInvestmentInit(selectedRecord: IInvestmentInit) {
    this.investmentInitService.investmentInitFormData = Object.assign({}, selectedRecord);
    this.investmentInitService.investmentDoctorFormData.investmentInitId = selectedRecord.id;
    this.investmentInitService.investmentInstitutionFormData.investmentInitId = selectedRecord.id;
    this.investmentInitService.investmentCampaignFormData.investmentInitId = selectedRecord.id;
    this.investmentInitService.investmentBcdsFormData.investmentInitId = selectedRecord.id;
    this.investmentInitService.investmentSocietyFormData.investmentInitId = selectedRecord.id;
    this.investmentInitService.investmentDetailFormData.investmentInitId = selectedRecord.id;
    this.isDonationValid = true;
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
      this.getInvestmentSociety();
    }
    this.getInvestmentDetails();
    this.getInvestmentTargetedProd();
    this.getInvestmentTargetedGroup();
    debugger;
    if(parseInt(this.empId)==this.investmentInitService.investmentInitFormData.employeeId)
    {
    this.isInvOther=false;
    this.isValid = true;
   // this.getInvestmentTargetedProd();
    }
    else{
      this.isInvOther=true;
      this.isValid = false;
    }
    this.isValid = true;
    this.InvestmentInitSearchModalRef.hide()
  }
  getInvestmentInit() {
    debugger;
    this.investmentInitService.getInvestmentInit(parseInt(this.empId), this.sbu).subscribe(response => {
      this.investmentInits = response.data;
      this.openInvestmentInitSearchModal(this.investmentInitSearchModal);
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
        let convertedDate = this.datePipe.transform(data.fromDate, 'ddMMyyyy');
        this.getLastFiveInvestment(this.investmentInitService.investmentInitFormData.marketCode, convertedDate);

      } else {
        this.toastr.warning('No Data Found', 'Investment');
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

        this.investmentInitService.investmentCampaignFormData.campaignMstId = data.campaignDtl.mstId;
        this.investmentInitService.investmentCampaignFormData.subCampStartDate = new DatePipe('en-US').transform(data.campaignDtl.subCampStartDate, 'dd/MM/yyyy');
        this.investmentInitService.investmentCampaignFormData.subCampEndDate = new DatePipe('en-US').transform(data.campaignDtl.subCampEndDate, 'dd/MM/yyyy');

        this.onChangeCampaignInCamp();
        this.onChangeSubCampaignInCamp();
      }
      else {
        this.toastr.warning('No Data Found', 'Investment');
      }

    }, error => {
      console.log(error);
    });
  }
  getInvestmentBcds() {
    this.investmentInitService.getInvestmentBcds(this.investmentInitService.investmentBcdsFormData.investmentInitId).subscribe(response => {
      var data = response[0] as IInvestmentBcds;
      if (data !== undefined) {
        this.investmentInitService.investmentBcdsFormData = data;
        this.onChangeBcdsInBcds();
      }
      else {
        this.toastr.warning('No Data Found', 'Investment');
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
      }
      else {
        this.toastr.warning('No Data Found', 'Investment');
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
      }
      else {
        this.toastr.warning('No Data Found', 'Investment');
      }

    }, error => {
      console.log(error);
    });
  }
  getInvestmentDoctor() {
    this.investmentInitService.getInvestmentDoctors(this.investmentInitService.investmentDoctorFormData.investmentInitId).subscribe(response => {

      var data = response[0] as IInvestmentDoctor;
      if (data !== undefined) {
        debugger;
        this.investmentInitService.investmentDoctorFormData = data;
        //this.investmentInitService.investmentDoctorFormData.doctorName = String(data.doctorId);
        this.onChangeDoctorInDoc();
        this.onChangeInstitutionInDoc();
      }
      else {
        this.toastr.warning('No Data Found', 'Investment');
      }

    }, error => {
      console.log(error);
    });
  }
  getInvestmentTargetedProd() {
    this.investmentInitService.getInvestmentTargetedProds(this.investmentInitService.investmentInitFormData.id,this.sbu).subscribe(response => {

      var data = response as IInvestmentTargetedProd[];
      if (data !== undefined) {
        this.investmentTargetedProds = data;

      }
      else {
        this.toastr.warning('No Data Found', 'Investment');
      }

    }, error => {
      console.log(error);
      
    });
  }
  getInvestmentTargetedGroup() {
    this.investmentInitService.getInvestmentTargetedGroups(this.investmentInitService.investmentInitFormData.id).subscribe(response => {

      var data = response as IInvestmentTargetedGroup[];
      if (data !== undefined) {
        this.investmentTargetedGroups = data;

      }
      else {
        this.toastr.warning('No Data Found', 'Investment');
      }

    }, error => {
      console.log(error);
    });
  }

  getEmployeeId() {

    this.empId = this.accountService.getEmployeeId();
    this.investmentInitService.investmentInitFormData.employeeId = parseInt(this.empId);

    this.getMarketGroupMsts();
    this.getEmployeeSbu();

  }
  getLastFiveInvestment(marketCode: string, toDayDate: string) {
    this.investmentInitService.getLastFiveInvestment(marketCode, toDayDate).subscribe(
      (response) => {
        this.investmentDetailsOld = response as IInvestmentDetailOld[];
      },
      (error) => {
        console.log(error);
      }
    );
  }

  getEmployeeSbu() {
    this.accountService.getEmployeeSbu(this.investmentInitService.investmentInitFormData.employeeId).subscribe(
      (response) => {
        this.sbu = response.sbu;
        this.investmentInitService.investmentInitFormData.marketCode = response.marketCode;
        this.getProduct();
        this.getLastFiveInvestment(this.investmentInitService.investmentInitFormData.marketCode, this.todayDate);
      },
      (error) => {
        console.log(error);
      }
    );
  }
  onChangeProposeFor() {
    if (this.investmentInitService.investmentInitFormData.proposeFor == "BrandCampaign") {
      this.investmentInitService.investmentInitFormData.donationTo = "Campaign";
      this.onChangeDonationTo();
      this.isDonationValid = true;
    }
    else {
      this.isDonationValid = false;
    }
  }
  onChangeDonationTo() {
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
      debugger;
      if (this.doctors[i].id == this.investmentInitService.investmentDoctorFormData.doctorId) {
        //this.investmentInitService.investmentDoctorFormData.doctorName=this.doctors[i].doctorName;
        this.investmentInitService.investmentDoctorFormData.doctorCode = this.doctors[i].doctorCode;
        this.investmentInitService.investmentDoctorFormData.degree = this.doctors[i].degree;
        this.investmentInitService.investmentDoctorFormData.designation = this.doctors[i].designation;
        break;
      }
    }
  }
  onChangeInstitutionInDoc() {
    for (var i = 0; i < this.institutions.length; i++) {
      if (this.institutions[i].id == this.investmentInitService.investmentDoctorFormData.institutionId) {
        this.investmentInitService.investmentDoctorFormData.address = this.institutions[i].address;

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
      debugger;
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


  }
  onChangeMarketGroupInTargetedGroup() {


    if (this.investmentTargetedGroups == null || this.investmentTargetedGroups.length == 0) {
      for (let i = 0; i < this.marketGroupMsts.length; i++) {
        if (this.marketGroupMsts[i].id == this.investmentInitService.investmentTargetedGroupFormData.marketGroupMstId) {
          var data = [];
          for (let j = 0; j < this.marketGroupMsts[i].marketGroupDtls.length; j++) {
            var marketGroupMstId = this.marketGroupMsts[i].marketGroupDtls[j].mstId;
            var marketCode = this.marketGroupMsts[i].marketGroupDtls[j].marketCode;
            var marketName = this.marketGroupMsts[i].marketGroupDtls[j].marketName;

            data.push({ id: 0, investmentInitId: this.investmentInitService.investmentInitFormData.id, marketGroupMst: this.marketGroupMsts[i], marketGroupMstId: marketGroupMstId, marketCode: marketCode, marketName: marketName });
            //this.investmentTargetedGroups.push({id:0,investmentInitId:this.investmentInitService.investmentInitFormData.id,marketGroup:null,marketGroupMstId:this.marketGroupMsts[i].marketGroupDtls[j].mstId,marketCode:this.marketGroupMsts[i].marketGroupDtls[j].marketCode,marketName:this.marketGroupMsts[i].marketGroupDtls[j].marketName});
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
    this.investmentInitService.getDoctors().subscribe(response => {

      this.doctors = response as IDoctor[];
      this.investmentInitService.getInstitutions().subscribe(response => {
        this.institutions = response as IInstitution[];
        if (this.investmentInitService.investmentInitFormData.id != null && this.investmentInitService.investmentInitFormData.id != undefined && this.investmentInitService.investmentInitFormData.id != 0) {
          this.getInvestmentDoctor();
        }
      }, error => {
        console.log(error);
      });

    }, error => {
      console.log(error);
    });
  }
  getInstitution() {
    this.investmentInitService.getInstitutions().subscribe(response => {
      this.institutions = response as IInstitution[];
      this.investmentInitService.getDoctors().subscribe(response => {
        this.doctors = response as IDoctor[];
        if (this.investmentInitService.investmentInitFormData.id != null && this.investmentInitService.investmentInitFormData.id != undefined && this.investmentInitService.investmentInitFormData.id != 0) {
          this.getInvestmentInstitution();
        }
      }, error => {
        console.log(error);
      });
    }, error => {
      console.log(error);
    });
  }
  getCampaignMst() {
    this.investmentInitService.getCampaignMsts().subscribe(response => {
      //debugger;
      this.campaignMsts = response as ICampaignMst[];
      this.investmentInitService.getDoctors().subscribe(response => {
        this.doctors = response as IDoctor[];
        this.investmentInitService.getInstitutions().subscribe(response => {
          this.institutions = response as IInstitution[];
          if (this.investmentInitService.investmentInitFormData.id != null && this.investmentInitService.investmentInitFormData.id != undefined && this.investmentInitService.investmentInitFormData.id != 0) {
            this.getInvestmentCampaign();
          }
        }, error => {
          console.log(error);
        });

      }, error => {
        console.log(error);
      });
    }, error => {
      console.log(error);
    });
  }
  getSociety() {
    this.investmentInitService.getSociety().subscribe(response => {

      this.society = response as ISocietyInfo[];
      if (this.investmentInitService.investmentInitFormData.id != null && this.investmentInitService.investmentInitFormData.id != undefined && this.investmentInitService.investmentInitFormData.id != 0) {
        this.getInvestmentSociety();
      }
    }, error => {
      console.log(error);
    });
  }
  getBcds() {
    this.investmentInitService.getBcds().subscribe(response => {
      this.bcds = response as IBcdsInfo[];
      if (this.investmentInitService.investmentInitFormData.id != null && this.investmentInitService.investmentInitFormData.id != undefined && this.investmentInitService.investmentInitFormData.id != 0) {
        this.getInvestmentBcds();
      }
    }, error => {
      console.log(error);
    });
  }

  getMarket() {
    this.investmentInitService.getMarkets().subscribe(response => {
      this.markets = response as IMarket[];
    }, error => {
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
      debugger;
      this.marketGroupMsts = response as IMarketGroupMst[];
    }, error => {
      console.log(error);
    });
  }
  onSubmit(form: NgForm) {
    if (this.investmentInitService.investmentInitFormData.id == 0)
      this.insertInvestmentInit();
    else
    if(parseInt(this.empId)==this.investmentInitService.investmentInitFormData.employeeId)
    {
      this.updateInvestmentInit();
    }
    else{
      this.updateInvestmentInitOther();
    }
  }
  insertInvestmentInit() {
    this.investmentInitService.insertInvestmentInit().subscribe(
      res => {

        this.investmentInitService.investmentInitFormData = res as IInvestmentInit;
        this.investmentInitService.investmentDoctorFormData.investmentInitId = this.investmentInitService.investmentInitFormData.id;
        this.investmentInitService.investmentInstitutionFormData.investmentInitId = this.investmentInitService.investmentInitFormData.id;
        this.isValid = true;
        this.toastr.success('Saved successfully', 'Investment')
      },
      err => { console.log(err); }
    );
  }
  updateInvestmentInit() {
    this.investmentInitService.updateInvestmentInit().subscribe(
      res => {

        this.isValid = true;
        this.investmentInitService.investmentDoctorFormData.investmentInitId = this.investmentInitService.investmentInitFormData.id;
        this.investmentInitService.investmentInstitutionFormData.investmentInitId = this.investmentInitService.investmentInitFormData.id;
        this.toastr.info('Updated successfully', 'Investment')
      },
      err => { console.log(err);
        this.toastr.info(err);
      }
    );
  }
  updateInvestmentInitOther() {
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
    if (this.investmentInitService.investmentInitFormData.id == null || this.investmentInitService.investmentInitFormData.id == undefined || this.investmentInitService.investmentInitFormData.id == 0) {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    if (this.investmentInitService.investmentDetailFormData.proposedAmount == null || this.investmentInitService.investmentDetailFormData.proposedAmount == undefined || this.investmentInitService.investmentDetailFormData.proposedAmount == "") {
      this.toastr.warning('Enter Proposed Amount First', 'Investment', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    if (this.investmentInitService.investmentDetailFormData.purpose == null || this.investmentInitService.investmentDetailFormData.purpose == undefined || this.investmentInitService.investmentDetailFormData.purpose == "") {
      this.toastr.warning('Enter Purpose First', 'Investment', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    if (this.investmentInitService.investmentDetailFormData.fromDate == null || this.investmentInitService.investmentDetailFormData.fromDate == undefined) {
      this.toastr.warning('Select From Date  First', 'Investment', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    if (this.investmentInitService.investmentDetailFormData.toDate == null || this.investmentInitService.investmentDetailFormData.toDate == undefined) {
      this.toastr.warning('Select To Date  First', 'Investment', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    if (this.investmentInitService.investmentDetailFormData.commitmentAllSBU == null || this.investmentInitService.investmentDetailFormData.commitmentAllSBU == undefined || this.investmentInitService.investmentDetailFormData.commitmentAllSBU == "") {
      this.toastr.warning('Enter Commitment All SBU First', 'Investment', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    if (this.investmentInitService.investmentDetailFormData.commitmentOwnSBU == null || this.investmentInitService.investmentDetailFormData.commitmentOwnSBU == undefined || this.investmentInitService.investmentDetailFormData.commitmentOwnSBU == "") {
      this.toastr.warning('Enter Commitment Own SBU First', 'Investment', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    if (this.investmentInitService.investmentDetailFormData.paymentMethod == null || this.investmentInitService.investmentDetailFormData.paymentMethod == undefined || this.investmentInitService.investmentDetailFormData.paymentMethod == "") {
      this.toastr.warning('Select Payment Method First', 'Investment', {
        positionClass: 'toast-top-right'
      });
      return false;
    }


    this.investmentInitService.investmentDetailFormData.investmentInitId = this.investmentInitService.investmentInitFormData.id;

    if (this.investmentInitService.investmentDetailFormData.id == null || this.investmentInitService.investmentDetailFormData.id == undefined || this.investmentInitService.investmentDetailFormData.id == 0) {
      this.investmentInitService.insertInvestmentDetail().subscribe(
        res => {
          var data = res as IInvestmentDetail;
          this.investmentInitService.investmentDetailFormData = data;
          //this.investmentInitService.investmentDoctorFormData.doctorName=String(data.doctorId);
          this.investmentInitService.investmentDetailFormData.fromDate = new Date(data.fromDate);
          this.investmentInitService.investmentDetailFormData.toDate = new Date(data.toDate);
          this.isDonationValid = true;
          this.toastr.success('Save successfully', 'Investment');
        },
        err => { console.log(err); }
      );
    }
    else {
      this.investmentInitService.updateInvestmentDetail().subscribe(
        res => {
          var data = res as IInvestmentDetail;
          this.investmentInitService.investmentDetailFormData = data;
          //this.investmentInitService.investmentDoctorFormData.doctorName=String(data.doctorId);
          this.investmentInitService.investmentDetailFormData.fromDate = new Date(data.fromDate);
          this.investmentInitService.investmentDetailFormData.toDate = new Date(data.toDate);
          this.isDonationValid = true;
          this.toastr.success('Save successfully', 'Investment');
        },
        err => { console.log(err); }
      );
    }

  }
  insertInvestmentDoctor() {
    if (this.investmentInitService.investmentInitFormData.id == null || this.investmentInitService.investmentInitFormData.id == undefined || this.investmentInitService.investmentInitFormData.id == 0) {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    // if(this.investmentInitService.investmentInitFormData.donationTo!=="Doctor")
    // {
    //   this.updateInvestmentInit();
    // }
    if (this.investmentInitService.investmentDoctorFormData.doctorId == null || this.investmentInitService.investmentDoctorFormData.doctorId == undefined || this.investmentInitService.investmentDoctorFormData.doctorId == 0) {
      this.toastr.warning('Select Doctor First', 'Investment', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    if (this.investmentInitService.investmentDoctorFormData.institutionId == null || this.investmentInitService.investmentDoctorFormData.institutionId == undefined || this.investmentInitService.investmentDoctorFormData.institutionId == 0) {
      this.toastr.warning('Select Institute First', 'Investment', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    this.investmentInitService.investmentDoctorFormData.investmentInitId = this.investmentInitService.investmentInitFormData.id;

    this.investmentInitService.insertInvestmentDoctor().subscribe(
      res => {
        var data = res as IInvestmentDoctor;
        this.investmentInitService.investmentDoctorFormData = data;
        this.investmentInitService.investmentDoctorFormData.doctorName = String(data.doctorId);
        this.onChangeDoctorInDoc();
        this.onChangeInstitutionInDoc();
        this.updateInvestmentInit();
        this.isDonationValid = true;
        this.toastr.success('Save successfully', 'Investment');
      },
      err => { console.log(err); }
    );
  }
  insertInvestmentInstitution() {

    if (this.investmentInitService.investmentInitFormData.id == null || this.investmentInitService.investmentInitFormData.id == undefined || this.investmentInitService.investmentInitFormData.id == 0) {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    // if(this.investmentInitService.investmentInitFormData.donationTo!=="Institution")
    // {

    // }
    if (this.investmentInitService.investmentInstitutionFormData.resposnsibleDoctorId == null || this.investmentInitService.investmentInstitutionFormData.resposnsibleDoctorId == undefined || this.investmentInitService.investmentInstitutionFormData.resposnsibleDoctorId == 0) {
      this.toastr.warning('Select Institution First', 'Investment', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    if (this.investmentInitService.investmentInstitutionFormData.institutionId == null || this.investmentInitService.investmentInstitutionFormData.institutionId == undefined || this.investmentInitService.investmentInstitutionFormData.institutionId == 0) {
      this.toastr.warning('Select Institute First', 'Investment', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    this.investmentInitService.investmentInstitutionFormData.investmentInitId = this.investmentInitService.investmentInitFormData.id;

    this.investmentInitService.insertInvestmentInstitution().subscribe(
      res => {
        this.investmentInitService.investmentInstitutionFormData = res as IInvestmentInstitution;
        this.onChangeInstitutionInInst();
        this.updateInvestmentInit();
        this.isDonationValid = true;
        this.toastr.success('Save successfully', 'Investment');
      },
      err => { console.log(err); }
    );
  }
  insertInvestmentCampaign() {

    if (this.investmentInitService.investmentInitFormData.id == null || this.investmentInitService.investmentInitFormData.id == undefined || this.investmentInitService.investmentInitFormData.id == 0) {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    // if(this.investmentInitService.investmentInitFormData.donationTo!=="Institution")
    // {

    // }
    if (this.investmentInitService.investmentCampaignFormData.campaignMstId == null || this.investmentInitService.investmentCampaignFormData.campaignMstId == undefined || this.investmentInitService.investmentCampaignFormData.campaignMstId == 0) {
      this.toastr.warning('Select Campaign First', 'Investment', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    if (this.investmentInitService.investmentCampaignFormData.campaignDtlId == null || this.investmentInitService.investmentCampaignFormData.campaignDtlId == undefined || this.investmentInitService.investmentCampaignFormData.campaignDtlId == 0) {
      this.toastr.warning('Select Sub-Campaign First', 'Investment', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    if (this.investmentInitService.investmentCampaignFormData.doctorId == null || this.investmentInitService.investmentCampaignFormData.doctorId == undefined || this.investmentInitService.investmentCampaignFormData.doctorId == 0) {
      this.toastr.warning('Select Doctor First', 'Investment', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    if (this.investmentInitService.investmentCampaignFormData.institutionId == null || this.investmentInitService.investmentCampaignFormData.institutionId == undefined || this.investmentInitService.investmentCampaignFormData.institutionId == 0) {
      this.toastr.warning('Select Institute First', 'Investment', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    this.investmentInitService.investmentCampaignFormData.investmentInitId = this.investmentInitService.investmentInitFormData.id;

    var tempMstId = this.investmentInitService.investmentCampaignFormData.campaignMstId;
    this.investmentInitService.insertInvestmentCampaign().subscribe(
      res => {

        this.investmentInitService.investmentCampaignFormData = res as IInvestmentCampaign;
        this.investmentInitService.investmentCampaignFormData.campaignMstId = tempMstId;
        this.onChangeCampaignInCamp();
        this.onChangeSubCampaignInCamp();
        this.updateInvestmentInit();

        this.isDonationValid = true;
        this.toastr.success('Save successfully', 'Investment');
      },
      err => { console.log(err); }
    );
  }
  insertInvestmentSociety() {

    if (this.investmentInitService.investmentInitFormData.id == null || this.investmentInitService.investmentInitFormData.id == undefined || this.investmentInitService.investmentInitFormData.id == 0) {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    // if(this.investmentInitService.investmentInitFormData.donationTo!=="Institution")
    // {

    // }
    if (this.investmentInitService.investmentSocietyFormData.societyId == null || this.investmentInitService.investmentSocietyFormData.societyId == undefined || this.investmentInitService.investmentSocietyFormData.societyId == 0) {
      this.toastr.warning('Select Society First', 'Investment', {
        positionClass: 'toast-top-right'
      });
      return false;
    }



    this.investmentInitService.investmentSocietyFormData.investmentInitId = this.investmentInitService.investmentInitFormData.id;

    this.investmentInitService.insertInvestmentSociety().subscribe(
      res => {

        this.investmentInitService.investmentSocietyFormData = res as IInvestmentSociety;
        this.onChangeSocietyInSociety();
        this.updateInvestmentInit();

        this.isDonationValid = true;
        this.toastr.success('Save successfully', 'Investment');
      },
      err => { console.log(err); }
    );
  }
  insertInvestmentBcds() {

    if (this.investmentInitService.investmentInitFormData.id == null || this.investmentInitService.investmentInitFormData.id == undefined || this.investmentInitService.investmentInitFormData.id == 0) {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    // if(this.investmentInitService.investmentInitFormData.donationTo!=="Institution")
    // {

    // }
    if (this.investmentInitService.investmentBcdsFormData.bcdsId == null || this.investmentInitService.investmentBcdsFormData.bcdsId == undefined || this.investmentInitService.investmentBcdsFormData.bcdsId == 0) {
      this.toastr.warning('Select Bcds First', 'Investment', {
        positionClass: 'toast-top-right'
      });
      return false;
    }



    this.investmentInitService.investmentBcdsFormData.investmentInitId = this.investmentInitService.investmentInitFormData.id;

    this.investmentInitService.insertInvestmentBcds().subscribe(
      res => {

        this.investmentInitService.investmentBcdsFormData = res as IInvestmentBcds;
        this.onChangeBcdsInBcds();
        this.updateInvestmentInit();

        this.isDonationValid = true;
        this.toastr.success('Save successfully', 'Investment');
      },
      err => { console.log(err); }
    );
  }
  insertInvestmentTargetedProd() {

    if (this.investmentInitService.investmentInitFormData.id == null || this.investmentInitService.investmentInitFormData.id == undefined || this.investmentInitService.investmentInitFormData.id == 0) {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment', {
        positionClass: 'toast-top-right'
      });
      return false;
    }

    if (this.investmentInitService.investmentTargetedProdFormData.productId == null || this.investmentInitService.investmentTargetedProdFormData.productId == undefined || this.investmentInitService.investmentTargetedProdFormData.productId == 0) {
      this.toastr.warning('Select Product First', 'Investment', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    if (this.investmentTargetedProds !== undefined) {
      for (let i = 0; i < this.investmentTargetedProds.length; i++) {
        if (this.investmentTargetedProds[i].productInfo.id === this.investmentInitService.investmentTargetedProdFormData.productId) {
          alert("product already exist !");
          return false;
        }
      }
    }
    this.investmentInitService.investmentTargetedProdFormData.investmentInitId = this.investmentInitService.investmentInitFormData.id;
    //for(var i=0;i<this.products.length; i++)
    //{
    // if(this.products[i].id==this.investmentInitService.investmentTargetedProdFormData.productId)
    // {
    this.investmentInitService.investmentTargetedProdFormData.sbu = this.sbu;
    // }
    //}
    if (this.investmentInitService.investmentTargetedProdFormData.id == null || this.investmentInitService.investmentTargetedProdFormData.id == undefined || this.investmentInitService.investmentTargetedProdFormData.id == 0) {
      this.investmentInitService.insertInvestmentTargetedProd().subscribe(
        res => {

          this.investmentInitService.investmentTargetedProdFormData = new InvestmentTargetedProd();

          this.getInvestmentTargetedProd();

          this.isDonationValid = true;
          this.toastr.success('Save successfully', 'Investment');
        },
        err => { console.log(err); }
      );
    }
    else {
      this.investmentInitService.updateInvestmentTargetedProd().subscribe(
        res => {

          this.investmentInitService.investmentTargetedProdFormData = new InvestmentTargetedProd();

          this.getInvestmentTargetedProd();

          this.isDonationValid = true;
          this.toastr.success('Update successfully', 'Investment');
        },
        err => { console.log(err); }
      );
    }
  }
  insertInvestmentTargetedGroup() {

    if (this.investmentInitService.investmentInitFormData.id == null || this.investmentInitService.investmentInitFormData.id == undefined || this.investmentInitService.investmentInitFormData.id == 0) {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment', {
        positionClass: 'toast-top-right'
      });
      return false;
    }

    if (this.investmentInitService.investmentTargetedGroupFormData.marketGroupMstId == null || this.investmentInitService.investmentTargetedGroupFormData.marketGroupMstId == undefined || this.investmentInitService.investmentTargetedGroupFormData.marketGroupMstId == 0) {
      this.toastr.warning('Select Market Group First', 'Investment', {
        positionClass: 'toast-top-right'
      });
      return false;
    }
    if (this.investmentTargetedGroups != null && this.investmentTargetedGroups.length > 0) {
      this.investmentInitService.investmentTargetedGroupFormData.investmentInitId = this.investmentInitService.investmentInitFormData.id;

      this.investmentInitService.insertInvestmentTargetedGroup(this.investmentTargetedGroups).subscribe(
        res => {

          this.investmentInitService.investmentTargetedProdFormData = new InvestmentTargetedProd();

          this.getInvestmentTargetedGroup();

          this.isDonationValid = true;
          this.toastr.success(res);
        },
        err => { console.log(err); }
      );
    }
    else {
      this.toastr.warning('Select Market Group First', 'Investment', {
        positionClass: 'toast-top-right'
      });
    }
  }
  editInvestmentTargetedProd(selectedRecord: IInvestmentTargetedProd) {
    this.investmentInitService.investmentTargetedProdFormData = Object.assign({}, selectedRecord);
    // var e = (document.getElementById("marketCode")) as HTMLSelectElement;
    // var sel = e.selectedIndex;
    // var opt = e.options[sel];
    // var selectedMarketCode = opt.value;
    // var selectedMarketName = opt.innerHTML;

  }
  populateForm() {
    //this.investmentInitService.campaignFormData = Object.assign({}, selectedRecord);
  }
  resetPage(form: NgForm) {
    form.reset();
    this.investmentInitService.investmentInitFormData = new InvestmentInit();
    this.isValid = false;
    this.isInvOther= false;
    this.isDonationValid = false;
    this.investmentTargetedGroups=[];
    this.investmentTargetedProds=[];
  }
  removeInvestmentDoctor() {
    var c = confirm("Are you sure you want to delete that?");
    if (c == true) {
      this.investmentInitService.removeInvestmentDoctor().subscribe(
        res => {
          this.toastr.success(res);
          this.isDonationValid = false;
          this.investmentInitService.investmentDoctorFormData = new InvestmentDoctor();
        },
        err => { console.log(err); }
      );
    }
  }
  removeInvestmentInstitution() {
    var c = confirm("Are you sure you want to delete that?");
    if (c == true) {
      this.investmentInitService.removeInvestmentInstitution().subscribe(
        res => {

          this.toastr.success(res);
          this.isDonationValid = false;
          this.investmentInitService.investmentInstitutionFormData = new InvestmentInstitution();
        },
        err => { console.log(err); }
      );
    }
  }
  removeInvestmentCampaign() {
    var c = confirm("Are you sure you want to delete that?");
    if (c == true) {
      this.investmentInitService.removeInvestmentCampaign().subscribe(
        res => {

          this.toastr.success(res);
          this.isDonationValid = false;
          this.investmentInitService.investmentCampaignFormData = new InvestmentCampaign();
        },
        err => { console.log(err); }
      );
    }
  }
  removeInvestmentSociety() {
    var c = confirm("Are you sure you want to delete that?");
    if (c == true) {
      this.investmentInitService.removeInvestmentSociety().subscribe(
        res => {

          this.toastr.success(res);
          this.isDonationValid = false;
          this.investmentInitService.investmentSocietyFormData = new InvestmentSociety();
        },
        err => { console.log(err); }
      );
    }
  }
  removeInvestmentBcds() {
    var c = confirm("Are you sure you want to delete that?");
    if (c == true) {
      this.investmentInitService.removeInvestmentBcds().subscribe(
        res => {

          this.toastr.success(res);
          this.isDonationValid = false;
          this.investmentInitService.investmentBcdsFormData = new InvestmentBcds();
        },
        err => { console.log(err); }
      );
    }
  }
  removeInvestmentTargetedProd(selectedRecord: IInvestmentTargetedProd) {
    this.investmentInitService.investmentTargetedProdFormData = Object.assign({}, selectedRecord);
    var c = confirm("Are you sure you want to delete that?");
    if (c == true) {
      this.investmentInitService.removeInvestmentTargetedProd().subscribe(
        res => {

          this.toastr.success(res);
          //this.isDonationValid=false;
          this.investmentInitService.investmentTargetedProdFormData = new InvestmentTargetedProd();
          this.getInvestmentTargetedProd();
        },
        err => { console.log(err); }
      );
    }
  }
  removeInvestmentTargetedGroup() {
    //this.investmentInitService.investmentTargetedProdFormData = Object.assign({}, selectedRecord);
    if (this.investmentTargetedGroups != null && this.investmentTargetedGroups.length > 0) {
      var c = confirm("Are you sure you want to delete that?");
      if (c == true) {
        this.investmentInitService.removeInvestmentTargetedGroup(this.investmentTargetedGroups).subscribe(
          res => {

            this.toastr.success(res);
            //this.isDonationValid=false;
            this.investmentInitService.investmentTargetedGroupFormData = new InvestmentTargetedGroup();
            this.getInvestmentTargetedGroup();
          },
          err => { console.log(err);
            this.toastr.warning(err, 'Investment'); }
        );
      }
    }
    else {
      this.toastr.warning('No Market Group Found', 'Investment', {
        positionClass: 'toast-top-right'
      });
    }
  }
}

