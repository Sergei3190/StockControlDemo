import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Guid } from 'guid-ts';
import { Subject } from 'rxjs';
import { INamedEntity } from 'src/app/shared/interfaces/named-entity.interface';
import { IReceiptItem } from '../../interfaces/receipt/receipt-item.interface';

@Component({
  selector: 'app-receipt-item-create',
  templateUrl: './receipt-item-create.component.html',
  styleUrls: ['./receipt-item-create.component.scss']
})
export class ReceiptItemCreateComponent implements OnInit, OnDestroy {
  private readonly destroy$ = new Subject<void>();

  title: string;
  form: FormGroup;

  constructor(private fb: FormBuilder,
    private dialogRef: MatDialogRef<ReceiptItemCreateComponent>,
    @Inject(MAT_DIALOG_DATA) data: any) {
      this.title = data.title;
  }

  ngOnInit(): void {
    this.initProperties();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
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
    if (!organization?.id){
      this.form.get('organization')?.setErrors({'incorrect': true});
      return;
    } 
    this.form.get('organization')?.setErrors(null);

    this.form.get('organization')?.setValue(organization);
    this.form.controls['organization'].markAsDirty();
  }

  onChangedNomenclature(nomenclature: INamedEntity): void {
    if (!nomenclature?.id){
      this.form.get('nomenclature')?.setErrors({'incorrect': true});
      return;
    } 
    this.form.get('nomenclature')?.setErrors(null);

    this.form.get('nomenclature')?.setValue(nomenclature);
    this.form.controls['nomenclature'].markAsDirty();
  }

  onChangedWarehouse(warehouse: INamedEntity): void {
    if (!warehouse?.id){
      this.form.get('warehouse')?.setErrors({'incorrect': true});
      return;
    } 
    this.form.get('warehouse')?.setErrors(null);

    this.form.get('warehouse')?.setValue(warehouse);
    this.form.controls['warehouse'].markAsDirty();
  }

  save() {
    const model = {...this.form.value};
    this.setCreateTime(model);
    console.log(model);
    this.dialogRef.close(model);
  }

  close() {
    this.dialogRef.close();
  }

  private setCreateTime(model: IReceiptItem) {
    model.createTime = model.createTime + ':00';
    if (model.party.createTime){
      model.party.createTime = model.party.createTime + ':00';
    }
  }

  private initProperties() {
    this.form = this.fb.group({
      id: [(Guid.empty().toString() as unknown as Guid)],
      number: [null, Validators.required],
      createDate: [null, Validators.required],
      createTime: [null, Validators.required],
      price: [null, Validators.required],
      quantity: [null, Validators.required],
      totalPrice: [null],
      party: this.fb.group({
        id: [(Guid.empty().toString() as unknown as Guid)],
        number: [null, Validators.required],
        extensionNumber: [null],
        createDate: [null],
        createTime: [null],
      }),
      nomenclature: [null, Validators.required], 
      warehouse: [null, Validators.required],
      organization: [null, Validators.required],
    });
  }
}