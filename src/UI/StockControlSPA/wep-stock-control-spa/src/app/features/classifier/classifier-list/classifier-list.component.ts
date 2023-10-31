import { Component, HostListener, OnDestroy, OnInit } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { IPagination } from 'src/app/shared/modules/pagination/interfaces/pagination.intarface';
import { ClassifiersService } from './services/classifiers.service';
import { ActivatedRoute, Router } from '@angular/router';
import { IClassifiers } from '../interfaces/classifier/classifiers.interface';
import { IClassifierFilter } from '../interfaces/classifier/classifier-filter.interface';
import { IClassifier } from '../interfaces/classifier/classifier.interface';
import { LoadingInfoModel } from 'src/app/shared/models/loading-info.model';
import { IError } from 'src/app/shared/interfaces/error.interface';
import { HttpStatusCode } from '@angular/common/http';

@Component({
  selector: 'app-classifier-list',
  templateUrl: './classifier-list.component.html',
  styleUrls: ['./classifier-list.component.scss']
})
export class ClassifierListComponent implements OnInit, OnDestroy  {
  private readonly destroy$ = new Subject<void>();

  items: IClassifier[];
  
  loadingInfo: LoadingInfoModel;
  
  canClearSearch = new Subject<boolean>();
  reload = () => {
    this.clearFilter();
  };

  content?: string;
  paginationInfo: IPagination;
  filter: IClassifierFilter

  constructor(
    private service: ClassifiersService,
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

  fell(path: string){
    this.router.navigate([`${path}`], {
      relativeTo: this.activatedRoute,
      queryParamsHandling: ""
    })
  }

  clearFilter(): void {
    this.canClearSearch.next(true);
    this.ngOnInit();
  }

  private initProperties(search?: string) {
    this.loadingInfo = new LoadingInfoModel();
    this.paginationInfo = {} as IPagination;
    this.content = "";
    this.filter = {
      search: search,
      page: 1,
      pageSize: 20,
    } as IClassifierFilter;
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
      return  this.service.selectClassifiers(this.filter)
        .pipe(takeUntil(this.destroy$))
        .subscribe(classifiers => {
          this.loadingInfo.isLoading = false;

          const handleErrorResult = this.handleError((classifiers as IError)?.status);
          if (handleErrorResult){
            return;
          }

          if (classifiers === null && isSearch){
            this.loadingInfo.isNoContent = true;
            return;
          }

          this.items = [];
          this.items.push(...(classifiers as IClassifiers).items);

          this.paginationInfo = this.getFilledPaginationInfo(classifiers as IClassifiers);
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

  private getFilledPaginationInfo(classifiers: IClassifiers): IPagination {
    return {
      page: classifiers.page,
      pageSize: classifiers.pageSize,
      totalPages: classifiers.totalPages,
      totalItems: classifiers.totalItems,
      items: classifiers.items.length
    } as IPagination;
  }
}
