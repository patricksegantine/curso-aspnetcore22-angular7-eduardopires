import { Evento } from './../models/evento';
import { EventosService } from './../eventos.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-lista-eventos',
  templateUrl: './lista-eventos.component.html',
  styleUrls: ['./lista-eventos.component.scss']
})
export class ListaEventosComponent implements OnInit {
  public eventos: Evento[];
  errorMessage: string;

  constructor(private eventoServico: EventosService) { }

  ngOnInit() {
    this.eventoServico.obterTodos()
      .subscribe(
        dados => {
          console.log(dados);
          this.eventos = dados;
        },
        error => this.errorMessage = error
      );
  }

}
