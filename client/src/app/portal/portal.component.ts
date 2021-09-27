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
  // constructor(private alertify: AlertifyService, private router: Router, private loginService: LoginService) {
  // }
  constructor(private router: Router) {
  }

  ngOnInit() {
    window.addEventListener('scroll', this.scrollEvent, true);
    // add the the body classes
   // this.body.classList.add('skin-blue');
  // this.body.classList.add('sidebar-mini');
  }
  ngOnDestroy() {
    window.removeEventListener('scroll', this.scrollEvent, true);
    // remove the the body classes
   // this.body.classList.remove('skin-blue');
   // this.body.classList.remove('sidebar-mini');
  }
  scrollEvent = (event: any): void => {
    var cardHeader = document.getElementsByClassName('card-header')[0];
    const n = event.srcElement.scrollingElement.scrollTop;
    if(n > 63){
      cardHeader.classList.add('stick');
    }else{
      cardHeader.classList.remove('stick');
    }
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

