import { Component, OnDestroy, OnInit, } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { BackendData } from '../../services/backendData.service';

import * as moment from 'moment';


import { timer } from 'rxjs';

@Component({
    selector: 'app-sensor',
    templateUrl: './sensor.component.html',
    styleUrls: ['./sensor.component.css'],
})
export class SensorComponent implements OnInit, OnDestroy {
    sensorData: any;
    constructor(private formBuilder: FormBuilder, private backendDataService: BackendData){
        
    }
    json(){
        this.backendDataService.downloadJson(this.filter);
    }
    csv(){
        this.backendDataService.downloadCsv(this.filter);
    }

    filter: FormGroup = this.formBuilder.group({
        id: '',
        type: '',
        orderBy: '',
        sort_mode: 'asc',
        startDate: '',
        endDate: '',
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

    sendFilter(){

    }
    
    displayedColumns: any = ['id', 'data', 'sensorId', 'date'];
  
    ngOnInit() {
      this.getData();
     }
    
    ngOnDestroy() {
      
    } 
}   