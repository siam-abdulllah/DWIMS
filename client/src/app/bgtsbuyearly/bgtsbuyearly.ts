import { DatePipe } from "@angular/common";
import { Component, ElementRef, OnInit, TemplateRef, ViewChild } from "@angular/core";
import { Router } from "@angular/router";
import { BsModalService,BsModalRef} from "ngx-bootstrap/modal";
import { NgxSpinnerService } from "ngx-spinner";
import { ToastrService } from 'ngx-toastr';
import { Observable } from "rxjs/internal/Observable";
import { AccountService } from "../account/account.service";
import { BudgetSbuYearly, BudgetYearly, IBudgetSbuYearly, IBudgetYearly, IPipelineDetails, ISbuDetails, SbuDetails } from "../shared/models/budgetyearly";
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
  @ViewChild('investmentRapidSearchModal', { static: false }) investmentInitSearchModal: TemplateRef<any>;
  @ViewChild('submissionConfirmModal', { static: false }) submissionConfirmModal: TemplateRef<any>;
  submissionConfirmRef: BsModalRef;
  bsValue: Date = new Date();
  today = new Date();
  empId: string;
  userRole:string;
  totalSbuBudget:any;
  totalBudget:any;
  SBUs: ISBU[];
  bgtYearlyTotal: BudgetYearly;
  bgtSbuYearlyList: IBudgetSbuYearly[];
  pipelineList: IPipelineDetails[];
  sbuDetails:ISbuDetails[];
  empSbu:Observable<IEmployeeInfo>;
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
    
      this.sbuDetails = []
  }
  getSBU() {
    this.bugetSbuYearlyService.getSBU().subscribe(response => {
      this.SBUs = response as ISBU[];
    }, error => {
      console.log(error);
    });
  }
  getYearlyBudget() {
   debugger;
    this.bugetSbuYearlyService.getYearlyBudget(this.bugetSbuYearlyService.budgetSbuYearly.deptId).subscribe(response => {
      this.bgtYearlyTotal = response as BudgetYearly;
      this.bugetSbuYearlyService.budgetSbuYearly.year = this.bgtYearlyTotal.year;
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
  getAllSbuBgtList()
  {

    this.sbuDetails =[];
      this.SpinnerService.show();
      debugger;
      this.bugetSbuYearlyService.getAllSbuBgtList(this.bugetSbuYearlyService.budgetSbuYearly.deptId,this.bugetSbuYearlyService.budgetSbuYearly.compId,this.bugetSbuYearlyService.budgetSbuYearly.year).subscribe(
        res => {
          this.bgtSbuYearlyList = res as IBudgetSbuYearly[];
          this.totalBudget =0;
          for(var i=0;i<this.bgtSbuYearlyList.length;i++)
          {
             let sbu = new SbuDetails();
     
             sbu.sbuAmount = this.bgtSbuYearlyList[i].sbuAmount;
             sbu.sbuName = this.bgtSbuYearlyList[i].sbuName;
             sbu.sbuCode = this.bgtSbuYearlyList[i].sbuCode;
             sbu.newAmount = this.bgtSbuYearlyList[i].sbuAmount;
             sbu.expense = this.bgtSbuYearlyList[i].expense;
             this.sbuDetails.push(sbu);
             const total = parseInt(this.totalBudget);
             const next = parseInt(this.bgtSbuYearlyList[i].sbuAmount);
             this.totalBudget = total+next;
          }
          debugger;
          var RemainingBudget = parseInt(this.bugetSbuYearlyService.budgetSbuYearly.totalBudget)-  parseInt(this.totalBudget)
          this.bugetSbuYearlyService.budgetSbuYearly.remainingBudget = RemainingBudget;
          this.getAllPipeLineExpenseList();
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
                this.sbuDetails[i].remaining = parseInt(this.sbuDetails[i].expense) - parseInt(this.pipelineList[j].pipeline)
              }
            }
          }
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
         if(this.sbuDetails[i].newAmount < this.sbuDetails[i].expense)
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
      if(this.totalSbuBudget > this.bugetSbuYearlyService.budgetSbuYearly.totalBudget)
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

