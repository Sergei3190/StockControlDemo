import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MovingItemCreateComponent } from './moving-item-create.component';

describe('MovingItemCreateComponent', () => {
  let component: MovingItemCreateComponent;
  let fixture: ComponentFixture<MovingItemCreateComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MovingItemCreateComponent]
    });
    fixture = TestBed.createComponent(MovingItemCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
