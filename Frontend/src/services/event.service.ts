import { Injectable } from '@angular/core';
import { Subject, Subscription } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { Observable } from 'rxjs/internal/Observable';

interface BcEvent {
    key: any;
    data?: any;
}

@Injectable({ providedIn: 'root' })
export class EventService {
    private eventBus: Subject<BcEvent>;

    constructor() { this.eventBus = new Subject<BcEvent>(); }

    broadcast(key: any, data?: any) { this.eventBus.next({ key, data }); }

    on<T>(key: any): Observable<T> {
        return this.eventBus.asObservable().pipe( filter((event: any) => event.key === key), map((event: any) => <T>event.data));
    }
}

export enum Events {
    FILTER,
    RESET,
}