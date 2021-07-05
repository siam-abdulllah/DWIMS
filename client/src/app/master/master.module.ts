import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { DonationComponent } from './donation/donation.component';
import { SubCampaignComponent } from './subCampaign/subCampaign.component';
import { CampaignComponent } from './campaign/campaign.component';
import { BcdsInfoComponent } from './bcds-info/bcds-info.component';
import { SocietyInfoComponent } from './society-info/society-info.component';
import { ApprovalAuthorityComponent } from './approvalAuthority/approvalAuthority.component';
import { EmployeeInfoComponent } from './employee-info/employee-info.component';
import { MasterRoutingModule } from './master-routing.module';

//import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { NgSelectModule } from '@ng-select/ng-select';

@NgModule({
  declarations: [DonationComponent,SubCampaignComponent,CampaignComponent, BcdsInfoComponent, SocietyInfoComponent, EmployeeInfoComponent,ApprovalAuthorityComponent],
  imports: [
    CommonModule,
    MasterRoutingModule,
    FormsModule,
    //BrowserAnimationsModule,
    BsDatepickerModule.forRoot(), 
    NgSelectModule   
  ]
})
export class MasterModule { }
