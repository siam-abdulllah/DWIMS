import { MenuHead, IMenuHead } from '../shared/models/menuHead';
import { GenericParams } from '../shared/models/genericParams';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { SubMenuService } from '../_services/subMenu.service';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ISubMenu, SubMenu } from '../shared/models/subMenu';

@Component({
  selector: 'app-subMenu',
  templateUrl: './subMenu.component.html',
  styles: [
  ]
})
export class SubMenuComponent implements OnInit {
  @ViewChild('search', {static: false}) 
  searchTerm!: ElementRef;
  genParams!: GenericParams;
  menuHeads!: IMenuHead[];
  subMenus!: ISubMenu[];
  totalCount = 0;
  //priorities =Array.from(Array(100).keys());
  priorities =Array.from({length: 100}, (_, i) => i + 1);
  constructor(public subMenuService: SubMenuService, private router: Router,
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
    this.subMenuService.getMenuHead().subscribe(response => {
      debugger;
      this.menuHeads = response as IMenuHead[];
      //this.totalCount = response.count;
    }, error => {
        console.log(error);
    });
  }
  getSubMenu(){
    this.subMenuService.getSubMenu().subscribe(response => {
      debugger;
      this.subMenus = response.data;
      this.totalCount = response.count;
    }, error => {
        console.log(error);
    });
  }
  onSubmit(form: NgForm) {
    debugger;
    if (this.subMenuService.subMenuFormData.id == 0)
      this.insertSubMenu(form);
    else
      this.updateSubMenu(form);
  }

  insertSubMenu(form: NgForm) {
    this.subMenuService.insertSubMenu().subscribe(
      res => {
        this.getSubMenu();
        this.resetForm(form);
        this.toastr.success('Submitted successfully', 'Sub Menu')
      },
      err => { console.log(err); }
    );
  }

  updateSubMenu(form: NgForm) {
    this.subMenuService.updateSubMenu().subscribe(
      res => {
        this.getMenuHead();
        this.resetForm(form);
       this.toastr.info('Updated successfully', 'Sub Menu')
      },
      err => { console.log(err); }
    );
  }

  populateForm(selectedRecord: ISubMenu) {
    this.subMenuService.subMenuFormData = Object.assign({}, selectedRecord);
  }
  resetForm(form: NgForm) {
    form.reset();
  }
  resetPage() {
   this.subMenuService.subMenuFormData=new SubMenu();
  }

   


}