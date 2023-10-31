import { Component, OnInit } from '@angular/core';
import { SideDrawerBaseComponent } from 'src/app/shared/modules/side-drawer/side-drawer-base/side-drawer-base.component';
import { INomenclatureItem } from '../../interfaces/nomenclature/nomenclature-item.interface';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable, debounceTime, take, takeUntil } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nomenclature-item',
  templateUrl: './nomenclature-item.component.html',
  styleUrls: ['./nomenclature-item.component.scss']
})
export class NomenclatureItemComponent extends SideDrawerBaseComponent<NomenclatureItemComponent> implements OnInit {

  title: string;
  model: INomenclatureItem;
  form: FormGroup;
  update: (model: INomenclatureItem) => Observable<INomenclatureItem | undefined>;

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

  getErrorMessage(name: string): string {
    switch (name) {
      case 'name':
        return this.form.get(name)?.hasError('required') ? 'Поле "Наименование" обязательно' : '';
      default: return '';
    }
  }

  private initProperties() {
    this.title = this.data.title;
    this.model = this.data.model;
    this.update = this.data.update;
    this.form = this.fb.group({
      id: [this.model.id],
      name: [this.model.name, Validators.required],
      classifier: [this.model.classifier],
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
            'Номенклатура изменена'
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
