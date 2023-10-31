import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StockAvailabilityFilterComponent } from './stock-availability-filter.component';

describe('StockAvailabilityFilterComponent', () => {
  let component: StockAvailabilityFilterComponent;
  let fixture: ComponentFixture<StockAvailabilityFilterComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [StockAvailabilityFilterComponent]
    });
    fixture = TestBed.createComponent(StockAvailabilityFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
