import { Component, OnInit } from '@angular/core';
import { AccountService } from '../../account/account.service';

@Component({
  selector: 'app-asidenav',
  templateUrl: './asidenav.component.html',
  styles: [
  ]
})
export class AsidenavComponent implements OnInit {

  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
  }
}
