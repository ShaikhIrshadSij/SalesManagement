import { NgModule } from "@angular/core"
import { BrowserModule } from "@angular/platform-browser"
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http"
import { FormsModule, ReactiveFormsModule } from "@angular/forms"
import { AppComponent } from "./app.component"
import { CarModelListComponent } from "./components/car-model-list/car-model-list.component"
import { CarModelFormComponent } from "./components/car-model-form/car-model-form.component"
import { SalesmanReportComponent } from "./components/salesman-report/salesman-report.component"
import { ErrorInterceptor } from "./interceptors/error.interceptor"
import { AuthGuard } from "./guards/auth.guard"
import { NotificationService } from "./services/notification.service"
import { AppRoutingModule } from './app-routing.module'
import { ApiInterceptor } from './interceptors/http.interceptor'
import { LoginComponent } from './components/login/login.component'

@NgModule({
    declarations: [AppComponent, CarModelListComponent, CarModelFormComponent, SalesmanReportComponent, LoginComponent],
    imports: [BrowserModule, AppRoutingModule, HttpClientModule, ReactiveFormsModule, FormsModule],
    providers: [
        { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
        { provide: HTTP_INTERCEPTORS, useClass: ApiInterceptor, multi: true },
        AuthGuard, NotificationService],
    bootstrap: [AppComponent],
})
export class AppModule { }

