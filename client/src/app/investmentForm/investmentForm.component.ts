import { InvestmentForm, IInvestmentForm, InvestmentOther, IInvestmentOther } from '../shared/models/investment';
import { Employee, IEmployee } from '../shared/models/employee';
import { InvestmentDoctor, IInvestmentDoctor, InvestmentInstitution, IInvestmentInstitution, InvestmentCampaign, IInvestmentCampaign } from '../shared/models/investment';
import { InvestmentBcds, IInvestmentBcds, InvestmentSociety, IInvestmentSociety } from '../shared/models/investment';
import { SubCampaign, ISubCampaign } from '../shared/models/subCampaign';
import { Donation, IDonation } from '../shared/models/donation';
import { Doctor, IDoctor } from '../shared/models/docotor';
import { Institution, IInstitution } from '../shared/models/institution';
import { Component, ElementRef, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { InvestmentFormService } from '../_services/investmentform.service';
import { FormGroup, NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Product, IProduct } from '../shared/models/product';
import { Market, IMarket } from '../shared/models/market';
import { CampaignMst, ICampaignMst, CampaignDtl, ICampaignDtl, CampaignDtlProduct, ICampaignDtlProduct, ISubCampaignRapid, SubCampaignRapid } from '../shared/models/campaign';
import { DatePipe } from '@angular/common';
import { IBcdsInfo } from '../shared/models/bcdsInfo';
import { ISocietyInfo } from '../shared/models/societyInfo';
import { MarketGroupMst, IMarketGroupMst } from '../shared/models/marketGroupMst';
import { MarketGroupDtl, IMarketGroupDtl } from '../shared/models/marketGroupDtl';
import { InvestmentInit, IInvestmentInit, InvestmentDetail, IInvestmentDetail, InvestmentTargetedProd, IInvestmentTargetedProd, InvestmentTargetedGroup, IInvestmentTargetedGroup, IInvestmentDetailOld, ILastFiveInvestmentDetail, IInvestmentMedicineProd, InvestmentMedicineProd } from '../shared/models/investment';
import { AccountService } from '../account/account.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { BreadcrumbModule } from 'xng-breadcrumb';
import { IMedicineProduct, MedicineProduct } from '../shared/models/medicineProduct';
import { IDepotInfo } from '../shared/models/depotInfo';
import { ISBU } from '../shared/models/sbu';
import { IBrand } from '../shared/models/brand';
@Component({
  selector: 'app-investmentForm',
  templateUrl: './investmentForm.component.html',
  styles: [
  ],
  providers: [DatePipe]
})
export class InvestmentFormComponent implements OnInit {
  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  @ViewChild('investmentRapidSearchModal', { static: false }) investmentInitSearchModal: TemplateRef<any>;
  @ViewChild('submissionConfirmModal', { static: false }) submissionConfirmModal: TemplateRef<any>;
  InvestmentInitSearchModalRef: BsModalRef;
  submissionConfirmRef: BsModalRef;
  convertedDate: string;
  empId: string;
  searchText = '';
  minDate: Date;
  maxDate: Date;
  brands: IBrand[];
  degree: any;
  designation: any;
  docInstaddress: any;
  instaddress: any;
  brandCode: any = null;
  userRole: string;
  sbu: string;
  marketCode: string;
  investmentForms: IInvestmentForm[];
  employees: IEmployee[];
  investmentDoctors: IInvestmentDoctor[];
  depots: IDepotInfo[];
  SBUs: ISBU[];
  isBrandDisable: boolean = false;
  isProdDisable: boolean = false;
  isValid: boolean = false;
  isInvOther: boolean = false;
  isAdmin: boolean = false;
  isDonationValid: boolean = false;
  isSubmitted: boolean = false;
  investmentInitForm: NgForm;
  numberPattern = "^[0-9]+(.[0-9]{1,10})?$";
  bcds: IBcdsInfo[];
  society: ISocietyInfo[];
  markets: IMarket[];
  products: IProduct[];
  medicineProducts: IMedicineProduct[];
  investmentMedicineProds: IInvestmentMedicineProd[];
  investmentTargetedProds: IInvestmentTargetedProd[];
  campaignDtlproducts: IProduct[];
  subCampaigns: ISubCampaign[];
  doctors: IDoctor[];
  institutions: IInstitution[];
  donations: IDonation[];
  campaignMsts: ICampaignMst[];
  campaignDtls: ICampaignDtl[];
  subCampaignRapid: ISubCampaignRapid[];
  campaignDtlProducts: ICampaignDtlProductRapid[];
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
  institutionType: string;
  prodsByBrand: IProduct[];
  constructor(private accountService: AccountService, public investmentFormService: InvestmentFormService,
    private router: Router, private toastr: ToastrService, private modalService: BsModalService, private datePipe: DatePipe,
    private SpinnerService: NgxSpinnerService) { }
  ngOnInit() {
    this.convertedDate = this.datePipe.transform(this.today, 'dd/MM/YYYY');
    this.getDonation();
    this.getDepot();
    //this.getCampain();
    this.getSBU();
    this.getEmployees();
    this.getProduct();
    this.getMedicineProds();
    this.getEmployeeId();
    this.bsConfig = Object.assign({}, { containerClass: 'theme-blue' }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();
    const currentDate = new Date();
    this.minDate = new Date(currentDate.getFullYear(), 0, 1);
    this.maxDate = new Date(currentDate.getFullYear(), 11, 31);
    this.investmentMedicineProds = [];
    this.investmentTargetedProds = [];
    this.investmentFormService.investmentMedicineProdFormData.medicineProduct = new MedicineProduct();
    this.investmentFormService.investmentTargetedProdFormData.productInfo = new Product();
    this.investmentFormService.investmentFormData.proposalDateStr = this.convertedDate;
    this.investmentFormService.investmentDoctorFormData = new InvestmentDoctor();
    this.investmentFormService.investmentInstitutionFormData = new InvestmentInstitution();
    this.investmentFormService.investmentCampaignFormData = new InvestmentCampaign();
    this.investmentFormService.investmentBcdsFormData = new InvestmentBcds();
    this.investmentFormService.investmentSocietyFormData = new InvestmentSociety();
    this.investmentFormService.investmentOtherFormData = new InvestmentOther();
    this.investmentFormService.investmentFormData.proposalDateStr = this.convertedDate;
  }
  resetPageLoad() {
    this.investmentFormService.investmentFormData = new InvestmentForm();
    this.investmentMedicineProds = [];
    this.investmentTargetedProds = [];
    this.getEmployeeId();
    this.investmentFormService.investmentFormData.proposalDateStr = this.convertedDate;
    this.brandCode = null;

    this.isDonationValid = false;
    this.isBrandDisable = false;
    this.isProdDisable = false;
    this.investmentFormService.investmentDoctorFormData = new InvestmentDoctor();
    this.investmentFormService.investmentInstitutionFormData = new InvestmentInstitution();
    this.investmentFormService.investmentCampaignFormData = new InvestmentCampaign();
    this.investmentFormService.investmentBcdsFormData = new InvestmentBcds();
    this.investmentFormService.investmentSocietyFormData = new InvestmentSociety();
    this.investmentFormService.investmentOtherFormData = new InvestmentOther();

  }
  customSearchFn(term: string, item: any) {
    term = term.toLocaleLowerCase();
    return item.employeeSAPCode.toLocaleLowerCase().indexOf(term) > -1 ||
      item.employeeName.toLocaleLowerCase().indexOf(term) > -1;
  }

  getBrand() {
    this.investmentFormService.getBrand(this.investmentFormService.investmentFormData.sbu).subscribe(response => {
      this.brands = response as IBrand[];
    }, error => {
      console.log(error);
    });
  }
  getProductByBrand() {
    if (this.brandCode != "" && this.brandCode != null && this.brandCode != undefined) {
      this.investmentFormService.investmentTargetedProdFormData.productId = null;
      this.isBrandDisable = false;
      this.isProdDisable = true;
      this.investmentFormService.getProductByBrand(this.brandCode, this.investmentFormService.investmentFormData.sbu).subscribe(response => {
        this.prodsByBrand = response as IProduct[];
      }, error => {
        console.log(error);
      });
    }
    else {
      this.isBrandDisable = false;
      this.isProdDisable = false;
    }

  }
  getDepot() {
    this.investmentFormService.getDepot().subscribe(response => {
      this.depots = response as IDepotInfo[]; this.depots = response as IDepotInfo[];
    }, error => {
      console.log(error);
    });
  }
  // getCampain() {
  //   this.investmentFormService.getRapidSubCampaigns(this.investmentFormService.investmentFormData.sbu).subscribe(response => {

  //     this.subCampaignRapid = response as ISubCampaignRapid[];
  //   }, error => {
  //     console.log(error);
  //   });
  // }
  async getDoctor() {
    this.investmentFormService.investmentDoctorFormData = new InvestmentDoctor();

    this.SpinnerService.show();
    await this.investmentFormService.getDoctors(this.investmentFormService.investmentFormData.sbu).then(response => {
      this.doctors = response as IDoctor[];
      this.investmentFormService.getInstitutions(this.investmentFormService.investmentFormData.sbu).then(response => {
        this.institutions = response as IInstitution[];
        if (this.investmentFormService.investmentFormData.id != null && this.investmentFormService.investmentFormData.id != undefined && this.investmentFormService.investmentFormData.id != 0) {
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
  customSearchFnDoc(term: string, item: any) {
    term = term.toLocaleLowerCase();
    return item.doctorCode.toLocaleLowerCase().indexOf(term) > -1 ||
      item.doctorName.toLocaleLowerCase().indexOf(term) > -1;
  }
  async getInstitution() {
    this.investmentFormService.investmentDoctorFormData = new InvestmentDoctor();
    this.SpinnerService.show();
    await this.investmentFormService.getInstitutions(this.investmentFormService.investmentFormData.sbu).then(response => {
      this.institutions = response as IInstitution[];
      this.investmentFormService.getDoctors(this.investmentFormService.investmentFormData.sbu).then(response => {
        this.doctors = response as IDoctor[];
        if (this.investmentFormService.investmentFormData.id != null && this.investmentFormService.investmentFormData.id != undefined && this.investmentFormService.investmentFormData.id != 0) {
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
  customSearchFnIns(term: string, item: any) {
    term = term.toLocaleLowerCase();
    return item.institutionCode.toLocaleLowerCase().indexOf(term) > -1 ||
      item.institutionName.toLocaleLowerCase().indexOf(term) > -1;
  }
  async getCampaignMst(empId: number) {
    this.investmentFormService.investmentCampaignFormData = new InvestmentCampaign();
    this.SpinnerService.show();
    await this.investmentFormService.getCampaignMsts(this.investmentFormService.investmentFormData.sbu).then(response => {
      this.campaignMsts = response as ICampaignMst[];
      this.investmentFormService.getInstitutions(this.investmentFormService.investmentFormData.sbu).then(response => {
        this.institutions = response as IInstitution[];
        if (this.investmentFormService.investmentFormData.id != null && this.investmentFormService.investmentFormData.id != undefined && this.investmentFormService.investmentFormData.id != 0) {
          this.investmentFormService.getInvestmentCampaigns(this.investmentFormService.investmentFormData.investmentInitId).then(response => {
            var data = response[0] as IInvestmentCampaign;
            if (data !== undefined) {
              this.investmentFormService.investmentCampaignFormData = data;
              this.investmentFormService.investmentCampaignFormData.campaignMstId = data.campaignDtl.mstId;
              this.investmentFormService.investmentCampaignFormData.subCampStartDate = new DatePipe('en-US').transform(data.campaignDtl.subCampStartDate, 'dd/MM/yyyy');
              this.investmentFormService.investmentCampaignFormData.subCampEndDate = new DatePipe('en-US').transform(data.campaignDtl.subCampEndDate, 'dd/MM/yyyy');
              this.onChangeCampaignInCamp();
              this.onChangeSubCampaignInCamp();
              this.investmentFormService.getDoctors(this.investmentFormService.investmentFormData.sbu).then(response => {
                this.doctors = response as IDoctor[];
              }, error => {
                this.SpinnerService.hide();
                console.log(error);
              });
            }
            else {
              this.toastr.warning('No Data Found', 'Investment');
            }

          }, error => {
            console.log(error);
          });
        }


      }, error => {
        this.SpinnerService.hide();
        console.log(error);
      });
    }, error => {
      this.SpinnerService.hide();
      console.log(error);
    });
  }
  async getSociety() {
    this.investmentFormService.investmentSocietyFormData = new InvestmentSociety();
    this.SpinnerService.show();
    this.investmentFormService.getSociety().then(response => {
      this.society = response as ISocietyInfo[];
      this.investmentFormService.getDoctors(this.investmentFormService.investmentFormData.sbu).then(response => {
        this.doctors = response as IDoctor[];
        this.investmentFormService.investmentSocietyFormData.responsibleDoctorId = 900000;
        if (this.investmentFormService.investmentFormData.id != null && this.investmentFormService.investmentFormData.id != undefined && this.investmentFormService.investmentFormData.id != 0) {
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
  async getOther() {
    this.investmentFormService.investmentSocietyFormData = new InvestmentSociety();

    this.SpinnerService.show();
    await this.investmentFormService.getSociety().then(response => {
      this.society = response as ISocietyInfo[];
      this.investmentFormService.getDoctors(this.investmentFormService.investmentFormData.sbu).then(response => {
        this.doctors = response as IDoctor[];
        this.investmentFormService.investmentSocietyFormData.responsibleDoctorId = 900000;
        if (this.investmentFormService.investmentFormData.id != null && this.investmentFormService.investmentFormData.id != undefined && this.investmentFormService.investmentFormData.id != 0) {
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
  async getBcds() {
    this.investmentFormService.investmentBcdsFormData = new InvestmentBcds();
    this.SpinnerService.show();
    await this.investmentFormService.getBcds().then(response => {
      this.bcds = response as IBcdsInfo[];
      this.investmentFormService.getDoctors(this.investmentFormService.investmentFormData.sbu).then(response => {
        this.doctors = response as IDoctor[];
        this.investmentFormService.investmentBcdsFormData.responsibleDoctorId = 900000;
        if (this.investmentFormService.investmentFormData.id != null && this.investmentFormService.investmentFormData.id != undefined && this.investmentFormService.investmentFormData.id != 0) {
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
  getSBU() {
    this.investmentFormService.getSBU().subscribe(response => {
      this.SBUs = response as ISBU[];
      console.log(this.SBUs);
    }, error => {
      console.log(error);
    });
  }
  getDonation() {
    this.investmentFormService.getDonations().subscribe(response => {
      this.donations = response as IDonation[];
    }, error => {
      console.log(error);
    });
  }
  getProduct() {

    this.SpinnerService.show();
    this.investmentFormService.getProduct(this.investmentFormService.investmentFormData.sbu).subscribe(response => {
      this.products = response as IProduct[];

      this.SpinnerService.hide();
    }, error => {
      this.SpinnerService.hide();
      console.log(error);
    });
  }
  getMedicineProds() {
    this.SpinnerService.show();
    this.investmentFormService.getMedicineProduct().subscribe(response => {
      this.medicineProducts = response as IMedicineProduct[];
      this.SpinnerService.hide();
    }, error => {
      this.SpinnerService.hide();
      console.log(error);
    });
  }
  getInvestmentTargetedProd() {
    var data = this.investmentTargetedProds;
    if (data !== undefined && data.length > 0) {
      this.investmentTargetedProds = data;
    }
    else {
      this.investmentTargetedProds = [];
    }
  }
  getInvestmentRecProd() {
    this.investmentFormService.getInvestmentTargetedProds(this.investmentFormService.investmentFormData.investmentInitId, this.investmentFormService.investmentFormData.sbu).subscribe(response => {
      var data = response as IInvestmentTargetedProd[];
      if (data !== undefined) {
        this.investmentTargetedProds = data;
      }
    }, error => {
      console.log(error);
    });
  }
  getEmployees() {
    this.investmentFormService.getEmployeesforRapid().subscribe(response => {
      this.employees = response as IEmployee[];
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
    this.investmentFormService.investmentMedicineProdFormData.employeeId = parseInt(this.empId);
    this.investmentFormService.investmentTargetedProdFormData.employeeId = parseInt(this.empId);
    this.investmentFormService.investmentFormData.initiatorId = parseInt(this.empId);
  }
  getInvestmentMedicineProd() {
    var data = this.investmentMedicineProds;
    if (data !== undefined && data.length > 0) {
      this.investmentMedicineProds = data;
      let sum = 0;
      for (let i = 0; i < this.investmentMedicineProds.length; i++) {
        sum = sum + this.investmentMedicineProds[i].tpVat;
      }
      //this.investmentFormService.investmentDetailFormData.proposedAmount=sum.toString();
      this.investmentFormService.investmentFormData.proposedAmount = ((Math.round(sum * 100) / 100).toFixed(2));
    }
    else {
      this.investmentFormService.investmentDetailFormData.proposedAmount = '';
      this.investmentMedicineProds = [];
    }
  }
  getInvestmentRapid(For: string) {
    const params = this.investmentFormService.getGenParams();
    this.SpinnerService.show();
    this.investmentFormService.getInvestmentRapids(parseInt(this.empId), "init", For).subscribe(response => {
      this.SpinnerService.hide();
      this.investmentForms = response as IInvestmentForm[];
      if (this.investmentForms.length > 0) {
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
  async getInvestmentBcds() {
    await this.investmentFormService.getInvestmentBcds(this.investmentFormService.investmentFormData.investmentInitId).then(response => {
      var data = response[0] as IInvestmentBcds;
      if (data !== undefined) {
        this.investmentFormService.investmentBcdsFormData = data;
        this.onChangeBcdsInBcds();
      }
      else {
        //this.toastr.warning('No Data Found', 'Investment');
      }
    }, error => {
      console.log(error);
    });
  }
  async getInvestmentSociety() {
    await this.investmentFormService.getInvestmentSociety(this.investmentFormService.investmentFormData.investmentInitId).then(response => {
      var data = response[0] as IInvestmentSociety;
      if (data !== undefined) {
        this.investmentFormService.investmentSocietyFormData = data;
        this.onChangeSocietyInSociety();
      }
      else {
        //this.toastr.warning('No Data Found', 'Investment');
      }

    }, error => {
      console.log(error);
    });
  }
  async getInvestmentOther() {
    await this.investmentFormService.getInvestmentOther(this.investmentFormService.investmentFormData.investmentInitId).then(response => {
      var data = response[0] as IInvestmentOther;
      if (data !== undefined) {
        this.investmentFormService.investmentOtherFormData = data;
        //this.onChangeSocietyInSociety();
      }
      else {
        //this.toastr.warning('No Data Found', 'Investment');
      }

    }, error => {
      console.log(error);
    });
  }

  async getInvestmentInstitution() {
    this.investmentFormService.getInvestmentInstitutions(this.investmentFormService.investmentFormData.investmentInitId).then(response => {
      var data = response[0] as IInvestmentInstitution;
      if (data !== undefined) {
        this.investmentFormService.investmentInstitutionFormData = data;
        this.onChangeInstitutionInInst();
      }
      else {
        //this.toastr.warning('No Data Found', 'Investment');
      }
    }, error => {
      console.log(error);
    });
  }
  async getInvestmentDoctor() {
    await this.investmentFormService.getInvestmentDoctors(this.investmentFormService.investmentFormData.investmentInitId).then(response => {
      var data = response[0] as IInvestmentDoctor;
      if (data !== undefined) {
        this.investmentFormService.investmentDoctorFormData = data;
        //this.investmentFormService.investmentDoctorFormData.doctorName = String(data.doctorId);
        this.onChangeDoctorInDoc();
        this.onChangeInstitutionInDoc();
      }
      else {
        //this.toastr.warning('No Data Found', 'Investment');
      }
    }, error => {
      console.log(error);
    });
  }
  async getInvestmentCampaign() {
    await this.investmentFormService.getInvestmentCampaigns(this.investmentFormService.investmentFormData.investmentInitId).then(response => {
      var data = response[0] as IInvestmentCampaign;
      if (data !== undefined) {
        this.investmentFormService.investmentCampaignFormData = data;
        this.investmentFormService.investmentCampaignFormData.campaignMstId = data.campaignDtl.mstId;
        this.investmentFormService.investmentCampaignFormData.subCampStartDate = new DatePipe('en-US').transform(data.campaignDtl.subCampStartDate, 'dd/MM/yyyy');
        this.investmentFormService.investmentCampaignFormData.subCampEndDate = new DatePipe('en-US').transform(data.campaignDtl.subCampEndDate, 'dd/MM/yyyy');

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
  ////////////////////////////////////////////////////////////////////////////////
  resetSearch() {
    this.searchText = '';
  }
  async selectInvestmentRapid(selectedRecord: IInvestmentForm) {
    this.investmentFormService.investmentFormData = Object.assign({}, selectedRecord);
    this.investmentFormService.investmentFormData.proposalDateStr = this.datePipe.transform(selectedRecord.propsalDate, 'dd/MM/YYYY');
    if (this.investmentFormService.investmentFormData.type == '4') {
      await this.investmentFormService.getInvestmentmedicineProducts(selectedRecord.investmentInitId).then(response => {
        this.SpinnerService.hide();
        this.investmentMedicineProds = response as IInvestmentMedicineProd[];
      }, error => {
        this.SpinnerService.hide();
        console.log(error);
      });
    }
    if (this.investmentFormService.investmentFormData.donationTo != "Campaign") {
      await this.investmentFormService.getEmployeesforRapidBySBU(this.investmentFormService.investmentFormData.proposeFor, this.investmentFormService.investmentFormData.sbu, parseInt(this.empId)).then(response => {
        this.employees = response as IEmployee[];
      }, error => {
        console.log(error);
      });
    }
    else {
      await this.investmentFormService.getEmployeesforRapidByCamp(this.investmentFormService.investmentCampaignFormData.campaignDtlId, parseInt(this.empId)).then(response => {
        this.employees = response as IEmployee[];
        this.investmentFormService.investmentFormData.approverId = selectedRecord.approverId;
      }, error => {
        console.log(error);
      });
    }
    if (this.investmentFormService.investmentFormData.donationTo == "Doctor") {
      //if (this.investmentFormService.investmentDoctorFormData.id == null || this.investmentFormService.investmentDoctorFormData.id == undefined || this.investmentFormService.investmentDoctorFormData.id == 0) {
      this.investmentFormService.investmentDoctorFormData = new InvestmentDoctor();
      await this.getDoctor();
      //this.getInstitution();
      //}
    }
    else if (this.investmentFormService.investmentFormData.donationTo == "Institution") {
      //if (this.investmentFormService.investmentInstitutionFormData.id == null || this.investmentFormService.investmentInstitutionFormData.id == undefined || this.investmentFormService.investmentInstitutionFormData.id == 0) {
      this.investmentFormService.investmentInstitutionFormData = new InvestmentInstitution();
      //this.getDoctor();
      await this.getInstitution();
      //}
    }
    else if (this.investmentFormService.investmentFormData.donationTo == "Campaign") {
      //if (this.investmentFormService.investmentCampaignFormData.id == null || this.investmentFormService.investmentCampaignFormData.id == undefined || this.investmentFormService.investmentCampaignFormData.id == 0) {
      this.investmentFormService.investmentCampaignFormData = new InvestmentCampaign();
      await this.getCampaignMst(parseInt(this.empId));
      //this.getDoctor();
      //this.getInstitution();
      //}
    }
    else if (this.investmentFormService.investmentFormData.donationTo == "Bcds") {
      //if (this.investmentFormService.investmentBcdsFormData.id == null || this.investmentFormService.investmentBcdsFormData.id == undefined || this.investmentFormService.investmentBcdsFormData.id == 0) {
      this.investmentFormService.investmentBcdsFormData = new InvestmentBcds();
      await this.getBcds();
      //}
    }
    else if (this.investmentFormService.investmentFormData.donationTo == "Society") {
      //if (this.investmentFormService.investmentSocietyFormData.id == null || this.investmentFormService.investmentSocietyFormData.id == undefined || this.investmentFormService.investmentSocietyFormData.id == 0) {
      this.investmentFormService.investmentSocietyFormData = new InvestmentSociety();
      await this.getSociety();
      //}
    }
    else if (this.investmentFormService.investmentFormData.donationTo == "Other") {
      //if (this.investmentFormService.investmentSocietyFormData.id == null || this.investmentFormService.investmentSocietyFormData.id == undefined || this.investmentFormService.investmentSocietyFormData.id == 0) {
      this.investmentFormService.investmentOtherFormData = new InvestmentOther();
      await this.getInvestmentOther();
      //}
    }
    await this.getInvestmentRecProd();
    await this.getProduct();
    this.isDonationValid = true;
    this.isValid = true;
    this.InvestmentInitSearchModalRef.hide();
  }
  //////////////////////////////////////////////////////////////////////////////
  confirmSubmission() {
    this.openSubmissionConfirmModal(this.submissionConfirmModal);
  }
  confirmSubmit() {
    this.submissionConfirmRef.hide();
    this.submitInvestmentForm();
  }
  declineSubmit() {
    this.submissionConfirmRef.hide();
  }
  submitInvestmentForm() {
    if (this.investmentFormService.investmentFormData.approverId == parseInt(this.empId)) {
      this.toastr.warning('Initiator can not be approver!');
      return;

    }
    if (this.investmentFormService.investmentFormData.paymentMethod == 'Cash' && this.investmentFormService.investmentFormData.depotCode == null) {
      this.toastr.warning('Depot Code is required!');
      return;
    }
    else if (this.investmentFormService.investmentFormData.paymentMethod == 'Cheque' && this.investmentFormService.investmentFormData.chequeTitle == null) {
      this.toastr.warning('Cheque Title is required!');
      return;
    }
    else if ((this.investmentTargetedProds == null || this.investmentTargetedProds.length == 0) && (this.investmentMedicineProds == null || this.investmentMedicineProds.length == 0)) {
      this.toastr.warning('No Product is added.');
      return;
    }
    else {
      this.SpinnerService.show();
      this.investmentFormService.investmentFormData.investmentMedicineProd = this.investmentMedicineProds;
      this.investmentFormService.investmentFormData.investmentRecProducts = this.investmentTargetedProds;
      if (this.investmentFormService.investmentFormData.donationTo == "Doctor") {
        this.investmentFormService.investmentFormData.investmentDoctor = this.investmentFormService.investmentDoctorFormData;
      }
      else if (this.investmentFormService.investmentFormData.donationTo == "Institution") {
        this.investmentFormService.investmentFormData.investmentInstitution = this.investmentFormService.investmentInstitutionFormData;
      }
      else if (this.investmentFormService.investmentFormData.donationTo == "Campaign") {
        this.investmentFormService.investmentFormData.investmentCampaign = this.investmentFormService.investmentCampaignFormData;
      }
      else if (this.investmentFormService.investmentFormData.donationTo == "Bcds") {
        this.investmentFormService.investmentFormData.investmentBcds = this.investmentFormService.investmentBcdsFormData;
      }
      else if (this.investmentFormService.investmentFormData.donationTo == "Society") {
        this.investmentFormService.investmentFormData.investmentSociety = this.investmentFormService.investmentSocietyFormData;
      }
      else if (this.investmentFormService.investmentFormData.donationTo == "Other") {
        this.investmentFormService.investmentFormData.investmentOther = this.investmentFormService.investmentOtherFormData;
      }
      for (let i = 0; i < this.depots.length; i++) {
        if (this.depots[i].depotCode == this.investmentFormService.investmentFormData.depotCode) {
          this.investmentFormService.investmentFormData.depotName = this.depots[i].depotName;
          break;
        }
      }
      // for (let i = 0; i < this.subCampaignRapid.length; i++) {
      //   if (this.subCampaignRapid[i].subCampId == this.investmentFormService.investmentFormData.subCampaignId) {
      //     this.investmentFormService.investmentFormData.subCampaignName = this.subCampaignRapid[i].subCampaignName;
      //     break;
      //   }
      // }
      for (let i = 0; i < this.SBUs.length; i++) {
        if (this.SBUs[i].sbuCode == this.investmentFormService.investmentFormData.sbu) {
          this.investmentFormService.investmentFormData.sbuName = this.SBUs[i].sbuName;
          break;
        }
      }
      this.investmentFormService.submitInvestment(parseInt(this.empId)).subscribe(
        res => {
          this.investmentFormService.investmentFormData = res as IInvestmentForm;

          this.toastr.success('Submitted successfully', 'Investment');
          this.resetPageLoad();
        },
        err => {
          console.log(err);
        }
      );
    }
  }
  insertInvestmentDoctor() {
    if (this.isSubmitted == true) {
      this.toastr.warning('This Investment has already been submitted', 'Investment');
      return false;
    }
    if (this.investmentFormService.investmentFormData.id == null || this.investmentFormService.investmentFormData.id == undefined || this.investmentFormService.investmentFormData.id == 0) {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment Doctor');
      return false;
    }
    // if(this.investmentFormService.investmentFormData.donationTo!=="Doctor")
    // {
    //   //this.updateInvestmentInit();
    // }
    if (this.investmentFormService.investmentDoctorFormData.doctorId == null || this.investmentFormService.investmentDoctorFormData.doctorId == undefined || this.investmentFormService.investmentDoctorFormData.doctorId == 0) {
      this.toastr.warning('Select Doctor First', 'Investment Doctor');
      return false;
    }
    if (this.investmentFormService.investmentDoctorFormData.institutionId == null || this.investmentFormService.investmentDoctorFormData.institutionId == undefined || this.investmentFormService.investmentDoctorFormData.institutionId == 0) {
      this.toastr.warning('Select Institute First', 'Investment Doctor');
      return false;
    }
    this.investmentFormService.investmentDoctorFormData.investmentInitId = this.investmentFormService.investmentFormData.id;
    this.SpinnerService.show();
    this.investmentFormService.insertInvestmentDoctor().subscribe(
      res => {
        var data = res as IInvestmentDoctor;
        this.investmentFormService.investmentDoctorFormData = data;
        this.investmentFormService.investmentDoctorFormData.doctorName = String(data.doctorId);
        this.onChangeDoctorInDoc();
        this.onChangeInstitutionInDoc();
        ////this.updateInvestmentInit();
        //this.getLastFiveInvestment(this.investmentFormService.investmentFormData.marketCode, this.todayDate);
        this.isDonationValid = true;
        this.toastr.success('Save successfully', 'Investment Doctor');
      },
      err => { console.log(err); }
    );
  }
  insertInvestmentInstitution() {
    if (this.isSubmitted == true) {
      this.toastr.warning('This Investment has already been submitted', 'Investment');
      return false;
    }
    if (this.investmentFormService.investmentFormData.id == null || this.investmentFormService.investmentFormData.id == undefined || this.investmentFormService.investmentFormData.id == 0) {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment Institution');
      return false;
    }
    // if(this.investmentFormService.investmentFormData.donationTo!=="Institution")
    // {

    // }
    if (this.investmentFormService.investmentInstitutionFormData.responsibleDoctorId == null || this.investmentFormService.investmentInstitutionFormData.responsibleDoctorId == undefined || this.investmentFormService.investmentInstitutionFormData.responsibleDoctorId == 0) {
      this.toastr.warning('Select Institution First', 'Investment Institution');
      return false;
    }
    if (this.investmentFormService.investmentInstitutionFormData.institutionId == null || this.investmentFormService.investmentInstitutionFormData.institutionId == undefined || this.investmentFormService.investmentInstitutionFormData.institutionId == 0) {
      this.toastr.warning('Select Institute First', 'Investment Institution');
      return false;
    }
    this.investmentFormService.investmentInstitutionFormData.investmentInitId = this.investmentFormService.investmentFormData.id;
    this.SpinnerService.show();
    this.investmentFormService.insertInvestmentInstitution().subscribe(
      res => {
        this.investmentFormService.investmentInstitutionFormData = res as IInvestmentInstitution;
        this.onChangeInstitutionInInst();
        ////this.updateInvestmentInit();
        this.isDonationValid = true;
        //this.getLastFiveInvestment(this.investmentFormService.investmentFormData.marketCode, this.todayDate);
        this.toastr.success('Save successfully', 'Investment Institution');
      },
      err => {
        console.log(err);
      }
    );
  }
  insertInvestmentCampaign() {
    if (this.isSubmitted == true) {
      this.toastr.warning('This Investment has already been submitted', 'Investment');
      return false;
    }
    if (this.investmentFormService.investmentFormData.id == null || this.investmentFormService.investmentFormData.id == undefined || this.investmentFormService.investmentFormData.id == 0) {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment Campaign');
      return false;
    }
    // if(this.investmentFormService.investmentFormData.donationTo!=="Institution")
    // {

    // }
    if (this.investmentFormService.investmentCampaignFormData.campaignMstId == null || this.investmentFormService.investmentCampaignFormData.campaignMstId == undefined || this.investmentFormService.investmentCampaignFormData.campaignMstId == 0) {
      this.toastr.warning('Select Campaign First', 'Investment Campaign');
      return false;
    }
    if (this.investmentFormService.investmentCampaignFormData.campaignDtlId == null || this.investmentFormService.investmentCampaignFormData.campaignDtlId == undefined || this.investmentFormService.investmentCampaignFormData.campaignDtlId == 0) {
      this.toastr.warning('Select Sub-Campaign First', 'Investment Campaign');
      return false;
    }
    if (this.investmentFormService.investmentCampaignFormData.doctorId == null || this.investmentFormService.investmentCampaignFormData.doctorId == undefined || this.investmentFormService.investmentCampaignFormData.doctorId == 0) {
      this.toastr.warning('Select Doctor First', 'Investment Campaign');
      return false;
    }
    if (this.investmentFormService.investmentCampaignFormData.institutionId == null || this.investmentFormService.investmentCampaignFormData.institutionId == undefined || this.investmentFormService.investmentCampaignFormData.institutionId == 0) {
      this.toastr.warning('Select Institute First', 'Investment Campaign');
      return false;
    }
    this.investmentFormService.investmentCampaignFormData.investmentInitId = this.investmentFormService.investmentFormData.id;

    var tempMstId = this.investmentFormService.investmentCampaignFormData.campaignMstId;
    this.SpinnerService.show();
    this.investmentFormService.insertInvestmentCampaign().subscribe(
      res => {
        this.investmentFormService.investmentCampaignFormData = res as IInvestmentCampaign;
        this.investmentFormService.investmentCampaignFormData.campaignMstId = tempMstId;
        this.onChangeCampaignInCamp();
        //this.onChangeSubCampaignInCamp();
        //this.updateInvestmentInit();

        this.isDonationValid = true;
        this.toastr.success('Save successfully', 'Investment Campaign');
      },
      err => { console.log(err); }
    );
  }
  insertInvestmentSociety() {
    if (this.isSubmitted == true) {
      this.toastr.warning('This Investment has already been submitted', 'Investment');
      return false;
    }
    if (this.investmentFormService.investmentFormData.id == null || this.investmentFormService.investmentFormData.id == undefined || this.investmentFormService.investmentFormData.id == 0) {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment Society');
      return false;
    }
    // if(this.investmentFormService.investmentFormData.donationTo!=="Institution")
    // {

    // }
    if (this.investmentFormService.investmentSocietyFormData.societyId == null || this.investmentFormService.investmentSocietyFormData.societyId == undefined || this.investmentFormService.investmentSocietyFormData.societyId == 0) {
      this.toastr.warning('Select Society First', 'Investment Society');
      return false;
    }



    this.investmentFormService.investmentSocietyFormData.investmentInitId = this.investmentFormService.investmentFormData.id;
    this.SpinnerService.show();
    this.investmentFormService.insertInvestmentSociety().subscribe(
      res => {

        this.investmentFormService.investmentSocietyFormData = res as IInvestmentSociety;
        this.onChangeSocietyInSociety();
        //this.updateInvestmentInit();

        this.isDonationValid = true;
        //this.getLastFiveInvestment(this.investmentFormService.investmentFormData.marketCode, this.todayDate);
        this.toastr.success('Save successfully', 'Investment Society');
      },
      err => { console.log(err); }
    );
  }
  insertInvestmentBcds() {
    if (this.isSubmitted == true) {
      this.toastr.warning('This Investment has already been submitted', 'Investment');
      return false;
    }
    if (this.investmentFormService.investmentFormData.id == null || this.investmentFormService.investmentFormData.id == undefined || this.investmentFormService.investmentFormData.id == 0) {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment Bcds');
      return false;
    }
    // if(this.investmentFormService.investmentFormData.donationTo!=="Institution")
    // {

    // }
    if (this.investmentFormService.investmentBcdsFormData.bcdsId == null || this.investmentFormService.investmentBcdsFormData.bcdsId == undefined || this.investmentFormService.investmentBcdsFormData.bcdsId == 0) {
      this.toastr.warning('Select Bcds First', 'Investment Bcds');
      return false;
    }
    this.investmentFormService.investmentBcdsFormData.investmentInitId = this.investmentFormService.investmentFormData.id;
    this.SpinnerService.show();
    this.investmentFormService.insertInvestmentBcds().subscribe(
      res => {

        this.investmentFormService.investmentBcdsFormData = res as IInvestmentBcds;
        this.onChangeBcdsInBcds();
        //this.updateInvestmentInit();

        this.isDonationValid = true;
        //this.getLastFiveInvestment(this.investmentFormService.investmentFormData.marketCode, this.todayDate);
        this.toastr.success('Save successfully', 'Investment Bcds');
      },
      err => { console.log(err); }
    );
  }
  //////////////////////////////////////////////////////////////////////////////
  // onChangeSubCampaignInCamp() {
  //   this.investmentTargetedProds = [];
  //   this.investmentFormService.getCampaignDtlProducts(this.investmentFormService.investmentFormData.subCampaignId).subscribe(response => {
  //     this.campaignDtlProducts = response as ICampaignDtlProductRapid[];
  //     let data = new InvestmentTargetedProd();
  //     let productData = new Product();
  //     for (let i = 0; i < this.campaignDtlProducts.length; i++) {
  //       data = new InvestmentTargetedProd();
  //       data.productId = this.campaignDtlProducts[i].productId;
  //       data.investmentInitId = this.investmentFormService.investmentTargetedProdFormData.investmentInitId;
  //       data.sbu = this.investmentFormService.investmentFormData.sbu;
  //       data.employeeId = this.investmentFormService.investmentMedicineProdFormData.employeeId;
  //       productData = new Product();
  //       productData.productName = this.campaignDtlProducts[i].productInfo.productName;
  //       productData.productCode = this.campaignDtlProducts[i].productInfo.productCode;
  //       productData.sbu = this.campaignDtlProducts[i].productInfo.sbu;
  //       productData.sbuName = this.campaignDtlProducts[i].productInfo.sbuName;
  //       productData.setOn = this.campaignDtlProducts[i].productInfo.setOn;
  //       productData.status = this.campaignDtlProducts[i].productInfo.status;
  //       productData.id = this.campaignDtlProducts[i].productInfo.id;

  //       data.productInfo = productData;
  //       this.investmentTargetedProds.push(data);
  //     }
  //     this.investmentFormService.investmentFormData.approverId = null;
  //     this.investmentFormService.getEmployeesforRapidByCamp(this.investmentFormService.investmentFormData.subCampaignId).subscribe(response => {
  //       this.employees = response as IEmployee[];
  //     }, error => {
  //       console.log(error);
  //     });
  //   }, error => {
  //     console.log(error);
  //   });
  // }
  onChangeProduct() {
    if (this.medicineProducts !== undefined) {
      for (let i = 0; i < this.medicineProducts.length; i++) {
        if (this.medicineProducts[i].id === this.investmentFormService.investmentMedicineProdFormData.productId) {
          this.investmentFormService.investmentMedicineProdFormData.medicineProduct.productName = this.medicineProducts[i].productName;
          this.investmentFormService.investmentMedicineProdFormData.medicineProduct.unitTp = this.medicineProducts[i].unitTp;
          this.investmentFormService.investmentMedicineProdFormData.medicineProduct.unitVat = this.medicineProducts[i].unitVat;
          this.investmentFormService.investmentMedicineProdFormData.medicineProduct.productCode = this.medicineProducts[i].productCode;
          this.investmentFormService.investmentMedicineProdFormData.medicineProduct.status = this.medicineProducts[i].status;
          this.investmentFormService.investmentMedicineProdFormData.medicineProduct.id = this.medicineProducts[i].id;
          return false;
        }
      }
    }

  }
  onChangeTargetedProduct() {
    if (this.investmentFormService.investmentTargetedProdFormData.productId == null || this.investmentFormService.investmentTargetedProdFormData.productId == undefined || this.investmentFormService.investmentTargetedProdFormData.productId == 0) {
      this.isBrandDisable = false;
      this.isProdDisable = false;
      return;
    }
    this.brandCode = null;
    this.isBrandDisable = true;
    this.isProdDisable = false;
    if (this.products !== undefined) {
      for (let i = 0; i < this.products.length; i++) {
        if (this.products[i].id === this.investmentFormService.investmentTargetedProdFormData.productId) {

          this.investmentFormService.investmentTargetedProdFormData.productInfo.productName = this.products[i].productName;
          this.investmentFormService.investmentTargetedProdFormData.productInfo.productCode = this.products[i].productCode;
          this.investmentFormService.investmentTargetedProdFormData.productInfo.sbu = this.products[i].sbu;
          this.investmentFormService.investmentTargetedProdFormData.productInfo.sbuName = this.products[i].sbuName;
          this.investmentFormService.investmentTargetedProdFormData.productInfo.setOn = this.products[i].setOn;
          this.investmentFormService.investmentTargetedProdFormData.productInfo.status = this.products[i].status;
          this.investmentFormService.investmentTargetedProdFormData.productInfo.id = this.products[i].id;
          return false;
        }
      }
    }

  }
  onChangePayMethod() {
    if (this.investmentFormService.investmentFormData.paymentMethod != "Cash") {
      this.investmentFormService.investmentFormData.depotCode = null;
    }
    if (this.investmentFormService.investmentFormData.paymentMethod != "Cheque") {
      this.investmentFormService.investmentFormData.chequeTitle = "";
    }
  }
  onChangeDonationTo() {
    if (this.investmentFormService.investmentFormData.proposeFor != "PMD" && this.investmentFormService.investmentFormData.donationTo == "Campaign" && this.investmentFormService.investmentFormData.donationTo != null) {
      this.toastr.warning("For Campaign, must select PMD");
      this.investmentFormService.investmentFormData.donationTo = "";
      return false;
    }
    if (this.investmentFormService.investmentFormData.sbu == "" || this.investmentFormService.investmentFormData.sbu == null || this.investmentFormService.investmentFormData.sbu == undefined) {
      this.toastr.warning("Please Select SBU first");
      this.investmentFormService.investmentFormData.donationTo = "";
      return false;
    }
    // if (this.investmentFormService.investmentFormData.proposeFor == "Others" && this.investmentFormService.investmentFormData.donationTo == "Campaign" && this.investmentFormService.investmentFormData.donationTo != null) {
    //   this.toastr.warning("For Campaign, must select Brand Campaign");
    //   this.investmentFormService.investmentFormData.proposeFor = null;
    //   return false;
    // }


    if (this.investmentFormService.investmentFormData.donationTo == "Doctor") {
      if (this.investmentFormService.investmentDoctorFormData.id == null || this.investmentFormService.investmentDoctorFormData.id == undefined || this.investmentFormService.investmentDoctorFormData.id == 0) {
        this.investmentFormService.investmentDoctorFormData = new InvestmentDoctor();
        this.getDoctor();
        this.getInstitution();
      }
    }
    else if (this.investmentFormService.investmentFormData.donationTo == "Institution") {
      if (this.investmentFormService.investmentInstitutionFormData.id == null || this.investmentFormService.investmentInstitutionFormData.id == undefined || this.investmentFormService.investmentInstitutionFormData.id == 0) {
        this.investmentFormService.investmentDoctorFormData = new InvestmentDoctor();
        this.getDoctor();
        this.getInstitution();
      }
    }
    else if (this.investmentFormService.investmentFormData.donationTo == "Campaign") {
      if (this.investmentFormService.investmentCampaignFormData.id == null || this.investmentFormService.investmentCampaignFormData.id == undefined || this.investmentFormService.investmentCampaignFormData.id == 0) {
        this.investmentFormService.investmentCampaignFormData = new InvestmentCampaign();
        this.getCampaignMst(parseInt(this.empId));
        this.getDoctor();
        this.getInstitution();
      }
    }
    else if (this.investmentFormService.investmentFormData.donationTo == "Bcds") {
      if (this.investmentFormService.investmentBcdsFormData.id == null || this.investmentFormService.investmentBcdsFormData.id == undefined || this.investmentFormService.investmentBcdsFormData.id == 0) {
        this.investmentFormService.investmentBcdsFormData = new InvestmentBcds();
        this.getBcds();
      }
    }
    else if (this.investmentFormService.investmentFormData.donationTo == "Society") {
      if (this.investmentFormService.investmentSocietyFormData.id == null || this.investmentFormService.investmentSocietyFormData.id == undefined || this.investmentFormService.investmentSocietyFormData.id == 0) {
        this.investmentFormService.investmentSocietyFormData = new InvestmentSociety();
        this.getSociety();
      }
    }
    if (this.investmentFormService.investmentFormData.id != null && this.investmentFormService.investmentFormData.id != undefined && this.investmentFormService.investmentFormData.id != 0) {
      this.investmentFormService.investmentDoctorFormData.investmentInitId = this.investmentFormService.investmentFormData.id;
      this.investmentFormService.investmentInstitutionFormData.investmentInitId = this.investmentFormService.investmentFormData.id;
      this.investmentFormService.investmentCampaignFormData.investmentInitId = this.investmentFormService.investmentFormData.id;
      this.investmentFormService.investmentBcdsFormData.investmentInitId = this.investmentFormService.investmentFormData.id;
      this.investmentFormService.investmentSocietyFormData.investmentInitId = this.investmentFormService.investmentFormData.id;
    }
  }
  onChangeDoctorInDoc() {
    for (var i = 0; i < this.doctors.length; i++) {
      if (this.doctors[i].id == this.investmentFormService.investmentDoctorFormData.doctorId) {
        //this.investmentFormService.investmentDoctorFormData.doctorName=this.doctors[i].doctorName;
        this.investmentFormService.investmentDoctorFormData.doctorCode = this.doctors[i].doctorCode;
        this.degree = this.doctors[i].degree;
        this.designation = this.doctors[i].designation;
        break;
      }
    }
    //this.getLastFiveInvestment(this.investmentFormService.investmentFormData.marketCode, this.convertedDate);
  }
  onChangeInstitutionInDoc() {
    for (var i = 0; i < this.institutions.length; i++) {
      if (this.institutions[i].id == this.investmentFormService.investmentDoctorFormData.institutionId) {
        this.docInstaddress = this.institutions[i].address;

        break;
      }
    }
  }
  onChangeInstitutionInInst() {
    for (var i = 0; i < this.institutions.length; i++) {
      if (this.institutions[i].id == this.investmentFormService.investmentInstitutionFormData.institutionId) {
        this.instaddress = this.institutions[i].address;
        this.institutionType = this.institutions[i].institutionType;
        break;
      }
    }
    // this.getLastFiveInvestment(this.investmentFormService.investmentFormData.marketCode, this.convertedDate);
  }
  async onChangeCampaignInCamp() {
    await this.investmentFormService.getCampaignDtls(this.investmentFormService.investmentCampaignFormData.campaignMstId).then(response => {
      this.campaignDtls = response as ICampaignDtl[];
      this.onChangeSubCampaignInCamp();
    }, error => {
      console.log(error);
    });
  }
  async onChangeSubCampaignInCamp() {
    if(this.campaignDtls!=undefined)
    {
      for (var i = 0; i < this.campaignDtls.length; i++) {
        if (this.campaignDtls[i].id == this.investmentFormService.investmentCampaignFormData.campaignDtlId) {
          this.investmentFormService.investmentCampaignFormData.subCampStartDate = new DatePipe('en-US').transform(this.campaignDtls[i].subCampStartDate, 'dd/MM/yyyy');
          this.investmentFormService.investmentCampaignFormData.subCampEndDate = new DatePipe('en-US').transform(this.campaignDtls[i].subCampEndDate, 'dd/MM/yyyy');
          break;
        }
      }
    }
   
    await this.investmentFormService.getCampaignDtlProducts(this.investmentFormService.investmentCampaignFormData.campaignDtlId).then(response => {
      this.campaignDtlProducts = response as ICampaignDtlProductRapid[];
      if (this.campaignDtlProducts.length > 0) {
        for (let i = 0; i < this.campaignDtlProducts.length; i++) {
          let data = new InvestmentTargetedProd();
          let productData = new Product();
          data = new InvestmentTargetedProd();
          data.productId = this.campaignDtlProducts[i].productId;
          data.investmentInitId = this.investmentFormService.investmentTargetedProdFormData.investmentInitId;
          data.sbu = this.investmentFormService.investmentFormData.sbu;
          data.employeeId = this.investmentFormService.investmentMedicineProdFormData.employeeId;
          productData = new Product();
          productData.productName = this.campaignDtlProducts[i].productInfo.productName;
          productData.productCode = this.campaignDtlProducts[i].productInfo.productCode;
          productData.sbu = this.campaignDtlProducts[i].productInfo.sbu;
          productData.sbuName = this.campaignDtlProducts[i].productInfo.sbuName;
          productData.setOn = this.campaignDtlProducts[i].productInfo.setOn;
          productData.status = this.campaignDtlProducts[i].productInfo.status;
          productData.id = this.campaignDtlProducts[i].productInfo.id;

          data.productInfo = productData;
          this.investmentTargetedProds.push(data);
        }
      }
      if (this.investmentFormService.investmentFormData.id == null || this.investmentFormService.investmentFormData.id == undefined || this.investmentFormService.investmentFormData.id == 0) {
        this.investmentFormService.investmentFormData.approverId = null;
      }
      this.investmentFormService.getEmployeesforRapidByCamp(this.investmentFormService.investmentCampaignFormData.campaignDtlId, parseInt(this.empId)).then(response => {
        this.employees = response as IEmployee[];
      }, error => {
        console.log(error);
      });
    }, error => {
      console.log(error);
    });

  }
  onChangeSocietyInSociety() {
    for (var i = 0; i < this.society.length; i++) {
      if (this.society[i].id == this.investmentFormService.investmentSocietyFormData.societyId) {
        this.investmentFormService.investmentSocietyFormData.societyAddress = this.society[i].societyAddress;
        this.investmentFormService.investmentSocietyFormData.noOfMember = this.society[i].noOfMember;

        break;
      }
    }
    // this.getLastFiveInvestment(this.investmentFormService.investmentFormData.marketCode, this.convertedDate);
  }
  onChangeBcdsInBcds() {
    for (var i = 0; i < this.bcds.length; i++) {
      if (this.bcds[i].id == this.investmentFormService.investmentBcdsFormData.bcdsId) {
        this.investmentFormService.investmentBcdsFormData.bcdsAddress = this.bcds[i].bcdsAddress;
        this.investmentFormService.investmentBcdsFormData.noOfMember = this.bcds[i].noOfMember;
        break;
      }
    }
    // this.getLastFiveInvestment(this.investmentFormService.investmentFormData.marketCode, this.convertedDate);
  }
  /////////////////////////////////////////////////////////////////////////////////
  ChangeSBU() {
    if (this.investmentFormService.investmentFormData.proposeFor == '' || this.investmentFormService.investmentFormData.proposeFor == null || this.investmentFormService.investmentFormData.proposeFor == undefined) {
      this.toastr.warning('Please Select Propose For first!');
      this.investmentFormService.investmentFormData.sbu = 'null';
      return;
    }
    this.getBrand();
    this.getProduct();
    //this.getCampain();
    // if (this.investmentFormService.investmentFormData.subCampaignId != 0) {
    //   this.investmentFormService.investmentFormData.subCampaignId = 0;
    //   this.campaignDtlProducts = [];
    //   this.investmentTargetedProds = [];
    // }
    this.investmentFormService.getEmployeesforRapidBySBU(this.investmentFormService.investmentFormData.proposeFor, this.investmentFormService.investmentFormData.sbu, parseInt(this.empId)).then(response => {
      this.investmentFormService.investmentFormData.approverId = null;
      this.employees = response as IEmployee[];
    }, error => {
      console.log(error);
    });
  }
  ChangeProposeFor() {
    this.subCampaignRapid = [];
    this.investmentFormService.investmentFormData.approverId = null;
    this.investmentFormService.getEmployeesforRapidByDpt(this.investmentFormService.investmentFormData.proposeFor, this.investmentFormService.investmentFormData.sbu, parseInt(this.empId)).subscribe(response => {
      this.employees = response as IEmployee[];
    }, error => {
      console.log(error);
    });
  }
  //////////////////////////////////////////////////////////////////////////////////
  insertInvestmentTargetedProd() {
    if (this.brandCode != "" && this.brandCode != null && this.brandCode != undefined) {
      this.investmentTargetedProds = [];
      for (let j = 0; j < this.prodsByBrand.length; j++) {
        if (this.investmentTargetedProds !== undefined) {
          for (let i = 0; i < this.investmentTargetedProds.length; i++) {
            if (this.investmentTargetedProds[i].productInfo.id === this.prodsByBrand[j].id) {
              this.toastr.warning("Product already exist !");
              return false;
            }
          }
        }
        this.investmentFormService.investmentTargetedProdFormData.investmentInitId = this.investmentFormService.investmentFormData.id;
        this.investmentFormService.investmentTargetedProdFormData.sbu = this.prodsByBrand[j].sbu;
        let data = new InvestmentTargetedProd();
        let productData = new Product();
        data.id = 0;

        data.productId = this.prodsByBrand[j].id;
        data.investmentInitId = this.investmentFormService.investmentTargetedProdFormData.investmentInitId;
        data.sbu = this.prodsByBrand[j].sbu;
        data.employeeId = this.investmentFormService.investmentMedicineProdFormData.employeeId;

        productData.productName = this.prodsByBrand[j].productName;
        productData.productCode = this.prodsByBrand[j].productCode;
        productData.sbu = this.prodsByBrand[j].sbu;
        productData.sbuName = this.prodsByBrand[j].sbuName;
        productData.setOn = this.prodsByBrand[j].setOn;
        productData.status = this.prodsByBrand[j].status;
        productData.id = this.prodsByBrand[j].id;

        data.productInfo = productData;

        this.investmentFormService.investmentTargetedProdFormData.employeeId = parseInt(this.empId);
        // for (let i = 0; i < this.products.length; i++) {
        //   if (this.investmentFormService.investmentTargetedProdFormData.productId == this.products[i].id) {
        //     this.investmentFormService.investmentTargetedProdFormData.sbu = this.products[i].sbu;
        //   }
        // }
        if (this.isSubmitted == true && parseInt(this.empId) == this.investmentFormService.investmentFormData.initiatorId) {
          this.toastr.warning("Investment already submitted");
          return false;
        }
        else {
          this.investmentTargetedProds.push(data);
          this.isDonationValid = true;
          this.SpinnerService.hide();
          this.getInvestmentTargetedProd();
          this.investmentFormService.investmentTargetedProdFormData.productId = null;
        }
      }
      this.toastr.success('Save successfully', 'Targeted  Product');
    }
    else {
      if (this.investmentFormService.investmentTargetedProdFormData.productId == null || this.investmentFormService.investmentTargetedProdFormData.productId == undefined || this.investmentFormService.investmentTargetedProdFormData.productId == 0) {
        this.toastr.warning('Select Brand or Product First', 'Investment Product');
        return false;
      }

      if (this.investmentTargetedProds !== undefined) {
        for (let i = 0; i < this.investmentTargetedProds.length; i++) {
          if (this.investmentTargetedProds[i].productInfo.id === this.investmentFormService.investmentTargetedProdFormData.productId) {
            this.toastr.warning("Product already exist !");
            return false;
          }
        }
      }
      let data = new InvestmentTargetedProd();
      let productData = new Product();
      data.id = 0;

      data.productId = this.investmentFormService.investmentTargetedProdFormData.productId;
      data.investmentInitId = this.investmentFormService.investmentTargetedProdFormData.investmentInitId;
      data.sbu = this.investmentFormService.investmentFormData.sbu;
      data.employeeId = this.investmentFormService.investmentMedicineProdFormData.employeeId;

      productData.productName = this.investmentFormService.investmentTargetedProdFormData.productInfo.productName;
      productData.productCode = this.investmentFormService.investmentTargetedProdFormData.productInfo.productCode;
      productData.sbu = this.investmentFormService.investmentTargetedProdFormData.productInfo.sbu;
      productData.sbuName = this.investmentFormService.investmentTargetedProdFormData.productInfo.sbuName;
      productData.setOn = this.investmentFormService.investmentTargetedProdFormData.productInfo.setOn;
      productData.status = this.investmentFormService.investmentTargetedProdFormData.productInfo.status;
      productData.id = this.investmentFormService.investmentTargetedProdFormData.productInfo.id;

      data.productInfo = productData;
      this.investmentFormService.investmentTargetedProdFormData.investmentInitId = this.investmentFormService.investmentFormData.id;
      this.investmentFormService.investmentTargetedProdFormData.employeeId = parseInt(this.empId);
      for (let i = 0; i < this.products.length; i++) {
        if (this.investmentFormService.investmentTargetedProdFormData.productId == this.products[i].id) {
          this.investmentFormService.investmentTargetedProdFormData.sbu = this.products[i].sbu;
        }
      }
      if (this.isSubmitted == true && parseInt(this.empId) == this.investmentFormService.investmentFormData.initiatorId) {
        this.toastr.warning("Investment already submitted");
        return false;
      }
      else {
        this.investmentTargetedProds.push(data);
        this.isDonationValid = true;
        this.toastr.success('Save successfully', 'Targeted  Product');
        this.SpinnerService.hide();
        this.getInvestmentTargetedProd();
        this.investmentFormService.investmentTargetedProdFormData.productId = null;
      }
    }
  }
  insertInvestmentMedicineProd() {
    if (this.investmentFormService.investmentMedicineProdFormData.productId == null || this.investmentFormService.investmentMedicineProdFormData.productId == undefined || this.investmentFormService.investmentMedicineProdFormData.productId == 0) {
      this.toastr.warning('Select Product First');
      return false;
    }
    if (this.investmentFormService.investmentMedicineProdFormData.boxQuantity == null || this.investmentFormService.investmentMedicineProdFormData.boxQuantity == undefined || this.investmentFormService.investmentMedicineProdFormData.boxQuantity == 0) {
      this.toastr.warning('Insert Box Quantity First');
      return false;
    }
    let data = new InvestmentMedicineProd();
    let medicineProductdata = new MedicineProduct();
    data.id = 0;

    data.productId = this.investmentFormService.investmentMedicineProdFormData.productId;
    data.investmentInitId = this.investmentFormService.investmentMedicineProdFormData.investmentInitId;
    data.boxQuantity = this.investmentFormService.investmentMedicineProdFormData.boxQuantity;
    data.employeeId = this.investmentFormService.investmentMedicineProdFormData.employeeId;

    medicineProductdata.id = this.investmentFormService.investmentMedicineProdFormData.medicineProduct.id;
    medicineProductdata.productCode = this.investmentFormService.investmentMedicineProdFormData.medicineProduct.productCode;
    medicineProductdata.productName = this.investmentFormService.investmentMedicineProdFormData.medicineProduct.productName;
    medicineProductdata.unitTp = this.investmentFormService.investmentMedicineProdFormData.medicineProduct.unitTp;
    medicineProductdata.unitVat = this.investmentFormService.investmentMedicineProdFormData.medicineProduct.unitVat;
    medicineProductdata.sorgaCode = this.investmentFormService.investmentMedicineProdFormData.medicineProduct.sorgaCode;

    data.tpVat = (medicineProductdata.unitTp + medicineProductdata.unitVat) * this.investmentFormService.investmentMedicineProdFormData.boxQuantity;
    data.medicineProduct = medicineProductdata;
    if (this.investmentMedicineProds !== undefined) {

      for (let i = 0; i < this.investmentMedicineProds.length; i++) {
        if (this.investmentMedicineProds[i].medicineProduct.id === data.medicineProduct.id) {
          this.toastr.warning("Product already exist!");
          return false;
        }
      }
    }

    if (this.isSubmitted == true && parseInt(this.empId) == this.investmentFormService.investmentFormData.initiatorId) {
      this.toastr.warning("Investment already submitted");
      return false;
    }
    else {
      this.investmentFormService.investmentMedicineProdFormData.investmentInitId = this.investmentFormService.investmentFormData.id;
      this.investmentFormService.investmentMedicineProdFormData.employeeId = parseInt(this.empId);
      this.SpinnerService.show();



      this.investmentMedicineProds.push(data);
      this.isDonationValid = true;
      this.toastr.success('Save successfully', 'Investment  Product');
      this.SpinnerService.hide();
      this.getInvestmentMedicineProd();
      this.investmentFormService.investmentMedicineProdFormData.productId = null;
      this.investmentFormService.investmentMedicineProdFormData.boxQuantity = null;
      // this.investmentFormService.insertInvestmentMedicineProd().subscribe(
      //   res => {
      //     this.investmentFormService.investmentMedicineProdFormData = new InvestmentMedicineProd();
      //     this.getInvestmentMedicineProd();
      //     this.isDonationValid = true;
      //     this.toastr.success('Save successfully', 'Investment  Product');
      //   },
      //   err => { console.log(err); }
      // );
    }
  }
  ///////////////////////////////////////////////////////////
  removeInvestmentMedicineProd(selectedRecord: IInvestmentMedicineProd) {
    var c = confirm("Are you sure you want to delete that?");
    if (c == true) {
      this.investmentFormService.investmentMedicineProdFormData = Object.assign({}, selectedRecord);
      this.SpinnerService.show();

      const index: number = this.investmentMedicineProds.indexOf(selectedRecord);
      if (index !== -1) {
        this.investmentMedicineProds.splice(index, 1);
        this.getInvestmentMedicineProd();
        this.SpinnerService.hide();
      }
    }
  }
  removeInvestmentTargetedProd(selectedRecord: IInvestmentTargetedProd) {
    var c = confirm("Are you sure you want to delete that?");
    if (c == true) {
      this.investmentFormService.investmentTargetedProdFormData = Object.assign({}, selectedRecord);
      this.SpinnerService.show();

      const index: number = this.investmentTargetedProds.indexOf(selectedRecord);
      if (index !== -1) {
        this.investmentTargetedProds.splice(index, 1);
        this.getInvestmentTargetedProd();
        this.SpinnerService.hide();
      }
    }
  }
  /////////////////////////////////////////////////////////
  openInvestmentInitSearchModal(template: TemplateRef<any>) {
    this.InvestmentInitSearchModalRef = this.modalService.show(template, this.config);
  }
  openSubmissionConfirmModal(template: TemplateRef<any>) {
    this.submissionConfirmRef = this.modalService.show(template, {
      keyboard: false,
      class: 'modal-md',
      ignoreBackdropClick: true
    });
  }
}

export interface ICampaignDtlProductRapid {
  id: number;
  dtlId: number;
  productId: number;
  productInfo: Product;

}

export class CampaignDtlProductRapid implements ICampaignDtlProductRapid {
  id: number = 0;
  dtlId: number;
  productId: number = null;
  productInfo: Product;
}