import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WriteOffListComponent } from './write-off-list.component';

describe('WriteOffListComponent', () => {
  let component: WriteOffListComponent;
  let fixture: ComponentFixture<WriteOffListComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [WriteOffListComponent]
    });
    fixture = TestBed.createComponent(WriteOffListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
