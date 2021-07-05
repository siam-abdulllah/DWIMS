import { DonationComponent } from './donation/donation.component';
import { SubCampaignComponent } from './subCampaign/subCampaign.component';
import { CampaignComponent } from './campaign/campaign.component';
import { BcdsInfoComponent } from './bcds-info/bcds-info.component';
import { EmployeeInfoComponent } from './employee-info/employee-info.component';
import { SocietyInfoComponent } from './society-info/society-info.component';
import { ApprovalAuthorityComponent } from './approvalAuthority/approvalAuthority.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from '../home/home.component';
// import { LoginComponent } from './login/login.component';
// import { RegisterComponent } from './register/register.component';

const routes: Routes = [
  // {path: 'login', component: LoginComponent},
  // {path: 'register', component: RegisterComponent, data: {breadcrumb: 'Register User'}},
  // {path: 'register/:id', component: RegisterComponent, data: {breadcrumb: 'User Details'}},
  {path: '', component: HomeComponent, data: {breadcrumb: 'Home'} },
  {path: 'donation', component: DonationComponent}  ,
  {path: 'subCampaign', component: SubCampaignComponent},
  {path: 'campaign', component: CampaignComponent} ,
  {path: 'bcds-info', component: BcdsInfoComponent}  ,
  {path: 'employee-info', component: EmployeeInfoComponent},
  {path: 'society-info', component: SocietyInfoComponent}  ,
  {path: 'approvalAuthority', component: ApprovalAuthorityComponent}    

];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class MasterRoutingModule { }
