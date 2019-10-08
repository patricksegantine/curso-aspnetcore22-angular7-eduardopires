import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

// Third componentes
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { CustomFormsModule } from 'ng2-validation'
import { ToastrModule } from 'ngx-toastr';

// Shared componentes
import { MenuSuperiorComponent } from './shared/components/menu-superior/menu-superior.component';
import { MenuLoginComponent } from './shared/components/menu-login/menu-login.component';
import { FooterComponent } from './shared/components/footer/footer.component';
import { AppRoutingModule } from './app.routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { AcessoNegadoComponent } from './shared/components/acesso-negado/acesso-negado.component';

// Modules
import { EventosModule } from './eventos/eventos.module';

// Services
import { SeoService } from './shared/services/seo.service';
import { AccountModule } from './account/account.module';

@NgModule({
  declarations: [
    AppComponent,
    MenuSuperiorComponent,
    MenuLoginComponent,
    FooterComponent,
    HomeComponent,
    AcessoNegadoComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    CustomFormsModule,
    EventosModule,
    AccountModule,
    AppRoutingModule,
    ToastrModule.forRoot(),
    CollapseModule.forRoot(),
    CarouselModule.forRoot()
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
