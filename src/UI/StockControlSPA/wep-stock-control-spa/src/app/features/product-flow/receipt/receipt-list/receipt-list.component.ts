import { Component, ComponentRef, HostListener, Input, OnDestroy, OnInit, ViewChild, Output } from '@angular/core';
import { ReceiptItem } from '../../models/receipt-item.model';
import { MatTableDataSource } from '@angular/material/table';
import { Observable, Subject, pipe, skipWhile, switchMap, takeUntil, tap } from 'rxjs';
import { SideDrawerContainerComponent } from 'src/app/shared/modules/side-drawer/side-drawer-container/side-drawer-container.component';
import { SideDrawerBaseComponent } from 'src/app/shared/modules/side-drawer/side-drawer-base/side-drawer-base.component';
import { LoadingInfoModel } from 'src/app/shared/models/loading-info.model';
import { IPagination } from 'src/app/shared/modules/pagination/interfaces/pagination.intarface';
import { IReceiptFilter } from '../../interfaces/receipt/receip-filter.interface';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ReceiptsService } from './services/receipts.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Sort } from '@angular/material/sort';
import { IOrder } from 'src/app/shared/interfaces/order.interface';
import { ReceiptFilterComponent } from '../receipt-filter/receipt-filter.component';
import { Guid } from 'guid-ts';
import { ISideDrawerContainerConfig } from 'src/app/shared/modules/side-drawer/interfaces/side-drawer-container-config.interface';
import { ReceiptItemComponent } from '../receipt-item/receipt-item.component';
import { IReceiptItem } from '../../interfaces/receipt/receipt-item.interface';
import { ISideDrawerConfig } from 'src/app/shared/modules/side-drawer/interfaces/side-drawer-config.interface';
import { IError } from 'src/app/shared/interfaces/error.interface';
import { IReceipts } from '../../interfaces/receipt/receipts.interface';
import { HttpStatusCode } from '@angular/common/http';
import { ReceiptItemCreateComponent } from '../receipt-item-create/receipt-item-create.component';
import { ToastrService } from 'ngx-toastr';
import { ConfirmDeleteComponent } from 'src/app/shared/modules/confirm-delete/confirm-delete.component';
import { IBulkDeleteResult } from 'src/app/shared/interfaces/bulk-delete-result.interface';
import { SelectionModel } from '@angular/cdk/collections';
import { productFlowUrls } from '../../product-flow-routing.module';
import { SignalrService } from 'src/app/shared/modules/signalr/services/signalr.service';

@Component({
  selector: 'app-receipt-list',
  templateUrl: './receipt-list.component.html',
  styleUrls: ['./receipt-list.component.scss']
})
export class ReceiptListComponent implements OnInit, OnDestroy  {
  private readonly destroy$ = new Subject<void>();

  displayedColumns: string[] = [
    'select',
    'number',
    'createDate',
    'createTime',
    'nomenclature', 
    'party',
    'warehouse',
    'organization', 
    'price', 
    'quantity', 
    'totalPrice',
    'actions'
  ];

  dataSource = new MatTableDataSource(new Array<ReceiptItem>());

  selection = new SelectionModel<ReceiptItem>(true, []);

  selected: ReceiptItem[] = [];

  @ViewChild('container', { static: true }) container: SideDrawerContainerComponent;
  private openedDrawer: ComponentRef<SideDrawerBaseComponent>;
  
  loadingInfo: LoadingInfoModel;
  
  canClearSearch = new Subject<boolean>();
  reload = () => {
    this.clearFilter();
  };

  content?: string;
  paginationInfo: IPagination;
  filter: IReceiptFilter

  sideDrawerDataChange = new Subject<ReceiptItemComponent>;

  @Input()
  footerDisabled = false;

  @Input()
  selectedPage: Observable<number>;

  @Output()
  paginationInfoChanged = new Subject<IPagination>();

  constructor(private dialog: MatDialog,
    private service: ReceiptsService,
    private toastr: ToastrService,
    private signalrService: SignalrService,
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
      title: 'Фильтр поступлений'
    };

    const dialogRef = this.dialog.open(ReceiptFilterComponent , dialogConfig);

