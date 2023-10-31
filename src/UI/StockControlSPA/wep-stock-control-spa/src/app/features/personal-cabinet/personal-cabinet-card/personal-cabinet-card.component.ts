import { Component, HostListener, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, switchMap, takeUntil, tap, skipWhile, of } from 'rxjs';
import { IPagination } from 'src/app/shared/modules/pagination/interfaces/pagination.intarface';
import { StorageService } from 'src/app/shared/modules/storage/services/storage.service';
import { IPersonDocumentFilter } from '../interfaces/person-document/person-document-filter.interface';
import { UserPersonsService } from '../personal-cabinet-main-info/services/user-persons.service';
import { IUserPersonItem } from '../interfaces/user-person/user-person-item.interface';
import { PersonPhotosService } from '../personal-cabinet-photo/services/person-photos.service';
import { PersonDocumentsService } from '../personal-cabinet-document-list/services/person-documents.service';
import { IPersonDocumentItem } from '../interfaces/person-document/person-document-item.interface';
import { IPersonDocuments } from '../interfaces/person-document/person-documents.interface';
import { IPersonPhotoItem } from '../interfaces/person-photo/person-photo-item.interface';
import { LoadingInfoModel } from 'src/app/shared/models/loading-info.model';
import { IError } from 'src/app/shared/interfaces/error.interface';
import { HttpStatusCode } from '@angular/common/http';

@Component({
  selector: 'app-personal-cabinet-card',
  templateUrl: './personal-cabinet-card.component.html',
  styleUrls: ['./personal-cabinet-card.component.scss']
})
export class PersonalCabinetCardComponent implements OnInit, OnDestroy {
  private readonly destroy$ = new Subject<void>();
  
  userName: string;
  personMainInfo: IUserPersonItem;
  personPhoto: IPersonPhotoItem;
  personDocuments: IPersonDocumentItem[];
  
  loadingInfo: LoadingInfoModel;

  canClearSearch = new Subject<boolean>();
  reload = () => {
    this.clearFilter();
  };

  content?: string;
  paginationInfo: IPagination;
  filter: IPersonDocumentFilter

  constructor(
    private personService: UserPersonsService,
    private photoService: PersonPhotosService,
    private documentService: PersonDocumentsService,
    private storage: StorageService,
    private router: Router,
    private activatedRoute: ActivatedRoute){
  }

  ngOnInit(): void {
    this.initProperties();
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
    this.loadPersonDocuments(true);
    this.loadingInfo.isResultFilter = true;
  }

  onPageChanged(value: any): void {
    this.paginationInfo.page = value;
    this.filter.page = value;
    this.setStartRoute();
    this.loadPersonDocuments();
  }

  onDeleteDocument(flag: boolean): void {
    if (flag){
      if (this.paginationInfo.items - 1 === 0 && this.paginationInfo.page > 1){
        this.filter.page = this.filter.page - 1;
      }
      this.loadPersonDocuments();
    }
  }

  onLoadingDocument(flag: boolean): void {
    if (flag) {
      this.loadingInfo.isLoading = true;
    }
    else {
      this.loadPersonDocuments();
    }
  }

  clearFilter(): void {
    this.canClearSearch.next(true);
    this.initProperties();
    this.loadPersonDocuments();
  }

  private loadPersonDocuments(isSearch?: boolean): void {
    this.loadingInfo.isLoading = true;
    this.documentService.getPersonDocuments(this.filter)
      .pipe(
        takeUntil(this.destroy$)
      )
      .subscribe(documents => {
        this.loadingInfo.isLoading = false;

        const handleErrorResult = this.handleError((documents as IError)?.status);
        if (handleErrorResult){
          return;
        }

        if (documents === null && isSearch){
          this.loadingInfo.isNoContent = true;
          return;
        }

        this.personDocuments = [];
        if (documents === null && !isSearch){
          this.paginationInfo = this.getEmptyPaginationInfo();
        } else {
          this.personDocuments.push(...(documents as IPersonDocuments).items);
          this.paginationInfo = this.getFilledPaginationInfo(documents as IPersonDocuments);
        }
      });
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

  private setStartRoute() {
    this.router.navigate(['./'], {
      relativeTo: this.activatedRoute,
      queryParams: this.filter,
      queryParamsHandling: ""
    })
  }

  private initProperties(search?: string) {
    this.userName = this.storage.retrieve('userData')?.name;
    this.loadingInfo = new LoadingInfoModel();
    this.paginationInfo = {} as IPagination;
    this.content = "";
    this.filter = {
      cardId: this.personMainInfo?.cardId,
      search: search,
      page: 1,
      pageSize: 2,
    } as IPersonDocumentFilter;
  }

  private load() {
    this.loadingInfo.isLoading = true;
    this.personService.getUserPersonCurrentUser()
      .pipe(
        skipWhile(person => !person),
        tap(person => {
          if (!person){
            this.personMainInfo = {} as IUserPersonItem;
          } else {
            this.personMainInfo = person as IUserPersonItem;
            this.filter.cardId = (person as IUserPersonItem).cardId;
          }
        }),
        switchMap(person => this.photoService.getPersonPhotoByCardId((person as IUserPersonItem).cardId)),
        tap(photo => {
          if (!photo) {
            this.personPhoto = {
              cardId: this.personMainInfo.cardId,
            } as IPersonPhotoItem;
          } else {
            this.personPhoto = {...photo as IPersonPhotoItem};
          }
        }),
        switchMap(photo => this.documentService.getPersonDocuments(this.filter)),
        tap(documents => {
          this.personDocuments = [];
          if (documents) {
            this.personDocuments.push(...(documents as IPersonDocuments).items);
            this.paginationInfo = this.getFilledPaginationInfo(documents as IPersonDocuments);
          } else {
            this.paginationInfo = this.getEmptyPaginationInfo();
          }       
        }),
        takeUntil(this.destroy$)
      )
      .subscribe(_ => {
        this.loadingInfo.isLoading = false; 
      });   
  }

  private getFilledPaginationInfo(documents: IPersonDocuments): IPagination {
    return {
      page: documents.page,
      pageSize: documents.pageSize,
      totalPages: documents.totalPages,
      totalItems: documents.totalItems,
      items: documents.items.length
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