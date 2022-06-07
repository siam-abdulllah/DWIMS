import { DatePipe } from "@angular/common";
import { Byte } from "@angular/compiler/src/util";
import { Component, ElementRef, OnInit, TemplateRef, ViewChild } from "@angular/core";
import { NgForm } from "@angular/forms";
import { Router } from "@angular/router";
import { BsModalService,BsModalRef} from "ngx-bootstrap/modal";
import { NgxSpinnerService } from "ngx-spinner";
import { ToastrService } from 'ngx-toastr';
import { Observable } from "rxjs/internal/Observable";
import { AccountService } from "../account/account.service";
import { BudgetYearly, IBudgetYearly } from "../shared/models/budgetyearly";
import { IEmployeeInfo } from "../shared/models/employeeInfo";
import { BudgetYearlyService } from "../_services/budgetYearly.service";


@Component({
  selector: 'app-bgtyearly',
  templateUrl: './bgtyearly.component.html',
  styles: [
  ],
  providers: [DatePipe]
})
export class BgtYearlyComponent implements OnInit {
  [x: string]: any;
  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  @ViewChild('budgetYearlySearchModal', { static: false }) budgetYearlySearchModal: TemplateRef<any>;
  @ViewChild('submissionConfirmModal', { static: false }) submissionConfirmModal: TemplateRef<any>;
  submissionConfirmRef: BsModalRef;
  BudgetYearlySearchModalRef: BsModalRef;
  bsValue: Date = new Date();
  response: {dbPath: ''};
  today = new Date();
  addAmountShow:boolean=false;
  newAmountShow:boolean=true;
  budgetYearly: IBudgetYearly[];
  empId: string;
  deptId: any;
  userRole:string;
  UserPhoto:any;
  budgetTotalForm: NgForm;
  isValid: boolean = false;
  searchText:string;
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
  constructor(private accountService: AccountService,private router: Router,public bugetYearlyService: BudgetYearlyService,private toastr: ToastrService, private modalService: BsModalService, private datePipe: DatePipe, 
    private SpinnerService: NgxSpinnerService) { }
  ngOnInit() {
    this.getEmployeeId()

  }
  isAddChecked(){
     this.addAmountShow = true;
     this.newAmountShow = false;
     this.bugetYearlyService.budgetYearly.newAmount = 0;
  }
  isNewChecked(){
    this.addAmountShow = false;
    this.newAmountShow = true;
    this.bugetYearlyService.budgetYearly.addAmount = 0;
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
    this.bugetYearlyService.budgetYearly.enteredBy = parseInt(this.empId);
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
    this.bugetYearlyService.budgetYearly = new BudgetYearly();
  }
  openBudgetYearlySearchModal(template: TemplateRef<any>) {
    debugger;
    this.BudgetYearlySearchModalRef = this.modalService.show(template, this.config);
  }
  getBgtYearly() {
    debugger;
    const params = this.bugetYearlyService.getGenParams();
    this.SpinnerService.show();
    this.bugetYearlyService.getBudgetYearly().subscribe(response => {
      this.SpinnerService.hide();
      this.budgetYearly = response as IBudgetYearly[];
      if (this.budgetYearly.length > 0) {
        this.openBudgetYearlySearchModal(this.budgetYearlySearchModal);
        }
        else {
          this.toastr.warning('No Data Found');
        }
    }, error => {
      this.SpinnerService.hide();
      console.log(error);
    });
  }
  getTotalExpense() {
    //this.SpinnerService.show();
    this.bugetYearlyService.getTotalExpense(this.bugetYearlyService.budgetYearly.deptId,this.bugetYearlyService.budgetYearly.year).subscribe(response => {
     this.bugetYearlyService.budgetYearly.totalExpense = response;
     this.getTotalPipeline();
    }, error => {
      this.SpinnerService.hide();
      console.log(error);
    });
  }
  getTotalBudget() {
   // this.SpinnerService.show();
    this.bugetYearlyService.getBudgetAmount(this.bugetYearlyService.budgetYearly.deptId,this.bugetYearlyService.budgetYearly.year).subscribe(response => {
     this.bugetYearlyService.budgetYearly.totalAmount = response as number;
     //this.bugetYearlyService.budgetYearly.totalRemaining = parseInt(this.bugetYearlyService.budgetYearly.totalAmount) -   parseInt(this.bugetYearlyService.budgetYearly.totalExpense);
     this.getTotalAllocated();
     //this.SpinnerService.hide();
    }, error => {
      this.SpinnerService.hide();
      console.log(error);
    });
  }
  getTotalAllocated(){
    this.bugetYearlyService.getTotalAllocated(this.bugetYearlyService.budgetYearly.deptId,this.bugetYearlyService.budgetYearly.year).subscribe(
      res => {
        this.bugetYearlyService.budgetYearly.totalAllocated = res as number;
        this.bugetYearlyService.budgetYearly.totalRemaining = parseInt(this.bugetYearlyService.budgetYearly.totalAmount) - parseInt(this.bugetYearlyService.budgetYearly.totalAllocated);
        this.SpinnerService.hide();
      },
      err => { console.log(err); }
    );
  }
  // getTotalAllocated() {
  //  // this.SpinnerService.show();
  //   this.bugetYearlyService.getTotalAllocated(this.bugetYearlyService.budgetYearly.deptId,this.bugetYearlyService.budgetYearly.year).subscribe(response => {
  //   //   this.bugetYearlyService.budgetYearly.totalAllocated = response as number;
  //   //  this.bugetYearlyService.budgetYearly.totalRemaining = parseInt(this.bugetYearlyService.budgetYearly.totalAmount) -   parseInt(this.bugetYearlyService.budgetYearly.totalExpense);
  //   //  this.SpinnerService.hide();
  //   }, error => {
  //     this.SpinnerService.hide();
  //     console.log(error);
  //   });
  // }
  LoadForm()
  {
    this.SpinnerService.show();
    this.getTotalExpense();
    //this.getTotalPipeline();
    //this.getTotalBudget();
    //this.SpinnerService.show();
    //setTimeout(() => {
      //this.SpinnerService.show();
        //this.bugetYearlyService.budgetYearly.totalRemaining = parseInt(this.bugetYearlyService.budgetYearly.totalAmount) -  (parseInt(this.bugetYearlyService.budgetYearly.totalExpense)+ parseInt(this.bugetYearlyService.budgetYearly.totalPipeline));
        //this.bugetYearlyService.budgetYearly.totalRemaining = parseInt(this.bugetYearlyService.budgetYearly.totalAmount) -   parseInt(this.bugetYearlyService.budgetYearly.totalExpense);
        //this.SpinnerService.hide();
    
    //}, 3000);
    
  }
  getTotalPipeline() {
    //this.SpinnerService.show();
    this.bugetYearlyService.getTotalPipeline(this.bugetYearlyService.budgetYearly.deptId,this.bugetYearlyService.budgetYearly.year).subscribe(response => {
     this.bugetYearlyService.budgetYearly.totalPipeline = response;
     this.getTotalBudget();
     //this.SpinnerService.hide();
    }, error => {
     this.SpinnerService.hide();
      console.log(error);
    });
  }
  onYearchange() {
    debugger;
    if (this.bugetYearlyService.budgetYearly.year != null && this.bugetYearlyService.budgetYearly.year  != undefined) {
      var year = new Date(this.bugetYearlyService.budgetYearly.year).getFullYear();
      this.bugetYearlyService.budgetYearly.year = year;
    }
    this.LoadForm();

  }
  selectBgtYearly(selectedRecord: IBudgetYearly) {

    debugger;
    this.bugetYearlyService.budgetYearly = Object.assign({}, selectedRecord);
    this.LoadForm()
    this.BudgetYearlySearchModalRef.hide()
  }
  resetSearch() {
    this.searchText = '';
  }
  submitBgtYearly() {
    debugger;
    var newAmount = this.bugetYearlyService.budgetYearly.newAmount;
    var totalExpense = parseInt(this.bugetYearlyService.budgetYearly.totalExpense);
    var totalPipeline = parseInt(this.bugetYearlyService.budgetYearly.totalPipeline);
    var addAmount = parseInt(this.bugetYearlyService.budgetYearly.addAmount);
    var totalAmount = parseInt(this.bugetYearlyService.budgetYearly.totalAmount);
    if(newAmount != '' && newAmount >0)
    {
      if((newAmount > totalExpense) && (newAmount >=  this.bugetYearlyService.budgetYearly.totalAllocated))
      {
        this.SpinnerService.show();
        this.bugetYearlyService.submitBudgetYearly().subscribe(
          res => {
            this.bugetYearlyService.budgetYearly = res as IBudgetYearly;
            this.toastr.success('Submitted successfully', 'Budget');
            this.resetPageLoad();
          },
          err => { 
            console.log(err); 
          }
        );
      }
      else{
        this.toastr.warning('Budget can not be lower then expense or allocated amount!');
        this.SpinnerService.hide();
        return;
      }
    }
    else if(addAmount>0)
    {
      if((totalAmount+addAmount  > totalExpense) && (totalAmount+addAmount  >=  this.bugetYearlyService.budgetYearly.totalAllocated))
      {
        this.SpinnerService.show();
        this.bugetYearlyService.submitBudgetYearly().subscribe(
          res => {
            this.bugetYearlyService.budgetYearly = res as IBudgetYearly;
            this.toastr.success('Submitted successfully', 'Budget');
            this.resetPageLoad();
          },
          err => { 
            console.log(err); 
          }
        );
      }
      else{
        this.toastr.warning('Budget can not be lower then expense or allocated amount!!');
        this.SpinnerService.hide();
        return;
      }
    }
 
  
      
  }

  uploadPhoto(event: any)
  {

   debugger;

    var file=event.target.files[0];
    const formData:FormData=new FormData();
    var filePath = "";
    if(this.bugetYearlyService.budgetYearly.compId == 1000)
    {
        filePath = "Square Pharmaceuticals Limited/"
    }
    if(this.bugetYearlyService.budgetYearly.deptId == 1)
    {
        filePath += "Sales/"
    }
    if(this.bugetYearlyService.budgetYearly.deptId == 2)
    {
        filePath += "PMD/"
    }
    filePath += this.bugetYearlyService.budgetYearly.year+"/";
    formData.append(filePath,file,file.Name);
    debugger;
    this.bugetYearlyService.uploadBudgetYearlyFile(formData).subscribe(
      res => {
       debugger;
        this.UserPhoto = res as Byte[];
      },
      err => { 
        console.log(err); 
      }
    );
  }
  uploadFinished = (event) => { 
    this.response = event; 
  }


}

