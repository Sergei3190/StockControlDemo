import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PersonalCabinetPhotoComponent } from './personal-cabinet-photo.component';

describe('PersonalCabinetPhotoComponent', () => {
  let component: PersonalCabinetPhotoComponent;
  let fixture: ComponentFixture<PersonalCabinetPhotoComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PersonalCabinetPhotoComponent]
    });
    fixture = TestBed.createComponent(PersonalCabinetPhotoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
