import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SensorComponent } from '../components/sensor/sensor.component';


enum Views{
    ANY = '**',
    MAIN = 'main'
}

const routes: Routes = [
  { path: Views.MAIN, component: SensorComponent },
  { path: Views.ANY, redirectTo: Views.MAIN },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}

