import { MenuHead, IMenuHead } from '../shared/models/menuHead';
import { GenericParams } from '../shared/models/genericParams';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MenuHeadService } from '../_services/menuHead.service';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../account/account.service';
import { ISubMenu } from '../shared/models/subMenu';
import { IMenuConfig, MenuConfig } from '../shared/models/menuConfig';
import { MenuConfigService } from '../_services/menuConfig.service';

@Component({
  selector: 'app-menuConfig',
  templateUrl: './menuConfig.component.html',
  styles: [
  ]
})
export class MenuConfigComponent implements OnInit {
  @ViewChild('search', {static: false}) 
  searchTerm!: ElementRef;
  genParams!: GenericParams;
  menuHeads!: IMenuHead[];
  subMenus!: ISubMenu[];
  menuConfigs!: IMenuConfig[];
  totalCount = 0;
  roleList = [];
  //priorities =Array.from(Array(100).keys());
  priorities =Array.from({length: 100}, (_, i) => i + 1);
  constructor(private accountService: AccountService,
    public menuConfigService: MenuConfigService, 
    private router: Router,
    private toastr: ToastrService
    ) { 
      
    }

  ngOnInit() {
    this.resetPage();
    this.getMenuHead();
    this.getRoles();
  }
  counter(i: number) {
    return new Array(i);
}
getRoles() {
  this.roleList = [];
  this.accountService.getRoles().subscribe(
    (response) => {
      if (response) {
        this.roleList = response;
      }
    },
    (error) => {
      console.log(error);
    }
  );
}
  getMenuConfig(){
    this.menuConfigService.getMenuConfig().subscribe(response => {
      debugger;
      this.menuConfigs = response as IMenuConfig[];
      
    }, error => {
        console.log(error);
    });
  }
  getMenuHead(){
    this.menuConfigService.getMenuHead().subscribe(response => {
      debugger;
      this.menuHeads = response as IMenuHead[];
      
    }, error => {
        console.log(error);
    });
    
  }
  getSubMenu(){
    this.menuConfigService.getSubMenu().subscribe(response => {
      debugger;
      this.subMenus = response as ISubMenu[];
      
    }, error => {
        console.log(error);
    });
    if(this.menuConfigService.menuConfigFormData.roleId != "" || this.menuConfigService.menuConfigFormData.roleId != null || this.menuConfigService.menuConfigFormData.roleId != undefined){
      this.getMenuConfig()
    }
  }
  onSubmit(form: NgForm) {
    debugger;
    if (this.menuConfigService.menuConfigFormData.id == 0)
      this.insertMenuConfig(form);
    else
      this.updateMenuConfig(form);
  }

  insertMenuConfig(form: NgForm) {
    this.menuConfigService.insertMenuConfig().subscribe(
      res => {
        debugger;
        this.resetForm(form);
        this.getMenuHead();
        this.toastr.success('Submitted successfully', 'Menu Config')
      },
      err => { console.log(err); }
    );
  }

  updateMenuConfig(form: NgForm) {
    this.menuConfigService.updateMenuConfig().subscribe(
      res => {
        debugger;
        this.resetForm(form);
        this.getMenuHead();
       this.toastr.info('Updated successfully', 'Menu Config')
      },
      err => { console.log(err); }
    );
  }

  populateForm(selectedRecord: IMenuConfig) {
    this.menuConfigService.menuConfigFormData = Object.assign({}, selectedRecord);
  }
  remove(selectedRecord: IMenuConfig) {
    var result = confirm("Do you want to delete?");
    if (result) {
      this.menuConfigService.removeMenuConfig(selectedRecord.id).subscribe(
        res => {
          this.getSubMenu();
          this.toastr.success(res);
          
        },
        err => { console.log(err); }
      );
    }
  }

  resetForm(form: NgForm) {
    form.reset();
  }
  resetPage() {
   this.menuConfigService.menuConfigFormData=new MenuConfig();
  }

   


}
