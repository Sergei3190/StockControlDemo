import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectOrganizationsComponent } from './select-organizations.component';

describe('SelectOrganizationsComponent', () => {
  let component: SelectOrganizationsComponent;
  let fixture: ComponentFixture<SelectOrganizationsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SelectOrganizationsComponent]
    });
    fixture = TestBed.createComponent(SelectOrganizationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
