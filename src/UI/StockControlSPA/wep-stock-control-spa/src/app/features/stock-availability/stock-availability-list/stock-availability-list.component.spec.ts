import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StockAvailabilityListComponent } from './stock-availability-list.component';

describe('StockAvailabilityListComponent', () => {
  let component: StockAvailabilityListComponent;
  let fixture: ComponentFixture<StockAvailabilityListComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [StockAvailabilityListComponent]
    });
    fixture = TestBed.createComponent(StockAvailabilityListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
