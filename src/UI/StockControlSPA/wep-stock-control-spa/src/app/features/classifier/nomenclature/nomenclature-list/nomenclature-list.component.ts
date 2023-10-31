import { Component, ComponentRef, HostListener, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Subject, skipWhile, takeUntil, switchMap, tap, of, map } from 'rxjs';
import { IPagination } from 'src/app/shared/modules/pagination/interfaces/pagination.intarface';
import { INomenclatures } from '../../interfaces/nomenclature/nomenclatures.interface';
import { NomenclaturesService } from './services/nomenclatures.service';
import { ActivatedRoute, Router } from '@angular/router';
import { INomenclatureFilter } from '../../interfaces/nomenclature/nomenclature-filter.interface';
import { INomenclatureItem } from '../../interfaces/nomenclature/nomenclature-item.interface';
import { MatTableDataSource } from '@angular/material/table';
import { Sort } from '@angular/material/sort';
import { IOrder } from 'src/app/shared/interfaces/order.interface';
import { Guid } from 'guid-ts';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ConfirmDeleteComponent } from 'src/app/shared/modules/confirm-delete/confirm-delete.component';
import { ToastrService } from 'ngx-toastr';
import { ISideDrawerContainerConfig } from 'src/app/shared/modules/side-drawer/interfaces/side-drawer-container-config.interface';
import { NomenclatureItemComponent } from '../nomenclature-item/nomenclature-item.component';
import { ISideDrawerConfig } from 'src/app/shared/modules/side-drawer/interfaces/side-drawer-config.interface';
import { SideDrawerContainerComponent } from 'src/app/shared/modules/side-drawer/side-drawer-container/side-drawer-container.component';
import { SideDrawerBaseComponent } from 'src/app/shared/modules/side-drawer/side-drawer-base/side-drawer-base.component';
import { IOrganizationItem } from '../../interfaces/organization/organization-item.interface';
import { NomenclatureItemCreateComponent } from '../nomenclature-item-create/nomenclature-item-create.component';
import { SelectionModel } from '@angular/cdk/collections';
import { LoadingInfoModel } from 'src/app/shared/models/loading-info.model';
import { IError } from 'src/app/shared/interfaces/error.interface';
import { IBulkDeleteResult } from 'src/app/shared/interfaces/bulk-delete-result.interface';
import { HttpStatusCode } from '@angular/common/http';

@Component({
  selector: 'app-nomenclature-list',
  templateUrl: './nomenclature-list.component.html',
  styleUrls: ['./nomenclature-list.component.scss']
})
export class NomenclatureListComponent implements OnInit, OnDestroy {
  private readonly destroy$ = new Subject<void>();

  displayedColumns: string[] = ['select', 'name', 'actions'];
  dataSource = new MatTableDataSource(new Array<INomenclatureItem>());

  selection = new SelectionModel<INomenclatureItem>(true, []);
  selected: INomenclatureItem[] = [];

  @ViewChild('container', { static: true }) container: SideDrawerContainerComponent;
  private openedDrawer: ComponentRef<SideDrawerBaseComponent>;
  
  loadingInfo: LoadingInfoModel;
  
  canClearSearch = new Subject<boolean>();
  reload = () => {
    this.clearFilter();
  };

  content?: string = "";
  paginationInfo: IPagination;
  filter: INomenclatureFilter

  sideDrawerDataChange = new Subject<NomenclatureItemComponent>;

  constructor(private dialog: MatDialog,
    private service: NomenclaturesService,
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
      title: 'Создание номенклатуры'
    };

    const dialogRef = this.dialog.open(NomenclatureItemCreateComponent , dialogConfig);

    dialogRef.afterClosed()
      .pipe(
        skipWhile(d => !d),
        switchMap(data => this.service.createNomenclature(data)),
        tap(result => {
          if (result){
            this.toastr.success('Номенклатура успешно создана');
            this.load();
          }
        }),
        takeUntil(this.destroy$))
      .subscribe(_ => {})
  }

  editItem(id: Guid): void {
    this.service.getNomenclatureById(id)
      .pipe(
        takeUntil(this.destroy$)
        )
      .subscribe(nomenclature => {
        if (!nomenclature){
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
          type : NomenclatureItemComponent,
          data : {
            title: 'Редактирование',
            model: {...nomenclature as INomenclatureItem},
            update: (model) => {
              this.service.updateNomenclature(model)
                .pipe(
                  takeUntil(this.destroy$)
                )
                .subscribe(flag => {
                  if (flag){
                    this.toastr.success('Номенклатура успешно изменена');
                  }
                });
            },
            sideDrawerDataChange: this.sideDrawerDataChange
          },
          close: () => {
            this.container.closeDrawer();
            this.setStartRoute();
          } 
        } as ISideDrawerConfig<NomenclatureItemComponent>;

        this.openedDrawer = this.container.openDrawer(containerConfig, drawerConfig);
      });
  }

  deleteItem(id: Guid): void {
    const dialogConfig = new MatDialogConfig();

    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.position = { top: '50px' };
    dialogConfig.data = {
      title: 'Подтверждение удаления номенклатуры',
      content: 'Вы уверены, что хотите удалить выбранную номенклатуру?'
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
        this.service.deleteNomenclature(id)
          .pipe(
            takeUntil(this.destroy$)
          )
          .subscribe(nomenclature => {
            if ((nomenclature as IError).status){
              return;
            }
            if (this.paginationInfo.items - 1 === 0 && this.paginationInfo.page > 1){
              this.filter.page = this.filter.page - 1;
            }
            this.toastr.success('Номенклатура успешно удалена');
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
      pageSize: 18,
    } as INomenclatureFilter;
  }

  private initSubscriptions() {
    this.sideDrawerDataChange
      .pipe(
        skipWhile(m => !m),
        takeUntil(this.destroy$)
      )
      .subscribe(data => {
        let item = this.dataSource.data.find(d => d.id === data.model.id) as IOrganizationItem;
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
    console.log('load',this.filter);
    this.loadingInfo.isLoading = true;
    setTimeout(() =>{
      return  this.service.getNomenclatures(this.filter)
        .pipe(takeUntil(this.destroy$))
        .subscribe(nomenclatures => {
          this.loadingInfo.isLoading = false;

          const handleErrorResult = this.handleError((nomenclatures as IError)?.status);
          if (handleErrorResult){
            return;
          }

          if (nomenclatures === null && isSearch){
            this.loadingInfo.isNoContent = true;
            return;
          }

          if (nomenclatures === null && !isSearch){
            this.paginationInfo = this.getEmptyPaginationInfo();
            return;
          }
        
          this.dataSource.data = (nomenclatures as INomenclatures).items;

          this.setSeletedItems();

          this.paginationInfo = this.getFilledPaginationInfo(nomenclatures as INomenclatures);
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

  private getFilledPaginationInfo(nomenclatures: INomenclatures): IPagination {
    return {
      page: nomenclatures.page,
      pageSize: nomenclatures.pageSize,
      totalPages: nomenclatures.totalPages,
      totalItems: nomenclatures.totalItems,
      items: nomenclatures.items.length
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
