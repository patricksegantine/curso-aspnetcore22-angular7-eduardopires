import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { AccountService } from 'src/app/account/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-menu-login',
  templateUrl: './menu-login.component.html',
  styleUrls: ['./menu-login.component.scss']
})
export class MenuLoginComponent implements OnInit {

  public user: any;
  public nome: string = "";

  constructor(private organizadorService: AccountService,
              private router: Router,
              private toastr: ToastrService) { }

  ngOnInit() {
  }

  usuarioLogado() : boolean {
    this.user = JSON.parse(this.organizadorService.getUser());

    if(this.user) {
      this.nome = this.user.nome;
    }

    return this.organizadorService.isAuthenticated();
  }

  logout() {
    this.organizadorService.removeUserToken();
    this.router.navigate(['/home']);
  }
}
