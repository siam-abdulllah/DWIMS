import { DatePipe } from "@angular/common";
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
  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  @ViewChild('budgetYearlySearchModal', { static: false }) budgetYearlySearchModal: TemplateRef<any>;
  @ViewChild('submissionConfirmModal', { static: false }) submissionConfirmModal: TemplateRef<any>;
  submissionConfirmRef: BsModalRef;
  BudgetYearlySearchModalRef: BsModalRef;
  bsValue: Date = new Date();
  today = new Date();
  budgetYearly: IBudgetYearly[];
  empId: string;
  userRole:string;
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
    this.getTotalExpense()
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
    debugger;

    this.SpinnerService.show();
    this.bugetYearlyService.getTotalExpense().subscribe(response => {
    
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

  }
  selectBgtYearly(selectedRecord: IBudgetYearly) {

    debugger;
    this.bugetYearlyService.budgetYearly = Object.assign({}, selectedRecord);
    
    this.BudgetYearlySearchModalRef.hide()
  }
  resetSearch() {
    this.searchText = '';
  }
  submitBgtYearly() {
    debugger;
 
  
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
}

