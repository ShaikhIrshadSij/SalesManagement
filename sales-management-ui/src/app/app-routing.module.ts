import { NgModule } from "@angular/core"
import { RouterModule, Routes } from "@angular/router"
import { CarModelListComponent } from "./components/car-model-list/car-model-list.component"
import { CarModelFormComponent } from "./components/car-model-form/car-model-form.component"
import { SalesmanReportComponent } from "./components/salesman-report/salesman-report.component"
import { LoginComponent } from "./components/login/login.component"
import { AuthGuard } from "./guards/auth.guard"

const routes: Routes = [
    { path: "", redirectTo: "/car-models", pathMatch: "full" },
    { path: "login", component: LoginComponent },
    { path: "car-models", component: CarModelListComponent, canActivate: [AuthGuard] },
    { path: "car-models/new", component: CarModelFormComponent, canActivate: [AuthGuard] },
    { path: "car-models/:id", component: CarModelFormComponent, canActivate: [AuthGuard] },
    { path: "salesman-report", component: SalesmanReportComponent, canActivate: [AuthGuard] },
    { path: "**", redirectTo: "/car-models" },
]

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
})
export class AppRoutingModule { }

