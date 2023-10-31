import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WriteOffItemCreateComponent } from './write-off-item-create.component';

describe('WriteOffItemCreateComponent', () => {
  let component: WriteOffItemCreateComponent;
  let fixture: ComponentFixture<WriteOffItemCreateComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [WriteOffItemCreateComponent]
    });
    fixture = TestBed.createComponent(WriteOffItemCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
