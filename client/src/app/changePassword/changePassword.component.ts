// import { Component, OnInit } from '@angular/core';
// //import { LoginService } from 'src/app/_services/login.service';
// import { Router } from '@angular/router';
// //import { EmployeeService } from 'src/app/_services/employee.service';
// //import { ICreateOrEditEmployeeDto, CreateOrEditEmployeeDto } from '../create-edit-employee-modal.component';
// import { finalize } from 'rxjs/operators';
// import { AlertifyService } from 'src/app/_services/alertify.service';
// import { FormGroup, FormControl, Validators } from '@angular/forms';
// import { AccountService } from '../account/account.service';
// //import { RegisterService } from 'src/app/_services/register.service';
// @Component({
//   selector: 'app-changePassword',
//   templateUrl: './changePassword.component.html',
//   styleUrls: ['./changePassword.component.css']
// })
// export class EditEmployeeInfoComponent implements OnInit {
//   saving = false;

//   //employeeInfo:CreateOrEditEmployeeDto= new CreateOrEditEmployeeDto();
//   changeViewId=0;

//   //changePassword form
//   loading = false;
//   changePasswordForm: FormGroup;
//   verifyCrntPass = false;
//   empId: string;
//   userRole: string;
//   sbu: string;
//   //
//   constructor(
//     //private _employeeService: EmployeeService,
//     //private loginService: LoginService,
//     private accountService: AccountService,
//     private router: Router,
//     private _alertifyService: AlertifyService,
//     //private registerService: RegisterService,
//   ) { }

//   ngOnInit() {
//     this.getEmployeeId();
//     this.createChangePasswordForm();
//     this.changePasswordForm.get('newPassword').disable();
//     this.changePasswordForm.get('confirmPassword').disable();
//   }
//   getEmployeeId() {
//     this.empId = this.accountService.getEmployeeId();
//     this.userRole = this.accountService.getUserRole();
//   }
//   changePasswordBtn() {
//     this.changeViewId=2;
//   }
//   editEmployeeInfo() {
//     this.changeViewId=1;
//   //  this.router.navigate(['portal/editinfo']);
//   }
//   backToEmployeeInfo(){
//     this.changeViewId=0;
//   }
//   update( employeeInfo:CreateOrEditEmployeeDto): void {
//     this.saving = true;
//     this._employeeService.updateEmployeeInfoes(employeeInfo,employeeInfo.id)
//       .pipe(finalize(() => { this.saving = false; }))
//       .subscribe(() => {
//         this._alertifyService.success('Update Successfully');
//       });
//   }
// // change Password start
// createChangePasswordForm() {
//   this.changePasswordForm = new FormGroup({
//     currentPassword: new FormControl('', [Validators.required]),
//     newPassword: new FormControl('',  [Validators.required, Validators.minLength(5), Validators.maxLength(10)]),
//     confirmPassword : new FormControl('', [Validators.required])
//   }, this.passwordMatchValidator);
// }
// passwordMatchValidator(g: FormGroup) {
//   return g.get('newPassword').value === g.get('confirmPassword').value ? null : {mismatch: true };
// }

// verifyCurrentPassword() {
//   const empId = this.loginService.getEmpOrImpName();
//   const employee: IVerifyCrntPassDto = {
//     employeeId: empId,
//     currentPassword: this.changePasswordForm.get('currentPassword').value
//   };
//   this.registerService.verifyCurrentPasswordEmployee(employee).subscribe(resp => {
//     if (resp) {
//       this.changePasswordForm.get('newPassword').enable();
//       this.changePasswordForm.get('confirmPassword').enable();
//       this.verifyCrntPass = false;
//     } else {
//       this.changePasswordForm.get('newPassword').disable();
//       this.changePasswordForm.get('confirmPassword').disable();
//       this.verifyCrntPass = true;
//     }
//   }, err => {
//     console.log(err);
//   });
// }
// changePassword() {
//   this.loading = true;
//   const empId = this.loginService.getEmpOrImpName();
//   const employee: IChangePassDto = {
//     employeeId: empId,
//     newPassword: this.changePasswordForm.get('newPassword').value
//   };
//   this.registerService.changePasswordEmployee(employee).subscribe(resp => {
//     this.loading = false;
//     if (resp) {
//       this._alertifyService.success('Password Updated Successful');
//     }
//   }, err => {
//     this.loading = false;
//     console.log(err);
//     this._alertifyService.error(err.error);
//   });
// }

// }
// interface IVerifyCrntPassDto {
// employeeId: number;
// currentPassword: string;
// }
// interface IChangePassDto {
// employeeId: number;
// newPassword: string;
// }


