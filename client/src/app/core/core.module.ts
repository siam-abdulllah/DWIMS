import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
//import { NavBarComponent } from './nav-bar/nav-bar.component';
import { RouterModule } from '@angular/router';
import { NotFoundComponent } from './not-found/not-found.component';
import { ServerErrorComponent } from './server-error/server-error.component';
import { ToastrModule } from 'ngx-toastr';
import { TestErrorComponent } from './test-error/test-error.component';
import { BreadcrumbModule } from 'xng-breadcrumb';
import { SectionHeaderComponent } from './section-header/section-header.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    BreadcrumbModule,
    SharedModule,
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right',
      preventDuplicates: true
    })
  ],
  declarations: [
    //NavBarComponent,
    NotFoundComponent,
    ServerErrorComponent,
    TestErrorComponent,
    SectionHeaderComponent
    ],
  exports: [//NavBarComponent, 
    SectionHeaderComponent]
})
export class CoreModule { }
