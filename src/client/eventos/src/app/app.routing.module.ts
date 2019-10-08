import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { InscricaoComponent } from './account/inscricao/inscricao.component';
import { LoginComponent } from './account/login/login.component';
import { AuthGuard } from './shared/guards/auth.guard';

const routes: Routes = [
  { path:'', redirectTo: 'home', pathMatch: 'full' },
  { path:'home', component: HomeComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {useHash: true})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
