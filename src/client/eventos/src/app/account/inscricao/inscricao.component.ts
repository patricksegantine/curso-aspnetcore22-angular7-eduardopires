import { Component, OnInit, AfterViewInit, ViewChildren, ElementRef } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControlName, FormControl } from '@angular/forms';
import { Router } from '@angular/router';

import { Observable, fromEvent, merge } from 'rxjs';
import { CustomValidators } from 'ng2-validation';

import { GenericValidator } from 'src/app/utils/genericValidator';
import { AccountService } from '../account.service';
import { Organizador } from '../models/organizador.model';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-inscricao',
  templateUrl: './inscricao.component.html',
  styleUrls: ['./inscricao.component.scss']
})
export class InscricaoComponent implements OnInit, AfterViewInit {

  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[];

  inscricaoForm: FormGroup;
  organizador: Organizador;
  validationMessages: { [key: string]: { [key: string]: string } };
  displayMessage: { [key: string]: string } = {};
  erros: any[] = [];

  genericValidator: GenericValidator;

  constructor(private formBuilder: FormBuilder,
              private accountService: AccountService,
              private router: Router,
              private toastr: ToastrService) {
    this.validationMessages = {
      nome: {
        required: 'O campo Nome é requerido',
        minlength: 'O Nome precisa ter no mínimo 3 caracteres',
        maxlength: 'O Nome pode ter no máximo 150 caracteres'
      },
      cpfCnpj: {
        required: 'Informe o CPF ou CNPJ do organizador'
      },
      email: {
        required: 'Informe o e-mail',
        email: 'E-mail inválido'
      },
      password: {
        required: 'Informe a senha',
        minlength: 'A senha dever ter no mínimo 6 caracteres',
      },
      confirmPassword: {
        required: 'Informe a confirmação da senha',
        minlength: 'A senha deve ter no mínimo 6 caracteres',
        equalTo: 'As senhas não conferem'
      }
    }

    this.genericValidator = new GenericValidator(this.validationMessages);
  }

  ngOnInit() {
    let senha = new FormControl('', [Validators.required, Validators.minLength(6)]);
    let senhaConfirmada = new FormControl('', [Validators.required, Validators.minLength(6), CustomValidators.equalTo(senha)]);

    this.inscricaoForm = this.formBuilder.group({
      nome: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(150)]],
      cpfCnpj: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      password: senha,
      confirmPassword: senhaConfirmada
    });
  }

  ngAfterViewInit() {
    let controlsBlur: Observable<any>[] = this.formInputElements
      .map((formControl: ElementRef) => fromEvent(formControl.nativeElement, 'blur'));
    
    // operador spread
    merge(...controlsBlur).subscribe(value => {
      this.displayMessage = this.genericValidator.processMessages(this.inscricaoForm);
    });
  }

  onSubmit() {
    if (this.inscricaoForm.valid && this.inscricaoForm.dirty) {

      let novoOrganizador = Object.assign({}, this.organizador, this.inscricaoForm.value);

      this.accountService.registrarOrganizador(novoOrganizador)
        .subscribe(
          result => { this.onSaveComplete(result) },
          fail => { this.onError(fail) })
    }
  }

  onSaveComplete(response: any) {
    this.inscricaoForm.reset();
    this.erros = [];

    this.accountService.setLocalStorage(response.result.access_token, JSON.stringify(response.result.user));

    let toastrMessage = this.toastr.success("Usuário cadastrado com sucesso!", "");
    if (toastrMessage) {
      toastrMessage.onHidden.subscribe(() => {
        this.router.navigate(['/home']);
      });
    }
  }

  onError(fail: any) {
    console.log(fail);
    
    this.erros = fail.error.erros;
    console.log(JSON.stringify(this.erros));

    this.toastr.error("Erro ao cadastrar o usuário", "");
  }

}
