import { Component, Input, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { Subject, debounceTime, takeUntil } from 'rxjs';
import { IPagination } from '../pagination/interfaces/pagination.intarface';
import { ISelectOrganizationFilter } from './interfaces/select-organization-filter.interface';
import { INamedEntity } from '../../interfaces/named-entity.interface';
import { MatAutocomplete } from '@angular/material/autocomplete';
import { SelectOrganizationsService } from './services/select-organizations.service';
import { ISelectOrganizations } from './interfaces/select-organizations.interface';
import { ISelectOrganizationFilterParams } from './interfaces/select-organization-filter-params.interface';

@Component({
  selector: 'app-select-organizations',
  templateUrl: './select-organizations.component.html',
  styleUrls: ['./select-organizations.component.scss']
})
export class SelectOrganizationsComponent implements OnInit, OnDestroy {
  private readonly destroy$ = new Subject<void>();

  paginationInfo: IPagination;
  filter: ISelectOrganizationFilter;

  items: INamedEntity[] = [];

  isNoContent: boolean;

  @Input()
  name: string;

  @Input()
  disabled: boolean;

  @Input()
  isRequired = false;

  @Input()
  params: ISelectOrganizationFilterParams;

  @Output()
  changedSelect = new Subject<INamedEntity>();

  @ViewChild('auto', { static: true }) auto: MatAutocomplete;
  
  constructor(
    private service: SelectOrganizationsService) {
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
    return 'Поле "Организация" обязательно';
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
    } as ISelectOrganizationFilter;
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
    this.service.selectOrganizations(this.filter)
      .pipe(takeUntil(this.destroy$))
      .subscribe(organizations => {
        if (!organizations){
          this.items = [];
          this.isNoContent = true;
          return;
        }

        this.isNoContent = false;

        if (canNextPage){
          this.items.push(...(organizations as ISelectOrganizations).items);
        } else{
          this.items = (organizations as ISelectOrganizations).items.slice();
        }

        this.paginationInfo = this.getFilledPaginationInfo(organizations);
      });
    }

  private setParams() : void {
    if (!this.params){
      return;
    }
    this.filter.nomenclatureId = this.params.nomenclatureId;
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
