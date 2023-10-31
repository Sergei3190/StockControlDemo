import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WriteOffFilterComponent } from './write-off-filter.component';

describe('WriteOffFilterComponent', () => {
  let component: WriteOffFilterComponent;
  let fixture: ComponentFixture<WriteOffFilterComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [WriteOffFilterComponent]
    });
    fixture = TestBed.createComponent(WriteOffFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
