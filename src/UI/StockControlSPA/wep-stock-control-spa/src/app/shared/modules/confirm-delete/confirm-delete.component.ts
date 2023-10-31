import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-confirm-delete',
  templateUrl: './confirm-delete.component.html',
  styleUrls: ['./confirm-delete.component.scss']
})

export class ConfirmDeleteComponent implements OnInit {

  title: string;
  content: string;

  constructor(private dialogRef: MatDialogRef<ConfirmDeleteComponent>,
    @Inject(MAT_DIALOG_DATA) data: any) {
      this.title = data.title;
      this.content = data.content;
  }

  ngOnInit(): void {
  }

  save() {
    this.dialogRef.close(true);
  }

  close() {
      this.dialogRef.close(false);
  }
}
