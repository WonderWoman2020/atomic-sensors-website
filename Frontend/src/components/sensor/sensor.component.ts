import { Component, Inject, OnDestroy, OnInit, numberAttribute } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { BackendData } from '../../services/backendData.service';
import { isNgContainer } from '@angular/compiler';
import * as moment from 'moment';
import { EventService, Events } from '../../services/event.service';
import * as signalR from '@microsoft/signalr';
import { Subscription } from 'rxjs';


@Component({
    selector: 'app-sensor',
    templateUrl: './sensor.component.html',
    styleUrls: ['./sensor.component.scss'],
})
export class SensorComponent implements OnInit, OnDestroy{

    constructor(private formBuilder: FormBuilder, private backendDataService: BackendData, private eventService: EventService){

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

    sensorData: any;

    getData(){
        this.backendDataService.getData(this.reduceFilter(this.filter)).subscribe((data) => {
            data.map((row: any) => {
                row.time = moment.default(row.time).format('HH:mm:ss dd-mm-YYYY');
                return row;
            });
            this.sensorData = data;
        });
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
    private subscriptions: Subscription[] = [];

    ngOnInit(): void {
        this.backendConnection = new signalR.HubConnectionBuilder().withUrl('http://localhost:4545/api/notify').build();

        this.backendConnection.start().then(
            () => { 
                this.backendConnection.push(this.backendConnection.on('Receive', (data:any) =>{
                    const {type, value} = data;
                    this.panelAddValue(type, value);
                }));
                this.getData(); 
            });

        this.subscriptions.push(this.eventService.on(Events.FILTER).subscribe((data:any) =>{
            this.getData();
        }));
    }

    ngOnDestroy(): void {
        this.subscriptions.forEach((subscription) => subscription.unsubscribe());
    }

    columns: any[] = [
        { field: 'type', header: 'Sensor Type' },
        { field: 'ID', header: 'Sensor ID' },
        { field: 'value', header: 'Value' },
        { field: 'date', header: 'Date' },
    ];
    
}   