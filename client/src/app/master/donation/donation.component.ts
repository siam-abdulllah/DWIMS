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
  totalCount = 0;
  constructor(public masterService: MasterService, private router: Router,
    private toastr: ToastrService
    ) { }

  ngOnInit() {
    this.getDonation();
  }
  getDonation(){
    this.masterService.getDonation().subscribe(response => {
      this.donations = response.data;
      this.totalCount = response.count;
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
        this.toastr.success('Submitted successfully', 'Payment Detail Register')
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
       this.toastr.info('Updated successfully', 'Payment Detail Register')
      },
      err => { console.log(err); }
    );
  }

  populateForm(selectedRecord: IDonation) {
    debugger;
    this.masterService.donationFormData = Object.assign({}, selectedRecord);
  }
  resetForm(form: NgForm) {
    debugger;
    form.reset();
    //this.masterService.donationFormData = Object.assign({},this.donations);
    //this.masterService.donationFormData.status='Active';
    this.masterService.donationFormData.status="Active";
  }

}
