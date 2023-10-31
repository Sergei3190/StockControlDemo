import { Component, ComponentRef, HostListener, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { IWarehouses } from '../../interfaces/warehouse/warehouses.interface';
import { IPagination } from 'src/app/shared/modules/pagination/interfaces/pagination.intarface';
import { Subject, skipWhile, switchMap, takeUntil, tap } from 'rxjs';
import { IWarehouseFilter } from '../../interfaces/warehouse/warehouse-filter.interface';
import { ActivatedRoute, Router } from '@angular/router';
import { WarehousesService } from './services/warehouses.service';
import { IWarehouseItem } from '../../interfaces/warehouse/warehouse-item.interface';
import { ToastrService } from 'ngx-toastr';
import { MatTableDataSource } from '@angular/material/table';
import { IOrder } from 'src/app/shared/interfaces/order.interface';
import { Guid } from 'guid-ts';
import { Sort } from '@angular/material/sort';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ConfirmDeleteComponent } from 'src/app/shared/modules/confirm-delete/confirm-delete.component';
import { SideDrawerContainerComponent } from 'src/app/shared/modules/side-drawer/side-drawer-container/side-drawer-container.component';
import { SideDrawerBaseComponent } from 'src/app/shared/modules/side-drawer/side-drawer-base/side-drawer-base.component';
import { ISideDrawerContainerConfig } from 'src/app/shared/modules/side-drawer/interfaces/side-drawer-container-config.interface';
import { WarehouseItemComponent } from '../warehouse-item/warehouse-item.component';
import { ISideDrawerConfig } from 'src/app/shared/modules/side-drawer/interfaces/side-drawer-config.interface';
import { WarehouseItemCreateComponent } from '../warehouse-item-create/warehouse-item-create.component';
import { SelectionModel } from '@angular/cdk/collections';
import { LoadingInfoModel } from 'src/app/shared/models/loading-info.model';
import { IError } from 'src/app/shared/interfaces/error.interface';
import { IBulkDeleteResult } from 'src/app/shared/interfaces/bulk-delete-result.interface';
import { HttpStatusCode } from '@angular/common/http';

@Component({
  selector: 'app-warehouse-list',
  templateUrl: './warehouse-list.component.html',
  styleUrls: ['./warehouse-list.component.scss']
})
export class WarehouseListComponent implements OnInit, OnDestroy  {
  private readonly destroy$ = new Subject<void>();

  displayedColumns: string[] = ['select', 'name', 'actions'];
  dataSource = new MatTableDataSource(new Array<IWarehouseItem>());

  selection = new SelectionModel<IWarehouseItem>(true, []);
  selected: IWarehouseItem[] = [];

  @ViewChild('container', { static: true }) container: SideDrawerContainerComponent;
  private openedDrawer: ComponentRef<SideDrawerBaseComponent>;
  
  loadingInfo: LoadingInfoModel;
  
  canClearSearch = new Subject<boolean>();
  reload = () => {
    this.clearFilter();
  };

  content?: string;
  paginationInfo: IPagination;
  filter: IWarehouseFilter

  sideDrawerDataChange = new Subject<WarehouseItemComponent>;

  constructor(private dialog: MatDialog,
    private service: WarehousesService,
    private toastr: ToastrService,
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

  addItem(): void {
    const dialogConfig = new MatDialogConfig();

    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = false;
    dialogConfig.width = '370px';
    dialogConfig.position = { top: '50px' };
    dialogConfig.data = {
      title: 'Создание склада'
    };

    const dialogRef = this.dialog.open(WarehouseItemCreateComponent , dialogConfig);

    dialogRef.afterClosed()
      .pipe(
        skipWhile(d => !d),
        switchMap(data => this.service.createWarehouse(data)),
        tap(result => {
          if (result){
            this.toastr.success('Склад успешно создан');
            this.load();
          }
        }),
        takeUntil(this.destroy$)) 
      .subscribe(_ => {})
  }

  editItem(id: Guid): void {
    this.service.getWarehouseById(id)
      .pipe(
        takeUntil(this.destroy$)
        )
      .subscribe(warehouse => {
        if (!warehouse){
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
          type : WarehouseItemComponent,
          data : {
            title: 'Редактирование',
            model: {...warehouse as IWarehouseItem},
            update: (model) => {
              this.service.updateWarehouse(model)
                .pipe(
                  takeUntil(this.destroy$)
                )
                .subscribe(flag => {
                  if (flag){
                    this.toastr.success('Склад успешно изменен');
                  }
                });
            },
            sideDrawerDataChange: this.sideDrawerDataChange
          },
          close: () => {
            this.container.closeDrawer();
            this.setStartRoute();
          } 
        } as ISideDrawerConfig<WarehouseItemComponent>;

        this.openedDrawer = this.container.openDrawer(containerConfig, drawerConfig);
      });
  }

  deleteItem(id: Guid): void {
    const dialogConfig = new MatDialogConfig();

    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.position = { top: '50px' };
    dialogConfig.data = {
      title: 'Подтверждение удаления склада',
      content: 'Вы уверены, что хотите удалить выбранный склад?'
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
        this.service.deleteWarehouse(id)
          .pipe(
            takeUntil(this.destroy$)
          )
          .subscribe(warehouse => {
            if ((warehouse as IError).status){
              return;
            }
            if (this.paginationInfo.items - 1 === 0 && this.paginationInfo.page > 1){
              this.filter.page = this.filter.page - 1;
            }
            this.toastr.success('Склад успешно удален');
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
        console.log('bulk',result);

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
    } as IWarehouseFilter;
  }

  private initSubscriptions() {
    this.sideDrawerDataChange
      .pipe(
        skipWhile(m => !m),
        takeUntil(this.destroy$)
      )
      .subscribe(data => {
        let item = this.dataSource.data.find(d => d.id === data.model.id) as IWarehouseItem;
        item = Object.assign(item, data.model);
      })
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
      return  this.service.getWarehouses(this.filter)
        .pipe(takeUntil(this.destroy$))
        .subscribe(warehouses => {          
          this.loadingInfo.isLoading = false;

          const handleErrorResult = this.handleError((warehouses as IError)?.status);
          if (handleErrorResult){
            return;
          }

          if (warehouses === null && isSearch){
            this.loadingInfo.isNoContent = true;
            return;
          }

          if (warehouses === null && !isSearch){
            this.paginationInfo = this.getEmptyPaginationInfo();
            return;
          }

          this.dataSource.data = (warehouses as IWarehouses).items;

          this.setSeletedItems();

          this.paginationInfo = this.getFilledPaginationInfo(warehouses as IWarehouses);
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

  private getFilledPaginationInfo(warehouses: IWarehouses): IPagination {
    return {
      page: warehouses.page,
      pageSize: warehouses.pageSize,
      totalPages: warehouses.totalPages,
      totalItems: warehouses.totalItems,
      items: warehouses.items.length
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
