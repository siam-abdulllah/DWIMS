import { BcdsInfo, IBcdsInfo } from './../../shared/models/bcdsInfo';
import { GenericParams } from './../../shared/models/genericParams';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MasterService } from '../master.service';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-bcds-info',
  templateUrl: './bcds-info.component.html',
})
export class BcdsInfoComponent implements OnInit {

  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  genParams: GenericParams;
  numberPattern = "^[0-9]+(.[0-9]{1,10})?$";
  bcdsInfo: IBcdsInfo[];
  searchText = '';
  config: any;
  totalCount = 0;
  constructor(public masterService: MasterService, private router: Router, private toastr: ToastrService) {
   }

  ngOnInit() {
    this.resetPage();
    this.getBcds();
  }

  getBcds(){
    this.masterService.getBcdsList().subscribe(response => {
      const params = this.masterService.getGenParams();
      this.bcdsInfo = response.data;
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

  onSubmit(form: NgForm) {
    debugger;
    if (this.masterService.bcdsFormData.id == 0)
      this.insertBcds(form);
    else
      this.updateBcds(form);
  }


  insertBcds(form: NgForm) {
    this.masterService.insertBcds().subscribe(
      res => {
        debugger;
        this.resetForm(form);
        this.getBcds();
        this.toastr.success('Data Saved successfully', 'BCDS Information')
      },
      err => { console.log(err); }
    );
  }

  updateBcds(form: NgForm) {
    this.masterService.updateBcds().subscribe(
      res => {
        debugger;
        this.resetForm(form);
        this.getBcds();
        this.toastr.info('Data Updated Successfully', 'BCDS Information')
      },
      err => { console.log(err); }
    );
  }

  resetSearch(){
    this.searchText = '';
}

onPageChanged(event: any){
  const params = this.masterService.getGenParams();
  if (params.pageIndex !== event)
  {
    params.pageIndex = event;
    this.masterService.setGenParams(params);
    this.getBcds();
  }
}

onSearch(){
  const params = this.masterService.getGenParams();
  params.search = this.searchTerm.nativeElement.value;
  params.pageIndex = 1;
  this.masterService.setGenParams(params);
  this.getBcds();
}

  populateForm(selectedRecord: IBcdsInfo) {
    this.masterService.bcdsFormData = Object.assign({}, selectedRecord);
  }
  // resetForm(form: NgForm) {
  //   form.form.reset();
  //   this.masterService.bcdsFormData = new BcdsInfo();
  // }
  resetForm(form: NgForm) {
    this.searchText = '';
    form.reset();
  }
  resetPage() {
    this.masterService.bcdsFormData=new BcdsInfo();
  }
}
