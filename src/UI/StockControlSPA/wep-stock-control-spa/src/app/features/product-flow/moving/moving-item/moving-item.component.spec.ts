import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MovingItemComponent } from './moving-item.component';

describe('MovingItemComponent', () => {
  let component: MovingItemComponent;
  let fixture: ComponentFixture<MovingItemComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MovingItemComponent]
    });
    fixture = TestBed.createComponent(MovingItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
