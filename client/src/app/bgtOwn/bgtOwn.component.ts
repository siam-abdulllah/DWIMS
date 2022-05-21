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
  regions: IRegion[];
  zones: IZone[];
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
  createbgtOwnForm() {
    this.bgtOwn = new FormGroup({
      deptId: new FormControl({ value: '', disabled: true }, [Validators.required]),
      year: new FormControl('', [Validators.required]),
      authId: new FormControl({ value: '', disabled: true }, [Validators.required]),
      sbu: new FormControl('', [Validators.required]),
      totalAmount: new FormControl({ value: '', disabled: true }),
      donationId: new FormControl(''),
      donationAmt: new FormControl(''),
      transLimit: new FormControl(''),
      totalExpense: new FormControl(''),
      totalPipeline: new FormControl(''),
      donationExp: new FormControl({ value: '', disabled: true },),
      donationPipeLine: new FormControl({ value: '', disabled: true },),
      donationRemain: new FormControl({ value: '', disabled: true },),
      segment: new FormControl(''),
      permEdit: new FormControl(''),
      permView: new FormControl(''),
      prevAllocate: new FormControl(''),
      remaining: new FormControl(''),
      employee: new FormControl(''),
    });
  }
  amountCal() {
    if (this.bgtOwn.value.totalAmount != 0 && this.bgtOwn.value.totalAmount != "" || this.bgtOwn.value.totalAmount != undefined) {
      this.bgtOwn.patchValue({
        remaining: parseInt(this.bgtOwn.value.prevAllocate) - parseInt(this.bgtOwn.value.totalAmount),
      });
    }
  }
  donAmountCal(selectedRecord: IBgtOwn) {
    debugger;

    if (selectedRecord.newAmount != 0 && selectedRecord.newAmount != "" || selectedRecord.newAmount != undefined) {
      var sum=0;
      for (let i = 0; i < this.bgtOwns.length; i++) {
        this.bgtOwns[i].newAmount = this.bgtOwns[i].amount;
        this.bgtOwns[i].newAmountLimit = this.bgtOwns[i].amtLimit;
        sum = sum + this.bgtOwns[i].newAmount;
      }
      this.bgtOwn.patchValue({
        totalAmount: (sum - parseInt(selectedRecord.amount))+ parseInt(selectedRecord.newAmount),
        remaining: this.bgtOwn.value.prevAllocate - this.bgtOwn.getRawValue().totalAmount
      });
      //this.bgtOwn.value.totalAmount = (this.bgtOwn.value.totalAmount - selectedRecord.amount) + selectedRecord.newAmount
      //this.bgtOwn.value.remaining = this.bgtOwn.value.prevAllocate - this.bgtOwn.value.totalAmount
      if (this.bgtOwn.value.remaining < 0) {
        this.toastr.warning('Donation wise budget can not be greater than total budget')
      }
    }
    // if (this.bgtOwn.value.totalAmount != 0 && this.bgtOwn.value.totalAmount != "" || this.bgtOwn.value.totalAmount != undefined) {
    //   this.bgtOwn.patchValue({
    //     remaining: parseInt(this.bgtOwn.value.prevAllocate) - parseInt(this.bgtOwn.value.totalAmount),
    //   });
    // }
  }
  customSearchFnEmp(term: string, item: any) {
    term = term.toLocaleLowerCase();
    return item.employeeSAPCode.toLocaleLowerCase().indexOf(term) > -1 ||
      item.employeeName.toLocaleLowerCase().indexOf(term) > -1;
  }
  onChangeAuthority() {
    if (this.bgtOwn.value.authId == 3) {

    }
    else if (this.bgtOwn.value.authId == 4) {

    }
    else {

    }
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
  onChangeDonation() {
    debugger;
    if (this.bgtOwn.value.sbu == "" || this.bgtOwn.value.sbu == null) {
      this.toastr.error('Select SBU');
      this.bgtOwn.patchValue({
        donationId: "",
      });
      return;
    }
    if (this.bgtOwn.value.employee == "" || this.bgtOwn.value.employee == null) {
      this.toastr.error('Select employee');
      this.bgtOwn.patchValue({
        donationId: "",
      });
      return;
    }
    if (this.bgtOwn.value.year == "" || this.bgtOwn.value.year == null) {
      this.toastr.error('Select year');
      this.bgtOwn.patchValue({
        donationId: "",
      });
      return;
    }
    for (let i = 0; i < this.bgtOwns.length; i++) {
      if (this.bgtOwns[i].donationId == this.bgtOwn.value.donationId) {
        this.toastr.error('Donation already existed');
        this.bgtOwn.patchValue({
          donationId: "",
        });
        return;
      }
    }
    var yr = new Date(this.bgtOwn.value.year);
    yr.getFullYear();

    this.bgtService.getDonWiseBgt(this.bgtOwn.value.employee, this.bgtOwn.value.sbu, yr.getFullYear(), 1000, this.bgtOwn.getRawValue().deptId, this.bgtOwn.value.donationId).subscribe(response => {
      this.bgtOwn.patchValue({
        transLimit: response[0].amtLimit,
        donationAmt: response[0].Amount,
        //totalAmount: response[0].amount,
        //donationRemain: this.bgtOwn.value.prevAllocate-this.bgtOwn.value.totalAmount,

      });
    }, error => {
      console.log(error);
    });

    this.bgtService.getEmpWiseTotExp(this.bgtOwn.value.employee, this.bgtOwn.value.sbu, yr.getFullYear(), 1000, this.bgtOwn.getRawValue().deptId, this.bgtOwn.getRawValue().authId).subscribe(response => {
      debugger;
      this.bgtOwn.patchValue({
        totalExpense: response[0].count
      });
    }, error => {
      console.log(error);
    });

    // this.bgtService.getEmpWiseTotPipe(this.bgtOwn.value.employee, this.bgtOwn.value.sbu,yr.getFullYear(),1000,this.bgtOwn.getRawValue().deptId).subscribe(response => {
    //   debugger;
    //   this.bgtOwn.patchValue({
    //     totalPipeline:response[0].count
    //   });
    // }, error => {
    //   console.log(error);
    // });
    this.bgtService.getEmpOwnBgt(this.bgtOwn.value.employee, this.bgtOwn.value.sbu, yr.getFullYear(), 1000, this.bgtOwn.getRawValue().deptId, this.bgtOwn.getRawValue().authId).subscribe(response => {
      debugger;
      this.bgtOwns = response as IBgtOwn[];
      for (let i = 0; i < this.bgtOwns.length; i++) {
        this.bgtOwns[i].newAmount = this.bgtOwns[i].amount;
        this.bgtOwns[i].newAmountLimit = this.bgtOwns[i].amtLimit;
      }
    }, error => {
      console.log(error);
    });

  }
  onChangeYear() {
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
    this.bgtService.getEmpWiseBgt(this.bgtOwn.value.employee, this.bgtOwn.value.sbu, yr.getFullYear(), 1000, this.bgtOwn.getRawValue().deptId, this.bgtOwn.getRawValue().authId).subscribe(response => {
      this.bgtOwn.patchValue({
        prevAllocate: response[0].amount,
        //totalAmount: response[0].amount,
        //remaining: this.bgtOwn.value.prevAllocate - this.bgtOwn.value.totalAmount,
        permEdit: response[0].permEdit,
        permView: response[0].permView,
      });
      this.bgtService.getEmpWiseTotExp(this.bgtOwn.value.employee, this.bgtOwn.value.sbu, yr.getFullYear(), 1000, this.bgtOwn.getRawValue().deptId, this.bgtOwn.getRawValue().authId).subscribe(response => {
        this.bgtOwn.patchValue({
          prevAllocate: response[0].amount,
          //totalAmount: response[0].amount,
          //remaining: this.bgtOwn.value.prevAllocate - this.bgtOwn.value.totalAmount,
          permEdit: response[0].permEdit,
          permView: response[0].permView,
        });

      }, error => {
        console.log(error);
      });
    }, error => {
      console.log(error);
    });

    // this.bgtService.getEmpWiseTotExp(this.bgtOwn.value.employee, this.bgtOwn.value.sbu,yr.getFullYear(),1000,this.bgtOwn.getRawValue().deptId,this.bgtOwn.getRawValue().authId).subscribe(response => {
    //   debugger;
    //   this.bgtOwn.patchValue({
    //     totalExpense:response[0].count
    //   });
    // }, error => {
    //   console.log(error);
    // });

    // this.bgtService.getEmpWiseTotPipe(this.bgtOwn.value.employee, this.bgtOwn.value.sbu,yr.getFullYear(),1000,this.bgtOwn.getRawValue().deptId).subscribe(response => {
    //   debugger;
    //   this.bgtOwn.patchValue({
    //     totalPipeline:response[0].count
    //   });
    // }, error => {
    //   console.log(error);
    // });
    this.bgtService.getEmpOwnBgt(this.bgtOwn.value.employee, this.bgtOwn.value.sbu, yr.getFullYear(), 1000, this.bgtOwn.getRawValue().deptId, this.bgtOwn.getRawValue().authId).subscribe(response => {
      debugger;
      var sum = 0;
      this.bgtOwns = response as IBgtOwn[];
      for (let i = 0; i < this.bgtOwns.length; i++) {
        this.bgtOwns[i].newAmount = this.bgtOwns[i].amount;
        this.bgtOwns[i].newAmountLimit = this.bgtOwns[i].amtLimit;
        sum = sum + this.bgtOwns[i].newAmount;
      }
      this.bgtOwn.patchValue({
        totalAmount: sum,
        remaining: this.bgtOwn.value.prevAllocate - sum,
      });

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

    // if (this.bgtOwn.value.segment == "" || this.bgtOwn.value.segment == null) {
    //   this.toastr.error('Select Segmentation');
    //   return;
    // }

    //if (this.bgtOwn.value.segment == "Monthly") {
    // const d = new Date();

    // var remMonth = 12 - d.getMonth() - 1;
    // var ttlAloc = this.bgtOwn.value.ttlPerson * this.bgtOwn.value.ttlAmount * remMonth;

    // this.bgtOwn.patchValue({
    //   ttlAllocate: ttlAloc,
    // });
    //}
    //else if (this.bgtOwn.value.segment == "Yearly") {
    // var ttlAloc = this.bgtOwn.value.ttlPerson * this.bgtOwn.value.ttlAmount;

    // this.bgtOwn.patchValue({
    //   ttlAllocate: ttlAloc,
    // });
    //}
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
  insertbgtEmpDetail() {
    debugger;
    var a = this.bgtOwns;
    // if(this.bgtOwn.value.remaining <= 0)
    // {
    //   this.toastr.error('There is not enough budget left to be allocated');
    //   return;
    // }

    if (this.bgtOwn.getRawValue().deptId == "" || this.bgtOwn.getRawValue().deptId == null) {
      this.toastr.error('Select Department');
      return;
    }
    if (this.bgtOwn.value.sbu == "" || this.bgtOwn.value.sbu == null) {
      this.toastr.error('Select SBU');
      return;
    }
    if (this.bgtOwn.value.year == "" || this.bgtOwn.value.year == null) {
      this.toastr.error('Enter Year');
      return;
    }
    if (this.bgtOwn.value.totalAmount == "" || this.bgtOwn.value.totalAmount == null) {
      this.toastr.error('Amount Can not be 0');
      return;
    }

    var yr = new Date(this.bgtOwn.value.year);

    this.bgtService.bgtEmpFormData.compId = 1000;
    this.bgtService.bgtEmpFormData.deptId = this.bgtOwn.getRawValue().deptId;
    this.bgtService.bgtEmpFormData.year = yr.getFullYear();
    this.bgtService.bgtEmpFormData.sbu = this.bgtOwn.value.sbu;
    this.bgtService.bgtEmpFormData.authId = this.bgtOwn.getRawValue().authId;
    this.bgtService.bgtEmpFormData.amount = this.bgtOwn.value.totalAmount;
    this.bgtService.bgtEmpFormData.employeeId = this.bgtOwn.value.employee;
    this.bgtService.bgtEmpFormData.permEdit = this.bgtOwn.value.permEdit;
    this.bgtService.bgtEmpFormData.permView = this.bgtOwn.value.permView;
    this.bgtService.bgtEmpFormData.enteredBy = parseInt(this.empId);

    this.bgtService.insertBgtEmp(this.bgtService.bgtEmpFormData).subscribe(
      res => {
        this.toastr.success('Master Budget Data Saved successfully', 'Budget Dispatch')

      },
      err => { console.log(err); }
    );
  }
  insertbgtOwnDetail() {
    debugger;

    if (this.bgtOwn.value.donationId != "" && this.bgtOwn.value.donationId != null && this.bgtOwn.value.donationId != undefined) {
      for (let i = 0; i < this.bgtOwns.length; i++) {
        if (this.bgtOwns[i].donationId == this.bgtOwn.value.donationId) {
          this.toastr.error('Donation already existed');
          this.bgtOwn.patchValue({
            donationId: "",
          });
          return;
        }
      }
      // if(this.bgtOwn.value.remaining <= 0)
      // {
      //   this.toastr.error('There is not enough budget left to be allocated');
      //   return;
      // }

      if (this.bgtOwn.getRawValue().deptId == "" || this.bgtOwn.getRawValue().deptId == null) {
        this.toastr.error('Select Department');
        return;
      }
      if (this.bgtOwn.value.sbu == "" || this.bgtOwn.value.sbu == null) {
        this.toastr.error('Select SBU');
        return;
      }
      if (this.bgtOwn.value.year == "" || this.bgtOwn.value.year == null) {
        this.toastr.error('Enter Year');
        return;
      }
      if (this.bgtOwn.value.totalAmount == "" || this.bgtOwn.value.totalAmount == null) {
        this.toastr.error('Amount Can not be 0');
        return;
      }
      if (this.bgtOwn.value.transLimit == "" || this.bgtOwn.value.transLimit == null || this.bgtOwn.value.transLimit == undefined) {
        this.toastr.error('Please Insert Transation Limit');
        return;
      }
      if (this.bgtOwn.value.donationAmt == "" || this.bgtOwn.value.donationAmt == null || this.bgtOwn.value.donationAmt == undefined) {
        this.toastr.error('Please Insert Donation Amount');
        return;
      }
      debugger;
      var yr = new Date(this.bgtOwn.value.year);
      //yr.getFullYear();
      let data = new BgtOwn();
      data.compId = 1000;
      data.deptId = this.bgtOwn.getRawValue().deptId;
      data.authId = this.bgtOwn.getRawValue().authId;
      data.year = yr.getFullYear();
      data.SBU = this.bgtOwn.value.sbu;
      data.donationId = this.bgtOwn.value.donationId;
      data.employeeId = this.bgtOwn.value.employee;
      data.amount = this.bgtOwn.value.donationAmt;
      data.amtLimit = this.bgtOwn.value.transLimit;
      data.segment = this.bgtOwn.value.segment;
      //this.bgtOwns.push(data);
      this.bgtOwns.push({ compId: 1000, deptId: this.bgtOwn.getRawValue().deptId, authId: this.bgtOwn.getRawValue().authId, year: yr.getFullYear(), SBU: this.bgtOwn.value.sbu, donationId: this.bgtOwn.value.donationId, employeeId: this.bgtOwn.value.employee, enteredBy: parseInt(this.empId), amount: this.bgtOwn.value.donationAmt, amtLimit: this.bgtOwn.value.transLimit, segment: this.bgtOwn.value.segment, month: 0, newAmount: this.bgtOwn.value.donationAmt, newAmountLimit: this.bgtOwn.value.transLimit, expense: 0, pipeLine: 0 });
    }
    for (let i = 0; i < this.bgtOwns.length; i++) {
      this.bgtOwns[i].employeeId = this.bgtOwn.value.employee;
      this.bgtOwns[i].amount = this.bgtOwns[i].newAmount;
      this.bgtOwns[i].amtLimit = this.bgtOwns[i].newAmountLimit;
    }
    this.bgtService.insertBgtOwn(this.bgtOwns).subscribe(
      res => {
        this.toastr.success('Detail Budget Data Saved successfully', 'Budget Dispatch')

      },
      err => {
        debugger;
        console.log(err);
        alert(err);
      }
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
      donationExp: "",
      donationPipeLine: "",
      donationRemain: "",
      segment: "",
      permEdit: "",
      permView: "",
      prevAllocate: "",
      remaining: "",
      employee: "",
    });
    this.bgtOwns = [];
  }
  tranCal(selectedRecord: IBgtOwn) {
    selectedRecord.amount = selectedRecord.newAmount;
  }
}
export interface IBgtOwn {
  compId: number;
  deptId: number;
  authId: number;
  year: number;
  month: number;
  employeeId: number;
  enteredBy: number;
  SBU: string;
  donationId: number;
  amount: any;
  amtLimit: any;
  segment: string;
  newAmount: any;
  newAmountLimit: any;
  expense: any;
  pipeLine: any;
}
export class BgtOwn implements IBgtOwn {
  compId: number;
  deptId: number;
  authId: number;
  year: number;
  month: number;
  employeeId: number;
  enteredBy: number;
  SBU: string;
  donationId: number;
  amount: any;
  amtLimit: any;
  segment: string;
  newAmount: any;
  newAmountLimit: any;
  expense: any;
  pipeLine: any;
}
export interface IRegion {

  sBU: string;
  sBUName: string;
  regionCode: string;
  regionName: any;
  serial: any;
  tagCode: any;
}
export interface IZone {

  sBU: string;
  sBUName: string;
  zoneCode: string;
  zoneName: any;
  serial: any;
  tagCode: any;
}
