import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { IUser } from 'src/app/shared/models/user';
import { AccountService } from 'src/app/account/account.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent implements OnInit {

  currentUser$: Observable<IUser>;

  constructor(private accountService: AccountService) { }

  // tslint:disable-next-line: typedef
  ngOnInit() {
    this.currentUser$ = this.accountService.currentUser$;
  }

  // tslint:disable-next-line: typedef
  logout() {
    this.accountService.logout();
  }
}
