import { Component, ElementRef, OnInit, ViewChild, TemplateRef } from '@angular/core';
import 'jspdf-autotable';
import * as jsPDF from 'jspdf';
import { Router } from '@angular/router';
import { FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from "ngx-spinner";
import { GenericParams } from '../shared/models/genericParams';
import { AccountService } from '../account/account.service';
import { DatePipe, getLocaleDateTimeFormat } from '@angular/common';
import { BgtOwnService } from '../_services/bgtOwn.service';
import { IApprovalAuthority } from '../shared/models/approvalAuthority';
import { ISBU } from '../shared/models/sbu';
import { DATE } from 'ngx-bootstrap/chronos/units/constants';
import { IEmployeeInfo } from '../shared/models/employeeInfo';
import { IEmployee } from '../shared/models/employee';
import { IDonation } from '../shared/models/donation';

@Component({
  selector: 'bgtOwn',
  templateUrl: './bgtOwn.component.html',
})
export class BgtOwnComponent implements OnInit {

  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  @ViewChild('pendingListModal', { static: false }) pendingListModal: TemplateRef<any>;
  pendingListModalRef: BsModalRef;
  bsConfig: Partial<BsDatepickerConfig>;
  bsValue: Date = new Date();
  bgtOwn: FormGroup;
  empId: string;
  genParams: GenericParams;
  approvalAuthorities: IApprovalAuthority[];
  SBUs: ISBU[];
  employees: IEmployeeInfo[];
  numberPattern = "^[0-9]+(.[0-9]{1,10})?$";
  isValid: boolean = true;
  valShow: boolean = true;
  isHide: boolean = false;
  searchText = '';
  config: any;
  totalCount = 0;
  userRole: any;
  donations: IDonation[];
  bgtOwns: IBgtOwn[];
  constructor(public bgtService: BgtOwnService, private SpinnerService: NgxSpinnerService, private modalService: BsModalService, private accountService: AccountService,
    private router: Router, private toastr: ToastrService, private datePipe: DatePipe,) {
  }
  customSearchFnEmp(term: string, item: any) {
    term = term.toLocaleLowerCase();
    return item.employeeSAPCode.toLocaleLowerCase().indexOf(term) > -1 ||
      item.employeeName.toLocaleLowerCase().indexOf(term) > -1;
  }
  onChangeEmployee() {
    this.bgtService.getEmpWiseData(this.bgtOwn.value.employee).subscribe(response => {
      var data = response as IApprovalAuthority[];
      this.bgtOwn.patchValue({
        authId: data[0].id,
      });
    }, error => {
      console.log(error);
    });
    this.bgtService.getEmpWiseSBU(this.bgtOwn.value.employee).subscribe(response => {
      this.SBUs = response as ISBU[];
    }, error => {
      console.log(error);
    });
    debugger;
    for (let index = 0; index < this.employees.length; index++) {
      if (this.bgtOwn.value.employee == this.employees[index].id) {
        if (this.employees[index].departmentName == 'Sales') {
          this.bgtOwn.patchValue({
            deptId: 1,
          });
        }
        else if (this.employees[index].departmentName == 'PMD') {
          this.bgtOwn.patchValue({
            deptId: 2,
          });
        }
      }
    }
    // this.bgtService.getEmpWiseBgt(this.bgtOwn.value.employee).subscribe(response => {    
    //   this.employees=response as IEmployeeInfo[];


    //  }, error => {
    //     console.log(error);
    //  });
  }
  onChangeSBU() {
    // for (let index = 0; index < this.employees.length; index++) {
    //   if (this.bgtOwn.value.employee == this.employees[index].id) {
    //     if (this.employees[index].departmentName == 'Sales') {
    //       this.bgtOwn.patchValue({
    //         deptId: 1,
    //       });
    //     }
    //     else if (this.employees[index].departmentName == 'PMD') {
    //       this.bgtOwn.patchValue({
    //         deptId: 2,
    //       });
    //     }
    //   }
    // }
    //this.bgtService.getSbuWiseEmp(this.bgtOwn.value.sbu).subscribe(response => {
     // this.employees = response as IEmployeeInfo[];
      // this.bgtOwn.patchValue({
      //   sbuTotalBudget: response[0].count,
      // });

    //}, error => {
    //  console.log(error);
    //});

  }
  createbgtOwnForm() {
    this.bgtOwn = new FormGroup({
      deptId: new FormControl({ value: '', disabled: true }, [Validators.required]),
      year: new FormControl('', [Validators.required]),
      authId: new FormControl({ value: '', disabled: true }, [Validators.required]),
      sbu: new FormControl('', [Validators.required]),
      totalAmount: new FormControl(''),
      donationId: new FormControl(''),
      donationAmt: new FormControl(''),
      transLimit: new FormControl(''),
      totalExpense: new FormControl(''),
      totalPipeline: new FormControl(''),
      segment: new FormControl(''),
      permEdit: new FormControl(''),
      permView: new FormControl(''),
      prevAllocate: new FormControl(''),
      remaining: new FormControl(''),
      employee: new FormControl(''),
    });

    
  }

  getDeptSbuWiseBudgetAmt() {
    // if (this.bgtOwn.getRawValue().deptId == "" || this.bgtOwn.getRawValue().deptId == null) {
    //   this.toastr.error('Select Department');
    //   this.bgtOwn.patchValue({
    //     year: "",
    //   });
    //   return;
    // }
    if (this.bgtOwn.value.sbu == "" || this.bgtOwn.value.sbu == null) {
      this.toastr.error('Select SBU');
      this.bgtOwn.patchValue({
        year: "",
      });
      return;
    }
    if (this.bgtOwn.value.employee == "" || this.bgtOwn.value.employee == null) {
      this.toastr.error('Select employee');
      this.bgtOwn.patchValue({
        year: "",
      });
      return;
    }

    var yr = new Date(this.bgtOwn.value.year);
    yr.getFullYear();

    this.bgtService.getEmpWiseBgt(this.bgtOwn.value.employee, this.bgtOwn.value.sbu,yr.getFullYear(),1000,this.bgtOwn.getRawValue().deptId).subscribe(response => {
      this.bgtOwn.patchValue({
        segment: response[0].segment,
        prevAllocate: response[0].amount,
        totalAmount: response[0].amount,
        remaining: this.bgtOwn.value.prevAllocate-this.bgtOwn.value.totalAmount,
        permEdit: response[0].permEdit,
        permView: response[0].permView,
      });
    }, error => {
      console.log(error);
    });
    
    this.bgtService.getEmpWiseTotExp(this.bgtOwn.value.employee, this.bgtOwn.value.sbu,yr.getFullYear(),1000,this.bgtOwn.getRawValue().deptId).subscribe(response => {
      debugger;
      this.bgtOwn.patchValue({
        totalExpense:response[0].count
      });
    }, error => {
      console.log(error);
    });

    this.bgtService.getEmpWiseTotPipe(this.bgtOwn.value.employee, this.bgtOwn.value.sbu,yr.getFullYear(),1000,this.bgtOwn.getRawValue().deptId).subscribe(response => {
      debugger;
      this.bgtOwn.patchValue({
        totalPipeline:response[0].count
      });
    }, error => {
      console.log(error);
    });
    this.bgtService.getEmpOwnBgt(this.bgtOwn.value.employee, this.bgtOwn.value.sbu,yr.getFullYear(),1000,this.bgtOwn.getRawValue().deptId).subscribe(response => {
      debugger;
    this.bgtOwns=response as IBgtOwn[];
    
    }, error => {
      console.log(error);
    });
  }

  getAuthPersonCount() {
    if (this.bgtOwn.value.authId == "" || this.bgtOwn.value.authId == null) {
      this.toastr.error('Select Authorization Level');
      return;
    }

    this.bgtService.getAuthPersonCount(this.bgtOwn.value.authId).subscribe(response => {
      // this.bgtOwn.patchValue({
      //   ttlPerson: response[0].count,
      // });

    }, error => {
      console.log(error);
    });
  }


  getAllocatedAmount() {

    if (this.bgtOwn.value.segment == "" || this.bgtOwn.value.segment == null) {
      this.toastr.error('Select Segmentation');
      return;
    }

    if (this.bgtOwn.value.segment == "Monthly") {
      const d = new Date();

      var remMonth = 12 - d.getMonth() - 1;
      // var ttlAloc = this.bgtOwn.value.ttlPerson * this.bgtOwn.value.ttlAmount * remMonth;

      // this.bgtOwn.patchValue({
      //   ttlAllocate: ttlAloc,
      // });
    }
    else if (this.bgtOwn.value.segment == "Yearly") {
      // var ttlAloc = this.bgtOwn.value.ttlPerson * this.bgtOwn.value.ttlAmount;

      // this.bgtOwn.patchValue({
      //   ttlAllocate: ttlAloc,
      // });
    }
  }

  getApprovalAuthority() {
    this.bgtService.getApprovalAuthority().subscribe(response => {
      this.approvalAuthorities = response as IApprovalAuthority[];
    }, error => {
      console.log(error);
    });
  }

  getSBU() {
    this.bgtService.getSBU().subscribe(response => {
      this.SBUs = response as ISBU[];
    }, error => {
      console.log(error);
    });
  }


  ngOnInit() {
    this.createbgtOwnForm();
    this.reset();
    this.getEmployeeId();
    this.getEmployee();
    this.getSBU();
    this.getApprovalAuthority();
    this.getDonation();
  }
  getDonation() {
    this.bgtService.getDonations().subscribe(response => {
      this.donations = response as IDonation[];
    }, error => {
      console.log(error);
    });
  }
  getEmployeeId() {
    this.empId = this.accountService.getEmployeeId();
    this.userRole = this.accountService.getUserRole();
  }
  getEmployee() {
    this.bgtService.getAllEmp().subscribe(response => {
      this.employees = response as IEmployeeInfo[];

    }, error => {
      console.log(error);
    });
  }

  // insertbgtOwn() {

  //   alert(this.bgtOwn.value.permEdit);

  //   if (this.bgtOwn.getRawValue().deptId == "" || this.bgtOwn.getRawValue().deptId == null) {
  //     this.toastr.error('Select Department');
  //     return;
  //   }
  //   if (this.bgtOwn.value.sbu == "" || this.bgtOwn.value.sbu == null) {
  //     this.toastr.error('Select SBU');
  //     return;
  //   }
  //   if (this.bgtOwn.value.year == "" || this.bgtOwn.value.year == null) {
  //     this.toastr.error('Enter Year');
  //     return;
  //   }
  //   if (this.bgtOwn.value.amount == "" || this.bgtOwn.value.amount == null) {
  //     this.toastr.error('Amount Can not be 0');
  //     return;
  //   }


  //   // this.pendingService.medDispatchFormData.investmentInitId = this.medDispatchForm.value.investmentInitId;
  //   // this.pendingService.medDispatchFormData.issueReference = this.medDispatchForm.value.issueReference;
  //   // this.pendingService.medDispatchFormData.issueDate = this.medDispatchForm.value.issueDate;
  //   // this.pendingService.medDispatchFormData.sapRefNo = this.medDispatchForm.value.issueReference;
  //   // this.pendingService.medDispatchFormData.payRefNo = this.medDispatchForm.value.payRefNo;
  //   // this.pendingService.medDispatchFormData.depotName = "";
  //   // this.pendingService.medDispatchFormData.depotCode = "";
  //   // this.pendingService.medDispatchFormData.employeeId = parseInt(this.empId);
  //   // this.pendingService.medDispatchFormData.remarks = this.medDispatchForm.value.remarks;
  //   // this.pendingService.medDispatchFormData.dispatchAmt = this.medDispatchForm.value.dispatchAmt;
  //   // this.pendingService.medDispatchFormData.proposeAmt = this.medDispatchForm.value.proposeAmt;


  //   // this.pendingService.insertDispatch(this.pendingService.medDispatchFormData).subscribe(
  //   //   res => {
  //   //     this.SaveMedicineDetail();
  //   //     this.toastr.success('Data Saved successfully', 'Medicine Dispatch') 
  //   //     this.isValid = false;
  //   //   },
  //   //   err => { console.log(err); }
  //   // );
  // }
  insertbgtOwn() {
debugger;
  var a =this.bgtOwns;
  return false;
    if(this.bgtOwn.value.remaining < 0)
    {
      this.toastr.error('There is not enough budget left to be allocated');
      return;
    }

    if(this.bgtOwn.value.deptId == "" || this.bgtOwn.value.deptId == null)
    {
      this.toastr.error('Select Department');
      return;
    }
    if(this.bgtOwn.value.sbu == "" || this.bgtOwn.value.sbu == null)
    {
      this.toastr.error('Select SBU');
      return;
    }
    if(this.bgtOwn.value.year == "" || this.bgtOwn.value.year == null)
    {
      this.toastr.error('Enter Year');
      return;
    }
    if(this.bgtOwn.value.ttlAmount == "" || this.bgtOwn.value.ttlAmount == null)
    {
      this.toastr.error('Amount Can not be 0');
      return;
    }

    var yr = new Date(this.bgtOwn.value.year);
    
    this.bgtService.bgtEmpFormData.deptId = this.bgtOwn.value.deptId;
    this.bgtService.bgtEmpFormData.year = yr.getFullYear();
    this.bgtService.bgtEmpFormData.sbu = this.bgtOwn.value.sbu;
    this.bgtService.bgtEmpFormData.authId = this.bgtOwn.value.authId;
    this.bgtService.bgtEmpFormData.amount = this.bgtOwn.value.ttlAmount;
    this.bgtService.bgtEmpFormData.segment = this.bgtOwn.value.segment;
    this.bgtService.bgtEmpFormData.permEdit = this.bgtOwn.value.permEdit;
    this.bgtService.bgtEmpFormData.permView = this.bgtOwn.value.permView;
    this.bgtService.bgtEmpFormData.enteredBy = parseInt(this.empId);

    this.bgtService.insertBgtEmp(this.bgtService.bgtEmpFormData).subscribe(
      res => {
        this.toastr.success('Master Budget Data Saved successfully', 'Budget Dispatch') 
        //this.btnShow = false;
      },
      err => { console.log(err); }
    );
  }
  // modifyData(selectedRecord: IMedicineDispatchDtl)
  // {
  //   this.valShow = false;
  //   this.isHide = true;
  //   this.medDispatchForm.patchValue({
  //     productName: selectedRecord.productName,
  //     productId: selectedRecord.productId,
  //     originVal:  selectedRecord.tpVat,
  //     originQty: selectedRecord.boxQuantity,
  //     dispVal:  selectedRecord.dispatchTpVat,
  //     dispQty:  selectedRecord.dispatchQuantity,
  //     // formControlName2: myValue2 (can be omitted)
  //   });

  //   const index = this.investmentMedicineProds.indexOf(selectedRecord);
  //   this.investmentMedicineProds.splice(index, 1);
  // }

  private formatDate(date) {
    const d = new Date(date);
    let month = '' + (d.getMonth() + 1);
    let day = '' + d.getDate();
    const year = d.getFullYear();
    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;
    return [day, month, year].join('-');
  }


  resetSearch() {
    this.searchText = '';
  }

  openPendingListModal(template: TemplateRef<any>) {
    this.pendingListModalRef = this.modalService.show(template, this.config);
  }

  reset() {
    this.isValid = true;
    this.valShow = true;
    this.isHide = false;
    this.bgtOwn.setValue({
      deptId: "",
      year: "",
      authId: "",
      sbu: "",
      donationId: "",
      donationAmt: "",
      transLimit: "",
      totalAmount: "",
      totalExpense: "",
      totalPipeline: "",
      segment: "",
      permEdit: "",
      permView: "",
      prevAllocate: "",
      remaining: "",
      employee: "",
    });
    this.bgtOwns=[];
  }

}
export interface IBgtOwn {
    compId :number;
    deptId :number;
    year :number;
    month :number;
    employeeId :number;
    SBU :string;
    donationId :number;
    amount :any;
    amtLimit :any;
    segment :string;
    newAmount:any;
    expense:any;
    pipeLine:any;
}
