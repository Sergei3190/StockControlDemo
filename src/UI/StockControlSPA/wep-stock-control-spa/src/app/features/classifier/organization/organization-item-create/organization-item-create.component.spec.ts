import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrganizationItemCreateComponent } from './organization-item-create.component';

describe('OrganizationItemCreateComponent', () => {
  let component: OrganizationItemCreateComponent;
  let fixture: ComponentFixture<OrganizationItemCreateComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OrganizationItemCreateComponent]
    });
    fixture = TestBed.createComponent(OrganizationItemCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
