import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AccountService } from '../account.service';
import { Router, ActivatedRoute } from '@angular/router';
import { NgxSpinnerService } from "ngx-spinner"; 

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  returnUrl: string;
  body: HTMLBodyElement = document.getElementsByTagName('body')[0];
  constructor(private accountService: AccountService, private router: Router, 
    private activatedRoute: ActivatedRoute,
    private SpinnerService: NgxSpinnerService) { }

  // tslint:disable-next-line: typedef
  ngOnInit() {
    //this.returnUrl = this.activatedRoute.snapshot.queryParams.returnUrl || '/shop';
    this.returnUrl = this.activatedRoute.snapshot.queryParams.returnUrl || '/portal/home';
    //this.router.navigate(['/portal/home']);
    this.createLoginForm();
    this.body.classList.add('loginBGImage');
  }
  ngOnDestroy() {
    // remove the the body classes
    this.body.classList.remove('loginBGImage');
   // this.body.classList.remove('sidebar-mini');
  }
  // tslint:disable-next-line: typedef
  createLoginForm() {
    this.loginForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators
        .pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')]),
      password: new FormControl('', Validators.required)
    });
  }

  // tslint:disable-next-line: typedef
  onSubmit() {
    this.SpinnerService.show();  
    this.accountService.login(this.loginForm.value).subscribe(() => {
      //this.router.navigateByUrl(this.returnUrl);
      //this.router.navigate(['/portal/home']);
      this.router.navigate(['/portal/home'])
  .then(() => {
    window.location.reload();
  });
    }, error => {
      console.log(error);
    });
    this.SpinnerService.hide();  
  }
  NavigateToRegister() {
    this.router.navigate(['/register']);
  }
}
