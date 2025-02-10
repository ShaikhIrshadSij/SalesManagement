import { Component } from "@angular/core"
import { Router } from "@angular/router"
import { AuthService } from "./services/auth.service"
import { NotificationService } from "./services/notification.service"

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.scss"],
})
export class AppComponent {
  title = "Car Sales Management"
  notification: string | null = null
  currentUser: any

  constructor(
    private router: Router,
    private authService: AuthService,
    private notificationService: NotificationService,
  ) {
    this.authService.currentUser.subscribe((x) => (this.currentUser = x))
    this.notificationService.notifications$.subscribe((message) => {
      this.notification = message
      setTimeout(() => (this.notification = null), 5000)
    })
  }

  logout() {
    this.authService.logout()
    this.router.navigate(["/login"])
  }

  get isAdmin() {
    return this.currentUser && this.currentUser.role === "Admin"
  }

  get isSalesman() {
    return this.currentUser && this.currentUser.role === "Salesman"
  }
}

