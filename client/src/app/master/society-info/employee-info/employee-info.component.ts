import { EmployeeInfo, IEmployeeInfo } from './../../shared/models/employeeInfo';
import { GenericParams } from './../../shared/models/genericParams';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MasterService } from '../master.service';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-employee-info',
  templateUrl: './employee-info.component.html',
  styleUrls: ['./employee-info.component.scss']
})
export class EmployeeInfoComponent implements OnInit {
  
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  genParams: GenericParams;
  empInfo: IEmployeeInfo[];
  totalCount = 0;
  constructor(public masterService: MasterService, private router: Router, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.getEmployee();
  }

  getEmployee(){
    this.masterService.getEmployeeList().subscribe(response => {
      debugger;
      this.empInfo = response.data;
      this.totalCount = response.count;
    }, error => {
        console.log(error);
    });
  }
  resetForm(form: NgForm) {
    form.form.reset();
    this.masterService.employeeFormData = new EmployeeInfo();
  }
}
