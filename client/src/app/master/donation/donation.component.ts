import { Donation, IDonation } from './../../shared/models/donation';
import { GenericParams } from './../../shared/models/genericParams';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MasterService } from '../master.service';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-donation',
  templateUrl: './donation.component.html',
  styles: [
  ]
})
export class DonationComponent implements OnInit {
  @ViewChild('search', {static: false}) 
  searchTerm!: ElementRef;
  genParams!: GenericParams;
  donations!: IDonation[];
  config: any;
  searchText = '';
  totalCount = 0;
  constructor(public masterService: MasterService, private router: Router,
    private toastr: ToastrService
    ) { }

  ngOnInit() {
    this.resetPage();
    this.getDonation();
  }
  getDonation(){
    const params = this.masterService.getGenParams();
    this.masterService.getDonation().subscribe(response => {    
      this.donations = response.data;
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
    if (this.masterService.donationFormData.id == 0)
      this.insertDonation(form);
    else
      this.updateDonation(form);
  }

  insertDonation(form: NgForm) {
    this.masterService.insertDonation().subscribe(
      res => {
        debugger;
        this.resetForm(form);
        this.getDonation();
        this.toastr.success('Save successfully', 'Donation')
      },
      err => { console.log(err); }
    );
  }

  updateDonation(form: NgForm) {
    this.masterService.updateDonation().subscribe(
      res => {
        debugger;
        this.resetForm(form);
        this.getDonation();
       this.toastr.info('Updated successfully', 'Donation')
      },
      err => { console.log(err); }
    );
  }

  onPageChanged(event: any){
    const params = this.masterService.getGenParams();
    if (params.pageIndex !== event)
    {
      params.pageIndex = event;
      this.masterService.setGenParams(params);
      this.getDonation();
    }
  }
  
  onSearch(){
    const params = this.masterService.getGenParams();
    params.search = this.searchTerm.nativeElement.value;
    params.pageIndex = 1;
    this.masterService.setGenParams(params);
    this.getDonation();
  }

  populateForm(selectedRecord: IDonation) {
    debugger;
    this.masterService.donationFormData = Object.assign({}, selectedRecord);
  }
  resetForm(form: NgForm) {
    this.searchText = '';
    form.reset();
  }
  resetPage() {
    this.masterService.donationFormData=new Donation();
    this.config = {
      currentPage: 1,
      itemsPerPage: 10,
      totalItems:50,
      };
  }

  resetSearch(){
    this.searchText = '';
    this.config = {
      currentPage: 1,
      itemsPerPage: 10,
      totalItems:50,
      };
}

}
