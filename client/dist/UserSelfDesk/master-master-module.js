(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["master-master-module"],{

/***/ "1VYe":
/*!*******************************************!*\
  !*** ./src/app/shared/models/campaign.ts ***!
  \*******************************************/
/*! exports provided: Campaign */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "Campaign", function() { return Campaign; });
class Campaign {
    constructor() {
        this.id = 0;
        this.status = "Active";
    }
}


/***/ }),

/***/ "EbiW":
/*!********************************************************!*\
  !*** ./src/app/shared/models/subCampaignPagination.ts ***!
  \********************************************************/
/*! exports provided: SubCampaignPagination */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SubCampaignPagination", function() { return SubCampaignPagination; });
class SubCampaignPagination {
    constructor() {
        this.data = [];
    }
}


/***/ }),

/***/ "KNdb":
/*!*****************************************************!*\
  !*** ./src/app/shared/models/donationPagination.ts ***!
  \*****************************************************/
/*! exports provided: DonationPagination */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DonationPagination", function() { return DonationPagination; });
class DonationPagination {
    constructor() {
        this.data = [];
    }
}


/***/ }),

/***/ "KmaA":
/*!**********************************************!*\
  !*** ./src/app/shared/models/subCampaign.ts ***!
  \**********************************************/
/*! exports provided: SubCampaign */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SubCampaign", function() { return SubCampaign; });
class SubCampaign {
    constructor() {
        this.id = 0;
        this.status = "Active";
    }
}


/***/ }),

/***/ "Nm5a":
/*!*****************************************!*\
  !*** ./src/app/master/master.module.ts ***!
  \*****************************************/
/*! exports provided: MasterModule */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "MasterModule", function() { return MasterModule; });
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/forms */ "3Pt+");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/common */ "ofXK");
/* harmony import */ var _donation_donation_component__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./donation/donation.component */ "bpRh");
/* harmony import */ var _subCampaign_subCampaign_component__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./subCampaign/subCampaign.component */ "ok+D");
/* harmony import */ var _master_routing_module__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./master-routing.module */ "tNAB");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @angular/core */ "fXoL");






class MasterModule {
}
MasterModule.ɵmod = _angular_core__WEBPACK_IMPORTED_MODULE_5__["ɵɵdefineNgModule"]({ type: MasterModule });
MasterModule.ɵinj = _angular_core__WEBPACK_IMPORTED_MODULE_5__["ɵɵdefineInjector"]({ factory: function MasterModule_Factory(t) { return new (t || MasterModule)(); }, imports: [[
            _angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"],
            _master_routing_module__WEBPACK_IMPORTED_MODULE_4__["MasterRoutingModule"],
            _angular_forms__WEBPACK_IMPORTED_MODULE_0__["FormsModule"]
        ]] });
(function () { (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_5__["ɵɵsetNgModuleScope"](MasterModule, { declarations: [_donation_donation_component__WEBPACK_IMPORTED_MODULE_2__["DonationComponent"], _subCampaign_subCampaign_component__WEBPACK_IMPORTED_MODULE_3__["SubCampaignComponent"]], imports: [_angular_common__WEBPACK_IMPORTED_MODULE_1__["CommonModule"],
        _master_routing_module__WEBPACK_IMPORTED_MODULE_4__["MasterRoutingModule"],
        _angular_forms__WEBPACK_IMPORTED_MODULE_0__["FormsModule"]] }); })();


/***/ }),

/***/ "bpRh":
/*!*******************************************************!*\
  !*** ./src/app/master/donation/donation.component.ts ***!
  \*******************************************************/
/*! exports provided: DonationComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DonationComponent", function() { return DonationComponent; });
/* harmony import */ var _shared_models_donation__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./../../shared/models/donation */ "mAM3");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "fXoL");
/* harmony import */ var _master_service__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../master.service */ "napU");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/router */ "tyNb");
/* harmony import */ var ngx_toastr__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ngx-toastr */ "5eHb");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @angular/forms */ "3Pt+");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! @angular/common */ "ofXK");







