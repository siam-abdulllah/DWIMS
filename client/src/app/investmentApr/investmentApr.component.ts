
import { InvestmentApr, IInvestmentApr,InvestmentInit,IInvestmentInit,
  InvestmentTargetedProd,IInvestmentTargetedProd,InvestmentTargetedGroup,IInvestmentTargetedGroup, IInvestmentAprComment } from '../shared/models/investmentApr';
import { InvestmentDoctor, IInvestmentDoctor,InvestmentInstitution,IInvestmentInstitution,InvestmentCampaign,IInvestmentCampaign } from '../shared/models/investmentApr';
import { InvestmentBcds, IInvestmentBcds,InvestmentSociety,IInvestmentSociety } from '../shared/models/investmentApr';
import { SubCampaign, ISubCampaign } from '../shared/models/subCampaign';
import { Donation, IDonation } from '../shared/models/donation';
import { Doctor, IDoctor } from '../shared/models/docotor';
import { Institution, IInstitution } from '../shared/models/institution';
import { GenericParams } from '../shared/models/genericParams';
import { Component, ElementRef, OnInit, ViewChild , TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { InvestmentAprService } from '../_services/investmentApr.service';
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
import { AccountService } from '../account/account.service';
import { IInvestmentDetailOld } from '../shared/models/investment';
@Component({
  selector: 'app-investmentApr',
  templateUrl: './investmentApr.component.html',
  styles: [
  ],
  providers: [DatePipe]
})
export class InvestmentAprComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  @ViewChild('investmentInitSearchModal', { static: false }) investmentInitSearchModal: TemplateRef<any>;
  @ViewChild('investmentAprSearchModal', { static: false }) investmentAprSearchModal: TemplateRef<any>;
  InvestmentInitSearchModalRef: BsModalRef;
  InvestmentAprSearchModalRef: BsModalRef;
  investmentDetailsOld: IInvestmentDetailOld[];
  // genParams: GenericParams;
  // campaigns: ICampaign[]; 
  investmentAprs: IInvestmentApr[];
  investmentTargetedProds: IInvestmentTargetedProd[];
  investmentTargetedGroups: IInvestmentTargetedGroup[];
  investmentDetails: IInvestmentApr[];
  investmentDoctors: IInvestmentDoctor[];
  isValid: boolean=false; 
  isInvOther: boolean = false;
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
  empId:string;
  sbu:string;
  config = {
    keyboard: false,
    class: 'modal-lg',
    ignoreBackdropClick: true
  };
  constructor(private accountService: AccountService,public investmentAprService: InvestmentAprService, private router: Router,
    private toastr: ToastrService,private modalService: BsModalService,private datePipe: DatePipe) { }

    openInvestmentInitSearchModal(template: TemplateRef<any>) {
      this.InvestmentInitSearchModalRef = this.modalService.show(template, this.config);
    }
    openInvestmentAprSearchModal(template: TemplateRef<any>) {
      this.InvestmentAprSearchModalRef = this.modalService.show(template, this.config);
    }
    selectInvestmentInit(selectedAprord: IInvestmentInit) {
      //
      this.investmentAprService.investmentAprFormData = Object.assign({}, selectedAprord);
      this.investmentAprService.investmentDetailFormData.investmentInitId =selectedAprord.id;
      this.investmentAprService.investmentAprCommentFormData.investmentInitId =selectedAprord.id;
      this.isDonationValid=true;
      if(this.investmentAprService.investmentAprFormData.donationTo=="Doctor")
      {
      
      this.getInvestmentDoctor();
     }
      else if(this.investmentAprService.investmentAprFormData.donationTo=="Institution")
     {
      
      this.getInvestmentInstitution();
      }
      else if(this.investmentAprService.investmentAprFormData.donationTo=="Campaign")
      {
      //this.getCampaignMst();
      this.getInvestmentCampaign();
      }
      else if(this.investmentAprService.investmentAprFormData.donationTo=="Bcds")
      {
      this.getInvestmentBcds();
      }
      else if(this.investmentAprService.investmentAprFormData.donationTo=="Society")
      {
      this.getInvestmentSociety();
      }
      this.getInvestmentDetails();
      this.getInvestmentTargetedProd();
      this.getInvestmentTargetedGroup();
      if (this.sbu == this.investmentAprService.investmentAprFormData.sbu) {
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
    selectInvestmentApr(selectedAprord: IInvestmentInit) {
      //
      this.investmentAprService.investmentAprFormData = Object.assign({}, selectedAprord);
      this.investmentAprService.investmentDetailFormData.investmentInitId =selectedAprord.id;
      this.investmentAprService.investmentAprCommentFormData.investmentInitId =selectedAprord.id;
      this.isDonationValid=true;
      if(this.investmentAprService.investmentAprFormData.donationTo=="Doctor")
      {
      
      this.getInvestmentDoctor();
     }
      else if(this.investmentAprService.investmentAprFormData.donationTo=="Institution")
     {
      
      this.getInvestmentInstitution();
      }
      else if(this.investmentAprService.investmentAprFormData.donationTo=="Campaign")
      {
      //this.getCampaignMst();
      this.getInvestmentCampaign();
      }
      else if(this.investmentAprService.investmentAprFormData.donationTo=="Bcds")
      {
      this.getInvestmentBcds();
      }
      else if(this.investmentAprService.investmentAprFormData.donationTo=="Society")
      {
      this.getInvestmentSociety();
      }
      this.getInvestmentAprDetails();
      this.getInvestmentAprProducts();
      this.getInvestmentAprComment();
      this.getInvestmentTargetedGroup();
      if (this.sbu == this.investmentAprService.investmentAprFormData.sbu) {
        this.isInvOther = false;
        this.isValid = true;
        // this.getInvestmentTargetedProd();
      }
      else {
        this.isInvOther = true;
        this.isValid = false;
      }
      this.InvestmentAprSearchModalRef.hide()
      }
      getLastFiveInvestment(marketCode:string,toDayDate:string)
      {
      this.investmentAprService.getLastFiveInvestment(marketCode,toDayDate).subscribe(
      (response) => {
        
        this.investmentDetailsOld= response as IInvestmentDetailOld[];
      },
      (error) => {
        console.log(error);
      }
      );
      }
      getCampaignMst(){
        this.investmentAprService.getCampaignMsts().subscribe(response => {
          //
          this.campaignMsts = response as ICampaignMst[];
        }, error => {
            console.log(error);
        });
      }
      getInvestmentInit(){
      this.investmentAprService.getInvestmentInit(parseInt(this.empId),this.sbu).subscribe(response => {
        //
       this.investmentAprs = response.data;
       this.openInvestmentInitSearchModal(this.investmentInitSearchModal);
      }, error => {
          console.log(error);
      });
    }
    getInvestmentApproved(){
      this.investmentAprService.getInvestmentApproved(parseInt(this.empId),this.sbu).subscribe(response => {
        //
       this.investmentAprs = response.data;
       this.openInvestmentAprSearchModal(this.investmentAprSearchModal);
      }, error => {
          console.log(error);
     });
   }
   
   getInvestmentCampaign(){
    this.investmentAprService.getInvestmentCampaigns(this.investmentAprService.investmentAprFormData.id).subscribe(response => {
      
      
      var data=response[0] as IInvestmentCampaign;
      if(data!==undefined)
      {
      this.investmentAprService.investmentCampaignFormData=data;
      this.investmentAprService.investmentCampaignFormData.campaignMstId=data.campaignDtl.mstId;
      this.investmentAprService.investmentCampaignFormData.subCampaignName=data.campaignDtl.subCampaignName;
      this.investmentAprService.investmentCampaignFormData.doctorName=data.doctorInfo.doctorName;
      this.investmentAprService.investmentCampaignFormData.institutionName=data.institutionInfo.institutionName;
      this.investmentAprService.investmentCampaignFormData.subCampStartDate=new DatePipe('en-US').transform(data.campaignDtl.subCampStartDate, 'dd/MM/yyyy');
      this.investmentAprService.investmentCampaignFormData.subCampEndDate=new DatePipe('en-US').transform(data.campaignDtl.subCampEndDate, 'dd/MM/yyyy')
      this.investmentAprService.getCampaignMsts().subscribe(response => {
        this.campaignMsts = response as ICampaignMst[];
        for (let i = 0; i < this.campaignMsts.length; i++) {
          if(this.campaignMsts[i].id==this.investmentAprService.investmentCampaignFormData.campaignDtl.mstId)
          {
            this.investmentAprService.investmentCampaignFormData.campaignName=this.campaignMsts[i].campaignName;
          }
        }
        this.investmentAprService.getCampaignDtls(data.campaignDtl.mstId).subscribe(response => {
          
          this.campaignDtls = response as ICampaignDtl[];
          for (let i = 0; i < this.campaignDtls.length; i++) {
            if(this.campaignDtls[i].id==data.campaignDtl.id)
            {
              this.investmentAprService.investmentCampaignFormData.subCampaignName=this.campaignDtls[i].subCampaign.subCampaignName;
            }
          }
        }, error => {
            console.log(error);
        });
      }, error => {
          console.log(error);
      });
     
     
      this.investmentAprService.getCampaignDtlProducts(data.campaignDtl.id).subscribe(response => {
        
        this.campaignDtlProducts = response as ICampaignDtlProduct[];
      }, error => {
          console.log(error);
      });
    }
    else{
      this.toastr.warning('No Data Found', 'Investment ');
    }
      
    }, error => {
        console.log(error);
    });
    
  }
   getInvestmentBcds(){
    this.investmentAprService.getInvestmentBcds(this.investmentAprService.investmentAprFormData.id).subscribe(response => {
      var data=response[0] as IInvestmentBcds;
      if(data!==undefined)
      {
      this.investmentAprService.investmentBcdsFormData=data;
      this.investmentAprService.investmentBcdsFormData.responsibleDoctorName = data.doctorInfo.doctorName;
      this.investmentAprService.investmentBcdsFormData.bcdsName=data.bcds.bcdsName;
      this.investmentAprService.investmentBcdsFormData.bcdsAddress=data.bcds.bcdsAddress;
      this.investmentAprService.investmentBcdsFormData.noOfMember=data.bcds.noOfMember;
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
    this.investmentAprService.getInvestmentSociety(this.investmentAprService.investmentAprFormData.id).subscribe(response => {
      var data=response[0] as IInvestmentSociety;
      if(data!==undefined)
      {
      this.investmentAprService.investmentSocietyFormData=data;
      this.investmentAprService.investmentSocietyFormData.responsibleDoctorName = data.doctorInfo.doctorName;
      this.investmentAprService.investmentSocietyFormData.societyName=data.society.societyName;
      this.investmentAprService.investmentSocietyFormData.noOfMember=data.society.noOfMember;
      this.investmentAprService.investmentSocietyFormData.societyAddress=data.society.societyAddress;
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
    this.investmentAprService.getInvestmentInstitutions(this.investmentAprService.investmentAprFormData.id).subscribe(response => {
      var data=response[0] as IInvestmentInstitution;
      if(data!==undefined)
      {
      this.investmentAprService.investmentInstitutionFormData=data;
      this.investmentAprService.investmentInstitutionFormData.responsibleDoctorName=data.doctorInfo.doctorName;
      this.investmentAprService.investmentInstitutionFormData.institutionName=data.institutionInfo.institutionName;
      this.investmentAprService.investmentInstitutionFormData.address=data.institutionInfo.address;
      this.investmentAprService.investmentInstitutionFormData.institutionType=data.institutionInfo.institutionType;
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
    this.investmentAprService.getInvestmentDoctors(this.investmentAprService.investmentAprFormData.id).subscribe(response => {
      var data=response[0] as IInvestmentDoctor;
      if( data!==undefined)
      {
      this.investmentAprService.investmentDoctorFormData=data;
      this.investmentAprService.investmentDoctorFormData.doctorName=data.doctorInfo.doctorName;
      this.investmentAprService.investmentDoctorFormData.degree=data.doctorInfo.degree;
      this.investmentAprService.investmentDoctorFormData.designation=data.doctorInfo.designation;
      this.investmentAprService.investmentDoctorFormData.institutionName=data.institutionInfo.institutionName;
      this.investmentAprService.investmentDoctorFormData.address=data.institutionInfo.address;
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
  getInvestmentAprComment(){
    this.investmentAprService.getInvestmentAprComment(this.investmentAprService.investmentAprFormData.id,this.empId).subscribe(response => {
      var data=response[0] as IInvestmentAprComment;
      if( data!==undefined)
      {
      this.investmentAprService.investmentAprCommentFormData=data;
      
    }
    else{
      this.toastr.warning('No Data Found', 'Investment ');
    }
      
    }, error => {
        console.log(error);
    });
  }
  getInvestmentDetails(){
    this.investmentAprService.getInvestmentDetails(this.investmentAprService.investmentAprFormData.id ).subscribe(response => {
      var data=response[0] as IInvestmentApr;
      if(data!==undefined)
      {
      this.investmentAprService.investmentDetailFormData=data;
      this.investmentAprService.investmentDetailFormData.id=0;
     this.investmentAprService.investmentDetailFormData.fromDate=new Date(data.fromDate);
      this.investmentAprService.investmentDetailFormData.toDate=new Date(data.toDate);
      let convertedDate =this.datePipe.transform(data.fromDate, 'ddMMyyyy');
      this.getLastFiveInvestment(this.investmentAprService.investmentAprFormData.marketCode,convertedDate);
    } else{
      this.toastr.warning('No Data Found', 'Investment');
    }
       }, error => {
        console.log(error);
   });
 }
   getInvestmentTargetedProd(){
    this.investmentAprService.getInvestmentTargetedProds(this.investmentAprService.investmentAprFormData.id,this.sbu).subscribe(response => {
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
  getInvestmentAprDetails(){
    this.investmentAprService.getInvestmentAprDetails(this.investmentAprService.investmentAprFormData.id).subscribe(response => {
      var data=response[0] as IInvestmentApr;
      if(data!==undefined)
      {
      this.investmentAprService.investmentDetailFormData=data;
      this.investmentAprService.investmentDetailFormData.id=0;
     this.investmentAprService.investmentDetailFormData.fromDate=new Date(data.fromDate);
      this.investmentAprService.investmentDetailFormData.toDate=new Date(data.toDate);
      let convertedDate =this.datePipe.transform(data.fromDate, 'ddMMyyyy');
      this.getLastFiveInvestment(this.investmentAprService.investmentAprFormData.marketCode,convertedDate);
    } else{
      this.toastr.warning('No Data Found', 'Investment ');
    }
       }, error => {
        console.log(error);
   });
 }
   getInvestmentAprProducts(){
    //
    this.investmentAprService.getInvestmentAprProducts(this.investmentAprService.investmentAprFormData.id,this.sbu).subscribe(response => {
      
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
    this.investmentAprService.getInvestmentTargetedGroups(this.investmentAprService.investmentAprFormData.id).subscribe(response => {
      //
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
    this.getEmployeeId();
    this.getProduct();
    //this.getMarketGroupMsts();
    this.bsConfig = Object.assign({}, { containerClass: 'theme-green' }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();

  }
 
  changeDateInDetail(){
    //
    
    //this.printingDate=this.getDigitBanglaFromEnglish(this.datePipe.transform(value, "dd/MM/yyyy"));
    if(this.investmentAprService.investmentDetailFormData.fromDate==null || this.investmentAprService.investmentDetailFormData.fromDate==undefined )
    {
      
     return false;
    }
    if(this.investmentAprService.investmentDetailFormData.toDate==null || this.investmentAprService.investmentDetailFormData.toDate==undefined )
    {
      
     return false;
    }
    let dateFrom = this.investmentAprService.investmentDetailFormData.fromDate;
    let dateTo = this.investmentAprService.investmentDetailFormData.toDate;
    //let dateFrom = new Date();
    //let dateTo = new Date();

    this.investmentAprService.investmentDetailFormData.totalMonth = dateTo.getMonth() - dateFrom.getMonth() + (12 * (dateTo.getFullYear() - dateFrom.getFullYear()));

  }
  
 getProduct(){
  this.investmentAprService.getProduct(this.sbu).subscribe(response => {
    //
    this.products = response as IProduct[];
  }, error => {
      console.log(error);
  });
}
getEmployeeId(){
  //
  this.empId=this.accountService.getEmployeeId();
  this.investmentAprService.investmentAprCommentFormData.employeeId=parseInt(this.empId);
  this.getEmployeeSbu();
}
 getEmployeeSbu()
  {
    //
    this.accountService.getEmployeeSbu(this.investmentAprService.investmentAprCommentFormData.employeeId).subscribe(
      (response) => {
        //
        this.sbu= response.sbu;
      },
      (error) => {
        console.log(error);
      }
    );
  }
  onSubmit(form: NgForm) {
    if(this.investmentAprService.investmentAprCommentFormData.id==null || this.investmentAprService.investmentAprCommentFormData.id==undefined || this.investmentAprService.investmentAprCommentFormData.id==0)
   
      this.insertInvestmentApr();
    else
      this.updateInvestmentApr();
  }
  insertInvestmentApr() {
    this.investmentAprService.insertInvestmentApr().subscribe(
      res => {
        //
        this.investmentAprService.investmentAprCommentFormData=res as IInvestmentAprComment;
       //this.investmentAprService.investmentDoctorFormData.investmentInitId=this.investmentAprService.investmentAprFormData.id;
       //this.investmentAprService.investmentInstitutionFormData.investmentInitId=this.investmentAprService.investmentAprFormData.id;
        this.isValid=true;
        if(this.sbu==this.investmentAprService.investmentAprFormData.sbu)
        {this.insertInvestmentDetails();}
        this.insertInvestmentTargetedProd();
        this.toastr.success('Save successfully', 'Investment ')
      },
      err => { console.log(err); }
    );
  }
  updateInvestmentApr() {
    this.investmentAprService.updateInvestmentApr().subscribe(
      res => {
        //
        this.isValid=true;
        this.investmentAprService.investmentAprCommentFormData=res as IInvestmentAprComment;
        if(this.sbu==this.investmentAprService.investmentAprFormData.sbu)
        {this.insertInvestmentDetails();}
        this.insertInvestmentTargetedProd();
        //this.investmentAprService.investmentDoctorFormData.investmentInitId=this.investmentAprService.investmentAprFormData.id;
      // this.investmentAprService.investmentInstitutionFormData.investmentInitId=this.investmentAprService.investmentAprFormData.id;
        this.toastr.info('Updated successfully', 'Investment ')
      },
      err => { console.log(err); }
    );
  }
  insertInvestmentDetails() {
    if(this.investmentAprService.investmentAprFormData.id==null || this.investmentAprService.investmentAprFormData.id==undefined || this.investmentAprService.investmentAprFormData.id==0)
    {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    if(this.investmentAprService.investmentDetailFormData.proposedAmount==null || this.investmentAprService.investmentDetailFormData.proposedAmount==undefined || this.investmentAprService.investmentDetailFormData.proposedAmount=="")
    {
      this.toastr.warning('Enter Proposed Amount First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    if(this.investmentAprService.investmentDetailFormData.purpose==null || this.investmentAprService.investmentDetailFormData.purpose==undefined || this.investmentAprService.investmentDetailFormData.purpose=="")
    {
      this.toastr.warning('Enter Purpose First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    if(this.investmentAprService.investmentDetailFormData.fromDate==null || this.investmentAprService.investmentDetailFormData.fromDate==undefined )
    {
      this.toastr.warning('Select From Date  First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    if(this.investmentAprService.investmentDetailFormData.toDate==null || this.investmentAprService.investmentDetailFormData.toDate==undefined )
    {
      this.toastr.warning('Select To Date  First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    if(this.investmentAprService.investmentDetailFormData.commitmentAllSBU==null || this.investmentAprService.investmentDetailFormData.commitmentAllSBU==undefined || this.investmentAprService.investmentDetailFormData.commitmentAllSBU=="")
    {
      this.toastr.warning('Enter Commitment All SBU First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    if(this.investmentAprService.investmentDetailFormData.commitmentOwnSBU==null || this.investmentAprService.investmentDetailFormData.commitmentOwnSBU==undefined || this.investmentAprService.investmentDetailFormData.commitmentOwnSBU=="")
    {
      this.toastr.warning('Enter Commitment Own SBU First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    if(this.investmentAprService.investmentDetailFormData.paymentMethod==null || this.investmentAprService.investmentDetailFormData.paymentMethod==undefined || this.investmentAprService.investmentDetailFormData.paymentMethod=="")
    {
      this.toastr.warning('Select Payment Method First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    
    
    this.investmentAprService.investmentDetailFormData.investmentInitId =this.investmentAprService.investmentAprFormData.id;
     
    //if(this.investmentAprService.investmentDetailFormData.id==null || this.investmentAprService.investmentDetailFormData.id==undefined || this.investmentAprService.investmentDetailFormData.id==0)
    //{
      this.investmentAprService.insertInvestmentDetail(parseInt(this.empId),this.sbu).subscribe(
        res => {
         var data=res as IInvestmentApr;
         this.investmentAprService.investmentDetailFormData=data;
         //this.investmentAprService.investmentDoctorFormData.doctorName=String(data.doctorId);
         this.investmentAprService.investmentDetailFormData.fromDate=new Date(data.fromDate);
        this.investmentAprService.investmentDetailFormData.toDate=new Date(data.toDate);
         this.isDonationValid=true;
         this.toastr.success('Save successfully', 'Investment ');
        },
        err => { console.log(err); }
      );
    // }
    // else{
    //   this.investmentAprService.updateInvestmentDetail().subscribe(
    //     res => {
    //      var data=res as IInvestmentApr;
    //      this.investmentAprService.investmentDetailFormData=data;
    //      //this.investmentAprService.investmentDoctorFormData.doctorName=String(data.doctorId);
    //      this.investmentAprService.investmentDetailFormData.fromDate=new Date(data.fromDate);
    //     this.investmentAprService.investmentDetailFormData.toDate=new Date(data.toDate);
    //      this.isDonationValid=true;
    //      this.toastr.success('Save successfully', 'Investment ');
    //     },
    //     err => { console.log(err); }
    //   );
    // }
    
  }
 
 
  
  insertInvestmentTargetedProd() {
    //
    if(this.investmentAprService.investmentAprFormData.id==null || this.investmentAprService.investmentAprFormData.id==undefined || this.investmentAprService.investmentAprFormData.id==0)
    {
      this.toastr.warning('Insert Investment Initialisation First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    // if(this.investmentAprService.investmentDetailFormData.id==null || this.investmentAprService.investmentDetailFormData.id==undefined || this.investmentAprService.investmentDetailFormData.id==0)
    // {
    //   this.toastr.warning('Insert Investment Detail First', 'Investment ', {
    //     positionClass: 'toast-top-right' 
    //  });
    //  return false;
    // }
   
    // if(this.investmentAprService.investmentTargetedProdFormData.productId==null || this.investmentAprService.investmentTargetedProdFormData.productId==undefined || this.investmentAprService.investmentTargetedProdFormData.productId==0)
    // {
    //   this.toastr.warning('Select Product First', 'Investment ', {
    //     positionClass: 'toast-top-right' 
    //  });
    //  return false;
    // }
    if(this.investmentTargetedProds!==undefined){
      for (let i = 0; i < this.investmentTargetedProds.length; i++) {
        if(this.investmentTargetedProds[i].productInfo.id==this.investmentAprService.investmentTargetedProdFormData.productId)
        {
        alert("product already exist !");
        return false;
        }
      }
    }
    else{
         this.toastr.warning('Select Product First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
      return false;
    }
    this.investmentAprService.investmentTargetedProdFormData.investmentInitId =this.investmentAprService.investmentAprFormData.id;
    //if(this.investmentAprService.investmentTargetedProdFormData.id==null || this.investmentAprService.investmentTargetedProdFormData.id==undefined || this.investmentAprService.investmentTargetedProdFormData.id==0)
    //{
      
    this.investmentAprService.insertInvestmentTargetedProd(this.investmentTargetedProds).subscribe(
      res => {
        //
       //this.investmentAprService.investmentTargetedProdFormData=new InvestmentTargetedProd();
       
       this.getInvestmentTargetedProd();
       
       this.isDonationValid=true;
        this.toastr.success('Save successfully', 'Investment ');
      },
      err => { console.log(err); }
    );
    // }
    // else{
    //   this.investmentAprService.updateInvestmentTargetedProd().subscribe(
    //     res => {
    //       
    //      this.investmentAprService.investmentTargetedProdFormData=new InvestmentTargetedProd();
         
    //      this.getInvestmentTargetedProd();
         
    //      this.isDonationValid=true;
    //       this.toastr.success('Update successfully', 'Investment ');
    //     },
    //     err => { console.log(err); }
    //   );
    // }
  }
  addInvestmentTargetedProd() {
    //
    if(this.investmentAprService.investmentTargetedProdFormData.productId==null || this.investmentAprService.investmentTargetedProdFormData.productId==undefined || this.investmentAprService.investmentTargetedProdFormData.productId==0)
    {
      this.toastr.warning('Select Product First', 'Investment ', {
        positionClass: 'toast-top-right' 
     });
     return false;
    }
    if(this.investmentTargetedProds!==undefined){
      for (let i = 0; i < this.investmentTargetedProds.length; i++) {
        if(this.investmentTargetedProds[i].productInfo.id==this.investmentAprService.investmentTargetedProdFormData.productId)
        {
        alert("product already exist !");
        return false;
        }

      }
      for (let i = 0; i < this.products.length; i++) {
        if(this.products[i].id==this.investmentAprService.investmentTargetedProdFormData.productId)
        {
          let data=new InvestmentTargetedProd();
          data.employeeId = parseInt(this.empId);
          data.investmentInitId=this.investmentAprService.investmentAprFormData.id;
          data.productId=this.investmentAprService.investmentTargetedProdFormData.productId;
          data.productInfo=this.products[i];
          //data.productInfo.push({ id: this.products[i].id, productName: this.products[i].productName,productCode: this.products[i].productCode});
          
          //data.productInfo.productName=this.products[i].productName;
          //data.productInfo.productCode=this.products[i].productCode;
          this.investmentTargetedProds.push(data);
        return false;
        }

      }
      
      
      // this.investmentTargetedProds.push(      
      //   { id: 0, investmentInitId: this.investmentAprService.investmentAprFormData.id,productId:0 });
    }
    
  }
  
  editInvestmentTargetedProd(selectedAprord: IInvestmentTargetedProd) {
    this.investmentAprService.investmentTargetedProdFormData = Object.assign({}, selectedAprord);
    // var e = (document.getElementById("marketCode")) as HTMLSelectElement;
    // var sel = e.selectedIndex;
    // var opt = e.options[sel];
    // var selectedMarketCode = opt.value;
    // var selectedMarketName = opt.innerHTML;
    
  }
  populateForm() {
    //this.investmentAprService.campaignFormData = Object.assign({}, selectedAprord);
  }
  resetPage(form: NgForm) {
    form.reset();
    this.investmentAprService.investmentAprFormData=new InvestmentInit();
    this.isValid=false;
  }
  
  removeInvestmentTargetedProd(selectedAprord: IInvestmentTargetedProd) {
  if (this.investmentTargetedProds.find(x => x.productId == selectedAprord.productId)) {
    this.investmentTargetedProds.splice(this.investmentTargetedProds.findIndex(x => x.productId == selectedAprord.productId), 1);
   }
  if(this.investmentAprService.investmentAprCommentFormData.id==null || this.investmentAprService.investmentAprCommentFormData.id==undefined || this.investmentAprService.investmentAprCommentFormData.id==0)
    {
      return false;
    }
  this.investmentAprService.investmentTargetedProdFormData = Object.assign({}, selectedAprord);
  var c = confirm("Are you sure you want to delete that?"); 
    if (c == true) {  
    this.investmentAprService.removeInvestmentTargetedProd().subscribe(
      res => {
        //
        this.toastr.success(res);
        //this.isDonationValid=false;
        this.investmentAprService.investmentTargetedProdFormData=new InvestmentTargetedProd();
        this.getInvestmentTargetedProd();
      },
      err => { console.log(err); }
    );
  }

  }

}

