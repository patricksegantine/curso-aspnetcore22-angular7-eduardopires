import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ListaEventosComponent } from './lista-eventos/lista-eventos.component';
import { AdicionarEventoComponent } from './adicionar-evento/adicionar-evento.component';

const eventosRoutes: Routes = [
  { path:'eventos', component: ListaEventosComponent },
  { path:'novo-evento', component: AdicionarEventoComponent }, //canActivate:[EventosGuard], data: [{claim: {nome: 'Eventos', valor: 'Gravar'}}] },
  { path:'', redirectTo: 'eventos', pathMatch: 'full' }];

@NgModule({
  imports: [RouterModule.forChild(eventosRoutes)],
  exports: [RouterModule]
})
export class EventosRoutingModule {}