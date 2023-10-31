import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { MomentDateAdapter } from '@angular/material-moment-adapter';

export const MAT_DATE_PICKER_FORMATS = {
  parse: {
      dateInput: 'DD.MM.YYYY' 
  },
  display: {
      dateInput: 'DD.MM.YYYY',
      monthYearLabel: 'MMMM YYYY',
      dateA11yLabel: 'LL',
      monthYearA11yLabel: 'MMMM YYYY'
  }
};

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ]
})

export class MatDatepickerAdapterModule {
  static forRoot(): ModuleWithProviders<MatDatepickerAdapterModule> {
    return {
      ngModule: MatDatepickerAdapterModule,
      providers: [
        { provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE] },
        { provide: MAT_DATE_FORMATS, useValue: MAT_DATE_PICKER_FORMATS }
      ]
    };
  }
 }
