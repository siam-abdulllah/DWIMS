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
import { InvestmentInitComponent } from '../investmentInit/investmentInit.component';
import { ApprAuthConfigComponent } from '../apprAuthConfig/apprAuthConfig.component';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { NgSelectModule } from '@ng-select/ng-select';
import { MasterModule } from '../master/master.module';
import { MasterRoutingModule } from '../master/master-routing.module';
import { ModalModule } from 'ngx-bootstrap/modal';

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
    InvestmentInitComponent,
  ],
  providers: [DatePipe]
})
export class PortalModule { }
