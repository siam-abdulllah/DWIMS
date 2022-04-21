import { RptCampaignSummaryComponent } from './../rptCampaignSummary/rptCampaignSummary.component';
import { ChangePasswordAllComponent } from './../changePasswordAll/changePasswordAll.component';
import { RptDocLocMapComponent } from './../RptDocLocMap/RptDocLocMap.component';
import { ParamInvestSummaryComponent } from './../paramInvestmentSummary/paramInvestmentSummary.component';
import { PortalComponent } from './portal.component';
import { MarketGroupComponent } from '../marketGroup/marketGroup.component';
import { ApprAuthConfigComponent } from '../apprAuthConfig/apprAuthConfig.component';
import { ApprovalCeilingComponent } from '../approval-ceiling/approval-ceiling.component';
import { ApprovalTimeLimitComponent } from '../approval-time-limit/approval-time-limit.component';
import { DocHonApprComponent } from '../docHonAppr/docHonAppr.component';
import { RptInvestSummaryComponent } from '../rptInvestmentSummary/rptInvestmentSummary.component';
import { RptInvestmentDetailComponent } from '../RptInvestmentDetail/rptInvestmentDetail.component';
import { RegApprovalComponent } from '../regApproval/regApproval.component';
import { InvestmentInitComponent } from '../investmentInit/investmentInit.component';
import { InvestmentRecComponent } from '../investmentRec/investmentRec.component';
import { InvestmentAprComponent } from '../investmentApr/investmentApr.component';
import { InvestmentAprNoSbuComponent } from './../investmentAprNoSbu/investmentAprNoSbu.component';
import { InvestmentRcvComponent } from '../investmentRcv/investmentRcv.component';
import { SbuWiseBudgetComponent } from '../sbu-wise-budget/sbu-wise-budget.component';
import { ReportInvestmentComponent } from './../report-investment/report-investment.component';
import { PendingPrintDepotComponent } from '../pendingPrintDepot/pendingPrintDepot.component';
import { BillTrackComponent } from '../billTrack/billTrack.component';
import { ChangeDepotComponent } from '../changeDepot/changeDepot.component';
import { MedDispatchComponent } from './../medDispatch/medDispatch.component';
import { PendingMedDispatchComponent } from './../PendingMedDispatch/pendingMedDispatch.component';
import { RptMedDispatchComponent } from '../rptMedDispatch/rptMedDispatch.component';
import { ChequeTrackComponent } from './../chequeTrack/chequeTrack.component'; 
import { PendingDisburseComponent } from './../pendingDisburse/pendingDisburse.component'; 
import { ReportYearlyBudgetComponent } from '../reportYearlyBudget/reportYearlyBudget.component';
import { ReportYearlySbuExpComponent } from '../reportYearlySbuExp/reportYearlySbuExp.component';
import { NgModule } from '@angular/core';
import { RptInvestmentDetailSummaryComponent } from '../RptInvDetailForSummary/rptInvDetailForSummary.component'; ;
import { PendingChqPrintDepotComponent } from '../pendingChqPrintDepot/pendingChqPrintDepot.component';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from '../home/home.component';
import { SuperAdminRoleGuard } from '../_guard/superAdminRole.guard';
import { ClusterInfoComponent } from '../master/cluster-info/cluster-info.component';
import { MenuHeadComponent } from '../menuHead/menuHead.component';
import { SubMenuComponent } from '../subMenu/subMenu.component';
import { MenuConfigComponent } from '../menuConfig/menuConfig.component';
import { ChangePasswordComponent } from '../changePassword/changePassword.component';
import { RptChqDispatchComponent } from '../rptChqDispatch/rptChqDispatch.component';
import { RptInvSummarySingleComponent } from '../rptInvSummarySingle/rptInvSummarySingle.component';
import { RptEmpInfoComponent } from '../rptEmpInfo/rptEmpInfo.component';
import { RptEmpWiseExpComponent } from '../rptEmpWiseExp/rptEmpWiseExp.component';
import { ReportSystemSummaryComponent } from '../reportSystemSummary/reportSystemSummary.component'
import { InvestmentFormComponent } from '../investmentForm/investmentForm.component';
import { InvestmentRapidAprComponent } from '../investmentRapidApr/investmentRapidApr.component';
import { BgtEmployeeComponent } from '../bgtEmployee/bgtEmployee.component';

