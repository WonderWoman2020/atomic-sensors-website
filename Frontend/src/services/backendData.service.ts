import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class BackendData {

    constructor(private http: HttpClient) {}

    downloadFile(url: string): Observable<Blob> {
        return this.http.get(url, {responseType: 'blob', observe: 'body'});
    }

    downloadJson(filters: any): Observable<any> {
        return this.downloadFile('http://backend:5000/api/data/json'+filters);
    }

    downloadCsv(filters: any): Observable<any> {
        return this.downloadFile('http://backend:5000/api/data/csv'+filters);
    }

    getData(filters: any): Observable<any> {
        return this.http.get('http://backend:5000/api/data'+filters, {});
    }
    
}