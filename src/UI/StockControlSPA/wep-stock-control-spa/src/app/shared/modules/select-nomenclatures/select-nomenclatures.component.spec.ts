import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectNomenclaturesComponent } from './select-nomenclatures.component';

describe('SelectNomenclaturesComponent', () => {
  let component: SelectNomenclaturesComponent;
  let fixture: ComponentFixture<SelectNomenclaturesComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SelectNomenclaturesComponent]
    });
    fixture = TestBed.createComponent(SelectNomenclaturesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
