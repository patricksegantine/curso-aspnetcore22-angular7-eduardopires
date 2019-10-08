import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { InscricaoComponent } from './inscricao/inscricao.component';

const organizadorRoutes : Routes = [
  { path: 'entrar', component: LoginComponent },
  { path: 'inscricao', component: InscricaoComponent }
];

@NgModule({
  imports: [RouterModule.forChild(organizadorRoutes)],
  exports: [RouterModule]
})
export class AccountRoutingModule { }
