import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectWarehousesComponent } from './select-warehouses.component';

describe('SelectWarehousesComponent', () => {
  let component: SelectWarehousesComponent;
  let fixture: ComponentFixture<SelectWarehousesComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SelectWarehousesComponent]
    });
    fixture = TestBed.createComponent(SelectWarehousesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
