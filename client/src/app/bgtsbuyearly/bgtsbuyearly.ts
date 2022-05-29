import { DatePipe } from "@angular/common";
import { Component, ElementRef, OnInit, TemplateRef, ViewChild } from "@angular/core";
import { Router } from "@angular/router";
import { BsModalService,BsModalRef} from "ngx-bootstrap/modal";
import { NgxSpinnerService } from "ngx-spinner";
import { ToastrService } from 'ngx-toastr';
import { Observable } from "rxjs/internal/Observable";
import { AccountService } from "../account/account.service";
import { ApprovalAuthDetails, BudgetSbuYearly, BudgetYearly, IApprovalAuthDetails, IAuthExpense, IBudgetSbuYearly, IBudgetYearly, IPipelineDetails, ISbuDetails, SbuDetails } from "../shared/models/budgetyearly";
import { IEmployeeInfo } from "../shared/models/employeeInfo";
import { ISBU } from "../shared/models/sbu";
import { BudgetSbuYearlyService } from "../_services/budgetSbuYearly.service";



@Component({
  selector: 'app-bgtyearly',
  templateUrl: './bgtsbuyearly.component.html',
  styles: [
  ],
  providers: [DatePipe]
})
export class BgtSbuYearlyComponent implements OnInit {
  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  @ViewChild('authDetailsModal', { static: false }) authDetailsModal: TemplateRef<any>;
  @ViewChild('submissionConfirmModal', { static: false }) submissionConfirmModal: TemplateRef<any>;
  submissionConfirmRef: BsModalRef;
  bsValue: Date = new Date();
  today = new Date();
  AuthDetailsModalRef: BsModalRef;
  empId: string;
  userRole:string;
  totalSbuBudget:any;
  allowcatedBudget:any;
  totalBudget:any;
  remainingBudget:any;
  authTotalExpense:any;
  deptId: any;
  SBUs: ISBU[];
  bgtYearlyTotal: BudgetYearly;
  bgtSbuYearlyList: IBudgetSbuYearly[];
  approvalAuthDetails:ApprovalAuthDetails[];
  pipelineList: IPipelineDetails[];
  authExpenseList: IAuthExpense[];
  sbuDetails:ISbuDetails[];
  empSbu:Observable<IEmployeeInfo>;
  sbuIndividualAmount:any;
  SbuName:string;
  SbuCode:string;
  expense:any;
  isAdmin: boolean = false;
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
  constructor(private accountService: AccountService,private router: Router,public bugetSbuYearlyService: BudgetSbuYearlyService,private toastr: ToastrService, private modalService: BsModalService, private datePipe: DatePipe, 
    private SpinnerService: NgxSpinnerService) { }
  ngOnInit() {
      this.getSBU()
      this.getEmployeeId()

      this.bugetSbuYearlyService.budgetSbuYearly.year = 2022;
      this.sbuDetails = []

  }
  getSBU() {
    this.bugetSbuYearlyService.getSBU().subscribe(response => {
      this.SBUs = response as ISBU[];
    }, error => {
      console.log(error);
    });
  }
  getEmployeeId() {
    this.empId = this.accountService.getEmployeeId();
    this.bugetSbuYearlyService.budgetSbuYearly.enteredBy = parseInt(this.empId);
  }
  openBudgetYearlySearchModal(template: TemplateRef<any>) {
    this.AuthDetailsModalRef = this.modalService.show(template, this.config);
  }
  
