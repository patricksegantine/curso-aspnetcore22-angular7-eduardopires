import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

import { ServiceBase } from '../shared/services/service.base';
import { Organizador } from './models/organizador.model';

@Injectable({
  providedIn: 'root'
})
export class AccountService extends ServiceBase {

  constructor(private http: HttpClient) {
    super();
  }

  registrarOrganizador(organizador: Organizador): Observable<Organizador> {
    return this.http
      .post(this.UrlServiceV1 + '/nova-conta', organizador, super.ObterHeaderJson())
      .pipe(
        map(super.extractData),
        catchError(super.serviceError)
      );
  }

  login(organizador: Organizador) : Observable<Organizador> {
    return this.http
      .post(this.UrlServiceV1 + 'conta', organizador, super.ObterHeaderJson())
      .pipe(
        map(super.extractData),
        catchError(super.serviceError)
      );
  }
}