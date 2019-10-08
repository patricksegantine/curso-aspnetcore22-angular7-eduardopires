import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router } from '@angular/router';
import { AccountService } from '../../account/account.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  
  constructor(private organizadorService: AccountService,
              private router: Router) { 

  }

  canActivate(route: ActivatedRouteSnapshot): boolean {
    if(!this.organizadorService.isAuthenticated()) {
      this.router.navigate(['/entrar']);
    }
    let claims: any = route.data[0];
    if(claims !== undefined) {
      let claim = route.data[0]['claim'];

      if(claim) {
        let user = JSON.parse(this.organizadorService.getUser());

        if(!user.claims) {
          this.router.navigate(['/acesso-negado']);
        }

        let userClaims = user.claims.some(x => x.type === claim.nome && x.value === claim.valor);
        if(!userClaims) {
          this.router.navigate(['/acesso-negado']);
        }
      }
    }
    return true;
  }
  
}
