import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReceiptItemComponent } from './receipt-item.component';

describe('ReceiptItemComponent', () => {
  let component: ReceiptItemComponent;
  let fixture: ComponentFixture<ReceiptItemComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ReceiptItemComponent]
    });
    fixture = TestBed.createComponent(ReceiptItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