const _c0 = ["search"];
function DonationComponent_tr_36_Template(rf, ctx) { if (rf & 1) {
    const _r7 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "td", 17);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function DonationComponent_tr_36_Template_td_click_7_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r7); const donation_r5 = ctx.$implicit; const ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r6.populateForm(donation_r5); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](8, " Details ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const donation_r5 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", donation_r5.donationTypeName, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", donation_r5.status, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", donation_r5.remarks, " ");
} }
class DonationComponent {
    constructor(masterService, router, toastr) {
        this.masterService = masterService;
        this.router = router;
        this.toastr = toastr;
        this.totalCount = 0;
    }
    ngOnInit() {
        debugger;
        alert("sdasd");
        //this.getDonation();
    }
    getDonation() {
        this.masterService.getDonation().subscribe(response => {
            this.donations = response.data;
            this.totalCount = response.count;
        }, error => {
            console.log(error);
        });
    }
    onSubmit(form) {
        debugger;
        if (this.masterService.donationFormData.id == 0)
            this.insertDonation(form);
        else
            this.updateDonation(form);
    }
    insertDonation(form) {
        this.masterService.insertDonation().subscribe(res => {
            debugger;
            this.resetForm(form);
            this.getDonation();
            this.toastr.success('Submitted successfully', 'Payment Detail Register');
        }, err => { console.log(err); });
    }
    updateDonation(form) {
        this.masterService.updateDonation().subscribe(res => {
            debugger;
            this.resetForm(form);
            this.getDonation();
            this.toastr.info('Updated successfully', 'Payment Detail Register');
        }, err => { console.log(err); });
    }
    populateForm(selectedRecord) {
        this.masterService.donationFormData = Object.assign({}, selectedRecord);
    }
    resetForm(form) {
        form.form.reset();
        this.masterService.donationFormData = new _shared_models_donation__WEBPACK_IMPORTED_MODULE_0__["Donation"]();
    }
}
DonationComponent.ɵfac = function DonationComponent_Factory(t) { return new (t || DonationComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_master_service__WEBPACK_IMPORTED_MODULE_2__["MasterService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_angular_router__WEBPACK_IMPORTED_MODULE_3__["Router"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](ngx_toastr__WEBPACK_IMPORTED_MODULE_4__["ToastrService"])); };
DonationComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdefineComponent"]({ type: DonationComponent, selectors: [["app-donation"]], viewQuery: function DonationComponent_Query(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵviewQuery"](_c0, 1);
    } if (rf & 2) {
        let _t;
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵloadQuery"]()) && (ctx.searchTerm = _t.first);
    } }, decls: 38, vars: 12, consts: [[1, "d-flex", "justify-content-center", "mt-4"], [1, "col-md-6"], ["novalidate", "", "autocomplete", "off", 3, "submit"], ["form", "ngForm"], ["type", "hidden", "name", "id", 3, "value"], [1, "form-group"], ["placeholder", "Donation", "name", "donationTypeName", "required", "", 1, "form-control", "form-control-lg", 3, "ngModel", "ngModelChange"], ["donationTypeName", "ngModel"], ["name", "", "id", "", "name", "status", "required", "", 1, "form-control", "form-control-lg", 3, "ngModel", "ngModelChange"], ["status", "ngModel"], ["value", "Active"], ["value", "Inactive"], ["placeholder", "Remarks", "name", "remarks", "required", "", 1, "form-control", "form-control-lg", 3, "ngModel", "ngModelChange"], ["remarks", "ngModel"], ["type", "submit", 1, "btn", "btn-info", "btn-lg", "btn-block", 3, "disabled"], [1, "table"], [4, "ngFor", "ngForOf"], [3, "click"]], template: function DonationComponent_Template(rf, ctx) { if (rf & 1) {
        const _r8 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "form", 2, 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("submit", function DonationComponent_Template_form_submit_2_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r8); const _r0 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](3); return ctx.onSubmit(_r0); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](4, "input", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](7, "Donation");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "input", 6, 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function DonationComponent_Template_input_ngModelChange_8_listener($event) { return ctx.masterService.donationFormData.donationTypeName = $event; });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](12, "Status");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "select", 8, 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function DonationComponent_Template_select_ngModelChange_13_listener($event) { return ctx.masterService.donationFormData.status = $event; });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "option", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](16, "Active");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "option", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](18, "Inactive");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](20, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](21, "Remarks");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](22, "textarea", 12, 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function DonationComponent_Template_textarea_ngModelChange_22_listener($event) { return ctx.masterService.donationFormData.remarks = $event; });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](24, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "button", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](26, "SUBMIT");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](28, "table", 15);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](29, "tr");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](30, "td");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](31, "Donation");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](32, "td");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](33, "Status");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](34, "td");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](35, "Remarks");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](36, DonationComponent_tr_36_Template, 9, 3, "tr", 16);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](37, "div");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    } if (rf & 2) {
        const _r0 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](3);
        const _r1 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](9);
        const _r2 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](14);
        const _r3 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](23);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("value", ctx.masterService.donationFormData.id);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵclassProp"]("invalid", _r1.invalid && _r1.touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", ctx.masterService.donationFormData.donationTypeName);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵclassProp"]("invalid", _r2.invalid && _r2.touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", ctx.masterService.donationFormData.status);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵclassProp"]("invalid", _r3.invalid && _r3.touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", ctx.masterService.donationFormData.remarks);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("disabled", _r0.invalid);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](11);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx.donations);
    } }, directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["ɵangular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["NgForm"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["RequiredValidator"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["NgModel"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["SelectControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["NgSelectOption"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["ɵangular_packages_forms_forms_x"], _angular_common__WEBPACK_IMPORTED_MODULE_6__["NgForOf"]], encapsulation: 2 });


