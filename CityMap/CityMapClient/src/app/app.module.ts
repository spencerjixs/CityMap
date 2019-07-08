import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from "@angular/forms";
import { HttpClientModule } from "@angular/common/http";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { AgmCoreModule } from '@agm/core';
import { AgmSnazzyInfoWindowModule } from '@agm/snazzy-info-window';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CityMapComponent } from './city-map/city-map.component';
import { CityMapService } from './city-map/city-map.service';

@NgModule({
  declarations: [
    AppComponent,
    CityMapComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    AgmCoreModule.forRoot({
    apiKey: 'AIzaSyCD6_zE3cGVcISXTKtW-WJkEtMks4mfBNE'
    }),
    AgmSnazzyInfoWindowModule
  ],
  providers: [CityMapService],
  bootstrap: [AppComponent]
})
export class AppModule { }