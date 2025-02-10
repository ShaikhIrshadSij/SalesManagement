import { Injectable } from "@angular/core"
import { HttpClient } from "@angular/common/http"
import { Observable } from "rxjs"
import { SalesmanCommissionReport } from "../models/salesman-commission-report.model"

@Injectable({
    providedIn: "root",
})
export class ReportService {
    private apiUrl = "api/report"

    constructor(private http: HttpClient) { }

    getSalesmanCommissionReport(): Observable<SalesmanCommissionReport[]> {
        return this.http.get<SalesmanCommissionReport[]>(`${this.apiUrl}/salesman-commission`)
    }
}

