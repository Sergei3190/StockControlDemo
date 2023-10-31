import { Component, OnInit } from '@angular/core';
import { SideDrawerBaseComponent } from 'src/app/shared/modules/side-drawer/side-drawer-base/side-drawer-base.component';
import { IWriteOffItem } from '../../interfaces/write-off/write-off-item.interface';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, debounceTime, take, takeUntil } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { productFlowUrls } from '../../product-flow-routing.module';

@Component({
  selector: 'app-write-off-component',
  templateUrl: './write-off-item.component.html',
  styleUrls: ['./write-off-item.component.scss']
})

export class WriteOffItemComponent extends SideDrawerBaseComponent<WriteOffItemComponent> implements OnInit {

  title: string;
  model: IWriteOffItem;
  form: FormGroup;
  update: (model: IWriteOffItem) => Observable<IWriteOffItem>;

  constructor(
    private router: Router,
    private toastr: ToastrService,
    private activatedRoute: ActivatedRoute,
    private fb: FormBuilder) {
      super();
  }

  ngOnInit(): void {
    this.initProperties();
    this.initSubscriptions();
    this.setStartRoute();
  }

  getErrorMessage(name: string): string {
    switch (name) {
      case 'number':
        return this.form.get(name)?.hasError('required') ? 'Поле "Номер" обязательно' : '';
      case 'price':
        return this.form.get(name)?.hasError('required') ? 'Поле "Цена" обязательно' : '';
      case 'quantity':
        return this.form.get(name)?.hasError('required') ? 'Поле "Количество" обязательно' : '';
      default: return '';
    }
  }

  private initProperties() {
    this.title = this.data.title;
    this.model = this.data.model;
    this.update = this.data.update;
    this.form = this.fb.group({
      id: [{ value: this.model.id, disabled: true }],
      number: [{ value: this.model.number, disabled: false }],
      createDate: [{ value: this.model.createDate, disabled: true }],
      createTime: [{ value: this.model.createTime, disabled: true }],
      price: [{ value: this.model.price, disabled: false }],
      quantity: [{ value: this.model.quantity, disabled: false }],
      totalPrice: [{ value: this.model.totalPrice, disabled: true }],
      reason: [{ value: this.model.reason, disabled: false }],
      party: this.fb.group({
        id: [{ value: this.model.party?.id, disabled: true}],
        number: [{ value: this.model.party.number, disabled: true}],
        extensionNumber: [{ value: this.model.party?.extensionNumber, disabled: true }],
        createDate: [{ value: this.model.party?.createDate, disabled: true }],
        createTime: [{ value: this.model.party?.createTime, disabled: true }],
      }),
      nomenclature: [{ value: this.model.nomenclature, disabled: true }], 
      sendingWarehouse: [{ value: this.model.sendingWarehouse, disabled: true }],
      warehouse: [{ value: this.model.warehouse, disabled: true }],
      organization: [{ value: this.model.organization, disabled: true }],
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
            'Списание изменено'
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
    this.router.navigate([`./${productFlowUrls.writeOffs}`, this.model.id], {
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
