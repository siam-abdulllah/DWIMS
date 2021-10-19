import { Component, OnInit } from '@angular/core';
import { AccountService } from '../../account/account.service';
import { Directive, ElementRef, HostListener } from '@angular/core';
import { Router } from '@angular/router';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

@Component({
  selector: 'app-asidenav',
  templateUrl: './asidenav.component.html',
  styles: [
  ]
})
export class AsidenavComponent implements OnInit {
  constructor(private accountService: AccountService,private el: ElementRef, private router: Router,
    private sanitizer: DomSanitizer) { }
  
  htmlToAdd: SafeHtml;
  @HostListener('click', ['$event'])
  public onClick(event) {
    debugger;
    if (event.target.tagName === 'A' && event.target.getAttribute('href')!=="javascript:void(0)") {
      
      this.router.navigate([event.target.getAttribute('href')]);
      event.preventDefault();
    } else {
      return;
    }
  }

  ngOnInit(): void {
    const menuList = JSON.parse(localStorage.getItem("menu"));
    var menuHeadName = "";
    var menu ="<li class=\"nav-item\">" ;
    menu = menu + "<a href=\"/portal/home\" class=\"nav-link\">" ;
    menu = menu + "<i class=\"nav-icon fas fa-home\"></i>" ;
    menu = menu + "<p>" ;
    menu = menu + "Home" ;
    menu = menu + "</p>" ;
    menu = menu + "</a>" ;
    menu = menu + "</li>";
    for (let i = 0; i < menuList.length; i++) {
      if (menuHeadName != menuList[i].menuHeadName) {
        menu = menu + "<li class=\"nav-item has-treeview\">";
        menu = menu + "<a href=\"javascript:void(0)\" class=\"nav-link\">";
        menu = menu + "<i class=\"nav-icon fas fa-user\"></i>";
        menu = menu + "<p>" + menuList[i].menuHeadName + "";
        menu = menu + "<i class=\"fas fa-angle-left right\"></i>";
        menu = menu + "</p>";
        menu = menu + "</a>";
        menu = menu + "<ul class=\"nav nav-treeview\">";
        for (let j = 0; j < menuList.length; j++) {
          if (menuList[i].menuHeadName == menuList[j].menuHeadName) {
            menu = menu + "<li class='nav-item' routerLinkActive='active'>";
            //menu = menu + "<a [routerLink]='[" + menuList[j].url + "]' class=\"nav-link\">";
            menu = menu + "<a  href=\"" + menuList[j].url + "\" class=\"nav-link\">";
            menu = menu + "<i class=\"far fa-circle nav-icon\"></i>";
            //menu = menu + "<p>" + menuList[j].subMenuName + "</p>";
            menu = menu +  menuList[j].subMenuName;
            menu = menu + "</a>";
            menu = menu + "</li>";
          }
        }
        menu = menu + "</ul>";
        menu = menu + "</li>";
        menuHeadName = menuList[i].menuHeadName;
      }
      
    }
    this.htmlToAdd=this.sanitizer.bypassSecurityTrustHtml(menu);
    
  }

}
