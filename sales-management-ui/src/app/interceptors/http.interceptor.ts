import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler } from '@angular/common/http';
import { environment } from '../../environments/environment'; // Import environment variables

@Injectable()
export class ApiInterceptor implements HttpInterceptor {
    private apiBaseUrl = environment.apiBaseUrl; // Get API base URL from environment

    intercept(req: HttpRequest<any>, next: HttpHandler) {
        // If request URL is already absolute, do not modify it
        if (req.url.startsWith('http')) {
            return next.handle(req);
        }

        // Clone the request and prepend the API base URL
        const modifiedReq = req.clone({
            url: `${this.apiBaseUrl}/${req.url}`
        });

        return next.handle(modifiedReq);
    }
}
