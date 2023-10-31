import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subject, debounceTime, takeUntil } from 'rxjs';

@Component({
  selector: 'app-note-filter',
  templateUrl: './note-filter.component.html',
  styleUrls: ['./note-filter.component.scss']
})
export class NoteFilterComponent implements OnInit, OnDestroy {
  private readonly destroy$ = new Subject<void>();

  title: string;
  form: FormGroup;
  canDisabledForm: boolean;

  constructor(private fb: FormBuilder,
    private dialogRef: MatDialogRef<NoteFilterComponent>,
    @Inject(MAT_DIALOG_DATA) data: any) {
      this.title = data.title;
  }

  ngOnInit(): void {
    this.initProperties();
    this.initSubscriptions();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  save() {
    this.dialogRef.close(this.form.value);
  }

  close() {
      this.dialogRef.close();
  }

  private initProperties() {
    this.canDisabledForm = true;
    this.form = this.fb.group({
      executionDate: [null]
    });
  }

  private initSubscriptions() {
    this.form.valueChanges
      .pipe(
        debounceTime(100),
        takeUntil(this.destroy$))
      .subscribe(values => {
        this.canDisabledForm = !Object.keys(values)
            .map(key => values[key])
            .some(v => !!v);
      });
  }
}
