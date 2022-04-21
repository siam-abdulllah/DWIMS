import { DatePipe } from "@angular/common";
import { Component, ElementRef, OnInit, TemplateRef, ViewChild } from "@angular/core";
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
  constructor(private accountService: AccountService,private router: Router,public bugetyearlyservice: BudgetYearlyService,private toastr: ToastrService, private modalService: BsModalService, private datePipe: DatePipe, 
    private SpinnerService: NgxSpinnerService) { }
  ngOnInit() {
    this.getEmployeeId()
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
    this.bugetyearlyservice.budgetYearly.enteredBy = parseInt(this.empId);
  }
  confirmSubmission() {
    debugger;
    this.openSubmissionConfirmModal(this.submissionConfirmModal);
  }
  confirmSubmit() {
    this.submissionConfirmRef.hide();
    //this.submitInvestmentForm();
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
    this.bugetyearlyservice.budgetYearly = new BudgetYearly();
  }

  submitBgtYearly() {
      this.SpinnerService.show();
      this.bugetyearlyservice.submitBudgetYearly().subscribe(
        res => {
          this.bugetyearlyservice.budgetYearly = res as IBudgetYearly;
          this.toastr.success('Submitted successfully', 'Investment');
          this.resetPageLoad();
        },
        err => { 
          console.log(err); 
        }
      );
  }
}

