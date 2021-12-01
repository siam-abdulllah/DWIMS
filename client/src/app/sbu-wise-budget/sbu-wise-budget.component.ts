import { ISBUWiseBudgetPagination } from './../shared/models/sbuWiseBudgetPagination';
import { SBUWiseBudgetService } from './../_services/sbu-wise-budget.service';
import { GenericParams } from './../shared/models/genericParams';
import { SBUWiseBudget, ISBUWiseBudget } from './../shared/models/sbuWiseBudget';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ISBU } from '../shared/models/sbu';
import { IDonation } from '../shared/models/donation';
import { DatePipe } from '@angular/common';
import { NgxSpinnerService } from "ngx-spinner";
import { IYearlyBudget, YearlyBudget } from '../shared/models/yearlyBudget';
@Component({
  selector: 'app-sbu-wise-budget',
  templateUrl: './sbu-wise-budget.component.html'
})
export class SbuWiseBudgetComponent implements OnInit {

  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  genParams: GenericParams;
  sbuWiseBudgets: ISBUWiseBudget[];
  donations: IDonation[];
  bsConfig: Partial<BsDatepickerConfig>;
  bsValue: Date = new Date();
  totalCount = 0;
  searchText = '';
  config: any;
  remainingBudget: number;
  SBUs: ISBU[];
  numberPattern = "^[0-9]+(.[0-9]{1,10})?$";
  constructor(public sbuWiseBudgetService: SBUWiseBudgetService, private router: Router, private toastr: ToastrService,
    private datePipe: DatePipe, private SpinnerService: NgxSpinnerService) { }
  //constructor(private router: Router, private toastr: ToastrService) { }
  ngOnInit() {
    this.resetPage();
    this.getSBU();
    this.getSBUWiseBudget();
    this.getDonation();
    this.bsConfig = Object.assign({}, { containerClass: 'theme-blue' }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();
  }
  getSBU() {
    this.sbuWiseBudgetService.getSBU().subscribe(response => {
      this.SBUs = response as ISBU[];
    }, error => {
      console.log(error);
    });
  }
  onYearchange() {
    if (this.sbuWiseBudgetService.yearlyBudgetForm.year != null && this.sbuWiseBudgetService.yearlyBudgetForm.year != "" && this.sbuWiseBudgetService.yearlyBudgetForm.year != undefined) {
      var year = new Date(this.sbuWiseBudgetService.yearlyBudgetForm.year).getFullYear();
      this.sbuWiseBudgetService.getYearlyTotalAmount(year).subscribe(response => {
        ////debugger;
        if (response != null) {
          this.sbuWiseBudgetService.yearlyBudgetForm = response as IYearlyBudget;
        }
        if (this.sbuWiseBudgetService.yearlyBudgetForm.amount != null && this.sbuWiseBudgetService.yearlyBudgetForm.amount != 0 && this.sbuWiseBudgetService.yearlyBudgetForm.amount != undefined) {

          var totalExpense = "";
          var year = 0;
          if (typeof (this.sbuWiseBudgetService.yearlyBudgetForm.year) !== 'string') {
            year = new Date(this.sbuWiseBudgetService.yearlyBudgetForm.year).getFullYear();
          }
          else {
            year = parseInt(this.sbuWiseBudgetService.yearlyBudgetForm.year);
          }
          this.sbuWiseBudgetService.getYearlyTotalExpense(year).subscribe(response => {
            //debugger;
            if (response != null) {
              totalExpense = response as string;
              this.remainingBudget = this.sbuWiseBudgetService.yearlyBudgetForm.amount - parseInt(totalExpense);
              this.onTotalAmountchange();
            }
          }, error => {
            console.log(error);
          });
        }
      }, error => {
        console.log(error);
      });
    }

  }
  getTotalBudgetByYear(year: number) {
    this.sbuWiseBudgetService.getYearlyTotalAmount(year).subscribe(response => {
      //debugger;
      if (response != null) {
        this.sbuWiseBudgetService.yearlyBudgetForm = response as IYearlyBudget;
        if (this.sbuWiseBudgetService.yearlyBudgetForm.amount != null && this.sbuWiseBudgetService.yearlyBudgetForm.amount != 0 && this.sbuWiseBudgetService.yearlyBudgetForm.amount != undefined && this.sbuWiseBudgets != null) {
          var totalExpense = "";
          // this.sbuWiseBudgets.forEach(element => {
          //   if (element.year == this.sbuWiseBudgetService.yearlyBudgetForm.year) {
          //     totalExpense = totalExpense + element.amount;
          //   }
          // });
          // this.remainingBudget = this.sbuWiseBudgetService.yearlyBudgetForm.amount - totalExpense;
          this.sbuWiseBudgetService.getYearlyTotalExpense(this.sbuWiseBudgetService.yearlyBudgetForm.year).subscribe(response => {
            //debugger;
            if (response != null) {
              totalExpense = response as string;
              this.remainingBudget = this.sbuWiseBudgetService.yearlyBudgetForm.amount - parseInt(totalExpense);
            }
          }, error => {
            console.log(error);
          });
        }
      }
    }, error => {
      console.log(error);
    });
  }
  onTotalAmountchange() {
    if (this.sbuWiseBudgetService.yearlyBudgetForm.amount != null && this.sbuWiseBudgetService.yearlyBudgetForm.amount != 0 && this.sbuWiseBudgetService.yearlyBudgetForm.amount != undefined
      && this.sbuWiseBudgetService.yearlyBudgetForm.year != null && this.sbuWiseBudgetService.yearlyBudgetForm.year != 0 && this.sbuWiseBudgetService.yearlyBudgetForm.year != undefined) {
      var totalExpense = "";
      var year = 0;
      //debugger;
      //if(this.sbuWiseBudgetService.yearlyBudgetForm.year.length>3)
      if (typeof (this.sbuWiseBudgetService.yearlyBudgetForm.year) !== 'string') {
        year = new Date(this.sbuWiseBudgetService.yearlyBudgetForm.year).getFullYear();
      }
      else {
        year = parseInt(this.sbuWiseBudgetService.yearlyBudgetForm.year);
      }
      this.sbuWiseBudgetService.getYearlyTotalExpense(year).subscribe(response => {
        //debugger;
        if (response != null) {
          totalExpense = response as string;
          this.remainingBudget = this.sbuWiseBudgetService.yearlyBudgetForm.amount - parseInt(totalExpense);
          if (this.sbuWiseBudgetService.yearlyBudgetForm.amount != null && this.sbuWiseBudgetService.yearlyBudgetForm.amount != 0
            && this.sbuWiseBudgetService.yearlyBudgetForm.amount != undefined) {
            this.onAmountchange();
          }
        }
      }, error => {
        console.log(error);
      });


      //   this.sbuWiseBudgets.forEach(element => {
      //     if (element.year == this.sbuWiseBudgetService.yearlyBudgetForm.year) {
      //       totalExpense = totalExpense + element.amount;
      //     }
      //   });
      //   if (totalExpense != 0) {
      //     this.remainingBudget = this.sbuWiseBudgetService.yearlyBudgetForm.amount - totalExpense;
      //   }
      //   else {
      //     this.remainingBudget = this.sbuWiseBudgetService.yearlyBudgetForm.amount;
      //   }
    }
  }
  onAmountchange() {
    if (this.sbuWiseBudgetService.yearlyBudgetForm.amount != null && this.sbuWiseBudgetService.yearlyBudgetForm.amount != 0
      && this.sbuWiseBudgetService.yearlyBudgetForm.amount != undefined) {
      if (this.sbuWiseBudgetService.sbuwiseBudgetFormData.amount != null && this.sbuWiseBudgetService.sbuwiseBudgetFormData.amount != 0
        && this.sbuWiseBudgetService.sbuwiseBudgetFormData.amount != undefined) {
        if (this.remainingBudget < this.sbuWiseBudgetService.sbuwiseBudgetFormData.amount) {
          this.toastr.warning("Total budget exceeded");
          this.sbuWiseBudgetService.sbuwiseBudgetFormData.amount = 0;
          return false;
        }
      }
      // else {
      //   this.sbuWiseBudgetService.sbuwiseBudgetFormData.amount=0;
      //   this.toastr.warning("Please Enter SBU wise Budget");
      //   return false;
      // }
    }
    else {
      //this.sbuWiseBudgetService.sbuwiseBudgetFormData.amount=0;
      this.toastr.warning("Please Enter Total Budget");
      return false;
    }
  }
  getSBUWiseBudget() {
    this.sbuWiseBudgetService.getSBUWiseBudget().subscribe(response => {
      const params = this.sbuWiseBudgetService.getGenParams();
      // this.sbuWiseBudgets = response.data;
      //debugger;
      this.sbuWiseBudgets = response.data;
      this.sbuWiseBudgets.forEach(element => {
        var convertedDate = new Date(element.toDate);
        element.year = convertedDate.getFullYear();
      });
      this.totalCount = response.count;
      this.config = {
        currentPage: params.pageIndex,
        itemsPerPage: params.pageSize,
        totalItems: this.totalCount,
      };
    }, error => {
      console.log(error);
    });
  }

  dateCompare() {
    var year=0;
    if (typeof (this.sbuWiseBudgetService.yearlyBudgetForm.year) !== 'string') {
      year = new Date(this.sbuWiseBudgetService.yearlyBudgetForm.year).getFullYear();
    }
    else {
      year = parseInt(this.sbuWiseBudgetService.yearlyBudgetForm.year);
    }
    if (this.sbuWiseBudgetService.sbuwiseBudgetFormData.fromDate != null && this.sbuWiseBudgetService.sbuwiseBudgetFormData.toDate != null) {
      if (year != new Date(this.sbuWiseBudgetService.sbuwiseBudgetFormData.fromDate).getFullYear()) {
        this.toastr.warning('Select Appropriate Date Range');
        return false;
      }
      if (year != new Date(this.sbuWiseBudgetService.sbuwiseBudgetFormData.toDate).getFullYear()) {
        this.toastr.warning('Select Appropriate Date Range');
        return false;
      }
      if (this.sbuWiseBudgetService.sbuwiseBudgetFormData.toDate > this.sbuWiseBudgetService.sbuwiseBudgetFormData.fromDate) {
        return true;
      }
      else {
        //form.controls.fromDate.setValue(null);
        //form.controls.toDate.setValue(null);
        this.toastr.error('Select Appropriate Date Range', 'Error');
        return false;
      }
    }
  }
  getDonation() {
    this.sbuWiseBudgetService.getDonations().subscribe(response => {
      this.donations = response as IDonation[];
    }, error => {
      console.log(error);
    });
  }

  onPageChanged(event: any) {
    const params = this.sbuWiseBudgetService.getGenParams();
    if (params.pageIndex !== event) {
      params.pageIndex = event;
      this.sbuWiseBudgetService.setGenParams(params);
      this.getSBUWiseBudget();
    }
  }

  onSearch() {
    const params = this.sbuWiseBudgetService.getGenParams();
    params.search = this.searchTerm.nativeElement.value;
    params.pageIndex = 1;
    this.sbuWiseBudgetService.setGenParams(params);
    this.getSBUWiseBudget();
  }

  resetSearch() {
    this.searchText = '';
  }

  onSubmit(form: NgForm) {
    this.sbuWiseBudgetService.sbuwiseBudgetFormData.fromDate = this.datePipe.transform(this.sbuWiseBudgetService.sbuwiseBudgetFormData.fromDate, 'yyyy-MM-dd HH:mm:ss');
    this.sbuWiseBudgetService.sbuwiseBudgetFormData.toDate = this.datePipe.transform(this.sbuWiseBudgetService.sbuwiseBudgetFormData.toDate, 'yyyy-MM-dd HH:mm:ss');
    if (this.sbuWiseBudgetService.sbuwiseBudgetFormData.id == 0)
      this.insertSBUWiseBudget(form);
    else
      this.updateSBUWiseBudget(form);
  }


  insertSBUWiseBudget(form: NgForm) {
    if (this.sbuWiseBudgetService.yearlyBudgetForm.year == null || this.sbuWiseBudgetService.yearlyBudgetForm.year == "" || this.sbuWiseBudgetService.yearlyBudgetForm.year == undefined) {
      this.toastr.warning('Please Select Year First');
      return false;
    }
    if (this.sbuWiseBudgetService.yearlyBudgetForm.amount == null || this.sbuWiseBudgetService.yearlyBudgetForm.amount == 0 || this.sbuWiseBudgetService.yearlyBudgetForm.amount == undefined) {
      this.toastr.warning('Please enter amount first');
      return false;
    }
    if (this.sbuWiseBudgetService.sbuwiseBudgetFormData.amount == 0) {
      this.toastr.warning('SBU wise budget must be greater than zero');
      return false;
    }
    for (let i = 0; i < this.SBUs.length; i++) {
      if (this.SBUs[i].sbuCode === this.sbuWiseBudgetService.sbuwiseBudgetFormData.sbu) {
        this.sbuWiseBudgetService.sbuwiseBudgetFormData.sbuName = this.SBUs[i].sbuName;
        break;
      }
    }
    if (this.dateCompare()) {
      var year=0;
    if (typeof (this.sbuWiseBudgetService.yearlyBudgetForm.year) !== 'string') {
      year = new Date(this.sbuWiseBudgetService.yearlyBudgetForm.year).getFullYear();
    }
    else {
      year = parseInt(this.sbuWiseBudgetService.yearlyBudgetForm.year);
    }
      this.SpinnerService.show();
      this.sbuWiseBudgetService.insertSBUWiseBudget(year, this.sbuWiseBudgetService.yearlyBudgetForm.amount).subscribe(
        res => {
          this.resetForm(form);
          this.getSBUWiseBudget();
          this.SpinnerService.hide();
          this.toastr.success('Data Saved successfully', 'SBU Wise Budget')
        },
        err => {
          this.sbuWiseBudgetService.sbuwiseBudgetFormData.fromDate = new Date(this.sbuWiseBudgetService.sbuwiseBudgetFormData.fromDate);
          this.sbuWiseBudgetService.sbuwiseBudgetFormData.toDate = new Date(this.sbuWiseBudgetService.sbuwiseBudgetFormData.toDate);
          this.SpinnerService.hide();
          this.toastr.error(err.errors[0], 'SBU Wise Budget')
          console.log(err);
        }
      );
    }
  }

  updateSBUWiseBudget(form: NgForm) {
    if (this.sbuWiseBudgetService.yearlyBudgetForm.year == null || this.sbuWiseBudgetService.yearlyBudgetForm.year == "" || this.sbuWiseBudgetService.yearlyBudgetForm.year == undefined) {
      this.toastr.warning('Please Select Year First');
      return false;
    }
    if (this.sbuWiseBudgetService.yearlyBudgetForm.amount == null || this.sbuWiseBudgetService.yearlyBudgetForm.amount == 0 || this.sbuWiseBudgetService.yearlyBudgetForm.amount == undefined) {
      this.toastr.warning('Please enter amount first');
      return false;
    }
    if (this.sbuWiseBudgetService.sbuwiseBudgetFormData.amount == 0) {
      this.toastr.warning('SBU wise budget must be greater than zero');
      return false;
    }
    for (let i = 0; i < this.SBUs.length; i++) {
      if (this.SBUs[i].sbuCode === this.sbuWiseBudgetService.sbuwiseBudgetFormData.sbu) {
        this.sbuWiseBudgetService.sbuwiseBudgetFormData.sbuName = this.SBUs[i].sbuName;
        break;
      }
    }
    if (this.dateCompare()) {
      var year=0;
    if (typeof (this.sbuWiseBudgetService.yearlyBudgetForm.year) !== 'string') {
      year = new Date(this.sbuWiseBudgetService.yearlyBudgetForm.year).getFullYear();
    }
    else {
      year = parseInt(this.sbuWiseBudgetService.yearlyBudgetForm.year);
    }
      this.SpinnerService.show();
      this.sbuWiseBudgetService.updateSBUWiseBudget(year, this.sbuWiseBudgetService.yearlyBudgetForm.amount).subscribe(
        res => {
          this.resetForm(form);
          this.getSBUWiseBudget();
          this.SpinnerService.hide();
          this.toastr.info('Data Updated Successfully', 'SBU Wise Budget')
        },
        err => {
          this.sbuWiseBudgetService.sbuwiseBudgetFormData.fromDate = new Date(this.sbuWiseBudgetService.sbuwiseBudgetFormData.fromDate);
          this.sbuWiseBudgetService.sbuwiseBudgetFormData.toDate = new Date(this.sbuWiseBudgetService.sbuwiseBudgetFormData.toDate);
          this.SpinnerService.hide();
          this.toastr.error(err.errors[0], 'SBU Wise Budget')
          console.log(err);
        }
      );
    }
  }

  populateForm(selectedRecord: ISBUWiseBudget) {
    this.sbuWiseBudgetService.sbuwiseBudgetFormData = Object.assign({}, selectedRecord);
    this.sbuWiseBudgetService.sbuwiseBudgetFormData.fromDate = new Date(selectedRecord.fromDate);
    this.sbuWiseBudgetService.sbuwiseBudgetFormData.toDate = new Date(selectedRecord.toDate);
    this.sbuWiseBudgetService.yearlyBudgetForm.year = this.sbuWiseBudgetService.sbuwiseBudgetFormData.toDate.getFullYear();
    this.getTotalBudgetByYear(this.sbuWiseBudgetService.sbuwiseBudgetFormData.toDate.getFullYear());
  }

  remove(selectedRecord: ISBUWiseBudget) {
    var result = confirm("Do you want to delete?");
    if (result) {
      this.sbuWiseBudgetService.removeSBUWiseBudget(selectedRecord).subscribe(
        res => {
          this.getSBUWiseBudget();
          this.toastr.success(res, 'SBU Wise Budget')
        },
        err => { console.log(err); }
      );
    }
  }

  resetForm(form: NgForm) {
    form.form.reset();
    this.sbuWiseBudgetService.sbuwiseBudgetFormData = new SBUWiseBudget();
    this.sbuWiseBudgetService.yearlyBudgetForm = new YearlyBudget();
  }

  resetPage() {
    this.sbuWiseBudgetService.sbuwiseBudgetFormData = new SBUWiseBudget();
    this.sbuWiseBudgetService.yearlyBudgetForm = new YearlyBudget();
    this.config = {
      currentPage: 1,
      itemsPerPage: 10,
      totalItems: 50,
    };
  }

}
