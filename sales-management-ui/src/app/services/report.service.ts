import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SalesmanCommissionReport } from '../models/salesman-commission-report.model';
import { SalesByBrandReport } from '../models/sales-by-brand-report.model';
import { SalesByClassReport } from '../models/sales-by-class-report.model';
import { TopSellingModelReport } from '../models/top-selling-model-report.model';

@Injectable({
    providedIn: 'root'
})
export class ReportService {
    private apiUrl = 'api/report';

    constructor(private http: HttpClient) { }

    getSalesmanCommissionReport(): Observable<SalesmanCommissionReport[]> {
        return this.http.get<SalesmanCommissionReport[]>(`${this.apiUrl}/salesman-commission`);
    }

    getSalesByBrandReport(): Observable<SalesByBrandReport[]> {
        return this.http.get<SalesByBrandReport[]>(`${this.apiUrl}/sales-by-brand`);
    }

    getSalesByClassReport(): Observable<SalesByClassReport[]> {
        return this.http.get<SalesByClassReport[]>(`${this.apiUrl}/sales-by-class`);
    }

    getTopSellingModelsReport(top: number = 10): Observable<TopSellingModelReport[]> {
        return this.http.get<TopSellingModelReport[]>(`${this.apiUrl}/top-selling-models`, { params: { top: top.toString() } });
    }
}