import { Component, OnInit, OnDestroy } from '@angular/core';
// import { $ } from 'protractor';
// import { AlertifyService } from '../_services/alertify.service';
 import { Router } from '@angular/router';
// import { LoginService } from '../_services/login.service';

@Component({
  selector: 'app-portal',
  templateUrl: './portal.component.html',
  styleUrls: ['./portal.component.css']
})
export class PortalComponent implements OnInit, OnDestroy {
//  bodyClasses = 'skin-blue sidebar-mini';
  // body: HTMLBodyElement = document.getElementsByTagName('body')[0];
  // constructor(private alertify: AlertifyService, private router: Router, private loginService: LoginService) {
  // }
  constructor(private router: Router) {
  }

  ngOnInit() {
    // add the the body classes
   // this.body.classList.add('skin-blue');
  // this.body.classList.add('sidebar-mini');
  }
  ngOnDestroy() {
    // remove the the body classes
   // this.body.classList.remove('skin-blue');
   // this.body.classList.remove('sidebar-mini');
  }
  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }
  editImporter() {
    this.router.navigate(['/portal/editimporterinfo']);
  }
  checkPermission() {
    // if(this.loginService.getUserRole()=="Importer")
    // {
    //   this.router.navigate(['/portal/editimporterinfo']);
    // }
    // else if(this.loginService.getUserRole()=="Admin" || "SA" || "Executive")
    // {
    //   this.router.navigate(['/portal/editemployeeinfo']);
    // }
    // else{
    //   console.log("Other");
    // }
  }
  getUserRole() {
   // return this.loginService.getUserRole();
  }
  getUserName() {
    //return this.loginService.getUserName();
  }
  getOrganizationName() {
    //return this.loginService.getOrganizationName();
  }
  getPosition() {
    //return this.loginService.getPosition();
  }
 }

