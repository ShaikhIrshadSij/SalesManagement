import { Injectable } from "@angular/core"
import { HttpClient } from "@angular/common/http"
import { BehaviorSubject, Observable } from "rxjs"
import { map } from "rxjs/operators"

@Injectable({
    providedIn: "root",
})
export class AuthService {
    private currentUserSubject: BehaviorSubject<any>
    public currentUser: Observable<any>

    constructor(private http: HttpClient) {
        this.currentUserSubject = new BehaviorSubject<any>(JSON.parse(localStorage.getItem("currentUser") as string))
        this.currentUser = this.currentUserSubject.asObservable()
    }

    public get currentUserValue() {
        return this.currentUserSubject.value
    }

    login(username: string, password: string) {
        return this.http.post<any>(`api/auth/login`, { username, password }).pipe(
            map((user) => {
                localStorage.setItem("currentUser", JSON.stringify(user))
                this.currentUserSubject.next(user)
                return user
            }),
        )
    }

    logout() {
        localStorage.removeItem("currentUser")
        this.currentUserSubject.next(null)
    }
}

