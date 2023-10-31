import { Component, ComponentRef, HostListener, Input, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { Observable, Subject, skipWhile, switchMap, takeUntil, tap } from 'rxjs';
import { WriteOffItem } from '../../models/write-off-item.model';
import { SelectionModel } from '@angular/cdk/collections';
import { SideDrawerContainerComponent } from 'src/app/shared/modules/side-drawer/side-drawer-container/side-drawer-container.component';
import { SideDrawerBaseComponent } from 'src/app/shared/modules/side-drawer/side-drawer-base/side-drawer-base.component';
import { LoadingInfoModel } from 'src/app/shared/models/loading-info.model';
import { IPagination } from 'src/app/shared/modules/pagination/interfaces/pagination.intarface';
import { IWriteOffFilter } from '../../interfaces/write-off/write-off-filter.interface';
import { WriteOffItemComponent } from '../write-off-item/write-off-item.component';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { WriteOffsService } from './services/write-offs.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { Sort } from '@angular/material/sort';
import { IOrder } from 'src/app/shared/interfaces/order.interface';
import { WriteOffFilterComponent } from '../write-off-filter/write-off-filter.component';
import { WriteOffItemCreateComponent } from '../write-off-item-create/write-off-item-create.component';
import { IError } from 'src/app/shared/interfaces/error.interface';
import { Guid } from 'guid-ts';
import { ISideDrawerContainerConfig } from 'src/app/shared/modules/side-drawer/interfaces/side-drawer-container-config.interface';
import { IWriteOffItem } from '../../interfaces/write-off/write-off-item.interface';
import { ISideDrawerConfig } from 'src/app/shared/modules/side-drawer/interfaces/side-drawer-config.interface';
import { ConfirmDeleteComponent } from 'src/app/shared/modules/confirm-delete/confirm-delete.component';
import { IBulkDeleteResult } from 'src/app/shared/interfaces/bulk-delete-result.interface';
import { productFlowUrls } from '../../product-flow-routing.module';
import { IWriteOffs } from '../../interfaces/write-off/write-offs.interface';
import { HttpStatusCode } from '@angular/common/http';
import { SignalrService } from 'src/app/shared/modules/signalr/services/signalr.service';
import { NotificationSettingsService } from 'src/app/shared/modules/notification-settings/services/notification-settings.service';
import { StorageService } from 'src/app/shared/modules/storage/services/storage.service';

@Component({
  selector: 'app-write-off-list',
  templateUrl: './write-off-list.component.html',
  styleUrls: ['./write-off-list.component.scss']
})
export class WriteOffListComponent implements OnInit, OnDestroy  {
  private readonly destroy$ = new Subject<void>();

  displayedColumns: string[] = [
    'select',
    'number',
    'createDate',
    'createTime',
    'nomenclature', 
    'party',
    'sendingWarehouse',
    'warehouse',
    'organization', 
    'price', 
    'quantity', 
    'totalPrice',
    'reason',
    'actions'
  ];

  dataSource = new MatTableDataSource(new Array<WriteOffItem>());

  selection = new SelectionModel<WriteOffItem>(true, []);

  selected: WriteOffItem[] = [];

  @ViewChild('container', { static: true }) container: SideDrawerContainerComponent;
  private openedDrawer: ComponentRef<SideDrawerBaseComponent>;
  
  loadingInfo: LoadingInfoModel;
  
  canClearSearch = new Subject<boolean>();
  reload = () => {
    this.clearFilter();
  };

  content?: string;
  paginationInfo: IPagination;
  filter: IWriteOffFilter

  sideDrawerDataChange = new Subject<WriteOffItemComponent>;

  @Input()
  footerDisabled = false;

  @Input()
  selectedPage: Observable<number>;

  @Output()
  paginationInfoChanged = new Subject<IPagination>();

  constructor(private dialog: MatDialog,
    private service: WriteOffsService,
    private toastr: ToastrService,
    private signalrService: SignalrService,
    private storageService: StorageService,
    private notificationSettingsService: NotificationSettingsService,
    private router: Router,
    private activatedRoute: ActivatedRoute){
  }

  ngOnInit(): void {
    this.initProperties();
    this.initSubscriptions();
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

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  toggleAllRows() {
    if (this.isAllSelected()) {
      this.selection.clear();
      return;
    }

    this.selection.select(...this.dataSource.data);
  }

  announceSortChange(sortState: Sort) {
    this.filter.order = {
      column: sortState.active,
      direction: sortState.direction
    } as IOrder;
    this.filter.page = 1;
    // когда применяем сортировку загружаем с первой страницы c сохранением всех предыдущих фильтров
    // в производственном приложении можно добавить сохранение сортировки и вообзще всех фильтров пользователя
    this.setStartRoute();
    this.load();
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

  openFilter(): void {
    const dialogConfig = new MatDialogConfig();

    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = false;
    dialogConfig.width = '400px';
    dialogConfig.position = { top: '50px' };
    dialogConfig.data = {
      title: 'Фильтр списаний'
    };

    const dialogRef = this.dialog.open(WriteOffFilterComponent , dialogConfig);

    dialogRef.afterClosed()
      .pipe(
        skipWhile(d => !d),
        takeUntil(this.destroy$))
      .subscribe(data => {
        this.filter.partyId = data?.party?.id; 
        this.filter.nomenclatureId = data?.nomenclature?.id; 
        this.filter.organizationId = data?.organization?.id; 
        this.filter.warehouseId = data?.warehouse?.id; 
        this.filter.sendingWarehouseId = data?.sendingWarehouse?.id;
        this.setStartRoute();
        this.load();
        this.loadingInfo.isResultFilter = true;
      })
  }

  addItem(): void {
    const dialogConfig = new MatDialogConfig();

    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = false;
    dialogConfig.width = '370px';
    dialogConfig.position = { top: '50px' };
    dialogConfig.data = {
      title: 'Создание списания'
    };

    const dialogRef = this.dialog.open(WriteOffItemCreateComponent , dialogConfig);

    dialogRef.afterClosed()
      .pipe(
        skipWhile(d => !d),
        switchMap(data => this.service.createWriteOff(data)),
        tap(result => {
          const error = (result as IError).status;
          if (error){
            return;
          }
          this.toastr.success('Списание успешно создано');
          this.load();
        }),
        takeUntil(this.destroy$))
      .subscribe(_ => {})
  }

  editItem(id: Guid): void {
    this.service.getWriteOffById(id)
      .pipe(
        takeUntil(this.destroy$)
        )
      .subscribe(WriteOff => {
        if (!WriteOff){
          return;
        }

        this.checkExistsDrawer();

        const containerConfig = { 
          hasBackdrop: 'false',
          autosize: 'false',
          mode: 'over', 
          position: 'end',
          drawerclass: 'modal-sm'
        } as ISideDrawerContainerConfig;

        const drawerConfig = {
          type : WriteOffItemComponent,
          data : {
            title: 'Редактирование',
            model: {...WriteOff as IWriteOffItem},
            update: (model) => {
              this.service.updateWriteOff(model)
                .pipe(
                  takeUntil(this.destroy$)
                )
                .subscribe(flag => {
                  const error = (flag as IError).status;
                  if (error || !flag){
                    return;
                  }
                  this.toastr.success('Списание успешно изменено');
                });
            },
            sideDrawerDataChange: this.sideDrawerDataChange
          },
          close: () => {
            this.container.closeDrawer();
            this.setStartRoute();
          } 
        } as ISideDrawerConfig<WriteOffItemComponent>;

        this.openedDrawer = this.container.openDrawer(containerConfig, drawerConfig);
      });
  }

  deleteItem(id: Guid): void {
    const dialogConfig = new MatDialogConfig();

    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.position = { top: '50px' };
    dialogConfig.data = {
      title: 'Подтверждение удаления списания',
      content: 'Вы уверены, что хотите удалить выбранное списание?'
    };

    const dialogRef = this.dialog.open(ConfirmDeleteComponent, dialogConfig);

    dialogRef.afterClosed()
      .pipe(
        skipWhile(d => !d),
        takeUntil(this.destroy$))
      .subscribe(flag => {
        if (!flag){
          return;
        }
        this.service.deleteWriteOff(id)
          .pipe(
            takeUntil(this.destroy$)
          )
          .subscribe(flag => {
            const error = (flag as IError).status;
            if (error || !flag){
              return;
            }
            if (this.paginationInfo.items - 1 === 0 && this.paginationInfo.page > 1){
              this.filter.page = this.filter.page - 1;
            }
            this.toastr.success('Списание успешно удалено');
            this.load();
          });
      })
  }

  clearFilter(): void {
    this.canClearSearch.next(true);
    this.ngOnInit();
  }

  bulkDelete() {
    this.service.bulkDelete(this.selection.selected.map(s => s.id))
      .pipe(
        takeUntil(this.destroy$)
      )
      .subscribe(result => {
        const handleErrorResult = this.handleError((result as IError)?.status);
        if (handleErrorResult){
          return;
        }

        const resultModel = result as IBulkDeleteResult;
        this.showBulkDeleteResult(resultModel);

        this.load();
      });
  }

  private showBulkDeleteResult(result: IBulkDeleteResult) {
    if (!result){
      return;
    }
    if ((result as IBulkDeleteResult).successMessage){
      this.toastr.success(result.successMessage?.message);
      this.clearSelectedItems(result.successMessage?.ids)
    }
    if (result.errorMessage){
      result.errorMessage.forEach(e => this.toastr.error(e, 'ОШИБКА'));
    }
  }

  private clearSelectedItems(ids?: Guid[]) {
    if (!ids){
      return;
    }

    this.selected = this.selected.filter(s => !ids.includes(s.id));
    this.selection.selected.forEach(s => {
      if (ids.includes(s.id)){
        this.selection.deselect(s);
      }
    });
  }

  private checkExistsDrawer() {
    if (this.openedDrawer) {
      this.openedDrawer.instance.close();
      this.openedDrawer.destroy();
    }
  }

  private initProperties(search?: string, order?: IOrder) {
    this.loadingInfo = new LoadingInfoModel();
    this.paginationInfo = {} as IPagination;
    this.content = "";
    this.filter = {
      search: search,
      order: order,
      page: 1,
      pageSize: 2,
    } as IWriteOffFilter;
  }

  private setStartRoute() {
    this.router.navigate([`./${productFlowUrls.writeOffs}`], {
      relativeTo: this.activatedRoute,
      queryParams: this.filter,
      queryParamsHandling: ""
    })
  }

  private initSubscriptions() {
    this.sideDrawerDataChange
      .pipe(
        skipWhile(m => !m),
        takeUntil(this.destroy$)
      )
      .subscribe(data => {
        let item = this.dataSource.data.find(d => d.id === data.model.id) as IWriteOffItem;
        item = Object.assign(item, data.model);
      })

    this.signalrService.messageReceived
      .pipe(
        takeUntil(this.destroy$)
      )
      .subscribe(msg => {
        // можно конечно прописать отдельную логику для каждой операции (создания, удаления, обновления и массового удаления),
        // например, чтобы данные обновлялись без перезагрузки, но
        // в целях демонстрации будем просто обновлять список
        console.log('write-off signalR', msg);
        this.load();
      });

    this.selectedPage
      .pipe(
        skipWhile(n => !n),
        tap(value => this.onPageChanged(value)),
        takeUntil(this.destroy$)
      )
      .subscribe();
  }

  private load(isSearch?: boolean) : void {
    this.loadingInfo.isLoading = true;
    setTimeout(() =>{
      return  this.service.getWriteOffs(this.filter)
        .pipe(takeUntil(this.destroy$))
        .subscribe(WriteOffs => {
          this.loadingInfo.isLoading = false;

          const handleErrorResult = this.handleError((WriteOffs as IError)?.status);
          if (handleErrorResult){
            return;
          }
  
          if (WriteOffs === null && isSearch){
            this.loadingInfo.isNoContent = true;
            return;
          }

          if (WriteOffs === null && !isSearch) {
            this.paginationInfo = this.getEmptyPaginationInfo();
            this.paginationInfoChanged.next(this.paginationInfo);
            return;
          }

          this.dataSource.data = (WriteOffs as IWriteOffs)?.items;

          this.setSeletedItems();

          this.paginationInfo = this.getFilledPaginationInfo(WriteOffs as IWriteOffs);
          this.paginationInfoChanged.next(this.paginationInfo);
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

  // данный метод помогает сохранить выбранные элементы при изменении сортировки и страницы
  private setSeletedItems() {
    this.selection.selected.forEach((s) => {
      if (!this.selected.includes(s)) {
        this.selected.push(s);
      }
    });
    this.selection.clear();

    this.dataSource.data.forEach((d) => {
      if (this.selected.some((s) => s.id === d.id)) {
          this.selection.select(d);
          this.selected = this.selected.filter((s) => s.id !== d.id);
      }
    });
  }

  private getFilledPaginationInfo(WriteOffs: IWriteOffs): IPagination {
    return {
      page: WriteOffs?.page,
      pageSize: WriteOffs?.pageSize,
      totalPages: WriteOffs?.totalPages,
      totalItems: WriteOffs?.totalItems,
      items: WriteOffs?.items?.length
    } as IPagination;
  }

  private getEmptyPaginationInfo(): IPagination {
    return {
      page: 0,
      pageSize: 0,
      totalPages: 0,
      totalItems: 0,
      items: 0,
    } as IPagination;
  }
}
