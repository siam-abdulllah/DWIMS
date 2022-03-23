import { Component, ElementRef, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { environment } from '../../environments/environment';
import 'jspdf-autotable';
import * as jsPDF from 'jspdf';
import { Router } from '@angular/router';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { GenericParams } from '../shared/models/genericParams';
import { DatePipe } from '@angular/common';
import { EmployeeService } from '../_services/employee.service';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'rptEmpInfo',
  templateUrl: './rptEmpInfo.component.html',
  styles: [
  ]
})

export class RptEmpInfoComponent implements OnInit {
  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  genParams: GenericParams;
  searchText = '';
  empList:any;
  bsConfig: Partial<BsDatepickerConfig>;
  bsValue: Date = new Date();
  baseUrl = environment.apiUrl;
  config = {
    keyboard: false,
    class: 'modal-lg',
    ignoreBackdropClick: true
  };
  userRole: any;
  date: Date;

  constructor(private router: Router,
    public datepipe: DatePipe,
    public empService: EmployeeService,  
    private toastr: ToastrService, private modalService: BsModalService,
    private SpinnerService: NgxSpinnerService,) { }

  ngOnInit() {
     this.ViewData();
  }
  resetSearch(){
    this.searchText = '';
}

  ViewData() {
    this.empService.getAllEmployee().subscribe(response => {
      this.empList = response;
    }, error => {
      console.log(error);
    });
  }
}
