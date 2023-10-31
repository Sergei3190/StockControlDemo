import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseItemComponent } from './warehouse-item.component';

describe('WarehouseItemComponent', () => {
  let component: WarehouseItemComponent;
  let fixture: ComponentFixture<WarehouseItemComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [WarehouseItemComponent]
    });
    fixture = TestBed.createComponent(WarehouseItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
