import { Injectable } from "@angular/core"
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router"
import { AuthService } from '../services/auth.service'

@Injectable({ providedIn: "root" })
export class AuthGuard implements CanActivate {
    constructor(
        private router: Router,
        private authService: AuthService,
    ) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        const currentUser = this.authService.currentUserValue
        if (currentUser) {
            const data = route.data as any
            if (data.roles && data.roles.indexOf(currentUser.role) === -1) {
                // role not authorised so redirect to home page
                this.router.navigate(["/"])
                return false
            }

            // authorised so return true
            return true
        }

        // not logged in so redirect to login page with the return url
        this.router.navigate(["/login"], { queryParams: { returnUrl: state.url } })
        return false
    }
}

