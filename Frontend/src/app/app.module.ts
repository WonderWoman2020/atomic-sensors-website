import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app.routes';
import { AppComponent } from './app.component';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { PrimeNgModule } from './primeng.module';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { PanelModule } from 'primeng/panel';
import { InplaceModule } from 'primeng/inplace';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { InputSwitchModule } from 'primeng/inputswitch';

import { SensorComponent } from '../components/sensor/sensor.component';
import { SensorFilterComponent } from '../components/sensor-filter/sensor.filter.component';


@NgModule({
    declarations: [
        AppComponent,
        SensorComponent,
        SensorFilterComponent,
    ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        AppRoutingModule,
        HttpClientModule, 
        PrimeNgModule,
        InputSwitchModule,
        ReactiveFormsModule,
        FormsModule,
        PanelModule,
        InplaceModule,
    ],
    providers: [],
    bootstrap:[
        AppComponent,
    ]
})

export class AppModule {}
