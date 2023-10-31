import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseItemCreateComponent } from './warehouse-item-create.component';

describe('WarehouseItemCreateComponent', () => {
  let component: WarehouseItemCreateComponent;
  let fixture: ComponentFixture<WarehouseItemCreateComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [WarehouseItemCreateComponent]
    });
    fixture = TestBed.createComponent(WarehouseItemCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
