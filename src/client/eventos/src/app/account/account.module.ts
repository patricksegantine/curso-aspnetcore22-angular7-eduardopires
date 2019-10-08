import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { LoginComponent } from './login/login.component';
import { InscricaoComponent } from './inscricao/inscricao.component';
import { AccountRoutingModule } from './account.routing.module';

@NgModule({
    declarations: [
        LoginComponent,
        InscricaoComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        AccountRoutingModule
    ],
    exports: []
})
export class AccountModule {

}