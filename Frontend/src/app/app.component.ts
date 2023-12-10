import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PrimeNGConfig } from 'primeng/api';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit{

  EventBusSubscription?: Subscription;

  constructor(
    private config: PrimeNGConfig,
    private router: Router
  ) {  }

  ngOnInit(): void {}
}
