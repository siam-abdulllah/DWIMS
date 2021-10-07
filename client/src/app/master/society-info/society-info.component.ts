import { SocietyInfo, ISocietyInfo } from './../../shared/models/societyInfo';
import { GenericParams } from './../../shared/models/genericParams';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MasterService } from '../master.service';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-society-info',
  templateUrl: './society-info.component.html',
  styleUrls: ['./society-info.component.scss']
})
export class SocietyInfoComponent implements OnInit {

  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  genParams: GenericParams;
  numberPattern = "^[0-9]+(.[0-9]{1,10})?$";
  societyinfo: ISocietyInfo[];
  searchText = '';
  totalCount = 0;
  constructor(public masterService: MasterService, private router: Router, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.resetPage()
    this.getSociety();
  }

  getSociety(){
    this.masterService.getSocietyList().subscribe(response => {
      debugger;
      this.societyinfo = response.data;
      this.totalCount = response.count;
    }, error => {
        console.log(error);
    });
  }

  onSubmit(form: NgForm) {
    debugger;
    if (this.masterService.societyFormData.id == 0)
      this.insertSociety(form);
    else
      this.updateSociety(form);
  }


  insertSociety(form: NgForm) {
    this.masterService.insertSociety().subscribe(
      res => {
        debugger;
        this.resetForm(form);
        this.getSociety();
        this.toastr.success('Data Saved successfully', 'Society Information')
      },
      err => { console.log(err); }
    );
  }

  updateSociety(form: NgForm) {
    this.masterService.updateSociety().subscribe(
      res => {
        debugger;
        this.resetForm(form);
        this.getSociety();
        this.toastr.info('Data Updated Successfully', 'Society Information')
      },
      err => { console.log(err); }
    );
  }

  onPageChanged(event: any){
    const params = this.masterService.getGenParams();
    if (params.pageNumber !== event)
    {
      params.pageNumber = event;
      this.masterService.setGenParams(params);
      this.getSociety();
    }
  }
  
  onSearch(){
    const params = this.masterService.getGenParams();
    params.search = this.searchTerm.nativeElement.value;
    params.pageNumber = 1;
    this.masterService.setGenParams(params);
    this.getSociety();
  }

  resetSearch(){
    this.searchText = '';
}

  populateForm(selectedRecord: ISocietyInfo) {
    this.masterService.societyFormData = Object.assign({}, selectedRecord);
  }
  resetForm(form: NgForm) {
    this.searchText = '';
    form.form.reset();
    this.masterService.societyFormData = new SocietyInfo();
  }
  resetPage() {
    this.masterService.societyFormData = new SocietyInfo();
  }

}
