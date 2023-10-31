import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PersonalCabinetMainInfoComponent } from './personal-cabinet-main-info.component';

describe('PersonalCabinetMainInfoComponent', () => {
  let component: PersonalCabinetMainInfoComponent;
  let fixture: ComponentFixture<PersonalCabinetMainInfoComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PersonalCabinetMainInfoComponent]
    });
    fixture = TestBed.createComponent(PersonalCabinetMainInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
