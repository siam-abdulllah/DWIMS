import { Component, OnInit } from '@angular/core';
//import { LoginService } from 'src/app/_services/login.service';
import { Router } from '@angular/router';
//import { EmployeeService } from 'src/app/_services/employee.service';
//import { ICreateOrEditEmployeeDto, CreateOrEditEmployeeDto } from '../create-edit-employee-modal.component';
import { finalize } from 'rxjs/operators';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AccountService } from '../account/account.service';
import { ChangePasswordService } from '../_services/changePassword.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
//import { RegisterService } from 'src/app/_services/register.service';
@Component({
    selector: 'app-changePassword',
    templateUrl: './changePassword.component.html',
    styleUrls: ['./changePassword.component.css']
})
export class ChangePasswordComponent implements OnInit {
    saving = false;

    //employeeInfo:CreateOrEditEmployeeDto= new CreateOrEditEmployeeDto();


    //changePassword form
    loading = false;
    changePasswordForm: FormGroup;
    verifyCrntPass = false;
    empId: string;
    userRole: string;
    sbu: string;
    isTextFieldType = false;
    public showPassword: boolean;
    //
    constructor(
        //private _employeeService: EmployeeService,
        //private loginService: LoginService,
        private accountService: AccountService,
        private router: Router,
        private _alertifyService: AlertifyService,
        private changePasswordService: ChangePasswordService, private toastr: ToastrService, private SpinnerService: NgxSpinnerService
    ) { }

    ngOnInit() {
        this.getEmployeeId();
        this.createChangePasswordForm();
        this.changePasswordForm.get('newPassword').disable();
        this.changePasswordForm.get('confirmPassword').disable();
    }
    getEmployeeId() {
        this.empId = this.accountService.getEmployeeId();
        this.userRole = this.accountService.getUserRole();
    }


    // change Password start
    createChangePasswordForm() {
        this.changePasswordForm = new FormGroup({
            currentPassword: new FormControl('', [Validators.required]),
            newPassword: new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(10)]),
            confirmPassword: new FormControl('', [Validators.required])
        }, this.passwordMatchValidator);
    }
    passwordMatchValidator(g: FormGroup) {
        return g.get('newPassword').value === g.get('confirmPassword').value ? null : { mismatch: true };
    }

    verifyCurrentPassword() {
        //const empId = this.loginService.getEmpOrImpName();
        const employee: IVerifyCrntPassDto = {
            employeeSAPCode: this.empId,
            currentPassword: this.changePasswordForm.get('currentPassword').value
        };
        this.changePasswordService.verifyCurrentPassword(employee).subscribe(resp => {
            //debugger;
            if (resp) {
                this.changePasswordForm.get('newPassword').enable();
                this.changePasswordForm.get('confirmPassword').enable();
                this.verifyCrntPass = false;
            }
            else {
                this.changePasswordForm.get('newPassword').disable();
                this.changePasswordForm.get('confirmPassword').disable();
                this.verifyCrntPass = true;
            }
        }, err => {
            this.SpinnerService.hide();
            console.log(err);
        });
    }
    changePassword() {
        //const empId = this.loginService.getEmpOrImpName();
        const employee: IChangePassDto = {
            employeeSAPCode: this.empId,
            currentPassword: this.changePasswordForm.get('currentPassword').value,
            newPassword: this.changePasswordForm.get('newPassword').value
        };
        this.SpinnerService.show();
        this.changePasswordService.changePassword(employee).subscribe(resp => {
            this.SpinnerService.hide();
            if (resp) {
                this.toastr.success('Password Updated Successful')
            }
        }, err => {
            this.SpinnerService.hide();
            console.log(err);
            this._alertifyService.error(err.error);
        });
    }
    toggleShowPassword() {
        debugger;
        this.isTextFieldType = !this.isTextFieldType;
    }
}

interface IVerifyCrntPassDto {
    employeeSAPCode: string;
    currentPassword: string;
}
interface IChangePassDto {
    employeeSAPCode: string;
    newPassword: string;
    currentPassword: string;
}


