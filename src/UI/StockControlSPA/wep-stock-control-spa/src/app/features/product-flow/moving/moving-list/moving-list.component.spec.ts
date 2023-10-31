import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MovingListComponent } from './moving-list.component';

describe('MovingListComponent', () => {
  let component: MovingListComponent;
  let fixture: ComponentFixture<MovingListComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MovingListComponent]
    });
    fixture = TestBed.createComponent(MovingListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
