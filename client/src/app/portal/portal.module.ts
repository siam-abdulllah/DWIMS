import { ParamInvestSummaryComponent } from './../paramInvestmentSummary/paramInvestmentSummary.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

import { PortalComponent } from './portal.component';
import { PortalRoutingModule } from './portal-routing.module';
import { ChangePasswordAllComponent } from './../changePasswordAll/changePasswordAll.component';
import { TopnavComponent } from '../mastertheme/topnav/topnav.component';
import { AsidenavComponent } from '../mastertheme/asidenav/asidenav.component';
import { FooterComponent } from '../mastertheme/footer/footer.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Ng2SearchPipeModule } from 'ng2-search-filter';
import {DatePipe} from '@angular/common';
import { MarketGroupComponent } from '../marketGroup/marketGroup.component';
import { RegApprovalComponent } from '../regApproval/regApproval.component';
import { InvestmentInitComponent } from '../investmentInit/investmentInit.component';
import { InvestmentRecComponent } from '../investmentRec/investmentRec.component';
import { InvestmentAprComponent } from '../investmentApr/investmentApr.component';
import { InvestmentAprNoSbuComponent } from './../investmentAprNoSbu/investmentAprNoSbu.component';
import { ReportInvestmentComponent } from './../report-investment/report-investment.component';
import { PendingChqPrintDepotComponent } from '../pendingChqPrintDepot/pendingChqPrintDepot.component';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { NgSelectModule } from '@ng-select/ng-select';
import { MasterModule } from '../master/master.module';
import { MasterRoutingModule } from '../master/master-routing.module';
import { ModalModule } from 'ngx-bootstrap/modal';
import { ApprAuthConfigComponent } from '../apprAuthConfig/apprAuthConfig.component';
import { ApprovalCeilingComponent } from '../approval-ceiling/approval-ceiling.component';
import { ApprovalTimeLimitComponent } from '../approval-time-limit/approval-time-limit.component';
import { SbuWiseBudgetComponent } from '../sbu-wise-budget/sbu-wise-budget.component';
import { DocHonApprComponent } from '../docHonAppr/docHonAppr.component';
import { ClusterInfoComponent } from '../master/cluster-info/cluster-info.component';
import { RptInvestSummaryComponent } from '../rptInvestmentSummary/rptInvestmentSummary.component';
import { RptInvestmentDetailComponent } from '../RptInvestmentDetail/rptInvestmentDetail.component';
import { RptInvestmentDetailSummaryComponent } from '../RptInvDetailForSummary/rptInvDetailForSummary.component'; ;
import { PendingPrintDepotComponent } from '../pendingPrintDepot/pendingPrintDepot.component';
import { PendingMedDispatchComponent } from './../PendingMedDispatch/pendingMedDispatch.component';
import { ChequeTrackComponent } from './../chequeTrack/chequeTrack.component'; 
import { NgxSpinnerModule } from 'ngx-spinner';
import { MenuHeadComponent } from '../menuHead/menuHead.component';
import { SubMenuComponent } from '../subMenu/subMenu.component';
import { MenuConfigComponent } from '../menuConfig/menuConfig.component';
import { MedDispatchComponent } from './../medDispatch/medDispatch.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { InvestmentRcvComponent } from '../investmentRcv/investmentRcv.component';
import { ChangePasswordComponent } from '../changePassword/changePassword.component';
import { BillTrackComponent } from '../billTrack/billTrack.component';
import { RptMedDispatchComponent } from '../rptMedDispatch/rptMedDispatch.component';
import { ChangeDepotComponent } from '../changeDepot/changeDepot.component';
import { RptChqDispatchComponent } from '../rptChqDispatch/rptChqDispatch.component';
import { ReportYearlyBudgetComponent } from '../reportYearlyBudget/reportYearlyBudget.component';
import { PendingDisburseComponent } from './../pendingDisburse/pendingDisburse.component'; 
import { RptInvSummarySingleComponent } from '../rptInvSummarySingle/rptInvSummarySingle.component';
import { RptDocLocMapComponent } from './../RptDocLocMap/RptDocLocMap.component';
import { RptEmpInfoComponent } from '../rptEmpInfo/rptEmpInfo.component';
import { ReportYearlySbuExpComponent } from '../reportYearlySbuExp/reportYearlySbuExp.component';
import { RptEmpWiseExpComponent } from '../rptEmpWiseExp/rptEmpWiseExp.component';
import { RptCampaignSummaryComponent } from './../rptCampaignSummary/rptCampaignSummary.component';
import {ReportSystemSummaryComponent} from '../reportSystemSummary/reportSystemSummary.component'

@NgModule({
  imports: [
    
    CommonModule,
    //Ashiq added
    ModalModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    PortalRoutingModule,
    ReactiveFormsModule,
    BsDatepickerModule.forRoot(), 
    NgSelectModule  ,
    MasterModule,
    MasterRoutingModule,
    NgxSpinnerModule,
    NgxPaginationModule,
    Ng2SearchPipeModule,
  ],
  declarations: [
    PortalComponent,
    TopnavComponent,
    AsidenavComponent,
    FooterComponent,
    MarketGroupComponent,
    ApprAuthConfigComponent,
    ApprovalCeilingComponent,
    ApprovalTimeLimitComponent,
    InvestmentInitComponent,
    InvestmentRecComponent,
    InvestmentAprComponent,
    InvestmentAprNoSbuComponent,
    InvestmentRcvComponent,
    RegApprovalComponent,
    RptDocLocMapComponent,
    ChangePasswordAllComponent,
    RptEmpWiseExpComponent,
    ReportInvestmentComponent,
    ReportYearlyBudgetComponent,
    ReportYearlySbuExpComponent,
    ChangeDepotComponent,
    ChequeTrackComponent,
    RptInvestSummaryComponent,
    //RptInvestStatusComponent,
    ParamInvestSummaryComponent,
    RptInvestmentDetailComponent,
    RptInvestmentDetailSummaryComponent,
    RptCampaignSummaryComponent,
    RptInvSummarySingleComponent,
    MedDispatchComponent,
    PendingPrintDepotComponent,
    PendingChqPrintDepotComponent,
    PendingMedDispatchComponent,
    ReportSystemSummaryComponent,
    RptMedDispatchComponent,
    RptChqDispatchComponent,
    BillTrackComponent,
    SbuWiseBudgetComponent,
    DocHonApprComponent,
    ClusterInfoComponent,
    PendingDisburseComponent,
    MenuHeadComponent,
    SubMenuComponent,
    RptEmpInfoComponent,
    MenuConfigComponent,
    ChangePasswordComponent
  ],
  providers: [DatePipe]
})
export class PortalModule { }
