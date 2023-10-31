import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SideDrawerContainerComponent } from './side-drawer-container.component';

describe('SideDrawerConteinerComponent', () => {
  let component: SideDrawerContainerComponent;
  let fixture: ComponentFixture<SideDrawerContainerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SideDrawerContainerComponent]
    });
    fixture = TestBed.createComponent(SideDrawerContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
