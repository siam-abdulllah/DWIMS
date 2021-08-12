
import { InvestmentRec, IInvestmentRec,InvestmentDetail,IInvestmentDetail,
  InvestmentTargetedProd,IInvestmentTargetedProd,InvestmentTargetedGroup,IInvestmentTargetedGroup } from '../shared/models/investment';
import { InvestmentDoctor, IInvestmentDoctor,InvestmentInstitution,IInvestmentInstitution,InvestmentCampaign,IInvestmentCampaign } from '../shared/models/investment';
import { InvestmentBcds, IInvestmentBcds,InvestmentSociety,IInvestmentSociety } from '../shared/models/investment';
import { SubCampaign, ISubCampaign } from '../shared/models/subCampaign';
import { Donation, IDonation } from '../shared/models/donation';
import { Doctor, IDoctor } from '../shared/models/docotor';
import { Institution, IInstitution } from '../shared/models/institution';
import { GenericParams } from '../shared/models/genericParams';
import { Component, ElementRef, OnInit, ViewChild , TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { InvestmentRecService } from '../_services/investmentRec.service';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService} from 'ngx-bootstrap/modal';
import { Product, IProduct } from '../shared/models/product';
import { Market, IMarket } from '../shared/models/market';
import { CampaignMst, ICampaignMst,CampaignDtl, ICampaignDtl,CampaignDtlProduct, ICampaignDtlProduct } from '../shared/models/campaign';
import { DatePipe } from '@angular/common';
import { IBcdsInfo } from '../shared/models/bcdsInfo';
import { ISocietyInfo } from '../shared/models/societyInfo';
import { MarketGroupMst, IMarketGroupMst } from '../shared/models/marketGroupMst';
import { MarketGroupDtl, IMarketGroupDtl } from '../shared/models/marketGroupDtl';
@Component({
  selector: 'app-investmentRec',
  templateUrl: './investmentRec.component.html',
  styles: [
  ],
  providers: [DatePipe]
})
export class InvestmentRecComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  @ViewChild('investmentRecSearchModal', { static: false }) investmentRecSearchModal: TemplateRef<any>;
  InvestmentRecSearchModalRef: BsModalRef;
  // genParams: GenericParams;
  // campaigns: ICampaign[]; 
  investmentRecs: IInvestmentRec[];
  investmentTargetedProds: IInvestmentTargetedProd[];
  investmentTargetedGroups: IInvestmentTargetedGroup[];
  investmentDetails: IInvestmentDetail[];
  investmentDoctors: IInvestmentDoctor[];
  isValid: boolean=false; 
  isDonationValid: boolean=false; 
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
  donationToVal:string;
  totalCount = 0;
  bsConfig: Partial<BsDatepickerConfig>;
  bsValue: Date = new Date();
  config = {
    keyboard: false,
    class: 'modal-lg',
    ignoreBackdropClick: true
  };
  constructor(public investmentRecService: InvestmentRecService, private router: Router,
    private toastr: ToastrService,private modalService: BsModalService,private datePipe: DatePipe) { }

    openInvestmentRecSearchModal(template: TemplateRef<any>) {
      this.InvestmentRecSearchModalRef = this.modalService.show(template, this.config);
    }
    selectInvestmentRec(selectedRecord: IInvestmentRec) {
      this.investmentRecService.investmentRecFormData = Object.assign({}, selectedRecord);
      // this.investmentRecService.investmentDoctorFormData.investmentInitId =selectedRecord.id;
      // this.investmentRecService.investmentInstitutionFormData.investmentInitId =selectedRecord.id;
      // this.investmentRecService.investmentCampaignFormData.investmentInitId =selectedRecord.id;
      // this.investmentRecService.investmentBcdsFormData.investmentInitId =selectedRecord.id;
      // this.investmentRecService.investmentSocietyFormData.investmentInitId =selectedRecord.id;
      // this.investmentRecService.investmentDetailFormData.investmentInitId =selectedRecord.id;
      this.isDonationValid=true;
      if(this.investmentRecService.investmentRecFormData.donationTo=="Doctor")
    {
      
      this.getDoctor();
      this.getInstitution();
      this.getInvestmentDoctor();
    }
      else if(this.investmentRecService.investmentRecFormData.donationTo=="Institution")
    {
      
      this.getDoctor();
      this.getInstitution();
      this.getInvestmentInstitution();
    }
      else if(this.investmentRecService.investmentRecFormData.donationTo=="Campaign")
    {
      this.getCampaignMst();
      this.getDoctor();
      this.getInstitution();
      this.getInvestmentCampaign();
    }
      else if(this.investmentRecService.investmentRecFormData.donationTo=="Bcds")
    {
      this.getBcds();
      this.getInvestmentBcds();
    }
      else if(this.investmentRecService.investmentRecFormData.donationTo=="Society")
    {
      this.getSociety();
      this.getInvestmentSociety();
    }
      this.getInvestmentDetails();
      this.getInvestmentTargetedProd();
      this.getInvestmentTargetedGroup();
      this.isValid=true;
      this.InvestmentRecSearchModalRef.hide()
     }
     getInvestmentRec(){
      this.investmentRecService.getInvestmentRec().subscribe(response => {
        //debugger;
       this.investmentRecs = response.data;
       this.openInvestmentRecSearchModal(this.investmentRecSearchModal);
      }, error => {
          console.log(error);
     });
   }
   getInvestmentDetails(){
      this.investmentRecService.getInvestmentDetails(this.investmentRecService.investmentDetailFormData.investmentInitId ).subscribe(response => {
        debugger;
        var data=response[0] as IInvestmentDetail;
        if(data!==undefined)
        {
        this.investmentRecService.investmentDetailFormData=data;
       this.investmentRecService.investmentDetailFormData.fromDate=new Date(data.fromDate);
        this.investmentRecService.investmentDetailFormData.toDate=new Date(data.toDate);
      } else{
        this.toastr.warning('No Data Found', 'Investment ');
      }
         }, error => {
          console.log(error);
     });
   }
   getInvestmentCampaign(){
    this.investmentRecService.getInvestmentCampaigns(this.investmentRecService.investmentCampaignFormData.investmentInitId).subscribe(response => {
      debugger;
      
      var data=response[0] as IInvestmentCampaign;
      if(data!==undefined)
      {
      this.investmentRecService.investmentCampaignFormData=data;

      this.investmentRecService.investmentCampaignFormData.campaignMstId=data.campaignDtl.mstId;
      this.investmentRecService.investmentCampaignFormData.subCampStartDate=new DatePipe('en-US').transform(data.campaignDtl.subCampStartDate, 'dd/MM/yyyy');
      this.investmentRecService.investmentCampaignFormData.subCampEndDate=new DatePipe('en-US').transform(data.campaignDtl.subCampEndDate, 'dd/MM/yyyy');
     
      this.onChangeCampaignInCamp();
      this.onChangeSubCampaignInCamp();
    }
    else{
      this.toastr.warning('No Data Found', 'Investment ');
    }
      
    }, error => {
        console.log(error);
    });
  }
   getInvestmentBcds(){
    this.investmentRecService.getInvestmentBcds(this.investmentRecService.investmentBcdsFormData.investmentInitId).subscribe(response => {
      debugger;
      
      var data=response[0] as IInvestmentBcds;
      if(data!==undefined)
      {
      this.investmentRecService.investmentBcdsFormData=data;
      
      this.onChangeBcdsInBcds();
    }
    else{
      this.toastr.warning('No Data Found', 'Investment ');
    }
      
    }, error => {
        console.log(error);
    });
  }
   getInvestmentSociety(){
    this.investmentRecService.getInvestmentSociety(this.investmentRecService.investmentSocietyFormData.investmentInitId).subscribe(response => {
      debugger;
      
      var data=response[0] as IInvestmentSociety;
      if(data!==undefined)
      {
      this.investmentRecService.investmentSocietyFormData=data;
      
      this.onChangeSocietyInSociety();
    }
    else{
      this.toastr.warning('No Data Found', 'Investment ');
    }
      
    }, error => {
        console.log(error);
    });
  }
   getInvestmentInstitution(){
    this.investmentRecService.getInvestmentInstitutions(this.investmentRecService.investmentInstitutionFormData.investmentInitId).subscribe(response => {
      debugger;
      
      var data=response[0] as IInvestmentInstitution;
      if(data!==undefined)
      {
      this.investmentRecService.investmentInstitutionFormData=data;
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
    this.investmentRecService.getInvestmentDoctors(this.investmentRecService.investmentDoctorFormData.investmentInitId).subscribe(response => {
      debugger;
      var data=response[0] as IInvestmentDoctor;
      if( data!==undefined)
      {
      this.investmentRecService.investmentDoctorFormData=data;
      this.investmentRecService.investmentDoctorFormData.doctorName=String(data.doctorId);
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
   getInvestmentTargetedProd(){
    this.investmentRecService.getInvestmentTargetedProds(this.investmentRecService.investmentRecFormData.id).subscribe(response => {
      debugger;
      var data=response as IInvestmentTargetedProd[];
      if( data!==undefined)
      {
      this.investmentTargetedProds=data;
     
    }
    else{
      this.toastr.warning('No Data Found', 'Investment ');
    }
      
    }, error => {
        console.log(error);
    });
  }
   getInvestmentTargetedGroup(){
    this.investmentRecService.getInvestmentTargetedGroups(this.investmentRecService.investmentRecFormData.id).subscribe(response => {
      debugger;
      var data=response as IInvestmentTargetedGroup[];
      if( data!==undefined)
      {
      this.investmentTargetedGroups=data;
     
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
    this.getProduct();
    this.getMarketGroupMsts();
    this.bsConfig = Object.assign({}, { containerClass: 'theme-green' }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();

  }
  onChangeDonationTo(){
    debugger;
    
    if(this.investmentRecService.investmentRecFormData.donationTo=="Doctor" )
    {
      if(this.investmentRecService.investmentDoctorFormData.id==null || this.investmentRecService.investmentDoctorFormData.id==undefined || this.investmentRecService.investmentDoctorFormData.id==0)
      {
      this.investmentRecService.investmentDoctorFormData=new InvestmentDoctor();
      this.getDoctor();
      this. getInstitution();
     }
    }
    else if(this.investmentRecService.investmentRecFormData.donationTo=="Institution")
    {if(this.investmentRecService.investmentInstitutionFormData.id==null || this.investmentRecService.investmentInstitutionFormData.id==undefined || this.investmentRecService.investmentInstitutionFormData.id==0)
      {
      this.investmentRecService.investmentDoctorFormData=new InvestmentDoctor();
      this.getDoctor();
      this. getInstitution();
      }
    }
    else if(this.investmentRecService.investmentRecFormData.donationTo=="Campaign")
    {if(this.investmentRecService.investmentCampaignFormData.id==null || this.investmentRecService.investmentCampaignFormData.id==undefined || this.investmentRecService.investmentCampaignFormData.id==0)
      {
      this.investmentRecService.investmentCampaignFormData=new InvestmentCampaign();
      this.getCampaignMst();
      this.getDoctor();
      this. getInstitution();
      }
    }
    else if(this.investmentRecService.investmentRecFormData.donationTo=="Bcds")
    {if(this.investmentRecService.investmentBcdsFormData.id==null || this.investmentRecService.investmentBcdsFormData.id==undefined || this.investmentRecService.investmentBcdsFormData.id==0)
      {
      this.investmentRecService.investmentBcdsFormData=new InvestmentBcds();
      this.getBcds();
      }
    }
    else if(this.investmentRecService.investmentRecFormData.donationTo=="Society")
    {if(this.investmentRecService.investmentSocietyFormData.id==null || this.investmentRecService.investmentSocietyFormData.id==undefined || this.investmentRecService.investmentSocietyFormData.id==0)
      {
      this.investmentRecService.investmentSocietyFormData=new InvestmentSociety();
      this.getSociety();
      }
    }
    if(this.investmentRecService.investmentRecFormData.id!=null && this.investmentRecService.investmentRecFormData.id!=undefined && this.investmentRecService.investmentRecFormData.id!=0)
    {
      this.investmentRecService.investmentDoctorFormData.investmentInitId =this.investmentRecService.investmentRecFormData.id;
      this.investmentRecService.investmentInstitutionFormData.investmentInitId =this.investmentRecService.investmentRecFormData.id;
      this.investmentRecService.investmentCampaignFormData.investmentInitId =this.investmentRecService.investmentRecFormData.id;
      this.investmentRecService.investmentBcdsFormData.investmentInitId =this.investmentRecService.investmentRecFormData.id;
      this.investmentRecService.investmentSocietyFormData.investmentInitId =this.investmentRecService.investmentRecFormData.id;
    }
  }
  onChangeDoctorInDoc(){
    for(var i=0;i<this.doctors.length;i++){
    if(this.doctors[i].id==parseInt(this.investmentRecService.investmentDoctorFormData.doctorName))
    {
      //this.investmentRecService.investmentDoctorFormData.doctorName=this.doctors[i].doctorName;
      this.investmentRecService.investmentDoctorFormData.doctorId=this.doctors[i].id;
      this.investmentRecService.investmentDoctorFormData.degree=this.doctors[i].degree;
      this.investmentRecService.investmentDoctorFormData.designation=this.doctors[i].designation;
      break;
    }
  }
  }
  onChangeInstitutionInDoc(){
    debugger;
    for(var i=0;i<this.institutions.length;i++){
    if(this.institutions[i].id==this.investmentRecService.investmentDoctorFormData.institutionId)
    {
      this.investmentRecService.investmentDoctorFormData.address=this.institutions[i].address;
      
      break;
    }
  }
  }
  onChangeInstitutionInInst(){
    //debugger;
    for(var i=0;i<this.institutions.length;i++){
    if(this.institutions[i].id==this.investmentRecService.investmentInstitutionFormData.institutionId)
    {
      this.investmentRecService.investmentInstitutionFormData.address=this.institutions[i].address;
      this.investmentRecService.investmentInstitutionFormData.institutionType=this.institutions[i].institutionType;
      
      break;
    }
  }
  }
  onChangeCampaignInCamp(){
    debugger;
    this.investmentRecService.getCampaignDtls(this.investmentRecService.investmentCampaignFormData.campaignMstId).subscribe(response => {
      debugger;
      this.campaignDtls = response as ICampaignDtl[];
    }, error => {
        console.log(error);
    });
  }
  onChangeBcdsInBcds(){
    debugger;
    for(var i=0;i<this.bcds.length;i++){
      if(this.bcds[i].id==this.investmentRecService.investmentBcdsFormData.bcdsId)
      {
        this.investmentRecService.investmentBcdsFormData.bcdsAddress=this.bcds[i].bcdsAddress;
        this.investmentRecService.investmentBcdsFormData.noOfMember=this.bcds[i].noOfMember;
        
        break;
      }
    }
  }
  onChangeSubCampaignInCamp(){
    debugger;
    for(var i=0;i<this.campaignDtls.length;i++){
    if(this.campaignDtls[i].id==this.investmentRecService.investmentCampaignFormData.campaignDtlId)
    {
      this.investmentRecService.investmentCampaignFormData.subCampStartDate=new DatePipe('en-US').transform(this.campaignDtls[i].subCampStartDate, 'dd/MM/yyyy');
      this.investmentRecService.investmentCampaignFormData.subCampEndDate=new DatePipe('en-US').transform(this.campaignDtls[i].subCampEndDate, 'dd/MM/yyyy');
      
      break;
    }
  }
    this.investmentRecService.getCampaignDtlProducts(this.investmentRecService.investmentCampaignFormData.campaignDtlId).subscribe(response => {
      debugger;
      this.campaignDtlProducts = response as ICampaignDtlProduct[];
    }, error => {
        console.log(error);
    });
  
  }
  onChangeSocietyInSociety(){
    debugger;
    for(var i=0;i<this.society.length;i++){
    if(this.society[i].id==this.investmentRecService.investmentSocietyFormData.societyId)
    {
      this.investmentRecService.investmentSocietyFormData.societyAddress=this.society[i].societyAddress;
      this.investmentRecService.investmentSocietyFormData.noOfMember=this.society[i].noOfMember;
      
      break;
    }
  }
    
  
  }
  onChangeMarketGroupInTargetedGroup(){
    debugger;
    
    if(this.investmentTargetedGroups==null || this.investmentTargetedGroups.length==0)
    {
    for (let i = 0; i < this.marketGroupMsts.length; i++) {
      if(this.marketGroupMsts[i].id==this.investmentRecService.investmentTargetedGroupFormData.marketGroupMstId)
      {
        var data = []; 
        for (let j = 0; j < this.marketGroupMsts[i].marketGroupDtls.length; j++) {
         var marketGroupMstId=this.marketGroupMsts[i].marketGroupDtls[j].mstId;
          var marketCode=this.marketGroupMsts[i].marketGroupDtls[j].marketCode;
          var marketName=this.marketGroupMsts[i].marketGroupDtls[j].marketName;
          
          data.push({id:0,investmentInitId:this.investmentRecService.investmentRecFormData.id,marketGroupMst:this.marketGroupMsts[i],marketGroupMstId:marketGroupMstId,marketCode:marketCode,marketName:marketName});
          //this.investmentTargetedGroups.push({id:0,investmentInitId:this.investmentRecService.investmentRecFormData.id,marketGroup:null,marketGroupMstId:this.marketGroupMsts[i].marketGroupDtls[j].mstId,marketCode:this.marketGroupMsts[i].marketGroupDtls[j].marketCode,marketName:this.marketGroupMsts[i].marketGroupDtls[j].marketName});
        }
        this.investmentTargetedGroups=data;
      break 
      }
    }
  }
  else{
    this.toastr.warning('Already Market Group Exist', 'Investment ', {
      positionClass: 'toast-top-right' 
   });
  }
  }
  changeDateInDetail(){
    debugger;
    
    //this.printingDate=this.getDigitBanglaFromEnglish(this.datePipe.transform(value, "dd/MM/yyyy"));
    if(this.investmentRecService.investmentDetailFormData.fromDate==null || this.investmentRecService.investmentDetailFormData.fromDate==undefined )
    {
      
     return false;
    }
    if(this.investmentRecService.investmentDetailFormData.toDate==null || this.investmentRecService.investmentDetailFormData.toDate==undefined )
    {
      
     return false;
    }
    let dateFrom = this.investmentRecService.investmentDetailFormData.fromDate;
    let dateTo = this.investmentRecService.investmentDetailFormData.toDate;
    //let dateFrom = new Date();
    //let dateTo = new Date();

    this.investmentRecService.investmentDetailFormData.totalMonth = String(dateTo.getMonth() - dateFrom.getMonth() + (12 * (dateTo.getFullYear() - dateFrom.getFullYear())));

  }
  getDonation(){
    this.investmentRecService.getDonations().subscribe(response => {
      this.donations = response as IDonation[];
    }, error => {
        console.log(error);
    });
  }
  getSubCampaign(){
    this.investmentRecService.getSubCampaigns().subscribe(response => {
      //debugger;
      this.subCampaigns = response as ISubCampaign[];
    }, error => {
        console.log(error);
    });
  }
  getDoctor(){
    this.investmentRecService.getDoctors().subscribe(response => {
      //debugger;
      this.doctors = response as IDoctor[];
    }, error => {
        console.log(error);
    });
  }
  getCampaignMst(){
    this.investmentRecService.getCampaignMsts().subscribe(response => {
      //debugger;
      this.campaignMsts = response as ICampaignMst[];
    }, error => {
        console.log(error);
    });
  }
  getSociety(){
    this.investmentRecService.getSociety().subscribe(response => {
      //debugger;
      this.society = response as ISocietyInfo[];
    }, error => {
        console.log(error);
    });
  }
  getBcds(){
    this.investmentRecService.getBcds().subscribe(response => {
      //debugger;
      this.bcds = response as IBcdsInfo[];
    }, error => {
        console.log(error);
    });
  }
  getInstitution(){
    this.investmentRecService.getInstitutions().subscribe(response => {
      this.institutions = response as IInstitution[];
    }, error => {
        console.log(error);
    });
  }
  getMarket(){
    this.investmentRecService.getMarkets().subscribe(response => {
     this.markets = response as IMarket[];
    }, error => {
        console.log(error);
   });
 }
 getProduct(){
  this.investmentRecService.getProduct().subscribe(response => {
    //debugger;
    this.products = response as IProduct[];
  }, error => {
      console.log(error);
  });
}
getMarketGroupMsts(){
  this.investmentRecService.getMarketGroupMsts().subscribe(response => {
    debugger;
   this.marketGroupMsts = response as IMarketGroupMst[];
  }, error => {
      console.log(error);
 });
}
  onSubmit(form: NgForm) {
    if (this.investmentRecService.investmentRecFormData.id == 0)
      this.insertInvestmentRec();
    else
      this.updateInvestmentRec();
  }
  insertInvestmentRec() {
    this.investmentRecService.insertInvestmentRec().subscribe(
      res => {
        //debugger;
       this.investmentRecService.investmentRecFormData=res as IInvestmentRec;
       this.investmentRecService.investmentDoctorFormData.investmentInitId=this.investmentRecService.investmentRecFormData.id;
       this.investmentRecService.investmentInstitutionFormData.investmentInitId=this.investmentRecService.investmentRecFormData.id;
        this.isValid=true;
        this.toastr.success('Submitted successfully', 'Investment ')
      },
      err => { console.log(err); }
    );
  }
  updateInvestmentRec() {
    this.investmentRecService.updateInvestmentRec().subscribe(
      res => {
        debugger;
        this.isValid=true;
        this.investmentRecService.investmentDoctorFormData.investmentInitId=this.investmentRecService.investmentRecFormData.id;
       this.investmentRecService.investmentInstitutionFormData.investmentInitId=this.investmentRecService.investmentRecFormData.id;
        this.toastr.info('Updated successfully', 'Investment ')
      },
      err => { console.log(err); }
    );
  }
  insertInvestmentDetails() {
    if(this.investmentRecService.investmentRecFormData.id==null || this.investmentRecService.investmentRecFormData.id==undefined || this.investmentRecService.investmentRecFormData.id==0)
    {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    if(this.investmentRecService.investmentDetailFormData.proposedAmount==null || this.investmentRecService.investmentDetailFormData.proposedAmount==undefined || this.investmentRecService.investmentDetailFormData.proposedAmount=="")
    {
      this.toastr.warning('Enter Proposed Amount First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    if(this.investmentRecService.investmentDetailFormData.purpose==null || this.investmentRecService.investmentDetailFormData.purpose==undefined || this.investmentRecService.investmentDetailFormData.purpose=="")
    {
      this.toastr.warning('Enter Purpose First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    if(this.investmentRecService.investmentDetailFormData.fromDate==null || this.investmentRecService.investmentDetailFormData.fromDate==undefined )
    {
      this.toastr.warning('Select From Date  First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    if(this.investmentRecService.investmentDetailFormData.toDate==null || this.investmentRecService.investmentDetailFormData.toDate==undefined )
    {
      this.toastr.warning('Select To Date  First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    if(this.investmentRecService.investmentDetailFormData.commitmentAllSBU==null || this.investmentRecService.investmentDetailFormData.commitmentAllSBU==undefined || this.investmentRecService.investmentDetailFormData.commitmentAllSBU=="")
    {
      this.toastr.warning('Enter Commitment All SBU First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    if(this.investmentRecService.investmentDetailFormData.commitmentOwnSBU==null || this.investmentRecService.investmentDetailFormData.commitmentOwnSBU==undefined || this.investmentRecService.investmentDetailFormData.commitmentOwnSBU=="")
    {
      this.toastr.warning('Enter Commitment Own SBU First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    if(this.investmentRecService.investmentDetailFormData.paymentMethod==null || this.investmentRecService.investmentDetailFormData.paymentMethod==undefined || this.investmentRecService.investmentDetailFormData.paymentMethod=="")
    {
      this.toastr.warning('Select Payment Method First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    
    
    this.investmentRecService.investmentDetailFormData.investmentInitId =this.investmentRecService.investmentRecFormData.id;
     
    if(this.investmentRecService.investmentDetailFormData.id==null || this.investmentRecService.investmentDetailFormData.id==undefined || this.investmentRecService.investmentDetailFormData.id==0)
    {
      this.investmentRecService.insertInvestmentDetail().subscribe(
        res => {
         var data=res as IInvestmentDetail;
         this.investmentRecService.investmentDetailFormData=data;
         //this.investmentRecService.investmentDoctorFormData.doctorName=String(data.doctorId);
         this.investmentRecService.investmentDetailFormData.fromDate=new Date(data.fromDate);
        this.investmentRecService.investmentDetailFormData.toDate=new Date(data.toDate);
         this.isDonationValid=true;
         this.toastr.success('Save successfully', 'Investment ');
        },
        err => { console.log(err); }
      );
    }
    else{
      this.investmentRecService.updateInvestmentDetail().subscribe(
        res => {
         var data=res as IInvestmentDetail;
         this.investmentRecService.investmentDetailFormData=data;
         //this.investmentRecService.investmentDoctorFormData.doctorName=String(data.doctorId);
         this.investmentRecService.investmentDetailFormData.fromDate=new Date(data.fromDate);
        this.investmentRecService.investmentDetailFormData.toDate=new Date(data.toDate);
         this.isDonationValid=true;
         this.toastr.success('Save successfully', 'Investment ');
        },
        err => { console.log(err); }
      );
    }
    
  }
  insertInvestmentDoctor() {
    if(this.investmentRecService.investmentRecFormData.id==null || this.investmentRecService.investmentRecFormData.id==undefined || this.investmentRecService.investmentRecFormData.id==0)
    {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    // if(this.investmentRecService.investmentRecFormData.donationTo!=="Doctor")
    // {
    //   this.updateInvestmentRec();
    // }
    if(this.investmentRecService.investmentDoctorFormData.doctorId==null || this.investmentRecService.investmentDoctorFormData.doctorId==undefined || this.investmentRecService.investmentDoctorFormData.doctorId==0)
    {
      this.toastr.warning('Select Doctor First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    if(this.investmentRecService.investmentDoctorFormData.institutionId==null || this.investmentRecService.investmentDoctorFormData.institutionId==undefined || this.investmentRecService.investmentDoctorFormData.institutionId==0)
    {
      this.toastr.warning('Select Institute First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    this.investmentRecService.investmentDoctorFormData.investmentInitId =this.investmentRecService.investmentRecFormData.id;
     
    this.investmentRecService.insertInvestmentDoctor().subscribe(
      res => {
       var data=res as IInvestmentDoctor;
       this.investmentRecService.investmentDoctorFormData=data;
       this.investmentRecService.investmentDoctorFormData.doctorName=String(data.doctorId);
       this.onChangeDoctorInDoc();
       this.onChangeInstitutionInDoc();
       this.updateInvestmentRec();
       this.isDonationValid=true;
       this.toastr.success('Save successfully', 'Investment ');
      },
      err => { console.log(err); }
    );
  }
  insertInvestmentInstitution() {
    debugger;
    if(this.investmentRecService.investmentRecFormData.id==null || this.investmentRecService.investmentRecFormData.id==undefined || this.investmentRecService.investmentRecFormData.id==0)
    {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    // if(this.investmentRecService.investmentRecFormData.donationTo!=="Institution")
    // {
      
    // }
    if(this.investmentRecService.investmentInstitutionFormData.resposnsibleDoctorId==null || this.investmentRecService.investmentInstitutionFormData.resposnsibleDoctorId==undefined || this.investmentRecService.investmentInstitutionFormData.resposnsibleDoctorId==0)
    {
      this.toastr.warning('Select Institution First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    if(this.investmentRecService.investmentInstitutionFormData.institutionId==null || this.investmentRecService.investmentInstitutionFormData.institutionId==undefined || this.investmentRecService.investmentInstitutionFormData.institutionId==0)
    {
      this.toastr.warning('Select Institute First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
      this.investmentRecService.investmentInstitutionFormData.investmentInitId =this.investmentRecService.investmentRecFormData.id;
      
    this.investmentRecService.insertInvestmentInstitution().subscribe(
      res => {
        debugger;
       this.investmentRecService.investmentInstitutionFormData=res as IInvestmentInstitution;
       this.onChangeInstitutionInInst();
       this.updateInvestmentRec();
       this.isDonationValid=true;
        this.toastr.success('Save successfully', 'Investment ');
      },
      err => { console.log(err); }
    );
  }
  insertInvestmentCampaign() {
    debugger;
    if(this.investmentRecService.investmentRecFormData.id==null || this.investmentRecService.investmentRecFormData.id==undefined || this.investmentRecService.investmentRecFormData.id==0)
    {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    // if(this.investmentRecService.investmentRecFormData.donationTo!=="Institution")
    // {
      
    // }
    if(this.investmentRecService.investmentCampaignFormData.campaignMstId==null || this.investmentRecService.investmentCampaignFormData.campaignMstId==undefined || this.investmentRecService.investmentCampaignFormData.campaignMstId==0)
    {
      this.toastr.warning('Select Campaign First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    if(this.investmentRecService.investmentCampaignFormData.campaignDtlId==null || this.investmentRecService.investmentCampaignFormData.campaignDtlId==undefined || this.investmentRecService.investmentCampaignFormData.campaignDtlId==0)
    {
      this.toastr.warning('Select Sub-Campaign First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    if(this.investmentRecService.investmentCampaignFormData.doctorId==null || this.investmentRecService.investmentCampaignFormData.doctorId==undefined || this.investmentRecService.investmentCampaignFormData.doctorId==0)
    {
      this.toastr.warning('Select Doctor First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    if(this.investmentRecService.investmentCampaignFormData.institutionId==null || this.investmentRecService.investmentCampaignFormData.institutionId==undefined || this.investmentRecService.investmentCampaignFormData.institutionId==0)
    {
      this.toastr.warning('Select Institute First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
      this.investmentRecService.investmentCampaignFormData.investmentInitId =this.investmentRecService.investmentRecFormData.id;
      
    var tempMstId=this.investmentRecService.investmentCampaignFormData.campaignMstId;
    this.investmentRecService.insertInvestmentCampaign().subscribe(
      res => {
        debugger;
       this.investmentRecService.investmentCampaignFormData=res as IInvestmentCampaign;
       this.investmentRecService.investmentCampaignFormData.campaignMstId=tempMstId;
       this.onChangeCampaignInCamp();
       this.onChangeSubCampaignInCamp();
       this.updateInvestmentRec();
       
       this.isDonationValid=true;
        this.toastr.success('Save successfully', 'Investment ');
      },
      err => { console.log(err); }
    );
  }
  insertInvestmentSociety() {
    debugger;
    if(this.investmentRecService.investmentRecFormData.id==null || this.investmentRecService.investmentRecFormData.id==undefined || this.investmentRecService.investmentRecFormData.id==0)
    {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    // if(this.investmentRecService.investmentRecFormData.donationTo!=="Institution")
    // {
      
    // }
    if(this.investmentRecService.investmentSocietyFormData.societyId==null || this.investmentRecService.investmentSocietyFormData.societyId==undefined || this.investmentRecService.investmentSocietyFormData.societyId==0)
    {
      this.toastr.warning('Select Society First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    
    
    
       this.investmentRecService.investmentSocietyFormData.investmentInitId =this.investmentRecService.investmentRecFormData.id;
   
    this.investmentRecService.insertInvestmentSociety().subscribe(
      res => {
        debugger;
       this.investmentRecService.investmentSocietyFormData=res as IInvestmentSociety;
       this.onChangeSocietyInSociety();
       this.updateInvestmentRec();
       
       this.isDonationValid=true;
        this.toastr.success('Save successfully', 'Investment ');
      },
      err => { console.log(err); }
    );
  }
  insertInvestmentBcds() {
    debugger;
    if(this.investmentRecService.investmentRecFormData.id==null || this.investmentRecService.investmentRecFormData.id==undefined || this.investmentRecService.investmentRecFormData.id==0)
    {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    // if(this.investmentRecService.investmentRecFormData.donationTo!=="Institution")
    // {
      
    // }
    if(this.investmentRecService.investmentBcdsFormData.bcdsId==null || this.investmentRecService.investmentBcdsFormData.bcdsId==undefined || this.investmentRecService.investmentBcdsFormData.bcdsId==0)
    {
      this.toastr.warning('Select Bcds First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    
    
    
       this.investmentRecService.investmentBcdsFormData.investmentInitId =this.investmentRecService.investmentRecFormData.id;
   
    this.investmentRecService.insertInvestmentBcds().subscribe(
      res => {
        debugger;
       this.investmentRecService.investmentBcdsFormData=res as IInvestmentBcds;
       this.onChangeBcdsInBcds();
       this.updateInvestmentRec();
       
       this.isDonationValid=true;
        this.toastr.success('Save successfully', 'Investment ');
      },
      err => { console.log(err); }
    );
  }
  insertInvestmentTargetedProd() {
    debugger;
    if(this.investmentRecService.investmentRecFormData.id==null || this.investmentRecService.investmentRecFormData.id==undefined || this.investmentRecService.investmentRecFormData.id==0)
    {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
   
    if(this.investmentRecService.investmentTargetedProdFormData.productId==null || this.investmentRecService.investmentTargetedProdFormData.productId==undefined || this.investmentRecService.investmentTargetedProdFormData.productId==0)
    {
      this.toastr.warning('Select Product First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    if(this.investmentTargetedProds!==undefined){
      for (let i = 0; i < this.investmentTargetedProds.length; i++) {
        if(this.investmentTargetedProds[i].productInfo.id===this.investmentRecService.investmentTargetedProdFormData.productId)
        {
        alert("product already exist !");
        return false;
        }
      }
    }
    this.investmentRecService.investmentTargetedProdFormData.investmentInitId =this.investmentRecService.investmentRecFormData.id;
    if(this.investmentRecService.investmentTargetedProdFormData.id==null || this.investmentRecService.investmentTargetedProdFormData.id==undefined || this.investmentRecService.investmentTargetedProdFormData.id==0)
    {
    this.investmentRecService.insertInvestmentTargetedProd().subscribe(
      res => {
        debugger;
       this.investmentRecService.investmentTargetedProdFormData=new InvestmentTargetedProd();
       
       this.getInvestmentTargetedProd();
       
       this.isDonationValid=true;
        this.toastr.success('Save successfully', 'Investment ');
      },
      err => { console.log(err); }
    );
    }
    else{
      this.investmentRecService.updateInvestmentTargetedProd().subscribe(
        res => {
          debugger;
         this.investmentRecService.investmentTargetedProdFormData=new InvestmentTargetedProd();
         
         this.getInvestmentTargetedProd();
         
         this.isDonationValid=true;
          this.toastr.success('Update successfully', 'Investment ');
        },
        err => { console.log(err); }
      );
    }
  }
  insertInvestmentTargetedGroup() {
    debugger;
    if(this.investmentRecService.investmentRecFormData.id==null || this.investmentRecService.investmentRecFormData.id==undefined || this.investmentRecService.investmentRecFormData.id==0)
    {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
   
    if(this.investmentRecService.investmentTargetedGroupFormData.marketGroupMstId==null || this.investmentRecService.investmentTargetedGroupFormData.marketGroupMstId==undefined || this.investmentRecService.investmentTargetedGroupFormData.marketGroupMstId==0)
    {
      this.toastr.warning('Select Market Group First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    if(this.investmentTargetedGroups!=null && this.investmentTargetedGroups.length>0)
    {
    this.investmentRecService.investmentTargetedGroupFormData.investmentInitId =this.investmentRecService.investmentRecFormData.id;
    
    this.investmentRecService.insertInvestmentTargetedGroup(this.investmentTargetedGroups).subscribe(
      res => {
        debugger;
       this.investmentRecService.investmentTargetedProdFormData=new InvestmentTargetedProd();
       
       this.getInvestmentTargetedGroup();
       
       this.isDonationValid=true;
        this.toastr.success(res);
      },
      err => { console.log(err); }
    );
  }
  else{
    this.toastr.warning('Select Market Group First', 'Investment ', {
      positionClass: 'toast-top-right' 
   });
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
    this.investmentRecService.investmentRecFormData=new InvestmentRec();
    this.isValid=false;
  }
  removeInvestmentDoctor() {
    var c = confirm("Are you sure you want to delete that?"); 
    if (c == true) {  
    this.investmentRecService.removeInvestmentDoctor().subscribe(
      res => {
        this.toastr.success(res);
        this.isDonationValid=false;
        this.investmentRecService.investmentDoctorFormData=new InvestmentDoctor();
      },
      err => { debugger;console.log(err); }
    );
  }
  }
  removeInvestmentInstitution() {
    var c = confirm("Are you sure you want to delete that?"); 
    if (c == true) {  
    this.investmentRecService.removeInvestmentInstitution().subscribe(
      res => {
        debugger;
        this.toastr.success(res);
        this.isDonationValid=false;
        this.investmentRecService.investmentInstitutionFormData=new InvestmentInstitution();
      },
      err => { console.log(err); }
    );
  }
}
  removeInvestmentCampaign() {
    var c = confirm("Are you sure you want to delete that?"); 
    if (c == true) {  
    this.investmentRecService.removeInvestmentCampaign().subscribe(
      res => {
        debugger;
        this.toastr.success(res);
        this.isDonationValid=false;
        this.investmentRecService.investmentCampaignFormData=new InvestmentCampaign();
      },
      err => { console.log(err); }
    );
  }
}
  removeInvestmentSociety() {
    var c = confirm("Are you sure you want to delete that?"); 
    if (c == true) {  
    this.investmentRecService.removeInvestmentSociety().subscribe(
      res => {
        debugger;
        this.toastr.success(res);
        this.isDonationValid=false;
        this.investmentRecService.investmentSocietyFormData=new InvestmentSociety();
      },
      err => { console.log(err); }
    );
  }
}
  removeInvestmentBcds() {
    var c = confirm("Are you sure you want to delete that?"); 
    if (c == true) {  
    this.investmentRecService.removeInvestmentBcds().subscribe(
      res => {
        debugger;
        this.toastr.success(res);
        this.isDonationValid=false;
        this.investmentRecService.investmentBcdsFormData=new InvestmentBcds();
      },
      err => { console.log(err); }
    );
  }
}
removeInvestmentTargetedProd(selectedRecord: IInvestmentTargetedProd) {
  this.investmentRecService.investmentTargetedProdFormData = Object.assign({}, selectedRecord);
  var c = confirm("Are you sure you want to delete that?"); 
    if (c == true) {  
    this.investmentRecService.removeInvestmentTargetedProd().subscribe(
      res => {
        debugger;
        this.toastr.success(res);
        //this.isDonationValid=false;
        this.investmentRecService.investmentTargetedProdFormData=new InvestmentTargetedProd();
        this.getInvestmentTargetedProd();
      },
      err => { console.log(err); }
    );
  }
}
removeInvestmentTargetedGroup() {
  //this.investmentRecService.investmentTargetedProdFormData = Object.assign({}, selectedRecord);
  if(this.investmentTargetedGroups!=null && this.investmentTargetedGroups.length>0)
    {
  var c = confirm("Are you sure you want to delete that?"); 
    if (c == true) {  
    this.investmentRecService.removeInvestmentTargetedGroup(this.investmentTargetedGroups).subscribe(
      res => {
        debugger;
        this.toastr.success(res);
        //this.isDonationValid=false;
        this.investmentRecService.investmentTargetedGroupFormData=new InvestmentTargetedGroup();
        this.getInvestmentTargetedGroup();
      },
      err => { console.log(err); }
    );
  }
}
  else{
    this.toastr.warning('No Market Group Found', 'Investment ', {
      positionClass: 'toast-top-right' 
   });
  }
}
}

