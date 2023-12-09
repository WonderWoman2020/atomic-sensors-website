import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class backendData {

    constructor(private http: HttpClient) {}

    downloadFile(url: string, params: any): Observable<Blob> {

        let httpParams = new HttpParams();
        for(const key in params)
            if(params.hasOwnProperty(key))
                httpParams.append(key, params[key]);
        
        return this.http.get(url, {responseType: 'blob', observe: 'body', params: httpParams});
    }

    downloadJson(filters: any): Observable<any> {
        return this.downloadFile('/api/data/json', filters);
    }

    downloadCsv(filters: any): Observable<any> {
        return this.downloadFile('/api/data/csv', filters);
    }
}