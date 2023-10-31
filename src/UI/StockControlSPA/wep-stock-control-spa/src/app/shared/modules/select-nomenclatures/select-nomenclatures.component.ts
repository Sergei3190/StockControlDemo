import { Component, Input, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { IPagination } from '../pagination/interfaces/pagination.intarface';
import { Subject, debounceTime, takeUntil } from 'rxjs';
import { ISelectNomenclatureFilter } from './interfaces/select-nomenclature-filter.interface';
import { INamedEntity } from '../../interfaces/named-entity.interface';
import { MatAutocomplete } from '@angular/material/autocomplete';
import { SelectNomenclaturesService } from './services/select-nomenclatures.service';
import { ISelectNomenclatures } from './interfaces/select-nomenclatures.interface';
import { ISelectNomenclatureFilterParams } from './interfaces/select-nomenclature-filter-params.interface';

@Component({
  selector: 'app-select-nomenclatures',
  templateUrl: './select-nomenclatures.component.html',
  styleUrls: ['./select-nomenclatures.component.scss']
})
export class SelectNomenclaturesComponent implements OnInit, OnDestroy {
  private readonly destroy$ = new Subject<void>();

  paginationInfo: IPagination;
  filter: ISelectNomenclatureFilter;

  items: INamedEntity[] = [];

  isNoContent: boolean;
  isLoading: boolean;

  @Input()
  name: string;

  @Input()
  disabled = false;

  @Input()
  isRequired = false;

  @Input()
  params: ISelectNomenclatureFilterParams;

  @Output()
  changedSelect = new Subject<INamedEntity>();

  @ViewChild('auto', { static: true }) auto: MatAutocomplete;
    
  constructor(
    private service: SelectNomenclaturesService) {
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
    return 'Поле "Номенклатура" обязательно';
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
    } as ISelectNomenclatureFilter;
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
    this.service.selectNomenclatures(this.filter)
      .pipe(takeUntil(this.destroy$))
      .subscribe(nomenclatures => {

        if (!nomenclatures){
          this.items = [];
          this.isNoContent = true;
          return;
        }

        this.isNoContent = false;
        
        if (canNextPage){
          this.items.push(...(nomenclatures as ISelectNomenclatures).items);
        } else{
          this.items = (nomenclatures as ISelectNomenclatures).items.slice();
        }

        this.paginationInfo = this.getFilledPaginationInfo(nomenclatures);
      });
    }

  private setParams() : void {
    if (!this.params){
      return;
    }
    this.filter.organizationId = this.params.organizationId;
    this.filter.partyId = this.params.partyId;
    this.filter.warehouseId = this.params.warehouseId;
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
