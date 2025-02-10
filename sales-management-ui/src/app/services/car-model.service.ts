import { Injectable } from "@angular/core"
import { HttpClient } from "@angular/common/http"
import { Observable } from "rxjs"
import { CarModel } from "../models/car-model.model"

@Injectable({
    providedIn: "root",
})
export class CarModelService {
    private apiUrl = "api/CarModel"

    constructor(private http: HttpClient) { }

    getAllModels(): Observable<CarModel[]> {
        return this.http.get<CarModel[]>(this.apiUrl)
    }

    getModelById(id: number): Observable<CarModel> {
        return this.http.get<CarModel>(`${this.apiUrl}/${id}`)
    }

    createModel(model: CarModel): Observable<CarModel> {
        return this.http.post<CarModel>(this.apiUrl, model)
    }

    updateModel(id: number, model: CarModel): Observable<CarModel> {
        return this.http.put<CarModel>(`${this.apiUrl}/${id}`, model)
    }

    deleteModel(id: number): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`)
    }

    searchModels(searchTerm: string): Observable<CarModel[]> {
        return this.http.get<CarModel[]>(`${this.apiUrl}/search?searchTerm=${searchTerm}`)
    }
}

