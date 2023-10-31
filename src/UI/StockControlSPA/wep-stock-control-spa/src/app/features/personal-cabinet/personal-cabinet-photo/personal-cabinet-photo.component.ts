import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subject, switchMap, takeUntil, tap } from 'rxjs';
import { PersonPhotosService } from './services/person-photos.service';
import { ToastrService } from 'ngx-toastr';
import { FileStorageService } from 'src/app/shared/modules/file-storage/file-storage.service';
import { IFileInfo } from 'src/app/shared/interfaces/file-info.interface';
import { Guid } from 'guid-ts';
import { IPersonPhotoItem } from '../interfaces/person-photo/person-photo-item.interface';

@Component({
  selector: 'app-personal-cabinet-photo',
  templateUrl: './personal-cabinet-photo.component.html',
  styleUrls: ['./personal-cabinet-photo.component.scss']
})
export class PersonalCabinetPhotoComponent implements OnInit, OnDestroy {
  private readonly destroy$ = new Subject<void>();

  @Input()
  model: IPersonPhotoItem;

  isLoading = false;

  constructor(private service: PersonPhotosService,
    private fileStorageService: FileStorageService,
    private toastr: ToastrService) {
  }

  ngOnInit(): void {
    this.initPhoto();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  onFileChange(event: any): void {
    if (event.target.files.length === 0){
      return
    }
    const file = event.target.files[0] as File;
    if (file.type !== 'image/jpeg'){
      this.toastr.warning('Недопустимый формат файла, загрузите формат .jpg!', 'ПРЕДУПРЕЖДЕНИЕ!');
      return;
    }
    if (!this.model.id) {
      this.createPhoto(file);
    } else {
      this.updatePhoto(file);
    }
  }

  private createPhoto(file: File) {
    this.isLoading = true;
    this.fileStorageService.upload(file)
      .pipe(
        switchMap(id => this.fileStorageService.getById(id)),
        tap(fileDto => {
          this.model.file = {...fileDto as IFileInfo};
        }),
        switchMap(_ => this.service.createPersonPhoto(this.model)),
        tap(id => {
          if (!id) {
            this.toastr.error('Фото не загружено');
          }
          else if (id instanceof Guid){
            this.model.id = id;
            this.toastr.success('Фото успешно загружено');
          }
        }),
        takeUntil(this.destroy$)
      )
      .subscribe(_ => this.isLoading = false);
  }

  private updatePhoto(file: File): void {
    this.isLoading = true;
    this.fileStorageService.upload(file)
      .pipe(
        switchMap(id => this.fileStorageService.getById(id)),
        tap(fileDto => {
          this.model.file = {...fileDto as IFileInfo};
        }),
        switchMap(_ => this.service.updatePersonPhoto(this.model)),
        tap(flag => {
          if (flag){
            this.toastr.success('Фото успешно обновлено');
          }
        }),
        takeUntil(this.destroy$)
      )
      .subscribe(_ => this.isLoading = false); 
  }

  private initPhoto() {
    if (this.model.file && !this.model.file.content){
      this.fileStorageService.getById(this.model.file.id)
        .pipe(
          takeUntil(this.destroy$)
        )
        .subscribe(file => this.model.file = {...file as IFileInfo});
    }
  }
}

