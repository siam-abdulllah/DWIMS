import { DatePipe } from "@angular/common";
import { Component, ElementRef, OnInit, TemplateRef, ViewChild } from "@angular/core";
import { NgForm } from "@angular/forms";
import { Router } from "@angular/router";
import { BsModalService, BsModalRef } from "ngx-bootstrap/modal";
import { NgxSpinnerService } from "ngx-spinner";
import { ToastrService } from 'ngx-toastr';
import { Observable } from "rxjs/internal/Observable";
import { AccountService } from "../account/account.service";
import { IApprovalAuthority } from "../shared/models/approvalAuthority";
import { BudgetEmpSbuMap, IBudgetEmpSbuMap } from "../shared/models/BudgetEmpSbuMap";
import { BudgetYearly, IBudgetYearly } from "../shared/models/budgetyearly";
import { IEmployee } from "../shared/models/employee";
import { IEmployeeInfo } from "../shared/models/employeeInfo";
import { ISBU } from "../shared/models/sbu";
import { BudgetEmpSbuMapervice } from "../_services/BudgetEmpSbuMapService";
import { BudgetYearlyService } from "../_services/budgetYearly.service";


@Component({
  selector: 'app-bgtempsbumap',
  templateUrl: './bgtEmpSbuMap.component.html',
  styles: [
  ],
  providers: [DatePipe]
})
export class BgtEmpSbuMapComponent implements OnInit {
  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  @ViewChild('budgetYearlySearchModal', { static: false }) budgetYearlySearchModal: TemplateRef<any>;
  @ViewChild('submissionConfirmModal', { static: false }) submissionConfirmModal: TemplateRef<any>;
  submissionConfirmRef: BsModalRef;
  BudgetYearlySearchModalRef: BsModalRef;
  approvalAuthorities: IApprovalAuthority[];
  bsValue: Date = new Date();
  today = new Date();
  serials = Array.from({ length: 100 }, (_, i) => i + 1);
  budgetSbuMapList: IBudgetEmpSbuMap[];
  employees: IEmployee[];
  SBUs: ISBU[];
  empId: string;
  userRole: string;
  budgetTotalForm: NgForm;
  isValid: boolean = false;
  searchText: string;
  empSbu: Observable<IEmployeeInfo>;
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
  constructor(private accountService: AccountService, private router: Router, public budgetEmpSbuMapService: BudgetEmpSbuMapervice, private toastr: ToastrService, private modalService: BsModalService, private datePipe: DatePipe,
    private SpinnerService: NgxSpinnerService) { }
  ngOnInit() {
    // this.getEmployeeId()
    this.getEmployees();
    this.getSBU();
    this.getApprovalAuthority();
  }
  customSearchFnEmp(term: string, item: any) {
    term = term.toLocaleLowerCase();
    return item.employeeSAPCode.toLocaleLowerCase().indexOf(term) > -1 ||
      item.employeeName.toLocaleLowerCase().indexOf(term) > -1;
  }
  getEmployees() {
    this.budgetEmpSbuMapService.getEmployees().subscribe(response => {
      debugger;
      this.employees = response as IEmployee[];
      //   this.totalCount = response.count;
    }, error => {
      console.log(error);
    });
  }
  getApprovalAuthority(){
    this.budgetEmpSbuMapService.getApprovalAuthority().subscribe(response => {
      this.approvalAuthorities = response as IApprovalAuthority[];
     }, error => {
        console.log(error);
     });
  }
  getSBU() {
    this.budgetEmpSbuMapService.getSBU().subscribe(response => {
      this.SBUs = response as ISBU[];
      console.log(this.SBUs);
    }, error => {
      console.log(error);
    });
  }
  // getEmployeeId() {
  //   this.empId = this.accountService.getEmployeeId();
  //   this.userRole = this.accountService.getUserRole();
  //   if (this.userRole == 'Administrator') {
  //     this.isAdmin = true;
  //   }
  //   else {
  //     this.isAdmin = false;
  //   }
  //   this.budgetEmpSbuMapService.budgetYearly.enteredBy = parseInt(this.empId);
  // }
  confirmSubmission() {
    debugger;
    this.openSubmissionConfirmModal(this.submissionConfirmModal);
  }
  confirmSubmit() {
    this.submissionConfirmRef.hide();
    this.submitBgtSbuMap();
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
  onChangeEmployee() {
    this.budgetEmpSbuMapService.getEmpWiseData(this.budgetEmpSbuMapService.budgetSbuMap.employeeId).subscribe(response => {
      var data = response as IApprovalAuthority[];
      this.budgetEmpSbuMapService.budgetSbuMap.approvalAuthorityId= data[0].id;
      this.budgetEmpSbuMapService.getEmpSbuMappingListByEmp(this.budgetEmpSbuMapService.budgetSbuMap.employeeId,this.budgetEmpSbuMapService.budgetSbuMap.approvalAuthorityId,this.budgetEmpSbuMapService.budgetSbuMap.sbu,this.budgetEmpSbuMapService.budgetSbuMap.deptId).subscribe(response => {
        var data = response as IBudgetEmpSbuMap[];
        if (data !== undefined) {
          this.budgetSbuMapList = data;
        }
      }, error => {
        console.log(error);
      });
    }, error => {
      console.log(error);
    });
   
    
    
  }

  resetPageLoad() {
    this.budgetEmpSbuMapService.budgetSbuMap = new BudgetEmpSbuMap();
    this.budgetSbuMapList = [];
  }
  // openBudgetYearlySearchModal(template: TemplateRef<any>) {
  //   debugger;
  //   this.BudgetYearlySearchModalRef = this.modalService.show(template, this.config);
  // }
  // getBgtYearly() {
  //   debugger;
  //   const params = this.bugetYearlyService.getGenParams();
  //   this.SpinnerService.show();
  //   this.bugetYearlyService.getBudgetYearly().subscribe(response => {
  //     this.SpinnerService.hide();
  //     this.budgetYearly = response as IBudgetYearly[];
  //     if (this.budgetYearly.length > 0) {
  //       this.openBudgetYearlySearchModal(this.budgetYearlySearchModal);
  //       }
  //       else {
  //         this.toastr.warning('No Data Found');
  //       }
  //   }, error => {
  //     this.SpinnerService.hide();
  //     console.log(error);
  //   });
  // }
  // getTotalExpense() {
  //   debugger;

  //   this.SpinnerService.show();
  //   this.bugetYearlyService.getTotalExpense(this.bugetYearlyService.budgetYearly.deptId,this.bugetYearlyService.budgetYearly.year).subscribe(response => {
  //    this.bugetYearlyService.budgetYearly.totalExpense = response;
  //   }, error => {
  //     this.SpinnerService.hide();
  //     console.log(error);
  //   });
  // }
  // getTotalBudget() {
  //   debugger;

  //   this.SpinnerService.show();
  //   this.bugetYearlyService.getBudgetAmount(this.bugetYearlyService.budgetYearly.deptId,this.bugetYearlyService.budgetYearly.year).subscribe(response => {
  //    this.bugetYearlyService.budgetYearly.totalAmount = response as number;
  //   }, error => {
  //     this.SpinnerService.hide();
  //     console.log(error);
  //   });
  // }
  // LoadForm()
  // {

  //   this.getTotalExpense()
  //   this.getTotalPipeline()
  //   this.getTotalBudget()
  //   setTimeout(() => {
  //     if(parseInt(this.bugetYearlyService.budgetYearly.totalAmount)>(parseInt(this.bugetYearlyService.budgetYearly.totalExpense)+ parseInt(this.bugetYearlyService.budgetYearly.totalPipeline)))
  //     {
  //       this.bugetYearlyService.budgetYearly.totalRemaining = parseInt(this.bugetYearlyService.budgetYearly.totalAmount) -  (parseInt(this.bugetYearlyService.budgetYearly.totalExpense)+ parseInt(this.bugetYearlyService.budgetYearly.totalPipeline));
  //     }
  //     else{
  //       this.bugetYearlyService.budgetYearly.totalRemaining = 0;
  //     }

  //   }, 3000);
  // }
  // getTotalPipeline() {
  //   debugger;

  //   this.SpinnerService.show();
  //   this.bugetYearlyService.getTotalPipeline(this.bugetYearlyService.budgetYearly.deptId,this.bugetYearlyService.budgetYearly.year).subscribe(response => {
  //    this.bugetYearlyService.budgetYearly.totalPipeline = response;
  //   }, error => {
  //     this.SpinnerService.hide();
  //     console.log(error);
  //   });
  // }
  // onYearchange() {
  //   debugger;
  //   if (this.bugetYearlyService.budgetYearly.year != null && this.bugetYearlyService.budgetYearly.year  != undefined) {
  //     var year = new Date(this.bugetYearlyService.budgetYearly.year).getFullYear();
  //     this.bugetYearlyService.budgetYearly.year = year;
  //   }

  // }
  // selectBgtYearly(selectedRecord: IBudgetYearly) {

  //   debugger;
  //   this.bugetYearlyService.budgetYearly = Object.assign({}, selectedRecord);
  //   this.LoadForm()
  //   this.BudgetYearlySearchModalRef.hide()
  // }
  // resetSearch() {
  //   this.searchText = '';
  // }
  getEmpSbuMappingListByDept() {
    if (this.budgetEmpSbuMapService.budgetSbuMap.sbu == null || this.budgetEmpSbuMapService.budgetSbuMap.sbu == "" || this.budgetEmpSbuMapService.budgetSbuMap.sbu == undefined) {
      if (this.budgetEmpSbuMapService.budgetSbuMap.deptId != null && this.budgetEmpSbuMapService.budgetSbuMap.deptId != 0 && this.budgetEmpSbuMapService.budgetSbuMap.deptId != undefined) {
        this.budgetEmpSbuMapService.getEmpSbuMappingListByDept(this.budgetEmpSbuMapService.budgetSbuMap.deptId).subscribe(response => {
          var data = response as IBudgetEmpSbuMap[];
          if (data !== undefined) {
            this.budgetSbuMapList = data;
          }
        }, error => {
          console.log(error);
        });
      }
    }
    else {
      if (this.budgetEmpSbuMapService.budgetSbuMap.deptId != null && this.budgetEmpSbuMapService.budgetSbuMap.deptId != 0 && this.budgetEmpSbuMapService.budgetSbuMap.deptId != undefined) {
        this.budgetEmpSbuMapService.getEmpSbuMappingList(this.budgetEmpSbuMapService.budgetSbuMap.deptId, this.budgetEmpSbuMapService.budgetSbuMap.sbu).subscribe(response => {
          var data = response as IBudgetEmpSbuMap[];
          if (data !== undefined) {
            this.budgetSbuMapList = data;
          }
        }, error => {
          console.log(error);

        });
      }
    }
  }
  getEmpSbuMappingListBySbu() {
    if (this.budgetEmpSbuMapService.budgetSbuMap.deptId == null || this.budgetEmpSbuMapService.budgetSbuMap.deptId == 0 || this.budgetEmpSbuMapService.budgetSbuMap.deptId == undefined) {
      if (this.budgetEmpSbuMapService.budgetSbuMap.sbu != null && this.budgetEmpSbuMapService.budgetSbuMap.sbu != "" && this.budgetEmpSbuMapService.budgetSbuMap.sbu != undefined) {
        this.budgetEmpSbuMapService.getEmpSbuMappingListBySbu(this.budgetEmpSbuMapService.budgetSbuMap.sbu).subscribe(response => {
          var data = response as IBudgetEmpSbuMap[];
          if (data !== undefined) {
            this.budgetSbuMapList = data;
          }
        }, error => {
          console.log(error);
        });
      }
    }
    else {
      if (this.budgetEmpSbuMapService.budgetSbuMap.sbu != null && this.budgetEmpSbuMapService.budgetSbuMap.sbu != "" && this.budgetEmpSbuMapService.budgetSbuMap.sbu != undefined) {

        this.budgetEmpSbuMapService.getEmpSbuMappingList(this.budgetEmpSbuMapService.budgetSbuMap.deptId, this.budgetEmpSbuMapService.budgetSbuMap.sbu).subscribe(response => {
          var data = response as IBudgetEmpSbuMap[];
          if (data !== undefined) {
            this.budgetSbuMapList = data;
          }
        }, error => {
          console.log(error);

        });
      }
    }
  }
  removeSbuMapping(selectedRecord: IBudgetEmpSbuMap) {
    debugger;
    var c = confirm("Are you sure you want to delete that?");
    if (c == true) {

      this.SpinnerService.show();
      debugger;
      this.budgetEmpSbuMapService.removeEmpSbuMapping(selectedRecord).subscribe(
        res => {
          this.SpinnerService.hide();
          this.toastr.success(res);
          this.getEmpSbuMappingListByDept();
        },
        err => {
          this.SpinnerService.hide();
          console.log(err);
        }
      );
    }
  }
  submitBgtSbuMap() {
    debugger;

    for (let i = 0; i < this.SBUs.length; i++) {
      debugger;
      if (this.SBUs[i].sbuCode == this.budgetEmpSbuMapService.budgetSbuMap.sbu) {
        this.budgetEmpSbuMapService.budgetSbuMap.sbuName = this.SBUs[i].sbuName;
        break;
      }
    }
    this.SpinnerService.show();
    this.budgetEmpSbuMapService.SaveEmpSbuMapping().subscribe(
      res => {
        this.budgetEmpSbuMapService.budgetSbuMap = res as IBudgetEmpSbuMap;
        if (this.budgetEmpSbuMapService.budgetSbuMap.id > 0) {
          this.toastr.success('Submitted successfully', 'Sbu Map');

        }
        else {
          this.toastr.warning('Deta already exist!', 'Sbu Map');
        }
        this.resetPageLoad();
      },
      err => {
        console.log(err);
      }
    );
  }
}

