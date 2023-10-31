import { Component, Input, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { IPagination } from '../pagination/interfaces/pagination.intarface';
import { Subject, debounceTime, takeUntil } from 'rxjs';
import { ISelectPartyFilter } from './interfaces/select-party-filter.interface';
import { SelectPartiesService } from './services/select-parties.service';
import { MatAutocomplete } from '@angular/material/autocomplete';
import { INamedEntity } from '../../interfaces/named-entity.interface';
import { ISelectParties } from './interfaces/select-parties.interface';
import { ISelectPartyFilterParams } from './interfaces/select-party-filter-params.interface';

@Component({
  selector: 'app-select-parties',
  templateUrl: './select-parties.component.html',
  styleUrls: ['./select-parties.component.scss']
})
export class SelectPartiesComponent implements OnInit, OnDestroy {
  private readonly destroy$ = new Subject<void>();

  paginationInfo: IPagination;
  filter: ISelectPartyFilter;

  items: INamedEntity[] = [];

  isNoContent: boolean;

  @Input()
  name: string;

  @Input()
  disabled: boolean;

  @Input()
  isRequired = false;

  @Input()
  params: ISelectPartyFilterParams;

  @Output()
  changedSelect = new Subject<INamedEntity>();

  @ViewChild('auto', { static: true }) auto: MatAutocomplete;
    
  constructor(
    private service: SelectPartiesService) {
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
    return 'Поле "Партия" обязательно';
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
    } as ISelectPartyFilter;
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
    this.service.selectParties(this.filter)
      .pipe(takeUntil(this.destroy$))
      .subscribe(parties => {
        if (!parties){
          this.items = [];
          this.isNoContent = true;
          return;
        }

        this.isNoContent = false;

        if (canNextPage){
          this.items.push(...(parties as ISelectParties).items);
        } else{
          this.items = (parties as ISelectParties).items.slice();
        }

        this.paginationInfo = this.getFilledPaginationInfo(parties);
      });
    }

  private setParams() : void {
    if (!this.params){
      return;
    }
    this.filter.nomenclatureId = this.params.nomenclatureId;
    this.filter.organizationId = this.params.organizationId;
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