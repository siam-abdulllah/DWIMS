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

@Component({
  selector: 'app-sbu-wise-budget',
  templateUrl: './sbu-wise-budget.component.html'
})
export class SbuWiseBudgetComponent implements OnInit {

  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  genParams: GenericParams;
  sbuWiseBudgets: ISBUWiseBudget[];
  bsConfig: Partial<BsDatepickerConfig>;
  bsValue: Date = new Date();
  totalCount = 0;
  SBUs: ISBU[];
  numberPattern = "^[0-9]+(.[0-9]{1,10})?$";
  constructor(public sbuWiseBudgetService: SBUWiseBudgetService, private router: Router, private toastr: ToastrService) { }
  //constructor(private router: Router, private toastr: ToastrService) { }
  ngOnInit() {
    this.resetPage();
    this.getSBU();
    this.getSBUWiseBudget();
    this.bsConfig = Object.assign({}, { containerClass: 'theme-green' }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();
  }
  getSBU() {
    this.sbuWiseBudgetService.getSBU().subscribe(response => {
      this.SBUs = response as ISBU[];
    }, error => {
      console.log(error);
    });
  }
  getSBUWiseBudget() {
    this.sbuWiseBudgetService.getSBUWiseBudget().subscribe(response => {
      debugger;
      this.sbuWiseBudgets = response.data;
      this.totalCount = response.count;
    }, error => {
      console.log(error);
    });
  }

  dateCompare(form: NgForm) {
    if (this.sbuWiseBudgetService.sbuwiseBudgeFormData.fromDate != null && this.sbuWiseBudgetService.sbuwiseBudgeFormData.toDate != null) {
      if (this.sbuWiseBudgetService.sbuwiseBudgeFormData.toDate > this.sbuWiseBudgetService.sbuwiseBudgeFormData.fromDate) {
      }
      else {
        form.controls.fromDate.setValue(null);
        form.controls.toDate.setValue(null);
        this.toastr.error('Select Appropriate Date Range', 'Error')
      }
    }
  }

  onSubmit(form: NgForm) {
    debugger;
    if (this.sbuWiseBudgetService.sbuwiseBudgeFormData.id == 0)
      this.insertSBUWiseBudget(form);
    else
      this.updateSBUWiseBudget(form);
  }


  insertSBUWiseBudget(form: NgForm) {
    for (let i = 0; i < this.SBUs.length; i++) {
      if (this.SBUs[i].sbuCode === this.sbuWiseBudgetService.sbuwiseBudgeFormData.sbu) {

        this.sbuWiseBudgetService.sbuwiseBudgeFormData.sbuName = this.SBUs[i].sbuName;

        break;
      }
    }
    this.sbuWiseBudgetService.insertSBUWiseBudget().subscribe(
      res => {
        debugger;
        this.resetForm(form);
        this.getSBUWiseBudget();
        this.toastr.success('Data Saved successfully', 'SBU Wise Budget ')
      },
      err => {
        debugger;
        this.toastr.error(err.errors[0], 'SBU Wise Budget ')
        console.log(err);
      }
    );
  }

  updateSBUWiseBudget(form: NgForm) {
    for (let i = 0; i < this.SBUs.length; i++) {
      if (this.SBUs[i].sbuCode === this.sbuWiseBudgetService.sbuwiseBudgeFormData.sbu) {

        this.sbuWiseBudgetService.sbuwiseBudgeFormData.sbuName = this.SBUs[i].sbuName;

        break;
      }
    }
    this.sbuWiseBudgetService.updateSBUWiseBudget().subscribe(
      res => {
        debugger;
        this.resetForm(form);
        this.getSBUWiseBudget();
        this.toastr.info('Data Updated Successfully', 'SBU Wise Budget')
      },
      err => { console.log(err); }
    );
  }

  populateForm(selectedRecord: ISBUWiseBudget) {
    this.sbuWiseBudgetService.sbuwiseBudgeFormData = Object.assign({}, selectedRecord);
    this.sbuWiseBudgetService.sbuwiseBudgeFormData.fromDate = new Date(selectedRecord.fromDate);
    this.sbuWiseBudgetService.sbuwiseBudgeFormData.toDate = new Date(selectedRecord.toDate);

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
    this.sbuWiseBudgetService.sbuwiseBudgeFormData = new SBUWiseBudget();
  }
  resetPage() {
    this.sbuWiseBudgetService.sbuwiseBudgeFormData = new SBUWiseBudget();
  }

}
