import { Component, OnDestroy, OnInit, } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { BackendData } from '../../services/backendData.service';
import {Sort, MatSortModule} from '@angular/material/sort';

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
        this.backendDataService.downloadJson(this.filterToString()).subscribe((data) => {
            const a = document.createElement('a');
            const objectUrl = URL.createObjectURL(data);
            a.href = objectUrl;
            a.download = "data.json";
            a.click();
        });
    }
    csv(){
        //this.backendDataService.downloadCsv(this.filter);
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
    
    filterToString(): string{
        let params: string = "";

        params = 
        "?sort_mode=" + this.filter.get('sort_mode')?.value + "&" +
        "id_filter=" + this.filter.get('id')?.value + "&" +
        "type_filter=" + this.filter.get('type')?.value + "&" +
        "date_from=" + this.filter.get('startDate')?.value + "&" +
        "date_to=" + this.filter.get('endDate')?.value;
        console.log(params);
        return params;
    }

    getData(){
        this.backendDataService.getData(this.filterToString()).subscribe((data) => {
            this.sensorData = data;
        });
    }


    sortData(sort: Sort) {
        const data = this.sensorData.slice();
        if (!sort.active || sort.direction === '') {
          this.sensorData = data;
          return;
        }
    
        this.sensorData = data.sort((a:any, b:any) => {
          const isAsc = sort.direction === 'asc';
          switch (sort.active) {
            case 'sensorId':
              return this.compare(a.sensorId, b.sensorId, isAsc);
            case 'type':
              return this.compare(a.sensorType, b.sensorType, isAsc);
            case 'date':
              return this.compare(a.date, b.date, isAsc);
            case 'data':
              return this.compare(a.data, b.data, isAsc);
            default:
              return 0;
          }
        });
      }

    compare(a: number | string, b: number | string, isAsc: boolean) {
        return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
      }

    sendFilter(){
        this.getData();
    }
    
    displayedColumns: any = ['type', 'data', 'sensorId', 'date'];
  
    ngOnInit() {
      //this.getData();
     }
    
    ngOnDestroy() {
      
    } 
}   