//import { RptInvestStatusComponent } from '../rptInvestmentStatus/rptInvestmentStatus.component';

const portalRoutes: Routes = [
  {
    path: 'portal',
    component: PortalComponent,
    children: [
      
        //{path: 'home', component: HomeComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'home', component: HomeComponent},
        {path: 'apprAuthConfig', component: ApprAuthConfigComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'approval-ceiling', component: ApprovalCeilingComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'approval-time-limit', component: ApprovalTimeLimitComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'docHonAppr', component: DocHonApprComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'investmentInit', component: InvestmentInitComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'investmentrapid', component: InvestmentFormComponent},
        {path: 'investmentrapidapr', component: InvestmentRapidAprComponent},
       {path: 'investmentRec', component: InvestmentRecComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'investmentApr', component: InvestmentAprComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'investmentAprNoSbu', component: InvestmentAprNoSbuComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'investmentRcv', component: InvestmentRcvComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'marketGroup', component: MarketGroupComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'regApproval', component: RegApprovalComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'sbu-wise-budget', component: SbuWiseBudgetComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'rptInvestment', component: ReportInvestmentComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'rptInvestmentSummary', component: RptInvestSummaryComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'rptInvestmentSingle', component: RptInvSummarySingleComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'pendingPrintDepot', component: PendingPrintDepotComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'pendingChqPrintDepot', component: PendingChqPrintDepotComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'changePassAll', component: ChangePasswordAllComponent},
        {path: 'mpoPendingDisburse', component: PendingDisburseComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'billTrack', component: BillTrackComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'chequeTrack', component: ChequeTrackComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'medDispatch', component: MedDispatchComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'pendingMedDispatch', component: PendingMedDispatchComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'rptMedDispatch', component: RptMedDispatchComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'rptChqDispatch', component: RptChqDispatchComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'rptYearlyBudget', component: ReportYearlyBudgetComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'rptYearlySbuExp', component: ReportYearlySbuExpComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'rptCampSummary', component: RptCampaignSummaryComponent },
        {path: 'rptInvDataSummary', component: ReportSystemSummaryComponent },
        {path: 'rptEmpExp', component: RptEmpWiseExpComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'changeDepot', component: ChangeDepotComponent, canActivate: [SuperAdminRoleGuard]},

        {path: 'bgtEmployee', component: BgtEmployeeComponent},

        {path: 'paramInvestmentSummary/:param', component: ParamInvestSummaryComponent},
        //{path: 'rptInvestmentDetail', component: RptInvestmentDetailComponent},
        {path: 'rptInvestmentDetail/:id', component: RptInvestmentDetailComponent},
        {path: 'rptInvDtlSummary/:id', component: RptInvestmentDetailSummaryComponent},
        {path: 'cluster', component: ClusterInfoComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'menuHead', component: MenuHeadComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'subMenu', component: SubMenuComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'rptDocLoc', component: RptDocLocMapComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'rptEmpInfo', component: RptEmpInfoComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'menuConfig', component: MenuConfigComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'changePassword', component: ChangePasswordComponent},
        //{path: 'rptInvestStatus', component: RptInvestStatusComponent},
        {path: '', component: PortalComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'master', loadChildren: () => import('../master/master.module')
       .then(mod => mod.MasterModule) , data: {breadcrumb: {skip: true}}}, 
        ]
}
];
@NgModule({
  imports: [RouterModule.forChild(portalRoutes)],
  exports: [RouterModule]
})
export class PortalRoutingModule {}