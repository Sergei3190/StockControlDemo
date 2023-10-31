import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NomenclatureListComponent } from './nomenclature-list.component';

describe('NomenclatureListComponent', () => {
  let component: NomenclatureListComponent;
  let fixture: ComponentFixture<NomenclatureListComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [NomenclatureListComponent]
    });
    fixture = TestBed.createComponent(NomenclatureListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
