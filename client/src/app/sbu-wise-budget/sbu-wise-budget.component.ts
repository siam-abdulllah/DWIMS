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
  SBUs: ISBU[];
  numberPattern = "^[0-9]+(.[0-9]{1,10})?$";
  constructor(public sbuWiseBudgetService: SBUWiseBudgetService, private router: Router, private toastr: ToastrService,
    private datePipe: DatePipe,private SpinnerService: NgxSpinnerService) { }
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
  getSBUWiseBudget() {
    this.sbuWiseBudgetService.getSBUWiseBudget().subscribe(response => {
      const params = this.sbuWiseBudgetService.getGenParams();
      this.sbuWiseBudgets = response.data;
      this.totalCount = response.count;
      this.config = {
        currentPage: params.pageIndex,
        itemsPerPage: params.pageSize,
        totalItems:this.totalCount,
        };
    }, error => {
      console.log(error);
    });
  }

  dateCompare() {
    if (this.sbuWiseBudgetService.sbuwiseBudgeFormData.fromDate != null && this.sbuWiseBudgetService.sbuwiseBudgeFormData.toDate != null) {
      if (this.sbuWiseBudgetService.sbuwiseBudgeFormData.toDate > this.sbuWiseBudgetService.sbuwiseBudgeFormData.fromDate) {
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
  getDonation(){
    this.sbuWiseBudgetService.getDonations().subscribe(response => {
      this.donations = response as IDonation[];
    }, error => {
        console.log(error);
    });
  }

  onPageChanged(event: any){
    const params = this.sbuWiseBudgetService.getGenParams();
    if (params.pageIndex !== event)
    {
      params.pageIndex = event;
      this.sbuWiseBudgetService.setGenParams(params);
      this.getSBUWiseBudget();
    }
  }
  
  onSearch(){
    const params = this.sbuWiseBudgetService.getGenParams();
    params.search = this.searchTerm.nativeElement.value;
    params.pageIndex = 1;
    this.sbuWiseBudgetService.setGenParams(params);
    this.getSBUWiseBudget();
  }

  resetSearch(){
    this.searchText = '';
}

  onSubmit(form: NgForm) {
    this.sbuWiseBudgetService.sbuwiseBudgeFormData.fromDate = this.datePipe.transform(this.sbuWiseBudgetService.sbuwiseBudgeFormData.fromDate, 'yyyy-MM-dd HH:mm:ss');
    this.sbuWiseBudgetService.sbuwiseBudgeFormData.toDate = this.datePipe.transform(this.sbuWiseBudgetService.sbuwiseBudgeFormData.toDate, 'yyyy-MM-dd HH:mm:ss');
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
    if(this.dateCompare()){
      this.SpinnerService.show();  
    this.sbuWiseBudgetService.insertSBUWiseBudget().subscribe(
      res => {
        this.resetForm(form);
        this.getSBUWiseBudget();
        this.SpinnerService.hide();  
        this.toastr.success('Data Saved successfully', 'SBU Wise Budget')
      },
      err => {
        this.sbuWiseBudgetService.sbuwiseBudgeFormData.fromDate = new Date(this.sbuWiseBudgetService.sbuwiseBudgeFormData.fromDate);
        this.sbuWiseBudgetService.sbuwiseBudgeFormData.toDate = new Date(this.sbuWiseBudgetService.sbuwiseBudgeFormData.toDate);
        this.SpinnerService.hide();  
        this.toastr.error(err.errors[0], 'SBU Wise Budget')
        console.log(err);
      }
    );
  }
  }

  updateSBUWiseBudget(form: NgForm) {
    for (let i = 0; i < this.SBUs.length; i++) {
      if (this.SBUs[i].sbuCode === this.sbuWiseBudgetService.sbuwiseBudgeFormData.sbu) {

        this.sbuWiseBudgetService.sbuwiseBudgeFormData.sbuName = this.SBUs[i].sbuName;

        break;
      }
    }
    if(this.dateCompare()){
      this.SpinnerService.show();  
    this.sbuWiseBudgetService.updateSBUWiseBudget().subscribe(
      res => {
        this.resetForm(form);
        this.getSBUWiseBudget();
        this.SpinnerService.hide();  
        this.toastr.info('Data Updated Successfully', 'SBU Wise Budget')
      },
      err => { 
        this.sbuWiseBudgetService.sbuwiseBudgeFormData.fromDate = new Date(this.sbuWiseBudgetService.sbuwiseBudgeFormData.fromDate);
        this.sbuWiseBudgetService.sbuwiseBudgeFormData.toDate = new Date(this.sbuWiseBudgetService.sbuwiseBudgeFormData.toDate);
        this.SpinnerService.hide(); 
        this.toastr.error(err.errors[0], 'SBU Wise Budget ')
      console.log(err); 
    }
    );}
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
    this.config = {
      currentPage: 1,
      itemsPerPage: 10,
      totalItems:50,
      };
  }

}