/***/ }),

/***/ "doBC":
/*!************************************************!*\
  !*** ./src/app/shared/models/genericParams.ts ***!
  \************************************************/
/*! exports provided: GenericParams */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "GenericParams", function() { return GenericParams; });
class GenericParams {
    constructor() {
        this.sort = '';
        this.pageNumber = 1;
        this.pageSize = 10;
        this.search = '';
    }
}


/***/ }),

/***/ "mAM3":
/*!*******************************************!*\
  !*** ./src/app/shared/models/donation.ts ***!
  \*******************************************/
/*! exports provided: Donation */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "Donation", function() { return Donation; });
class Donation {
    constructor() {
        this.id = 0;
        this.status = "Active";
    }
}


/***/ }),

/***/ "napU":
/*!******************************************!*\
  !*** ./src/app/master/master.service.ts ***!
  \******************************************/
/*! exports provided: MasterService */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "MasterService", function() { return MasterService; });
/* harmony import */ var _shared_models_donationPagination__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./../shared/models/donationPagination */ "KNdb");
/* harmony import */ var _shared_models_subCampaignPagination__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./../shared/models/subCampaignPagination */ "EbiW");
/* harmony import */ var _shared_models_campaignPagination__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./../shared/models/campaignPagination */ "oTxW");
/* harmony import */ var _environments_environment__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../../environments/environment */ "AytR");
/* harmony import */ var _shared_models_donation__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ../shared/models/donation */ "mAM3");
/* harmony import */ var _shared_models_subCampaign__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ../shared/models/subCampaign */ "KmaA");
/* harmony import */ var _shared_models_campaign__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ../shared/models/campaign */ "1VYe");
/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! @angular/common/http */ "tk/3");
/* harmony import */ var rxjs_operators__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! rxjs/operators */ "kU1M");
/* harmony import */ var _shared_models_genericParams__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! ../shared/models/genericParams */ "doBC");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! @angular/core */ "fXoL");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! @angular/router */ "tyNb");













