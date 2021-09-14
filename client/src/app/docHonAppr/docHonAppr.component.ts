
import { SubCampaign, ISubCampaign } from '../shared/models/subCampaign';
import {  IDoctor } from '../shared/models/docotor';
import { GenericParams } from '../shared/models/genericParams';
import { Component, ElementRef, OnInit, ViewChild , TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
//import { MasterService } from '../master.service';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService} from 'ngx-bootstrap/modal';
import { DoctorHonAppr, IDoctorHonAppr } from '../shared/models/doctorHonAppr';
import { DoctorHonApprService } from '../_services/doctorHonAppr.service';
import { DatePipe } from '@angular/common';
@Component({
  selector: 'app-docHonAppr',
  templateUrl: './docHonAppr.component.html',
  styles: [
  ]
})
export class DocHonApprComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
 // @ViewChild('campaignModal', { static: false }) campaignModal: TemplateRef<any>;
  // campaignModalRef: BsModalRef;
  // genParams: GenericParams;
  // campaigns: ICampaign[]; 
  // subCampaigns: ISubCampaign[]; 
  month = '';
  fDate = '';
  doctorHonAppr: IDoctorHonAppr[]= [];
  doctors: IDoctor[];
  totalCount = 0;
  bsConfig: Partial<BsDatepickerConfig>;
  bsValue: Date = new Date();
  //constructor(public masterService: MasterService, private router: Router,
    constructor( private docHonApprService: DoctorHonApprService,private router: Router,
    private toastr: ToastrService,private datePipe: DatePipe) { }

  ngOnInit() {
    
    this.resetPage();
    //this.getCampaign();
    this.bsConfig = Object.assign({}, { containerClass: 'theme-green' }, { dateInputFormat: 'DD/MM/YYYY' });
    this.bsValue = new Date();
  }
  changeDateInDetail(){
    var fromDate = new Date(this.month);
     this.fDate = this.datePipe.transform(fromDate, "MMyyyy");
     this.docHonApprService.getDoctorHonAppr(this.fDate).subscribe(response => {
      debugger;
      this.doctorHonAppr = response.data;
      this.totalCount = response.count;
    }, error => {
        console.log(error);
    });
  }
  getCampaign(){
    // this.masterService.getCampaign().subscribe(response => {
    //   this.campaigns = response.data;
    //   this.totalCount = response.count;
    // }, error => {
    //     console.log(error);
    // });
  }
  onSubmit(form: NgForm) {
    
    // if (this.masterService.campaignFormData.id == 0)
    //   this.insertCampaign(form);
    // else
    //   this.updateCampaign(form);
  }

  insertCampaign(form: NgForm) {
    // this.masterService.insertCampaign().subscribe(
    //   res => {
    //     debugger;
    //     this.resetForm(form);
    //     this.getCampaign();
    //     this.toastr.success('Submitted successfully', 'Payment Detail Register')
    //   },
    //   err => { console.log(err); }
    // );
  }

  updateCampaign(form: NgForm) {
    // this.masterService.updateSubCampaign().subscribe(
    //   res => {
    //     debugger;
    //     this.resetForm(form);
    //     this.getCampaign();
    //     this.toastr.info('Updated successfully', 'Payment Detail Register')
    //   },
    //   err => { console.log(err); }
    // );
  }

  populateForm() {
    //this.masterService.campaignFormData = Object.assign({}, selectedRecord);
  }
  
  clickStatusDoctorHon(selectedRecord:IDoctorHonAppr) {
    debugger;
if(selectedRecord.id==0)
{
    this.docHonApprService.insertDocHonAppr(selectedRecord).subscribe(
      res => {
        
        this.changeDateInDetail();
       
        this.toastr.success('Save successfully', 'Investment ')
      },
      err => { console.log(err); }
    );
  }
  else{
    this.docHonApprService.updateDocHonAppr(selectedRecord).subscribe(
      res => {
        
        this.changeDateInDetail();
       
        this.toastr.success('Save successfully', 'Investment ')
      },
      err => { console.log(err); }
    );
  }
  }
  resetForm(form: NgForm) {
    form.form.reset();
    //this.masterService.campaignFormData = new Campaign();
  }
  resetPage() {
    this.docHonApprService.doctorHonApprFormData = new DoctorHonAppr();
  }
  
}
