
import { InvestmentInit, IInvestmentInit,InvestmentDetail,IInvestmentDetail,InvestmentTargetedProd,IInvestmentTargetedProd } from '../shared/models/investment';
import { InvestmentDoctor, IInvestmentDoctor,InvestmentInstitution,IInvestmentInstitution,InvestmentCampaign,IInvestmentCampaign } from '../shared/models/investment';
import { InvestmentBcds, IInvestmentBcds,InvestmentSociety,IInvestmentSociety } from '../shared/models/investment';
import { SubCampaign, ISubCampaign } from '../shared/models/subCampaign';
import { Donation, IDonation } from '../shared/models/donation';
import { Doctor, IDoctor } from '../shared/models/docotor';
import { Institution, IInstitution } from '../shared/models/institution';
import { GenericParams } from '../shared/models/genericParams';
import { Component, ElementRef, OnInit, ViewChild , TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { InvestmentInitService } from '../_services/investment.service';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService} from 'ngx-bootstrap/modal';
import { Product, IProduct } from '../shared/models/product';
import { Market, IMarket } from '../shared/models/market';
@Component({
  selector: 'app-investmentInit',
  templateUrl: './investmentInit.component.html',
  styles: [
  ]
})
export class InvestmentInitComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  @ViewChild('investmentInitSearchModal', { static: false }) investmentInitSearchModal: TemplateRef<any>;
  InvestmentInitSearchModalRef: BsModalRef;
  // genParams: GenericParams;
  // campaigns: ICampaign[]; 
  investmentInits: IInvestmentInit[];
  investmentDoctors: IInvestmentDoctor[];
  isValid: boolean=false; 
  isDonationValid: boolean=false; 
  markets: IMarket[]; 
  products: IProduct[];
  doctors: IDoctor[];
  institutions: IInstitution[];
  donations: IDonation[];
  donationToVal:string;
  totalCount = 0;
  bsConfig: Partial<BsDatepickerConfig>;
  bsValue: Date = new Date();
  config = {
    keyboard: false,
    class: 'modal-lg',
    ignoreBackdropClick: true
  };
  constructor(public investmentInitService: InvestmentInitService, private router: Router,
    private toastr: ToastrService,private modalService: BsModalService) { }

    openInvestmentInitSearchModal(template: TemplateRef<any>) {
      this.InvestmentInitSearchModalRef = this.modalService.show(template, this.config);
    }
    selectInvestmentInit(selectedRecord: IInvestmentInit) {
      this.investmentInitService.investmentInitFormData = Object.assign({}, selectedRecord);
      this.investmentInitService.investmentDoctorFormData.investmentInitId =selectedRecord.id;
      this.investmentInitService.investmentInstitutionFormData.investmentInitId =selectedRecord.id;
      this.isDonationValid=true;
      if(this.investmentInitService.investmentInitFormData.donationTo=="Doctor")
    {
      this.getDoctor();
      this. getInstitution();
      this.getInvestmentDoctor();
    }
      else if(this.investmentInitService.investmentInitFormData.donationTo=="Institution")
    {
      this.getDoctor();
      this. getInstitution();
      this.getInvestmentInstitution();
    }
      //this.getInvestmentInits();
      this.isValid=true;
      this.InvestmentInitSearchModalRef.hide()
     }
     getInvestmentInit(){
      this.investmentInitService.getInvestmentInit().subscribe(response => {
        //debugger;
       this.investmentInits = response.data;
       this.openInvestmentInitSearchModal(this.investmentInitSearchModal);
      }, error => {
          console.log(error);
     });
   }
   getInvestmentInstitution(){
    this.investmentInitService.getInvestmentInstitutions(this.investmentInitService.investmentInstitutionFormData.investmentInitId).subscribe(response => {
      debugger;
      
      var data=response[0] as IInvestmentInstitution;
      if(data!==undefined)
      {
      this.investmentInitService.investmentInstitutionFormData=data;
      this.onChangeInstitutionInInst();
    }
    else{
      this.toastr.warning('No Data Found', 'Investment ');
    }
      
    }, error => {
        console.log(error);
    });
  }
   getInvestmentDoctor(){
    this.investmentInitService.getInvestmentDoctors(this.investmentInitService.investmentDoctorFormData.investmentInitId).subscribe(response => {
      debugger;
      var data=response[0] as IInvestmentDoctor;
      if( data!==undefined)
      {
      this.investmentInitService.investmentDoctorFormData=data;
      this.investmentInitService.investmentDoctorFormData.doctorName=String(data.doctorId);
      this.onChangeDoctorInDoc();
      this.onChangeInstitutionInDoc();
    }
    else{
      this.toastr.warning('No Data Found', 'Investment ');
    }
      
    }, error => {
        console.log(error);
    });
  }
  ngOnInit() {
    this.getDonation();
    this.bsConfig = Object.assign({}, { containerClass: 'theme-green' }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();
  }
  onChangeDonationTo(){
    debugger;
    if(this.investmentInitService.investmentInitFormData.donationTo=="Doctor" )
    {
      if(this.investmentInitService.investmentDoctorFormData.id==null || this.investmentInitService.investmentDoctorFormData.id==undefined || this.investmentInitService.investmentDoctorFormData.id==0)
      {
      this.investmentInitService.investmentDoctorFormData=new InvestmentDoctor();
      this.getDoctor();
      this. getInstitution();
     }
    }
    else if(this.investmentInitService.investmentInitFormData.donationTo=="Institution")
    {if(this.investmentInitService.investmentInstitutionFormData.id==null || this.investmentInitService.investmentInstitutionFormData.id==undefined || this.investmentInitService.investmentInstitutionFormData.id==0)
      {
      this.investmentInitService.investmentDoctorFormData=new InvestmentDoctor();
      this.getDoctor();
      this. getInstitution();
      }
    }
  }
  onChangeDoctorInDoc(){
    for(var i=0;i<this.doctors.length;i++){
    if(this.doctors[i].id==parseInt(this.investmentInitService.investmentDoctorFormData.doctorName))
    {
      //this.investmentInitService.investmentDoctorFormData.doctorName=this.doctors[i].doctorName;
      this.investmentInitService.investmentDoctorFormData.doctorId=this.doctors[i].id;
      this.investmentInitService.investmentDoctorFormData.degree=this.doctors[i].degree;
      this.investmentInitService.investmentDoctorFormData.designation=this.doctors[i].designation;
      break;
    }
  }
  }
  onChangeInstitutionInDoc(){
    debugger;
    for(var i=0;i<this.doctors.length;i++){
    if(this.institutions[i].id==this.investmentInitService.investmentDoctorFormData.institutionId)
    {
      this.investmentInitService.investmentDoctorFormData.address=this.institutions[i].address;
      
      break;
    }
  }
  }
  onChangeInstitutionInInst(){
    //debugger;
    for(var i=0;i<this.doctors.length;i++){
    if(this.institutions[i].id==this.investmentInitService.investmentInstitutionFormData.institutionId)
    {
      this.investmentInitService.investmentInstitutionFormData.address=this.institutions[i].address;
      this.investmentInitService.investmentInstitutionFormData.institutionType=this.institutions[i].institutionType;
      
      break;
    }
  }
  }
  getDonation(){
    this.investmentInitService.getDonations().subscribe(response => {
      this.donations = response as IDonation[];
    }, error => {
        console.log(error);
    });
  }
  getDoctor(){
    this.investmentInitService.getDoctors().subscribe(response => {
      //debugger;
      this.doctors = response as IDoctor[];
    }, error => {
        console.log(error);
    });
  }
  getInstitution(){
    this.investmentInitService.getInstitutions().subscribe(response => {
      this.institutions = response as IInstitution[];
    }, error => {
        console.log(error);
    });
  }
  getMarket(){
    this.investmentInitService.getMarkets().subscribe(response => {
     this.markets = response as IMarket[];
    }, error => {
        console.log(error);
   });
 }
 getProduct(){
  this.investmentInitService.getProduct().subscribe(response => {
    //debugger;
    this.products = response as IProduct[];
  }, error => {
      console.log(error);
  });
}
  onSubmit(form: NgForm) {
    if (this.investmentInitService.investmentInitFormData.id == 0)
      this.insertInvestmentInit();
    else
      this.updateInvestmentInit();
  }
  insertInvestmentInit() {
    this.investmentInitService.insertInvestmentInit().subscribe(
      res => {
        //debugger;
       this.investmentInitService.investmentInitFormData=res as IInvestmentInit;
       this.investmentInitService.investmentDoctorFormData.investmentInitId=this.investmentInitService.investmentInitFormData.id;
       this.investmentInitService.investmentInstitutionFormData.investmentInitId=this.investmentInitService.investmentInitFormData.id;
        this.isValid=true;
        this.toastr.success('Submitted successfully', 'Investment ')
      },
      err => { console.log(err); }
    );
  }
  
  updateInvestmentInit() {
    this.investmentInitService.updateInvestmentInit().subscribe(
      res => {
        debugger;
        this.isValid=true;
        this.investmentInitService.investmentDoctorFormData.investmentInitId=this.investmentInitService.investmentInitFormData.id;
       this.investmentInitService.investmentInstitutionFormData.investmentInitId=this.investmentInitService.investmentInitFormData.id;
        this.toastr.info('Updated successfully', 'Investment ')
      },
      err => { console.log(err); }
    );
  }
  insertInvestmentDoctor() {
    if(this.investmentInitService.investmentDoctorFormData.investmentInitId==null || this.investmentInitService.investmentDoctorFormData.investmentInitId==undefined || this.investmentInitService.investmentDoctorFormData.investmentInitId==0)
    {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment ');
     return false;
    }
    // if(this.investmentInitService.investmentInitFormData.donationTo!=="Doctor")
    // {
    //   this.updateInvestmentInit();
    // }
    if(this.investmentInitService.investmentDoctorFormData.doctorId==null || this.investmentInitService.investmentDoctorFormData.doctorId==undefined || this.investmentInitService.investmentDoctorFormData.doctorId==0)
    {
      this.toastr.warning('Select Doctor First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    if(this.investmentInitService.investmentDoctorFormData.institutionId==null || this.investmentInitService.investmentDoctorFormData.institutionId==undefined || this.investmentInitService.investmentDoctorFormData.institutionId==0)
    {
      this.toastr.warning('Select Institute First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    this.investmentInitService.insertInvestmentDoctor().subscribe(
      res => {
       var data=res as IInvestmentDoctor;
       this.investmentInitService.investmentDoctorFormData=data;
       this.investmentInitService.investmentDoctorFormData.doctorName=String(data.doctorId);
       this.onChangeDoctorInDoc();
       this.onChangeInstitutionInDoc();
       this.updateInvestmentInit();
       this.toastr.success('Save successfully', 'Investment ');
      },
      err => { console.log(err); }
    );
  }
  insertInvestmentInstitution() {
    debugger;
    if(this.investmentInitService.investmentInstitutionFormData.investmentInitId==null || this.investmentInitService.investmentInstitutionFormData.investmentInitId==undefined || this.investmentInitService.investmentInstitutionFormData.investmentInitId==0)
    {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    // if(this.investmentInitService.investmentInitFormData.donationTo!=="Institution")
    // {
      
    // }
    if(this.investmentInitService.investmentInstitutionFormData.resposnsibleDoctorId==null || this.investmentInitService.investmentInstitutionFormData.resposnsibleDoctorId==undefined || this.investmentInitService.investmentInstitutionFormData.resposnsibleDoctorId==0)
    {
      this.toastr.warning('Select Institution First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    if(this.investmentInitService.investmentInstitutionFormData.institutionId==null || this.investmentInitService.investmentInstitutionFormData.institutionId==undefined || this.investmentInitService.investmentInstitutionFormData.institutionId==0)
    {
      this.toastr.warning('Select Institute First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    this.investmentInitService.insertInvestmentInstitution().subscribe(
      res => {
        debugger;
       this.investmentInitService.investmentInstitutionFormData=res as IInvestmentInstitution;
       this.onChangeInstitutionInInst();
       this.updateInvestmentInit();
        this.toastr.success('Save successfully', 'Investment ');
      },
      err => { console.log(err); }
    );
  }
  populateForm() {
    //this.investmentInitService.campaignFormData = Object.assign({}, selectedRecord);
  }
  resetPage(form: NgForm) {
    form.reset();
    this.investmentInitService.investmentInitFormData=new InvestmentInit();
    this.isValid=false;
  }
  removeInvestmentDoctor() {
    var c = confirm("Are you sure you want to delete that?"); 
    if (c == true) {  
    this.investmentInitService.removeInvestmentDoctor().subscribe(
      res => {
        this.toastr.success(res);
        this.isDonationValid=false;
        this.investmentInitService.investmentDoctorFormData=new InvestmentDoctor();
      },
      err => { debugger;console.log(err); }
    );
  }
  }
  removeInvestmentInstitute() {
    var c = confirm("Are you sure you want to delete that?"); 
    if (c == true) {  
    this.investmentInitService.removeInvestmentInstitution().subscribe(
      res => {
        debugger;
        this.toastr.success(res);
        this.isDonationValid=true;
        this.investmentInitService.investmentInstitutionFormData=new InvestmentInstitution();
      },
      err => { console.log(err); }
    );
  }
}
}
