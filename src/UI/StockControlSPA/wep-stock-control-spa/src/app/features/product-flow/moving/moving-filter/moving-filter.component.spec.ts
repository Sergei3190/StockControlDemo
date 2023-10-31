import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MovingFilterComponent } from './moving-filter.component';

describe('MovingFilterComponent', () => {
  let component: MovingFilterComponent;
  let fixture: ComponentFixture<MovingFilterComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MovingFilterComponent]
    });
    fixture = TestBed.createComponent(MovingFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
