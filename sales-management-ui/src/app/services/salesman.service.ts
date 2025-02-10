import { Injectable } from "@angular/core"
import { HttpClient } from "@angular/common/http"
import { Observable } from "rxjs"
import { Salesman } from "../models/salesman.model"
import { Sale } from "../models/sale.model"

@Injectable({
    providedIn: "root",
})
export class SalesmanService {
    private apiUrl = "api/salesmen"

    constructor(private http: HttpClient) { }

    getAllSalesmen(): Observable<Salesman[]> {
        return this.http.get<Salesman[]>(this.apiUrl)
    }

    getSalesmanById(id: number): Observable<Salesman> {
        return this.http.get<Salesman>(`${this.apiUrl}/${id}`)
    }

    addSale(sale: Sale): Observable<Sale> {
        return this.http.post<Sale>(`${this.apiUrl}/sale`, sale)
    }
}

