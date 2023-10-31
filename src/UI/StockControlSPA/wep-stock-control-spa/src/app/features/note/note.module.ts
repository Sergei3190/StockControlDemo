import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NoteRoutingModule } from './note-routing.module';
import { NoteListModule } from './note-list/note-list.module';
import { NoteFilterModule } from './note-filter/note-filter.module';
import { NoteItemModule } from './note-item/note-item.module';

@NgModule({
  imports: [
    CommonModule,
    NoteRoutingModule,
    NoteListModule.forRoot(),
    NoteItemModule,
    NoteFilterModule,
  ],
  exports: [
    NoteRoutingModule,
    NoteListModule,
    NoteItemModule,
    NoteFilterModule,
  ]
})
export class NoteModule {}
