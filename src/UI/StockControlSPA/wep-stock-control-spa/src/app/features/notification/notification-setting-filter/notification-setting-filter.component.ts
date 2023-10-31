import { Component, Inject, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { NotificationTypesService } from './services/notification-types.service';
import { Subject, debounceTime, takeUntil, of } from 'rxjs';
import { INamedEntity } from 'src/app/shared/interfaces/named-entity.interface';
import { MatAutocomplete } from '@angular/material/autocomplete';
import { IPagination } from 'src/app/shared/modules/pagination/interfaces/pagination.intarface';
import { INotificationTypeFilter } from '../interfaces/notification-type/notification-type-filter.interface';
import { INotificationTypes } from '../interfaces/notification-type/notification-types.interface';

@Component({
  selector: 'app-notification-setting-filter',
  templateUrl: './notification-setting-filter.component.html',
  styleUrls: ['./notification-setting-filter.component.scss']
})
export class NotificationSettingFilterComponent implements OnInit, OnDestroy {
  private readonly destroy$ = new Subject<void>();
  
  title: string;
  form: FormGroup;
  canDisabledForm: boolean;

  paginationInfo: IPagination;
  filter: INotificationTypeFilter;
  items: INamedEntity[];

  isNoContent: boolean;

  @ViewChild('auto', { static: true }) auto: MatAutocomplete;

  constructor(
    private service: NotificationTypesService,
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<NotificationSettingFilterComponent>,
    @Inject(MAT_DIALOG_DATA) data: any) {
      this.title = data.title;
  }

  ngOnInit(): void {
    this.initProperties();
    this.loadNotificationTypes(false);
    this.initSubscriptions();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  save() {
    this.dialogRef.close(this.form.value);
  }

  close() {
      this.dialogRef.close();
  }

  loadSelectItems() {
    this.filter.page = 1;
    this.filter.search = '';
    this.loadNotificationTypes(false);
  }

  private initProperties(search?: string) {
    this.items = [];
    this.isNoContent = false;
    this.canDisabledForm = true;
    this.paginationInfo = {} as IPagination;
    this.filter = {
      search: search,
      page: 1,
      pageSize: 10,
    } as INotificationTypeFilter;
    this.form = this.fb.group({
      notificationTypeName: [null],
      notificationType: [null]
    });
  }

  private loadNotificationTypes(canNextPage: boolean) {
    this.service.selectNotificationTypes(this.filter)
      .pipe(takeUntil(this.destroy$))
      .subscribe(notificationTypes => {
        if (!notificationTypes){
          this.items = [];
          this.isNoContent = true;
          return;
        }
        
        this.isNoContent = false;
        if (canNextPage){
          this.items.push(...(notificationTypes as INotificationTypes).items);
        } else{
          this.items = (notificationTypes as INotificationTypes).items.slice();
        }

        this.paginationInfo = this.getFilledPaginationInfo(notificationTypes as INotificationTypes);
      });
  }

  private getFilledPaginationInfo(notificationTypes: INotificationTypes): IPagination {
    return {
      page: notificationTypes.page,
      pageSize: notificationTypes.pageSize,
      totalPages: notificationTypes.totalPages,
      totalItems: notificationTypes.totalItems,
      items: notificationTypes.items.length
    } as IPagination;
  }

  private initSubscriptions() {
    this.auto.opened
      .pipe(
        debounceTime(100), // без этого this.auto.panel не успевает инициализироваться
        takeUntil(this.destroy$))
      .subscribe(() => {
        if (!this.auto.panel){
          return;
        }
        this.registerScrollEvent();
    });
    this.form.valueChanges
      .pipe(
        debounceTime(100),
        takeUntil(this.destroy$))
      .subscribe(values => {
        this.canDisabledForm = !Object.keys(values)
            .map(key => values[key])
            .some(v => !!v);
      });
    this.form.get('notificationTypeName')?.valueChanges
      .pipe(
        debounceTime(300),
        takeUntil(this.destroy$))
      .subscribe(name => {
        this.filter.page = 1;
        this.filter.search = name;
        this.loadNotificationTypes(false);
        this.setNotificationType(name);
    });
  }

  private setNotificationType(name: any) {
    const notificationType = this.items.find(i => i.name === name);   
    this.form.get('notificationType')?.setValue(notificationType);
    this.form.controls['notificationType'].markAsDirty();
  }

  private registerScrollEvent() : void {
    const panel = this.auto.panel.nativeElement;
    panel.addEventListener('scroll', (event: any) => {
      if (this.hasToBottomScroll(event.target)){
        this.filter.page = this.filter.page + 1;
        this.loadNotificationTypes(true);
      }
    });
  }

  private hasToBottomScroll(target: any) : boolean {
    return (target.offsetHeight + target.scrollTop >= target.scrollHeight - 1);
  }
}
