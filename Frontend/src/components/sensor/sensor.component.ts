import { Component } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { BackendData } from '../../services/backendData.service';
import {Sort} from '@angular/material/sort';

@Component({
    selector: 'app-sensor',
    templateUrl: './sensor.component.html',
    styleUrls: ['./sensor.component.css'],
})
export class SensorComponent {
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
        this.backendDataService.downloadCsv(this.filterToString()).subscribe((data) => {
            const a = document.createElement('a');
            const objectUrl = URL.createObjectURL(data);
            a.href = objectUrl;
            a.download = "data.csv";
            a.click();
        });
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
        //this.updateChart();
    }
    updateChart(){
        this.backendDataService.getData(this.filterToString()).subscribe((data) => {

            data.sort((a:any, b:any) => this.compare(a.date, b.date, true));
            
            const valuesTemperature = this.sensorData.filter((data1:any) => {
                return data1.sensorType == 'Temperature';
            }).map((item:any) => item.data);
            const datesTemperature = this.sensorData.filter((data1:any) => {
                return data1.sensorType == 'Temperature';
            }).map((item:any) => item.date);


            const valuesPressure = this.sensorData.filter((data1:any) => {
                return data1.sensorType == 'Pressure';
            }).map((item:any) => item.data);
            const datesPressure = this.sensorData.filter((data1:any) => {
                return data1.sensorType == 'Pressure';
            }).map((item:any) => item.date);

            const valuesRadiation = this.sensorData.filter((data1:any) => {
                return data1.sensorType == 'Radiation';
            }).map((item:any) => item.data);
            const datesRadiation = this.sensorData.filter((data1:any) => {
                return data1.sensorType == 'Radiation';
            }).map((item:any) => item.date);

            const valuesSeismometer = this.sensorData.filter((data1:any) => {
                return data1.sensorType == 'Seismometer';
            }).map((item:any) => item.data);
            const datesSeismometer = this.sensorData.filter((data1:any) => {
                return data1.sensorType == 'Seismometer';
            }).map((item:any) => item.date);

            let chartSeries: Array<any>;
            

            this.chartData = [{ data: valuesTemperature, label: 'Temperature' },
            { data: valuesPressure, label: 'Pressure' },
            { data: valuesRadiation, label: 'Radiation' },
            { data: valuesSeismometer, label: 'Seismometer' }];


            //const dates = data.map((item:any) => item.date);
            //this.labels = dates;

            //this.labels = [{datesTemperature, label: "Temperature"}, 
            //{datesPressure, label: "Pressure"}, 
            //{datesRadiation, label: "Radiation"}, 
            //{datesSeismometer, label: "Seismometer"}];
        });
    }
    
    displayedColumns: any = ['type', 'data', 'sensorId', 'date'];


    public options: any = { responsive: true };
    public chartData: Array<any> = [];
    //public labels: any = ["Temperature", "Pressure", "Seismometer", "Radiation"];
    public labels: Array<any> = [];

}   