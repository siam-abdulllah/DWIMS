import { PortalComponent } from './portal.component';
import { MarketGroupComponent } from '../marketGroup/marketGroup.component';
import { ApprAuthConfigComponent } from '../apprAuthConfig/apprAuthConfig.component';
import { DocHonApprComponent } from '../docHonAppr/docHonAppr.component';
import { RegApprovalComponent } from '../regApproval/regApproval.component';
import { InvestmentInitComponent } from '../investmentInit/investmentInit.component';
import { NgModule } from '@angular/core';
//import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from '../home/home.component';

const portalRoutes: Routes = [
  {
    path: 'portal',
    component: PortalComponent,
    children: [
        {path: 'home', component: HomeComponent},
        {path: 'apprAuthConfig', component: ApprAuthConfigComponent},
        {path: 'docHonAppr', component: DocHonApprComponent},
        {path: 'investmentInit', component: InvestmentInitComponent},
         {path: '', component: PortalComponent},
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

