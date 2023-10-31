import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Guid } from 'guid-ts';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-nomenclature-item-create',
  templateUrl: './nomenclature-item-create.component.html',
  styleUrls: ['./nomenclature-item-create.component.scss']
})
export class NomenclatureItemCreateComponent implements OnInit, OnDestroy {
  private readonly destroy$ = new Subject<void>();

  title: string;
  form: FormGroup;

  constructor(private fb: FormBuilder,
    private dialogRef: MatDialogRef<NomenclatureItemCreateComponent>,
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
      case 'name':
        return this.form.get(name)?.hasError('required') ? 'Поле "Наименование" обязательно' : '';
      default: return '';
    }
  }

  save() {
    const model = {...this.form.value};
    model.id = (Guid.empty().toString() as unknown as Guid);
    this.dialogRef.close(model);
  }

  close() {
      this.dialogRef.close();
  }

  private initProperties() {
    this.form = this.fb.group({
      id: [null],
      name: [null, Validators.required],
      classifier: [null],
    });
  }
}
