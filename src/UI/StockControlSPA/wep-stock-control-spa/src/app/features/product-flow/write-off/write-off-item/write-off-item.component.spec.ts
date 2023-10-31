import { ComponentFixture, TestBed } from '@angular/core/testing';
import { WriteOffItemComponent } from './write-off-item.component';

describe('WriteOffItemComponent', () => {
  let component: WriteOffItemComponent;
  let fixture: ComponentFixture<WriteOffItemComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [WriteOffItemComponent]
    });
    fixture = TestBed.createComponent(WriteOffItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
