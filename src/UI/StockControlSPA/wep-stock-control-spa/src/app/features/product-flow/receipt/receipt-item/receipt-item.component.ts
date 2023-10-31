import { Component, OnInit } from '@angular/core';
import { SideDrawerBaseComponent } from 'src/app/shared/modules/side-drawer/side-drawer-base/side-drawer-base.component';
import { IReceiptItem } from '../../interfaces/receipt/receipt-item.interface';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, debounceTime, take, takeUntil } from 'rxjs';
import { productFlowUrls } from '../../product-flow-routing.module';
import { INamedEntity } from 'src/app/shared/interfaces/named-entity.interface';
import { ToastrService } from 'ngx-toastr';
import { IParty } from 'src/app/shared/interfaces/party.interface';

@Component({
  selector: 'app-receipt-item',
  templateUrl: './receipt-item.component.html',
  styleUrls: ['./receipt-item.component.scss']
})
export class ReceiptItemComponent extends SideDrawerBaseComponent<ReceiptItemComponent> implements OnInit {

  title: string;
  model: IReceiptItem;
  form: FormGroup;
  update: (model: IReceiptItem) => Observable<IReceiptItem>;

  constructor(
    private toastr: ToastrService,
    private router: Router,
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
      case 'createDate':
        return this.form.get(name)?.hasError('required') ? 'Поле "Дата создания" обязательно' : '';
      case 'createTime':
        return this.form.get(name)?.hasError('required') ? 'Поле "Время создания" обязательно' : '';
      case 'price':
        return this.form.get(name)?.hasError('required') ? 'Поле "Цена" обязательно' : '';
      case 'quantity':
        return this.form.get(name)?.hasError('required') ? 'Поле "Количество" обязательно' : '';
      default: return '';
    }
  }

  getPartyErrorMessage(name: string): string {
    switch (name) {
      case 'number':
        return this.form.get('party')?.get(name)?.hasError('required') ? 'Поле "Номер партии" обязательно' : '';
      default: return '';
    }
  }

  onChangedOrganization(organization: INamedEntity): void {
    this.form.get('organization')?.setValue(organization);
    this.form.controls['organization'].markAsDirty();
  }

  onChangedNomenclature(nomenclature: INamedEntity): void {
    this.form.get('nomenclature')?.setValue(nomenclature);
    this.form.controls['nomenclature'].markAsDirty();
  }

  onChangedWarehouse(warehouse: INamedEntity): void {
    this.form.get('warehouse')?.setValue(warehouse);
    this.form.controls['warehouse'].markAsDirty();
  }

  private initProperties() {
    this.title = this.data.title;
    this.model = this.data.model;
    this.update = this.data.update;
    this.form = this.fb.group({
      id: [{ value: this.model.id, disabled: true }],
      number: [this.model.number, Validators.required],
      createDate: [this.model.createDate, Validators.required],
      createTime: [this.model.createTime, Validators.required],
      price: [this.model.price],
      quantity: [this.model.quantity, Validators.required],
      totalPrice: [this.model.totalPrice],
      party: this.fb.group({
        id: [{value: this.model.party?.id, disabled: true}],
        number: [this.model.party?.number, Validators.required],
        extensionNumber: [{ value: this.model.party?.extensionNumber, disabled: true }],
        createDate: [{ value: this.model.party?.createDate, disabled: false }],
        createTime: [{ value: this.model.party?.createTime, disabled: false }],
      }),
      nomenclature: [this.model.nomenclature, Validators.required], 
      warehouse: [this.model.warehouse, Validators.required],
      organization: [this.model.organization, Validators.required],
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
            'Поступление изменено'
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
    this.router.navigate([`./${productFlowUrls.receipts}`, this.model.id], {
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
    this.model.party.id = this.form.get('party')?.get('id')?.value;
    this.model.party.extensionNumber = this.form.get('party')?.get('extensionNumber')?.value;

    this.sideDrawerDataChange.next(this);
    this.update(this.model);
  }
}
