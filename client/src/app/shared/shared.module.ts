
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { PagingHeaderComponent } from './components/paging-header/paging-header.component';
import { PagerComponent } from './components/pager/pager.component';

import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { TextInputComponent } from './components/text-input/text-input.component';

import {CdkStepperModule} from '@angular/cdk/stepper';
import { StepperComponent } from './components/stepper/stepper.component';
import { RouterModule } from '@angular/router';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
@NgModule({
  imports: [
    CommonModule,
    PaginationModule.forRoot(),
    CarouselModule.forRoot(),
    BsDropdownModule.forRoot(),
    ReactiveFormsModule,
    FormsModule,
    CdkStepperModule,
    RouterModule,
    NgMultiSelectDropDownModule
  ],
  declarations: [
    PagingHeaderComponent,
    PagerComponent,
    TextInputComponent,
    StepperComponent
  ],
  exports: [
    PaginationModule,
    PagingHeaderComponent,
    PagerComponent,
    CarouselModule,
    BsDropdownModule,
    ReactiveFormsModule,
    FormsModule,
    TextInputComponent,
    CdkStepperModule,
    StepperComponent,
    NgMultiSelectDropDownModule
  ]
})
export class SharedModule { }
