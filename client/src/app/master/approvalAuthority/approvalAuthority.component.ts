import { ApprovalAuthority, IApprovalAuthority } from './../../shared/models/approvalAuthority';
import { GenericParams } from './../../shared/models/genericParams';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MasterService } from '../master.service';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-approvalAuthority',
  templateUrl: './approvalAuthority.component.html',
  styles: [
  ]
})
export class ApprovalAuthorityComponent implements OnInit {
  @ViewChild('search', {static: false}) 
  searchTerm!: ElementRef;
  genParams!: GenericParams;
  approvalAuthoritys!: IApprovalAuthority[];
  totalCount = 0;
  //priorities =Array.from(Array(100).keys());
  priorities =Array.from({length: 100}, (_, i) => i + 1);
  constructor(public masterService: MasterService, private router: Router,
    private toastr: ToastrService
    ) { 
      
    }

  ngOnInit() {
    this.getapprovalAuthority();
  }
  counter(i: number) {
    return new Array(i);
}
  getapprovalAuthority(){
    this.masterService.getApprovalAuthority().subscribe(response => {
      debugger;
      this.approvalAuthoritys = response.data;
      this.totalCount = response.count;
    }, error => {
        console.log(error);
    });
  }
  onSubmit(form: NgForm) {
    debugger;
    if (this.masterService.approvalAuthorityFormData.id == 0)
      this.insertapprovalAuthority(form);
    else
      this.updateapprovalAuthority(form);
  }

  insertapprovalAuthority(form: NgForm) {
    this.masterService.insertApprovalAuthority().subscribe(
      res => {
        debugger;
        this.resetForm(form);
        this.getapprovalAuthority();
        this.toastr.success('Submitted successfully', 'Payment Detail Register')
      },
      err => { console.log(err); }
    );
  }

  updateapprovalAuthority(form: NgForm) {
    this.masterService.updateApprovalAuthority().subscribe(
      res => {
        debugger;
        this.resetForm(form);
        this.getapprovalAuthority();
       this.toastr.info('Updated successfully', 'Payment Detail Register')
      },
      err => { console.log(err); }
    );
  }

  populateForm(selectedRecord: IApprovalAuthority) {
    this.masterService.approvalAuthorityFormData = Object.assign({}, selectedRecord);
  }
  resetForm(form: NgForm) {
    form.form.reset();
    this.masterService.approvalAuthorityFormData = new ApprovalAuthority();
  }

   foo = new Array(45);//create a 45 element array


}
