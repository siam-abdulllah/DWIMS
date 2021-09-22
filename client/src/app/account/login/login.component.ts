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
      // email: new FormControl('', [Validators.required, Validators
      //   .pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')]),
      userName: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required)
    });
  }

  // tslint:disable-next-line: typedef
  onSubmit() {
    this.SpinnerService.show();  
    this.accountService.login(this.loginForm.value).subscribe(() => {
      this.SpinnerService.hide();  
      //this.router.navigateByUrl(this.returnUrl);
      //this.router.navigate(['/portal/home']);
      //this.router.navigate(['/portal/home'])
      this.router.navigate(['/portal/regApproval'])
  .then(() => {
    window.location.reload();
  });
    }, error => {
      this.SpinnerService.hide();  
      console.log(error);
    });
    
  }
  NavigateToRegister() {
    this.router.navigate(['/register']);
  }
}
