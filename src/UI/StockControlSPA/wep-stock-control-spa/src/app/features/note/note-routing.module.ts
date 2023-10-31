import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NoteListComponent } from './note-list/note-list.component';
import { NoteItemComponent } from './note-item/note-item.component';

const routes: Routes = [
  {
    path: '',
    title: 'Заметки',
    component: NoteListComponent,

    children: [
      {
        path: ':id',
        title: 'Редактирование',
        component: NoteItemComponent,
      },
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class NoteRoutingModule { }
