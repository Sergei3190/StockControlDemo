import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable, take, takeUntil, debounceTime } from 'rxjs';
import { INoteItem } from '../interfaces/note-item.interface';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SideDrawerBaseComponent } from 'src/app/shared/modules/side-drawer/side-drawer-base/side-drawer-base.component';

@Component({
  selector: 'app-note-item',
  templateUrl: './note-item.component.html',
  styleUrls: ['./note-item.component.scss']
})

export class NoteItemComponent extends SideDrawerBaseComponent<NoteItemComponent> implements OnInit {

  title: string;
  model: INoteItem;
  form: FormGroup;
  update: (model: INoteItem) => Observable<INoteItem | undefined>;

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private fb: FormBuilder,
    private toastr: ToastrService) {
      super();
  }

  ngOnInit(): void {
    this.initProperties();
    this.initSubscriptions();
    this.setStartRoute();
  }

  changePinning(): void {
    this.form.get('isFix')?.setValue(!this.form.value.isFix);
    // помечаем элемент как изменённый пользователем
    this.form.controls['isFix'].markAsDirty();
  }

  clearContent(): void {
    this.form.get('content')?.setValue('');
  }

  getErrorMessage(name: string): string {
    switch (name) {
      case 'content':
        return this.form.get(name)?.hasError('required') ? 'Поле "Текст" обязательно' : '';
      default: return '';
    }
  }

  private initProperties() {
    this.title = this.data.title;
    this.model = this.data.model;
    this.update = this.data.update;
    this.form = this.fb.group({
      id: [this.model.id],
      content: [this.model.content, Validators.required],
      isFix: [this.model.isFix],
      sort: [this.model.sort],
      executionDate: [this.model.executionDate]
    });
    this.sideDrawerDataChange = this.data.sideDrawerDataChange;
  }

  private initSubscriptions() {
    this.form.valueChanges
      .pipe(
        debounceTime(300), // чтобы при нажатии клавиш пользователем несколько раз подряд, сохранялось актуальное значение
        takeUntil(this.destroy$))
      .subscribe(values => {
        if (this.form.valid && this.form.dirty) {

          const message = this.toastr.success(
            '<a class="restore-form-action text-decoration-underline">Вернуть</a> предыдущее значение',
            'Заметка изменена'
          );

          let action!: HTMLElement;

          message.onShown
            .pipe(take(1))
            .subscribe(() => {
              action = document.querySelector('.restore-form-action')!;
              if (action) {
                  action.addEventListener('click', () => this.restoreForm());
              }
          });

          message.onHidden.pipe(take(1)).subscribe(() => {
            if (action) {
                action.removeEventListener('click', () => this.restoreForm());
            }
            this.applyChanges({...values});
          });
        }
      });
  }

  private setStartRoute() {
    this.router.navigate(['./', this.model.id], {
      relativeTo: this.activatedRoute,
    });
  }

  private restoreForm(): void {
    this.form.reset(this.model);
  }

  private applyChanges(values: any) {
    if (!this.form.dirty) {
      return;
    }
    this.model = Object.assign(this.model, values);
    this.sideDrawerDataChange.next(this);
    this.update(this.model);
  }
}