  getbgtEmployeeForSbu(SbuName:string,deptId:number,year:number,comId:number) {

    this.bugetSbuYearlyService.getbgtEmployeeForSbu(SbuName,deptId,year,comId).subscribe(response => {
      debugger;
      this.approvalAuthDetails = response as IApprovalAuthDetails[];
      return this.approvalAuthDetails;
        this.SpinnerService.hide();
    }, error => {
      console.log(error);
    });
  }
  getApprovalAuth(SbuName:string,SbuCode:string,sbuAmount:any,expense:any) {
    this.getAllAuthWiseExpenseList(SbuCode, this.bugetSbuYearlyService.budgetSbuYearly.deptId, this.bugetSbuYearlyService.budgetSbuYearly.year,
      this.bugetSbuYearlyService.budgetSbuYearly.compId)
    this.sbuIndividualAmount = sbuAmount;
    this.SbuName = SbuName;
    this.SbuCode = SbuCode;
    this.expense = expense;
    this.allowcatedBudget = 0;
    this.remainingBudget = 0;

    this.bugetSbuYearlyService.getbgtEmployeeForSbu(SbuCode, this.bugetSbuYearlyService.budgetSbuYearly.deptId, this.bugetSbuYearlyService.budgetSbuYearly.year,
      this.bugetSbuYearlyService.budgetSbuYearly.compId).subscribe(response => {
      debugger;
  
      this.approvalAuthDetails = response as IApprovalAuthDetails[];
      if(this.approvalAuthDetails == null || this.approvalAuthDetails.length == 0)
      {
        this.bugetSbuYearlyService.getAppAuthDetails(SbuCode,this.bugetSbuYearlyService.budgetSbuYearly.deptId).subscribe(response => {
          debugger;
          this.approvalAuthDetails = response as IApprovalAuthDetails[];
          if (this.approvalAuthDetails.length > 0) {
            for(var i=0;i<this.approvalAuthDetails.length;i++)
            {
              this.approvalAuthDetails[i].deptId = this.bugetSbuYearlyService.budgetSbuYearly.deptId;
              this.approvalAuthDetails[i].compId = this.bugetSbuYearlyService.budgetSbuYearly.compId;
              this.approvalAuthDetails[i].sbu = this.SbuName;
              this.approvalAuthDetails[i].enteredBy = this.empId;
            }
            }
            else {
              this.toastr.warning('No Data Found');
            }
            this.SpinnerService.hide();
        }, error => {
          console.log(error);
        });
      }
      else{
        for(var i=0;i<this.approvalAuthDetails.length;i++)
        {
          this.allowcatedBudget = this.allowcatedBudget + this.approvalAuthDetails[i].amount;
        }
     
      }
   
      this.openBudgetYearlySearchModal(this.authDetailsModal);
    }, error => {
      console.log(error);
    });
  }


