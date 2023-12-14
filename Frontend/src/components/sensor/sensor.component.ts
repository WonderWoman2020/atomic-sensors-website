import { Component } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { BackendData } from '../../services/backendData.service';
import {Sort} from '@angular/material/sort';
import { Subscription, timer } from 'rxjs';
import { map, interval } from 'rxjs';


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
        this.updateChart();
    }
    updateChart(){
        this.backendDataService.getData(this.filterToString()).subscribe((data) => {

            data.sort((a:any, b:any) => this.compare(a.date, b.date, true));
            
            const valuesTemperature : any[] = this.sensorData.filter((data1:any) => {
                return data1.sensorType == 'Temperature';
            }).map((item:any) => item.data);
            const datesTemperature : any[] = this.sensorData.filter((data1:any) => {
                return data1.sensorType == 'Temperature';
            }).map((item:any) => item.date);


            const valuesPressure : any[] = this.sensorData.filter((data1:any) => {
                return data1.sensorType == 'Pressure';
            }).map((item:any) => item.data);
            const datesPressure : any[] = this.sensorData.filter((data1:any) => {
                return data1.sensorType == 'Pressure';
            }).map((item:any) => item.date);

            const valuesRadiation : any[]= this.sensorData.filter((data1:any) => {
                return data1.sensorType == 'Radiation';
            }).map((item:any) => item.data);
            const datesRadiation : any[] = this.sensorData.filter((data1:any) => {
                return data1.sensorType == 'Radiation';
            }).map((item:any) => item.date);

            const valuesSeismometer : any[] = this.sensorData.filter((data1:any) => {
                return data1.sensorType == 'Seismometer';
            }).map((item:any) => item.data);
            const datesSeismometer :any[] = this.sensorData.filter((data1:any) => {
                return data1.sensorType == 'Seismometer';
            }).map((item:any) => item.date);

            let chartSeries: Array<any>;
            

            this.chartData = [{ data: valuesTemperature, label: 'Temperature', backgroundColor: ["#ffc1bd"], borderColor: ["#f7564a"] }];/*[{ data: valuesTemperature, label: 'Temperature' },
            { data: valuesPressure, label: 'Pressure' },
            { data: valuesRadiation, label: 'Radiation' },
            { data: valuesSeismometer, label: 'Seismometer' }];*/
            this.chartData2 = [{ data: valuesPressure, label: 'Pressure', backgroundColor: ["#a2f59f"], borderColor: ["#43cf3e"] }];
            this.chartData3 = [{ data: valuesRadiation, label: 'Radiation', backgroundColor: ["#91cefa"], borderColor: ["#4fa8e8"] }];
            this.chartData4 = [{ data: valuesSeismometer, label: 'Seismometer', backgroundColor: ["#de91ff"], borderColor: ["#a43bd1"]  }];


            //const dates = data.map((item:any) => item.date);
            //this.labels = dates;
            this.labels = datesTemperature;
            this.labels2 = datesPressure;
            this.labels3 = datesRadiation;
            this.labels4 = datesSeismometer;
            console.log(valuesTemperature.length);
            console.log(datesTemperature.length);
            console.log(valuesTemperature);
            console.log(datesTemperature);


            /*this.labels = [{datesTemperature, label: "Temperature"}, 
            {datesPressure, label: "Pressure"}, 
            {datesRadiation, label: "Radiation"}, 
            {datesSeismometer, label: "Seismometer"}];*/
        });
    }
    
    displayedColumns: any = ['type', 'data', 'sensorId', 'date'];


    public options: any = { responsive: true };
    public chartData: Array<any> = [];
    public chartData2: Array<any> = [];
    public chartData3: Array<any> = [];
    public chartData4: Array<any> = [];
    //public labels: any = ["Temperature", "Pressure", "Seismometer", "Radiation"];
    public labels: Array<any> = [];
    public labels2: Array<any> = [];
    public labels3: Array<any> = [];
    public labels4: Array<any> = [];

    observable = interval(1000);
    subscription : any;

    public temperatureMean : number = 0;
    public temperatureLast : number = 0;

    public pressureMean : number = 0;
    public pressureLast : number = 0;

    public radiationMean : number = 0;
    public radiationLast : number = 0;

    public seismometerMean : number = 0;
    public seismometerLast : number = 0;

    public temperatureStats : any;
    public pressureStats : any;
    public radiationStats : any;
    public seismometerStats : any;

    ngOnInit() : void
    {
          this.subscription = this.observable.subscribe(x =>
            {
             console.log(x);
             this.backendDataService.getStats("?type_filter=Temperature").subscribe((data) => {
                this.temperatureStats = data;
                console.log(this.temperatureStats);
                this.temperatureLast = this.temperatureStats[0].last;
                this.temperatureMean = this.temperatureStats[0].mean;
            });
            this.backendDataService.getStats("?type_filter=Pressure").subscribe((data) => {
                this.pressureStats = data;
                console.log(this.pressureStats);
                this.pressureLast = this.pressureStats[0].last;
                this.pressureMean = this.pressureStats[0].mean;
            });
            this.backendDataService.getStats("?type_filter=Radiation").subscribe((data) => {
                this.radiationStats = data;
                console.log(this.radiationStats);
                this.radiationLast = this.radiationStats[0].last;
                this.radiationMean = this.radiationStats[0].mean;
            });
            this.backendDataService.getStats("?type_filter=Seismometer").subscribe((data) => {
                this.seismometerStats = data;
                console.log(this.seismometerStats);
                this.seismometerLast = this.seismometerStats[0].last;
                this.seismometerMean = this.seismometerStats[0].mean;
            });
            }
            );
    }

    ngOnDestroy() : void 
    {
        this.subscription.unsubscribe();
    }
    

}   