    dialogRef.afterClosed()
      .pipe(
        skipWhile(d => !d),
        takeUntil(this.destroy$))
      .subscribe(data => {
        this.filter.partyId = data?.party?.id; 
        this.filter.nomenclatureId = data?.nomenclature?.id; 
        this.filter.organizationId = data?.organization?.id; 
        this.filter.warehouseId = data?.warehouse?.id; 
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
      title: 'Создание поступления'
    };

    const dialogRef = this.dialog.open(ReceiptItemCreateComponent , dialogConfig);

    dialogRef.afterClosed()
      .pipe(
        skipWhile(d => !d),
        switchMap(data => this.service.createReceipt(data)),
        tap(result => {
          const error = (result as IError).status;
          if (error){
            return;
          }

          this.toastr.success('Поступление успешно создано');
          this.load();
        }),
        takeUntil(this.destroy$))
      .subscribe(_ => {})
  }

  editItem(id: Guid): void {
    this.service.getReceiptById(id)
      .pipe(
        takeUntil(this.destroy$)
        )
      .subscribe(receipt => {
        if (!receipt){
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
          type : ReceiptItemComponent,
          data : {
            title: 'Редактирование',
            model: {...receipt as IReceiptItem},
            update: (model) => {
              this.service.updateReceipt(model)
                .pipe(
                  takeUntil(this.destroy$)
                )
                .subscribe(flag => {
                  const error = (flag as IError).status;
                  if (error || !flag){
                    return;
                  }
                  this.toastr.success('Поступление успешно изменено');
                });
            },
            sideDrawerDataChange: this.sideDrawerDataChange
          },
          close: () => {
            this.container.closeDrawer();
            this.setStartRoute();
          } 
        } as ISideDrawerConfig<ReceiptItemComponent>;

        this.openedDrawer = this.container.openDrawer(containerConfig, drawerConfig);
      });
  }

  deleteItem(id: Guid): void {
    const dialogConfig = new MatDialogConfig();

    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.position = { top: '50px' };
    dialogConfig.data = {
      title: 'Подтверждение удаления поступления',
      content: 'Вы уверены, что хотите удалить выбранное поступление?'
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
        this.service.deleteReceipt(id)
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
            this.toastr.success('Поступление успешно удалено');
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
    } as IReceiptFilter;
  }

  private initSubscriptions() {
    this.sideDrawerDataChange
      .pipe(
        skipWhile(m => !m),
        takeUntil(this.destroy$)
      )
      .subscribe(data => {
        let item = this.dataSource.data.find(d => d.id === data.model.id) as IReceiptItem;
        item = Object.assign(item, data.model);
      });

    this.signalrService.messageReceived
    .pipe(
      takeUntil(this.destroy$)
    )
    .subscribe(msg => {
      // можно конечно прописать отдельную логику для каждой операции (создания, удаления, обновления и массового удаления),
      // например, чтобы данные обновлялись без перезагрузки, но
      // в целях демонстрации будем просто обновлять список
      console.log('receipt signalR', msg);
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

  private setStartRoute() {
    this.router.navigate([`./${productFlowUrls.receipts}`], {
      relativeTo: this.activatedRoute,
      queryParams: this.filter,
      queryParamsHandling: ""
    })
  }

  private load(isSearch?: boolean) : void {
    this.loadingInfo.isLoading = true;
    setTimeout(() =>{
      return  this.service.getReceipts(this.filter)
        .pipe(takeUntil(this.destroy$))
        .subscribe(receipts => {
          this.loadingInfo.isLoading = false;

          const handleErrorResult = this.handleError((receipts as IError)?.status);
          if (handleErrorResult){
            return;
          }
  
          if (receipts === null && isSearch){
            this.loadingInfo.isNoContent = true;
            return;
          }

          if (receipts === null && !isSearch) {
            this.paginationInfo = this.getEmptyPaginationInfo();
            this.paginationInfoChanged.next(this.paginationInfo);
            return;
          }

          this.dataSource.data = (receipts as IReceipts).items;

          this.setSeletedItems();

          this.paginationInfo = this.getFilledPaginationInfo(receipts as IReceipts);
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

  private getFilledPaginationInfo(receipts: IReceipts): IPagination {
    return {
      page: receipts.page,
      pageSize: receipts.pageSize,
      totalPages: receipts.totalPages,
      totalItems: receipts.totalItems,
      items: receipts.items.length
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
