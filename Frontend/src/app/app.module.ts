import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app.routes';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { PanelModule } from 'primeng/panel';
import { InplaceModule } from 'primeng/inplace';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { InputSwitchModule } from 'primeng/inputswitch';

import { SensorComponent } from '../components/sensor/sensor.component';
import { MatTableModule } from '@angular/material/table' 
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import {MatSortModule} from '@angular/material/sort';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatNativeDateModule} from '@angular/material/core';
import { NgChartsModule } from 'ng2-charts';

// widoki i style
import { TopBarComponent } from './top-bar/top-bar.component';
import { FooterComponent } from './footer/footer.component';
import { NgChartsConfiguration } from 'ng2-charts';
import { Subscription, timer } from 'rxjs';


@NgModule({
    declarations: [
        AppComponent,
        SensorComponent,
        TopBarComponent,
        FooterComponent
    ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        AppRoutingModule,
        HttpClientModule, 
        InputSwitchModule,
        ReactiveFormsModule,
        FormsModule,
        PanelModule,
        InplaceModule,
        MatTableModule,
        NgbModule,
        MatInputModule,
        MatSortModule,
        MatDatepickerModule,
        MatFormFieldModule,
        MatNativeDateModule,
        MatSelectModule,
        NgChartsModule,
    ],
    providers: [{ provide: NgChartsConfiguration, useValue: { generateColors: false }}],
    bootstrap:[
        AppComponent,
    ]
})

export class AppModule {}
