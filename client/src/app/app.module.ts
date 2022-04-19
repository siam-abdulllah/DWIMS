import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TopnavComponent } from './mastertheme/topnav/topnav.component';
import { AsidenavComponent } from './mastertheme/asidenav/asidenav.component';
import { FooterComponent } from './mastertheme/footer/footer.component';
import { HomeComponent } from './home/home.component';
import { FormsModule } from "@angular/forms";
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
//import { CoreModule } from './core/core.module';
import { NgxSpinnerModule } from 'ngx-spinner';
import { LoadingInterceptor } from './core/interceptors/loading.interceptors';
import { JwtInterceptor } from './core/interceptors/jwt.interceptor';
import { ErrorInterceptor } from './core/interceptors/error.interceptor';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { AccountModule } from './account/account.module';
import { PortalModule } from './portal/portal.module';
import { PortalRoutingModule } from './portal/portal-routing.module';
import { MasterModule } from './master/master.module';
import { MasterRoutingModule } from './master/master-routing.module';
import { AccountRoutingModule } from './account/account-routing.module';
import { ModalModule } from 'ngx-bootstrap/modal';
import { Ng2SearchPipeModule } from 'ng2-search-filter';
import { SheetJSComponent } from './sheetjs.component'

@NgModule({
  declarations: [
    SheetJSComponent,
    AppComponent,
    // TopnavComponent,
    // AsidenavComponent,
    // FooterComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    Ng2SearchPipeModule,
    BsDatepickerModule.forRoot(),
    ModalModule.forRoot(),
    HttpClientModule,
    AccountModule,
    PortalModule,
    MasterModule,
    AccountRoutingModule,
    PortalRoutingModule,
    MasterRoutingModule,
    NgxSpinnerModule,
    ToastrModule.forRoot({
      timeOut: 5000,
    positionClass: 'toast-bottom-right',
    preventDuplicates: true,
    progressBar: true,
    closeButton: true,
    })

  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true},
    { provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true},
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
