import { ISBUWiseBudgetPagination } from './../shared/models/sbuWiseBudgetPagination';
import { SBUWiseBudgetService } from './../_services/sbu-wise-budget.service';
import { GenericParams } from './../shared/models/genericParams';
import { SBUWiseBudget, ISBUWiseBudget }  from './../shared/models/sbuWiseBudget';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService} from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-sbu-wise-budget',
  templateUrl: './sbu-wise-budget.component.html',
  styleUrls: ['./sbu-wise-budget.component.scss']
})
export class SbuWiseBudgetComponent implements OnInit {

  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  genParams: GenericParams;
  sbuWiseBudget: ISBUWiseBudget[];
  bsConfig: Partial<BsDatepickerConfig>;
  bsValue: Date = new Date();
  totalCount = 0;
  constructor(public sbuService: SBUWiseBudgetService, private router: Router, private toastr: ToastrService) { }
  //constructor(private router: Router, private toastr: ToastrService) { }
  ngOnInit() {
    // this.getBcds();
  }

  getSBUWiseBudget(){
    this.sbuService.getSBUWiseBudget().subscribe(response => {
      debugger;
      this.sbuWiseBudget = response.data;
      this.totalCount = response.count;
    }, error => {
        console.log(error);
    });
  }

  onSubmit(form: NgForm) {
    debugger;
    if (this.sbuService.sbuwiseBudgeFormData.id == 0)
      this.insertSBUWiseBudget(form);
    else
      this.updateSBUWiseBudget(form);
  }


  insertSBUWiseBudget(form: NgForm) {
    this.sbuService.insertSBUWiseBudget().subscribe(
      res => {
        debugger;
        this.resetForm(form);
        //this.getSBUWiseBudget();
        this.toastr.success('Data Saved successfully', 'BCDS Information')
      },
      err => { console.log(err); }
    );
  }

  updateSBUWiseBudget(form: NgForm) {
    this.sbuService.updateSBUWiseBudget().subscribe(
      res => {
        debugger;
        this.resetForm(form);
        //this.getSBUWiseBudget();
        this.toastr.info('Data Updated Successfully', 'SBU Wise Budget')
      },
      err => { console.log(err); }
    );
  }

  populateForm(selectedRecord: ISBUWiseBudget) {
    this.sbuService.sbuwiseBudgeFormData = Object.assign({}, selectedRecord);
  }

  resetForm(form: NgForm) {
    form.form.reset();
    this.sbuService.sbuwiseBudgeFormData = new SBUWiseBudget();
  }

}
