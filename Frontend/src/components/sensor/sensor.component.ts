import { AfterViewInit, Component, Inject, OnDestroy, OnInit, numberAttribute } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { BackendData } from '../../services/backendData.service';
import { isNgContainer } from '@angular/compiler';
import * as moment from 'moment';
import { Subscription, elementAt } from 'rxjs';
import { HttpClient } from '@angular/common/http';

export interface Element {
    id: number;
    value: number;

  }



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
        order: 'ascending',
    });

    reduceFilter(rawFilter: any){
        return Object.keys(rawFilter).reduce((accumulator, key) => {
            if(rawFilter[key] !== undefined)
                return { ...accumulator, [key]: rawFilter[key]};
            else
                return accumulator;
            
        }, {});
    }
    
    tablica: any = [{
        "id": 1,
        "value": 24,
    },{
        "id": 2,
        "value": 22,
    },{
        "id": 5,
        "value": 24,
    }];

    getData(){
        this.backendDataService.getData(this.reduceFilter(this.filter)).subscribe((data) => {
            data.map((row: any) => {
                //row.time = moment.default(row.time).format('HH:mm:ss dd-mm-YYYY');
                id: row.sensorId;
                type: row.sensorType;
                value: data;
                return row;
            });
            this.sensorData = data;
        });
        console.log(this.sensorData);
        //this.http.get('http://localhost:5000/api/data');
    }

    ngAfterViewInit(): void {
        this.getData();
    }
    
    type: any = {values: [], lastValue: null, averageValue: null};
    panel: any = {
        temperature: {get: this.type},
        pressure: {get: this.type},
        seismometer: {get: this.type},
        radiation: {get: this.type},
    };

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

    ngOnInit(): void {
        
    }

    ngOnDestroy(): void {
        
    }

    columns: any[] = [
        { field: 'type', header: 'Sensor Type' },
        { field: 'ID', header: 'Sensor ID' },
        { field: 'value', header: 'Value' },
        { field: 'date', header: 'Date' },
    ];
    
}   