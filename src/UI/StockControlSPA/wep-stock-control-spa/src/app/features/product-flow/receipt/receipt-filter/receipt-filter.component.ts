import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { INamedEntity } from 'src/app/shared/interfaces/named-entity.interface';

@Component({
  selector: 'app-receipt-filter',
  templateUrl: './receipt-filter.component.html',
  styleUrls: ['./receipt-filter.component.scss']
})
export class ReceiptFilterComponent implements OnInit, OnDestroy {
  private readonly destroy$ = new Subject<void>();

  title: string;
  form: FormGroup;
  
  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<ReceiptFilterComponent>,
    @Inject(MAT_DIALOG_DATA) data: any) {
      this.title = data.title;
  }

  ngOnInit(): void {
    this.initForm();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  onChangedParty(party: INamedEntity): void {
    this.form.get('party')?.setValue(party);
    this.form.controls['party'].markAsDirty();
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

  save() {
    this.dialogRef.close(this.form.value);
  }

  close() {
      this.dialogRef.close();
  }

  private initForm() {
    this.form = this.fb.group({
      party: [null],
      nomenclature: [null],
      organization: [null],
      warehouse: [null],
    });
  }
}
