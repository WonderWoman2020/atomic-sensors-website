import { AfterViewInit, Component, Inject, OnDestroy, OnInit, numberAttribute } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { BackendData } from '../../services/backendData.service';
import { isNgContainer } from '@angular/compiler';
import * as moment from 'moment';
import { Subscription, elementAt } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import {MatGridListModule} from '@angular/material/grid-list';

import { timer } from 'rxjs';

@Component({
    selector: 'app-sensor',
    templateUrl: './sensor.component.html',
    styleUrls: ['./sensor.component.css'],
})
export class SensorComponent implements OnInit, OnDestroy, AfterViewInit {
    sensorData: any;
    constructor(private formBuilder: FormBuilder, private backendDataService: BackendData){
        
    }

    filter: FormGroup = this.formBuilder.group({
        id: undefined,
        type: undefined,
        orderBy: undefined,
        sort_mode: 'asc',
    });

    reduceFilter(rawFilter: any){
        return Object.keys(rawFilter).reduce((accumulator, key) => {
            if(rawFilter[key] !== undefined && rawFilter[key] !== null)
                return { ...accumulator, [key]: rawFilter[key]};
            else
                return accumulator;
            
        }, {});
    }
    

    getData(){
        this.backendDataService.getData(this.reduceFilter(this.filter)).subscribe((data) => {
            data.map((row: any) => {
                //row.date = moment.default(row.date).format('HH:mm:ss dd-mm-YYYY');
                return row;
            });
            this.sensorData = data;
        });
        console.log(this.sensorData);
    }

    ngAfterViewInit(): void {
        //this.getData();
    }
    
    type: any = {values: [], lastValue: null, averageValue: null};
    panel: any = {
        temperature: {get: this.type},
        pressure: {get: this.type},
        seismometer: {get: this.type},
        radiation: {get: this.type},
    };

    displayedColumns: any = ['id', 'data', 'sensorId', 'date'];

    panelAddValue(type: string, value: number){
        if(this.panel[type].values.length > 100)
            this.panel[type].values.shift();
        
        this.panel[type].lastValue = value;    
        this.panel[type].averageValue = this.panel[type].values.reduce((x:any, y:any) => x + y, 0)/this.panel[type].values.length;
    }

    charts: any = {
        temperature: {label: [], data: []},
        pressure: {label: [], data: []},
        seismometer: {label: [], data: []},
        radiation: {label: [], data: []},
    };

    backendConnection: any;
  
    ngOnInit() {
      this.getData();
     }
    
    ngOnDestroy() {
      
    }

    columns: any[] = [
        { field: 'type', header: 'Sensor Type' },
        { field: 'ID', header: 'Sensor ID' },
        { field: 'value', header: 'Value' },
        { field: 'date', header: 'Date' },
    ];
    
}   