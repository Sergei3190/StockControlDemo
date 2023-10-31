import { INoteItem } from './../interfaces/note-item.interface';
import { Component, ComponentRef, HostListener, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { Subject, skipWhile, takeUntil } from 'rxjs';
import { NoteFilterComponent } from '../note-filter/note-filter.component';
import { INoteFilter } from '../interfaces/note-filter.interface';
import { NoteItem } from '../models/note-item.model';
import { Guid } from 'guid-ts';
import { NotesService } from './services/notes.service';
import { ConfirmDeleteComponent } from '../../../shared/modules/confirm-delete/confirm-delete.component';
import { ActivatedRoute, Router } from '@angular/router';
import { NoteItemComponent } from '../note-item/note-item.component';
import { DatePipe } from '@angular/common';
import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { SideDrawerContainerComponent } from 'src/app/shared/modules/side-drawer/side-drawer-container/side-drawer-container.component';
import { SideDrawerBaseComponent } from 'src/app/shared/modules/side-drawer/side-drawer-base/side-drawer-base.component';
import { IPagination } from 'src/app/shared/modules/pagination/interfaces/pagination.intarface';
import { ToastrService } from 'ngx-toastr';
import { ISideDrawerContainerConfig } from 'src/app/shared/modules/side-drawer/interfaces/side-drawer-container-config.interface';
import { ISideDrawerConfig } from 'src/app/shared/modules/side-drawer/interfaces/side-drawer-config.interface';
import { INotes } from '../interfaces/notes.interface';
import { LoadingInfoModel } from 'src/app/shared/models/loading-info.model';
import { IError } from 'src/app/shared/interfaces/error.interface';
import { HttpStatusCode } from '@angular/common/http';

@Component({
  selector: 'app-note-list',
  templateUrl: './note-list.component.html',
  styleUrls: ['./note-list.component.scss']
})
export class NoteListComponent implements OnInit, OnDestroy {
  private readonly destroy$ = new Subject<void>();

  @ViewChild('container', { static: true }) container: SideDrawerContainerComponent;
  private openedDrawer: ComponentRef<SideDrawerBaseComponent>;

  loadingInfo: LoadingInfoModel;
  
  canClearSearch = new Subject<boolean>();
  reload = () => {
    this.clearFilter();
  };

  fixCtx: {
    isFix: boolean;
    notes: NoteItem[];
  };

  unFixCtx: {
    isFix: boolean;
    notes: NoteItem[];
  };

  content?: string;
  paginationInfo: IPagination;
  filter: INoteFilter

  sideDrawerDataChange = new Subject<NoteItemComponent>;

  constructor(private dialog: MatDialog, 
    private service: NotesService,
    private toastr: ToastrService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private datePipe: DatePipe){
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
    });
  }

  onChangedContent(event: any): void {
    this.content = event;
  }

  onChangedSearch(searchString: any): void {
    this.initProperties(searchString);
    this.checkExistsDrawer()
    this.setStartRoute();
    this.load(true)
    this.loadingInfo.isResultFilter = true;
  }

  onPageChanged(value: any): void {
    this.paginationInfo.page = value;
    this.filter.page = value;
    this.checkExistsDrawer()
    this.setStartRoute();
    this.load();
  }

  addNote(): void {
    const note = new NoteItem((Guid.empty().toString() as unknown as Guid), this.content!, false, -1,);
    this.service.createNote(note)
      .pipe(
        takeUntil(this.destroy$)
      )
      .subscribe(note => {
        if (!note){
          return;
        }
        this.toastr.success('Заметка успешно создана');
        this.load();
      });
  }

  editNote(id: Guid): void {
    this.service.getNoteById(id)
      .pipe(
        takeUntil(this.destroy$)
        )
      .subscribe(note => {
        if (!note){
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
          type : NoteItemComponent,
          data : {
            title: 'Редактирование',
            model: {...note as INoteItem},
            update: (model) => {
              this.service.updateNote(model)
                .pipe(
                  takeUntil(this.destroy$)
                )
                .subscribe(flag => {
                  if (flag){
                    this.toastr.success('Заметка успешно изменена');
                  }
                });
            },
            sideDrawerDataChange: this.sideDrawerDataChange
          },
          close: () => {
            this.container.closeDrawer();
            this.setStartRoute();
          } 
        } as ISideDrawerConfig<NoteItemComponent>;

        this.openedDrawer = this.container.openDrawer(containerConfig, drawerConfig);
      });
  }

  deleteNote(id: Guid): void {
    const dialogConfig = new MatDialogConfig();

    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.data = {
      title: 'Подтверждение удаления заметки',
      content: 'Вы уверены, что хотите удалить выбранную заметку?'
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
        this.service.deleteNote(id)
          .pipe(
            takeUntil(this.destroy$)
          )
          .subscribe(note => {
            if (!note){
              return;
            }
            if (this.paginationInfo.items - 1 === 0 && this.paginationInfo.page > 1){
              this.filter.page = this.filter.page - 1;
            }
            this.toastr.success('Заметка успешно удалена');
            this.load();
          });
      })
  }

  changePinning(note: NoteItem): void {
    note.isFix = !note.isFix;
    this.service.updateNote(note)
      .pipe(
        takeUntil(this.destroy$)
      )
      .subscribe(flag => {
        if (!flag){
          note.isFix = !note.isFix;
        } else {
          this.swapNote(note);
        }
      });
  }

  onMouseEnter(note: NoteItem){
    if (!note.isHover){
        note.isHover = !note.isHover;
    }
  }

  onMouseLeave(note: NoteItem){
    if (note.isHover){
        note.isHover = !note.isHover;
    }
  }

  clearFilter(): void {
    this.canClearSearch.next(true);
    this.initProperties();
    this.checkExistsDrawer()
    this.setStartRoute();
    this.load();
  }

  openFilter(): void {
    const dialogConfig = new MatDialogConfig();

    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.position = { top: '50px' };
    dialogConfig.data = {
      title: 'Фильтр списка заметок'
    };

    const dialogRef = this.dialog.open(NoteFilterComponent, dialogConfig);

    dialogRef.afterClosed()
      .pipe(
        skipWhile(d => !d),
        takeUntil(this.destroy$))
      .subscribe(data => {
        this.filter.executionDate = this.datePipe.transform(data.executionDate, "yyyy-MM-dd"); 
        this.checkExistsDrawer()
        this.setStartRoute();
        this.load();
        this.loadingInfo.isResultFilter = true;
      })
  }

  drop(event: CdkDragDrop<any[]>) {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex
      );
    }
  }

  canMoveToUnFix() {
    return false;
  }

  canMoveToFix() {
    return false;
  }

  templateDrop(event: CdkDragDrop<any[]>, isFix: boolean) {
    if (isFix) {
      moveItemInArray(this.fixCtx.notes, event.previousIndex, event.currentIndex);
    } else {
      moveItemInArray(this.unFixCtx.notes, event.previousIndex, event.currentIndex);
    }
    this.updateItemsSort();
  }

  private swapNote(note: NoteItem) {
    note.isHover = false;
    if (note.isFix){
      this.fixCtx.notes.push({...note});
      const index = this.unFixCtx.notes.indexOf(note);
      if (index > -1){
        this.unFixCtx.notes.splice(index, 1);
      }
    } else {
      this.unFixCtx.notes.push({...note});
      const index = this.fixCtx.notes.indexOf(note);
      if (index > -1){
        this.fixCtx.notes.splice(index, 1);
      }
    }
  }

  private updateItemsSort() {
    const notes = this.getAllNotes();
    // будем обновлять через GRPC
    this.service.updateSortGrpc(notes)
    .pipe(takeUntil(this.destroy$))
    .subscribe(_ => {});
  }

  private getAllNotes(): NoteItem[] {
    const notes = [] as NoteItem[];
    notes.push(...this.fixCtx.notes);
    notes.push(...this.unFixCtx.notes);
    return notes;
  }

  private initProperties(search?: string) {
    this.loadingInfo = new LoadingInfoModel();
    this.paginationInfo = {} as IPagination;
    this.content = "";
    this.filter = {
      search: search,
      page: 1,
      pageSize: 15,
    } as INoteFilter;
  }

  private initSubscriptions() {
    this.sideDrawerDataChange
      .pipe(
        skipWhile(m => !m),
        takeUntil(this.destroy$)
      )
      .subscribe(data => {
        const notes = this.getAllNotes();
        let note = notes.find(n => n.id === data.model.id) as INoteItem;

        const isFix = note.isFix;

        data.model.executionDate = this.datePipe.transform(data.model.executionDate, "yyyy-MM-dd"); 
        note = Object.assign(note, data.model);

        if (note.isFix !== isFix){
          this.swapNote(note as NoteItem);
        }
      })
  }

  private checkExistsDrawer() {
    if (this.openedDrawer) {
      this.openedDrawer.instance.close();
      this.openedDrawer.destroy();
    }
  }

  private setStartRoute() {
    this.router.navigate(['./'], {
      relativeTo: this.activatedRoute,
      queryParams: this.filter,
      queryParamsHandling: ""
    });
  }

  private load(isSearch?: boolean) : void {
    this.loadingInfo.isLoading = true;
    setTimeout(() => {
      return this.service.getNotes(this.filter)
        .pipe(
          takeUntil(this.destroy$)
        )
        .subscribe(notes => {
          this.loadingInfo.isLoading = false;

          const handleErrorResult = this.handleError((notes as IError)?.status);
          if (handleErrorResult){
            return;
          }

          if (notes === null && isSearch){
            this.loadingInfo.isNoContent = true;
            return;
          }

          this.setFixCtx((notes as INotes).items?.filter(n => n.isFix));
          this.setUnFixCtx((notes as INotes).items?.filter(n => !n.isFix));
    
          this.paginationInfo = this.getFilledPaginationInfo(notes as INotes);
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

  private getFilledPaginationInfo(notes: INotes): IPagination {
    return  {
      page: notes.page,
      pageSize: notes.pageSize,
      totalPages: notes.totalPages,
      totalItems: notes.totalItems,
      items: notes.items.length
    } as IPagination;
  }

  private setFixCtx(noteItems: NoteItem[]) {
    this.fixCtx = {
      isFix: true,
      notes: noteItems
    }
  }

  private setUnFixCtx(noteItems: NoteItem[]) {
    this.unFixCtx = {
      isFix: false,
      notes: noteItems
    }
  }
}