class MasterService {
    constructor(http, router) {
        this.http = http;
        this.router = router;
        this.roles = [];
        this.donations = [];
        this.donationPagination = new _shared_models_donationPagination__WEBPACK_IMPORTED_MODULE_0__["DonationPagination"]();
        this.subCampaigns = [];
        this.subCampaignPagination = new _shared_models_subCampaignPagination__WEBPACK_IMPORTED_MODULE_1__["SubCampaignPagination"]();
        this.campaigns = [];
        this.campaignPagination = new _shared_models_campaignPagination__WEBPACK_IMPORTED_MODULE_2__["CampaignPagination"]();
        this.baseUrl = _environments_environment__WEBPACK_IMPORTED_MODULE_3__["environment"].apiUrl;
        this.genParams = new _shared_models_genericParams__WEBPACK_IMPORTED_MODULE_9__["GenericParams"]();
        this.donationFormData = new _shared_models_donation__WEBPACK_IMPORTED_MODULE_4__["Donation"]();
        this.subCampaignFormData = new _shared_models_subCampaign__WEBPACK_IMPORTED_MODULE_5__["SubCampaign"]();
        this.campaignFormData = new _shared_models_campaign__WEBPACK_IMPORTED_MODULE_6__["Campaign"]();
    }
    getCampaign() {
        let params = new _angular_common_http__WEBPACK_IMPORTED_MODULE_7__["HttpParams"]();
        debugger;
        if (this.genParams.search) {
            params = params.append('search', this.genParams.search);
        }
        params = params.append('sort', this.genParams.sort);
        params = params.append('pageIndex', this.genParams.pageNumber.toString());
        params = params.append('pageSize', this.genParams.pageSize.toString());
        return this.http.get(this.baseUrl + 'campaign/campaigns', { observe: 'response', params })
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_8__["map"])(response => {
            this.campaigns = [...this.campaigns, ...response.body.data];
            this.campaignPagination = response.body;
            return this.campaignPagination;
        }));
    }
    insertCampaign() {
        return this.http.post(this.baseUrl + 'campaign/insert', this.campaignFormData);
        // return this.http.post(this.baseUrl + 'account/register', values).pipe(
        //   map((user: IUser) => {
        //     if (user) {
        //       // localStorage.setItem('token', user.token);
        //       // this.currentUserSource.next(user);
        //       return user;
        //     }
        //   })
        // );
    }
    updateCampaign() {
        return this.http.post(this.baseUrl + 'campaign/update', this.campaignFormData);
    }
    getSubCampaign() {
        let params = new _angular_common_http__WEBPACK_IMPORTED_MODULE_7__["HttpParams"]();
        debugger;
        if (this.genParams.search) {
            params = params.append('search', this.genParams.search);
        }
        params = params.append('sort', this.genParams.sort);
        params = params.append('pageIndex', this.genParams.pageNumber.toString());
        params = params.append('pageSize', this.genParams.pageSize.toString());
        return this.http.get(this.baseUrl + 'subCampaign/subCampaigns', { observe: 'response', params })
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_8__["map"])(response => {
            this.subCampaigns = [...this.subCampaigns, ...response.body.data];
            this.subCampaignPagination = response.body;
            return this.subCampaignPagination;
        }));
    }
    insertSubCampaign() {
        return this.http.post(this.baseUrl + 'subCampaign/insert', this.subCampaignFormData);
        // return this.http.post(this.baseUrl + 'account/register', values).pipe(
        //   map((user: IUser) => {
        //     if (user) {
        //       // localStorage.setItem('token', user.token);
        //       // this.currentUserSource.next(user);
        //       return user;
        //     }
        //   })
        // );
    }
    updateSubCampaign() {
        return this.http.post(this.baseUrl + 'subCampaign/update', this.subCampaignFormData);
    }
    getDonation() {
        let params = new _angular_common_http__WEBPACK_IMPORTED_MODULE_7__["HttpParams"]();
        debugger;
        if (this.genParams.search) {
            params = params.append('search', this.genParams.search);
        }
        params = params.append('sort', this.genParams.sort);
        params = params.append('pageIndex', this.genParams.pageNumber.toString());
        params = params.append('pageSize', this.genParams.pageSize.toString());
        return this.http.get(this.baseUrl + 'donation/donations', { observe: 'response', params })
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_8__["map"])(response => {
            this.donations = [...this.donations, ...response.body.data];
            this.donationPagination = response.body;
            return this.donationPagination;
        }));
    }
    insertDonation() {
        return this.http.post(this.baseUrl + 'donation/insert', this.donationFormData);
    }
    updateDonation() {
        return this.http.post(this.baseUrl + 'donation/update', this.donationFormData);
    }
}
MasterService.ɵfac = function MasterService_Factory(t) { return new (t || MasterService)(_angular_core__WEBPACK_IMPORTED_MODULE_10__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_7__["HttpClient"]), _angular_core__WEBPACK_IMPORTED_MODULE_10__["ɵɵinject"](_angular_router__WEBPACK_IMPORTED_MODULE_11__["Router"])); };
MasterService.ɵprov = _angular_core__WEBPACK_IMPORTED_MODULE_10__["ɵɵdefineInjectable"]({ token: MasterService, factory: MasterService.ɵfac, providedIn: 'root' });


