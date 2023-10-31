import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClassifierListComponent } from './classifier-list.component';

describe('ClassifierListComponent', () => {
  let component: ClassifierListComponent;
  let fixture: ComponentFixture<ClassifierListComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ClassifierListComponent]
    });
    fixture = TestBed.createComponent(ClassifierListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
