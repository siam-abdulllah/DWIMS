import { MenuHead, IMenuHead } from '../shared/models/menuHead';
import { GenericParams } from '../shared/models/genericParams';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MenuHeadService } from '../_services/menuHead.service';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-menuHead',
  templateUrl: './menuHead.component.html',
  styles: [
  ]
})
export class MenuHeadComponent implements OnInit {
  @ViewChild('search', {static: false}) 
  searchTerm!: ElementRef;
  genParams!: GenericParams;
  menuHeads!: IMenuHead[];
  totalCount = 0;
  //priorities =Array.from(Array(100).keys());
  priorities =Array.from({length: 100}, (_, i) => i + 1);
  constructor(public menuHeadService: MenuHeadService, private router: Router,
    private toastr: ToastrService
    ) { 
      
    }

  ngOnInit() {
    this.resetPage();
    this.getMenuHead();
  }
  counter(i: number) {
    return new Array(i);
}
  getMenuHead(){
    this.menuHeadService.getMenuHead().subscribe(response => {
      debugger;
      this.menuHeads = response.data;
      this.totalCount = response.count;
    }, error => {
        console.log(error);
    });
  }
  onSubmit(form: NgForm) {
    debugger;
    if (this.menuHeadService.menuHeadFormData.id == 0)
      this.insertMenuHead(form);
    else
      this.updateMenuHead(form);
  }

  insertMenuHead(form: NgForm) {
    this.menuHeadService.insertMenuHead().subscribe(
      res => {
        debugger;
        this.resetForm(form);
        this.getMenuHead();
        this.toastr.success('Submitted successfully', 'Payment Detail Register')
      },
      err => { console.log(err); }
    );
  }

  updateMenuHead(form: NgForm) {
    this.menuHeadService.updateMenuHead().subscribe(
      res => {
        debugger;
        this.resetForm(form);
        this.getMenuHead();
       this.toastr.info('Updated successfully', 'Payment Detail Register')
      },
      err => { console.log(err); }
    );
  }

  populateForm(selectedRecord: IMenuHead) {
    this.menuHeadService.menuHeadFormData = Object.assign({}, selectedRecord);
  }
  resetForm(form: NgForm) {
    form.reset();
  }
  resetPage() {
   this.menuHeadService.menuHeadFormData=new MenuHead();
  }

   


}
