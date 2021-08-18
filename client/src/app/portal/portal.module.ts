import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

import { PortalComponent } from './portal.component';
import { PortalRoutingModule } from './portal-routing.module';

import { TopnavComponent } from '../mastertheme/topnav/topnav.component';
import { AsidenavComponent } from '../mastertheme/asidenav/asidenav.component';
import { FooterComponent } from '../mastertheme/footer/footer.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {DatePipe} from '@angular/common';
import { MarketGroupComponent } from '../marketGroup/marketGroup.component';
import { RegApprovalComponent } from '../regApproval/regApproval.component';
import { InvestmentInitComponent } from '../investmentInit/investmentInit.component';
import { InvestmentRecComponent } from '../investmentRec/investmentRec.component';
import { InvestmentAprComponent } from '../investmentApr/investmentApr.component';
import { ReportInvestmentComponent } from './../report-investment/report-investment.component';
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
    RegApprovalComponent,
    ReportInvestmentComponent,
    SbuWiseBudgetComponent,
    DocHonApprComponent
  ],
  providers: [DatePipe]
})
export class PortalModule { }
