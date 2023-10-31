import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NotificationSettingFilterComponent } from './notification-setting-filter.component';

describe('NotificationSettingFilterComponent', () => {
  let component: NotificationSettingFilterComponent;
  let fixture: ComponentFixture<NotificationSettingFilterComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [NotificationSettingFilterComponent]
    });
    fixture = TestBed.createComponent(NotificationSettingFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
