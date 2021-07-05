import { SubCampaign, ISubCampaign } from '../../shared/models/subCampaign';
import { GenericParams } from '../../shared/models/genericParams';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MasterService } from '../master.service';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-subCampaign',
  templateUrl: './subCampaign.component.html',
  styles: [
  ]
})
export class SubCampaignComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  genParams: GenericParams;
  subCampaigns: ISubCampaign[];
  totalCount = 0;
  constructor(public masterService: MasterService, private router: Router,
    private toastr: ToastrService) { }

  ngOnInit() {
    this.getSubCampaign();
  }
  getSubCampaign(){
    this.masterService.getSubCampaign().subscribe(response => {
      this.subCampaigns = response.data;
      this.totalCount = response.count;
    }, error => {
        console.log(error);
    });
  }
  onSubmit(form: NgForm) {
    debugger;
    if (this.masterService.subCampaignFormData.id == 0)
      this.insertSubCampaign(form);
    else
      this.updateSubCampaign(form);
  }

  insertSubCampaign(form: NgForm) {
    this.masterService.insertSubCampaign().subscribe(
      res => {
        debugger;
        this.resetForm(form);
        this.getSubCampaign();
        this.toastr.success('Submitted successfully', 'Payment Detail Register')
      },
      err => { console.log(err); }
    );
  }

  updateSubCampaign(form: NgForm) {
    this.masterService.updateSubCampaign().subscribe(
      res => {
        debugger;
        this.resetForm(form);
        this.getSubCampaign();
        this.toastr.info('Updated successfully', 'Payment Detail Register')
      },
      err => { console.log(err); }
    );
  }

  populateForm(selectedRecord: ISubCampaign) {
    this.masterService.subCampaignFormData = Object.assign({}, selectedRecord);
  }
  resetForm(form: NgForm) {
    form.form.reset();
    this.masterService.subCampaignFormData = new SubCampaign();
  }

}
