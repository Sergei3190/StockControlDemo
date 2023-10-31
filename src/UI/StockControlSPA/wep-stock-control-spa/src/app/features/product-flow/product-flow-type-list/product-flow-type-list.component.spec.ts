import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ProductFlowTypeListComponent } from './product-flow-type-list.component';


describe('ProductFlowTypeListComponent', () => {
  let component: ProductFlowTypeListComponent;
  let fixture: ComponentFixture<ProductFlowTypeListComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ProductFlowTypeListComponent]
    });
    fixture = TestBed.createComponent(ProductFlowTypeListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
