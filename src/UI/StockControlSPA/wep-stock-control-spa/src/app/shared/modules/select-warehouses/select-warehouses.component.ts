import { Component, Input, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { Subject, debounceTime, takeUntil } from 'rxjs';
import { IPagination } from '../pagination/interfaces/pagination.intarface';
import { ISelectWarehouseFilter } from './interfaces/select-warehouse-filter.interface';
import { INamedEntity } from '../../interfaces/named-entity.interface';
import { MatAutocomplete } from '@angular/material/autocomplete';
import { SelectWarehousesService } from './services/select-warehouses.service';
import { ISelectWarehouses } from './interfaces/select-warehouses.interface';
import { ISelectWarehouseFilterParams } from './interfaces/select-warehouse-filter-params.interface';

@Component({
  selector: 'app-select-warehouses',
  templateUrl: './select-warehouses.component.html',
  styleUrls: ['./select-warehouses.component.scss']
})
export class SelectWarehousesComponent implements OnInit, OnDestroy {
  private readonly destroy$ = new Subject<void>();

  paginationInfo: IPagination;
  filter: ISelectWarehouseFilter;

  items: INamedEntity[] = [];

  isNoContent: boolean;

  @Input()
  name: string;

  @Input()
  disabled: boolean;

  @Input()
  fieldName = 'Склад';

  @Input()
  isRequired = false;

  @Input()
  params: ISelectWarehouseFilterParams;

  @Output()
  changedSelect = new Subject<INamedEntity>();

  @ViewChild('auto', { static: true }) auto: MatAutocomplete;
    
  constructor(
    private service: SelectWarehousesService) {
  }

  ngOnInit(): void {
    this.initPropertiesForFirstLoad();
    this.initSubscriptions();
    this.load(false);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  getErrorMessage(): string {
    return `Поле "${this.fieldName.toLowerCase()}" обязательно`;
  }

  onChangedInput(event: any) {
    setTimeout(() => {
      this.filter.page = 1;
      this.filter.search = event;
      this.load(false);
      this.setItems(event as string);
    }, 300);
  }

  loadSelectItems(): void {
    this.filter.page = 1;
    this.filter.search = '';
    this.load(false);
  }

  private initPropertiesForFirstLoad(search?: string) {
    this.paginationInfo = {} as IPagination;
    this.filter = {
      search: search,
      page: 1,
      pageSize: 10,
    } as ISelectWarehouseFilter;
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
  }

  private registerScrollEvent() : void {
    const panel = this.auto.panel.nativeElement;
    panel.addEventListener('scroll', (event: any) => {
      if (this.hasToBottomScroll(event.target)){
        this.filter.page = this.filter.page + 1;
        this.load(true);
      }
    });
  }

  private hasToBottomScroll(target: any) : boolean {
    return (target.offsetHeight + target.scrollTop >= target.scrollHeight - 1);
  }

  private load(canNextPage: boolean): void {
    this.setParams();
    this.service.selectWarehouses(this.filter)
      .pipe(takeUntil(this.destroy$))
      .subscribe(warehouses => {
        if (!warehouses){
          this.items = [];
          this.isNoContent = true;
          return;
        }

        this.isNoContent = false;

        if (canNextPage){
          this.items.push(...(warehouses as ISelectWarehouses).items);
        } else{
          this.items = (warehouses as ISelectWarehouses).items.slice();
        }

        this.paginationInfo = this.getFilledPaginationInfo(warehouses);
      });
    }

  private setParams() : void {
    if (!this.params){
      return;
    }
    this.filter.nomenclatureId = this.params.nomenclatureId;
    this.filter.organizationId = this.params.organizationId;
    this.filter.partyId = this.params.partyId;
  }
  
  private getFilledPaginationInfo(selectItems: any): IPagination {
    return {
      page: selectItems.page,
      pageSize: selectItems.pageSize,
      totalPages: selectItems.totalPages,
      totalItems: selectItems.totalItems,
      items: selectItems.items.length
    } as IPagination;
  }

  private setItems(name: string) : void {
    const item = this.items.find(i => i.name === name);   
    if (item) {
      this.changedSelect.next(item);
    }
    else {
      this.changedSelect.next({} as INamedEntity);
    }
  }
}
