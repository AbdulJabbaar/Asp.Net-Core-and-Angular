import { BrowserXhr } from '@angular/http'
import * as Raven from 'raven-js'
import { ErrorHandler } from '@angular/core'
import { BrowserModule } from '@angular/platform-browser'
import { NgModule } from '@angular/core'
import { RouterModule, Routes } from '@angular/router'
import { FormsModule } from '@angular/forms'
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http'

import { AppErrorHandler } from './app.error-handler'

import { AppComponent } from './app.component'
import { VehicleFormComponent } from './vehicle-form/vehicle-form.component'
import { VehicleComponent } from './vehicle/vehicle.component'
import { PaginationComponent } from './shared/pagination.component'

import { VehicleService } from './services/vehicle.service';
import { ViewVehicleComponent } from './view-vehicle/view-vehicle.component'
import { PhotoService } from './services/photo.service';
import { BrowserXhrWithProgress, ProgressService } from './services/progress.service';
import { AuthService } from './services/auth.service';
import { AdminComponent } from './admin/admin.component';
import { AuthGuard } from './services/auth-guard.service';
import { AuthInterceptor } from './services/authInterceptor.service';

Raven.config('https://5db3603647ff43bb861daf7dd34d4842@sentry.io/1216024').install();

@NgModule({
  declarations: [
    AppComponent,
    VehicleFormComponent,
    VehicleComponent,
    PaginationComponent,
    ViewVehicleComponent,
    AdminComponent
  ],
  imports: [
    BrowserModule,
    RouterModule.forRoot([
      { path: '', redirectTo: 'vehicles', pathMatch: 'full' },
      { path: 'vehicles', component: VehicleComponent },
      { path: 'vehicles/new', component: VehicleFormComponent },
      { path: 'vehicles/edit/:id', component: VehicleFormComponent },
      { path: 'vehicles/:id', component: ViewVehicleComponent },
      { path: 'admin', component: AdminComponent, canActivate: [AuthGuard] }
    ]),
    FormsModule,
    HttpClientModule
  ],
  providers: [
    { provide: ErrorHandler, useClass: AppErrorHandler },
    { provide: BrowserXhr, useClass: BrowserXhrWithProgress },
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    VehicleService,
    PhotoService,
    ProgressService,
    AuthService,
    AuthGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
