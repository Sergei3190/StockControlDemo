import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StockAvailabilityItemComponent } from './stock-availability-item.component';

describe('StockAvailabilityItemComponent', () => {
  let component: StockAvailabilityItemComponent;
  let fixture: ComponentFixture<StockAvailabilityItemComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [StockAvailabilityItemComponent]
    });
    fixture = TestBed.createComponent(StockAvailabilityItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
