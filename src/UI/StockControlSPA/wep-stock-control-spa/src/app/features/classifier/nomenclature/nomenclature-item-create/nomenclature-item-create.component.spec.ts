import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NomenclatureItemCreateComponent } from './nomenclature-item-create.component';

describe('NomenclatureItemCreateComponent', () => {
  let component: NomenclatureItemCreateComponent;
  let fixture: ComponentFixture<NomenclatureItemCreateComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [NomenclatureItemCreateComponent]
    });
    fixture = TestBed.createComponent(NomenclatureItemCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
