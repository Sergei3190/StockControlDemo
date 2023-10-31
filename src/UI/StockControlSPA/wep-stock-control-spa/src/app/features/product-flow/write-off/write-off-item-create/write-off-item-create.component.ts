import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Guid } from 'guid-ts';
import { Subject } from 'rxjs';
import { INamedEntity } from 'src/app/shared/interfaces/named-entity.interface';
import { IParty } from 'src/app/shared/interfaces/party.interface';
import { ISelectNomenclatureFilterParams } from 'src/app/shared/modules/select-nomenclatures/interfaces/select-nomenclature-filter-params.interface';
import { ISelectOrganizationFilterParams } from 'src/app/shared/modules/select-organizations/interfaces/select-organization-filter-params.interface';
import { ISelectPartyFilterParams } from 'src/app/shared/modules/select-parties/interfaces/select-party-filter-params.interface';
import { ISelectWarehouseFilterParams } from 'src/app/shared/modules/select-warehouses/interfaces/select-warehouse-filter-params.interface';
import { IWriteOffItem } from '../../interfaces/write-off/write-off-item.interface';

@Component({
  selector: 'app-write-off-item-create',
  templateUrl: './write-off-item-create.component.html',
  styleUrls: ['./write-off-item-create.component.scss']
})
export class WriteOffItemCreateComponent implements OnInit, OnDestroy {
  private readonly destroy$ = new Subject<void>();

  title: string;
  form: FormGroup;

  nomenclatureFilterParams: ISelectNomenclatureFilterParams;
  organizationFilterParams: ISelectOrganizationFilterParams;
  partyFilterParams: ISelectPartyFilterParams;
  warehouseFilterParams: ISelectWarehouseFilterParams;

  constructor(private fb: FormBuilder,
    private dialogRef: MatDialogRef<WriteOffItemCreateComponent>,
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

  onChangedParty(party: INamedEntity): void {
    console.log(party);
    
    if (!party?.id){
      this.form.get('party')?.setErrors({'incorrect': true});
      return;
    } 
    this.form.get('party')?.setErrors(null);

    this.form.get('party')?.setValue(party);
    this.form.controls['party'].markAsDirty();

    this.nomenclatureFilterParams.partyId = party?.id;
    this.organizationFilterParams.partyId = party?.id;
    this.warehouseFilterParams.partyId = party?.id;
  }

  onChangedOrganization(organization: INamedEntity): void {
    if (!organization?.id){
      this.form.get('organization')?.setErrors({'incorrect': true});
      return;
    } 
    this.form.get('organization')?.setErrors(null);

    this.form.get('organization')?.setValue(organization);
    this.form.controls['organization'].markAsDirty();

    this.partyFilterParams.organizationId = organization?.id;
    this.nomenclatureFilterParams.organizationId = organization?.id;
    this.warehouseFilterParams.organizationId = organization?.id;
  }

  onChangedNomenclature(nomenclature: INamedEntity): void {
    console.log(nomenclature);

    if (!nomenclature?.id){
      this.form.get('nomenclature')?.setErrors({'incorrect': true});
      return;
    } 
    this.form.get('nomenclature')?.setErrors(null);

    this.form.get('nomenclature')?.setValue(nomenclature);
    this.form.controls['nomenclature'].markAsDirty();

    this.partyFilterParams.nomenclatureId = nomenclature?.id;
    this.organizationFilterParams.nomenclatureId = nomenclature?.id;
    this.warehouseFilterParams.nomenclatureId = nomenclature?.id;
  }

  onChangedSendingWarehouse(sendingWarehouse: INamedEntity): void {
    if (!sendingWarehouse?.id){
      this.form.get('sendingWarehouse')?.setErrors({'incorrect': true});
      return;
    } 
    this.form.get('sendingWarehouse')?.setErrors(null);

    this.form.get('sendingWarehouse')?.setValue(sendingWarehouse);
    this.form.controls['sendingWarehouse'].markAsDirty();

    this.partyFilterParams.warehouseId = sendingWarehouse?.id;
    this.nomenclatureFilterParams.warehouseId = sendingWarehouse?.id;
    this.organizationFilterParams.warehouseId = sendingWarehouse?.id;
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
    this.setParty(model);
    this.dialogRef.close(model);
  }

  close() {
    this.dialogRef.close();
  }

  private setCreateTime(model: IWriteOffItem) {
    model.createTime = model.createTime + ':00';
  }

  private setParty(model: IWriteOffItem) {
    model.party = { id: model.party.id } as IParty;
  }

  private initProperties() {
    this.nomenclatureFilterParams = {} as ISelectNomenclatureFilterParams;
    this.organizationFilterParams = {} as ISelectOrganizationFilterParams;
    this.partyFilterParams = {} as ISelectPartyFilterParams;
    this.warehouseFilterParams = {} as ISelectWarehouseFilterParams;
    this.form = this.fb.group({
      id: [(Guid.empty().toString() as unknown as Guid)],
      number: [null, Validators.required],
      createDate: [null, Validators.required],
      createTime: [null, Validators.required],
      price: [null, Validators.required],
      quantity: [null, Validators.required],
      totalPrice: [null],
      reason: [null],
      party: [null, Validators.required],
      nomenclature: [null, Validators.required], 
      sendingWarehouse: [null, Validators.required],
      warehouse: [null, Validators.required],
      organization: [null, Validators.required],
    });
  }
}