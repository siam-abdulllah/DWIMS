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
import { SuperAdminRoleGuard } from '../_guard/superAdminRole.guard';
// import { LoginComponent } from './login/login.component';
// import { RegisterComponent } from './register/register.component';

const routes: Routes = [
  // {path: 'login', component: LoginComponent},
  // {path: 'register', component: RegisterComponent, data: {breadcrumb: 'Register User'}},
  // {path: 'register/:id', component: RegisterComponent, data: {breadcrumb: 'User Details'}},
  {path: '', component: HomeComponent, data: {breadcrumb: 'Home'} },
  {path: 'donation', component: DonationComponent, canActivate: [SuperAdminRoleGuard]}  ,
  {path: 'subCampaign', component: SubCampaignComponent, canActivate: [SuperAdminRoleGuard]},
  {path: 'campaign', component: CampaignComponent, canActivate: [SuperAdminRoleGuard]} ,
  {path: 'bcds-info', component: BcdsInfoComponent, canActivate: [SuperAdminRoleGuard]}  ,
  {path: 'employee-info', component: EmployeeInfoComponent, canActivate: [SuperAdminRoleGuard]},
  {path: 'society-info', component: SocietyInfoComponent, canActivate: [SuperAdminRoleGuard]}  ,
  {path: 'approvalAuthority', component: ApprovalAuthorityComponent, canActivate: [SuperAdminRoleGuard]}    

];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class MasterRoutingModule { }
