import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router'; 
import { authenticationGuard } from './shared/modules/authentication/guards/authentication.guard';
import { environment } from 'src/environments/environment';

const routes: Routes = [
  {
    path: '',
    canActivate: [authenticationGuard],
    loadChildren: () => import('./features/features.module').then(m => m.FeaturesModule)
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules, useHash: environment.useHash })],
  exports: [RouterModule]
})

export class AppRoutingModule { }
