import { LoadingInfoModel } from './../../../shared/models/loading-info.model';
import { Component, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { Subject, skipWhile, switchMap, takeUntil, tap } from 'rxjs';
import { PersonDocumentsService } from './services/person-documents.service';
import { ToastrService } from 'ngx-toastr';
import { IPersonDocumentItem } from '../interfaces/person-document/person-document-item.interface';
import { Guid } from 'guid-ts';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ConfirmDeleteComponent } from 'src/app/shared/modules/confirm-delete/confirm-delete.component';
import { FileStorageService } from 'src/app/shared/modules/file-storage/file-storage.service';
import { IFileInfo } from 'src/app/shared/interfaces/file-info.interface';
import { IError } from 'src/app/shared/interfaces/error.interface';

@Component({
  selector: 'app-personal-cabinet-document-list',
  templateUrl: './personal-cabinet-document-list.component.html',
  styleUrls: ['./personal-cabinet-document-list.component.scss']
})
export class PersonalCabinetDocumentListComponent implements OnInit, OnDestroy {
  private readonly destroy$ = new Subject<void>();

  @Input()
  items: IPersonDocumentItem[] = [];

  @Input()
  cardId: Guid;

  @Input()
  loadingInfo: LoadingInfoModel;

  @Input()
  reloadPage: () => void;

  @Output()
  loading = new Subject<boolean>();

  @Output()
  deleted = new Subject<boolean>();

  model: IPersonDocumentItem;

  constructor(private dialog: MatDialog, 
    private service: PersonDocumentsService,
    private fileStorageService: FileStorageService,
    private toastr: ToastrService) {
  }

  ngOnInit(): void {
    console.log(this.items);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  downloadFile(fileDto: IFileInfo) : void {
    this.fileStorageService.download(fileDto.id);
  }

  onFileChange(event: any): void {
    console.log(event.target.files);
    if (event.target.files.length === 0){
      return
    }
    const file = event.target.files[0] as File;
    this.model = this.items.find(d => d.isHover) as IPersonDocumentItem;
    if (!this.model) {
      console.log('add');
      this.addDocument(file);
    } else {
      console.log('replace');
      this.replaceDocument(file);
    }
  }

  onMouseEnter(item: IPersonDocumentItem){
    if (!item.isHover){
      item.isHover = !item.isHover;
    }
  }

  onMouseLeave(item: IPersonDocumentItem){
    if (item.isHover){
      item.isHover = !item.isHover;
    }
  }

  // тк файлы могут быть тяжелыми и настройка на такие файлы имеется только в файловым хранилище, а ткже чтобы 
  // не гонять в микросервис личного кабинета не нужные данные при создании и обновления документа, 
  // то мы сначала сохраняем данные без детальной инфы о файле
  addDocument(file: File): void{
    this.loading.next(true);
    this.fileStorageService.upload(file)
      .pipe(
        tap(id => {
          this.model = {
            cardId: this.cardId,
            file: {
              id: id,
              fileName: file.name,
            } as IFileInfo,
          } as IPersonDocumentItem;
          console.log(this.model);
        }),
        switchMap(_ => this.service.createPersonDocument(this.model)),
        tap(document => {
          console.log(13);
          if (!document) {
            this.toastr.error('Документ не сохранён в БД');
          }
          else {
            this.model.id = (document as IPersonDocumentItem).id;
          }
        }),
        switchMap(_ => this.fileStorageService.getById(this.model.file.id)),
        tap(fileDto => {
          console.log(12);
          if ((fileDto as IError).status) {
            this.toastr.error('Документ не загружен');
          } else {
            this.model.file = {...fileDto as IFileInfo };
            this.toastr.success('Документ успешно загружен');
          }
          console.log(this.model);
        }),        
        takeUntil(this.destroy$)
      )
      .subscribe(data => {
        if (data){
          this.loading.next(false)
        }
      }); 
  }

  replaceDocument(file: File): void {
    this.loading.next(true);
    this.fileStorageService.upload(file)
      .pipe(
        tap(id => {
          this.model.file = {
            id: id,
            fileName: file.name
          } as IFileInfo;
        }),
        switchMap(_ => this.service.updatePersonDocument(this.model)),
        tap(flag => {
          if (!flag) {
            this.toastr.error('Документ не обновлен в БД');
          }
          this.model.isHover = false;
        }),
        switchMap(_ => this.fileStorageService.getById(this.model.file.id)),
        tap(fileDto => {
          console.log(12);
          if ((fileDto as IError).status) {
            this.toastr.error('Документ не заменён');
          } else {
            this.model.file = {...fileDto as IFileInfo };
            this.toastr.success('Документ успешно заменён');
          }
        }),
        takeUntil(this.destroy$)
      )
      .subscribe(data => {
        if (data){
          this.loading.next(false)
        }
      });
  }

  deleteDocument(id: Guid): void {
    const dialogConfig = new MatDialogConfig();

    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.position = { top: '50px' };
    dialogConfig.data = {
      title: 'Подтверждение удаления документа',
      content: 'Вы уверены, что хотите удалить выбранный документ?'
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
        this.service.deletePersonDocument(id)
          .pipe(
            takeUntil(this.destroy$)
          )
          .subscribe(flag => {
            if (!flag){
              this.toastr.warning('Документ не удален');
              this.deleted.next(false);
              return;
            }
            this.toastr.success('Документ успешно удален');
            this.deleted.next(true);
          });
      })
  }
}