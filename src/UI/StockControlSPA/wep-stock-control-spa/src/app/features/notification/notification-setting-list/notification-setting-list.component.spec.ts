import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NotificationSettingListComponent } from './notification-setting-list.component';

describe('NotificationSettingListComponent', () => {
  let component: NotificationSettingListComponent;
  let fixture: ComponentFixture<NotificationSettingListComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [NotificationSettingListComponent]
    });
    fixture = TestBed.createComponent(NotificationSettingListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