/***/ }),

/***/ "oTxW":
/*!*****************************************************!*\
  !*** ./src/app/shared/models/campaignPagination.ts ***!
  \*****************************************************/
/*! exports provided: CampaignPagination */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "CampaignPagination", function() { return CampaignPagination; });
class CampaignPagination {
    constructor() {
        this.data = [];
    }
}


/***/ }),

/***/ "ok+D":
/*!*************************************************************!*\
  !*** ./src/app/master/subCampaign/subCampaign.component.ts ***!
  \*************************************************************/
/*! exports provided: SubCampaignComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SubCampaignComponent", function() { return SubCampaignComponent; });
/* harmony import */ var _shared_models_subCampaign__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../../shared/models/subCampaign */ "KmaA");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "fXoL");
/* harmony import */ var _master_service__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../master.service */ "napU");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/router */ "tyNb");
/* harmony import */ var ngx_toastr__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ngx-toastr */ "5eHb");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @angular/forms */ "3Pt+");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! @angular/common */ "ofXK");







const _c0 = ["search"];
function SubCampaignComponent_tr_36_Template(rf, ctx) { if (rf & 1) {
    const _r7 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "tr");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](3, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "td");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](6);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](7, "td", 17);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function SubCampaignComponent_tr_36_Template_td_click_7_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r7); const subCampaign_r5 = ctx.$implicit; const ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return ctx_r6.populateForm(subCampaign_r5); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](8, " Details ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const subCampaign_r5 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", subCampaign_r5.subCampaignName, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", subCampaign_r5.status, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate1"](" ", subCampaign_r5.remarks, " ");
} }
class SubCampaignComponent {
    constructor(masterService, router, toastr) {
        this.masterService = masterService;
        this.router = router;
        this.toastr = toastr;
        this.totalCount = 0;
    }
    ngOnInit() {
        this.getSubCampaign();
    }
    getSubCampaign() {
        this.masterService.getSubCampaign().subscribe(response => {
            this.subCampaigns = response.data;
            this.totalCount = response.count;
        }, error => {
            console.log(error);
        });
    }
    onSubmit(form) {
        debugger;
        if (this.masterService.subCampaignFormData.id == 0)
            this.insertSubCampaign(form);
        else
            this.updateSubCampaign(form);
    }
    insertSubCampaign(form) {
        this.masterService.insertSubCampaign().subscribe(res => {
            debugger;
            this.resetForm(form);
            this.getSubCampaign();
            this.toastr.success('Submitted successfully', 'Payment Detail Register');
        }, err => { console.log(err); });
    }
    updateSubCampaign(form) {
        this.masterService.updateSubCampaign().subscribe(res => {
            debugger;
            this.resetForm(form);
            this.getSubCampaign();
            this.toastr.info('Updated successfully', 'Payment Detail Register');
        }, err => { console.log(err); });
    }
    populateForm(selectedRecord) {
        this.masterService.subCampaignFormData = Object.assign({}, selectedRecord);
    }
    resetForm(form) {
        form.form.reset();
        this.masterService.subCampaignFormData = new _shared_models_subCampaign__WEBPACK_IMPORTED_MODULE_0__["SubCampaign"]();
    }
}
SubCampaignComponent.ɵfac = function SubCampaignComponent_Factory(t) { return new (t || SubCampaignComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_master_service__WEBPACK_IMPORTED_MODULE_2__["MasterService"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_angular_router__WEBPACK_IMPORTED_MODULE_3__["Router"]), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](ngx_toastr__WEBPACK_IMPORTED_MODULE_4__["ToastrService"])); };
SubCampaignComponent.ɵcmp = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdefineComponent"]({ type: SubCampaignComponent, selectors: [["app-subCampaign"]], viewQuery: function SubCampaignComponent_Query(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵviewQuery"](_c0, 1);
    } if (rf & 2) {
        let _t;
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵloadQuery"]()) && (ctx.searchTerm = _t.first);
    } }, decls: 38, vars: 12, consts: [[1, "d-flex", "justify-content-center", "mt-4"], [1, "col-md-6"], ["novalidate", "", "autocomplete", "off", 3, "submit"], ["form", "ngForm"], ["type", "hidden", "name", "id", 3, "value"], [1, "form-group"], ["placeholder", "Sub Campaign", "name", "subCampaignName", "required", "", 1, "form-control", "form-control-lg", 3, "ngModel", "ngModelChange"], ["subCampaignName", "ngModel"], ["name", "", "id", "", "name", "status", "required", "", 1, "form-control", "form-control-lg", 3, "ngModel", "ngModelChange"], ["status", "ngModel"], ["value", "Active"], ["value", "Inactive"], ["placeholder", "Remarks", "name", "remarks", 1, "form-control", "form-control-lg", 3, "ngModel", "ngModelChange"], ["remarks", "ngModel"], ["type", "submit", 1, "btn", "btn-info", "btn-lg", "btn-block", 3, "disabled"], [1, "table"], [4, "ngFor", "ngForOf"], [3, "click"]], template: function SubCampaignComponent_Template(rf, ctx) { if (rf & 1) {
        const _r8 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](1, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "form", 2, 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("submit", function SubCampaignComponent_Template_form_submit_2_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r8); const _r0 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](3); return ctx.onSubmit(_r0); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](4, "input", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](5, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](6, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](7, "Sub Campaign");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "input", 6, 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function SubCampaignComponent_Template_input_ngModelChange_8_listener($event) { return ctx.masterService.subCampaignFormData.subCampaignName = $event; });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](10, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](12, "Status");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](13, "select", 8, 9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function SubCampaignComponent_Template_select_ngModelChange_13_listener($event) { return ctx.masterService.subCampaignFormData.status = $event; });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](15, "option", 10);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](16, "Active");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](17, "option", 11);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](18, "Inactive");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](19, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](20, "label");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](21, "Remarks");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](22, "textarea", 12, 13);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function SubCampaignComponent_Template_textarea_ngModelChange_22_listener($event) { return ctx.masterService.subCampaignFormData.remarks = $event; });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](24, "div", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](25, "button", 14);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](26, "SUBMIT");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](27, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](28, "table", 15);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](29, "tr");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](30, "td");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](31, "Sub Campaign");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](32, "td");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](33, "Status");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](34, "td");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](35, "Remarks");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](36, SubCampaignComponent_tr_36_Template, 9, 3, "tr", 16);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelement"](37, "div");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
    } if (rf & 2) {
        const _r0 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](3);
        const _r1 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](9);
        const _r2 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](14);
        const _r3 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵreference"](23);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("value", ctx.masterService.subCampaignFormData.id);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵclassProp"]("invalid", _r1.invalid && _r1.touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", ctx.masterService.subCampaignFormData.subCampaignName);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](5);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵclassProp"]("invalid", _r2.invalid && _r2.touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", ctx.masterService.subCampaignFormData.status);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](9);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵclassProp"]("invalid", _r3.invalid && _r3.touched);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngModel", ctx.masterService.subCampaignFormData.remarks);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("disabled", _r0.invalid);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](11);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx.subCampaigns);
    } }, directives: [_angular_forms__WEBPACK_IMPORTED_MODULE_5__["ɵangular_packages_forms_forms_y"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["NgControlStatusGroup"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["NgForm"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["DefaultValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["RequiredValidator"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["NgControlStatus"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["NgModel"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["SelectControlValueAccessor"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["NgSelectOption"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__["ɵangular_packages_forms_forms_x"], _angular_common__WEBPACK_IMPORTED_MODULE_6__["NgForOf"]], encapsulation: 2 });


/***/ }),

/***/ "tNAB":
/*!*************************************************!*\
  !*** ./src/app/master/master-routing.module.ts ***!
  \*************************************************/
/*! exports provided: MasterRoutingModule */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "MasterRoutingModule", function() { return MasterRoutingModule; });
/* harmony import */ var _donation_donation_component__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./donation/donation.component */ "bpRh");
/* harmony import */ var _subCampaign_subCampaign_component__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./subCampaign/subCampaign.component */ "ok+D");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/router */ "tyNb");
/* harmony import */ var _home_home_component__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../home/home.component */ "9vUh");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @angular/core */ "fXoL");






