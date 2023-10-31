import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectPartiesComponent } from './select-parties.component';

describe('SelectPartiesComponent', () => {
  let component: SelectPartiesComponent;
  let fixture: ComponentFixture<SelectPartiesComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SelectPartiesComponent]
    });
    fixture = TestBed.createComponent(SelectPartiesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