  getYearlyBudget() {
    if(typeof(this.bugetSbuYearlyService.budgetSbuYearly.year) == 'undefined')
    {
      this.toastr.warning('Plaease Select Year First!');
      return;
    }
 
    if (this.bugetSbuYearlyService.budgetSbuYearly.year != null && this.bugetSbuYearlyService.budgetSbuYearly.year  != undefined && typeof(this.bugetSbuYearlyService.budgetSbuYearly.year)  != 'number') {
      var year = new Date(this.bugetSbuYearlyService.budgetSbuYearly.year).getFullYear();
      this.bugetSbuYearlyService.budgetSbuYearly.year = year;
    }
    this.bugetSbuYearlyService.getYearlyBudget(this.bugetSbuYearlyService.budgetSbuYearly.deptId,this.bugetSbuYearlyService.budgetSbuYearly.year).subscribe(response => {
      this.bgtYearlyTotal = response as BudgetYearly;

      this.bugetSbuYearlyService.budgetSbuYearly.totalBudget = this.bgtYearlyTotal.totalAmount;
      this.getAllSbuBgtList();
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
    this.submitBgtYearly();
  }
  declineSubmit() {
    this.submissionConfirmRef.hide();
  }
  openSubmissionConfirmModal(template: TemplateRef<any>) {
    this.submissionConfirmRef = this.modalService.show(template, {
      keyboard: false,
      class: 'modal-md',
      ignoreBackdropClick: true
    });
  }


  resetPageLoad() {
    this.bugetSbuYearlyService.budgetSbuYearly = new BudgetSbuYearly();
    this.sbuDetails = [];
  }
  UpdateSbuDetails(id:number,newAmount:any,expense:any)
  {
    debugger;
    this.bugetSbuYearlyService.budgetSbuYearly.id = id;
    this.bugetSbuYearlyService.budgetSbuYearly.sbuAmount = newAmount;
    if(newAmount < expense)
    {
      this.toastr.warning('Budget can not be lower then expense!');
    }
    else{
      this.bugetSbuYearlyService.updateSbuBudgetYearly().subscribe(
        res => {
          this.bugetSbuYearlyService.budgetSbuYearly = res as IBudgetSbuYearly;
          this.toastr.success('Updated successfully', 'Sbu Budget');
          this.resetPageLoad();
        },
        err => { 
          console.log(err); 
        }
      );
    }
   
  }
  UpdateAppAuthDetails(id:number,newAmount:any,expense:any,canView:any,canEdit:any)
  {
    this.bugetSbuYearlyService.budgetEmployee.id = id;
    this.bugetSbuYearlyService.budgetEmployee.amount = newAmount;
    this.bugetSbuYearlyService.budgetEmployee.permEdit = canEdit;
    this.bugetSbuYearlyService.budgetEmployee.permView = canView;
    if(newAmount < expense)
    {
      this.toastr.warning('Budget can not be lower then expense!');
      return;
    }
   
    debugger;
    var totalAllowcatedAuthbudget= 0;
    var flag = 0;
    for(var i=0;i<this.approvalAuthDetails.length;i++)
    {
       var num2 = 0;
       if(this.approvalAuthDetails[i].id == id)
       {
         num2 = parseInt(this.approvalAuthDetails[i].amount);
       }
       else{
         num2 = parseInt(newAmount);
       }
         
         totalAllowcatedAuthbudget = totalAllowcatedAuthbudget+num2;
    }
    if(totalAllowcatedAuthbudget> this.sbuIndividualAmount)
    {
      this.toastr.warning('Budget limit has exceeded.');
      return;
    }
      this.bugetSbuYearlyService.updateBgtEmployee().subscribe(
        res => {
          this.AuthDetailsModalRef.hide()
          this.bugetSbuYearlyService.budgetEmployee = res as IApprovalAuthDetails;
          this.getApprovalAuth(this.SbuName,this.SbuCode,this.sbuIndividualAmount,this.expense)
          this.toastr.success('Updated successfully', 'Sbu Budget');
     
        },
        err => { 
          console.log(err); 
        }
      );
  }
  getAllSbuBgtList()
  {
  
    this.sbuDetails =[];
      this.SpinnerService.show();
      debugger;
      this.bugetSbuYearlyService.getAllSbuBgtList(this.bugetSbuYearlyService.budgetSbuYearly.deptId,this.bugetSbuYearlyService.budgetSbuYearly.compId,this.bugetSbuYearlyService.budgetSbuYearly.year).subscribe(
        res => {
          this.bgtSbuYearlyList = res as IBudgetSbuYearly[];
     
          this.totalBudget =0;
          if(this.bgtSbuYearlyList != null)
          {
            for(var i=0;i<this.bgtSbuYearlyList.length;i++)
            {
               let sbu = new SbuDetails();
       
               sbu.sbuAmount = this.bgtSbuYearlyList[i].sbuAmount;
               sbu.sbuName = this.bgtSbuYearlyList[i].sbuName;
               sbu.sbuCode = this.bgtSbuYearlyList[i].sbuCode;
               sbu.bgtSbuId = this.bgtSbuYearlyList[i].bgtSbuId;
               sbu.newAmount = 0;
               sbu.expense = this.bgtSbuYearlyList[i].expense;
               sbu.totalAlowcated =  this.bgtSbuYearlyList[i].totalAllowcated;
               this.sbuDetails.push(sbu);
               const total = parseInt(this.totalBudget);
               const next = parseInt(this.bgtSbuYearlyList[i].sbuAmount);
               this.totalBudget = total+next;
            }
          }
        setTimeout(() => {
          debugger;
          this.bugetSbuYearlyService.getTotalExpense(this.bugetSbuYearlyService.budgetSbuYearly.deptId,this.bugetSbuYearlyService.budgetSbuYearly.year).subscribe(response => {
            this.bugetSbuYearlyService.budgetSbuYearly.totalExpense = response; 
            var RemainingBudget = parseInt(this.bugetSbuYearlyService.budgetSbuYearly.totalBudget)-  (this.bugetSbuYearlyService.budgetSbuYearly.totalExpense)
            this.bugetSbuYearlyService.budgetSbuYearly.remainingBudget = RemainingBudget;
           }, error => {
      
             console.log(error);
           });
           this.getAllPipeLineExpenseList();
        }, 1000);
         
    
        },
        err => { 
          console.log(err); 
        }
      );
  }



  getAllPipeLineExpenseList()
  {

   
      this.SpinnerService.show();
      debugger;
      this.bugetSbuYearlyService.getAllPipelineExpenseList(this.bugetSbuYearlyService.budgetSbuYearly.deptId,this.bugetSbuYearlyService.budgetSbuYearly.compId,this.bugetSbuYearlyService.budgetSbuYearly.year).subscribe(
        res => {
          debugger;
          this.pipelineList = res as IPipelineDetails[];
          for(var i =0;i<this.sbuDetails.length;i++)
          {
            for(var j =0;j<this.pipelineList.length;j++)
            {
              if(this.sbuDetails[i].sbuCode == this.pipelineList[j].sbuCode)
              {
                this.sbuDetails[i].pipeLine = this.pipelineList[j].pipeline;
             
              }
            }
            this.sbuDetails[i].remaining = parseInt(this.sbuDetails[i].sbuAmount) - parseInt(this.sbuDetails[i].totalAlowcated) 
          }
        },
        err => { 
          console.log(err); 
        }
      );
  }
  getAllAuthWiseExpenseList(sbuCode:string,deptId:number,year:number,comId:number)
  {
      this.SpinnerService.show();
      debugger;
      this.authTotalExpense = 0;
      this.bugetSbuYearlyService.getAllAuthExpenseList(sbuCode,deptId,year,comId).subscribe(
        res => {
          debugger;
          this.authExpenseList = res as IAuthExpense[];
          if(this.authExpenseList != undefined && this.authExpenseList.length > 0)
          {
             for(var i=0;i<this.approvalAuthDetails.length;i++)
             {
               for(var j=0;j<this.authExpenseList.length;j++)
               {
                  if(this.approvalAuthDetails[i].authority == this.authExpenseList[j].remarks)
                  {
                    this.approvalAuthDetails[i].expense = this.authExpenseList[j].expense
                  }
               }
               if( this.approvalAuthDetails[i].expense == '' ||this.approvalAuthDetails[i].expense == undefined)
               {
                  this.approvalAuthDetails[i].expense =0;
               }
               this.authTotalExpense = this.authTotalExpense+this.approvalAuthDetails[i].expense ;
             }
          }
          this.remainingBudget =  this.sbuIndividualAmount - (this.allowcatedBudget + this.authTotalExpense);
        },
        err => { 
          console.log(err); 
        }
      );
  }
  saveAuthSbuDetails(){
    debugger;
    this.bugetSbuYearlyService.approvalAuthDetailsModel.deptId =  this.bugetSbuYearlyService.budgetSbuYearly.deptId;
    this.bugetSbuYearlyService.approvalAuthDetailsModel.compId =  this.bugetSbuYearlyService.budgetSbuYearly.compId;
    this.bugetSbuYearlyService.approvalAuthDetailsModel.sbu =   this.SbuName;
    this.bugetSbuYearlyService.approvalAuthDetailsModel.sbuCode =   this.SbuCode;
    this.bugetSbuYearlyService.approvalAuthDetailsModel.year =  this.bugetSbuYearlyService.budgetSbuYearly.year;
    this.bugetSbuYearlyService.approvalAuthDetailsModel.bgtEmpList = this.approvalAuthDetails;

    var flag = 0;
    var totalAllowcatedAuthbudget = 0;
    for(var i=0;i<this.approvalAuthDetails.length;i++)
    {
       if(this.approvalAuthDetails[i].newAmount< this.approvalAuthDetails[i].expense  || this.approvalAuthDetails[i].newAmount == undefined)
       {
        flag = 1;
       }
        const num2 = parseInt(this.approvalAuthDetails[i].newAmount);
        totalAllowcatedAuthbudget = totalAllowcatedAuthbudget+num2;
    }
    if(totalAllowcatedAuthbudget> (this.sbuIndividualAmount- this.expense))
    {
      this.toastr.warning('Budget limit has exceeded!');
      return;
    }
    if(flag==1)
    {
      this.toastr.warning('Budget can not be lower then expense!');
      this.SpinnerService.hide();
      return;
    }
    this.bugetSbuYearlyService.saveAuthSbuDetails().subscribe(
      res => {
        //this.bugetSbuYearlyService.budgetSbuYearly = res as IBudgetSbuYearly;
        this.AuthDetailsModalRef.hide()
        this.getApprovalAuth(this.SbuName,this.SbuCode,this.sbuIndividualAmount,this.expense)
        this.toastr.success('Submitted successfully', 'Sbu Budget');
      
      },
      err => { 
        console.log(err); 
      }
    );
  }
  submitBgtYearly() {
      this.SpinnerService.show();
      debugger;
      this.bugetSbuYearlyService.budgetSbuYearly.sbuDetailsList = this.sbuDetails;
      this.totalSbuBudget= 0;
      var flag = 0;
      for(var i=0;i<this.sbuDetails.length;i++)
      {
         if(this.sbuDetails[i].newAmount < this.sbuDetails[i].expense ||this.sbuDetails[i].newAmount == undefined)
         {
          flag = 1;
         }
        const num1 = parseInt(this.totalSbuBudget);
        const num2 = parseInt(this.sbuDetails[i].newAmount);
        this.totalSbuBudget = num1+num2;
      }
      if(flag==1)
      {
        this.toastr.warning('Budget can not be lower then expense!');
        this.SpinnerService.hide();
        return;
      }
      console.log(this.totalSbuBudget);
      if(this.totalSbuBudget > (this.bugetSbuYearlyService.budgetSbuYearly.totalBudget - this.bugetSbuYearlyService.budgetSbuYearly.totalExpense))
      {
        this.toastr.warning('Budget limit has exceeded!');
        this.SpinnerService.hide();
        return;
      }
      this.bugetSbuYearlyService.submitSbuBudgetYearly().subscribe(
        res => {
          this.bugetSbuYearlyService.budgetSbuYearly = res as IBudgetSbuYearly;
          this.toastr.success('Submitted successfully', 'Sbu Budget');
          this.resetPageLoad();
        },
        err => { 
          console.log(err); 
        }
      );
  }
}

