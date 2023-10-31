import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject, skipWhile, takeUntil } from 'rxjs';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { NotificationSettingFilterComponent } from '../notification-setting-filter/notification-setting-filter.component';
import { HostListener } from '@angular/core';
import { IPagination } from 'src/app/shared/modules/pagination/interfaces/pagination.intarface';
import { LoadingInfoModel } from 'src/app/shared/models/loading-info.model';
import { IError } from 'src/app/shared/interfaces/error.interface';
import { HttpStatusCode } from '@angular/common/http';
import { INotificationSettingItem } from 'src/app/shared/modules/notification-settings/interfaces/notification-setting-item.interface';
import { INotificationSettingFilter } from 'src/app/shared/modules/notification-settings/interfaces/notification-setting-filter.interface';
import { NotificationSettingsService } from 'src/app/shared/modules/notification-settings/services/notification-settings.service';
import { INotificationSettings } from 'src/app/shared/modules/notification-settings/interfaces/notification-settings.interface';

@Component({
  selector: 'app-notification-setting-list',
  templateUrl: './notification-setting-list.component.html',
  styleUrls: ['./notification-setting-list.component.scss']
})
export class NotificationSettingListComponent implements OnInit, OnDestroy  {
  private readonly destroy$ = new Subject<void>();

  items: INotificationSettingItem[];
  
  loadingInfo: LoadingInfoModel;
  
  canClearSearch = new Subject<boolean>();
  reload = () => {
    this.clearFilter();
  };

  content?: string;
  paginationInfo: IPagination;
  filter: INotificationSettingFilter

  constructor(private dialog: MatDialog, 
    private service: NotificationSettingsService,
    private router: Router,
    private activatedRoute: ActivatedRoute){
  }

  ngOnInit(): void {
    this.initProperties();
    this.setStartRoute();
    this.load();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  @HostListener('window:popstate', ['$event'])
  onPopState(event: any) {
    this.router.navigate(['./'], {
      queryParams: event.target.location?.search,
      queryParamsHandling: ""
    });
  }

  onChangedSearch(searchString: any): void {
    this.initProperties(searchString);
    this.setStartRoute();
    this.load(true);
    this.loadingInfo.isResultFilter = true;
  }

  onPageChanged(value: any): void {
    this.paginationInfo.page = value;
    this.filter.page = value;
    this.setStartRoute();
    this.load();
  }

  updateItem(item: INotificationSettingItem): void {
    this.service.updateNotificationSetting(item)
      .pipe(takeUntil(this.destroy$))
      .subscribe(flag => {
        if (flag) {
          this.items.sort((prev, next) => {
            if (!prev.enable){
              return 1;
            } else if (prev.enable === next.enable) {
              return 0;
            } else {
              return -1;
            }
          });

          this.service.saveNotificationSettingsInStorage(this.items);
        }
      });
  }

  clearFilter(): void {
    this.canClearSearch.next(true);
    this.ngOnInit();
  }

  openFilter(): void {
    const dialogConfig = new MatDialogConfig();

    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = false;
    dialogConfig.position = { top: '50px' };

    dialogConfig.data = {
      title: 'Фильтр настройки уведомлений'
    };

    const dialogRef = this.dialog.open(NotificationSettingFilterComponent , dialogConfig);

    dialogRef.afterClosed()
      .pipe(
        skipWhile(d => !d),
        takeUntil(this.destroy$))
      .subscribe(data => {
        this.filter.notificationTypeId = data.notificationType?.id; 
        this.setStartRoute();
        this.load();
        this.loadingInfo.isResultFilter = true;
      })
  }

  private initProperties(search?: string) {
    this.loadingInfo = new LoadingInfoModel();
    this.paginationInfo = {} as IPagination;
    this.content = "";
    this.filter = {
      search: search,
      page: 1,
      pageSize: 20,
    } as INotificationSettingFilter;
  }

  private setStartRoute() {
    this.router.navigate(['./'], {
      relativeTo: this.activatedRoute,
      queryParams: this.filter,
      queryParamsHandling: ""
    })
  }

  private load(isSearch?: boolean) : void {
    this.loadingInfo.isLoading = true;
    setTimeout(() =>{
      return  this.service.getNotificationSettings(this.filter)
        .pipe(takeUntil(this.destroy$))
        .subscribe(notificationSettings => {
          this.loadingInfo.isLoading = false;

          const handleErrorResult = this.handleError((notificationSettings as IError)?.status);
          if (handleErrorResult){
            return;
          }

          if (notificationSettings === null && isSearch){
            this.loadingInfo.isNoContent = true;
            return;
          }

          this.items = [];
          this.items.push(...(notificationSettings as INotificationSettings).items);

          this.paginationInfo = this.getFilledPaginationInfo(notificationSettings as INotificationSettings);
        });
    }, 100); // для демонстрации прогресс бара
  }

  private handleError(status: number): boolean {
    if (!status) {
      return false;
    }

    switch (status) {
      case HttpStatusCode.NotFound:
        this.loadingInfo.isNotFound = true;
        return true;
      case HttpStatusCode.Unauthorized:
        this.loadingInfo.isUnauthorized = true;
        return true;
      default: 
        return false;
    }
  }

  private getFilledPaginationInfo(notificationSettings: INotificationSettings): IPagination {
    return {
      page: notificationSettings.page,
      pageSize: notificationSettings.pageSize,
      totalPages: notificationSettings.totalPages,
      totalItems: notificationSettings.totalItems,
      items: notificationSettings.items.length
    } as IPagination;
  }
}
