import { Injectable, Injector } from '@angular/core';
import { HttpInterceptor, HttpHandler, HttpSentEvent, HttpHeaderResponse, HttpProgressEvent, HttpResponse, HttpUserEvent, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

    constructor(private injector: Injector) { }


    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpSentEvent | HttpHeaderResponse | HttpProgressEvent | HttpResponse<any> | HttpUserEvent<any>> {
        const auth = this.injector.get(AuthService)
        const token = auth.getToken()
debugger
        const url = req.url.split('/')
        const urlName = url[url.length - 1]

        let newReq
        if (token) {
            newReq = req.clone({ headers: req.headers.append('Authorization', 'Bearer ' + token) })
        }
        else {
            newReq = req
        }
        return next.handle(newReq)
    }
}