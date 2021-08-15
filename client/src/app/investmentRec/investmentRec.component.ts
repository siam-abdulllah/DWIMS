
import { InvestmentRec, IInvestmentRec,InvestmentInit,IInvestmentInit,
  InvestmentTargetedProd,IInvestmentTargetedProd,InvestmentTargetedGroup,IInvestmentTargetedGroup, IInvestmentRecComment } from '../shared/models/investmentRec';
import { InvestmentDoctor, IInvestmentDoctor,InvestmentInstitution,IInvestmentInstitution,InvestmentCampaign,IInvestmentCampaign } from '../shared/models/investmentRec';
import { InvestmentBcds, IInvestmentBcds,InvestmentSociety,IInvestmentSociety } from '../shared/models/investmentRec';
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
  investmentDetails: IInvestmentRec[];
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
    selectInvestmentRec(selectedRecord: IInvestmentInit) {
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
      
      this.getInvestmentDoctor();
    }
      else if(this.investmentRecService.investmentRecFormData.donationTo=="Institution")
    {
      
      this.getInvestmentInstitution();
    }
      else if(this.investmentRecService.investmentRecFormData.donationTo=="Campaign")
    {
      this.getInvestmentCampaign();
    }
      else if(this.investmentRecService.investmentRecFormData.donationTo=="Bcds")
    {
      this.getInvestmentBcds();
    }
      else if(this.investmentRecService.investmentRecFormData.donationTo=="Society")
    {
      this.getInvestmentSociety();
    }
      this.getInvestmentDetails();
      this.getInvestmentTargetedProd();
      this.getInvestmentTargetedGroup();
      this.isValid=true;
      this.InvestmentRecSearchModalRef.hide()
     }
     getInvestmentInit(){
      this.investmentRecService.getInvestmentInit().subscribe(response => {
        //debugger;
       this.investmentRecs = response.data;
       this.openInvestmentRecSearchModal(this.investmentRecSearchModal);
      }, error => {
          console.log(error);
     });
   }
   getInvestmentRecommended(){
      this.investmentRecService.getInvestmentRecommended().subscribe(response => {
        //debugger;
       this.investmentRecs = response.data;
       this.openInvestmentRecSearchModal(this.investmentRecSearchModal);
      }, error => {
          console.log(error);
     });
   }
   getInvestmentDetails(){
      this.investmentRecService.getInvestmentDetails(this.investmentRecService.investmentRecFormData.id ).subscribe(response => {
        debugger;
        var data=response[0] as IInvestmentRec;
        if(data!==undefined)
        {
        this.investmentRecService.investmentDetailFormData=data;
        this.investmentRecService.investmentDetailFormData.id=0;
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
    this.investmentRecService.getInvestmentCampaigns(this.investmentRecService.investmentRecFormData.id).subscribe(response => {
      debugger;
      
      var data=response[0] as IInvestmentCampaign;
      if(data!==undefined)
      {
      this.investmentRecService.investmentCampaignFormData=data;

      this.investmentRecService.investmentCampaignFormData.campaignMstId=data.campaignDtl.mstId;
      this.investmentRecService.investmentCampaignFormData.subCampStartDate=new DatePipe('en-US').transform(data.campaignDtl.subCampStartDate, 'dd/MM/yyyy');
      this.investmentRecService.investmentCampaignFormData.subCampEndDate=new DatePipe('en-US').transform(data.campaignDtl.subCampEndDate, 'dd/MM/yyyy');
     
      
    }
    else{
      this.toastr.warning('No Data Found', 'Investment ');
    }
      
    }, error => {
        console.log(error);
    });
  }
   getInvestmentBcds(){
    this.investmentRecService.getInvestmentBcds(this.investmentRecService.investmentRecFormData.id).subscribe(response => {
      debugger;
      
      var data=response[0] as IInvestmentBcds;
      if(data!==undefined)
      {
      this.investmentRecService.investmentBcdsFormData=data;
      this.investmentRecService.investmentBcdsFormData.bcdsName=data.bcds.bcdsName;
      this.investmentRecService.investmentBcdsFormData.bcdsAddress=data.bcds.bcdsAddress;
      this.investmentRecService.investmentBcdsFormData.noOfMember=data.bcds.noOfMember;
      //this.onChangeBcdsInBcds();
    }
    else{
      this.toastr.warning('No Data Found', 'Investment ');
    }
      
    }, error => {
        console.log(error);
    });
  }
   getInvestmentSociety(){
    this.investmentRecService.getInvestmentSociety(this.investmentRecService.investmentRecFormData.id).subscribe(response => {
      debugger;
      
      var data=response[0] as IInvestmentSociety;
      if(data!==undefined)
      {
      this.investmentRecService.investmentSocietyFormData=data;
      this.investmentRecService.investmentSocietyFormData.societyName=data.society.societyName;
      this.investmentRecService.investmentSocietyFormData.noOfMember=data.society.noOfMember;
      this.investmentRecService.investmentSocietyFormData.societyAddress=data.society.societyAddress;
      
      //this.onChangeSocietyInSociety();
    }
    else{
      this.toastr.warning('No Data Found', 'Investment ');
    }
      
    }, error => {
        console.log(error);
    });
  }
   getInvestmentInstitution(){
    this.investmentRecService.getInvestmentInstitutions(this.investmentRecService.investmentRecFormData.id).subscribe(response => {
      debugger;
      
      var data=response[0] as IInvestmentInstitution;
      if(data!==undefined)
      {
      this.investmentRecService.investmentInstitutionFormData=data;
      this.investmentRecService.investmentInstitutionFormData.resposnsibleDoctorName=data.doctor.doctorName;
      this.investmentRecService.investmentInstitutionFormData.institutionName=data.institution.institutionName;
      this.investmentRecService.investmentInstitutionFormData.address=data.institution.address;
      this.investmentRecService.investmentInstitutionFormData.institutionType=data.institution.institutionType;
      //this.onChangeInstitutionInInst();
    }
    else{
      this.toastr.warning('No Data Found', 'Investment ');
    }
      
    }, error => {
        console.log(error);
    });
  }
   getInvestmentDoctor(){
    this.investmentRecService.getInvestmentDoctors(this.investmentRecService.investmentRecFormData.id).subscribe(response => {
      debugger;
      var data=response[0] as IInvestmentDoctor;
      if( data!==undefined)
      {
      this.investmentRecService.investmentDoctorFormData=data;
      this.investmentRecService.investmentDoctorFormData.doctorName=data.doctor.doctorName;
      this.investmentRecService.investmentDoctorFormData.degree=data.doctor.degree;
      this.investmentRecService.investmentDoctorFormData.designation=data.doctor.designation;
      this.investmentRecService.investmentDoctorFormData.institutionName=data.institution.institutionName;
      this.investmentRecService.investmentDoctorFormData.address=data.institution.address;
      //this.onChangeDoctorInDoc();
      //this.onChangeInstitutionInDoc();
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
    //this.getDonation();
    this.getProduct();
    //this.getMarketGroupMsts();
    this.bsConfig = Object.assign({}, { containerClass: 'theme-green' }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();

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
  onChangeSubCampaignInCamp(){
    debugger;
   
      //this.investmentRecService.investmentCampaignFormData.subCampStartDate=new DatePipe('en-US').transform(this.campaignDtls[i].subCampStartDate, 'dd/MM/yyyy');
     // this.investmentRecService.investmentCampaignFormData.subCampEndDate=new DatePipe('en-US').transform(this.campaignDtls[i].subCampEndDate, 'dd/MM/yyyy');
      
      
    this.investmentRecService.getCampaignDtlProducts(this.investmentRecService.investmentCampaignFormData.campaignDtlId).subscribe(response => {
      debugger;
      this.campaignDtlProducts = response as ICampaignDtlProduct[];
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
  
 getProduct(){
  this.investmentRecService.getProduct().subscribe(response => {
    //debugger;
    this.products = response as IProduct[];
  }, error => {
      console.log(error);
  });
}

  onSubmit(form: NgForm) {
    if(this.investmentRecService.investmentRecCommentFormData.id==null || this.investmentRecService.investmentRecCommentFormData.id==undefined || this.investmentRecService.investmentRecCommentFormData.id==0)
   
      this.insertInvestmentRec();
    else
      this.updateInvestmentRec();
  }
  insertInvestmentRec() {
    this.investmentRecService.insertInvestmentRec().subscribe(
      res => {
        //debugger;
        this.investmentRecService.investmentRecCommentFormData=res as IInvestmentRecComment;
       //this.investmentRecService.investmentDoctorFormData.investmentInitId=this.investmentRecService.investmentRecFormData.id;
       //this.investmentRecService.investmentInstitutionFormData.investmentInitId=this.investmentRecService.investmentRecFormData.id;
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
        this.investmentRecService.investmentRecCommentFormData=res as IInvestmentRecComment;
        //this.investmentRecService.investmentDoctorFormData.investmentInitId=this.investmentRecService.investmentRecFormData.id;
      // this.investmentRecService.investmentInstitutionFormData.investmentInitId=this.investmentRecService.investmentRecFormData.id;
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
         var data=res as IInvestmentRec;
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
         var data=res as IInvestmentRec;
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
 
 
  
  insertInvestmentTargetedProd() {
    debugger;
    if(this.investmentRecService.investmentRecFormData.id==null || this.investmentRecService.investmentRecFormData.id==undefined || this.investmentRecService.investmentRecFormData.id==0)
    {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    if(this.investmentRecService.investmentDetailFormData.id==null || this.investmentRecService.investmentDetailFormData.id==undefined || this.investmentRecService.investmentDetailFormData.id==0)
    {
      this.toastr.warning('Insert Investment Detail First', 'Investment ', {
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
        if(this.investmentTargetedProds[i].productInfo.id==this.investmentRecService.investmentTargetedProdFormData.productId)
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
  addInvestmentTargetedProd() {
    debugger;
    if(this.investmentRecService.investmentTargetedProdFormData.productId==null || this.investmentRecService.investmentTargetedProdFormData.productId==undefined || this.investmentRecService.investmentTargetedProdFormData.productId==0)
    {
      this.toastr.warning('Select Product First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    if(this.investmentTargetedProds!==undefined){
      for (let i = 0; i < this.investmentTargetedProds.length; i++) {
        if(this.investmentTargetedProds[i].productInfo.id==this.investmentRecService.investmentTargetedProdFormData.productId)
        {
        alert("product already exist !");
        return false;
        }

      }
      for (let i = 0; i < this.products.length; i++) {
        if(this.products[i].id==this.investmentRecService.investmentTargetedProdFormData.productId)
        {
          let data=new InvestmentTargetedProd();
          data.id=0;
          data.investmentInitId=this.investmentRecService.investmentRecFormData.id;
          data.productId=this.investmentRecService.investmentTargetedProdFormData.productId;
          data.productInfo=this.products[i];
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
    this.investmentRecService.investmentRecFormData=new InvestmentInit();
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
  if (this.investmentTargetedProds.find(x => x.productId == selectedRecord.productId)) {
    this.investmentTargetedProds.splice(this.investmentTargetedProds.findIndex(x => x.productId == selectedRecord.productId), 1);
 }
  if(this.investmentRecService.investmentRecCommentFormData.id==null || this.investmentRecService.investmentRecCommentFormData.id==undefined || this.investmentRecService.investmentRecCommentFormData.id==0)
    {
      return false;
    }
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

