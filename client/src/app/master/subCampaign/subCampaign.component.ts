import { SubCampaign, ISubCampaign } from '../../shared/models/subCampaign';
import { GenericParams } from '../../shared/models/genericParams';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MasterService } from '../master.service';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from "ngx-spinner"; 
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
  searchText = '';
  config: any;
  totalCount = 0;
  constructor(public masterService: MasterService, private router: Router,
    private toastr: ToastrService, private SpinnerService: NgxSpinnerService) { }

  ngOnInit() {
    this.resetPage();
    this.getSubCampaign();
  }
  getSubCampaign(){
    const params = this.masterService.getGenParams();
    this.SpinnerService.show();  
    this.masterService.getSubCampaign().subscribe(response => {
      this.subCampaigns = response.data;
      this.totalCount = response.count;
      this.config = {
        currentPage: params.pageIndex,
        itemsPerPage: params.pageSize,
        totalItems:this.totalCount,
        };
    }, error => {
        console.log(error);
    });
    this.SpinnerService.hide(); 
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
        this.toastr.success('Submitted successfully', 'Sub Campaign')
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
        this.toastr.info('Updated successfully', 'Sub Campaign')
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
      this.getSubCampaign();
    }
  }
  
  onSearch(){
    const params = this.masterService.getGenParams();
    params.search = this.searchTerm.nativeElement.value;
    params.pageIndex = 1;
    this.masterService.setGenParams(params);
    this.getSubCampaign();
  }

  resetSearch(){
    this.searchText = '';
}

  populateForm(selectedRecord: ISubCampaign) {
    this.masterService.subCampaignFormData = Object.assign({}, selectedRecord);
  }
  resetForm(form: NgForm) {
    this.searchText = '';
    form.reset();
  }
  resetPage() {
    this.masterService.subCampaignFormData=new SubCampaign();
  }

}
