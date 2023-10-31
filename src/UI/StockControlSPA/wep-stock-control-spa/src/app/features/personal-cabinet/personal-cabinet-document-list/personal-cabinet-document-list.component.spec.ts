import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PersonalCabinetDocumentListComponent } from './personal-cabinet-document-list.component';

describe('PersonalCabinetDocumentListComponent', () => {
  let component: PersonalCabinetDocumentListComponent;
  let fixture: ComponentFixture<PersonalCabinetDocumentListComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PersonalCabinetDocumentListComponent]
    });
    fixture = TestBed.createComponent(PersonalCabinetDocumentListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
