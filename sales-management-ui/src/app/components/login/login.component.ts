import { Component } from "@angular/core"
import { Router } from "@angular/router"
import { AuthService } from '../../services/auth.service'

@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.scss"],
})
export class LoginComponent {
  username = ""
  password = ""
  error = ""

  constructor(
    private authService: AuthService,
    private router: Router,
  ) { }

  onSubmit() {
    this.authService.login(this.username, this.password).subscribe(
      () => {
        this.router.navigate(["/"])
      },
      (error) => {
        this.error = "Invalid username or password"
      },
    )
  }
}

