import { Component, ComponentRef, HostListener, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Subject, skipWhile, takeUntil } from 'rxjs';
import { IPagination } from 'src/app/shared/modules/pagination/interfaces/pagination.intarface';
import { IStockAvailabilities } from '../interfaces/stock-availabilities.interface';
import { IStockAvailabilityItem } from '../interfaces/stock-availability-item.interface';
import { MatTableDataSource } from '@angular/material/table';
import { SideDrawerContainerComponent } from 'src/app/shared/modules/side-drawer/side-drawer-container/side-drawer-container.component';
import { SideDrawerBaseComponent } from 'src/app/shared/modules/side-drawer/side-drawer-base/side-drawer-base.component';
import { IStockAvailabilityFilter } from '../interfaces/stock-availability-filter.interface';
import { StockAvailabilityItemComponent } from '../stock-availability-item/stock-availability-item.component';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { StockAvailabilitiesService } from './services/stock-availabilities.service';
import { ActivatedRoute, Router } from '@angular/router';
import { IOrder } from 'src/app/shared/interfaces/order.interface';
import { Sort } from '@angular/material/sort';
import { Guid } from 'guid-ts';
import { ISideDrawerContainerConfig } from 'src/app/shared/modules/side-drawer/interfaces/side-drawer-container-config.interface';
import { ISideDrawerConfig } from 'src/app/shared/modules/side-drawer/interfaces/side-drawer-config.interface';
import { StockAvailabilityFilterComponent } from '../stock-availability-filter/stock-availability-filter.component';
import { StockAvailabilityItem } from '../models/stock-availability-item.model';
import { LoadingInfoModel } from 'src/app/shared/models/loading-info.model';
import { IError } from 'src/app/shared/interfaces/error.interface';
import { HttpStatusCode } from '@angular/common/http';
import { SignalrService } from '../../../shared/modules/signalr/services/signalr.service';

@Component({
  selector: 'app-stock-availability-list',
  templateUrl: './stock-availability-list.component.html',
  styleUrls: ['./stock-availability-list.component.scss']
})
export class StockAvailabilityListComponent implements OnInit, OnDestroy  {
  private readonly destroy$ = new Subject<void>();

  displayedColumns: string[] = [
    'nomenclature', 
    'party',
    'warehouse',
    'organization', 
    'price', 
    'quantity', 
    'totalPrice'
  ];

  dataSource = new MatTableDataSource(new Array<StockAvailabilityItem>());

  @ViewChild('container', { static: true }) container: SideDrawerContainerComponent;
  private openedDrawer: ComponentRef<SideDrawerBaseComponent>;
  
  loadingInfo: LoadingInfoModel;
  
  canClearSearch = new Subject<boolean>();
  reload = () => {
    this.clearFilter();
  };

  content?: string;
  paginationInfo: IPagination;
  filter: IStockAvailabilityFilter

  constructor(private dialog: MatDialog,
    private service: StockAvailabilitiesService,
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
    this.checkExistsDrawer();
    this.clearFilter();
  }

  announceSortChange(sortState: Sort) {
    this.filter.order = {
      column: sortState.active,
      direction: sortState.direction
    } as IOrder;
    this.filter.page = 1;
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
      title: 'Фильтр остатков'
    };

    const dialogRef = this.dialog.open(StockAvailabilityFilterComponent , dialogConfig);

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

  openItem(id: Guid): void {
    this.service.getById(id)
      .pipe(
        takeUntil(this.destroy$)
        )
      .subscribe(stockAvailability => {
        if (!stockAvailability){
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
          type: StockAvailabilityItemComponent,
          data: {
            title: `${(stockAvailability as IStockAvailabilityItem).nomenclature.name}`,
            model: { ...stockAvailability as IStockAvailabilityItem },
          },
          close: () => {
            this.container.closeDrawer();
            this.setStartRoute();
          }
        } as ISideDrawerConfig<StockAvailabilityItemComponent>;

        this.openedDrawer = this.container.openDrawer(containerConfig, drawerConfig);
      });
  }

  clearFilter(): void {
    this.canClearSearch.next(true);
    this.ngOnInit();
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
    } as IStockAvailabilityFilter;
  }

  private initSubscriptions() {
    this.signalrService.messageReceived
      .pipe(
        takeUntil(this.destroy$)
      )
      .subscribe(msg => {
        console.log('stock-availability signalR', msg);
        this.load();
      });
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
      return  this.service.getList(this.filter)
        .pipe(takeUntil(this.destroy$))
        .subscribe(stockAvailabilities => {
          this.loadingInfo.isLoading = false;

          const handleErrorResult = this.handleError((stockAvailabilities as IError)?.status);
          if (handleErrorResult){
            return;
          }
  
          if (stockAvailabilities === null && isSearch){
            this.loadingInfo.isNoContent = true;
            return;
          }

          if (stockAvailabilities === null && !isSearch){
            this.paginationInfo = this.getEmptyPaginationInfo();
            return;
          }

          this.dataSource.data = (stockAvailabilities as IStockAvailabilities).items;

          this.paginationInfo = this.getFilledPaginationInfo(stockAvailabilities as IStockAvailabilities);
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

  private getFilledPaginationInfo(stockAvailabilities: IStockAvailabilities): IPagination {
    return {
      page: stockAvailabilities.page,
      pageSize: stockAvailabilities.pageSize,
      totalPages: stockAvailabilities.totalPages,
      totalItems: stockAvailabilities.totalItems,
      items: stockAvailabilities.items.length
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
