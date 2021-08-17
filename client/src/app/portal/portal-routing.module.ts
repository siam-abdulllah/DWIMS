import { PortalComponent } from './portal.component';
import { MarketGroupComponent } from '../marketGroup/marketGroup.component';
import { ApprAuthConfigComponent } from '../apprAuthConfig/apprAuthConfig.component';
import { ApprovalCeilingComponent } from '../approval-ceiling/approval-ceiling.component';
import { ApprovalTimeLimitComponent } from '../approval-time-limit/approval-time-limit.component';
import { DocHonApprComponent } from '../docHonAppr/docHonAppr.component';
import { RegApprovalComponent } from '../regApproval/regApproval.component';
import { InvestmentInitComponent } from '../investmentInit/investmentInit.component';
import { InvestmentRecComponent } from '../investmentRec/investmentRec.component';
import { InvestmentAprComponent } from '../investmentApr/investmentApr.component';
import { SbuWiseBudgetComponent } from '../sbu-wise-budget/sbu-wise-budget.component';
import { ReportInvestmentComponent } from './../report-investment/report-investment.component';
import { NgModule } from '@angular/core';
//import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from '../home/home.component';
import { SuperAdminRoleGuard } from '../_guard/superAdminRole.guard';

const portalRoutes: Routes = [
  {
    path: 'portal',
    component: PortalComponent,
    children: [
        {path: 'home', component: HomeComponent},
        {path: 'apprAuthConfig', component: ApprAuthConfigComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'approval-ceiling', component: ApprovalCeilingComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'approval-time-limit', component: ApprovalTimeLimitComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'docHonAppr', component: DocHonApprComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'investmentInit', component: InvestmentInitComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'investmentRec', component: InvestmentRecComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'investmentApr', component: InvestmentAprComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'marketGroup', component: MarketGroupComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'regApproval', component: RegApprovalComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'sbu-wise-budget', component: SbuWiseBudgetComponent, canActivate: [SuperAdminRoleGuard]},
        {path: 'rptInvestment', component: ReportInvestmentComponent, canActivate: [SuperAdminRoleGuard]},
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