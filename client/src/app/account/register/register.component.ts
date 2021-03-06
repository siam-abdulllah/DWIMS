import { Component, OnInit } from '@angular/core';
import {
  FormGroup,
  FormBuilder,
  Validators,
  AsyncValidatorFn,
} from '@angular/forms';
import { AccountService } from '../account.service';
import { ActivatedRoute, Router } from '@angular/router';
import { timer, of } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { ToastrService } from 'ngx-toastr';
import { IUser, IUserResponse } from 'src/app/shared/models/user';
import { NgxSpinnerService } from "ngx-spinner"; 

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  errors: string[];
  //loading = false;
  dropdownSettings: IDropdownSettings;
  roleDropdownSettings: IDropdownSettings;
  roleList = [];
  selectedRole = [];
  id: string = null;
  user:IUserResponse = null;

  body: HTMLBodyElement = document.getElementsByTagName('body')[0];
  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private router: Router,
    private toastr: ToastrService,
    private route: ActivatedRoute,
    private SpinnerService: NgxSpinnerService
  ) {}

  // tslint:disable-next-line: typedef
  ngOnInit() {
    
    // this.getRoles();
    // this.loadRoles();
     this.createRegisterForm();
     this.body.classList.add('regBGImage');
    // this.id = this.route.snapshot.params.id;
    // if(this.id)
    // {
    //   this.getUserById();
    // }
  }
  ngOnDestroy() {
    // remove the the body classes
    this.body.classList.remove('regBGImage');
   // this.body.classList.remove('sidebar-mini');
  }
  backToLogin() {
    this.router.navigate(['/login']);
  }
  getUserById(){
    this.SpinnerService.show();  
    this.accountService.getUserById(this.id).subscribe(
      (response) => {
        if (response) 
        {
          this.user = response;         
          this.registerForm.controls["userForm"].patchValue({
            displayName: this.user.displayName,
            email: this.user.email,
            password:"*******",
            phoneNumber:this.user.phoneNumber,
            });          
          this.selectedRole = this.user.roles;
        }
        else{

        }
      },
      (error) => {
        this.errors = error.errors;
      }
    );
    this.SpinnerService.hide();  
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
        this.errors = error.errors;
      }
    );
  }

  // tslint:disable-next-line: typedef
  createRegisterForm() { 
    this.registerForm = this.fb.group({
      userForm: this.fb.group({
        displayName: [null, [Validators.required]],
        employeeId: [null, [Validators.required]],
        employeeSAPCode: [null, [Validators.required]],
        employeeName: [{value: null, disabled: true}],
        designationName: [{value: null, disabled: true}],
        departmentName: [{value: null, disabled: true}],
        //email: [{value: null, disabled: false}],
        email: [{value: null, disabled: false}],
        password: [null, [Validators.required]],
        phoneNumber: [{value: null, disabled: true}],
      }),
      // roleForm: this.fb.group({
      //   userRoles: [null, [Validators.required]],
      // }),
    });
    this.errors=[];
  }

  //############ Role ########.
  loadRoles() {
    // tslint:disable-next-line: no-unused-expression
    this.roleDropdownSettings = {
      singleSelection: false,
      idField: 'name',
      textField: 'name',
      selectAllText: 'Select All',
      unSelectAllText: 'UnSelect All',
      itemsShowLimit: 3,
      allowSearchFilter: false,
    };
  }

  // tslint:disable-next-line: typedef
  onItemSelect(item: any) {
    console.log(item);
  }

  // tslint:disable-next-line: typedef
  onSelectAll(items: any) {
    console.log(items);
  }

  // tslint:disable-next-line: typedef
  async onSubmit() {
   
    if(this.id)
    {
      this.updateRegisterUser();
    }
    else{
      this.registerUser();
    }
  }

  async updateRegisterUser()
  {
    
    this.SpinnerService.show();  
    this.registerForm.value.userForm.id= this.id;
    this.accountService.updateRegisterUser(this.registerForm.value).subscribe(
      (response) => {
        this.SpinnerService.hide();  
        this.registerForm.reset();
        this.toastr.success('Data Saved Successfully.');
      },
      (error) => {
        console.log(error);
        this.SpinnerService.hide();  
        this.errors = error.errors;
      }
    );
    
    
  }

  async registerUser()
  {
    
    this.SpinnerService.show();  
    this.accountService.register(this.registerForm.value).subscribe(
      (response) => {
        this.SpinnerService.hide();  
        this.registerForm.reset();
        this.toastr.success('Data Saved Successfully.');
      },
      (error) => {
        console.log(error);
        this.SpinnerService.hide();  
        this.errors = error.errors;
      }
    );
    
  }

  async employeeValidateById()
  {
    
    this.SpinnerService.show();  
    this.accountService.employeeValidateById(this.registerForm.value.userForm.employeeSAPCode).subscribe(
      (response) => {
       if(response.length>0) {
        this.SpinnerService.hide();  
        //this.registerForm.reset(); 
        this.createRegisterForm();
        this.toastr.success('Employee information found.');
        this.registerForm.controls.userForm.get('employeeId').setValue(response[0].id);
        this.registerForm.controls.userForm.get('employeeSAPCode').setValue(response[0].employeeSAPCode);
        this.registerForm.controls.userForm.get('employeeName').setValue(response[0].employeeName);
        this.registerForm.controls.userForm.get('designationName').setValue(response[0].designationName);
        this.registerForm.controls.userForm.get('departmentName').setValue(response[0].departmentName);
        this.registerForm.controls.userForm.get('email').setValue(response[0].email);
        this.registerForm.controls.userForm.get('phoneNumber').setValue(response[0].phone);
        //this.registerForm.controls.userForm.get('phoneNumber')['controls'].disable();
       }
       else{
        this.toastr.warning('Employee information not Found');
       }
      },
      (error) => {
        console.log(error);
        this.SpinnerService.hide();  
        this.errors = error.errors;
      }
    );
    
  }
}
