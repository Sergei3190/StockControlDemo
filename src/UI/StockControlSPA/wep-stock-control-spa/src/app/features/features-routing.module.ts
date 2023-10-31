import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FeaturesComponent } from './features.component';

const routes: Routes = [
{
    path: '',
    component: FeaturesComponent,
    children: [
        {
          path: '',
          redirectTo: 'stock-availability',
          pathMatch: 'full'
        },
        {
          path: 'stock-availability',
          loadChildren: () => import("./stock-availability/stock-availability.module").then((m) => m.StockAvailabilityModule)
        },
        {
          path: 'product-flow',
          loadChildren: () => import("./product-flow/product-flow.module").then((m) => m.ProductFlowModule)
        },        {
          path: 'classifiers',
          loadChildren: () => import("./classifier/classifier.module").then((m) => m.ClassifierModule)
        },
        {
          path: 'personal-cabinet',
          loadChildren: () => import("./personal-cabinet/personal-cabinet.module").then((m) => m.PersonalCabinetModule)
        },
        {
          path: 'notifications',
          loadChildren: () => import("./notification/notification.module").then((m) => m.NotificationModule)
        },
        {
          path: 'notes',
          loadChildren: () => import("./note/note.module").then((m) => m.NoteModule)
        }
      ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class FeaturesRoutingModule { }