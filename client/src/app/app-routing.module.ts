import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './account/login/login.component';
import { PortalComponent } from './portal/portal.component';
//import { TestErrorComponent } from './core/test-error/test-error.component';
//import { ServerErrorComponent } from './core/server-error/server-error.component';
//import { NotFoundComponent } from './core/not-found/not-found.component';
//import { AuthGuard } from './core/guards/auth.guard';

const routes: Routes = [
  //{path: '', component: HomeComponent, data: {breadcrumb: 'Home'} },
  //{path: 'not-found', component: NotFoundComponent, data: {breadcrumb: 'Not Found'} },
  {path: '', component: LoginComponent},
 //  {path: 'master', loadChildren: () => import('./master/master.module')
 //.then(mod => mod.MasterModule) , data: {breadcrumb: {skip: true}}}, 
// {path: 'portal', loadChildren: () => import('./portal/portal.module')
// .then(mod => mod.PortalModule) , data: {breadcrumb: {skip: true}}}, 
// {path: 'account', loadChildren: () => import('./account/account.module')
// .then(mod => mod.AccountModule) , data: {breadcrumb: {skip: true}}}, 
//   {path: '**', redirectTo: '', pathMatch: 'full'},
//   {path: '', component: LoginComponent},
  {path: 'login', component: LoginComponent},
  {path: 'portal', component: PortalComponent},
  
];


@NgModule({
  imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
