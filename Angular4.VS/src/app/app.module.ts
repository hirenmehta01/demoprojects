import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { routing } from './app.routing';
import { AppComponent } from './app.component';
import { ProductListComponent } from './products/products-list.component';
import { NavbarComponent } from './shared/navbar.component';
import { AngularHelp } from './angularhelp/angularhelp.component'
@NgModule({
  declarations: [
    AppComponent,
      ProductListComponent,
      NavbarComponent,
      AngularHelp
  ],
  imports: [
      BrowserModule, routing
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
