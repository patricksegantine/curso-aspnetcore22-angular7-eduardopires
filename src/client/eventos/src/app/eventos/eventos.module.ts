import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdicionarEventoComponent } from './adicionar-evento/adicionar-evento.component';
import { ListaEventosComponent } from './lista-eventos/lista-eventos.component';
import { EventosRoutingModule } from './eventos.routing.module';

@NgModule({
  declarations: [
    AdicionarEventoComponent,
    ListaEventosComponent
  ],
  imports: [
    CommonModule,
    EventosRoutingModule
  ]
})
export class EventosModule { }
