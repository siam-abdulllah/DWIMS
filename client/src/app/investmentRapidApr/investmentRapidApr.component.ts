
import { InvestmentForm, IInvestmentForm} from '../shared/models/investment';
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
import { CampaignMst, ICampaignMst, CampaignDtl, ICampaignDtl, CampaignDtlProduct, ICampaignDtlProduct,ISubCampaignRapid,SubCampaignRapid  } from '../shared/models/campaign';
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
@Component({
  selector: 'app-investmentRapidApr',
  templateUrl: './investmentRapidApr.component.html',
  styles: [
  ],
  providers: [DatePipe]
})
export class InvestmentRapidAprComponent implements OnInit {
  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  @ViewChild('investmentRapidSearchModal', { static: false }) investmentInitSearchModal: TemplateRef<any>;
  @ViewChild('submissionConfirmModal', { static: false }) submissionConfirmModal: TemplateRef<any>;
  InvestmentInitSearchModalRef: BsModalRef;
  submissionConfirmRef: BsModalRef;
  convertedDate: string;
  empId: string;
  searchText = '';
  //configs: any;
  minDate: Date;
  maxDate: Date;
  degree: any;
  designation: any;
  docInstaddress: any;
  instaddress: any;
  userRole: string;
  sbu: string;
  marketCode: string;
  investmentForms: IInvestmentForm[];
  employees: IEmployee[];
  investmentDoctors: IInvestmentDoctor[];
  depots: IDepotInfo[];
  SBUs: ISBU[];
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
  subCampaignRapid: ISubCampaignRapid[];
  campaignDtls: ICampaignDtl[];
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
  constructor(private accountService: AccountService, public investmentFormService: InvestmentFormService,
   private router: Router,private toastr: ToastrService, private modalService: BsModalService, private datePipe: DatePipe, 
    private SpinnerService: NgxSpinnerService) { }
  ngOnInit() {
    this.convertedDate = this.datePipe.transform(this.today, 'dd/MM/YYYY');
    this.getDonation()
    this.getDepot()
    this.getCampain()
    this.getSBU() 
    this.getEmployees()
    this.getEmployeeId() 
    this.getProduct()
    this.getMedicineProds()
    this.bsConfig = Object.assign({}, { containerClass: 'theme-blue' }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();
    const currentDate = new Date();
    this.minDate = new Date(currentDate.getFullYear(), 0, 1);
    this.maxDate = new Date(currentDate.getFullYear(), 11, 31);
    this.investmentMedicineProds = [];
    this.investmentFormService.investmentMedicineProdFormData.medicineProduct = new MedicineProduct()
    this.investmentTargetedProds = [];
   
    this.investmentFormService.investmentTargetedProdFormData.productInfo = new Product()
    this.investmentFormService.investmentFormData.proposalDateStr = this.convertedDate
  }
  customSearchFn(term: string, item: any) {
    term = term.toLocaleLowerCase();
    return item.employeeSAPCode.toLocaleLowerCase().indexOf(term) > -1 || 
    item.employeeName.toLocaleLowerCase().indexOf(term) > -1;
 }
  getDepot() {
    this.investmentFormService.getDepot().subscribe(response => {
      this.depots = response as IDepotInfo[];
    }, error => {
      console.log(error);
    });
  }
  getCampain() {
    this.investmentFormService.getRapidSubCampaigns(this.investmentFormService.investmentFormData.sbu).subscribe(response => {
      debugger;
      this.subCampaignRapid = response as ISubCampaignRapid[];
    }, error => {
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
  ChangeSBU(){
    this.getProduct();
    this.getCampain();
    if(this.investmentFormService.investmentFormData.subCampaignId != 0)
    {
      this.investmentFormService.investmentFormData.subCampaignId  = 0;
      this.campaignDtlProducts =[];
      this.investmentTargetedProds =[];
    }
  }
  getProduct() {
    debugger;
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
    var data =  this.investmentTargetedProds;
    if (data !== undefined && data.length>0) {
      this.investmentTargetedProds = data;
      
    }
    else {
      this.investmentTargetedProds =[];
    }
    
  }
  resetPageLoad() {

    this.investmentFormService.investmentFormData = new InvestmentForm();
    this.investmentMedicineProds = [];
    this.investmentTargetedProds =[];

  }
  resetSearch() {
    this.searchText = '';
  }
  selectInvestmentRapid(selectedRecord: IInvestmentForm) {

    debugger

    this.investmentFormService.investmentFormData = Object.assign({}, selectedRecord);
    this.getCampain();
     this.investmentFormService.investmentFormData.proposalDateStr =this.datePipe.transform(selectedRecord.propsalDate, 'dd/MM/YYYY');
     if(this.investmentFormService.investmentFormData.type=='4')
     {
        this.investmentFormService.getInvestmentmedicineProducts(selectedRecord.investmentInitId).subscribe(response => {
          this.SpinnerService.hide();
          this.investmentMedicineProds = response as IInvestmentMedicineProd[];
       
        }, error => {
          this.SpinnerService.hide();
          console.log(error);
        });
     }
    this.getInvestmentRecProd()
    this.getProduct()
    this.isDonationValid = true;
  

    this.isValid = true;
    this.InvestmentInitSearchModalRef.hide()
  }
  getEmployees(){
    this.investmentFormService.getEmployeesforrapid().subscribe(response => {
      debugger;
      this.employees = response as IEmployee[];
    //   this.totalCount = response.count;
     }, error => {
        console.log(error);
     });
  }
  confirmSubmission() {
    debugger;
    this.openSubmissionConfirmModal(this.submissionConfirmModal);
  }
  confirmSubmit() {
    this.submissionConfirmRef.hide();
    this.submitInvestmentForm();
  }
  declineSubmit() {
    this.submissionConfirmRef.hide();
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
    this.investmentFormService.investmentFormData.InitiatorId = parseInt(this.empId);
   
  }
  getInvestmentRecProd()
  {
    this.investmentFormService.getInvestmentTargetedProds(this.investmentFormService.investmentFormData.investmentInitId, this.investmentFormService.investmentFormData.sbu).subscribe(response => {

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
  onChangeSubCampaignInCamp() {
    this.investmentTargetedProds =[];
    this.investmentFormService.getCampaignDtlProducts(this.investmentFormService.investmentFormData.subCampaignId).subscribe(response => {
      this.campaignDtlProducts = response as ICampaignDtlProductRapid[];

      let data = new InvestmentTargetedProd();
      let productData = new Product();
      debugger;
      for (let i = 0; i < this.campaignDtlProducts.length; i++) {
        data = new InvestmentTargetedProd();
        data.productId =this.campaignDtlProducts[i].productId;
        data.investmentInitId = this.investmentFormService.investmentTargetedProdFormData.investmentInitId;
        data.sbu = this.investmentFormService.investmentFormData.sbu;
        data.employeeId = this.investmentFormService.investmentMedicineProdFormData.employeeId;
        productData = new Product();
            productData.productName=this.campaignDtlProducts[i].productInfo.productName;
            productData.productCode=this.campaignDtlProducts[i].productInfo.productCode;
            productData.sbu=this.campaignDtlProducts[i].productInfo.sbu;
            productData.sbuName=this.campaignDtlProducts[i].productInfo.sbuName;
            productData.setOn=this.campaignDtlProducts[i].productInfo.setOn;
            productData.status=this.campaignDtlProducts[i].productInfo.status;
            productData.id=this.campaignDtlProducts[i].productInfo.id;
          
        data.productInfo = productData;
        this.investmentTargetedProds.push(data);
      }
    }, error => {
      console.log(error);
    });

  }
  onChangeProduct() {
    debugger;
    if (this.medicineProducts !== undefined) {
      for (let i = 0; i < this.medicineProducts.length; i++) {
        if (this.medicineProducts[i].id === this.investmentFormService.investmentMedicineProdFormData.productId) {
          debugger;

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
    debugger;
    if (this.products !== undefined) {
      for (let i = 0; i < this.products.length; i++) {
        if (this.products[i].id === this.investmentFormService.investmentTargetedProdFormData.productId) {
          debugger;
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
    if(this.investmentFormService.investmentFormData.paymentMethod != "Cash")
    {
 
      this.investmentFormService.investmentFormData.depotCode = null;
    }
    if (this.investmentFormService.investmentFormData.paymentMethod != "Cheque")
    {
      this.investmentFormService.investmentFormData.chequeTitle = "";
    } 
  }

  insertInvestmentTargetedProd() {
    

    if (this.investmentFormService.investmentTargetedProdFormData.productId == null || this.investmentFormService.investmentTargetedProdFormData.productId == undefined || this.investmentFormService.investmentTargetedProdFormData.productId == 0) {
      this.toastr.warning('Select Product First', 'Investment Product');
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

    data.productId =this.investmentFormService.investmentTargetedProdFormData.productId;
    data.investmentInitId = this.investmentFormService.investmentTargetedProdFormData.investmentInitId;
    data.sbu = this.investmentFormService.investmentFormData.sbu;
    data.employeeId = this.investmentFormService.investmentMedicineProdFormData.employeeId;

    productData.productName=this.investmentFormService.investmentTargetedProdFormData.productInfo.productName;
    productData.productCode=this.investmentFormService.investmentTargetedProdFormData.productInfo.productCode;
    productData.sbu=this.investmentFormService.investmentTargetedProdFormData.productInfo.sbu;
    productData.sbuName=this.investmentFormService.investmentTargetedProdFormData.productInfo.sbuName;
    productData.setOn=this.investmentFormService.investmentTargetedProdFormData.productInfo.setOn;
    productData.status=this.investmentFormService.investmentTargetedProdFormData.productInfo.status;
    productData.id=this.investmentFormService.investmentTargetedProdFormData.productInfo.id;
  
    data.productInfo = productData;
    this.investmentFormService.investmentTargetedProdFormData.investmentInitId = this.investmentFormService.investmentFormData.id;
    this.investmentFormService.investmentTargetedProdFormData.employeeId = parseInt(this.empId);
    for (let i = 0; i < this.products.length; i++) {
      if (this.investmentFormService.investmentTargetedProdFormData.productId == this.products[i].id) {
        this.investmentFormService.investmentTargetedProdFormData.sbu = this.products[i].sbu;
      }
    }
    if (this.isSubmitted == true && parseInt(this.empId) == this.investmentFormService.investmentFormData.InitiatorId) {
      this.toastr.warning("Investment already submitted");
      return false;
    }
    else {

      debugger;
 

      this.investmentTargetedProds.push(data);
      this.isDonationValid = true;
      this.toastr.success('Save successfully', 'Targeted  Product');
      this.SpinnerService.hide();
      this.getInvestmentTargetedProd();
      this.investmentFormService.investmentTargetedProdFormData.productId = null;
  
      // if (this.investmentFormService.investmentTargetedProdFormData.id == null || this.investmentFormService.investmentTargetedProdFormData.id == undefined || this.investmentFormService.investmentTargetedProdFormData.id == 0) {
      //   this.SpinnerService.show();
      //   // this.investmentFormService.insertInvestmentTargetedProd().subscribe(
      //   //   res => {
      //   //     this.investmentFormService.investmentTargetedProdFormData = new InvestmentTargetedProd();

      //   //     this.getInvestmentTargetedProd();

      //   //     this.isDonationValid = true;
      //   //     this.toastr.success('Save successfully', 'Investment  Product');
      //   //   },
      //   //   err => { console.log(err); }
      //   // );
      // }
      // else {
      //   this.SpinnerService.show();
      //   // this.investmentFormService.updateInvestmentTargetedProd().subscribe(
      //   //   res => {
      //   //     this.investmentFormService.investmentTargetedProdFormData = new InvestmentTargetedProd();
      //   //     this.getInvestmentTargetedProd();
      //   //     this.isDonationValid = true;
      //   //     this.toastr.success('Update successfully', 'Investment  Product');
      //   //   },
      //   //   err => { console.log(err); }
      //   // );
      // }
    }
  }
  insertInvestmentMedicineProd() {
   
    debugger;
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

    data.productId =this.investmentFormService.investmentMedicineProdFormData.productId;
    data.investmentInitId = this.investmentFormService.investmentMedicineProdFormData.investmentInitId;
    data.boxQuantity = this.investmentFormService.investmentMedicineProdFormData.boxQuantity;
    data.employeeId = this.investmentFormService.investmentMedicineProdFormData.employeeId;

    medicineProductdata.id =  this.investmentFormService.investmentMedicineProdFormData.medicineProduct.id;
    medicineProductdata.productCode =  this.investmentFormService.investmentMedicineProdFormData.medicineProduct.productCode;
    medicineProductdata.productName =  this.investmentFormService.investmentMedicineProdFormData.medicineProduct.productName;
    medicineProductdata.unitTp =  this.investmentFormService.investmentMedicineProdFormData.medicineProduct.unitTp;
    medicineProductdata.unitVat =  this.investmentFormService.investmentMedicineProdFormData.medicineProduct.unitVat;
    medicineProductdata.sorgaCode =  this.investmentFormService.investmentMedicineProdFormData.medicineProduct.sorgaCode;
    debugger;
    data.tpVat =     (medicineProductdata.unitTp+ medicineProductdata.unitVat)*this.investmentFormService.investmentMedicineProdFormData.boxQuantity;
    data.medicineProduct = medicineProductdata;
    if (this.investmentMedicineProds !== undefined) {
 
      for (let i = 0; i < this.investmentMedicineProds.length; i++) {
        if (this.investmentMedicineProds[i].medicineProduct.id === data.medicineProduct.id) {
          this.toastr.warning("Product already exist!");
          return false;
        }
      }
    }
    if (this.isSubmitted == true && parseInt(this.empId) == this.investmentFormService.investmentFormData.InitiatorId) {
      this.toastr.warning("Investment already submitted");
      return false;
    }
    else {
      this.investmentFormService.investmentMedicineProdFormData.investmentInitId = this.investmentFormService.investmentFormData.id;
      this.investmentFormService.investmentMedicineProdFormData.employeeId = parseInt(this.empId);
        this.SpinnerService.show();
        debugger;
 

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

  removeInvestmentMedicineProd(selectedRecord: IInvestmentMedicineProd) {
    var c = confirm("Are you sure you want to delete that?");
    if (c == true) {
      this.investmentFormService.investmentMedicineProdFormData = Object.assign({}, selectedRecord);
      this.SpinnerService.show();
      debugger;
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
      debugger;
      const index: number = this.investmentTargetedProds.indexOf(selectedRecord);
      if (index !== -1) {
          this.investmentTargetedProds.splice(index, 1);
          this.getInvestmentTargetedProd();
          this.SpinnerService.hide();
      }
    }
  }
  getInvestmentMedicineProd() {
    debugger;
    var data =  this.investmentMedicineProds;

    if (data !== undefined && data.length>0) {
      this.investmentMedicineProds = data;
      let sum=0;
      for (let i = 0; i < this.investmentMedicineProds.length; i++) {
        sum=sum+this.investmentMedicineProds[i].tpVat;
      }
      //this.investmentInitService.investmentDetailFormData.proposedAmount=sum.toString();
      this.investmentFormService.investmentFormData.proposedAmount=((Math.round(sum * 100) / 100).toFixed(2));
    }
    else {
      this.investmentFormService.investmentDetailFormData.proposedAmount='';
      this.investmentMedicineProds =[];
    }
  }
  openInvestmentInitSearchModal(template: TemplateRef<any>) {
    this.InvestmentInitSearchModalRef = this.modalService.show(template, this.config);
  }
  getInvestmentRapid(For:string) {
    debugger;
    const params = this.investmentFormService.getGenParams();
    this.SpinnerService.show();
    this.investmentFormService.getInvestmentRapids(parseInt(this.empId),"appr",For).subscribe(response => {
      this.SpinnerService.hide();
      this.investmentForms = response as IInvestmentForm[];
     debugger;
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
  submitInvestmentForm() {
    debugger;
    if(this.investmentFormService.investmentFormData.paymentMethod == 'Cash' && this.investmentFormService.investmentFormData.depotCode == null )
    {
      this.toastr.warning('Depot is required!');
    }
    else if(this.investmentFormService.investmentFormData.paymentMethod == 'Cheque' && this.investmentFormService.investmentFormData.chequeTitle == null)
    {
      this.toastr.warning('Cheque Title is required!');
    }
    else if((this.investmentTargetedProds == null ||  this.investmentTargetedProds.length == 0) && (this.investmentMedicineProds == null || this.investmentMedicineProds.length == 0))
    {
      this.toastr.warning('No Product is added.');
    }
    else if((this.investmentTargetedProds == null ||  this.investmentTargetedProds.length == 0) && (this.investmentMedicineProds == null || this.investmentMedicineProds.length == 0))
    {
      this.toastr.warning('No Product is added.');
    }
    else{
      debugger;
      this.SpinnerService.show();
      this.investmentFormService.investmentFormData.investmentMedicineProd = this.investmentMedicineProds;
      this.investmentFormService.investmentFormData.investmentRecProducts = this.investmentTargetedProds;
      for (let i = 0; i < this.depots.length; i++) {
        if (this.depots[i].depotCode == this.investmentFormService.investmentFormData.depotCode) {
          this.investmentFormService.investmentFormData.depotName = this.depots[i].depotName;
          break;
        }
      }
      for (let i = 0; i < this.subCampaignRapid.length; i++) {
        debugger;
        if (this.subCampaignRapid[i].subCampId == this.investmentFormService.investmentFormData.subCampaignId) {
          this.investmentFormService.investmentFormData.subCampaignName = this.subCampaignRapid[i].subCampaignName;
          break;
        }
      }
      for (let i = 0; i < this.SBUs.length; i++) {
        debugger;
        if (this.SBUs[i].sbuCode == this.investmentFormService.investmentFormData.sbu) {
          this.investmentFormService.investmentFormData.sbuName = this.SBUs[i].sbuName;
          break;
        }
      }
      this.investmentFormService.submitInvestment().subscribe(
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
  id: number=0;
  dtlId: number;
  productId: number=null;
  productInfo: Product;
}
