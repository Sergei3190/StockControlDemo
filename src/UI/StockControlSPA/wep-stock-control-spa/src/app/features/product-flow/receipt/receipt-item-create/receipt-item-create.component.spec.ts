import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReceiptItemCreateComponent } from './receipt-item-create.component';

describe('ReceiptItemCreateComponent', () => {
  let component: ReceiptItemCreateComponent;
  let fixture: ComponentFixture<ReceiptItemCreateComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ReceiptItemCreateComponent]
    });
    fixture = TestBed.createComponent(ReceiptItemCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