// import { LoginComponent } from './login/login.component';
// import { RegisterComponent } from './register/register.component';
const routes = [
    // {path: 'login', component: LoginComponent},
    // {path: 'register', component: RegisterComponent, data: {breadcrumb: 'Register User'}},
    // {path: 'register/:id', component: RegisterComponent, data: {breadcrumb: 'User Details'}},
    { path: '', component: _home_home_component__WEBPACK_IMPORTED_MODULE_3__["HomeComponent"], data: { breadcrumb: 'Home' } },
    { path: 'donation', component: _donation_donation_component__WEBPACK_IMPORTED_MODULE_0__["DonationComponent"] },
    { path: 'subCampaign', component: _subCampaign_subCampaign_component__WEBPACK_IMPORTED_MODULE_1__["SubCampaignComponent"] }
];
class MasterRoutingModule {
}
MasterRoutingModule.ɵmod = _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵdefineNgModule"]({ type: MasterRoutingModule });
MasterRoutingModule.ɵinj = _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵdefineInjector"]({ factory: function MasterRoutingModule_Factory(t) { return new (t || MasterRoutingModule)(); }, imports: [[
            _angular_router__WEBPACK_IMPORTED_MODULE_2__["RouterModule"].forChild(routes)
        ], _angular_router__WEBPACK_IMPORTED_MODULE_2__["RouterModule"]] });
(function () { (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵsetNgModuleScope"](MasterRoutingModule, { imports: [_angular_router__WEBPACK_IMPORTED_MODULE_2__["RouterModule"]], exports: [_angular_router__WEBPACK_IMPORTED_MODULE_2__["RouterModule"]] }); })();


/***/ })

}]);
//# sourceMappingURL=master-master-module.js.map