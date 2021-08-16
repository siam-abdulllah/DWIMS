
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
import { AccountService } from '../account/account.service';
@Component({
  selector: 'app-investmentRec',
  templateUrl: './investmentRec.component.html',
  styles: [
  ],
  providers: [DatePipe]
})
export class InvestmentRecComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
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
  empId:string;
  sbu:string;
  config = {
    keyboard: false,
    class: 'modal-lg',
    ignoreBackdropClick: true
  };
  constructor(private accountService: AccountService,public investmentRecService: InvestmentRecService, private router: Router,
    private toastr: ToastrService,private modalService: BsModalService,private datePipe: DatePipe) { }

    openInvestmentInitSearchModal(template: TemplateRef<any>) {
      this.InvestmentInitSearchModalRef = this.modalService.show(template, this.config);
    }
    openInvestmentRecSearchModal(template: TemplateRef<any>) {
      this.InvestmentRecSearchModalRef = this.modalService.show(template, this.config);
    }
    selectInvestmentInit(selectedRecord: IInvestmentInit) {
      //debugger;
      this.investmentRecService.investmentRecFormData = Object.assign({}, selectedRecord);
      this.investmentRecService.investmentDetailFormData.investmentInitId =selectedRecord.id;
      this.investmentRecService.investmentRecCommentFormData.investmentInitId =selectedRecord.id;
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
      //this.getCampaignMst();
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
      this.InvestmentInitSearchModalRef.hide()
      }
    selectInvestmentRec(selectedRecord: IInvestmentInit) {
      //debugger;
      this.investmentRecService.investmentRecFormData = Object.assign({}, selectedRecord);
      this.investmentRecService.investmentDetailFormData.investmentInitId =selectedRecord.id;
      this.investmentRecService.investmentRecCommentFormData.investmentInitId =selectedRecord.id;
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
      //this.getCampaignMst();
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
      this.getInvestmentRecDetails();
      this.getInvestmentRecProducts();
      this.getInvestmentRecComment();
      this.getInvestmentTargetedGroup();
      this.isValid=true;
      this.InvestmentRecSearchModalRef.hide()
      }
      getCampaignMst(){
        this.investmentRecService.getCampaignMsts().subscribe(response => {
          //debugger;
          this.campaignMsts = response as ICampaignMst[];
        }, error => {
            console.log(error);
        });
      }
      getInvestmentInit(){
      this.investmentRecService.getInvestmentInit(this.sbu).subscribe(response => {
        //debugger;
       this.investmentRecs = response.data;
       this.openInvestmentInitSearchModal(this.investmentInitSearchModal);
      }, error => {
          console.log(error);
      });
    }
   getInvestmentRecommended(){
      this.investmentRecService.getInvestmentRecommended(this.sbu).subscribe(response => {
        //debugger;
       this.investmentRecs = response.data;
       this.openInvestmentRecSearchModal(this.investmentRecSearchModal);
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
      this.investmentRecService.investmentCampaignFormData.subCampaignName=data.campaignDtl.subCampaignName;
      this.investmentRecService.investmentCampaignFormData.doctorName=data.doctorInfo.doctorName;
      this.investmentRecService.investmentCampaignFormData.institutionName=data.institutionInfo.institutionName;
      this.investmentRecService.investmentCampaignFormData.subCampStartDate=new DatePipe('en-US').transform(data.campaignDtl.subCampStartDate, 'dd/MM/yyyy');
      this.investmentRecService.investmentCampaignFormData.subCampEndDate=new DatePipe('en-US').transform(data.campaignDtl.subCampEndDate, 'dd/MM/yyyy')
      this.investmentRecService.getCampaignMsts().subscribe(response => {
        this.campaignMsts = response as ICampaignMst[];
        for (let i = 0; i < this.campaignMsts.length; i++) {
          if(this.campaignMsts[i].id==this.investmentRecService.investmentCampaignFormData.campaignDtl.mstId)
          {
            this.investmentRecService.investmentCampaignFormData.campaignName=this.campaignMsts[i].campaignName;
          }
        }
        this.investmentRecService.getCampaignDtls(data.campaignDtl.mstId).subscribe(response => {
          debugger;
          this.campaignDtls = response as ICampaignDtl[];
          for (let i = 0; i < this.campaignDtls.length; i++) {
            if(this.campaignDtls[i].id==data.campaignDtl.id)
            {
              this.investmentRecService.investmentCampaignFormData.subCampaignName=this.campaignDtls[i].subCampaign.subCampaignName;
            }
          }
        }, error => {
            console.log(error);
        });
      }, error => {
          console.log(error);
      });
     
     
      this.investmentRecService.getCampaignDtlProducts(data.campaignDtl.id).subscribe(response => {
        debugger;
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
    this.investmentRecService.getInvestmentBcds(this.investmentRecService.investmentRecFormData.id).subscribe(response => {
      //debugger;
      
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
      //debugger;
      
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
      //debugger;
      
      var data=response[0] as IInvestmentInstitution;
      if(data!==undefined)
      {
      this.investmentRecService.investmentInstitutionFormData=data;
      this.investmentRecService.investmentInstitutionFormData.resposnsibleDoctorName=data.doctorInfo.doctorName;
      this.investmentRecService.investmentInstitutionFormData.institutionName=data.institutionInfo.institutionName;
      this.investmentRecService.investmentInstitutionFormData.address=data.institutionInfo.address;
      this.investmentRecService.investmentInstitutionFormData.institutionType=data.institutionInfo.institutionType;
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
      //debugger;
      var data=response[0] as IInvestmentDoctor;
      if( data!==undefined)
      {
      this.investmentRecService.investmentDoctorFormData=data;
      this.investmentRecService.investmentDoctorFormData.doctorName=data.doctorInfo.doctorName;
      this.investmentRecService.investmentDoctorFormData.degree=data.doctorInfo.degree;
      this.investmentRecService.investmentDoctorFormData.designation=data.doctorInfo.designation;
      this.investmentRecService.investmentDoctorFormData.institutionName=data.institutionInfo.institutionName;
      this.investmentRecService.investmentDoctorFormData.address=data.institutionInfo.address;
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
  getInvestmentRecComment(){
    this.investmentRecService.getInvestmentRecComment(this.investmentRecService.investmentRecFormData.id,this.empId).subscribe(response => {
      //debugger;
      var data=response[0] as IInvestmentRecComment;
      if( data!==undefined)
      {
      this.investmentRecService.investmentRecCommentFormData=data;
      
    }
    else{
      this.toastr.warning('No Data Found', 'Investment ');
    }
      
    }, error => {
        console.log(error);
    });
  }
  getInvestmentDetails(){
    this.investmentRecService.getInvestmentDetails(this.investmentRecService.investmentRecFormData.id ).subscribe(response => {
      //debugger;
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
   getInvestmentTargetedProd(){
    this.investmentRecService.getInvestmentTargetedProds(this.investmentRecService.investmentRecFormData.id,this.sbu).subscribe(response => {
      //debugger;
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
  getInvestmentRecDetails(){
    //debugger;
    this.investmentRecService.getInvestmentRecDetails(this.investmentRecService.investmentRecFormData.id).subscribe(response => {
      
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
   getInvestmentRecProducts(){
    //debugger;
    this.investmentRecService.getInvestmentRecProducts(this.investmentRecService.investmentRecFormData.id,this.sbu).subscribe(response => {
      
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
      //debugger;
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
    //debugger;
    
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
  this.investmentRecService.getProduct(this.sbu).subscribe(response => {
    //debugger;
    this.products = response as IProduct[];
  }, error => {
      console.log(error);
  });
}
getEmployeeId(){
  //debugger;
  this.empId=this.accountService.getEmployeeId();
  this.investmentRecService.investmentRecCommentFormData.employeeId=parseInt(this.empId);
  this.getEmployeeSbu();
}
 getEmployeeSbu()
  {
    //debugger;
    this.accountService.getEmployeeSbu(this.investmentRecService.investmentRecCommentFormData.employeeId).subscribe(
      (response) => {
        //debugger;
        this.sbu= response.sbu;
      },
      (error) => {
        console.log(error);
      }
    );
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
        this.insertInvestmentDetails();
        this.insertInvestmentTargetedProd();
        this.toastr.success('Save successfully', 'Investment ')
      },
      err => { console.log(err); }
    );
  }
  updateInvestmentRec() {
    this.investmentRecService.updateInvestmentRec().subscribe(
      res => {
        //debugger;
        this.isValid=true;
        this.investmentRecService.investmentRecCommentFormData=res as IInvestmentRecComment;
        this.insertInvestmentDetails();
        this.insertInvestmentTargetedProd();
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
     
    //if(this.investmentRecService.investmentDetailFormData.id==null || this.investmentRecService.investmentDetailFormData.id==undefined || this.investmentRecService.investmentDetailFormData.id==0)
    //{
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
    //debugger;
    if(this.investmentRecService.investmentRecFormData.id==null || this.investmentRecService.investmentRecFormData.id==undefined || this.investmentRecService.investmentRecFormData.id==0)
    {
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
    if(this.investmentTargetedProds!==undefined){
      for (let i = 0; i < this.investmentTargetedProds.length; i++) {
        if(this.investmentTargetedProds[i].productInfo.id==this.investmentRecService.investmentTargetedProdFormData.productId)
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
    this.investmentRecService.investmentTargetedProdFormData.investmentInitId =this.investmentRecService.investmentRecFormData.id;
    //if(this.investmentRecService.investmentTargetedProdFormData.id==null || this.investmentRecService.investmentTargetedProdFormData.id==undefined || this.investmentRecService.investmentTargetedProdFormData.id==0)
    //{
    this.investmentRecService.insertInvestmentTargetedProd(this.investmentTargetedProds).subscribe(
      res => {
        //debugger;
       //this.investmentRecService.investmentTargetedProdFormData=new InvestmentTargetedProd();
       
       this.getInvestmentTargetedProd();
       
       this.isDonationValid=true;
        this.toastr.success('Save successfully', 'Investment ');
      },
      err => { console.log(err); }
    );
    // }
    // else{
    //   this.investmentRecService.updateInvestmentTargetedProd().subscribe(
    //     res => {
    //       debugger;
    //      this.investmentRecService.investmentTargetedProdFormData=new InvestmentTargetedProd();
         
    //      this.getInvestmentTargetedProd();
         
    //      this.isDonationValid=true;
    //       this.toastr.success('Update successfully', 'Investment ');
    //     },
    //     err => { console.log(err); }
    //   );
    // }
  }
  addInvestmentTargetedProd() {
    //debugger;
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
        //debugger;
        this.toastr.success(res);
        //this.isDonationValid=false;
        this.investmentRecService.investmentTargetedProdFormData=new InvestmentTargetedProd();
        this.getInvestmentTargetedProd();
      },
      err => { console.log(err); }
    );
  }

  }

}

